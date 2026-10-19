using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using Org.BouncyCastle.Asn1.Ocsp;
using TSWebApplication.Application.IServices;
using TSWebApplication.Application.Services;
using TSWebApplication.Domain.Model;

using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;

using static TSWebApplication.Domain.Utility.UIConstants;
namespace TSWebApplication.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class VehicleMasterController : ControllerBase
    {
        private readonly IVehicleMasterService _vehicleMasterService;
        private readonly IAllCommonService _allCommonService;

        private readonly LocalizationServices _localization;
        public VehicleMasterController(IVehicleMasterService vehicleMasterService, LocalizationServices localization, IAllCommonService allCommonService)
        {
            _vehicleMasterService = vehicleMasterService;
            _localization = localization;
            _allCommonService = allCommonService;

        }
        [HttpPost("registerVehicle")]
        public ActionResult registerVehicle([FromBody] VehicleDetails request)
        {
            var vehicleCD = _vehicleMasterService.registerVehicle(request);
            return Ok(new { success = true, vehicleCD });
        }
        
        [HttpPost("UpdateVehicleDetails")]
        public ActionResult UpdateVehicleDetails([FromBody] VehicleDetails request)
        {
            var vehicleCD = _vehicleMasterService.UpdateVehicleDetails(request);
            return Ok(new { success = true, vehicleCD });
        }

        [HttpGet("GetVehicleById")]
        public IActionResult GetVehicleById(long vehicleId)
        {
            var vehicle = _vehicleMasterService.GetVehicleById(vehicleId);
            return Ok(new { success = true, message = vehicle });
        }

        [HttpGet("DeleteVehicleDetailsById")]
        public IActionResult DeleteVehicleDetailsById(long vehicleId)
        {
            try
            {
                bool result = _vehicleMasterService.DeleteVehicleDetailsById(vehicleId);

                return Ok(new { success = result });
            }
            catch (OracleException ex)
            {
                return BadRequest(new
                {
                    errorNumber = ex.Number,
                    message = ex.Message
                });
            }
        }
        
        [HttpPost("ChangeVehicleStatus")]
        public IActionResult ChangeVehicleStatus(long vehicleId)
        {
            bool result = _vehicleMasterService.ChangeVehicleStatus(vehicleId);
            return Ok(new { success = result });
        }

        //[HttpGet("FetchVehicleDetails")]
        //public IActionResult FetchVehicleDetails()
        //{
           
        //    var result = _vehicleMasterService.FetchVehicleDetails();
        //    return Ok(new { success = true, message = result });

        //}

        [HttpPost("FetchVehicleDetails")]
        public IActionResult FetchVehicleDetails([FromBody] GridSearchFields request)
        {            
            var result = _vehicleMasterService.FetchVehicleDetails(request);
            return Ok(new { success = true, message = result });

        }

        //[HttpGet("IsVehicleUsed")]
        //public IActionResult IsVehicleUsed(long vehicleId)
        //{
            
        //    bool isUsed = _vehicleMasterService.DeleteVehicleDetailsById(vehicleId);
        //    return Ok(new { success = isUsed });

           
        //}

        //[HttpPost("ECDetailsUpdate")]
        //public ActionResult ECDetailsUpdate([FromBody] ECDetails request)
        //{
        //    long msgCD = _eCService.UpdateECDetails(request);
        //    return Ok(new { IsSuccess = true, message = msgCD });
        //}


        //[HttpPost("ECDetailsDelete")]
        //public ActionResult ECDetailsDelete([FromBody] ECDetails request)
        //{
        //    _eCService.Delete(request);
        //    return Ok(new { IsSuccess = true, message = "" });
        //}

        //[HttpGet("GetFinalInspByECCD")]
        //public ActionResult GetFinalInspByECCD(long ecCD)
        //{
        //    string finalInsp = _eCService.GetFinalInspByECCD(ecCD);
        //    return Ok(new { IsSuccess = true, message = finalInsp });
        //}
    }
}
