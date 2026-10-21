using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Application.IServices;
using TSWebApplication.Application.Services;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;

namespace TSWebApplication.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProcessPlanHandoverController : ControllerBase
    {
        private readonly IProcessPlanHandoverService _processPlanHandoverService;
        private readonly IAllCommonService _commonService;
        private readonly LocalizationServices _localization;
        List<string> ValidChuubanList = new List<string>();
       // List<string> data = new List<string>();
        public ProcessPlanHandoverController(IProcessPlanHandoverService processPlanHandoverService, IAllCommonService commonService, LocalizationServices localization) {
            _processPlanHandoverService = processPlanHandoverService;
            _localization = localization;
            _commonService = commonService;
        }
       
        [HttpPost("GetDetailsByChuuban")]
        public IActionResult GetDetailsByChuuban(List<GridSearchFields> processPlanDetails)
        {
            string message = string.Empty;
            bool isSuccess = false;

            List<GridSearchFields> data = new List<GridSearchFields>();
            data = _processPlanHandoverService.GetDetailsByChuuban(processPlanDetails);
            isSuccess = true;
            return Ok(new { IsSuccess = isSuccess, data = data });
        }

        [HttpPost("SaveProcessPlanHandoverData")]
        public IActionResult SaveProcessPlanHandoverData([FromBody] List<GridSearchFields> processPlanDetails)
        {
        string result =  _processPlanHandoverService.SaveProcessPlanHandoverData(processPlanDetails);
           return Ok(new { IsSuccess = true, message = result });
        }
        [HttpPost("DeleteProcessPlanHandoverData")]
        public IActionResult DeleteProcessPlanHandoverData(List<GridSearchFields> processPlanDetails)

        {
            List<GridSearchFields> data = new List<GridSearchFields>();
            data =  _processPlanHandoverService.DeleteProcessPlanHandoverData(processPlanDetails);
            return Ok(new { IsSuccess = true, data = data });
        }
        private string CheckValidChubaan(string lstChubaan)
        {


            bool isValid = true;

            string enteredChuubans = string.Empty;
            string invalidChuubans = string.Empty;
            string notExistChuubans = string.Empty;

            List<string> ValidChuubans = new List<string>();
            string validChuubans = string.Empty;
            string completedChuubans = string.Empty;

            //foreach (string _chuuban in lstChubaan)
            //{

            if (enteredChuubans == string.Empty)
            {
                enteredChuubans = "'" + lstChubaan + "'";
            }
            else
            {
                enteredChuubans = enteredChuubans + "," + "'" + lstChubaan + "'";
            }
            //}

            if (enteredChuubans != string.Empty)
            {
                List<string> invalidList = new List<string>();
                List<string> notExistChuubanList = new List<string>();

                // Get chubaan details.
                var result = _commonService.GetChuubanListForInvaidCheck(enteredChuubans);

                if (result.Count == 0)
                {
                    //foreach (string _chuuban in lstChubaan)
                    //{
                    invalidList.Add(lstChubaan);
                    //}

                    foreach (string invalidKouban in invalidList)
                    {
                        if (invalidChuubans == string.Empty)
                        {
                            invalidChuubans = "" + invalidKouban + "";
                        }
                        else
                        {
                            invalidChuubans = invalidChuubans + "," + "" + invalidKouban + "";
                        }
                    }
                }
                else
                {
                    //foreach (string _enteredChuuban in lstChubaan)
                    //{

                    if (result.Any(lstChubaan.ToLower().Contains))
                    {
                        ValidChuubans.Add(lstChubaan);
                    }
                    else
                    {
                        if (lstChubaan.Length == 11)
                        {
                            notExistChuubanList.Add(lstChubaan);
                        }
                        else
                        {
                            invalidList.Add(lstChubaan);
                        }
                    }
                    //}

                    invalidList = invalidList.Distinct().ToList();
                    notExistChuubanList = notExistChuubanList.Distinct().ToList();

                    foreach (string invalidKouban in invalidList)
                    {
                        if (invalidChuubans == string.Empty)
                        {
                            invalidChuubans = "" + invalidKouban + "";
                        }
                        else
                        {
                            invalidChuubans = invalidChuubans + "," + "" + invalidKouban + "";
                        }
                    }

                    foreach (string notExistChuuban in notExistChuubanList)
                    {
                        if (notExistChuubans == string.Empty)
                        {
                            notExistChuubans = "" + notExistChuuban + "";
                        }
                        else
                        {
                            notExistChuubans = notExistChuubans + "," + "" + notExistChuuban + ""; ;
                        }
                    }

                    // Check chuuban is completed.

                    foreach (string chubaan in ValidChuubans)
                    {
                        if (validChuubans == string.Empty)
                        {
                            validChuubans = "" + chubaan + "";
                        }
                        else
                        {
                            validChuubans = notExistChuubans + "," + "" + chubaan + ""; ;
                        }
                    }

                    if (validChuubans != string.Empty)
                    {

                        var completedList = _commonService.GetCompletedChuubanList(validChuubans.ToLower());

                        ValidChuubanList = new List<string>();

                        foreach (string chubaan in ValidChuubans)
                        {

                            if (completedList.Any(chubaan.ToLower().Contains))
                            {
                                continue;
                            }
                            else
                            {
                                ValidChuubanList.Add(chubaan);
                            }

                        }

                        foreach (string completeChuuban in completedList)
                        {
                            if (completedChuubans == string.Empty)
                            {
                                completedChuubans = "" + completeChuuban + "";
                            }
                            else
                            {
                                completedChuubans = completedChuubans + "," + "" + completedChuubans + ""; ;
                            }
                        }
                    }
                }

                StringBuilder valMsg = new StringBuilder();

                if (enteredChuubans != string.Empty)
                {
                    if (invalidChuubans != string.Empty)
                    {
                        valMsg.Append(_localization.GetLocalizedHtmlString("valMsg_COM_FollowingChuubanNotFound"));
                        valMsg.Append("<br>");
                        invalidChuubans = invalidChuubans.Replace(",", "</br>");
                        valMsg.Append(invalidChuubans.ToUpper());
                    }


                    if (notExistChuubans != string.Empty)
                    {
                        valMsg.Append("<br>");
                        valMsg.Append(_localization.GetLocalizedHtmlString("valMsg_MultipleChuubansSearch_OrNoNotFound"));
                        valMsg.Append("<br>");
                        notExistChuubans = notExistChuubans.Replace(",", "</br>");
                        valMsg.Append(notExistChuubans.ToUpper());
                    }


                    if (completedChuubans != string.Empty)
                    {
                        valMsg.Append("<br>");
                        valMsg.Append(_localization.GetLocalizedHtmlString("valMsg_COM_MultipleChuubansSearchAlreadyCompleted"));
                        valMsg.Append("<br>");
                        completedChuubans = completedChuubans.Replace(",", "</br>");
                        valMsg.Append(completedChuubans.ToUpper());
                    }

                    if (Convert.ToString(valMsg) != string.Empty)
                    {
                        isValid = false;
                        return valMsg.ToString();
                        //popUpMessage.Visible = true;
                        //popUpMessage.ShowMessage(Constants.AlertType.WARNNING, valMsg.ToString());
                        //if (ViewState["controlID"] == null)
                        //{
                        //    ViewState["controlID"] = btnSearchByChubaanPopup.ClientID;
                        //}
                    }

                }
                else
                {
                    return _localization.GetLocalizedHtmlString("valMsg_COM_ChubanGyoubanRequired");
                }
                return string.Empty;
            }
            else
            {
                return string.Empty;
            }
        }

    }
}
