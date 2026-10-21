using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using TSWebApplication.Application.IServices;
using TSWebApplication.Application.Services;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Web.Models;
using System.Globalization;

namespace TSWebApplication.Web.Controllers.Transaction
{
    public class ProcessPlanHandoverHostController : Controller
    {
        private readonly IProcessPlanHandoverHostService _processPlanHandoverHostService;
        private readonly IViewRenderService _viewRenderService;
        private readonly LocalizationServices _localizationServices;

        public ProcessPlanHandoverHostController(IProcessPlanHandoverHostService processPlanHandoverHostService, IViewRenderService viewRenderService, LocalizationServices localizationServices)
        {
            _processPlanHandoverHostService = processPlanHandoverHostService;
            _viewRenderService = viewRenderService;
            _localizationServices = localizationServices;
        }

        public IActionResult ProcessPlanHandoverHost()
        {
            ViewBag.CultureName = UIHelper.GetCultureName();
            return View();
          
        }
        public IActionResult getValueCode([FromBody] List<GridSearchFields> processPlanDetails)
        {
            
            foreach (GridSearchFields field in processPlanDetails)
            {
                //if (!string.IsNullOrEmpty(field.COLOR_NAME))
                //{
                //    field.COLOR_ID = Convert.ToInt16(UIUtils.GetColorNameByID(field.COLOR_NAME.ToString()));
                //}
                if(field.CULTURE_NAME== "en")
                {
                    if (!string.IsNullOrEmpty(field.HOST_UNIT_TYPE_STRING))
                    {
                        if (field.HOST_UNIT_TYPE_STRING == "C")
                        {
                            field.HOST_UNIT_TYPE_STRING = Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_UnitType4Of4_L"));
                        }
                        else if (field.HOST_UNIT_TYPE_STRING == "P")
                        {
                            field.HOST_UNIT_TYPE_STRING = Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_Hone")); 
                        }
                        else if (field.HOST_UNIT_TYPE_STRING == "B")
                        {
                            field.HOST_UNIT_TYPE_STRING = Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_Thaba_L")); 
                        }
                        else
                        {
                            field.HOST_UNIT_TYPE_STRING = Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_Pieces_L"));
                        }
                        field.UNIT = Convert.ToInt16(UIUtils.GetUnitIDByName(field.HOST_UNIT_TYPE_STRING.ToString()));
                        field.PROCESS_PLAN_KUBUN = UIConstants.K_GENERAL;
                    }
                    if (!string.IsNullOrEmpty(field.MAGARI_KUBUN_STRING))
                    {
                        if (field.MAGARI_KUBUN_STRING == "ﾌﾚ")
                        {
                            field.MAGARI_KUBUN_STRING = Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_Bend_Round"));
                        }
                        else if (field.MAGARI_KUBUN_STRING == "ﾏｶﾞﾘ" || field.MAGARI_KUBUN_STRING.ToUpper() == "MAX")
                        {
                            field.MAGARI_KUBUN_STRING = Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_Bend")); 
                        }
                        else if (field.MAGARI_KUBUN_STRING == "ﾀﾝﾌﾚ")
                        {
                            field.MAGARI_KUBUN_STRING = Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_EdgeDistortion"));
                        }
                        field.MAGARI_KUBUN = Convert.ToInt16(UIUtils.GetMagariKubunValueByName(field.MAGARI_KUBUN_STRING.ToString())); 

                    }
                    if (!string.IsNullOrEmpty(field.HARDNESS_1))
                    {
                        field.HARDNESS_1 = UIUtils.GetStandardHardnessKubunValueByName(field.HARDNESS_1.ToString());
                    }
                    if (!string.IsNullOrEmpty(field.COIL_WND_DIR))
                    {
                        if (field.COIL_WND_DIR == "ﾐｷﾞ")
                        {
                            field.COIL_WND_DIR = Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_CoilWindDirections2of3")); 
                        }
                        else if (field.COIL_WND_DIR == "ﾋﾀﾞﾘ")
                        {
                            field.COIL_WND_DIR = Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_CoilWindDirections1of3")); 
                        }
                        field.COIL_WND_DIR = UIUtils.GetCoilDiaWindingKubunValueByName(field.COIL_WND_DIR.ToString());

                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(field.HOST_UNIT_TYPE_STRING))
                    {
                        if (field.HOST_UNIT_TYPE_STRING == "C")
                        {
                            field.HOST_UNIT_TYPE_STRING = Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_UnitType4Of4_L"));
                        }
                        else if (field.HOST_UNIT_TYPE_STRING == "P")
                        {
                            field.HOST_UNIT_TYPE_STRING = Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_Hone"));
                        }
                        else if (field.HOST_UNIT_TYPE_STRING == "B")
                        {
                            field.HOST_UNIT_TYPE_STRING = Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_Thaba_L"));
                        }
                        else
                        {
                            field.HOST_UNIT_TYPE_STRING = Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_Pieces_L"));
                        }
                        field.UNIT = Convert.ToInt16(UIUtils.GetUnitIDByName(field.HOST_UNIT_TYPE_STRING.ToString()));
                        field.PROCESS_PLAN_KUBUN = UIConstants.K_GENERAL;
                    }
                    if (!string.IsNullOrEmpty(field.MAGARI_KUBUN_STRING))
                    {
                        if (field.MAGARI_KUBUN_STRING == "ﾌﾚ")
                        {
                            field.MAGARI_KUBUN_STRING = Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_Bend_Round"));
                        }
                        else if (field.MAGARI_KUBUN_STRING == "ﾏｶﾞﾘ" || field.MAGARI_KUBUN_STRING.ToUpper() == "MAX")
                        {
                            field.MAGARI_KUBUN_STRING = Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_Bend"));
                        }
                        else if (field.MAGARI_KUBUN_STRING == "ﾀﾝﾌﾚ")
                        {
                            field.MAGARI_KUBUN_STRING = Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_EdgeDistortion"));
                        }
                        field.MAGARI_KUBUN = Convert.ToInt16(UIUtils.GetMagariKubunValueByName(field.MAGARI_KUBUN_STRING.ToString()));

                    }
                    if (!string.IsNullOrEmpty(field.HARDNESS_1))
                    {
                        field.HARDNESS_1 = UIUtils.GetStandardHardnessKubunValueByName(field.HARDNESS_1.ToString());
                    }
                    if (!string.IsNullOrEmpty(field.COIL_WND_DIR))
                    {
                        if (field.COIL_WND_DIR == "ﾐｷﾞ")
                        {
                            field.COIL_WND_DIR = Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_CoilWindDirections2of3"));
                        }
                        else if (field.COIL_WND_DIR == "ﾋﾀﾞﾘ")
                        {
                            field.COIL_WND_DIR = Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_CoilWindDirections1of3"));
                        }
                        field.COIL_WND_DIR = UIUtils.GetCoilDiaWindingKubunValueByName(field.COIL_WND_DIR.ToString());

                    }
                }
              
            }

            return Ok(new { IsSuccess = true, message = processPlanDetails });
        }
       
    }
}
