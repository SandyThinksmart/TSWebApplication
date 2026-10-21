using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;


namespace TSWebApplication.Infrastructure.IRepositories
{
    public interface IVehicleMasterRepository
    {
        public long registerVehicle(VehicleDetails request);
        public VehicleDetails GetVehicleById(long id);
        public bool DeleteVehicleDetailsById(long vehicleId);
        public bool ChangeVehicleStatus(long vehicleId);
        public List<VehicleDetails> FetchVehicleDetails(List<SearchField> searchCriteria, string sortCiteria);
        public long UpdateVehicleDetails(VehicleDetails request);
    }
}
