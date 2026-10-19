using System;
using System.Collections.Generic;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Transactions;
using TSWebApplication.Application.IServices;
using TSWebApplication.Infrastructure.IRepositories;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;
using System.Globalization;
using TSWebApplication.Infrastructure.Repositories;
using Org.BouncyCastle.Asn1.Cmp;
using TSWebApplication.Domain.Resources;
using static TSWebApplication.Domain.Utility.Enums;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Data;
using System.Linq;

namespace TSWebApplication.Application.Services
{
    public class HikinukiCoilToBarService : IHikinukiCoilToBarService
    {
        private readonly IHikinukiCoilToBarRepository _hikinukiCoilToBarRepository;
        private readonly IAllCommonRepository _commonRepository;
        private readonly LocalizationServices _localization;
        private readonly IAllCommonService _allCommonService;
        public HikinukiCoilToBarService(IHikinukiCoilToBarRepository hikinukiCoilToBarRepository, IAllCommonRepository commonRepository, LocalizationServices localization, IAllCommonService allCommonService)
        {
            _hikinukiCoilToBarRepository = hikinukiCoilToBarRepository;
            _commonRepository = commonRepository;
            _localization = localization;
            _allCommonService = allCommonService;
        }

        public List<ThabaDetails> GetThabaDetailsByChuuban(string chubaan, string gyouban)
        {
            var result= _hikinukiCoilToBarRepository.GetThabaDetails(chubaan, gyouban);
            foreach (ThabaDetails record in result)
            {

                string decisionName = UIUtils.GetResultKubunValue(Convert.ToString(record.DECISION));
                if (_localization.GetLocalizedHtmlString("lbl_OK") == decisionName)
                {
                    record.DECISION_VALUE = 1;
                }
                if (_localization.GetLocalizedHtmlString("lbl_NG") == decisionName)
                {
                    record.DECISION_VALUE = 2;
                }
                if (_localization.GetLocalizedHtmlString("lbl_Hold") == decisionName)
                {
                    record.DECISION_VALUE = 3;
                }
                if (_localization.GetLocalizedHtmlString("lbl_DecisionKubun4of4") == decisionName)
                {
                    record.DECISION_VALUE = 4;
                }
                if (_localization.GetLocalizedHtmlString("lbl_ReHeatTreatment") == decisionName || _localization.GetLocalizedHtmlString("lbl_ReDrawing") == decisionName ||
               _localization.GetLocalizedHtmlString("lbl_Restraightening") == decisionName || _localization.GetLocalizedHtmlString("lbl_Re_Chamfering") == decisionName || _localization.GetLocalizedHtmlString("lbl_ReEc") == decisionName)
                {
                    record.DECISION_VALUE = 5;
                }
            }
            return result;
        }

        public HikinukiDetails GetHikinukiDetailsByChuuban(string chuuban, string gyouban, string mechineCD, string cultureName)
        {
           

            var obj = _hikinukiCoilToBarRepository.GetHikinukiDetailsByChuuban(chuuban, gyouban, mechineCD);

            if (obj != null)
            {
                if (cultureName == "en")
                {
                    System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
                }
                else
                {
                    System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("ja-JP");
                }
                if (obj.PP_UNIT != null)
                {
                    obj.PP_NO_OF_UNITS_DECODE = Convert.ToDecimal(obj.PP_NO_OF_UNITS).ToString("0.####") + UIUtils.GetProductUnitKubunValue(Convert.ToString(obj.PP_UNIT));
                }

                //if (obj.DW_START_OPERATOR_NAME != null)    // Stat Data :  Bind DW No Of Unit
                //{
                //    obj.DW_NO_OF_COILS = Convert.ToDecimal(obj.PP_NO_OF_UNITS).ToString("0.####") + UIUtils.Instance.GetProductUnitKubunValue(Convert.ToString(obj.PP_UNIT));
                //}


                if (obj.MAGARI_KUBUN != null)
                {
                    obj.PP_MAGARI_BEND_TOL = Utils.GetMagariBendTolSpan(UIUtils.Instance.GetMagariKubunValue(Convert.ToString(obj.MAGARI_KUBUN)), Convert.ToDecimal(obj.MAGARI_BEND_TOL), Convert.ToInt32(obj.MAGARI_SPAN));  //Convert.ToDecimal(obj.PP_NO_OF_UNITS).ToString("0.####") + UIUtils.Instance.GetProductUnitKubunValue(Convert.ToString(obj.PP_UNIT));
                }

                if (obj.PP_UNIT_WT_KUBUN > 0)
                {
                    obj.PP_UNIT_WT_STRING = Utils.GetUnitWeigth(Convert.ToInt16(obj.PP_UNIT_WT_KUBUN), Convert.ToInt32(obj.PP_UNIT_WT_STRING), Convert.ToInt16(obj.PP_UNIT_WT_MIN), obj.PP_UNIT_WT_MAX);

                }

                if (obj.PP_COIL_DIA_WINDING != null)
                {
                    obj.PP_COIL_DIA_WINDING = UIUtils.GetCoilDiaWindingKubunValue(Convert.ToString(obj.PP_COIL_DIA_WINDING));
                }
                if (obj.PP_STD_DEL_PERIOD >= 0)
                {
                    obj.PP_STD_DEL_PERIOD_STRING = obj.PP_STD_DEL_PERIOD + " "+ _localization.GetLocalizedHtmlString("lbl_Day");
                }
                obj.DECISION_OK = Convert.ToInt16(UIUtils.GetResultKubunKey(_localization.GetLocalizedHtmlString("lbl_OK")));
                var messageList = _commonRepository.GetMessageDetailsByProcessPlanCD(Convert.ToInt64(obj.PP_PROCESS_PLAN_CD), true);
                obj.DW_MESSAGE_LIST = messageList;

                obj.PP_ELONGATION = Helper.GetProductPatternFormat(Convert.ToSingle(obj.ELONGATION_PATTERN1), obj.ELONGATION_PATTERN1_MIN_MAX, Convert.ToDecimal(obj.ELONGATION_PATTERN2_MIN), Convert.ToDecimal(obj.ELONGATION_PATTERN2_MAX), obj.ELONGATION_PATTERN3_ACTUAL);
                obj.PP_TENSILE_STRENGTH = Helper.GetProductPatternFormat(Convert.ToSingle(obj.TENSILE_STRN_PATTERN1), obj.TENSILE_STRN_PATTERN1_MIN_MAX, obj.TENSILE_STRN_PATTERN2_MIN, obj.TENSILE_STRN_PATTERN2_MAX, obj.TENSILE_STRN_PATTERN3_ACTUAL);
                obj.PP_HARDNESS = Helper.GetHardnessProductPatternFormat(obj.HARDNESS_PATTERN1_CODE, obj.HARDNESS_PATTERN1, obj.HARDNESS_PATTERN1_MIN_MAX, obj.HARDNESS_PATTERN2_CODE, obj.HARDNESS_PATTERN2_MIN, obj.HARDNESS_PATTERN2_MAX, obj.HARDNESS_PATTERN3_ACTUAL);
                obj.PP_DECARB = Helper.GetDecarbProductPatternFormat(obj.DECARB_1, Convert.ToSingle(obj.DECARB_VAL_1), obj.DECARB_2, Convert.ToSingle(obj.DECARB_VAL_2), obj.DECARB_3, Convert.ToSingle(obj.DECARB_VAL_3));
                obj.PP_YEILD = Helper.GetProductPatternFormat(Convert.ToSingle(obj.YEILD_PATTERN1), obj.YEILD_PATTERN1_MIN_MAX, obj.YEILD_PATTERN2_MIN, obj.YEILD_PATTERN2_MAX, obj.YEILD_PATTERN3_ACTUAL);
                obj.PP_PRESS = Helper.GetProductPatternFormat(Convert.ToSingle(obj.PRESS_PATTERN1), obj.PRESS_PATTERN1_MIN_MAX, obj.PRESS_PATTERN2_MIN, obj.PRESS_PATTERN2_MAX, obj.PRESS_PATTERN3_ACTUAL);

            }

            return obj;
        }

