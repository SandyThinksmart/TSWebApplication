using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Diagnostics.Eventing.Reader;
using System.Xml.Linq;
using TSWebApplication.Application.IServices;
using TSWebApplication.Application.Services;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;


namespace TSWebApplication.API.Controllers
{

    [Route("[Controller]")]
    [ApiController]
    public class HardnessController : Controller
    {
      
        private readonly IMIHardnessService _HardnessService;
        private readonly LocalizationServices _localization;
        private readonly IAllCommonService _allCommonService;


        public HardnessController(IMIHardnessService MIHardnessService, LocalizationServices localization, IAllCommonService allCommonService)
        {
            
            _localization = localization;
            _HardnessService = MIHardnessService;
            _allCommonService=allCommonService; 
        }
        [HttpPost("MappChuubanAndGyoubanByBarcode")]
        public IActionResult MappChuubanAndGyoubanByBarcode(string orderNo)
        {
            bool returnval = false;
            var result = _allCommonService.MappChuubanAndGyoubanByBarcode(orderNo);

            if (result.ORDER_NO_IN_CHUUBAN != null)
            {
                returnval = true;
            }

            return Ok(new { IsSuccess = returnval, message = result });
        }
        [HttpPost("ValidateChubban")]
        public ActionResult ValidateChubban([FromBody] CommonFields request)
        {
            var result = _allCommonService.ValidateChubban(request.CHUUBAN, request.GYOUBAN);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("GetHardnessDetailsByChuuban")]
        public IActionResult GetHardnessDetailsByChuuban(string chuuban, short? gyouban,string culture_Name)
        {
            var result = _HardnessService.GetHardnessDetailsByChuuban(chuuban, gyouban, culture_Name);
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpPost("GetSysDateTime")]
        public IActionResult GetSysDateTime()
        {
            DateTime dateTime = _allCommonService.GetSystemDateTime();
            string sysDate = Convert.ToString(dateTime.ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS));
            return Ok(new { IsSuccess = true, message = sysDate });
        }
        [HttpPost("GetNextProcess")]
        public IActionResult GetNextProcess()
        {
            var result = _allCommonService.GetNextProcessList(Convert.ToInt32(UIUtils.GetProcessKubun(_localization.GetLocalizedHtmlString("lbl_ProcessMechanical_Inspection_Hardness"))));
            return Ok(new { IsSuccess = true, message = result });
        }
        //[HttpPost("SetHardnessKubuns")]
        //public IActionResult setHardnessKubuns()
        //{
        //    var result = _HardnessService.SetHardnessKubunsList();
        //    return Ok(new { IsSuccess = true, message = result });
        //}
        
        [HttpPost("GetDecision")]
        public IActionResult GetDecision(string decisionvalue)
        {
            var result = _HardnessService.GetDecisionValue(decisionvalue);
            return Ok(new { IsSuccess = true, message = result });
        }

        
        [HttpPost("GetOperatorList")]
        public IActionResult GetOperatorList(string? name =null, int operatorCD=0)
        {
            var result = _allCommonService.GetOperatorName(name, operatorCD);
            return Ok(new { IsSuccess = true, message = result });
        }
        

            [HttpPost("SaveHardnessDetail")]
        public IActionResult SaveHardnessDetail([FromBody] MIHardnessDetails request)
        {
           _HardnessService.SaveHardnessData(request);
             return Ok(new { IsSuccess = true });
            
        }
        [HttpPost("GetThabaDetailsByChuuban")]
        public IActionResult GetThabaDetailsByChuuban([FromBody] MIHardnessDetails request)
        {
            var result = _HardnessService.GetThabaDetailsByChuuban(request.PP_CHUUBAN, request.PP_GYOUBAN, request.MI_HARDNESS_CD);
            return Ok(new { IsSuccess = true, message = result });
        }
       
        [HttpPost("GetMIHardnessDetailsbyHardnessCD")]
        public IActionResult GetMIHardnessDetailsbyHardnessCD([FromBody] MIHardnessDetails request)
        {
            var result = _HardnessService.GetMIHardnessDetailsbyHardnessCD(request.MI_HARDNESS_CD, request.PP_CHUUBAN, request.PP_GYOUBAN, request.CULTURE_NAME);
            return Ok(new { IsSuccess = true, message = result });


        }
        [HttpPost("GetThabaDetailsByThabaNumber")]
        public IActionResult GetThabaDetailsByThabaNumber([FromBody] MIHardnessDetails request)
        {
            var result = _HardnessService.GetThabaDetailsByThabaNumber(request.PP_CHUUBAN, request.PP_GYOUBAN, request.DETAIL_NO);
            return Ok(new { IsSuccess = true, message = result });


        }

