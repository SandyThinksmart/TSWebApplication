using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;
using TSWebApplication.Application.IServices;
using TSWebApplication.Application.Services;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;

namespace TSWebApplication.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HSKMillSheetController : ControllerBase
    {
        private readonly IHSKMillSheetServices _hskmillsheetService;
        private readonly IAllCommonService _commonService;
        private readonly LocalizationServices _localization;
        //private readonly IViewRenderService _viewRenderService;
        public HSKMillSheetController(IHSKMillSheetServices hskmillsheetServices, IAllCommonService commonService, LocalizationServices localization)
        {
            _localization = localization;
            _commonService = commonService;
            _hskmillsheetService = hskmillsheetServices;
        }
        [HttpPost("GetSysDateTime")]
        public ActionResult GetSysDateTime()
        {
            //string dateTime = DateTime.Now.ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS);
            //return Ok(new { IsSuccess = true, message = dateTime });

            DateTime dateTime = _commonService.GetSystemDateTime();
            string sysDate = Convert.ToString(dateTime.ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS));
            return Ok(new { IsSuccess = true, message = sysDate });
        }
        [HttpPost("ValidateChubban")]
        public ActionResult ValidateChubban([FromBody] CommonFields request)
        {
            var result = _commonService.ValidateChubban(request.CHUUBAN, request.GYOUBAN);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("GetHSKMillSheetDetailsByChuuban")]
        public IActionResult GetHSKMillSheetDetailsByChuuban([FromBody] HSKMillSheetDetails request)
        {
            var result = _hskmillsheetService.GetHSKMillSheetDetailsByChuuban(request.PP_CHUUBAN, request.PP_GYOUBAN, request.CULTURE_NAME);
            return Ok(new { issucess = true, message = result });
        }

        [HttpPost("GetHSKMillSheetProcessPlanCD")]
        public IActionResult GetHSKMillSheetProcessPlanCD(string ProcessPlanCD)
        {
            var result = _hskmillsheetService.GetHSKMillSheetProcessPlanCD(ProcessPlanCD);
            return Ok(new { issucess = true, message = result });
        }

        [HttpPost("GetProductCD")]
        public IActionResult GetMakerMillSheetDetail(string ProdSpecNo1, string ProdSpecNo2, string ProdVer)
        {
            var result = _hskmillsheetService.GetProductCD(ProdSpecNo1, ProdSpecNo2, ProdVer);
            return Ok(new { IsSuccess = true, message = result });
        }


        [HttpPost("GetProductDetails")]
        public IActionResult GetProductDetails(long Prod_CD)
        {
            var result = _hskmillsheetService.GetProductDetails(Prod_CD);
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpPost("GetProductMasterDefaultValue")]
        public IActionResult GetProductMasterDefaultValue(long Prod_CD)
        {
            var result = _hskmillsheetService.GetProductMasterDefaultValue(Prod_CD);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("GetCondDescFromProdCD")]
        public IActionResult GetCondDescFromProdCD(long Prod_CD)
        {
            var result = _hskmillsheetService.GetCondDescFromProdCD(Prod_CD);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("MakerMillSheetChemicalCompositionPopup")]
        public IActionResult MakerMillSheetChemicalCompositionPopup([FromBody] MakerMillSheetDetails request)
        {
            var result = _hskmillsheetService.MakerMillSheetChemicalCompositionPopup(request.FILENAME);
            return Ok(new { issucess = true, message = result });
        }

        [HttpPost("GetProductQuntity")]
        public ActionResult GetProductQuntity()
        {
            var result = _commonService.GetProductQuntityList();
            return Ok(new { IsSuccess = true, message = result });
        }

        //Maker List
        [HttpPost("GetCustomerList")]
        public IActionResult GetCustomerList(string? yobina = "")
        {
            var lst = _commonService.GetCustomerName(yobina);
            return Ok(new { IsSuccess = true, message = lst });
        }


        //Maker List
        [HttpPost("MakerList")]
        public IActionResult MakerList(string? yobina = "")
        {
            var lst = _commonService.GetMakerName(yobina);
            return Ok(new { IsSuccess = true, message = lst });
        }




        [HttpPost("GridSearch")]
        public IActionResult GridSearch([FromBody] GridSearchFields request)
        {
            var result = _hskmillsheetService.GetHSKMillSheetList(request);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("GridSearchHistoryPopup")]
        public IActionResult GridSearchHistoryPopup([FromBody] GridSearchFields request)
        {
            var result = _hskmillsheetService.GetHSKMillSheetHistoryPopUp(request);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("GetProcessPlanCD")]
        public ActionResult GetProcessPlanCD([FromBody] CommonFields request)
        {
            var result = _hskmillsheetService.GetProcessPlanCD(request.CHUUBAN, request.GYOUBAN);
            return Ok(new { IsSuccess = true, message = result });
        }
        
        [HttpPost("GetHSKMillSheetPostHTInspectionDetails")]
        public ActionResult GetHSKMillSheetPostHTInspectionDetails(string processPlanCD)
        {
            var result = _hskmillsheetService.GetHSKMillSheetPostHTInspectionDetails(processPlanCD);
            return Ok(new { IsSuccess = true, message = result });
        }

       [HttpPost("GetPostHTInspectionDetailsByPostHTCD")]
        public ActionResult GetPostHTInspectionDetailsByPostHTCD(string postHTCD)
        {
            var result = _hskmillsheetService.GetPostHTInspectionDetailsByPostHTCD(postHTCD);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("GetHSKMillSheetFinalInspectionDetails")]
        public ActionResult GetHSKMillSheetFinalInspectionDetails(string processPlanCD)
        {
            var result = _hskmillsheetService.GetHSKMillSheetFinalInspectionDetails(processPlanCD);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("GetFinalInspectionDetailsByPostFinalInspCD")]
        public ActionResult GetFinalInspectionDetailsByPostFinalInspCD(string finalInspCD)
        {
            var result = _hskmillsheetService.GetFinalInspectionDetailsByPostFinalInspCD(finalInspCD);
            return Ok(new { IsSuccess = true, message = result });
        }


        [HttpPost("GetHSKChemicalCompositionDetails")]
        public ActionResult GetHSKChemicalCompositionDetails([FromBody] CommonFields request)
        {
            var result = _hskmillsheetService.GetHSKChemicalCompositionDetails(request.CUSTOMER_CD, request.PRODUCT_NAME_CUST);
            return Ok(new { IsSuccess = true, message = result });
        }


        [HttpPost("GetHSKChemicalCompositionAandMakerDetails")]
        public ActionResult GetHSKChemicalCompositionAandMakerDetails([FromBody] CommonFields request)
        {
            var result = _hskmillsheetService.GetHSKChemicalCompositionAandMakerDetails(request.CUSTOMER_CD, request.PRODUCT_NAME_CUST, request.MAKER_MILL_SHEET_CD);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("GetProdOrderNoInspectionHardness")]
        public ActionResult GetProdOrderNoInspectionHardness([FromBody] CommonFields request)
        {
            var result = _hskmillsheetService.GetProdOrderNoInspectionHardness(request.CHUUBAN, request.GYOUBAN, request.CHARGE,request.MATDIMD,request.MATNAMEMAKER);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("GetProdOrderNoInspectionTensile")]
        public ActionResult GetProdOrderNoInspectionTensile([FromBody] CommonFields request)
        {
            var result = _hskmillsheetService.GetProdOrderNoInspectionTensile(request.CHUUBAN, request.GYOUBAN, request.CHARGE, request.MATDIMD, request.MATNAMEMAKER);
            return Ok(new { IsSuccess = true, message = result });
        }


        [HttpGet("LoadHardnessPattern1DetailNoData")]
        public IActionResult LoadHardnessPattern1DetailNoData(long prodOrderNo, int DetailNo)
        {
            var result = _hskmillsheetService.LoadHardnessPattern1DetailNoData(prodOrderNo, DetailNo);

            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("HSKMillSheetGridSearchChuubanLiveCheck")]
        public IActionResult HSKMillSheetGridSearchChuubanLiveCheck(string chuuban, string gyouban)
        {
            var result = _hskmillsheetService.HSKMillSheetGridSearchChuubanLiveCheck(chuuban, gyouban);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("GetMakerMillSheetDetails")]
        public IActionResult GetMakerMillSheetDetails([FromBody] GridSearchFields request)
        {
            var result = _hskmillsheetService.GetMakerMillsheetDetails(request);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("SaveHSKMillSheetDetails")]
        public IActionResult SaveHSKMillSheetDetails([FromBody] HSKMillSheetDetails request)
        {
            _hskmillsheetService.SaveHSKMillSheetDetails(request);
            return Ok(new { IsSuccess = true });

        }
    }
}
