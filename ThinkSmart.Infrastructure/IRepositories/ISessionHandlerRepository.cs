using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Domain.Model;

namespace TSWebApplication.Infrastructure.IRepositories
{
    public interface ISessionHandlerRepository
    {
        LoginStatus getActivityTime(string workstationName, string browserName, string randomvalue);
        void deleteLoginInfo(string uniqueid);
        void updateLastUsedTime(string uniqueid);
    }
}
