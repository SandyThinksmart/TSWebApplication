using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
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
    public class MIHardnessService : IMIHardnessService
    {
        private readonly IMIHardnessRepository _HardnessRepository;
        private readonly IAllCommonRepository _AllCommonRepository;
        private readonly IUserLoginRepository _UserLoginRepository;
        private readonly LocalizationServices _localization;
        private readonly IAllCommonService _allCommonService;
        public MIHardnessService(IMIHardnessRepository MIHardnessRepository, IAllCommonRepository allCommonRepository, IUserLoginRepository userLoginRepository, LocalizationServices localization, IAllCommonService allCommonService)
        {
            _HardnessRepository = MIHardnessRepository;
            _AllCommonRepository = allCommonRepository;
            _UserLoginRepository = userLoginRepository;
            _localization = localization;
            _allCommonService = allCommonService;
        }
        public MIHardnessDetails GetHardnessDetailsByChuuban(string chubaan, short? gyouban, string culture_Name)
        {

            if (culture_Name == "en")
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
            }
            else
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("ja-JP");
            }
            var obj = _HardnessRepository.GetHardnessDetailsByChuuban(chubaan, gyouban);
            if (obj != null)
            {
                if (obj.PP_STD_DEL_PERIOD >= 0)
                {
                    obj.PP_STD_DEL_PERIOD_STRING = obj.PP_STD_DEL_PERIOD + " " + _localization.GetLocalizedHtmlString("lbl_Day");
                }

                if (obj.PP_UNIT > 0)
                {
                    obj.PP_NO_OF_UNITS_DECODE = Convert.ToDecimal(obj.PP_NO_OF_UNITS).ToString("0.####") + UIUtils.GetProductUnitKubunValue(Convert.ToString(obj.PP_UNIT));
                }
                if (obj.PP_UNIT_WT_KUBUN != null)
                {
                    obj.PP_UNIT_WT_KUBUN_STRING = Utils.GetUnitWeigth(Convert.ToInt32(obj.PP_UNIT_WT_KUBUN), Convert.ToInt32(obj.PP_UNIT_WT), Convert.ToInt32(obj.PP_UNIT_WT_MIN), Convert.ToInt32(obj.PP_UNIT_WT_MAX));

                    if (obj.PP_UNIT_WT_KUBUN == 3)
                    {
                        obj.PP_UNIT_WT_KUBUN_STRING = Convert.ToString(obj.PP_UNIT_WT) + _localization.GetLocalizedHtmlString("lbl_Hone");
                    }


                }

                //if (obj.DW_START_OPERATOR_NAME != null)    // Stat Data :  Bind DW No Of Unit
                //{
                //    obj.DW_NO_OF_COILS = Convert.ToDecimal(obj.PP_NO_OF_UNITS).ToString("0.####") + UIUtils.Instance.GetProductUnitKubunValue(Convert.ToString(obj.PP_UNIT));
                //}


                if (obj.MAGARI_KUBUN > 0)
                {
                    obj.PP_MAGARI_BEND_TOL = Utils.GetMagariBendTolSpan(UIUtils.Instance.GetMagariKubunValue(Convert.ToString(obj.MAGARI_KUBUN)), Convert.ToDecimal(obj.MAGARI_BEND_TOL), Convert.ToInt32(obj.MAGARI_SPAN));  //Convert.ToDecimal(obj.PP_NO_OF_UNITS).ToString("0.####") + UIUtils.Instance.GetProductUnitKubunValue(Convert.ToString(obj.PP_UNIT));
                }

                //if (obj.PP_COLOR_ID > 0)
                //{
                //    obj.PP_COLOR = Utils.GetColorName(Convert.ToInt16(obj.PP_COLOR_ID));
                //}

                //if (!string.IsNullOrEmpty(obj.PP_COIL_DIA_WINDING))
                //{
                //    obj.PP_COIL_DIA_WINDING = UIUtils.GetCoilDiaWindingKubunValue(Convert.ToString(obj.PP_COIL_DIA_WINDING));
                //}
                if (obj.PP_COIL_DIA_WINDING != null)
                {
                    string PP_COIL_DIA_WINDING = UIUtils.GetCoilDiaWindingKubunValue(Convert.ToString(obj.PP_COIL_DIA_WINDING));
                }

                if (obj.HARDNESS_PATTERN1_CODE > 0)
                {

                    obj.HARDNESS_PATTERN1_CODE_STRING = UIUtils.GetStandardHardnessKubunValue(Convert.ToString(obj.HARDNESS_PATTERN1_CODE));
                    obj.HARDNESS_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(obj.HARDNESS_PATTERN1_MIN_MAX));
                }
                if (obj.HARDNESS_PATTERN2_CODE > 0)
                {

                    obj.HARDNESS_PATTERN2_CODE_STRING = UIUtils.GetStandardHardnessKubunValue(Convert.ToString(obj.HARDNESS_PATTERN2_CODE));

                }
                obj.DECISION_OK = Convert.ToInt16(UIUtils.GetResultKubunKey(_localization.GetLocalizedHtmlString("lbl_OK")));
                var messageList = _AllCommonRepository.GetMessageDetailsByProcessPlanCD(Convert.ToInt64(obj.PP_PROCESS_PLAN_CD), true);
                obj.MI_MESSAGE_LIST = messageList;

                obj.PP_ELONGATION = Helper.GetProductPatternFormat(Convert.ToSingle(obj.ELONGATION_PATTERN1), obj.ELONGATION_PATTERN1_MIN_MAX, Convert.ToDecimal(obj.ELONGATION_PATTERN2_MIN), Convert.ToDecimal(obj.ELONGATION_PATTERN2_MAX), obj.ELONGATION_PATTERN3_ACTUAL);
                obj.PP_TENSILE_STRENGTH = Helper.GetProductPatternFormat(Convert.ToSingle(obj.TENSILE_STRN_PATTERN1), obj.TENSILE_STRN_PATTERN1_MIN_MAX, obj.TENSILE_STRN_PATTERN2_MIN, obj.TENSILE_STRN_PATTERN2_MAX, obj.TENSILE_STRN_PATTERN3_ACTUAL);
                obj.PP_HARDNESS = Helper.GetHardnessProductPatternFormat(obj.HARDNESS_PATTERN1_CODE, obj.HARDNESS_PATTERN1, obj.HARDNESS_PATTERN1_MIN_MAX, obj.HARDNESS_PATTERN2_CODE, obj.HARDNESS_PATTERN2_MIN, obj.HARDNESS_PATTERN2_MAX, obj.HARDNESS_PATTERN3_ACTUAL);
                obj.PP_DECARB = Helper.GetDecarbProductPatternFormat(obj.DECARB_1, Convert.ToSingle(obj.DECARB_VAL_1), obj.DECARB_2, Convert.ToSingle(obj.DECARB_VAL_2), obj.DECARB_3, Convert.ToSingle(obj.DECARB_VAL_3));
                obj.PP_YEILD = Helper.GetProductPatternFormat(Convert.ToSingle(obj.YEILD_PATTERN1), obj.YEILD_PATTERN1_MIN_MAX, obj.YEILD_PATTERN2_MIN, obj.YEILD_PATTERN2_MAX, obj.YEILD_PATTERN3_ACTUAL);
                obj.PP_PRESS = Helper.GetProductPatternFormat(Convert.ToSingle(obj.PRESS_PATTERN1), obj.PRESS_PATTERN1_MIN_MAX, obj.PRESS_PATTERN2_MIN, obj.PRESS_PATTERN2_MAX, obj.PRESS_PATTERN3_ACTUAL);

            }




            return obj;
        }
        public string HardnessDetailsDelete(MIHardnessDetails request)
        {
            var result = "";
            using (TransactionScope transaction = new TransactionScope())
            {

                if (request.MI_HARDNESS_CD > 0)
                {
                    result = _HardnessRepository.DeleteHardnessDetails(Convert.ToInt64(request.MI_HARDNESS_CD));
                }
                if (request.MSG_CD > 0 && result == "1") //1:Data Deleted Successfully 
                {
                    _AllCommonRepository.MessageDetailsDelete(Convert.ToInt64(request.MSG_CD));
                }
                //_AllCommonRepository.DeleteWIPStatusData(Convert.ToInt64(request.PROCESS_PLAN_CD), Convert.ToInt16(request.DETAIL_NO));              //code commited on 08/10/2025 for without taba requirment data no need to save into TRN_WIP_STATUS table

                transaction.Complete();
            }
            return result;
        }

        public List<NameValue> GetNextProcessList()
        {
            var _nextProcessList = UIUtils.GetNextProcesKubun();
            return _nextProcessList;
        }
        public List<NameValue> SetHardnessKubunsList()
        {
            var _HardnessKubunList = UIUtils.GetStandardHardnessKubun();

            return _HardnessKubunList;
        }

        public List<NameValue> SetDecisionList()
        {
            var _decisionkubunList = UIUtils.GetResultKubun();

            return _decisionkubunList;
        }

        public string GetDecisionValue(string value)
        {
            return UIUtils.GetDimDecisionKubunValue(value);
        }

        public void SaveHardnessData(MIHardnessDetails request)
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                if (request.USER_ID != null && request.USER_ID > 0)
                {
                    request.INPUT_LOCATION_CD = _UserLoginRepository.GetUserLocationById(request.USER_ID.Value);
                    request.LOCATION_CD = request.INPUT_LOCATION_CD;
                }
                request.PROCESS_KUBUN_CD = Convert.ToInt16(UIUtils.GetProcessKubun(_localization.GetLocalizedHtmlString("lbl_ProcessMechanical_Inspection_Hardness")));
                if (request.MSG_CD == Constants.NullValue.INT && !string.IsNullOrEmpty(request.REMARKS))
                {
                    MessageDetails messageDetails = new MessageDetails();
                    messageDetails.PROCESS_PLAN_CD = Convert.ToInt64(request.PROCESS_PLAN_CD);

                    messageDetails.PROCESS_KUBUN_ID = Convert.ToInt16(UIUtils.GetProcessKubun(_localization.GetLocalizedHtmlString("lbl_ProcessMechanical_Inspection_Hardness")));
                    messageDetails.OPERATOR_CD = Convert.ToInt64(request.HD_TEST_OPERATOR_CD);
                    messageDetails.MSG_CONTENTS = request.REMARKS;
                    messageDetails.USER_ID = Convert.ToInt32(request.USER_ID);
                    //     messageDetails.LOCATION_CD = Convert.ToInt32(request.LOCATION_CD);
                    messageDetails.INPUT_LOCATION_CD = Convert.ToInt32(request.INPUT_LOCATION_CD);


                    messageDetails = _AllCommonRepository.MessageDetailsInsert(messageDetails);

                    if (messageDetails != null)
                    {
                        request.MSG_CD = Convert.ToInt32(messageDetails.MSG_CD);
                    }


                }
                _HardnessRepository.InsertHardnessDetails(request);

                //if (request.HD_NEXT_PROCESS != 999)          //code commited on 08/10/2025 for without taba requirment data no need to save into TRN_WIP_STATUS table
                //{

                //    WipStatusDetails wipDetails = new WipStatusDetails();
                //    wipDetails = new WipStatusDetails();
                //    wipDetails.PROCESS_PLAN_CD = request.PROCESS_PLAN_CD;
                //    wipDetails.LOCATION_CD = Convert.ToInt16(request.LOCATION_CD);
                //    wipDetails.MSG_CD = request.MSG_CD;
                //    wipDetails.FINISHED_BARS = null;
                //    wipDetails.UNIT = null;
                //    wipDetails.FINISHED_QTY = null;
                //    wipDetails.PROCESS_START_DATE = request.HD_TEST_DATE;
                //    wipDetails.PROCESS_END_DATE = request.HD_TEST_DATE;
                //    wipDetails.PROCESS_KUBUN_CD = request.PROCESS_KUBUN_CD;
                //    wipDetails.PRE_PROSESS_DECISION = request.HD_DECISION;
                //    wipDetails.NEXT_PROCESS = Convert.ToInt16(request.HD_NEXT_PROCESS);
                //    wipDetails.START_OPERATOR_CD = request.HD_TEST_OPERATOR_CD;
                //    wipDetails.END_OPERATOR_CD = request.HD_TEST_OPERATOR_CD;
                //    wipDetails.TABA_NO = Convert.ToInt16(request.DETAIL_NO);
                //    wipDetails.NIFUDA_CD = null;

                //    _AllCommonRepository.InsertWIPStatusData(wipDetails);

                //}
                transaction.Complete();

            }
        }
        public int GetUserLocationById(int userId)
        {

            return _UserLoginRepository.GetUserLocationById(userId);
        }
        public List<ThabaDetails> GetThabaDetailsByChuuban(string chuuban, short? gyouban, long? MIHardnessCD)
        {


            var result = _HardnessRepository.GetThabaDetailsByChuuban(chuuban, gyouban, MIHardnessCD);

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
                //if (_localization.GetLocalizedHtmlString("lbl_ReHeatTreatment") == decisionName)
                //{
                //    record.DECISION_VALUE = 5;
                //}
                //if (_localization.GetLocalizedHtmlString("lbl_ReDrawing") == decisionName)
                //{
                //    record.DECISION_VALUE = 5;
                //}
                //if (_localization.GetLocalizedHtmlString("lbl_Restraightening") == decisionName)
                //{
                //    record.DECISION_VALUE = 5;
                //}
                //if (_localization.GetLocalizedHtmlString("lbl_Re_Chamfering") == decisionName)
                //{
                //    record.DECISION_VALUE = 5;
                //}
                //if (_localization.GetLocalizedHtmlString("lbl_ReEc") == decisionName)
                //{
                //    record.DECISION_VALUE = 5;
                //}

            }

            return result;
        }

        public List<MIHardnessDetails> GetThabaDetailsByThabaNumber(string chuuban, short? gyouban, long ThabaNo)
        {
            var result = _HardnessRepository.GetThabaDetailsByThabaNumber(chuuban, gyouban, ThabaNo);
            return result;
        }

        public List<MIHardnessDetails> GetMIHardnessdetailsList(GridSearchFields gridSearchRequest)
        {
            short Location_CD = Convert.ToInt16(_UserLoginRepository.GetUserLocationById(Convert.ToInt16(gridSearchRequest.USER_ID)));
            gridSearchRequest.LOCATION_CD = Location_CD;
            List<MIHardnessDetails> result = new List<MIHardnessDetails>();
            List<SearchField> searchFields = ComposeSearchCriteria(gridSearchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);


            result = _HardnessRepository.GetMIHardnessList(searchFields, selectedSortCriteria, gridSearchRequest.LOCATION_CD);
            foreach (MIHardnessDetails record in result)
            {

                record.HD_DECISION_STRING = record.HD_DECISION != 0 ? UIUtils.GetResultKubunValue(Convert.ToString(record.HD_DECISION)) : string.Empty;

            }
            return result;
        }
        public long UpdateHardnessDetails(MIHardnessDetails request)
        {
            long msgCd = 0;
            bool deleteMsg = false;
            MIHardnessDetails result = new MIHardnessDetails();
            if (request.USER_ID != null && request.USER_ID > 0)
            {
                request.INPUT_LOCATION_CD = _UserLoginRepository.GetUserLocationById(request.USER_ID.Value);
                request.LOCATION_CD = request.INPUT_LOCATION_CD;
            }

            using (TransactionScope transaction = new TransactionScope())
            {

                MessageDetails messageDetails = new MessageDetails();
                messageDetails.PROCESS_PLAN_CD = Convert.ToInt64(request.PROCESS_PLAN_CD);
                messageDetails.PROCESS_KUBUN_ID = Convert.ToInt16(UIUtils.GetProcessKubun(_localization.GetLocalizedHtmlString("lbl_ProcessMechanical_Inspection_Hardness")));
                messageDetails.OPERATOR_CD = Convert.ToInt64(request.HD_TEST_OPERATOR_CD);
                messageDetails.MSG_CONTENTS = request.REMARKS;
                messageDetails.MSG_CD = Convert.ToInt32(request.MSG_CD);
                messageDetails.USER_ID = Convert.ToInt32(request.USER_ID);
                messageDetails.LOCATION_CD = Convert.ToInt32(request.LOCATION_CD);
                messageDetails.INPUT_LOCATION_CD = Convert.ToInt32(request.INPUT_LOCATION_CD);
                if (request.MSG_CD == Constants.NullValue.INT && !string.IsNullOrEmpty(request.REMARKS))
                {

                    messageDetails = _AllCommonRepository.MessageDetailsInsert(messageDetails);

                    if (messageDetails.MSG_CD != Constants.NullValue.INT && messageDetails.MSG_CD > 0)
                    {
                        request.MSG_CD = Convert.ToInt32(messageDetails.MSG_CD);
                    }
                }

                else if (request.MSG_CD > 0 && request.REMARKS != null)
                {
                    messageDetails.MSG_CD = Convert.ToInt32(request.MSG_CD);
                    _AllCommonRepository.MessageDetailsUpdate(messageDetails);

                }

                else if (request.MSG_CD > 0 && request.REMARKS == null)
                {
                    long messageCD = Convert.ToInt64(request.MSG_CD);
                    request.MSG_CD = Constants.NullValue.LONG;
                    msgCd = _HardnessRepository.UpdateHardnessDetails(request);
                    _AllCommonRepository.MessageDetailsDelete(messageCD);
                    deleteMsg = true;
                }
                if (deleteMsg == false)
                {
                    msgCd = _HardnessRepository.UpdateHardnessDetails(request);
                }
                //WipStatusDetails wipDetails = new WipStatusDetails();

                //wipDetails = new WipStatusDetails();
                //wipDetails.PROCESS_PLAN_CD = request.PROCESS_PLAN_CD;
                //wipDetails.LOCATION_CD = Convert.ToInt16(request.LOCATION_CD);
                //wipDetails.MSG_CD = request.MSG_CD;
                //wipDetails.FINISHED_BARS =null;
                //wipDetails.UNIT = null;
                //wipDetails.FINISHED_QTY = null;
                //wipDetails.PROCESS_START_DATE = request.HD_TEST_DATE;
                //wipDetails.PROCESS_END_DATE = null;
                //wipDetails.PROCESS_KUBUN_CD = Convert.ToInt32(UIUtils.GetProcessKubun(_localization.GetLocalizedHtmlString("lbl_ProcessMechanical_Inspection_Hardness")));
                //wipDetails.PRE_PROSESS_DECISION = request.HD_DECISION;
                //wipDetails.NEXT_PROCESS = Convert.ToInt16(request.HD_NEXT_PROCESS);
                //wipDetails.START_OPERATOR_CD = request.HD_TEST_OPERATOR_CD;
                //wipDetails.END_OPERATOR_CD = null;
                //wipDetails.TABA_NO =Convert.ToInt16(request.DETAIL_NO);
                //wipDetails.NIFUDA_CD = null;

                //_AllCommonRepository.InsertWIPStatusData(wipDetails);
                transaction.Complete();
            }
            return msgCd;
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
                    //FieldName = "PP.CHUUBAN",
                    //FieldName = "CONCAT(PP.CHUUBAN,CASE WHEN LENGTH(TRIM(PP.GYOUBAN))=1 THEN '0'||PP.GYOUBAN END)",
                    FieldName = "CONCAT(PP.CHUUBAN_YEAR, CONCAT(PP.CHUUBAN,CASE WHEN LENGTH(TRIM(PP.GYOUBAN))=1 THEN '0'||PP.GYOUBAN END))",
                    SearchType = SearchType.Text,

                    SearchText1 = gridSearchRequest.CHUUBAN

                };

                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.CUSTOMER_NAME))
            {
                var searchField = new SearchField
                {
                    FieldName = "CST.SHORT_NAME",
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
                    // FieldName = "PP.PROD_NAME_INT",
                    FieldName = "INTE.INTERNAL_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.INTERNAL_NAME_STRING.ToLower().Trim(),
                    SearchCondition = SearchCondition.Equal.ToString(),
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
            if (string.IsNullOrEmpty(gridSearchRequest.END_DATETIME))
            {
                if (!string.IsNullOrEmpty(gridSearchRequest.START_DATETIME))
                {
                    var searchField = new SearchField
                    {
                        FieldName = "HA.HD_TEST_DATE",
                        SearchType = SearchType.Date,
                        SearchText1 = gridSearchRequest.START_DATETIME
                    };
                    searchFields.Add(searchField);
                }
            }
            else if (string.IsNullOrEmpty(gridSearchRequest.START_DATETIME))
            {
                if (!string.IsNullOrEmpty(gridSearchRequest.END_DATETIME))
                {
                    var searchField = new SearchField
                    {
                        FieldName = "HA.HD_TEST_DATE",
                        SearchType = SearchType.Date,
                        SearchText1 = gridSearchRequest.END_DATETIME
                    };
                    searchFields.Add(searchField);
                }
            }
            else if (!string.IsNullOrEmpty(gridSearchRequest.START_DATETIME) && !string.IsNullOrEmpty(gridSearchRequest.END_DATETIME))
            {
                var searchField = new SearchField
                {
                    FieldName = "HA.HD_TEST_DATE",
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
        public MIHardnessDetails GetMIHardnessDetailsbyHardnessCD(long? drawingCD, string chuuban, short? gyouban, string culture_Name)
        {
            if (culture_Name == "en")
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
            }
            else
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("ja-JP");
            }
            var _hardnessDetails = _HardnessRepository.GetHDdetailsbyHRDCD(drawingCD, chuuban, gyouban);

            if (_hardnessDetails != null)
            {
                if (_hardnessDetails.PP_STD_DEL_PERIOD >= 0)
                {
                    _hardnessDetails.PP_STD_DEL_PERIOD_STRING = _hardnessDetails.PP_STD_DEL_PERIOD + " " + _localization.GetLocalizedHtmlString("lbl_Day");
                }

                if (_hardnessDetails.PP_UNIT > 0)
                {
                    _hardnessDetails.PP_NO_OF_UNITS_DECODE = Convert.ToDecimal(_hardnessDetails.PP_NO_OF_UNITS).ToString("0.####") + UIUtils.GetProductUnitKubunValue(Convert.ToString(_hardnessDetails.PP_UNIT));
                }
                if (_hardnessDetails.PP_UNIT_WT_KUBUN != null)
                {
                    _hardnessDetails.PP_UNIT_WT_KUBUN_STRING = Utils.GetUnitWeigth(Convert.ToInt32(_hardnessDetails.PP_UNIT_WT_KUBUN), Convert.ToInt32(_hardnessDetails.PP_UNIT_WT), Convert.ToInt32(_hardnessDetails.PP_UNIT_WT_MIN), Convert.ToInt32(_hardnessDetails.PP_UNIT_WT_MAX));

                    if (_hardnessDetails.PP_UNIT_WT_KUBUN == 3)
                    {
                        _hardnessDetails.PP_UNIT_WT_KUBUN_STRING = Convert.ToString(_hardnessDetails.PP_UNIT_WT) + _localization.GetLocalizedHtmlString("lbl_Hone");
                    }


                }

                if (_hardnessDetails.MAGARI_KUBUN > 0)
                {
                    _hardnessDetails.PP_MAGARI_BEND_TOL = Utils.GetMagariBendTolSpan(UIUtils.Instance.GetMagariKubunValue(Convert.ToString(_hardnessDetails.MAGARI_KUBUN)), Convert.ToDecimal(_hardnessDetails.MAGARI_BEND_TOL), Convert.ToInt32(_hardnessDetails.MAGARI_SPAN));
                }

                //if (_hardnessDetails.PP_COLOR_ID > 0)
                //{
                //    _hardnessDetails.PP_COLOR = UIUtils.GetColorKubunValue(Convert.ToString(_hardnessDetails.PP_COLOR_ID));   // Utils.GetColorName(Convert.ToInt16(_stamppingDetails.PP_COLOR_ID));

                // }
                if (_hardnessDetails.HARDNESS_PATTERN1_CODE > 0)
                {

                    _hardnessDetails.HARDNESS_PATTERN1_CODE_STRING = UIUtils.GetStandardHardnessKubunValue(Convert.ToString(_hardnessDetails.HARDNESS_PATTERN1_CODE));
                    _hardnessDetails.HARDNESS_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(_hardnessDetails.HARDNESS_PATTERN1_MIN_MAX));
                }
                if (_hardnessDetails.HARDNESS_PATTERN2_CODE > 0)
                {

                    _hardnessDetails.HARDNESS_PATTERN2_CODE_STRING = UIUtils.GetStandardHardnessKubunValue(Convert.ToString(_hardnessDetails.HARDNESS_PATTERN2_CODE));

                }
                _hardnessDetails.DECISION_OK = Convert.ToInt16(UIUtils.GetResultKubunKey(_localization.GetLocalizedHtmlString("lbl_OK")));
                //if (_drawingDetails.PP_COIL_DIA_WINDING != null)
                //{
                //    _drawingDetails.PP_COIL_DIA_WINDING = UIUtils.GetCoilDiaWindingKubunValue(Convert.ToString(_drawingDetails.PP_COIL_DIA_WINDING)); //  Utils.GetCoilDiaWindingValue(Convert.ToInt16(_stamppingDetails.PP_COIL_DIA_WINDING));
                //}
                return _hardnessDetails;
            }
            else
            {
                return new MIHardnessDetails();
            }
        }
        public List<ThabaDetails> CheckThabaExistorNot(string chuuban, short? gyouban, long? MIHardnessCD)
        {


            var result = _HardnessRepository.CheckThabaExistorNot(chuuban, gyouban, MIHardnessCD);
            return result;
        }
        public List<MIHardnessDetails> GetMechHardnessOnFactoryDailyWorkStatus(GridSearchFields gridSearchRequest)
        {

            List<SearchField> searchFields = ComposeSearchCriteriaforFactory(gridSearchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);
            int rowLimit = gridSearchRequest.ROW_LIMIT;
            var obj = _HardnessRepository.GetMechHardnessOnFactoryDailyWorkStatus(searchFields, selectedSortCriteria, rowLimit);
            return obj;
        }
        public List<MIHardnessDetails> GetMechHardnessOnDailyProcessStatus(GridSearchFields gridSearchRequest)
        {

            List<SearchField> searchFields = ComposeSearchCriteriaforDailyProcessStatus(gridSearchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);
            var obj = _HardnessRepository.GetMechHardnessOnDailyProcessStatus(searchFields, selectedSortCriteria, gridSearchRequest);
            return obj;
        }
        private List<SearchField> ComposeSearchCriteriaforFactory(GridSearchFields gridSearchRequest)
        {
            var searchFields = new List<SearchField>();

            if (gridSearchRequest.LOCATION_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "MI.INPUT_LOCATION_CD",//change 2025/12/11 as per client requirement
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
                    FieldName = "MI.HD_TEST_OPERATOR_CD",
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
                    FieldName = "INTE.INTERNAL_NAME ",
                    SearchType = SearchType.Text,
                    SearchCondition = SearchCondition.Equal.ToString(),
                    SearchText1 = gridSearchRequest.INTERNAL_NAME_STRING.ToLower().Trim()
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
                    FieldName = "PP.PROD_LENGTH ",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.PROD_LENGTH_MIN),
                    SearchText2 = Convert.ToString(gridSearchRequest.PROD_LENGTH_MAX)
                };
                searchFields.Add(searchField);
            }

            if (string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE2))
            {
                if (!string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE1))
                {
                    var searchField = new SearchField
                    {
                        FieldName = "MI.HD_TEST_DATE",
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
                        FieldName = "MI.HD_TEST_DATE",
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
                    FieldName = "MI.HD_TEST_DATE",
                    SearchType = SearchType.DateTime,
                    SearchText1 = gridSearchRequest.INSPECTION_DATE1,
                    SearchText2 = gridSearchRequest.INSPECTION_DATE2
                };
                searchFields.Add(searchField);
            }

            return searchFields;
        }
        private List<SearchField> ComposeSearchCriteriaforDailyProcessStatus(GridSearchFields gridSearchRequest)
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
                    SearchText1 = gridSearchRequest.CUSTOMER_NAME
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_NAME_CUST))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_NAME_CUST",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.PROD_NAME_CUST
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
                    SearchText1 = Convert.ToString(gridSearchRequest.PROD_DIM_OTHER_STRING)
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
                    SearchText1 = Convert.ToString(gridSearchRequest.LENGTH_OTHER),

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
                    SearchText1 = gridSearchRequest.MATL_NAME_MAK
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
                    SearchText1 = gridSearchRequest.MATL_DIM_OTHER
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
                    SearchText1 = gridSearchRequest.CHARGE
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
            if (!string.IsNullOrEmpty(gridSearchRequest.LOCATION_NAME))
            {
                var searchField = new SearchField
                {
                    FieldName = "LOCA.SHORT_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.LOCATION_NAME
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE1))
            {
                var searchField = new SearchField
                {
                    FieldName = "MIHARD.HD_TEST_DATE",
                    SearchType = SearchType.Date,
                    SearchText1 = gridSearchRequest.INSPECTION_DATE1,
                    SearchText2 = gridSearchRequest.INSPECTION_DATE2
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.EMPLOYEE_NAME))
            {
                var searchField = new SearchField
                {
                    FieldName = "EMP1.SHORT_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.EMPLOYEE_NAME
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.FURNACE_NO))
            {
                var searchField = new SearchField
                {
                    FieldName = "MIHARD.HD_FURNACE_NO",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.FURNACE_NO
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.DECISION > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "MIHARD.HD_DECISION",
                    SearchType = SearchType.Text,
                    SearchText1 = Convert.ToString(gridSearchRequest.DECISION)
                };
                searchFields.Add(searchField);
            }

            return searchFields;
        }


        public int GetRowCountForMechHardnessFactory(GridSearchFields gridSearchRequest)
        {
            List<SearchField> searchFields = ComposeSearchCriteriaforFactory(gridSearchRequest);
            int count = _HardnessRepository.GetRowCountForMechHardnessFactory(searchFields);
            return count;
        }

        public int GetRowCountForMechHardnessDailyAnalysis(GridSearchFields gridSearchRequest)
        {
            List<SearchField> searchFields = ComposeSearchCriteriaforDailyProcessStatus(gridSearchRequest);
            int count = _HardnessRepository.GetRowCountForMechHardnessDailyAnalysis(searchFields);
            return count;
        }
        public MIHardnessDetails GetFurnanceNo(string chuuban, short? gyouban, int detail_No)
        {
            return _HardnessRepository.GetFurnanceNo(chuuban, gyouban, detail_No);

        }
        public List<MIHardnessDetails> GetFICombinedDecisionDetails(string chuuban, short? gyouban)
        {
            var result = _HardnessRepository.GetFICombinedDecisionDetails(chuuban, gyouban);
            return result;
        }
        public void UpdateFICombinedDecision(short hdDecision, long processPlanCd)
        {

            using (TransactionScope transaction = new TransactionScope())
            {
                _HardnessRepository.UpdateFICombinedDecision(hdDecision, processPlanCd);
                transaction.Complete();
            }
        }
        
        public string ValidateHardnessDetailsOnUpdateBtn(long MITensileCD)
        {
            var result = _HardnessRepository.ValidateHardnessDetailsOnUpdateBtn(MITensileCD);
            return result;
        }

    }
}
