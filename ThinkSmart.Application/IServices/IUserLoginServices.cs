using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;


namespace TSWebApplication.Application.IServices
{
    public interface IUserLoginServices
    {
        UserDetails AuthenticateUser(string username, string password);
    }
}
