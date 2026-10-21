using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using TSWebApplication.Application.IServices;
using TSWebApplication.Application.Services;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;


namespace TSWebApplication.API.Controllers
{
    [Route("[Controller]")]
    [ApiController]
    public class WarehouseToSupplierController : ControllerBase
    {
        private readonly LocalizationServices _localization;
       private readonly IAllCommonService _allCommonService;
        private readonly IWarehouseToSupplierService _IWarehouseToSupplierService;

        public WarehouseToSupplierController(IWarehouseToSupplierService WarehouseToSupplierService, LocalizationServices localization, IAllCommonService commonService)
        {
           _allCommonService = commonService;
            _IWarehouseToSupplierService = WarehouseToSupplierService;
            _localization = localization;
        }

        [HttpPost("SearchBykouban")]
        public IActionResult SearchBykouban(string kouban)
        {

            string message = string.Empty;
            string validationMsg = string.Empty;
            if (kouban != null)
            {
                var result = _IWarehouseToSupplierService.GetKoubanDetails(kouban);
                return Ok(new { IsSuccess = true, message = result });

            }
            else
            {
                validationMsg += _localization.GetLocalizedHtmlString("valMsg_KoubanRequired") + Constants.StringValues.NEW_LINE;

            }

            return Ok(new { IsSuccess = true, message = message });
        }

        [HttpPost("GetUnitType")]
        public ActionResult GetUnitType()
        {
            var result = _IWarehouseToSupplierService.GetUnitTypeList();
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpPost("GetFutouList")]
        public IActionResult GetFutouList(string? name = null)
        {
           var result = _allCommonService.GetFutouName(name);
           return Ok(new { IsSuccess = true, message = result });
        }
        [HttpPost("GetMochikomisakiList")]
        public IActionResult GetMochikomisakiList(string? name = null)
        {
            var result = _allCommonService.GetMochikomisakiName(name);
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpPost("saveWarehouseToSupplierDetails")]
        public IActionResult saveWarehouseToSupplierDetails([FromBody] MaterialTensouDetails request)
        {

            var result = _IWarehouseToSupplierService.saveWarehouseToSupplierDetails(request);
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpPost("updateWarehouseToSupplierDetails")]
        public IActionResult updateWarehouseToSupplierDetails([FromBody] MaterialTensouDetails request)
        {

            var result = _IWarehouseToSupplierService.updateWarehouseToSupplierDetails(request);
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpPost("getMaterialDataTransferDetailsByTrasferCD")]
        public ActionResult getMaterialDataTransferDetailsByTrasferCD(long TransferCD)
        {
            var result = _IWarehouseToSupplierService.GetMaterialTransferDetailsByTrasferCD(TransferCD);

            return Ok(new { IsSuccess = true, message = result });

        }
        [HttpGet("GeMatlMakerList")]
        public IActionResult GeMatlMakerList()
        {
            var result = _allCommonService.GetMaterialMakerName();
            return Ok(new { IsSuccess = true, message = result });
        }
        private IActionResult Json(object value)
        {
            return new JsonResult(value);
        }
        [HttpPost("GridSearch")]
        public IActionResult GridSearch([FromBody] GridSearchFields request)
        {
            string message = ValidateSearchFields(request);
            if (string.IsNullOrEmpty(message))
            {
                var result = _IWarehouseToSupplierService.GetDataOnGridView(request);
                return Ok(new { IsSuccess = true, message = result });
            }
            else
            {
                return Ok(new { IsSuccess = false, message = message });
            }
            
        }
        private string ValidateSearchFields(GridSearchFields request)
        {
            string isRequiredString = _localization.GetLocalizedHtmlString("valMsg_COM_IsRequired");
            string validationMsg = string.Empty;
            if (request.PROD_DIM_D_MAX > 0)
            {
                if (request.PROD_DIM_D_MIN > 0)
                {
                    validationMsg += Constants.StringValues.NEW_LINE;
                    validationMsg += _localization.GetLocalizedHtmlString("valMsg_COM_DimDStartRequired");
                }
            }
            if (!string.IsNullOrEmpty(request.START_DATETIME?.Trim()) && string.IsNullOrEmpty(request?.END_DATETIME?.Trim()))
            {

                validationMsg += Constants.StringValues.NEW_LINE;
                validationMsg += _localization.GetLocalizedHtmlString("valMsg_TransferDateStartRequired");

            }

            return validationMsg;
        }


        [HttpGet("InitializeSortPopup")]
        public ActionResult InitializeSortPopup()
        {
            var sortFields = new List<TSSortField> {
                                        new TSSortField {
                                            FieldName = "PP.PROD_NAME_CUST",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_ProdNameCust"), //1                                             
                                            SortOrder=TSSortDirection.Descending
                                        },
                                              new TSSortField {
                                            FieldName = "INTE.INTERNAL_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_InternalName"), //7
                                             IsDefaultSort=false,
                                            SortOrder=TSSortDirection.Descending
                                        },
                                               new TSSortField {
                                            FieldName = "PP.PROD_SHAPE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Shape"), //3
                                             IsDefaultSort=false,
                                            SortOrder=TSSortDirection.Descending
                                        },
                                        new TSSortField {
                                            FieldName = "PP.PROD_DIM_D",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_DimD"),//2
                                             IsDefaultSort=false,
                                            SortOrder=TSSortDirection.Descending
                                        },

                                        new TSSortField {
                                            FieldName = "PP.PROD_LENGTH",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Length"), //4
                                             IsDefaultSort=false,
                                            SortOrder=TSSortDirection.Descending
                                        },
                                          new TSSortField {
                                            FieldName = "DW.DW_START_OPERATOR_CD",
                                            DisplayName =  _localization.GetLocalizedHtmlString("lbl_OperationDay"), //5
                                             IsDefaultSort=false,
                                            SortOrder=TSSortDirection.Descending
                                        },
                                          new TSSortField {
                                                 FieldName = "EMP.SHORT_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Operator"), //6
                                             IsDefaultSort=false,
                                            SortOrder=TSSortDirection.Descending
                                        },
                                         new TSSortField {
                                            FieldName = "PP.CHUUBAN",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Chuuban"),
                                            IsDefaultSort=true,
                                            SortOrder=TSSortDirection.Ascending
                                        },
                                           new TSSortField {
                                            FieldName = "DW.DW_TABA_NO_",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_TabaNo"),
                                            IsDefaultSort=true,
                                            SortOrder=TSSortDirection.Ascending
                                        }
                                    };
            return Ok(new { IsSuccess = true, message = sortFields });
        }

    }
}
