using System.Collections.Generic;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Domain.Model;

namespace TSWebApplication.Infrastructure.IRepositories
{
    public interface IUserLoginRepository
    {
        UserDetails GetUserByUserIdPassword(string username, string password);
        int GetUserLocationById(int userID);
        List<NameValue> GetForwordingLocation();
        List<NameValue> GetUserLocationNameById(int userID); 
        List<NameValue> GetVehicleName();
    }
}
