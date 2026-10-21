using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Domain.Model;

namespace TSWebApplication.Application.IServices
{
    public interface ISessionHandlerServices
    {
        LoginStatus checkUserLastActivity (string workstationName, string browserName, string randomvalue);
        void deleteLoginData(string uniqueid);
        void updateLastUsedTime(string uniqueid);
    }
}