        [HttpPost("HardnessDetailsDelete")]
        public IActionResult HardnessDetailsDelete([FromBody] MIHardnessDetails request)
        {
            var result = _HardnessService.HardnessDetailsDelete(request);
            
            return Json(new { IsSuccess = true, message = result });
        }
        [HttpPost("GridSearch")]
        public IActionResult GridSearch([FromBody] GridSearchFields request)
        {
            
           // string message = ValidateSearchFields(request); handled in JS
            //if (string.IsNullOrEmpty(message))
            //{
                //int Location_CD = _HardnessService.GetUserLocationById(Convert.ToInt32(request.locationCD));
                //request.locationCD = Location_CD;
                var result = _HardnessService.GetMIHardnessdetailsList(request);


              
                return Json(new { IsSuccess = true, message = result });
            //}
            //else
            //{
            //    return Json(new { IsSuccess = false, message = message });
            //}
        }
        private string ValidateSearchFields(GridSearchFields request)
        {
            string isRequiredString = _localization.GetLocalizedHtmlString("valMsg_COM_IsRequired");
            string validationMsg = string.Empty;
            if (request.PROD_DIM_D_MAX > 0 && request.PROD_DIM_D_MIN <=0)
            {
                validationMsg += Constants.StringValues.NEW_LINE;
                validationMsg += _localization.GetLocalizedHtmlString("valMsg_COM_DimDStartRequired");
            }
            if (request.PROD_LENGTH_MAX > 0 && request.PROD_LENGTH_MIN <=0)
            {
                validationMsg += Constants.StringValues.NEW_LINE;
                validationMsg += _localization.GetLocalizedHtmlString("valMsg_COM_LengthStartRequired");
            }
            return validationMsg;
        }
        [HttpPost("HardnessDetailsUpdate")]
        public IActionResult HardnessDetailsUpdate([FromBody] MIHardnessDetails request)
        {
            long msgCd = _HardnessService.UpdateHardnessDetails(request);
            return Ok(new { IsSuccess = true, message = msgCd });
        }

        [HttpPost("CheckThabaExistorNot")]

        public IActionResult CheckThabaExistorNot ([FromBody] MIHardnessDetails request)
        {

            var result = _HardnessService.CheckThabaExistorNot(request.PP_CHUUBAN, request.PP_GYOUBAN, request.MI_HARDNESS_CD);
            return Ok(new { IsSuccess = true, message = result });


        }
        [HttpPost("GetMechHardnessOnFactoryDailyWorkStatus")]
        public IActionResult GetMechHardnessOnFactoryDailyWorkStatus(GridSearchFields gridSearchRequest)
        {
            bool isSuccess = false;
            var result = _HardnessService.GetMechHardnessOnFactoryDailyWorkStatus(gridSearchRequest);
            if (result.Count > 0)
            {
                isSuccess = true;
            }
            return Ok(new { IsSuccess = isSuccess, message = result });
        }
        [HttpPost("GetMechHardnessOnDailyProcessStatus")]
        public IActionResult GetMechHardnessOnDailyProcessStatus(GridSearchFields gridSearchRequest)
        {
            bool isSuccess = false;
            var result = _HardnessService.GetMechHardnessOnDailyProcessStatus(gridSearchRequest);
            if (result.Count > 0)
            {
                isSuccess = true;
            }
            return Ok(new { IsSuccess = isSuccess, message = result });
        }

