using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Transactions;
using TSWebApplication.Application.IServices;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Infrastructure.IRepositories;
using TSWebApplication.Infrastructure.Repositories;

namespace TSWebApplication.Application.Services
{
    public class Straightening2Service : IStraightening2Service
    {
        private readonly IStraightening2Repository _straighteningRepository;
        private readonly IAllCommonRepository _allCommonRepository;
        private readonly IUserLoginRepository _userLoginRepository;
        private readonly LocalizationServices _localization;
        private readonly IAllCommonService _allCommonService;

        public Straightening2Service(IStraightening2Repository straighteningRepository, IAllCommonRepository allCommonRepository, IUserLoginRepository userLoginRepository, LocalizationServices localization, IAllCommonService allCommonService)
        {
            _straighteningRepository = straighteningRepository;
            _allCommonRepository = allCommonRepository;
            _userLoginRepository = userLoginRepository;
            _localization = localization;
            _allCommonService = allCommonService;
        }

        public List<Straightening2Details> GetStraightening2List(GridSearchFields gridSearchRequest)
        {
            short input_Location_CD = Convert.ToInt16(_userLoginRepository.GetUserLocationById(Convert.ToInt32(gridSearchRequest.USER_ID)));
            gridSearchRequest.INPUT_LOCATION_CD = input_Location_CD;

            List<Straightening2Details> result = new List<Straightening2Details>();
            List<SearchField> searchFields = ComposeSearchCriteria(gridSearchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);
            result = _straighteningRepository.GetStraightening2List(searchFields, selectedSortCriteria, gridSearchRequest.INPUT_LOCATION_CD);            
            return result;
        }

        public void Delete(Straightening2Details request)
        {
            WipStatusDetails wipDetails = new WipStatusDetails();
            wipDetails = new WipStatusDetails();
            wipDetails.PROCESS_PLAN_CD = request.PP_PROCESS_PLAN_CD;
            using (TransactionScope transaction = new TransactionScope())
            {
                if (request.STRAIGHTENING_CD > 0)
                {
                    _straighteningRepository.Delete(Convert.ToInt64(request.STRAIGHTENING_CD));
                }
               
                if (request.STR_TABA_NO_1 > 0)
                {
                    wipDetails.TABA_NO = request.STR_TABA_NO_1;
                    _allCommonRepository.DeleteWIPStatusData(wipDetails);
                    //_allCommonRepository.DeleteWIPStatusData(Convert.ToInt64(request.PP_PROCESS_PLAN_CD), Convert.ToInt16(request.STR_TABA_NO_1));
                }
                if (request.STR_TABA_NO_2 > 0)
                {
                    wipDetails.TABA_NO = request.STR_TABA_NO_2;
                    _allCommonRepository.DeleteWIPStatusData(wipDetails);
                    //_allCommonRepository.DeleteWIPStatusData(Convert.ToInt64(request.PP_PROCESS_PLAN_CD), Convert.ToInt16(request.STR_TABA_NO_2));
                }
                if (request.STR_TABA_NO_3 > 0)
                {
                    wipDetails.TABA_NO = request.STR_TABA_NO_3;
                    _allCommonRepository.DeleteWIPStatusData(wipDetails);
                    //_allCommonRepository.DeleteWIPStatusData(Convert.ToInt64(request.PP_PROCESS_PLAN_CD), Convert.ToInt16(request.STR_TABA_NO_3));
                }
                if (request.STR_TABA_NO_4 > 0)
                {
                    wipDetails.TABA_NO = request.STR_TABA_NO_4;
                    _allCommonRepository.DeleteWIPStatusData(wipDetails);
                    //_allCommonRepository.DeleteWIPStatusData(Convert.ToInt64(request.PP_PROCESS_PLAN_CD), Convert.ToInt16(request.STR_TABA_NO_4));
                }
                if (request.MSG_CD > 0)
                {
                    _allCommonRepository.MessageDetailsDelete(Convert.ToInt64(request.MSG_CD));
                }
                transaction.Complete();
            }
        }

