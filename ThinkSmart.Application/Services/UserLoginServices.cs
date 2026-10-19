using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Application.IServices;
using TSWebApplication.Infrastructure.IRepositories;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;


namespace TSWebApplication.Application.Services
{
    public class UserLoginServices : IUserLoginServices
    {
        private readonly IUserLoginRepository _userLoginRepository;
        public UserLoginServices(IUserLoginRepository userLoginRepository)
        {
            _userLoginRepository = userLoginRepository;
        }
        public UserDetails AuthenticateUser(string username, string password)
        {

            var obj = _userLoginRepository.GetUserByUserIdPassword(username, password);

                      

            return obj;
        }
    }
}
