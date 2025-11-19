using AutoMapper;
using SportClub_Bancu.Domain.ModelsDb;
using SportClub_Bancu.Servise.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using SporClub_Bancu.DAL;
using SportClub_Bancu.Domain.Response;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using SportClub_Bancu.Domain.Validators;
using System.Security.Claims;
using SportClub_Bancu.Domain.Helpers;
using System.Linq.Expressions;

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
                return new BaseResponse<ClaimsIdentity>
                {
                    Data = _mapper.Map<ClaimsIdentity>(existingUser),
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

        public async Task<BaseResponse<ClaimsIdentity>> Register(User model)
        {
            try
            {
                model.PathImage = "images/user.png";
                model.CreatedAt = DateTime.Now;
                model.Password = HashPasswordHelper.HashPassword(model.Password);

                await _validationRules.ValidateAndThrowAsync(model);

                var userdb = _mapper.Map<UserDb>(model);

                if (await _userStorage.GetAll().FirstOrDefaultAsync(x => x.Email == userdb.Email) != null)
                {
                    return new BaseResponse<ClaimsIdentity>
                    {
                        Description = "Пользователь с такой почтой уже есть",
                    };
                }
                await _userStorage.Add(userdb);

                //.
                var result = AuthenticateUserHelper.Authenticate(model);

                return new BaseResponse<ClaimsIdentity>
                {

                    //была ошибка
                    Data = result,
                    Description = "Пользователь зарегистрирован",
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


    }
}