        public Straightening2Details GetStraightening2DetailsByCD(long straighteningCD, string cultureName)
        {
            var _straighteningDetails = _straighteningRepository.GetStraightening2DetailsByCD(straighteningCD);
            if (_straighteningDetails != null)
            {
                if (cultureName == "en")
                {
                    System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
                }
                else
                {
                    System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("ja-JP");
                }
                if (_straighteningDetails.PP_STD_DEL_PERIOD >= 0)
                {
                    _straighteningDetails.PP_STD_DEL_PERIOD_STRING = _straighteningDetails.PP_STD_DEL_PERIOD + " " + _localization.GetLocalizedHtmlString("lbl_Day");
                }

                if (_straighteningDetails.PP_UNIT > 0)
                {
                    _straighteningDetails.PP_NO_OF_UNITS_DECODE = Convert.ToDecimal(_straighteningDetails.PP_NO_OF_UNITS).ToString("0.####") + UIUtils.GetProductUnitKubunValue(Convert.ToString(_straighteningDetails.PP_UNIT));
                }
                if (_straighteningDetails.MAGARI_KUBUN > 0)
                {
                    _straighteningDetails.PP_MAGARI_BEND_TOL = Utils.GetMagariBendTolSpan(UIUtils.Instance.GetMagariKubunValue(Convert.ToString(_straighteningDetails.MAGARI_KUBUN)), Convert.ToDecimal(_straighteningDetails.MAGARI_BEND_TOL), Convert.ToInt32(_straighteningDetails.MAGARI_SPAN));  //Convert.ToDecimal(_stamppingDetails.PP_NO_OF_UNITS).ToString("0.####") + UIUtils.Instance.GetProductUnitKubunValue(Convert.ToString(_stamppingDetails.PP_UNIT));
                }

                //if (_straighteningDetails.PP_COLOR_ID > 0)
                //{
                //    _straighteningDetails.PP_COLOR = UIUtils.GetColorKubunValue(Convert.ToString(_straighteningDetails.PP_COLOR_ID));   // Utils.GetColorName(Convert.ToInt16(_stamppingDetails.PP_COLOR_ID));

                //}
                if (_straighteningDetails.PP_COIL_DIA_WINDING != null)
                {
                    _straighteningDetails.PP_COIL_DIA_WINDING = UIUtils.GetCoilDiaWindingKubunValue(Convert.ToString(_straighteningDetails.PP_COIL_DIA_WINDING)); //  Utils.GetCoilDiaWindingValue(Convert.ToInt16(_stamppingDetails.PP_COIL_DIA_WINDING));
                }
                if (_straighteningDetails.PP_UNIT_WT_KUBUN > 0)
                {
                    _straighteningDetails.PP_UNIT_WT_KUBUN_STRING = Utils.GetUnitWeigth(Convert.ToInt32(_straighteningDetails.PP_UNIT_WT_KUBUN), Convert.ToInt32(_straighteningDetails.PP_UNIT_WT), Convert.ToInt32(_straighteningDetails.PP_UNIT_WT_MIN), Convert.ToInt32(_straighteningDetails.PP_UNIT_WT_MAX));

                    if (_straighteningDetails.PP_UNIT_WT_KUBUN == 3)
                    {
                        _straighteningDetails.PP_UNIT_WT_KUBUN_STRING = Convert.ToString(_straighteningDetails.PP_UNIT_WT) + _localization.GetLocalizedHtmlString("lbl_Hone");
                    }


                }
                return _straighteningDetails;
            }
            else
            {
                return new Straightening2Details();
            }
        }
        public List<NameValue> GetMachineNameList()
        {
            var machineNameList = _straighteningRepository.GetMachineNameList();
            return machineNameList;
        }
        public List<ThabaDetails> GetThabaDetailsByChuuban(string chubaan, short? gyouban)
        {
            var result = _straighteningRepository.GetThabaDetails(chubaan, gyouban);
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
                if (_localization.GetLocalizedHtmlString("lbl_ReHeatTreatment") == decisionName)
                {
                    record.DECISION_VALUE = 5;
                }
                if (_localization.GetLocalizedHtmlString("lbl_ReDrawing") == decisionName)
                {
                    record.DECISION_VALUE = 5;
                }
                if (_localization.GetLocalizedHtmlString("lbl_Restraightening") == decisionName)
                {
                    record.DECISION_VALUE = 5;
                }
                if (_localization.GetLocalizedHtmlString("lbl_Re_Chamfering") == decisionName)
                {
                    record.DECISION_VALUE = 5;
                }
                if (_localization.GetLocalizedHtmlString("lbl_ReEc") == decisionName)
                {
                    record.DECISION_VALUE = 5;
                }

            }
            return result;
        }