        [HttpPost("IsValidDateRange")]
        public ActionResult IsValidDateRange(DateTimeDetails request)
        {
            bool returnval = false;

            DateTime minDate = Convert.ToDateTime(DateTime.Now.ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS));
            DateTime maxDate = Convert.ToDateTime(DateTime.Now.ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS));
            minDate = minDate.AddDays(-request.DATERANGE);
            maxDate = maxDate.AddDays(+request.DATERANGE);

            if (Convert.ToDateTime(request.RANGE_DATETIME) < Convert.ToDateTime(minDate) || Convert.ToDateTime(request.RANGE_DATETIME) > Convert.ToDateTime(maxDate))
            {
                returnval = false;
            }
            else
            {
                returnval = true;
            }

            return Ok(new { IsSuccess = returnval });
        }

        //[HttpGet("InitializeSortPopup")]
        //public IActionResult InitializeSortPopup()
        //{
        //    var sortFields = new List<TSSortField> {

        //                                new TSSortField {
        //                                    FieldName = "HD_TEST_DATE",
        //                                    DisplayName = _localization.GetLocalizedHtmlString("lbl_InspectionDate"),
        //                                    IsDefaultSort=false,
        //                                    SortOrder=TSSortDirection.Descending
        //                                },
        //                                // new TSSortField {
        //                                //    FieldName = "dw_machine_name",
        //                                //    DisplayName = _localization.GetLocalizedHtmlString("lbl_Machine_L"),
        //                                //     IsDefaultSort=false,
        //                                //     SortOrder=TSSortDirection.Ascending
        //                                //},

        //                                 new TSSortField {
        //                                    FieldName = "pp_chuuban_gyouban",
        //                                    DisplayName = _localization.GetLocalizedHtmlString("lbl_Chuuban"),
        //                                    IsDefaultSort=false,
        //                                    SortOrder=TSSortDirection.Descending
        //                                },

        //                                    new TSSortField {
        //                                    FieldName =   "DETAIL_NO" ,
        //                                    DisplayName = _localization.GetLocalizedHtmlString("lbl_SplitNo"),
        //                                    IsDefaultSort=false,
        //                                    SortOrder=TSSortDirection.Ascending
        //                                },
        //                                      new TSSortField {
        //                                    FieldName = "pp_customer_name",
        //                                    DisplayName = _localization.GetLocalizedHtmlString("lbl_Customer"),
        //                                     IsDefaultSort=false,
        //                                    SortOrder=TSSortDirection.Ascending
        //                                },
        //                                new TSSortField {
        //                                    FieldName = "pp_prod_name_cust",
        //                                    DisplayName = _localization.GetLocalizedHtmlString("lbl_ProdNameCust"),
        //                                    IsDefaultSort=false,
        //                                    SortOrder=TSSortDirection.Ascending
        //                                },

        //                                   new TSSortField {
        //                                    FieldName = "pp_prod_shape",
        //                                    DisplayName = _localization.GetLocalizedHtmlString("lbl_ProdShape"),
        //                                    IsDefaultSort=false,
        //                                    SortOrder=TSSortDirection.Ascending
        //                                },
        //                                  new TSSortField {
        //                                    FieldName = "PP_PROD_DIM_D_STRING",
        //                                    DisplayName = _localization.GetLocalizedHtmlString("lbl_ProductDim"),
        //                                     IsDefaultSort=false,
        //                                    SortOrder=TSSortDirection.Ascending
        //                                },
        //                                    new TSSortField {
        //                                    FieldName = "pp_prod_length",
        //                                    DisplayName = _localization.GetLocalizedHtmlString("lbl_ProductLength"),
        //                                     IsDefaultSort=false,
        //                                    SortOrder=TSSortDirection.Ascending
        //                                },





        //                            };
        //    return Json(new { IsSuccess = true, message = sortFields });
        //}

        [HttpPost("GetRowCountForMechHardnessFactory")]
        public ActionResult GetRowCountForMechHardnessFactory(GridSearchFields gridSearchRequest)
        {
            int count = _HardnessService.GetRowCountForMechHardnessFactory(gridSearchRequest);
            return Ok(new { IsSuccess = true, message = count });
        }

        [HttpPost("GetRowCountForMechHardnessDailyAnalysis")]
        public ActionResult GetRowCountForMechHardnessDailyAnalysis(GridSearchFields gridSearchRequest)
        {
            int count = _HardnessService.GetRowCountForMechHardnessDailyAnalysis(gridSearchRequest);
            return Ok(new { IsSuccess = true, message = count });
        }
        [HttpPost("GetFurnanceNo")]
        public IActionResult GetFurnanceNo([FromBody] MIHardnessDetails request)
        {
            bool returnVal = false;

            MIHardnessDetails result = new MIHardnessDetails();
            result = _HardnessService.GetFurnanceNo(request.PP_CHUUBAN, request.PP_GYOUBAN,request.DETAIL_NO);

            if (result != null)

            {
                returnVal = true;
            }

            return Ok(new { IsSuccess = returnVal, message = result });
        }

        [HttpGet("GetOperatorNamebyCD")]
        public IActionResult GetOperatorNamebyCD(long OperatorCD)
        {
            var result = _allCommonService.GetEmployeeNameByID(OperatorCD);
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpPost("GetFICombinedDecisionDetails")]
        public ActionResult GetFICombinedDecisionDetails([FromBody] CommonFields request)
        {
            var result = _HardnessService.GetFICombinedDecisionDetails(request.CHUUBAN, request.GYOUBAN);
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpPost("UpdateFICombinedDecision")]
        public IActionResult UpdateFICombinedDecision(short hdDecision , long processPlanCd)
        {
            _HardnessService.UpdateFICombinedDecision(hdDecision, processPlanCd);
            return Ok(new { IsSuccess = true });

        }
        [HttpPost("ValidateHardnessDetailsOnUpdateBtn")]
        public IActionResult ValidateHardnessDetailsOnUpdateBtn(long MIHardnessCD)
        {
            var result = _HardnessService.ValidateHardnessDetailsOnUpdateBtn(MIHardnessCD);
            return Ok(new { IsSuccess = true, message = result });

        }

    }
}
