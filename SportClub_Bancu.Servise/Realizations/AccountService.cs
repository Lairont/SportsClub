using AutoMapper;
using SportClub_Bancu.Domain.ModelsDb;
using SportClub_Bancu.Servise.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using SporClub_Bancu.DAL;
using SportClub_Bancu.Domain.Response;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Threading.Tasks;

namespace SportClub_Bancu.Servise.Realizations
{
    public class AccountService : IAccountService
    {
        private readonly IBaseStorage<UserDb> _userStorage;

        private IMapper _mapper { get; set; }

         MapperConfiguration mapperConfiguration = new MapperConfiguration(p =>
        {
           p.AddProfile<AppMappingProfile>();
        });

        public AccountService(IBaseStorage<UserDb> userStorage)
        {
            _userStorage = userStorage;
            _mapper = mapperConfiguration.CreateMapper();
            //var config = new MapperConfiguration(cfg =>
            //{
            //    cfg.AddProfile<AppMappingProfile>();
            //});
            //_mapper = config.CreateMapper();
        }

        public async Task<BaseResponse<User>> Login(User model) 
        {
            try
            {
                var userdb = _mapper.Map<UserDb>(model);
                var existingUser = await _userStorage.GetAll().FirstOrDefaultAsync(x => x.Email == userdb.Email);
                if (existingUser == null)
                {
                    return new BaseResponse<User>
                    {
                        Description = "Пользователь не найден"
                    };
                }
                if (existingUser.Password != userdb.Password) 
                {
                    return new BaseResponse<User>
                    {
                        Description = "Неверный пароль или почта"
                    };
                }
                return new BaseResponse<User>
                {
                    Data = _mapper.Map<User>(existingUser), 
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<User>
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalError
                };
            }
        }

        public async Task<BaseResponse<User>> Register(User model)
        {
            try
            {
                model.PathImage = "images/user.png";
                model.CreatedAt = DateTime.Now;
                var userdb = _mapper.Map<UserDb>(model); 

                if (await _userStorage.GetAll().FirstOrDefaultAsync(x => x.Email == userdb.Email) != null) 
                {
                    return new BaseResponse<User>
                    {
                        Description = "Пользователь с такой почтой уже есть", 
                    };
                }
                await _userStorage.Add(userdb); 
                return new BaseResponse<User>
                {
                    Data = model,
                    Description = "Пользователь зарегистрирован",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<User> 
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalError // Исправлено: InternalServerError
                };
            }
        }
    }
}