        public List<Straightening2Details> GetMachine24Details(string machineCd, string cultureName)
        {
            var obj = _straighteningRepository.GetStraightening24MachineDetails(machineCd);
            if (cultureName == "en")
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
            }
            else
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("ja-JP");
            }
            foreach (Straightening2Details record in obj)
            {
                string decisionName = UIUtils.GetResultKubunValue(Convert.ToString(record.STR_DECISION));
                if (_localization.GetLocalizedHtmlString("lbl_OK") == decisionName)
                {
                    record.STR_DECISION = 1;
                }
                if (_localization.GetLocalizedHtmlString("lbl_NG") == decisionName)
                {
                    record.STR_DECISION = 2;
                }
                if (_localization.GetLocalizedHtmlString("lbl_Hold") == decisionName)
                {
                    record.STR_DECISION = 3;
                }
                if (_localization.GetLocalizedHtmlString("lbl_DecisionKubun4of4") == decisionName)
                {
                    record.STR_DECISION = 4;
                }
                if (_localization.GetLocalizedHtmlString("lbl_ReHeatTreatment") == decisionName)
                {
                    record.STR_DECISION = 5;
                }
                if (_localization.GetLocalizedHtmlString("lbl_ReDrawing") == decisionName)
                {
                    record.STR_DECISION = 5;
                }
                if (_localization.GetLocalizedHtmlString("lbl_Restraightening") == decisionName)
                {
                    record.STR_DECISION = 5;
                }
                if (_localization.GetLocalizedHtmlString("lbl_Re_Chamfering") == decisionName)
                {
                    record.STR_DECISION = 5;
                }
                if (_localization.GetLocalizedHtmlString("lbl_ReEc") == decisionName)
                {
                    record.STR_DECISION = 5;
                }

                record.PP_NO_OF_UNITS_DECODE = record.PP_NO_OF_UNITS != null ? record.PP_NO_OF_UNITS + UIUtils.GetProductUnitKubunValue(Convert.ToString(record.PP_UNIT)) : string.Empty;               
                record.STR_EXT_TEXTURE_STATUS = UIUtils.GetExteriorCondition(Convert.ToString(record.STR_EXT_TEXTURE));
                record.STR_BEND_STATUS_VAL = UIUtils.GetBendStateKubunValue(Convert.ToString(record.STR_BEND_STATUS));
                record.STR_BARS_DECODE = record.STR_BARS != null ? record.STR_BARS + UIUtils.GetProductUnitKubunValue(Convert.ToString(record.STR_UNIT_TYPE)) : string.Empty;


                
            }
            return obj;
        }

        public Straightening2Details GetStraighteningDetailsByChuuban(string chuuban, short? gyouban, int? machineCD,string cultureName)
        {
            var obj = _straighteningRepository.GetStraighteningDetailsByChuubanGyoubanMachine(chuuban, Convert.ToInt16(gyouban), machineCD);
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
                if (obj.PP_STD_DEL_PERIOD >= 0)
                {
                    obj.PP_STD_DEL_PERIOD_STRING = obj.PP_STD_DEL_PERIOD + " " + _localization.GetLocalizedHtmlString("lbl_Day");
                }

                if (obj.PP_UNIT != null)
                {
                    obj.PP_NO_OF_UNITS_DECODE = Convert.ToDecimal(obj.PP_NO_OF_UNITS).ToString("0.####") + UIUtils.GetProductUnitKubunValue(Convert.ToString(obj.PP_UNIT));
                }

                if (obj.MAGARI_KUBUN != null)
                {
                    obj.PP_MAGARI_BEND_TOL = Utils.GetMagariBendTolSpan(UIUtils.Instance.GetMagariKubunValue(Convert.ToString(obj.MAGARI_KUBUN)), Convert.ToDecimal(obj.MAGARI_BEND_TOL), Convert.ToInt32(obj.MAGARI_SPAN));  //Convert.ToDecimal(obj.PP_NO_OF_UNITS).ToString("0.####") + UIUtils.Instance.GetProductUnitKubunValue(Convert.ToString(obj.PP_UNIT));
                }
                //if (obj.PP_COLOR_ID > 0)
                //{
                //    obj.PP_COLOR = UIUtils.GetColorKubunValue(Convert.ToString(obj.PP_COLOR_ID));  //Utils.GetColorName(Convert.ToInt16(obj.PP_COLOR_ID));
                //}
                if (obj.COIL_DIA_WINDING != null)
                {
                    obj.PP_COIL_DIA_WINDING = UIUtils.GetCoilDiaWindingKubunValue(Convert.ToString(obj.COIL_DIA_WINDING)); //  Utils.GetCoilDiaWindingValue(Convert.ToInt16(_stamppingDetails.PP_COIL_DIA_WINDING));
                }
                if (obj.PP_UNIT_WT_KUBUN > 0)
                {
                    obj.PP_UNIT_WT_KUBUN_STRING = Utils.GetUnitWeigth(Convert.ToInt32(obj.PP_UNIT_WT_KUBUN), Convert.ToInt32(obj.PP_UNIT_WT), Convert.ToInt32(obj.PP_UNIT_WT_MIN), Convert.ToInt32(obj.PP_UNIT_WT_MAX));

                    if (obj.PP_UNIT_WT_KUBUN == 3)
                    {
                        obj.PP_UNIT_WT_KUBUN_STRING = Convert.ToString(obj.PP_UNIT_WT) + _localization.GetLocalizedHtmlString("lbl_Hone");
                    }


                }

                var messageList = _allCommonRepository.GetMessageDetailsByProcessPlanCD(Convert.ToInt64(obj.PP_PROCESS_PLAN_CD), true);
                obj.DW_MESSAGE_LIST = messageList;
                obj.DECISION_OK = Convert.ToInt16(UIUtils.GetResultKubunKey(_localization.GetLocalizedHtmlString("lbl_OK")));
                obj.PP_ELONGATION = Helper.GetProductPatternFormat(obj.ELONGATION_PATTERN1.HasValue ? obj.ELONGATION_PATTERN1.Value : 0, obj.ELONGATION_PATTERN1_MIN_MAX, obj.ELONGATION_PATTERN2_MIN, obj.ELONGATION_PATTERN2_MAX, obj.ELONGATION_PATTERN3_ACTUAL);
                obj.PP_TENSILE_STRENGTH = Helper.GetProductPatternFormat(Convert.ToSingle(obj.TENSILE_STRN_PATTERN1), obj.TENSILE_STRN_PATTERN1_MIN_MAX, obj.TENSILE_STRN_PATTERN2_MIN, obj.TENSILE_STRN_PATTERN2_MAX, obj.TENSILE_STRN_PATTERN3_ACTUAL);
                obj.PP_HARDNESS = Helper.GetHardnessProductPatternFormat(obj.HARDNESS_PATTERN1_CODE, obj.HARDNESS_PATTERN1, obj.HARDNESS_PATTERN1_MIN_MAX, obj.HARDNESS_PATTERN2_CODE, obj.HARDNESS_PATTERN2_MIN, obj.HARDNESS_PATTERN2_MAX, obj.HARDNESS_PATTERN3_ACTUAL);
                obj.PP_DECARB = Helper.GetDecarbProductPatternFormat(obj.DECARB_1,Convert.ToSingle(obj.DECARB_VAL_1), obj.DECARB_2, Convert.ToSingle(obj.DECARB_VAL_2), obj.DECARB_3, Convert.ToSingle(obj.DECARB_VAL_3));
                obj.PP_YEILD = Helper.GetProductPatternFormat(Convert.ToSingle(obj.YEILD_PATTERN1), obj.YEILD_PATTERN1_MIN_MAX, obj.YEILD_PATTERN2_MIN, obj.YEILD_PATTERN2_MAX, obj.YEILD_PATTERN3_ACTUAL);
                obj.PP_PRESS = Helper.GetProductPatternFormat(Convert.ToSingle(obj.PRESS_PATTERN1), obj.PRESS_PATTERN1_MIN_MAX, obj.PRESS_PATTERN2_MIN, obj.PRESS_PATTERN2_MAX, obj.PRESS_PATTERN3_ACTUAL);
            }
            return obj;
        }

        public Straightening2Details InsertStraightening2Details(Straightening2Details request)
        {
            Straightening2Details result = new Straightening2Details();
            if (request.USER_ID != null && request.USER_ID > 0)
            {
                request.INPUT_LOCATION_CD = _straighteningRepository.GetUserLocationById(request.USER_ID.Value);
               // request.LOCATION_CD = request.INPUT_LOCATION_CD;
            }

            if (request.MACHINE_CD != null && request.MACHINE_CD > 0)
            {
                request.LOCATION_CD = Convert.ToInt32(_allCommonRepository.GetMachineNameByID(request.MACHINE_CD.Value)?.LOCATION_CD);
            }

            request.PROCESS_KUBUN_CD = Convert.ToInt32(UIConstants.K_PROCESS_PRESS_STRAIGHTENING);
            using (TransactionScope transaction = new TransactionScope())
            {
                WipStatusDetails wipDetails = new WipStatusDetails();
                wipDetails = new WipStatusDetails();
                wipDetails.PROCESS_PLAN_CD = Convert.ToInt32(request.PROCESS_PLAN_CD);
                wipDetails.LOCATION_CD = Convert.ToInt16(request.LOCATION_CD);
                wipDetails.MSG_CD = null;
                wipDetails.FINISHED_BARS = null;
                wipDetails.UNIT = null;
                wipDetails.FINISHED_QTY = null;
                wipDetails.PROCESS_START_DATE = Convert.ToDateTime(request.STR_START_DATE);
                wipDetails.PROCESS_END_DATE = null;
                wipDetails.PROCESS_KUBUN_CD = Convert.ToInt32(request.PROCESS_KUBUN_CD);
                wipDetails.PRE_PROSESS_DECISION = null;
                wipDetails.NEXT_PROCESS = null;
                wipDetails.START_OPERATOR_CD = request.STR_START_OPERATOR_CD;
                wipDetails.END_OPERATOR_CD = null;
                wipDetails.TABA_NO = 0;
                if (request.STR_TABA_NO_1 != null)
                {
                    wipDetails.TABA_NO = Convert.ToInt16(request.STR_TABA_NO_1);
                }
                else
                {
                    wipDetails.TABA_NO = 0;
                }

                wipDetails.NIFUDA_CD = null;
                if (request.STR_TABA_NO_1 != null && request.STR_TABA_NO_2 != null || request.STR_TABA_NO_3 != null || request.STR_TABA_NO_4 != null)
                {
                    wipDetails.TABA_GROUP_NO = request.STR_TABA_NO_1;
                }

                _allCommonRepository.DeleteWIPStatusData(wipDetails);

                if (request.STR_TABA_NO_2 > 0)
                {
                    wipDetails.TABA_NO = request.STR_TABA_NO_2;
                    _allCommonRepository.DeleteWIPStatusData(wipDetails);
                }
                if (request.STR_TABA_NO_3 > 0)
                {
                    wipDetails.TABA_NO = request.STR_TABA_NO_3;
                    _allCommonRepository.DeleteWIPStatusData(wipDetails);
                }
                if (request.STR_TABA_NO_4 > 0)
                {
                    wipDetails.TABA_NO = request.STR_TABA_NO_4;
                    _allCommonRepository.DeleteWIPStatusData(wipDetails);
                }

                 //_allCommonRepository.InsertWIPStatusData(wipDetails);

                _straighteningRepository.InsertStraightening2Details(request);
                
                result = GetStraighteningDetailsByChuuban(request.PP_CHUUBAN, request.PP_GYOUBAN, request.MACHINE_CD, request.CULTURE_NAME);

               
                transaction.Complete();
            }
            return result;

        }

        public long Straightening2DetailsUpdate(Straightening2Details request)
        {
            long msgCd = 0;
            bool deleteMsg = false;
            if (request.STR_NG_DETAILS1 == 0)
            {
                request.STR_NG_DETAILS1 = null;
            }
            if (request.STR_NG_DETAILS2 == 0)
            {
                request.STR_NG_DETAILS2 = null;
            }
            if (request.STR_NG_DETAILS3 == 0)
            {
                request.STR_NG_DETAILS3 = null;
            }


            if (request.USER_ID != null && request.USER_ID > 0)
            {
                request.INPUT_LOCATION_CD = _straighteningRepository.GetUserLocationById(request.USER_ID.Value);
              //  request.LOCATION_CD = request.INPUT_LOCATION_CD;
            }

            if (request.MACHINE_CD != null && request.MACHINE_CD > 0)
            {
                request.LOCATION_CD = Convert.ToInt32(_allCommonRepository.GetMachineNameByID(Convert.ToInt32(request.MACHINE_CD))?.LOCATION_CD);
            }
            //_straighteningRepository.UpdateStraightening2Details(request);
            //request.PP_PROCESS_PLAN_CD = request.STR_PROCESS_PLAN_CD;
            using (TransactionScope transaction = new TransactionScope())
            {
                MessageDetails messageDetails = new MessageDetails();
              
                    messageDetails.PROCESS_PLAN_CD = Convert.ToInt64(request.PROCESS_PLAN_CD);
                    messageDetails.MACHINE_CD = Convert.ToInt32(request.MACHINE_CD);
                   // messageDetails.PROCESS_KUBUN_ID = Convert.ToInt32( UIUtils.GetProcessKubun(_localization.GetLocalizedHtmlString("lbl_ProcessMultiROLLStrighening")));
                messageDetails.PROCESS_KUBUN_ID = Convert.ToInt32(UIConstants.K_PROCESS_PRESS_STRAIGHTENING);
                messageDetails.OPERATOR_CD = Convert.ToInt64(request.STR_END_OPERATOR_CD);
                    messageDetails.MSG_CONTENTS = request.STR_REMARKS;
                    messageDetails.USER_ID = Convert.ToInt32(request.STR_USER_ID);
                    messageDetails.LOCATION_CD = Convert.ToInt32(request.LOCATION_CD);
                    messageDetails.INPUT_LOCATION_CD = Convert.ToInt32(request.INPUT_LOCATION_CD);
                    messageDetails.MSG_CD = Convert.ToInt32(request.MSG_CD);

                if (request.MSG_CD == Constants.NullValue.INT && !string.IsNullOrEmpty(request.STR_REMARKS))
                {
                    messageDetails = _allCommonRepository.MessageDetailsInsert(messageDetails);
                    if (messageDetails.MSG_CD != Constants.NullValue.LONG && messageDetails.MSG_CD > 0)
                    {
                        request.MSG_CD = messageDetails.MSG_CD;
                    }

                    
                }
                // If REMARKS was already present, and it is updated,  
                // then UPDATE the TRN_MESSAGE
                else if (request.MSG_CD > 0 && !string.IsNullOrEmpty(request.STR_REMARKS) )
                {
                   _allCommonRepository.MessageDetailsUpdate(messageDetails);

                   
                }

                // If REMARKS was already present, but it is delete now,
                // then DELETE the TRN_MESSAGE
                else if (request.MSG_CD > 0 && string.IsNullOrEmpty(request.STR_REMARKS))
                {
                    long messageCD = Convert.ToInt64(request.MSG_CD);
                    request.MSG_CD = Constants.NullValue.LONG;
                    
                    msgCd = _straighteningRepository.UpdateStraightening2Details(request);
                    _allCommonRepository.MessageDetailsDelete(messageCD);
                    deleteMsg = true;
                }
                if (deleteMsg == false)
                {
                    msgCd = _straighteningRepository.UpdateStraightening2Details(request);
                }
                if (request.STR_NEXT_PROCESS != 999)
                {
                    WipStatusDetails wipDetails = new WipStatusDetails();

                    wipDetails = new WipStatusDetails();
                    wipDetails.PROCESS_PLAN_CD = Convert.ToInt64(request.PROCESS_PLAN_CD);
                    wipDetails.LOCATION_CD = Convert.ToInt16(request.LOCATION_CD);
                    wipDetails.MSG_CD = request.MSG_CD;
                    wipDetails.FINISHED_BARS = Convert.ToSingle(request.STR_BARS);
                    wipDetails.UNIT = request.UNIT_TYPE;
                    wipDetails.FINISHED_QTY = request.STR_QTY;
                    wipDetails.PROCESS_START_DATE = request.STR_START_DATE;
                    wipDetails.PROCESS_END_DATE = request.STR_END_DATE;
                    wipDetails.PROCESS_KUBUN_CD = Convert.ToInt32(UIConstants.K_PROCESS_PRESS_STRAIGHTENING);
                    wipDetails.PRE_PROSESS_DECISION = request.STR_DECISION;
                    wipDetails.NEXT_PROCESS = request.STR_NEXT_PROCESS;
                    wipDetails.START_OPERATOR_CD = request.STR_START_OPERATOR_CD;
                    wipDetails.END_OPERATOR_CD = request.STR_END_OPERATOR_CD;
                    //wipDetails.TABA_NO = request.STR_TABA_NO_1;
                    wipDetails.NIFUDA_CD = null;
                    if (request.STR_TABA_NO_1 != null && request.STR_TABA_NO_2 != null || request.STR_TABA_NO_3 != null || request.STR_TABA_NO_4 != null)
                    {
                        wipDetails.TABA_GROUP_NO = request.STR_TABA_NO_1;
                    }
                    if (request.STR_TABA_NO_1 > 0)
                    {

                        wipDetails.TABA_NO = Convert.ToInt16(request.STR_TABA_NO_1);
                        _allCommonRepository.DeleteWIPStatusData(wipDetails);
                    }                   

                    if (request.STR_TABA_NO_2 > 0)
                    {

                        wipDetails.FINISHED_QTY = null;
                        wipDetails.FINISHED_BARS = null;
                        wipDetails.UNIT = null;
                        wipDetails.TABA_NO = request.STR_TABA_NO_2;
                        _allCommonRepository.DeleteWIPStatusData(wipDetails);
                    }
                    if (request.STR_TABA_NO_3 > 0)
                    {

                        wipDetails.FINISHED_QTY = null;
                        wipDetails.FINISHED_BARS = null;
                        wipDetails.UNIT = null;
                        wipDetails.TABA_NO = request.STR_TABA_NO_3;
                        _allCommonRepository.DeleteWIPStatusData(wipDetails);
                    }
                    if (request.STR_TABA_NO_4 > 0)
                    {

                        wipDetails.FINISHED_QTY = null;
                        wipDetails.FINISHED_BARS = null;
                        wipDetails.UNIT = null;
                        wipDetails.TABA_NO = request.STR_TABA_NO_4;
                        _allCommonRepository.DeleteWIPStatusData(wipDetails);
                    }
                }
                transaction.Complete();
            }
            return msgCd;

        }

        public List<ThabaDetails> GetThabaDetailsWithOKDecision(string chuuban, short? gyuban, long? straighteningCD)
        {
            return _straighteningRepository.GetThabaDetailsWithOKDecision(chuuban, gyuban, straighteningCD);
        }
        //public List<Straightening2Details> GetThabaDetailsByThabaNumber(string chuuban, short? gyouban, string THABA_NO_STRING)
        //{
        //    var result = _straighteningRepository.GetThabaDetailsByThabaNumber(chuuban, gyouban, THABA_NO_STRING);
        //    return result;
        //}
        public string GetThabaDetailsByThabaNumber(GridSearchFields request)
        {

            string duplicateThaba = string.Empty;
            var result = _straighteningRepository.GetThabaDetailsByThabaNumber(Convert.ToInt64(request.PROCESS_PLAN_CD), request.TABA_NO_STRING);
            foreach (Straightening2Details record in result)
            {
                string decisionName = UIUtils.GetResultKubunValue(Convert.ToString(record.STR_DECISION));
                if (_localization.GetLocalizedHtmlString("lbl_OK") == decisionName)
                {
                    record.STR_DECISION = 1;
                }
                if (_localization.GetLocalizedHtmlString("lbl_NG") == decisionName)
                {
                    record.STR_DECISION = 2;
                }
                if (_localization.GetLocalizedHtmlString("lbl_Hold") == decisionName)
                {
                    record.STR_DECISION = 3;
                }
                if (_localization.GetLocalizedHtmlString("lbl_DecisionKubun4of4") == decisionName)
                {
                    record.STR_DECISION = 4;
                }
            }
            if (result != null)
            {
                Straightening2Details getData = result.AsEnumerable().Select(x => x).OrderBy(r => r.STRAIGHTENING_CD).FirstOrDefault();
                bool isTaba1Correct = true;
                bool isTaba2Correct = true;
                bool isTaba3Correct = true;
                bool isTaba4Correct = true;

                if (result.Count > 1 && request.ISCHECKED == 0)
                {
                    if (request.TABA_NO_1 > 0)
                    {
                        var taba1Data = result.AsEnumerable().Select(x => x).OrderBy(r => r.STRAIGHTENING_CD).Where(r => r.STR_TABA_NO_1 == request.TABA_NO_1 && r.STRAIGHTENING_CD == request.STRAIGHTENING_CD && r.RESTRAIGHTING == 0).FirstOrDefault();
                        if (taba1Data == null)
                        {
                            isTaba1Correct = false;
                        }
                    }
                    if (request.TABA_NO_2 > 0)
                    {
                        var taba2Data = result.AsEnumerable().Select(x => x).OrderBy(r => r.STRAIGHTENING_CD).Where(r => r.STR_TABA_NO_2 == request.TABA_NO_2 && r.STRAIGHTENING_CD == request.STRAIGHTENING_CD && r.RESTRAIGHTING == 0).FirstOrDefault();
                        if (taba2Data == null)
                        {
                            isTaba2Correct = false;
                        }
                    }
                    if (request.TABA_NO_3 > 0)
                    {
                        var taba3Data = result.AsEnumerable().Select(x => x).OrderBy(r => r.STRAIGHTENING_CD).Where(r => r.STR_TABA_NO_3 == request.TABA_NO_3 && r.STRAIGHTENING_CD == request.STRAIGHTENING_CD && r.RESTRAIGHTING == 0).FirstOrDefault();
                        if (taba3Data == null)
                        {
                            isTaba3Correct = false;
                        }
                    }
                    if (request.TABA_NO_4 > 0)
                    {
                        var taba4Data = result.AsEnumerable().Select(x => x).OrderBy(r => r.STRAIGHTENING_CD).Where(r => r.STR_TABA_NO_4 == request.TABA_NO_4 && r.STRAIGHTENING_CD == request.STRAIGHTENING_CD && r.RESTRAIGHTING == 0).FirstOrDefault();
                        if (taba4Data == null)
                        {
                            isTaba4Correct = false;
                        }
                    }
                    //if (getData.STRAIGHTENING_CD == request.STRAIGHTENING_CD)
                    //{
                    //    duplicateThaba = string.Empty;
                    //}
                    //else
                    //{
                    //    List<short> list = new List<short>();
                    //    list.Add(Convert.ToInt16(request.TABA_NO_1));
                    //    if (request.TABA_NO_2 != null)
                    //    {
                    //        list.Add(Convert.ToInt16(request.TABA_NO_2));
                    //    }
                    //    if (request.TABA_NO_3 != null)
                    //    {
                    //        list.Add(Convert.ToInt16(request.TABA_NO_3));
                    //    }
                    //    if (request.TABA_NO_4 != null)
                    //    {
                    //        list.Add(Convert.ToInt16(request.TABA_NO_4));
                    //    }
                    if (isTaba1Correct && isTaba2Correct && isTaba3Correct && isTaba4Correct)
                    {
                        duplicateThaba = string.Empty;
                    }
                    else
                    {

                        List<Straightening2Details> details = new List<Straightening2Details>();

                      //  details = result.AsEnumerable().Where(x => x.STRAIGHTENING_CD != request.STRAIGHTENING_CD && list.Contains(Convert.ToInt16(x.STR_TABA_NO_1)) || list.Contains(Convert.ToInt16(x.STR_TABA_NO_2)) || list.Contains(Convert.ToInt16(x.STR_TABA_NO_3)) || list.Contains(Convert.ToInt16(x.STR_TABA_NO_4))).Select(x => x).ToList();
                        details = result.AsEnumerable().Where(x => x.STRAIGHTENING_CD != request.STRAIGHTENING_CD).Select(x => x).ToList();

                    foreach (var item in details)
                    {
                        if (item.STR_TABA_NO_1 > 0)
                        {
                            if (item.STR_TABA_NO_1 == request.TABA_NO_1 || item.STR_TABA_NO_1 == request.TABA_NO_2 || item.STR_TABA_NO_1 == request.TABA_NO_3 || item.STR_TABA_NO_1 == request.TABA_NO_4)
                            {
                                if (string.IsNullOrEmpty(duplicateThaba))
                                {
                                    duplicateThaba = Convert.ToString(item.STR_TABA_NO_1);
                                }
                                else
                                {
                                    duplicateThaba = duplicateThaba + "," + Convert.ToString(item.STR_TABA_NO_1);
                                }
                            }
                        }
                        if (item.STR_TABA_NO_2 > 0)
                        {
                            if (item.STR_TABA_NO_2 == request.TABA_NO_1 || item.STR_TABA_NO_2 == request.TABA_NO_2 || item.STR_TABA_NO_2 == request.TABA_NO_3 || item.STR_TABA_NO_2 == request.TABA_NO_4)
                            {
                                if (string.IsNullOrEmpty(duplicateThaba))
                                {
                                    duplicateThaba = Convert.ToString(item.STR_TABA_NO_2);
                                }
                                else
                                {
                                    duplicateThaba = duplicateThaba + "," + Convert.ToString(item.STR_TABA_NO_2);
                                }
                            }
                        }
                        if (item.STR_TABA_NO_3 > 0)
                        {
                            if (item.STR_TABA_NO_3 == request.TABA_NO_1 || item.STR_TABA_NO_3 == request.TABA_NO_2 || item.STR_TABA_NO_3 == request.TABA_NO_3 || item.STR_TABA_NO_3 == request.TABA_NO_4)
                            {
                                if (string.IsNullOrEmpty(duplicateThaba))
                                {
                                    duplicateThaba = Convert.ToString(item.STR_TABA_NO_3);
                                }
                                else
                                {
                                    duplicateThaba = duplicateThaba + "," + Convert.ToString(item.STR_TABA_NO_3);
                                }
                            }
                        }
                        if (item.STR_TABA_NO_4 > 0)
                        {
                            if (item.STR_TABA_NO_4 == request.TABA_NO_1 || item.STR_TABA_NO_4 == request.TABA_NO_2 || item.STR_TABA_NO_4 == request.TABA_NO_3 || item.STR_TABA_NO_4 == request.TABA_NO_4)
                            {
                                if (string.IsNullOrEmpty(duplicateThaba))
                                {
                                    duplicateThaba = Convert.ToString(item.STR_TABA_NO_4);
                                }
                                else
                                {
                                    duplicateThaba = duplicateThaba + "," + Convert.ToString(item.STR_TABA_NO_4);
                                }
                            }
                        }
                        // }
                        if (!string.IsNullOrEmpty(duplicateThaba))
                        {
                            string[] value = duplicateThaba.Split(',');
                            var distinctItems = value.Distinct();
                            duplicateThaba = string.Join(", ", distinctItems);
                        }
                    }

                }


                }
                else if (result.Count == 1 && request.ISCHECKED == 0 && result[0].STRAIGHTENING_CD != request.STRAIGHTENING_CD)
                {
                    if (result[0].STR_TABA_NO_1 == request.TABA_NO_1 || result[0].STR_TABA_NO_1 == request.TABA_NO_2 || result[0].STR_TABA_NO_1 == request.TABA_NO_3 || result[0].STR_TABA_NO_1 == request.TABA_NO_4)
                    {
                        if (string.IsNullOrEmpty(duplicateThaba))
                        {
                            duplicateThaba = Convert.ToString(result[0].STR_TABA_NO_1);
                        }
                        else
                        {
                            duplicateThaba = duplicateThaba + "," + Convert.ToString(result[0].STR_TABA_NO_1);
                        }
                    }
                    if (Convert.ToInt16(result[0].STR_TABA_NO_2) > 0)
                    {
                        if (result[0].STR_TABA_NO_2 == request.TABA_NO_1 || result[0].STR_TABA_NO_2 == request.TABA_NO_2 || result[0].STR_TABA_NO_2 == request.TABA_NO_3 || result[0].STR_TABA_NO_2 == request.TABA_NO_4)
                        {
                            if (string.IsNullOrEmpty(duplicateThaba))
                            {
                                duplicateThaba = Convert.ToString(result[0].STR_TABA_NO_2);
                            }
                            else
                            {
                                duplicateThaba = duplicateThaba + "," + Convert.ToString(result[0].STR_TABA_NO_2);
                            }
                        }
                    }
                    if (Convert.ToInt16(result[0].STR_TABA_NO_3) > 0)
                    {
                        if (Convert.ToInt16(result[0].STR_TABA_NO_3) > 0 && result[0].STR_TABA_NO_3 == request.TABA_NO_1 || result[0].STR_TABA_NO_3 == request.TABA_NO_2 || result[0].STR_TABA_NO_3 == request.TABA_NO_3 || result[0].STR_TABA_NO_3 == request.TABA_NO_4)
                        {
                            if (string.IsNullOrEmpty(duplicateThaba))
                            {
                                duplicateThaba = Convert.ToString(result[0].STR_TABA_NO_3);
                            }
                            else
                            {
                                duplicateThaba = duplicateThaba + "," + Convert.ToString(result[0].STR_TABA_NO_3);
                            }
                        }
                    }
                    if (Convert.ToInt16(result[0].STR_TABA_NO_4) > 0)
                    {
                        if (Convert.ToInt16(result[0].STR_TABA_NO_4) > 0 && result[0].STR_TABA_NO_4 == request.TABA_NO_1 || result[0].STR_TABA_NO_4 == request.TABA_NO_2 || result[0].STR_TABA_NO_4 == request.TABA_NO_3 || result[0].STR_TABA_NO_4 == request.TABA_NO_4)
                        {
                            if (string.IsNullOrEmpty(duplicateThaba))
                            {
                                duplicateThaba = Convert.ToString(result[0].STR_TABA_NO_4);
                            }
                            else
                            {
                                duplicateThaba = duplicateThaba + "," + Convert.ToString(result[0].STR_TABA_NO_4);
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

        private List<SearchField> ComposeSearchCriteria(GridSearchFields gridSearchRequest)
        {
            var searchFields = new List<SearchField>();

            if (!string.IsNullOrEmpty(gridSearchRequest.CHUUBAN))
            {
                //string chuuban = string.Empty;
                //if (gridSearchRequest.CHUUBAN.Length > 9)
                //{
                //    chuuban = gridSearchRequest.CHUUBAN.Substring(0, 9);
                //}
                //else
                //{
                //    chuuban = gridSearchRequest.CHUUBAN.Trim();
                //}

                var searchField = new SearchField
                {
                   // FieldName = "PP.CHUUBAN",
                    FieldName = "CONCAT(PP.CHUUBAN_YEAR, CONCAT(PP.CHUUBAN,CASE WHEN LENGTH(TRIM(PP.GYOUBAN))=1 THEN '0'||PP.GYOUBAN END))",
                    SearchType = SearchType.Text,

                    SearchText1 = gridSearchRequest.CHUUBAN

                };

                searchFields.Add(searchField);
            }

            if (gridSearchRequest.TABA_NO > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "ST.STR_TABA_NO_1",
                    SearchType = SearchType.Text,
                    SearchText1 = Convert.ToString(gridSearchRequest.TABA_NO)
                };

                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.CUSTOMER_NAME))
            {
                var searchField = new SearchField
                {
                    FieldName = "CST.CUSTOMER_NAME",
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

            if (gridSearchRequest.PROD_DIM_D_MAX == 0)
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
            else if (gridSearchRequest.PROD_DIM_D_MIN == 0)
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

            //if (gridSearchRequest.PROD_DIM_D_MIN > 0)
            //{
            //    var searchField = new SearchField
            //    {
            //        FieldName = "PP.PROD_DIM_D",
            //        SearchType = SearchType.Number,
            //        SearchText1 = Convert.ToString(gridSearchRequest.PROD_DIM_D_MIN),
            //        SearchText2 = Convert.ToString(gridSearchRequest.PROD_DIM_D_MAX)

            //    };
            //    searchFields.Add(searchField);
            //}

            if (!string.IsNullOrEmpty(gridSearchRequest.SHAPE))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_SHAPE",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.SHAPE.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }

            //if (gridSearchRequest.PROD_LENGTH_MIN > 0)
            //{
            //    var searchField = new SearchField
            //    {
            //        FieldName = "PP.PROD_LENGTH",
            //        SearchType = SearchType.Number,
            //        SearchText1 = Convert.ToString(gridSearchRequest.PROD_LENGTH_MIN),
            //        SearchText2 = Convert.ToString(gridSearchRequest.PROD_LENGTH_MAX)
            //    };
            //    searchFields.Add(searchField);
            //}
            if (gridSearchRequest.PROD_LENGTH_MAX == 0)
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
            else if (gridSearchRequest.PROD_LENGTH_MIN == 0)
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

            if (!string.IsNullOrEmpty(gridSearchRequest.START_DATETIME))
            {
                var searchField = new SearchField
                {
                    FieldName = "TO_DATE((TO_CHAR(ST.STR_END_DATE, 'mm/dd/yyyy')), 'mm/dd/yyyy')",
                    SearchType = SearchType.Date,
                    SearchText1 = gridSearchRequest.START_DATETIME,
                    SearchText2 = gridSearchRequest.END_DATETIME
                };
                searchFields.Add(searchField);
            }

            if (gridSearchRequest.EMPLOYEE_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "EMPLOYEE_CD",
                    ReferenceTable = "EMP",
                    SearchType = SearchType.Number,
                    SearchText1 = gridSearchRequest.EMPLOYEE_CD.ToString()
                };

                searchFields.Add(searchField);
            }
            return searchFields;
        }
        public List<NameValue> GetUnitTypeList()
        {
            var _UnitTypeList = UIUtils.GetUnitTypetKubun();
            return _UnitTypeList;
        }
        public List<NameValue> GetTPTestList()
        {
            var _tpTestList = UIUtils.GetTPTest();
            return _tpTestList;
        }
        public List<NameValue> GetNextProcessList()
        {
            var _nextProcessList = UIUtils.GetNextProcesKubun();
            return _nextProcessList;
        }
        public List<NameValue> GetDecisionList()
        {
            var _DecisionList = UIUtils.GetResultKubun();
            return _DecisionList;
        }
        public List<NameValue> GetExteriorConditionList()
        {
            var _conditionList = UIUtils.GetExteriorConditionKubunGetKubun();
            return _conditionList;
        }
        public List<NameValue> GetBendStatusList()
        {
            var _statusList = UIUtils.GetBendStatus();
            return _statusList;
        }
        public List<Straightening2Details> GetStraightning2DailyReportDataOnFactoryDailyWorkStatus(GridSearchFields gridSearchRequest)
        {
            //List<ECDetails> result = new List<ECDetails>();
            List<SearchField> searchFields = ComposeSearchCriteriaforFactory(gridSearchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);
            int rowLimit = gridSearchRequest.ROW_LIMIT;

            var obj = _straighteningRepository.GetStraightning2DailyReportDataOnFactoryDailyWorkStatus(searchFields, selectedSortCriteria, rowLimit, gridSearchRequest.CULTURE_NAME);           
            return obj;
        }
        public List<Straightening2Details> GetStraightning2DailyProcessAnalysisStatus(GridSearchFields gridSearchRequest)
        {
            //List<ECDetails> result = new List<ECDetails>();
            List<SearchField> searchFields = ComposeSearchCriteriaforStraightning2DailyProcessAnalysisStatus(gridSearchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);
            var obj = _straighteningRepository.GetStraightning2DailyProcessAnalysisStatus(searchFields, selectedSortCriteria, gridSearchRequest);
            
            return obj;
        }
        private List<SearchField> ComposeSearchCriteriaforFactory(GridSearchFields gridSearchRequest)
        {
            var searchFields = new List<SearchField>();
            if (gridSearchRequest.MACHINE_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "STR2.MACHINE_CD",
                    SearchType = SearchType.Number,
                    SearchText1 =Convert.ToString(gridSearchRequest.MACHINE_CD)
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.LOCATION_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "STR2.LOCATION_CD",
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
                    FieldName = "STR2.STR_END_OPERATOR_CD",
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
                    FieldName = "INTE.INTERNAL_NAME",
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
                    SearchText1 ="'" + gridSearchRequest.PROD_SPEC_NO1 +"'"
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_SPEC_NO2))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_SPEC_NO2",
                    SearchType = SearchType.Number,
                    SearchText1 = "'" + gridSearchRequest.PROD_SPEC_NO2 +"'"
                };
                searchFields.Add(searchField);
            }
            if (string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE2))
            {
                if (!string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE1))
                {
                    var searchField = new SearchField
                    {
                        FieldName = "STR2.STR_END_DATE",
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
                        FieldName = "STR2.STR_END_DATE",
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
                    FieldName = "STR2.STR_END_DATE",
                    SearchType = SearchType.DateTime,
                    SearchText1 = gridSearchRequest.INSPECTION_DATE1,
                    SearchText2 = gridSearchRequest.INSPECTION_DATE2
                };
                searchFields.Add(searchField);
            }


            return searchFields;
        }
        private List<SearchField> ComposeSearchCriteriaforStraightning2DailyProcessAnalysisStatus(GridSearchFields gridSearchRequest)
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
                    SearchText1 ="'" + gridSearchRequest.PROD_SPEC_NO1 + "'"
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_SPEC_NO2))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_SPEC_NO2",
                    SearchType = SearchType.Number,
                    SearchText1 ="'" + gridSearchRequest.PROD_SPEC_NO2 + "'"
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
                    FieldName = "CONDITION.CONDITION_CD",
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
                    SearchText1 =Convert.ToString(gridSearchRequest.MATL_DIM_W)
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
                    FieldName = "MACH.MACHINE_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.MACHINE_NAME.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.LOCATION_NAME))
            {
                var searchField = new SearchField
                {
                    FieldName = "LOCA.SHORT_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.LOCATION_NAME.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.BEGINNING_START_DATE))
            {
                var searchField = new SearchField
                {
                    FieldName = "STR.STR_START_DATE",
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
                    FieldName = "EMPL.SHORT_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.START_OPERATOR.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.METALLIC_PLATE_CODE_STRING))
            {
                var searchField = new SearchField
                {
                    FieldName = "STR.STR_METALLIC_PLATE_CODE",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.METALLIC_PLATE_CODE_STRING.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.FURNACE_NO))
            {
                var searchField = new SearchField
                {
                    FieldName = "STR.FURNACE_NO",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.FURNACE_NO.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.END_DATE1))
            {
                var searchField = new SearchField
                {
                    FieldName = "STR.STR_END_DATE",
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
                    FieldName = "EMPL1.SHORT_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.END_OPERATOR.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.DECISION > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "STR.STR_DECISION",
                    SearchType = SearchType.Text,
                    SearchText1 = Convert.ToString(gridSearchRequest.DECISION)
                };
                searchFields.Add(searchField);
            }

            return searchFields;
        }

              
        public int GetRowCountForStraightening2Factory(GridSearchFields gridSearchRequest)
        {
            List<SearchField> searchFields = ComposeSearchCriteriaforFactory(gridSearchRequest);
            int count = _straighteningRepository.GetRowCountForStraightening2Factory(searchFields);
            return count;
        }

        public int GetRowCountForStraightening2DailyAnalysis(GridSearchFields gridSearchRequest)
        {
            List<SearchField> searchFields = ComposeSearchCriteriaforStraightning2DailyProcessAnalysisStatus(gridSearchRequest);
            int count = _straighteningRepository.GetRowCountForStraightening2DailyAnalysis(searchFields);
            return count;
        }
    }
}