        public HikinukiDetails GetHikinukiHistoryByChuuban(GridSearchFields request)
        {
            HikinukiDetails result = new HikinukiDetails();
            if (request.CULTURE_NAME == "en")
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
            }
            else
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("ja-JP");
            }

            result = _hikinukiCoilToBarRepository.GetHikinukiHistoryDetailsByChuuban(request.CHUUBAN, request.GYOUBAN);
            if (result != null)
            {
                if (result.MAGARI_KUBUN != null)
                {
                    result.PP_MAGARI_BEND_TOL = Utils.GetMagariBendTolSpan(UIUtils.Instance.GetMagariKubunValue(Convert.ToString(result.MAGARI_KUBUN)), Convert.ToDecimal(result.MAGARI_BEND_TOL), Convert.ToInt32(result.MAGARI_SPAN));
                }
                if (result.DW_DECISION != null)
                {
                    result.DW_DECISION_STRING = UIUtils.GetResultKubunValue(Convert.ToString(result.DW_DECISION));
                }
                if (result.PP_STD_DEL_PERIOD >= 0)
                {
                    result.PP_STD_DEL_PERIOD_STRING = result.PP_STD_DEL_PERIOD + " " + _localization.GetLocalizedHtmlString("lbl_Day");
                }
                if (result.MAGARI_KUBUN != null)
                {
                    result.PP_MAGARI_BEND_TOL = Utils.GetMagariBendTolSpan(UIUtils.Instance.GetMagariKubunValue(Convert.ToString(result.MAGARI_KUBUN)), Convert.ToDecimal(result.MAGARI_BEND_TOL), Convert.ToInt32(result.MAGARI_SPAN));  //Convert.ToDecimal(obj.PP_NO_OF_UNITS).ToString("0.####") + UIUtils.Instance.GetProductUnitKubunValue(Convert.ToString(obj.PP_UNIT));
                }

                if (result.PP_UNIT_WT_KUBUN > 0)
                {
                    result.PP_UNIT_WT_STRING = Utils.GetUnitWeigth(Convert.ToInt16(result.PP_UNIT_WT_KUBUN), Convert.ToInt32(result.PP_UNIT_WT_STRING), Convert.ToInt16(result.PP_UNIT_WT_MIN), result.PP_UNIT_WT_MAX);

                }

                if (result.PP_COIL_DIA_WINDING != null)
                {
                    result.PP_COIL_DIA_WINDING = UIUtils.GetCoilDiaWindingKubunValue(Convert.ToString(result.PP_COIL_DIA_WINDING));
                }
                if (result.PP_UNIT != null)
                {
                    result.PP_NO_OF_UNITS_DECODE = Convert.ToDecimal(result.PP_NO_OF_UNITS).ToString("0.####") + UIUtils.GetProductUnitKubunValue(Convert.ToString(result.PP_UNIT));
                }

            }

