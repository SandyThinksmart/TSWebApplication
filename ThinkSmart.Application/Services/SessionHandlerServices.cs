using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Application.IServices;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Infrastructure.IRepositories;

namespace TSWebApplication.Application.Services
{
    public class SessionHandlerServices : ISessionHandlerServices
    {
        private readonly ISessionHandlerRepository _sessionHandlerRepository;
        public SessionHandlerServices(ISessionHandlerRepository sessionHandlerRepository)
        {
            _sessionHandlerRepository = sessionHandlerRepository;
           
        }
        public LoginStatus checkUserLastActivity(string workstationName, string browserName, string randomvalue)
        {
            return _sessionHandlerRepository.getActivityTime(workstationName, browserName, randomvalue);

        }
        public void deleteLoginData(string uniqueid)
        {
            _sessionHandlerRepository.deleteLoginInfo(uniqueid);

        }
        public void updateLastUsedTime(string uniqueid)
        {
            _sessionHandlerRepository.updateLastUsedTime(uniqueid);

        }
    }
}
