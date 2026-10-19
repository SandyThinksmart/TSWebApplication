using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;
namespace TSWebApplication.Application.IServices
{
    public interface IVehicleMasterService
    {
        public long registerVehicle(VehicleDetails request);
        public VehicleDetails GetVehicleById(long vehicleId);
        public bool DeleteVehicleDetailsById(long vehicleId); 
        public bool ChangeVehicleStatus(long vehicleId);
        public List<VehicleDetails> FetchVehicleDetails(GridSearchFields gridSearchRequest);
        public long UpdateVehicleDetails(VehicleDetails request);

    }
}
