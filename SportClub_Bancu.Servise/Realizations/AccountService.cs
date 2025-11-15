using AutoMapper;
using SportClub_Bancu.Domain.ModelsDb;
using SportClub_Bancu.Servise.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportClub_Bancu.Servise.Realizations
{
    public class AccountService : IAccountService
    {
        private readonly IBaseStorage<UserDb> _userStorage;

        private IMapper _mapper;{ get; set; }
        MapperConfiguration mapperConfiguration = new MapperConfiguration(p =>
        {
            p.AddProfile<AppMappingProfile>();
        });

        public AccountService(IBaseStorage<UserDb> userStorage)
        {
            _userStorage = userStorage;
            _mapper = mapperConfiguration.CreateMapper();
        }
    }
}
