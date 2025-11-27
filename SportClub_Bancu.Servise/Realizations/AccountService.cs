using AutoMapper;
using FluentValidation;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using SporClub_Bancu.DAL;
using SportClub_Bancu.Domain.Helpers;
using SportClub_Bancu.Domain.ModelsDb;
using SportClub_Bancu.Domain.Response;
using SportClub_Bancu.Domain.Validators;
using SportClub_Bancu.Servise.Interfaces;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SportClub_Bancu.Servise.Realizations
{
    public class AccountService : IAccountService
    {
        private readonly IBaseStorage<UserDb> _userStorage;

        private IMapper _mapper { get; set; }

        private UserValidator _validationRules { get; set; }

        MapperConfiguration mapperConfiguration = new MapperConfiguration(p =>
        {
            p.AddProfile<AppMappingProfile>();
        });

        public AccountService(IBaseStorage<UserDb> userStorage)
        {
            _userStorage = userStorage;
            _mapper = mapperConfiguration.CreateMapper();
            _validationRules = new UserValidator();
        }




        public async Task<BaseResponse<ClaimsIdentity>> Login(User model)
        {
            try
            {
                await _validationRules.ValidateAndThrowAsync(model);
                var userdb = _mapper.Map<UserDb>(model);
                var existingUser = await _userStorage.GetAll().FirstOrDefaultAsync(x => x.Email == userdb.Email);
                if (existingUser == null)
                {
                    return new BaseResponse<ClaimsIdentity>
                    {
                        Description = "Пользователь не найден"
                    };
                }
                if (existingUser.Password != HashPasswordHelper.HashPassword(model.Password))
                {
                    return new BaseResponse<ClaimsIdentity>
                    {
                        Description = "Неверный пароль или почта"
                    };
                }

                var claims = new List<Claim>
        {
            new Claim(ClaimsIdentity.DefaultNameClaimType, existingUser.Login ?? string.Empty),
            new Claim(ClaimTypes.Email, existingUser.Email ?? string.Empty),
            new Claim(ClaimTypes.Role, existingUser.Role.ToString())
        };
                var identity = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

                return new BaseResponse<ClaimsIdentity>
                {
                    Data = identity,
                    StatusCode = StatusCode.OK
                };
            }
            catch (ValidationException ex)
            {
                var errorMessages = string.Join("; ", ex.Errors.Select(e => e.ErrorMessage));
                return new BaseResponse<ClaimsIdentity>
                {
                    Description = errorMessages,
                    StatusCode = StatusCode.BadRequest
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<ClaimsIdentity>
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalError
                };
            }
        }

        //public async Task<BaseResponse<ClaimsIdentity>> Login(User model)
        //{
        //    try
        //    {
        //        await _validationRules.ValidateAndThrowAsync(model);
        //        var userdb = _mapper.Map<UserDb>(model);
        //        var existingUser = await _userStorage.GetAll().FirstOrDefaultAsync(x => x.Email == userdb.Email);
        //        if (existingUser == null)
        //        {
        //            return new BaseResponse<ClaimsIdentity>
        //            {
        //                Description = "Пользователь не найден"
        //            };
        //        }
        //        if (existingUser.Password != HashPasswordHelper.HashPassword(model.Password))
        //        {
        //            return new BaseResponse<ClaimsIdentity>
        //            {
        //                Description = "Неверный пароль или почта"
        //            };
        //        }
        //        return new BaseResponse<ClaimsIdentity>
        //        {
        //            Data = _mapper.Map<ClaimsIdentity>(existingUser),
        //            StatusCode = StatusCode.OK
        //        };
        //    }
        //    catch (ValidationException ex)
        //    {
        //        var errorMessages = string.Join("; ", ex.Errors.Select(e => e.ErrorMessage));
        //        return new BaseResponse<ClaimsIdentity>
        //        {
        //            Description = errorMessages,
        //            StatusCode = StatusCode.BadRequest
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new BaseResponse<ClaimsIdentity>
        //        {
        //            Description = ex.Message,
        //            StatusCode = StatusCode.InternalError
        //        };
        //    }
        //}

        public async Task<BaseResponse<string>> Register(User model)
        {
            try
            {
                Random random = new Random();
                string confirmationCode = $"{random.Next(10)}{random.Next(10)}{random.Next(10)}{random.Next(10)}";

                if (await _userStorage.GetAll().FirstOrDefaultAsync(x => x.Email == model.Email) != null)
                {
                    return new BaseResponse<string>()
                    {
                        Description = "Пользователь с такой почтой уже есть",
                    };
                }
                await SendEmail(model.Email, confirmationCode);

                return new BaseResponse<string>()
                {

                    //была ошибка
                    Data = confirmationCode,
                    Description = "Письмо отправлено",
                    StatusCode = StatusCode.OK
                };
            }
            catch (ValidationException ex)
            {
                var errorMessages = string.Join("; ", ex.Errors.Select(e => e.ErrorMessage));
                return new BaseResponse<string>()
                {
                    Description = errorMessages,
                    StatusCode = StatusCode.BadRequest
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<string>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalError
                };
            }
        }


        public async Task SendEmail(string email, string confirmationCode)
        {
            try
            {
                string path = "F:\\Visual Studio\\job\\practica\\SportsClub\\SportsClub_Bancu\\password.txt";
                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress("Администрация сайта", "FitStock@bk.ru"));
                emailMessage.To.Add(new MailboxAddress("", email));
                emailMessage.Subject = "Добро пожаловать!";


                var htmlBody = $@"
                   <html>
                   <head>
                  <style>
                          body {{ font-family: Arial, sans-serif; background-color: #f2f2f2; }}
                         .container {{ max-width: 600px; margin: 0 auto; padding: 20px; background-color: #fff; border-radius: 10px; box-shadow: 0px 0px 10px rgba(0,0,0,0.1); }}
                      .header {{ text-align: center; margin-bottom: 20px; }}
                    .message {{ font-size: 16px; line-height: 1.6; }}
                      .code {{ background-color: #f0f0f0; padding: 5px; border-radius: 5px; font-weight: bold; }}
                     .container .code {{ text-align: center; }}
                  </style>
                   </head>
                    <body>
                     <div class='container'>
                     <div class='header'><h1>Добро пожаловать на сайт FitStock!</h1></div>
                   <div class='message'>
                  <p>Пожалуйста, введите данный код на сайте, чтобы подтвердить ваш email и завершить регистрацию:</p>
                  <p class='code'>{confirmationCode}</p>
                          </div>
                   </div>
                  </body>
                  </html>";

                emailMessage.Body = new TextPart("html") { Text = htmlBody };

                if (!File.Exists(path))
                {
                    throw new FileNotFoundException($"Файл с паролем не найден: {path}");
                }

                string password;
                using (var reader = new StreamReader(path))
                {
                    password = await reader.ReadToEndAsync().ConfigureAwait(false);
                }


                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync("smtp.gmail.com", 465, true).ConfigureAwait(false);
                    await client.AuthenticateAsync("petrbancu@gmail.com", password).ConfigureAwait(false);
                    await client.SendAsync(emailMessage).ConfigureAwait(false);
                    await client.DisconnectAsync(true).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {

                throw new InvalidOperationException("Не удалось отправить письмо.", ex);
            }
        }


        public async Task<BaseResponse<ClaimsIdentity>> ConfirmEmail(User model, string code, string confirmCode)
        {
            try
            {

                if (code != confirmCode)
                {
                    return new BaseResponse<ClaimsIdentity>
                    {
                        Description = "Неверный код подтверждения. Регистрация не выполнена.",
                        StatusCode = StatusCode.BadRequest
                    };
                }


                model.PathImage = "/images/user.png";
                model.CreatedAt = DateTime.Now;
                model.Password = HashPasswordHelper.HashPassword(model.Password);


                await _validationRules.ValidateAndThrowAsync(model);


                var userDb = _mapper.Map<UserDb>(model);


                await _userStorage.Add(userDb);


                var authResult = AuthenticateUserHelper.Authenticate(model);


                return new BaseResponse<ClaimsIdentity>
                {
                    Data = authResult,
                    Description = "Регистрация успешно завершена. Пользователь добавлен.",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<ClaimsIdentity>
                {
                    Description = $"Внутренняя ошибка сервера: {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }


        }


        // Метод для проверки и создания аккаунта через Google
        public async Task<BaseResponse<ClaimsIdentity>> IsCreatedAccount(User model)
        {
            try
            {
                var userDb = new UserDb();
                if (await _userStorage.GetAll().FirstOrDefaultAsync(x => x.Email == model.Email) == null)
                {
                    model.Password = "google";
                    model.CreatedAt = DateTime.Now;

                    userDb = _mapper.Map<UserDb>(model);
                    await _userStorage.Add(userDb);

                    var resultRegister = AuthenticateUserHelper.Authenticate(model);
                    return new BaseResponse<ClaimsIdentity>()
                    {
                        Data = resultRegister,
                        Description = "Объект добавился",
                        StatusCode = StatusCode.OK
                    };
                }

                var resultLogin = AuthenticateUserHelper.Authenticate(model);
                return new BaseResponse<ClaimsIdentity>()
                {
                    Data = resultLogin,
                    Description = "Объект уже был создан",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<ClaimsIdentity>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

    }
}