            var historyList = BindHistoryGrid(request.CHUUBAN, request.GYOUBAN, request.CULTURE_NAME);
            foreach (HikinukiDetails record in historyList)
            {
                record.ng_details = string.Empty;

                if (Convert.ToDecimal(record.DW_FINISHED_BARS) > 0)
                {
                    record.DW_FINISHED_BARS_DECODE = Convert.ToString(record.DW_FINISHED_BARS + UIUtils.GetProductUnitKubunValue(Convert.ToString(record.DW_UNIT_TYPE)));
                }

                if (record.DW_NG_BARS1 > 0)
                {
                    record.DW_NG_DETAILS1_DECODE = record.DW_NG_BARS1 + Constants.StringValues.OPEN_PARAMETER + record.DW_NG_DETAILS1_DECODE + Constants.StringValues.CLOSE_PARAMETER;
                }
                if (record.DW_NG_BARS2 > 0)
                {
                    record.DW_NG_DETAILS2_DECODE = record.DW_NG_BARS2 + Constants.StringValues.OPEN_PARAMETER + record.DW_NG_DETAILS2_DECODE + Constants.StringValues.CLOSE_PARAMETER;
                }
                if (record.DW_NG_BARS3 > 0)
                {
                    record.DW_NG_DETAILS3_DECODE = record.DW_NG_BARS3 + Constants.StringValues.OPEN_PARAMETER + record.DW_NG_DETAILS3_DECODE + Constants.StringValues.CLOSE_PARAMETER;
                }

                if (record.DW_BEND_STATUS > 0)
                {

                    record.DW_BEND_STATUS_DECODE = UIUtils.GetBendStateKubunValue(Convert.ToString(record.DW_BEND_STATUS));
                }
                if (record.DW_EXT_TEXTURE_STATUS > 0)
                {
                    record.DW_EXT_TEXTURE_STATUS_DECODE = UIUtils.GetExteriorConditionKubunValue(Convert.ToString(record.DW_EXT_TEXTURE_STATUS));
                }
                //    if (record.DW_DIE_SHAPE != null)
                //    {
                //        record.DW_DIE_SHAPE = UIUtils.GetDieShapeList(Convert.ToString(record.DW_DIE_SHAPE));
                //    }
                //    if (record.DW_DECISION != null)
                {
                    record.DW_DECISION_STRING = UIUtils.GetResultKubunValue(Convert.ToString(record.DW_DECISION));
                }


            }
                if (historyList.Count > 0)
            {
                result.PROCESS_HISTORY = historyList;
            }
            return result;
        }
        
        public List<HikinukiDetails> BindHistoryGrid(string chubban, short? gyouban, string cultureName)
        {
            return _hikinukiCoilToBarRepository.GetProcessHistoryList(chubban, gyouban, cultureName);
        }

        public List<HikinukiDetails> GetHikinuki24MachineDetails(string machineCd, string cultureName)
        {
            var obj = _hikinukiCoilToBarRepository.GetHikinuki24MachineList(machineCd);
            if (cultureName == "en")
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
            }
            else
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("ja-JP");
            }
            foreach (HikinukiDetails record in obj)
            {
                string decisionName = UIUtils.GetResultKubunValue(Convert.ToString(record.DW_DECISION));
                if (_localization.GetLocalizedHtmlString("lbl_OK") == decisionName)
                {
                    record.DW_DECISION = 1;
                }
                if (_localization.GetLocalizedHtmlString("lbl_NG") == decisionName)
                {
                    record.DW_DECISION = 2;
                }
                if (_localization.GetLocalizedHtmlString("lbl_Hold") == decisionName)
                {
                    record.DW_DECISION = 3;
                }
                if (_localization.GetLocalizedHtmlString("lbl_DecisionKubun4of4") == decisionName)
                {
                    record.DW_DECISION = 4;
                }
                if (_localization.GetLocalizedHtmlString("lbl_ReHeatTreatment") == decisionName || _localization.GetLocalizedHtmlString("lbl_ReDrawing") == decisionName ||
              _localization.GetLocalizedHtmlString("lbl_Restraightening") == decisionName || _localization.GetLocalizedHtmlString("lbl_Re_Chamfering") == decisionName || _localization.GetLocalizedHtmlString("lbl_ReEc") == decisionName)
                {
                    record.DW_DECISION = 5;
                }
                if (record.UNIT_TYPE != null)
                {
                    record.DW_UNIT_TYPE_STRING = UIUtils.GetUnitTypeName(Convert.ToString(record.UNIT_TYPE));
                }

                record.DW_FINISHED_BARS_DECODE = record.DW_FINISHED_BARS != null ? record.DW_FINISHED_BARS + UIUtils.GetProductUnitKubunValue(Convert.ToString(record.DW_UNIT_TYPE)) : string.Empty;
                record.PP_NO_OF_UNITS_DECODE = record.PP_NO_OF_UNITS != null ? record.PP_NO_OF_UNITS + UIUtils.GetProductUnitKubunValue(Convert.ToString(record.PP_UNIT)) : string.Empty;
                record.DW_BEND_STATUS_DECODE = record.DW_BEND_STATUS != null ? UIUtils.GetBendStateKubunValue(Convert.ToString(record.DW_BEND_STATUS)) : string.Empty;
                record.DW_EXT_TEXTURE_STATUS_DECODE = record.DW_EXT_TEXTURE_STATUS != null ? UIUtils.GetExteriorConditionKubunValue(Convert.ToString(record.DW_EXT_TEXTURE_STATUS)) : string.Empty;
                if (record.DW_DIE_DIM > 0)
                {
                    record.DW_DIE_DIM_STRING = record.DW_DIE_DIM + UIUtils.GetDieShapeList(Convert.ToString(record.DW_DIE_SHAPE));
                }
            }
            return obj;
        }

        public HikinukiDetails InsertHikinukiDetails(HikinukiDetails request)
        {

            using (TransactionScope transaction = new TransactionScope())
            {
                if (request.DW_USER_ID != null && request.DW_USER_ID > 0)
                {
                    request.INPUT_LOCATION_CD = Convert.ToInt16(_commonRepository.GetUserLocationById(request.DW_USER_ID.Value));
                }
                // request.DW_LOCATION_CD = request.INPUT_LOCATION_CD;

                if (request.DW_MACHINE_CD != null && request.DW_MACHINE_CD > 0)
                {
                    request.DW_LOCATION_CD = _commonRepository.GetMachineNameByID(Convert.ToInt64(request.DW_MACHINE_CD.Value))?.LOCATION_CD;
                }
                request.PROCESS_KUBUN_CD = Convert.ToInt16(UIUtils.GetProcessKubun(_localization.GetLocalizedHtmlString("lbl_ProcessDrawingCtoB")));
                WipStatusDetails wipDetails = new WipStatusDetails();
                wipDetails = new WipStatusDetails();
                wipDetails.PROCESS_PLAN_CD = Convert.ToInt32(request.PP_PROCESS_PLAN_CD);
                wipDetails.LOCATION_CD = Convert.ToInt16(request.DW_LOCATION_CD);
                wipDetails.MSG_CD = null;
                wipDetails.FINISHED_BARS = null;
                wipDetails.UNIT =null;
                wipDetails.FINISHED_QTY = null;
                wipDetails.PROCESS_START_DATE = Convert.ToDateTime(request.DW_START_DATE);
                wipDetails.PROCESS_END_DATE = null;
                    wipDetails.PROCESS_KUBUN_CD = request.PROCESS_KUBUN_CD;
                    wipDetails.PRE_PROSESS_DECISION = null;
                    wipDetails.NEXT_PROCESS = null;
                    wipDetails.START_OPERATOR_CD = request.DW_START_OPERATOR > 0 ? request.DW_START_OPERATOR : null;
                    wipDetails.END_OPERATOR_CD = null;
                if (request.DW_TABA_NO_1 != null)
                {
                    wipDetails.TABA_NO = Convert.ToInt16(request.DW_TABA_NO_1);
                }
                else
                {
                    wipDetails.TABA_NO = 0;
                }
                if (request.DW_TABA_NO_1 != null && request.DW_TABA_NO_2 != null || request.DW_TABA_NO_3 != null || request.DW_TABA_NO_4 != null)
                {
                    wipDetails.TABA_GROUP_NO = request.DW_TABA_NO_1;
                }
                 _commonRepository.DeleteWIPStatusData(wipDetails);
                if (request.DW_TABA_NO_1 > 0)
                {
                    wipDetails.TABA_NO = Convert.ToInt16(request.DW_TABA_NO_1);
                    _commonRepository.DeleteWIPStatusData(wipDetails);
                }
                if (request.DW_TABA_NO_2 > 0)
                {
                    
                    wipDetails.TABA_NO = Convert.ToInt16(request.DW_TABA_NO_2);
                    _commonRepository.DeleteWIPStatusData(wipDetails);
                }
                if (request.DW_TABA_NO_3 > 0)
                {
                   
                    wipDetails.TABA_NO = Convert.ToInt16(request.DW_TABA_NO_3);
                    _commonRepository.DeleteWIPStatusData(wipDetails);
                }
                if (request.DW_TABA_NO_4 > 0)
                {
                    
                    wipDetails.TABA_NO = Convert.ToInt16(request.DW_TABA_NO_4);
                    _commonRepository.DeleteWIPStatusData(wipDetails);
                }

            var hikinukiDetails = _hikinukiCoilToBarRepository.InsertHikinukiDetails(request);
            if (hikinukiDetails != null)
            {
                if (hikinukiDetails.PP_UNIT != null)
                {
                    hikinukiDetails.PP_NO_OF_UNITS_DECODE = Convert.ToDecimal(hikinukiDetails.PP_NO_OF_UNITS).ToString("0.####") + UIUtils.GetProductUnitKubunValue(Convert.ToString(hikinukiDetails.PP_UNIT));
                }

                if (hikinukiDetails.MAGARI_KUBUN != null)
                {
                    hikinukiDetails.PP_MAGARI_BEND_TOL = Utils.GetMagariBendTolSpan(UIUtils.Instance.GetMagariKubunValue(Convert.ToString(hikinukiDetails.MAGARI_KUBUN)), Convert.ToDecimal(hikinukiDetails.MAGARI_BEND_TOL), Convert.ToInt32(hikinukiDetails.MAGARI_SPAN));  //Convert.ToDecimal(obj.PP_NO_OF_UNITS).ToString("0.####") + UIUtils.Instance.GetProductUnitKubunValue(Convert.ToString(obj.PP_UNIT));
                }

                    if (hikinukiDetails.PP_UNIT_WT_KUBUN > 0)
                    {
                        hikinukiDetails.PP_UNIT_WT_STRING = Utils.GetUnitWeigth(Convert.ToInt16(hikinukiDetails.PP_UNIT_WT_KUBUN), Convert.ToInt32(hikinukiDetails.PP_UNIT_WT_STRING), Convert.ToInt16(hikinukiDetails.PP_UNIT_WT_MIN), hikinukiDetails.PP_UNIT_WT_MAX);

                    }

                    if (hikinukiDetails.PP_COIL_DIA_WINDING != null)
                {
                    hikinukiDetails.PP_COIL_DIA_WINDING = UIUtils.GetCoilDiaWindingKubunValue(Convert.ToString(hikinukiDetails.PP_COIL_DIA_WINDING));
                }
                if (hikinukiDetails.DW_DECISION != null)
                {
                    hikinukiDetails.DW_DECISION_STRING = UIUtils.GetResultKubunValue(Convert.ToString(hikinukiDetails.DW_DECISION));
                }
                    if (hikinukiDetails.PP_STD_DEL_PERIOD >= 0)
                    {
                        hikinukiDetails.PP_STD_DEL_PERIOD_STRING = hikinukiDetails.PP_STD_DEL_PERIOD + " " + _localization.GetLocalizedHtmlString("lbl_Day");
                    }
                    hikinukiDetails.DECISION_OK = Convert.ToInt16(UIUtils.GetResultKubunKey(_localization.GetLocalizedHtmlString("lbl_OK")));
                    //if (hikinukiDetails.PP_COLOR_ID != null)
                    //{
                    //    hikinukiDetails.PP_COLOR = UIUtils.GetColorKubunValue(Convert.ToString(hikinukiDetails.PP_COLOR_ID));         // Utils.GetColorName(Convert.ToInt16(obj.PP_COLOR_ID));
                    //}
                }

            var messageList = _commonRepository.GetMessageDetailsByProcessPlanCD(Convert.ToInt64(hikinukiDetails.PP_PROCESS_PLAN_CD), true);       //Added by sagar b on 18/8/2017
            hikinukiDetails.DW_MESSAGE_LIST = messageList;

                transaction.Complete();

                return hikinukiDetails;
            }
        }

        public long UpdateHikinukiDetails(HikinukiDetails request)
        {
            long msgCD = 0;
            bool deleteMsg=false;
            if (request.DW_USER_ID != null && request.DW_USER_ID > 0)
            {
                request.INPUT_LOCATION_CD = Convert.ToInt16(_commonRepository.GetUserLocationById(request.DW_USER_ID.Value));
            }
            // request.DW_LOCATION_CD = request.INPUT_LOCATION_CD;

            if (request.DW_MACHINE_CD != null && request.DW_MACHINE_CD > 0)
            {
                request.DW_LOCATION_CD = _commonRepository.GetMachineNameByID(Convert.ToInt64(request.DW_MACHINE_CD.Value))?.LOCATION_CD;
            }
            using (TransactionScope transaction = new TransactionScope())
            {
                // If Remarks is specified, first save the Remarks in TRN_MESSAGE
                MessageDetails msgDetails = new MessageDetails();
                msgDetails.PROCESS_PLAN_CD = Convert.ToInt64(request.PP_PROCESS_PLAN_CD);
                msgDetails.MACHINE_CD = Convert.ToInt32(request.DW_MACHINE_CD);
                msgDetails.PROCESS_KUBUN_ID = Convert.ToInt32(UIUtils.GetProcessKubun(_localization.GetLocalizedHtmlString("lbl_ProcessDrawingCtoB")));
                msgDetails.OPERATOR_CD = Convert.ToInt64(request.DW_END_OPERATOR);
                msgDetails.MSG_CONTENTS = request.DW_REMARKS;
                msgDetails.USER_ID = Convert.ToInt32(request.DW_USER_ID);
                msgDetails.LOCATION_CD = Convert.ToInt32(request.DW_LOCATION_CD);
                msgDetails.INPUT_LOCATION_CD = Convert.ToInt32(request.DW_LOCATION_CD);

                if (!string.IsNullOrEmpty(request.DW_REMARKS) && request.DW_MSG_CD == null)
                {
                   

                    msgDetails = _commonRepository.MessageDetailsInsert(msgDetails);
                    if (msgDetails.MSG_CD != Constants.NullValue.LONG && msgDetails.MSG_CD > 0)
                    {
                        request.DW_MSG_CD = msgDetails.MSG_CD;
                    }
                }
                else if (request.DW_MSG_CD > 0 && !string.IsNullOrEmpty(request.DW_REMARKS))
                {
                    msgDetails.MSG_CD = Convert.ToInt64(request.DW_MSG_CD);
                    _commonRepository.MessageDetailsUpdate(msgDetails);
                   
                }
                else if (request.DW_MSG_CD > 0 && string.IsNullOrEmpty(request.DW_REMARKS))
                {
                    long messageCD = Convert.ToInt64(request.DW_MSG_CD);
                    request.DW_MSG_CD = null;
                   
                    msgCD = _hikinukiCoilToBarRepository.UpdateHikinukiDetails(request);
                    _commonRepository.MessageDetailsDelete(messageCD);
                    deleteMsg = true;   

                }
                if(deleteMsg == false)
                {
                    msgCD = _hikinukiCoilToBarRepository.UpdateHikinukiDetails(request);
                }
                


                if (request.DW_NEXT_PROCESS != 999)
                {
                    WipStatusDetails wipDetails = new WipStatusDetails();
                    wipDetails = new WipStatusDetails();
                    wipDetails.PROCESS_PLAN_CD = Convert.ToInt32(request.PP_PROCESS_PLAN_CD);
                    wipDetails.LOCATION_CD = Convert.ToInt16(request.DW_LOCATION_CD);
                    wipDetails.MSG_CD = request.DW_MSG_CD > 0 ? request.DW_MSG_CD : null;
                    wipDetails.FINISHED_BARS = request.DW_FINISHED_BARS;
                    wipDetails.UNIT = request.DW_UNIT_TYPE;
                    wipDetails.FINISHED_QTY = request.DW_FINISHED_QTY;
                    wipDetails.PROCESS_START_DATE = Convert.ToDateTime(request.DW_START_DATE);
                    wipDetails.PROCESS_END_DATE = Convert.ToDateTime(request.DW_END_DATE);
                    wipDetails.PROCESS_KUBUN_CD = Convert.ToInt32(UIUtils.GetProcessKubun(_localization.GetLocalizedHtmlString("lbl_ProcessDrawingCtoB")));
                    wipDetails.PRE_PROSESS_DECISION = request.DW_DECISION;
                    wipDetails.NEXT_PROCESS = Convert.ToInt16(request.DW_NEXT_PROCESS);
                    wipDetails.START_OPERATOR_CD = request.DW_START_OPERATOR > 0 ? request.DW_START_OPERATOR : null;
                    wipDetails.END_OPERATOR_CD = request.DW_END_OPERATOR > 0 ? request.DW_END_OPERATOR : null;

                    if (request.DW_TABA_NO_1 != null && request.DW_TABA_NO_2 != null || request.DW_TABA_NO_3 != null || request.DW_TABA_NO_4 != null)
                    {
                        wipDetails.TABA_GROUP_NO = request.DW_TABA_NO_1;
                    }

                    if (request.DW_TABA_NO_1 > 0)
                    {

                        wipDetails.TABA_NO = Convert.ToInt16(request.DW_TABA_NO_1);
                        _commonRepository.DeleteWIPStatusData(wipDetails);
                    }
                    if (request.DW_TABA_NO_2 > 0)
                    {
                        wipDetails.FINISHED_QTY = null;
                        wipDetails.FINISHED_BARS = null;
                        wipDetails.UNIT = null;
                        wipDetails.TABA_NO = Convert.ToInt16(request.DW_TABA_NO_2);
                        _commonRepository.DeleteWIPStatusData(wipDetails);
                    }
                    if (request.DW_TABA_NO_3 > 0)
                    {
                        wipDetails.FINISHED_QTY = null;
                        wipDetails.FINISHED_BARS = null;
                        wipDetails.UNIT = null;
                        wipDetails.TABA_NO = Convert.ToInt16(request.DW_TABA_NO_3);
                        _commonRepository.DeleteWIPStatusData(wipDetails);
                    }
                    if (request.DW_TABA_NO_4 > 0)
                    {
                        wipDetails.FINISHED_QTY = null;
                        wipDetails.FINISHED_BARS = null;
                        wipDetails.UNIT = null;
                        wipDetails.TABA_NO = Convert.ToInt16(request.DW_TABA_NO_4);
                        _commonRepository.DeleteWIPStatusData(wipDetails);
                    }
                }
                
                    transaction.Complete();
                }
            return msgCD;
        }



        public List<HikinukiDetails> GetHikinukiCoilToBarList(GridSearchFields gridSearchRequest)
        {
            short locationCD = Convert.ToInt16(_commonRepository.GetUserLocationById(Convert.ToInt32(gridSearchRequest.USER_ID)));
            List<HikinukiDetails> result = new List<HikinukiDetails>();
            List<SearchField> searchFields = ComposeSearchCriteria(gridSearchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);
            result = _hikinukiCoilToBarRepository.GetHikinukiList(searchFields, selectedSortCriteria, locationCD);
                       
            return result;
        }

        public HikinukiDetails GetHikinukiDetailsDrawingByCD(long drawingCD, string cultureName)
        {
            if (cultureName == "en")
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
            }
            else
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("ja-JP");
            }

            var obj = _hikinukiCoilToBarRepository.GetHikinukiDetailsDrawingByCD(drawingCD);
            if (obj != null)
            {
                if (obj.PP_UNIT != null)
                {
                    obj.PP_NO_OF_UNITS_DECODE = Convert.ToDecimal(obj.PP_NO_OF_UNITS).ToString("0.####") + UIUtils.GetProductUnitKubunValue(Convert.ToString(obj.PP_UNIT));
                }

                if (obj.MAGARI_KUBUN != null)
                {
                    obj.PP_MAGARI_BEND_TOL = Utils.GetMagariBendTolSpan(UIUtils.Instance.GetMagariKubunValue(Convert.ToString(obj.MAGARI_KUBUN)), Convert.ToDecimal(obj.MAGARI_BEND_TOL), Convert.ToInt32(obj.MAGARI_SPAN));  //Convert.ToDecimal(obj.PP_NO_OF_UNITS).ToString("0.####") + UIUtils.Instance.GetProductUnitKubunValue(Convert.ToString(obj.PP_UNIT));
                }

                if (obj.PP_UNIT_WT_KUBUN > 0)
                {
                    obj.PP_UNIT_WT_STRING = Utils.GetUnitWeigth(Convert.ToInt16(obj.PP_UNIT_WT_KUBUN), Convert.ToInt32(obj.PP_UNIT_WT_STRING), Convert.ToInt16(obj.PP_UNIT_WT_MIN), obj.PP_UNIT_WT_MAX);

                }

                if (obj.PP_COIL_DIA_WINDING != null)
                {
                    obj.PP_COIL_DIA_WINDING = UIUtils.GetCoilDiaWindingKubunValue(Convert.ToString(obj.PP_COIL_DIA_WINDING)); //  Utils.GetCoilDiaWindingValue(Convert.ToInt16(obj.PP_COIL_DIA_WINDING));
                }
                if (obj.PP_STD_DEL_PERIOD >= 0)
                {
                    obj.PP_STD_DEL_PERIOD_STRING = obj.PP_STD_DEL_PERIOD + " " + _localization.GetLocalizedHtmlString("lbl_Day");
                }
                obj.DECISION_OK = Convert.ToInt16(UIUtils.GetResultKubunKey(_localization.GetLocalizedHtmlString("lbl_OK")));
            }
            return obj;
        }

        public void Delete(HikinukiDetails details)
        {

            WipStatusDetails wipDetails = new WipStatusDetails();
            wipDetails = new WipStatusDetails();
            wipDetails.PROCESS_PLAN_CD = details.PP_PROCESS_PLAN_CD;
            using (TransactionScope transaction = new TransactionScope())
            {
                
                    _hikinukiCoilToBarRepository.Delete(Convert.ToInt64(details.DW_DRAWING_CD));
               
                if (details.DW_TABA_NO_1 > 0)
                {
                    wipDetails.TABA_NO = details.DW_TABA_NO_1;
                    _commonRepository.DeleteWIPStatusData(wipDetails);
                }
                if (details.DW_TABA_NO_2 > 0)
                {
                    wipDetails.TABA_NO = details.DW_TABA_NO_2;
                    _commonRepository.DeleteWIPStatusData(wipDetails);
                }
                if (details.DW_TABA_NO_3 > 0)
                {
                    wipDetails.TABA_NO = details.DW_TABA_NO_3;
                    _commonRepository.DeleteWIPStatusData(wipDetails);
                }
                if (details.DW_TABA_NO_4 > 0)
                {
                    wipDetails.TABA_NO = details.DW_TABA_NO_4;
                    _commonRepository.DeleteWIPStatusData(wipDetails);
                }

                if (details.DW_MSG_CD > 0)
                {
                    _commonRepository.MessageDetailsDelete(Convert.ToInt64(details.DW_MSG_CD));
                }

                transaction.Complete();
            }
        }

        private List<SearchField> ComposeSearchCriteria(GridSearchFields gridSearchRequest)
        {
            var searchFields = new List<SearchField>();

            if (!string.IsNullOrEmpty(gridSearchRequest.CHUUBAN))
            {
                
                var searchField = new SearchField
                {
                    FieldName = "CONCAT(PP.CHUUBAN,CASE WHEN LENGTH(TRIM(PP.GYOUBAN))=1 THEN '0'||PP.GYOUBAN END)",// FieldName = "PP.CHUUBAN",
                    SearchType = SearchType.Text,

                    SearchText1 = gridSearchRequest.CHUUBAN.Trim()

                };

                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.CUSTOMER_NAME))
            {
                var searchField = new SearchField
                {
                    FieldName = "CST.SHORT_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.CUSTOMER_NAME.ToLower().Trim()
                };

                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_NAME_CUST))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_NAME_CUST",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.PROD_NAME_CUST.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.INTERNAL_NAME_STRING))
            {
                var searchField = new SearchField
                {
                    // FieldName = "PP.PROD_NAME_INT",
                    FieldName = "INTE.INTERNAL_NAME",
                    SearchType = SearchType.Text,
                    SearchCondition = SearchCondition.Equal.ToString(),
                    SearchText1 = gridSearchRequest.INTERNAL_NAME_STRING.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }

            if (gridSearchRequest.PROD_DIM_D_MIN > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_DIM_D",
                    SearchType = SearchType.Number,
                    SearchText1 = gridSearchRequest.PROD_DIM_D_MIN.ToString(),
                    SearchText2 = gridSearchRequest.PROD_DIM_D_MAX.ToString()

                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_SHAPE))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_SHAPE",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.PROD_SHAPE.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }

            if (gridSearchRequest.PROD_LENGTH_MIN > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_LENGTH",
                    SearchType = SearchType.Number,
                    SearchText1 = gridSearchRequest.PROD_LENGTH_MIN.ToString(),
                    SearchText2 = gridSearchRequest.PROD_LENGTH_MAX.ToString()
                };
                searchFields.Add(searchField);
            }
            return searchFields;
        }

        public List<HikinukiDetails> GetCombinedDailyReportData(GridSearchFields gridSearchRequest)
        {

            List<SearchField> searchFields = ComposeSearchCriteriaforFactoryDailyWorkStatus(gridSearchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);
            int rowLimit = gridSearchRequest.ROW_LIMIT;
            var obj = _hikinukiCoilToBarRepository.GetCombinedDailyReportData(searchFields, selectedSortCriteria, rowLimit, gridSearchRequest.CULTURE_NAME);
            return obj;

        }
        private List<SearchField> ComposeSearchCriteriaforFactoryDailyWorkStatus(GridSearchFields gridSearchRequest)
        {
            var searchFields = new List<SearchField>();
            if (gridSearchRequest.MACHINE_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "DW.MACHINE_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MACHINE_CD)
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.LOCATION_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "DW.LOCATION_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.LOCATION_CD)
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.CUSTOMER_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.CUSTOMER_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.CUSTOMER_CD)
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.END_OPERATOR_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "DW.DW_END_OPERATOR_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.END_OPERATOR_CD)
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_NAME_CUST))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_NAME_CUST",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.PROD_NAME_CUST.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.INTERNAL_NAME_STRING))
            {
                var searchField = new SearchField
                {
                    FieldName = "INTERN.INTERNAL_NAME",
                    SearchType = SearchType.Text,
                    SearchCondition = SearchCondition.Equal.ToString(),
                    SearchText1 = gridSearchRequest.INTERNAL_NAME_STRING.ToLower().Trim()
                    
                };
                searchFields.Add(searchField);
            }


            if (gridSearchRequest.PROD_DIM_D_MAX == null)
            {
                if (gridSearchRequest.PROD_DIM_D_MIN > 0)
                {
                    var searchField = new SearchField
                    {
                        FieldName = "PP.PROD_DIM_D",
                        SearchType = SearchType.Number,
                        SearchText1 = Convert.ToString(gridSearchRequest.PROD_DIM_D_MIN)
                    };
                    searchFields.Add(searchField);
                }
            }
            else if (gridSearchRequest.PROD_DIM_D_MIN == null)
            {
                if (gridSearchRequest.PROD_DIM_D_MAX > 0)
                {
                    var searchField = new SearchField
                    {
                        FieldName = "PP.PROD_DIM_D",
                        SearchType = SearchType.Number,
                        SearchText1 = Convert.ToString(gridSearchRequest.PROD_DIM_D_MAX)
                    };
                    searchFields.Add(searchField);
                }
            }
            else if (gridSearchRequest.PROD_DIM_D_MIN > 0 && gridSearchRequest.PROD_DIM_D_MAX > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_DIM_D",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.PROD_DIM_D_MIN),
                    SearchText2 = Convert.ToString(gridSearchRequest.PROD_DIM_D_MAX)

                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.PROD_LENGTH_MAX == null)
            {
                if (gridSearchRequest.PROD_LENGTH_MIN > 0)
                {
                    var searchField = new SearchField
                    {
                        FieldName = "PP.PROD_LENGTH",
                        SearchType = SearchType.Number,
                        SearchText1 = Convert.ToString(gridSearchRequest.PROD_LENGTH_MIN)
                    };
                    searchFields.Add(searchField);
                }
            }
            else if (gridSearchRequest.PROD_LENGTH_MIN == null)
            {
                if (gridSearchRequest.PROD_LENGTH_MAX > 0)
                {
                    var searchField = new SearchField
                    {
                        FieldName = "PP.PROD_LENGTH",
                        SearchType = SearchType.Number,
                        SearchText1 = Convert.ToString(gridSearchRequest.PROD_LENGTH_MAX)
                    };
                    searchFields.Add(searchField);
                }
            }
            else if (gridSearchRequest.PROD_LENGTH_MIN > 0 && gridSearchRequest.PROD_LENGTH_MAX > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_LENGTH",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.PROD_LENGTH_MIN),
                    SearchText2 = Convert.ToString(gridSearchRequest.PROD_LENGTH_MAX)
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.SHAPE))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_SHAPE",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.SHAPE
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.CHARGE))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.CHARGE",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.CHARGE.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            //if (!string.IsNullOrEmpty(gridSearchRequest.CHUUBAN))
            //{
            //    var searchField = new SearchField
            //    {
            //        FieldName = "CONCAT(PP.CHUUBAN,CASE WHEN LENGTH(TRIM(PP.GYOUBAN))=1 THEN '0'||PP.GYOUBAN END)",
            //        SearchType = SearchType.Text,
            //        SearchText1 = gridSearchRequest.CHUUBAN
            //    };
            //    searchFields.Add(searchField);
            //}
            if (!string.IsNullOrEmpty(gridSearchRequest.CHUUBAN))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.CHUUBAN_YEAR||PP.CHUUBAN",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.CHUUBAN
                };
                searchFields.Add(searchField);
            }

            if (gridSearchRequest.GYOUBAN > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.GYOUBAN",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.GYOUBAN)
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_SPEC_NO1))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_SPEC_NO1",
                    SearchType = SearchType.Number,
                    SearchText1 = "'" + gridSearchRequest.PROD_SPEC_NO1 + "'"
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_SPEC_NO2))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_SPEC_NO2",
                    SearchType = SearchType.Number,
                    SearchText1 = "'" + gridSearchRequest.PROD_SPEC_NO2 + "'"
                };
                searchFields.Add(searchField);
            }
            if (string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE2))
            {
                if (!string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE1))
                {
                    var searchField = new SearchField
                    {
                        FieldName = "DW.DW_END_DATE",
                        SearchType = SearchType.DateTime,
                        SearchText1 = gridSearchRequest.INSPECTION_DATE1
                    };
                    searchFields.Add(searchField);
                }
            }
            else if (string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE1))
            {
                if (!string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE2))
                {
                    var searchField = new SearchField
                    {
                        FieldName = "DW.DW_END_DATE",
                        SearchType = SearchType.DateTime,
                        SearchText1 = gridSearchRequest.INSPECTION_DATE2
                    };
                    searchFields.Add(searchField);
                }
            }
            else if (!string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE1) && !string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE2))
            {
                var searchField = new SearchField
                {
                    FieldName = "DW.DW_END_DATE",
                    SearchType = SearchType.DateTime,
                    SearchText1 = gridSearchRequest.INSPECTION_DATE1,
                    SearchText2 = gridSearchRequest.INSPECTION_DATE2
                };
                searchFields.Add(searchField);
            }


            return searchFields;
        }
        public List<HikinukiDetails> GetHikinukiCoilToBarDetailsOnDailyProcessAnalysis(GridSearchFields gridSearchRequest)
        {


            List<SearchField> searchFields = ComposeSearchCriteriaforDailyProcess(gridSearchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);

            var obj = _hikinukiCoilToBarRepository.GetHikinukiCoilToBarDetailsOnDailyProcessAnalysis(searchFields, selectedSortCriteria, gridSearchRequest);
            return obj;
        }
               

        private List<SearchField> ComposeSearchCriteriaforDailyProcess(GridSearchFields gridSearchRequest)
        {
            var searchFields = new List<SearchField>();
            if (!string.IsNullOrEmpty(gridSearchRequest.CHUUBAN))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.CHUUBAN_YEAR||PP.CHUUBAN",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.CHUUBAN
                };
                searchFields.Add(searchField);
            }

            if (gridSearchRequest.GYOUBAN > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.GYOUBAN",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.GYOUBAN)
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_SPEC_NO1))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_SPEC_NO1",
                    SearchType = SearchType.Number,
                    SearchText1 = "'" + gridSearchRequest.PROD_SPEC_NO1 + "'"
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_SPEC_NO2))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_SPEC_NO2",
                    SearchType = SearchType.Number,
                    SearchText1 = "'" + gridSearchRequest.PROD_SPEC_NO2 + "'"
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_VER))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.VER",
                    SearchType = SearchType.Number,
                    SearchText1 = gridSearchRequest.PROD_VER
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.CUSTOMER_NAME))
            {
                var searchField = new SearchField
                {
                    FieldName = "CUST.SHORT_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.CUSTOMER_NAME.Trim()
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_NAME_CUST))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_NAME_CUST",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.PROD_NAME_CUST.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.INTERNAL_NAME_STRING))
            {
                var searchField = new SearchField
                {
                    FieldName = "INTERN.INTERNAL_NAME",
                    SearchType = SearchType.Text,
                    SearchCondition = SearchCondition.Equal.ToString(),
                    SearchText1 = gridSearchRequest.INTERNAL_NAME_STRING.ToLower().Trim()
                   

                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_SHAPE))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_SHAPE",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.PROD_SHAPE
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.PROD_DIM_D_MIN > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_DIM_D",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.PROD_DIM_D_MIN),
                    SearchText2 = Convert.ToString(gridSearchRequest.PROD_DIM_D_MAX)

                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.PROD_DIM_W > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_DIM_W",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.PROD_DIM_W)
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_DIM_OTHER_STRING))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_DIM_OTHER",
                    SearchType = SearchType.Text,
                    SearchText1 = Convert.ToString(gridSearchRequest.PROD_DIM_OTHER_STRING).ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.PROD_LENGTH_MIN > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_LENGTH",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.PROD_LENGTH_MIN),
                    SearchText2 = Convert.ToString(gridSearchRequest.PROD_LENGTH_MAX)
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.LENGTH_OTHER))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.LENGTH_OTHER",
                    SearchType = SearchType.Text,
                    SearchText1 = Convert.ToString(gridSearchRequest.LENGTH_OTHER).ToLower().Trim(),

                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.PROD_CONDITION_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "CONDITION.CONDITION_CD ",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.PROD_CONDITION_CD)
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.MATL_TYPE_CBK > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_TYPE_CBK",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MATL_TYPE_CBK)
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.MATL_NAME_MAK))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_NAME_MAK",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.MATL_NAME_MAK.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.MATL_SHAPE))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_SHAPE",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.MATL_SHAPE
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.MATL_DIM_D_MIN > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_DIM_D",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MATL_DIM_D_MIN),
                    SearchText2 = Convert.ToString(gridSearchRequest.MATL_DIM_D_MAX)

                };
                searchFields.Add(searchField);
            }

            if (gridSearchRequest.MATL_DIM_W > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_DIM_W",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MATL_DIM_W)
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.MATL_DIM_OTHER))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_DIM_OTHER",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.MATL_DIM_OTHER.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.MATL_LENGTH_MIN > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_LENGTH",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MATL_LENGTH_MIN),
                    SearchText2 = Convert.ToString(gridSearchRequest.MATL_LENGTH_MAX)
                };
                searchFields.Add(searchField);
            }

            if (gridSearchRequest.MATL_CONDITION_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "CONDITION1.CONDITION_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MATL_CONDITION_CD)
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.CHARGE))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.CHARGE",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.CHARGE.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.ACT_ALLOC_START_DATE))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.ACT_ALLOC_DATE",
                    SearchType = SearchType.Date,
                    SearchText1 = gridSearchRequest.ACT_ALLOC_START_DATE,
                    SearchText2 = gridSearchRequest.ACT_ALLOC_END_DATE
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.MACHINE_NAME))
            {
                var searchField = new SearchField
                {
                    FieldName = "MAC.MACHINE_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.MACHINE_NAME.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.LOCATION_NAME))
            {
                var searchField = new SearchField
                {
                    FieldName = "LOC.SHORT_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.LOCATION_NAME.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.BEGINNING_START_DATE))
            {
                var searchField = new SearchField
                {
                    FieldName = "DW.DW_START_DATE",
                    SearchType = SearchType.DateTime,
                    SearchText1 = gridSearchRequest.BEGINNING_START_DATE,
                    SearchText2 = gridSearchRequest.BEGINNING_END_DATE
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.START_OPERATOR))
            {
                var searchField = new SearchField
                {
                    FieldName = "EMP1.SHORT_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.START_OPERATOR.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.FURNACE_NO))
            {
                var searchField = new SearchField
                {
                    FieldName = "DW.FURNACE_NO",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.FURNACE_NO.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.METALLIC_PLATE_CODE_STRING))
            {
                var searchField = new SearchField
                {
                    FieldName = "DW.DW_METALLIC_PLATE_CODE",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.METALLIC_PLATE_CODE_STRING.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.DIE_NO))
            {
                var searchField = new SearchField
                {
                    FieldName = "DW.DIE_NO",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.DIE_NO.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.END_DATE1))
            {
                var searchField = new SearchField
                {
                    FieldName = "DW.DW_END_DATE",
                    SearchType = SearchType.DateTime,
                    SearchText1 = gridSearchRequest.END_DATE1,
                    SearchText2 = gridSearchRequest.END_DATE2
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.END_OPERATOR))
            {
                var searchField = new SearchField
                {
                    FieldName = " EMP2.SHORT_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.END_OPERATOR.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.DECISION > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "DW.DW_DECISION",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.DECISION)
                };
                searchFields.Add(searchField);
            }

            return searchFields;
        }
        public List<NameValue> GetTPTestList()
        {
            var _tpTestList = UIUtils.GetTPTest();
            return _tpTestList;
        }
        public List<NameValue> GetDieDiaShapeList()
        {
            var _DieShapeList = UIUtils.GetDieShape();
            return _DieShapeList;
        }
        public List<NameValue> GetUnitTypeList()
        {
            var _UnitTypeList = UIUtils.GetUnitTypetKubun();
            return _UnitTypeList;
        }
        public List<NameValue> GetBendStatusList()
        {
            var _statusList = UIUtils.GetBendStatus();
            return _statusList;
        }
        public List<NameValue> GetExteriorConditionList()
        {
            var _conditionList = UIUtils.GetExteriorConditionKubunGetKubun();
            return _conditionList;
        }
        public List<NameValue> GetDecisionList()
        {
            var _DecisionList = UIUtils.GetResultKubun();
            return _DecisionList;
        }
        public List<NameValue> GetNextProcessList()
        {
            var _nextProcessList = UIUtils.GetNextProcesKubun();
            return _nextProcessList;
        }
        public string GetThabaDetailsByThabaNumber(GridSearchFields request)
        {

            string duplicateThaba = string.Empty;
            var result = _hikinukiCoilToBarRepository.GetThabaDetailsByThabaNumber(Convert.ToInt64(request.PROCESS_PLAN_CD), request.TABA_NO_STRING);
           foreach(HikinukiDetails record in result)
            {
                string decisionName = UIUtils.GetResultKubunValue(Convert.ToString(record.DW_DECISION));
                if (_localization.GetLocalizedHtmlString("lbl_OK") == decisionName)
                {
                    record.DW_DECISION = 1;
                }
                if (_localization.GetLocalizedHtmlString("lbl_NG") == decisionName)
                {
                    record.DW_DECISION = 2;
                }
                if (_localization.GetLocalizedHtmlString("lbl_Hold") == decisionName)
                {
                    record.DW_DECISION = 3;
                }
                if (_localization.GetLocalizedHtmlString("lbl_DecisionKubun4of4") == decisionName)
                {
                    record.DW_DECISION = 4;
                }
            }
            if (result != null)
            {
                HikinukiDetails getData = result.AsEnumerable().Select(x => x).OrderBy(r => r.DW_DRAWING_CD).FirstOrDefault();
                bool isTaba1Correct = true;
                bool isTaba2Correct = true;
                bool isTaba3Correct = true;
                bool isTaba4Correct = true;

                if (result.Count > 1 && request.ISCHECKED == 0)
                {
                    if(request.TABA_NO_1 > 0)
                    {
                        var taba1Data = result.AsEnumerable().Select(x => x).OrderBy(r => r.DW_DRAWING_CD).Where(r=>r.DW_TABA_NO_1 == request.TABA_NO_1 && r.DW_DRAWING_CD == request.DRAWING_CD && r.DW_REDRAWING == 0).FirstOrDefault();
                        if (taba1Data == null)
                        {
                            isTaba1Correct = false;
                        }
                    }
                    if (request.TABA_NO_2 > 0)
                    {
                        var taba2Data = result.AsEnumerable().Select(x => x).OrderBy(r => r.DW_DRAWING_CD).Where(r => r.DW_TABA_NO_2 == request.TABA_NO_2 && r.DW_DRAWING_CD == request.DRAWING_CD && r.DW_REDRAWING == 0).FirstOrDefault();
                        if (taba2Data == null)
                        {
                            isTaba2Correct = false;
                        }
                    }
                    if (request.TABA_NO_3 > 0)
                    {
                        var taba3Data = result.AsEnumerable().Select(x => x).OrderBy(r => r.DW_DRAWING_CD).Where(r => r.DW_TABA_NO_3 == request.TABA_NO_3 && r.DW_DRAWING_CD == request.DRAWING_CD && r.DW_REDRAWING == 0).FirstOrDefault();
                        if (taba3Data == null)
                        {
                            isTaba3Correct = false;
                        }
                    }
                    if (request.TABA_NO_4 > 0)
                    {
                        var taba4Data = result.AsEnumerable().Select(x => x).OrderBy(r => r.DW_DRAWING_CD).Where(r => r.DW_TABA_NO_4 == request.TABA_NO_4 && r.DW_DRAWING_CD == request.DRAWING_CD && r.DW_REDRAWING == 0).FirstOrDefault();
                        if (taba4Data == null)
                        {
                            isTaba4Correct = false;
                        }
                    }

                    //if (getData.DW_DRAWING_CD == request.DRAWING_CD)
                    //{
                    //    duplicateThaba = string.Empty;
                    //}
                    //else
                    //{
                    //List<short> list = new List<short>();
                    //list.Add(Convert.ToInt16(request.TABA_NO_1));
                    //if (request.TABA_NO_2 != null)
                    //{
                    //    list.Add(Convert.ToInt16(request.TABA_NO_2));
                    //}
                    //if (request.TABA_NO_3 != null)
                    //{
                    //    list.Add(Convert.ToInt16(request.TABA_NO_3));
                    //}
                    //if (request.TABA_NO_4 != null)
                    //{
                    //    list.Add(Convert.ToInt16(request.TABA_NO_4));
                    //}

                    if (isTaba1Correct && isTaba2Correct && isTaba3Correct && isTaba4Correct)
                    {
                        duplicateThaba = string.Empty;
                    }
                    else
                    {
                        List<HikinukiDetails> details = new List<HikinukiDetails>();

                        //  details = result.AsEnumerable().Where(x => x.DW_DRAWING_CD != request.DRAWING_CD && list.Contains(Convert.ToInt16(x.DW_TABA_NO_1)) || list.Contains(Convert.ToInt16(x.DW_TABA_NO_2)) || list.Contains(Convert.ToInt16(x.DW_TABA_NO_3)) || list.Contains(Convert.ToInt16(x.DW_TABA_NO_4))).Select(x => x).ToList();
                        details = result.AsEnumerable().Where(x => x.DW_DRAWING_CD != request.DRAWING_CD).Select(x => x).ToList();

                        foreach (var item in details)
                        {
                            if (item.DW_TABA_NO_1 > 0)
                            {
                                if (item.DW_TABA_NO_1 == request.TABA_NO_1 || item.DW_TABA_NO_1 == request.TABA_NO_2 || item.DW_TABA_NO_1 == request.TABA_NO_3 || item.DW_TABA_NO_1 == request.TABA_NO_4)
                                {
                                    if (string.IsNullOrEmpty(duplicateThaba))
                                    {
                                        duplicateThaba = Convert.ToString(item.DW_TABA_NO_1);
                                    }
                                    else
                                    {
                                        duplicateThaba = duplicateThaba + "," + Convert.ToString(item.DW_TABA_NO_1);
                                    }
                                }
                            }
                            if (item.DW_TABA_NO_2 > 0)
                            {
                                if (item.DW_TABA_NO_2 == request.TABA_NO_1 || item.DW_TABA_NO_2 == request.TABA_NO_2 || item.DW_TABA_NO_2 == request.TABA_NO_3 || item.DW_TABA_NO_2 == request.TABA_NO_4)
                                {
                                    if (string.IsNullOrEmpty(duplicateThaba))
                                    {
                                        duplicateThaba = Convert.ToString(item.DW_TABA_NO_2);
                                    }
                                    else
                                    {
                                        duplicateThaba = duplicateThaba + "," + Convert.ToString(item.DW_TABA_NO_2);
                                    }
                                }
                            }
                            if (item.DW_TABA_NO_3 > 0)
                            {
                                if (item.DW_TABA_NO_3 == request.TABA_NO_1 || item.DW_TABA_NO_3 == request.TABA_NO_2 || item.DW_TABA_NO_3 == request.TABA_NO_3 || item.DW_TABA_NO_3 == request.TABA_NO_4)
                                {
                                    if (string.IsNullOrEmpty(duplicateThaba))
                                    {
                                        duplicateThaba = Convert.ToString(item.DW_TABA_NO_3);
                                    }
                                    else
                                    {
                                        duplicateThaba = duplicateThaba + "," + Convert.ToString(item.DW_TABA_NO_3);
                                    }
                                }
                            }
                            if (item.DW_TABA_NO_4 > 0)
                            {
                                if (item.DW_TABA_NO_4 == request.TABA_NO_1 || item.DW_TABA_NO_4 == request.TABA_NO_2 || item.DW_TABA_NO_4 == request.TABA_NO_3 || item.DW_TABA_NO_4 == request.TABA_NO_4)
                                {
                                    if (string.IsNullOrEmpty(duplicateThaba))
                                    {
                                        duplicateThaba = Convert.ToString(item.DW_TABA_NO_4);
                                    }
                                    else
                                    {
                                        duplicateThaba = duplicateThaba + "," + Convert.ToString(item.DW_TABA_NO_4);
                                    }
                                }
                            }
                            //}
                            if (!string.IsNullOrEmpty(duplicateThaba))
                            {
                                string[] value = duplicateThaba.Split(',');
                                var distinctItems = value.Distinct();
                                duplicateThaba = string.Join(", ", distinctItems);
                            }


                        }
                    }

                }
                else if (result.Count == 1 && request.ISCHECKED == 0 && result[0].DW_DRAWING_CD != request.DRAWING_CD)
                {
                    if (result[0].DW_TABA_NO_1 == request.TABA_NO_1 || result[0].DW_TABA_NO_1 == request.TABA_NO_2 || result[0].DW_TABA_NO_1 == request.TABA_NO_3 || result[0].DW_TABA_NO_1 == request.TABA_NO_4)
                    {
                        if (string.IsNullOrEmpty(duplicateThaba))
                        {
                            duplicateThaba = Convert.ToString(result[0].DW_TABA_NO_1);
                        }
                        else
                        {
                            duplicateThaba = duplicateThaba + "," + Convert.ToString(result[0].DW_TABA_NO_1);
                        }
                    }
                    if (Convert.ToInt16(result[0].DW_TABA_NO_2) > 0)
                    {
                        if (result[0].DW_TABA_NO_2 == request.TABA_NO_1 || result[0].DW_TABA_NO_2 == request.TABA_NO_2 || result[0].DW_TABA_NO_2 == request.TABA_NO_3 || result[0].DW_TABA_NO_2 == request.TABA_NO_4)
                        {
                            if (string.IsNullOrEmpty(duplicateThaba))
                            {
                                duplicateThaba = Convert.ToString(result[0].DW_TABA_NO_2);
                            }
                            else
                            {
                                duplicateThaba = duplicateThaba + "," + Convert.ToString(result[0].DW_TABA_NO_2);
                            }
                        }
                    }
                    if (Convert.ToInt16(result[0].DW_TABA_NO_3) > 0)
                    {
                        if (Convert.ToInt16(result[0].DW_TABA_NO_3) > 0 && result[0].DW_TABA_NO_3 == request.TABA_NO_1 || result[0].DW_TABA_NO_3 == request.TABA_NO_2 || result[0].DW_TABA_NO_3 == request.TABA_NO_3 || result[0].DW_TABA_NO_3 == request.TABA_NO_4)
                        {
                            if (string.IsNullOrEmpty(duplicateThaba))
                            {
                                duplicateThaba = Convert.ToString(result[0].DW_TABA_NO_3);
                            }
                            else
                            {
                                duplicateThaba = duplicateThaba + "," + Convert.ToString(result[0].DW_TABA_NO_3);
                            }
                        }
                    }
                    if (Convert.ToInt16(result[0].DW_TABA_NO_4) > 0)
                    {
                        if (Convert.ToInt16(result[0].DW_TABA_NO_4) > 0 && result[0].DW_TABA_NO_4 == request.TABA_NO_1 || result[0].DW_TABA_NO_4 == request.TABA_NO_2 || result[0].DW_TABA_NO_4 == request.TABA_NO_3 || result[0].DW_TABA_NO_4 == request.TABA_NO_4)
                        {
                            if (string.IsNullOrEmpty(duplicateThaba))
                            {
                                duplicateThaba = Convert.ToString(result[0].DW_TABA_NO_4);
                            }
                            else
                            {
                                duplicateThaba = duplicateThaba + "," + Convert.ToString(result[0].DW_TABA_NO_4);
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(duplicateThaba))
                    {
                        string[] value = duplicateThaba.Split(',');
                        var distinctItems = value.Distinct();
                        duplicateThaba = string.Join(", ", distinctItems);
                    }
                    
                }
                else
                {
                    duplicateThaba = string.Empty;
                }


            }
            else
            {
                duplicateThaba = string.Empty;
            }
            return duplicateThaba;
        }

        public int GetRowCountForCombinedDailyReportFactory(GridSearchFields gridSearchRequest)
        {
            List<SearchField> searchFields = ComposeSearchCriteriaforFactoryDailyWorkStatus(gridSearchRequest);
            int count = _hikinukiCoilToBarRepository.GetRowCountForCombinedDailyReportFactory(searchFields);
            return count;
        }

        public int GetRowCountForHikinukiCoilToBarDailyAnalysis(GridSearchFields gridSearchRequest)
        {
            List<SearchField> searchFields = ComposeSearchCriteriaforDailyProcess(gridSearchRequest);
            int count = _hikinukiCoilToBarRepository.GetRowCountForHikinukiCoilToBarDailyAnalysis(searchFields);
            return count;
        }
        public int GetHikinukiDataForOracleToHost(string startDate, string endDate,int userID, string type)
        {
           short locationCD = Convert.ToInt16(_commonRepository.GetUserLocationById(userID));

            return _hikinukiCoilToBarRepository.GetHikinukiList(startDate, endDate, locationCD, type);
        }
        public int GetBatchStatusCode()
        {
            return _hikinukiCoilToBarRepository.GetStatusCode();
        }
        public List<NameValue> GetHikinukiCoilToBarMachineTypeList()
        {
            var machineNameList = _hikinukiCoilToBarRepository.GetHikinukiCoilToBarMachineTypeList();
            return machineNameList;
        }
    }
}
