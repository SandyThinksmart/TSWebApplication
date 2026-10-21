using MailKit.Search;
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
    public class TensileService : ITensileService
    {
        private readonly ITensileRepository _tensileRepository;
        private readonly IAllCommonRepository _allCommonRepository;
        private readonly IUserLoginRepository _userLoginRepository;
        private readonly LocalizationServices _localization;
        private readonly IAllCommonService _allCommonService;
        public TensileService(ITensileRepository TensileRepository, IAllCommonRepository allCommonRepository, IUserLoginRepository userLoginRepository, LocalizationServices localization, IAllCommonService allCommonService)
        {
            _tensileRepository = TensileRepository;
            _allCommonRepository = allCommonRepository;
            _userLoginRepository = userLoginRepository;
            _localization =  localization;
            _allCommonService = allCommonService;
        }
        public int GetUserLocationById(int userID)
        {
            return _tensileRepository.GetUserLocationById(userID);
        }
        public TensileDetails GetTensileDetailsByChuuban(string chubaan, short? gyouban, string culture_Name)
        {
            if (culture_Name == "en")
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
            }
            else
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("ja-JP");
            }
            var obj = _tensileRepository.GetTensileDetailsByChuuban(chubaan, gyouban);

            if (obj != null)
            {
                if (obj.PP_STD_DEL_PERIOD > 0)
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

                if (!string.IsNullOrEmpty(obj.PP_COIL_DIA_WINDING))
                {
                    obj.PP_COIL_DIA_WINDING = UIUtils.GetCoilDiaWindingKubunValue(Convert.ToString(obj.PP_COIL_DIA_WINDING));
                }
                obj.DECISION_OK = Convert.ToInt16(UIUtils.GetResultKubunKey(_localization.GetLocalizedHtmlString("lbl_OK")));
                var messageList = _tensileRepository.GetMessageDetailsByProcessPlanCD(Convert.ToInt64(obj.PP_PROCESS_PLAN_CD), true);
                obj.MESSAGE_LIST = messageList;

                obj.PP_ELONGATION = Helper.GetProductPatternFormat(obj.ELONGATION_PATTERN1.HasValue ? obj.ELONGATION_PATTERN1.Value : 0, obj.ELONGATION_PATTERN1_MIN_MAX,Convert.ToDecimal(obj.ELONGATION_PATTERN2_MIN), Convert.ToDecimal(obj.ELONGATION_PATTERN2_MAX), obj.ELONGATION_PATTERN3_ACTUAL);
                obj.PP_TENSILE_STRENGTH = Helper.GetProductPatternFormat(Convert.ToSingle(obj.TENSILE_STRN_PATTERN1), obj.TENSILE_STRN_PATTERN1_MIN_MAX, obj.TENSILE_STRN_PATTERN2_MIN, obj.TENSILE_STRN_PATTERN2_MAX, obj.TENSILE_STRN_PATTERN3_ACTUAL);
                obj.PP_HARDNESS = Helper.GetHardnessProductPatternFormat(obj.HARDNESS_PATTERN1_CODE, obj.HARDNESS_PATTERN1, obj.HARDNESS_PATTERN1_MIN_MAX, obj.HARDNESS_PATTERN2_CODE, obj.HARDNESS_PATTERN2_MIN, obj.HARDNESS_PATTERN2_MAX, obj.HARDNESS_PATTERN3_ACTUAL);
                obj.PP_DECARB = Helper.GetDecarbProductPatternFormat(obj.DECARB_1, Convert.ToSingle(obj.DECARB_VAL_1), obj.DECARB_2, Convert.ToSingle(obj.DECARB_VAL_2), obj.DECARB_3, Convert.ToSingle(obj.DECARB_VAL_3));
                obj.PP_YEILD = Helper.GetProductPatternFormat(Convert.ToSingle(obj.YEILD_PATTERN1), obj.YEILD_PATTERN1_MIN_MAX, obj.YEILD_PATTERN2_MIN, obj.YEILD_PATTERN2_MAX, obj.YEILD_PATTERN3_ACTUAL);
                obj.PP_PRESS = Helper.GetProductPatternFormat(Convert.ToSingle(obj.PRESS_PATTERN1), obj.PRESS_PATTERN1_MIN_MAX, obj.PRESS_PATTERN2_MIN, obj.PRESS_PATTERN2_MAX, obj.PRESS_PATTERN3_ACTUAL);

            }

            return obj;
        }
        public List<NameValue> GetOperatorName(string name, int operatorCD)
        {


            return _allCommonRepository.GetOperatorNameList(name, operatorCD);
        }

        public List<NameValue> GetJISTestList()

        {
            var JISTestList = new List<NameValue>();
            JISTestList = _tensileRepository.GetJISList();

            return JISTestList;
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
        public List<NameValue> GetDimShapeList()
        {
            var _DieShapeList = UIUtils.GetDieShape();
            return _DieShapeList;
        }
        public TensileDetails InsertTensileDetails(TensileDetails request)
        {
            if (request.TEST_SPECIMEN_CD ==0)
            {
                request.TEST_SPECIMEN_CD = null;

            }
            if (request.TS_ACT_DIM_SHAPE == "0")
            {
                request.TS_ACT_DIM_SHAPE = null;

            }
            

            TensileDetails result = new TensileDetails();

            using (TransactionScope transaction = new TransactionScope())
            {

                if (request.USER_ID != null && request.USER_ID > 0)
            {
                request.INPUT_LOCATION_CD = _tensileRepository.GetUserLocationById(request.USER_ID.Value);
                request.LOCATION_CD = request.INPUT_LOCATION_CD;
            }
            if (request.MSG_CD == Constants.NullValue.INT && !string.IsNullOrEmpty(request.REMARKS))
            {
                MessageDetails messageDetails = new MessageDetails();
                messageDetails.PROCESS_PLAN_CD = Convert.ToInt64(request.PROCESS_PLAN_CD);

                messageDetails.PROCESS_KUBUN_ID = Convert.ToInt16(UIUtils.GetProcessKubun(_localization.GetLocalizedHtmlString("lbl_ProcessMechanical_Insp_Hardness_Tensile")));
                messageDetails.OPERATOR_CD = Convert.ToInt64(request.TS_TEST_OPERATOR_CD);
                messageDetails.MSG_CONTENTS = request.REMARKS;
                messageDetails.USER_ID = Convert.ToInt32(request.USER_ID);
                //     messageDetails.LOCATION_CD = Convert.ToInt32(request.LOCATION_CD);
                messageDetails.INPUT_LOCATION_CD = Convert.ToInt32(request.INPUT_LOCATION_CD);


                messageDetails = _allCommonRepository.MessageDetailsInsert(messageDetails);

                if (messageDetails != null)
                {
                    request.MSG_CD = Convert.ToInt32(messageDetails.MSG_CD);
                }
            }
            request.PROCESS_KUBUN_CD = Convert.ToInt16(UIUtils.GetProcessKubun(_localization.GetLocalizedHtmlString("lbl_ProcessMechanical_Insp_Hardness_Tensile")));

            result = _tensileRepository.InsertTensileDetails(request);

                //if (request.TS_NEXT_PROCESS != 999)
                //{

                //    WipStatusDetails wipDetails = new WipStatusDetails(); //code commited on 09/10/2025 for without taba requirment data no need to save into TRN_WIP_STATUS table

                //    wipDetails = new WipStatusDetails();
                //    wipDetails.PROCESS_PLAN_CD = request.PROCESS_PLAN_CD;
                //    wipDetails.LOCATION_CD = Convert.ToInt16(request.LOCATION_CD);
                //    wipDetails.MSG_CD = null;
                //    wipDetails.FINISHED_BARS = null;
                //    wipDetails.UNIT = null;
                //    wipDetails.FINISHED_QTY = null;
                //    wipDetails.PROCESS_START_DATE = request.TS_TEST_DATE;
                //    wipDetails.PROCESS_END_DATE = request.TS_TEST_DATE;
                //    wipDetails.PROCESS_KUBUN_CD = Convert.ToInt32(request.PROCESS_KUBUN_CD);
                //    wipDetails.PRE_PROSESS_DECISION = request.TS_DECISION;
                //    wipDetails.NEXT_PROCESS =Convert.ToInt16(request.TS_NEXT_PROCESS);
                //    wipDetails.START_OPERATOR_CD = request.TS_TEST_OPERATOR_CD;
                //    wipDetails.END_OPERATOR_CD = null;
                //    wipDetails.TABA_NO = Convert.ToInt16(request.DETAIL_NO);
                //    wipDetails.NIFUDA_CD = null;

                //    _allCommonRepository.InsertWIPStatusData(wipDetails);
                //}
                if (result.PP_COLOR_ID > 0)
            {
                result.PP_COLOR = Utils.GetColorName(Convert.ToInt16(result.PP_COLOR_ID));
            }

            if (!string.IsNullOrEmpty(result.PP_COIL_DIA_WINDING))
            {
                result.PP_COIL_DIA_WINDING = UIUtils.GetCoilDiaWindingKubunValue(Convert.ToString(result.PP_COIL_DIA_WINDING));
            }

                transaction.Complete();

            }
            //var messageList = _CommonRepository.GetMessageDetailsByProcessPlanCD(Convert.ToInt64(result.PP_PROCESS_PLAN_CD), true);
            //result.DW_MESSAGE_LIST = messageList;

            return result;
        }
        public List<ThabaDetails> GetThabaDetailsByChuuban(string chuuban, short? gyouban, long tensileCD)
        {


            var result = _tensileRepository.GetThabaDetailsByChuuban(chuuban, gyouban, tensileCD);
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
        public ThabaDetails GetThabaDetailsByThabaNumber(long processPlan_cd, short tabaNo)
        {
            var result = _tensileRepository.GetThabaDetailsByThabaNumber(processPlan_cd, tabaNo);
            return result;
        }

        public TensileDetails GetFurnanceNo(string chuuban, short? gyouban,int detail_No)
        {
            return _tensileRepository.GetFurnanceNo(chuuban, gyouban, detail_No);

        }
        public List<TensileDetails> GetTensileList(GridSearchFields gridSearchRequest)
        {
            short Location_CD = Convert.ToInt16(_userLoginRepository.GetUserLocationById(Convert.ToInt32(gridSearchRequest.USER_ID)));
            gridSearchRequest.LOCATION_CD = Location_CD;

            List<TensileDetails> result = new List<TensileDetails>();
            List<SearchField> searchFields = ComposeSearchCriteria(gridSearchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);
            result = _tensileRepository.GetTensileLoadList(searchFields, selectedSortCriteria, gridSearchRequest.LOCATION_CD);
            foreach (TensileDetails record in result)
            {
                record.TS_DECISION_STRING = UIUtils.GetResultKubunValue(Convert.ToString(record.TS_DECISION));
            }
            return result;
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
                    //  FieldName = "PP.CHUUBAN",
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
                    SearchText1 =Convert.ToString(gridSearchRequest.PROD_LENGTH_MIN),
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
                        FieldName = "TS.TS_TEST_DATE",
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
                        FieldName = "TS.TS_TEST_DATE",
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
                    FieldName = "TS.TS_TEST_DATE",
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
        public TensileDetails GetTensileDetailsByCD(long tensileCD, string chuuban, long gyouban,string culture_Name)
        {
            if (culture_Name == "en")
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
            }
            else
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("ja-JP");
            }
            var _tensileDetails = _tensileRepository.GetTensileDetailsByCD(tensileCD, chuuban, gyouban);
            if (_tensileDetails != null)
            {
                if (_tensileDetails.PP_STD_DEL_PERIOD >= 0)
                {
                    _tensileDetails.PP_STD_DEL_PERIOD_STRING = _tensileDetails.PP_STD_DEL_PERIOD + " " + _localization.GetLocalizedHtmlString("lbl_Day");
                }

                if (_tensileDetails.PP_UNIT > 0)
                {
                    _tensileDetails.PP_NO_OF_UNITS_DECODE = Convert.ToDecimal(_tensileDetails.PP_NO_OF_UNITS).ToString("0.####") + UIUtils.GetProductUnitKubunValue(Convert.ToString(_tensileDetails.PP_UNIT));
                }
                if (_tensileDetails.PP_UNIT > 0)
                {
                    _tensileDetails.PP_NO_OF_UNITS_DECODE = Convert.ToDecimal(_tensileDetails.PP_NO_OF_UNITS).ToString("0.####") + UIUtils.GetProductUnitKubunValue(Convert.ToString(_tensileDetails.PP_UNIT));
                }
                if (_tensileDetails.PP_UNIT_WT_KUBUN != null)
                {
                    _tensileDetails.PP_UNIT_WT_KUBUN_STRING = Utils.GetUnitWeigth(Convert.ToInt32(_tensileDetails.PP_UNIT_WT_KUBUN), Convert.ToInt32(_tensileDetails.PP_UNIT_WT), Convert.ToInt32(_tensileDetails.PP_UNIT_WT_MIN), Convert.ToInt32(_tensileDetails.PP_UNIT_WT_MAX));

                    if (_tensileDetails.PP_UNIT_WT_KUBUN == 3)
                    {
                        _tensileDetails.PP_UNIT_WT_KUBUN_STRING = Convert.ToString(_tensileDetails.PP_UNIT_WT) + _localization.GetLocalizedHtmlString("lbl_Hone");
                    }


                }
                //if (_drawingDetails.DW_WINDING_SHAPE > 0)
                //{
                //    _drawingDetails.DW_WINDING_SHAPE1 = Convert.ToDecimal(_drawingDetails.DW_WINDING_SHAPE1).ToString("0.####") + UIUtils.GetColdDrawingWindingShape(Convert.ToString(_drawingDetails.DW_WINDING_SHAPE));
                //}

                if (_tensileDetails.MAGARI_KUBUN > 0)
                {
                    _tensileDetails.PP_MAGARI_BEND_TOL = Utils.GetMagariBendTolSpan(UIUtils.Instance.GetMagariKubunValue(Convert.ToString(_tensileDetails.MAGARI_KUBUN)), Convert.ToDecimal(_tensileDetails.MAGARI_BEND_TOL), Convert.ToInt32(_tensileDetails.MAGARI_SPAN));  //Convert.ToDecimal(_stamppingDetails.PP_NO_OF_UNITS).ToString("0.####") + UIUtils.Instance.GetProductUnitKubunValue(Convert.ToString(_stamppingDetails.PP_UNIT));
                }

                //if (_tensileDetails.PP_COLOR_ID > 0)
                //{
                //    _tensileDetails.PP_COLOR = UIUtils.GetColorKubunValue(Convert.ToString(_tensileDetails.PP_COLOR_ID));   // Utils.GetColorName(Convert.ToInt16(_stamppingDetails.PP_COLOR_ID));

                //}
                if (_tensileDetails.PP_COIL_DIA_WINDING != null)
                {
                    _tensileDetails.PP_COIL_DIA_WINDING = UIUtils.GetCoilDiaWindingKubunValue(Convert.ToString(_tensileDetails.PP_COIL_DIA_WINDING)); //  Utils.GetCoilDiaWindingValue(Convert.ToInt16(_stamppingDetails.PP_COIL_DIA_WINDING));
                }
                _tensileDetails.DECISION_OK = Convert.ToInt16(UIUtils.GetResultKubunKey(_localization.GetLocalizedHtmlString("lbl_OK")));
                _tensileDetails.PP_ELONGATION = Helper.GetProductPatternFormat(_tensileDetails.ELONGATION_PATTERN1.HasValue ? _tensileDetails.ELONGATION_PATTERN1.Value : 0, _tensileDetails.ELONGATION_PATTERN1_MIN_MAX, Convert.ToDecimal(Convert.ToDecimal(_tensileDetails.ELONGATION_PATTERN2_MIN)), Convert.ToDecimal(_tensileDetails.ELONGATION_PATTERN2_MAX), _tensileDetails.ELONGATION_PATTERN3_ACTUAL);
                _tensileDetails.PP_TENSILE_STRENGTH = Helper.GetProductPatternFormat(Convert.ToSingle(_tensileDetails.TENSILE_STRN_PATTERN1), _tensileDetails.TENSILE_STRN_PATTERN1_MIN_MAX, _tensileDetails.TENSILE_STRN_PATTERN2_MIN, _tensileDetails.TENSILE_STRN_PATTERN2_MAX, _tensileDetails.TENSILE_STRN_PATTERN3_ACTUAL);
                _tensileDetails.PP_HARDNESS = Helper.GetHardnessProductPatternFormat(_tensileDetails.HARDNESS_PATTERN1_CODE, _tensileDetails.HARDNESS_PATTERN1, _tensileDetails.HARDNESS_PATTERN1_MIN_MAX, _tensileDetails.HARDNESS_PATTERN2_CODE, _tensileDetails.HARDNESS_PATTERN2_MIN, _tensileDetails.HARDNESS_PATTERN2_MAX, _tensileDetails.HARDNESS_PATTERN3_ACTUAL);
                _tensileDetails.PP_DECARB = Helper.GetDecarbProductPatternFormat(_tensileDetails.DECARB_1, Convert.ToSingle(_tensileDetails.DECARB_VAL_1), _tensileDetails.DECARB_2, Convert.ToSingle(_tensileDetails.DECARB_VAL_2), _tensileDetails.DECARB_3, Convert.ToSingle(_tensileDetails.DECARB_VAL_3));
                _tensileDetails.PP_YEILD = Helper.GetProductPatternFormat(Convert.ToSingle(_tensileDetails.YEILD_PATTERN1), _tensileDetails.YEILD_PATTERN1_MIN_MAX, _tensileDetails.YEILD_PATTERN2_MIN, _tensileDetails.YEILD_PATTERN2_MAX, _tensileDetails.YEILD_PATTERN3_ACTUAL);
                _tensileDetails.PP_PRESS = Helper.GetProductPatternFormat(Convert.ToSingle(_tensileDetails.PRESS_PATTERN1), _tensileDetails.PRESS_PATTERN1_MIN_MAX, _tensileDetails.PRESS_PATTERN2_MIN, _tensileDetails.PRESS_PATTERN2_MAX, _tensileDetails.PRESS_PATTERN3_ACTUAL);

                return _tensileDetails;
            }
            else
            {
                return new TensileDetails();
            }
        }
        public long UpdateTensileDetails(TensileDetails request)
        {
            long msgCd = 0;
            bool deleteMsg = false;
            if (request.TEST_SPECIMEN_CD == 0)
            {
                request.TEST_SPECIMEN_CD = null;
            }

            if (request.TS_ACT_DIM_SHAPE == "0")
            {
                request.TS_ACT_DIM_SHAPE = null;
            }

            TensileDetails result = new TensileDetails();
            if (request.USER_ID != null && request.USER_ID > 0)
            {
                request.INPUT_LOCATION_CD = _userLoginRepository.GetUserLocationById(request.USER_ID.Value);
                request.LOCATION_CD = request.INPUT_LOCATION_CD;
            }
           
            using (TransactionScope transaction = new TransactionScope())
            {
                MessageDetails messageDetails = new MessageDetails();
                messageDetails.PROCESS_PLAN_CD = Convert.ToInt64(request.PROCESS_PLAN_CD);

                messageDetails.PROCESS_KUBUN_ID = Convert.ToInt16(UIUtils.GetProcessKubun(_localization.GetLocalizedHtmlString("lbl_ProcessMechanical_Insp_Hardness_Tensile")));
                messageDetails.OPERATOR_CD = Convert.ToInt64(request.TS_TEST_OPERATOR_CD);
                messageDetails.MSG_CONTENTS = request.REMARKS;
                messageDetails.MSG_CD = Convert.ToInt32(request.MSG_CD);
                messageDetails.USER_ID = Convert.ToInt32(request.USER_ID);
                messageDetails.INPUT_LOCATION_CD = Convert.ToInt32(request.INPUT_LOCATION_CD);

                if (request.MSG_CD == Constants.NullValue.INT && !string.IsNullOrEmpty(request.REMARKS))
                {
                   
                    messageDetails = _allCommonRepository.MessageDetailsInsert(messageDetails);

                    if (messageDetails.MSG_CD != Constants.NullValue.INT && messageDetails.MSG_CD > 0)
                    {
                        request.MSG_CD = Convert.ToInt32(messageDetails.MSG_CD);
                    }
                }
                // If REMARKS was already present, and it is updated,  
                // then UPDATE the TRN_MESSAGE
                else if ((request.MSG_CD > 0 && request.REMARKS != null))
                {
                     messageDetails.MSG_CD= Convert.ToInt32(request.MSG_CD);
                    _allCommonRepository.MessageDetailsUpdate(messageDetails);

                }
                // If REMARKS was already present, but it is delete now,
                // then DELETE the TRN_MESSAGE
                else if (request.MSG_CD > 0 && request.REMARKS == null)
                {
                    long messageCD = Convert.ToInt64(request.MSG_CD);
                    request.MSG_CD = Convert.ToInt32(Constants.NullValue.LONG);
                    if (request.MSG_CD == 0)
                    {
                        request.MSG_CD = null;
                    }
                    msgCd = _tensileRepository.UpdateTensileDetails(request);
                    _allCommonRepository.MessageDetailsDelete(messageCD);
                    deleteMsg = true;
                }
                if (deleteMsg == false)
                {
                    msgCd = _tensileRepository.UpdateTensileDetails(request);
                }
                //WipStatusDetails wipDetails = new WipStatusDetails();          //code commited on 09/10/2025 for without taba requirment data no need to save into TRN_WIP_STATUS table

                //wipDetails = new WipStatusDetails();
                //wipDetails.PROCESS_PLAN_CD = request.PROCESS_PLAN_CD;
                //wipDetails.LOCATION_CD = Convert.ToInt16(request.LOCATION_CD);
                //wipDetails.MSG_CD = request.MSG_CD;
                //wipDetails.FINISHED_BARS = null;
                //wipDetails.UNIT = null;
                //wipDetails.FINISHED_QTY = null;
                //wipDetails.PROCESS_START_DATE = request.TS_TEST_DATE;
                //wipDetails.PROCESS_END_DATE = null;
                //wipDetails.PROCESS_KUBUN_CD = Convert.ToInt32(request.PROCESS_KUBUN_CD);
                //wipDetails.PRE_PROSESS_DECISION = request.TS_DECISION;
                //wipDetails.NEXT_PROCESS = Convert.ToInt16(request.TS_NEXT_PROCESS);
                //wipDetails.START_OPERATOR_CD = request.TS_TEST_OPERATOR_CD;
                //wipDetails.END_OPERATOR_CD = null;
                //wipDetails.TABA_NO = Convert.ToInt16(request.DETAIL_NO);
                //wipDetails.NIFUDA_CD = null;

                //_allCommonRepository.InsertWIPStatusData(wipDetails);

                transaction.Complete();
            }
            return msgCd;
        }
        public string DeleteTensileDetails(TensileDetails request)
        {
            var result = "";
            using (TransactionScope transaction = new TransactionScope())
            {

                if (request.MI_TS_CD > 0)
            {
                    result = _tensileRepository.DeleteTensileDetails(request.MI_TS_CD);
            }
            if (request.MSG_CD > 0 && result == "1")
            {
                _allCommonRepository.MessageDetailsDelete(Convert.ToInt32(request.MSG_CD));
            }
                //_allCommonRepository.DeleteWIPStatusData(Convert.ToInt64(request.PROCESS_PLAN_CD), Convert.ToInt16(request.DETAIL_NO));             //code commited on 09/10/2025 for without taba requirment data no need to save into TRN_WIP_STATUS table

                transaction.Complete();
            }
            return result;
        }


        public List<TensileDetails> GetMECHTensileOnFactoryDailyWorkStatus(GridSearchFields gridSearchRequest)
        {
            List<SearchField> searchFields = ComposeSearchCriteriaforFactoryDailyWorkStatus(gridSearchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);
            int rowLimit = gridSearchRequest.ROW_LIMIT;
            var result = _tensileRepository.GetMECHTensileOnFactoryDailyWorkStatus(searchFields, selectedSortCriteria, rowLimit);            
            return result;
        }
        public List<TensileDetails> GetMECHTensileOnDailyProcessStatus(GridSearchFields gridSearchRequest)
        {
            List<SearchField> searchFields = ComposeSearchCriteriaforDailyProcessAnalysisStatus(gridSearchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);
            var result = _tensileRepository.GetMECHTensileOnDailyProcessStatus(searchFields, selectedSortCriteria, gridSearchRequest);           
            return result;
        }

        private List<SearchField> ComposeSearchCriteriaforFactoryDailyWorkStatus(GridSearchFields gridSearchRequest)
        {
            var searchFields = new List<SearchField>();           
            if (gridSearchRequest.LOCATION_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "TENSILE.INPUT_LOCATION_CD",//change 2025/12/11 as per client requirement
                    SearchType = SearchType.Number,
                    SearchText1 =Convert.ToString(gridSearchRequest.LOCATION_CD)
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.CUSTOMER_CD >  0)
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
                    FieldName = "TENSILE.TS_TEST_OPERATOR_CD",
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
                    SearchText1 ="'" + gridSearchRequest.PROD_SPEC_NO2 +"'"
                };
                searchFields.Add(searchField);
            }          
            if (string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE2))
            {
                if (!string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE1))
                {
                    var searchField = new SearchField
                    {
                        FieldName = "TENSILE.TS_TEST_DATE",
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
                        FieldName = "TENSILE.TS_TEST_DATE",
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
                    FieldName = "TENSILE.TS_TEST_DATE",
                    SearchType = SearchType.DateTime,
                    SearchText1 = gridSearchRequest.INSPECTION_DATE1,
                    SearchText2 = gridSearchRequest.INSPECTION_DATE2
                };
                searchFields.Add(searchField);
            }


            return searchFields;
        }
        private List<SearchField> ComposeSearchCriteriaforDailyProcessAnalysisStatus(GridSearchFields gridSearchRequest)
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
                    SearchText1 =Convert.ToString( gridSearchRequest.GYOUBAN)
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
                    SearchText1 =gridSearchRequest.PROD_VER
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
                    SearchText1 =Convert.ToString(gridSearchRequest.PROD_DIM_W)
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
                    FieldName = "MITENS.TS_TEST_DATE",
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
                    FieldName = "MITENS.TS_FURNACE_NO",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.FURNACE_NO
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.DECISION > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "MITENS.TS_DECISION",
                    SearchType = SearchType.Text,
                    SearchText1 = Convert.ToString(gridSearchRequest.DECISION)
                };
                searchFields.Add(searchField);
            }

            return searchFields;
        }

              
        public int GetRowCountForMechTensileFactory(GridSearchFields gridSearchRequest)
        {
            List<SearchField> searchFields = ComposeSearchCriteriaforFactoryDailyWorkStatus(gridSearchRequest);
            int count = _tensileRepository.GetRowCountForMechTensileFactory(searchFields);
            return count;
        }
        public int GetRowCountForMechTensileDailyAnalysis(GridSearchFields gridSearchRequest)
        {
            List<SearchField> searchFields = ComposeSearchCriteriaforDailyProcessAnalysisStatus(gridSearchRequest);
            int count = _tensileRepository.GetRowCountForMechTensileDailyAnalysis(searchFields);
            return count;
        }
        public decimal GetJISTestMagnification(int testSpecCd)
        {
            decimal testSpecCdMagnification = _tensileRepository.GetJISTestMagnification(testSpecCd);
            return testSpecCdMagnification;
        }
        public List<TensileDetails> GetFICombinedDecisionDetails(string chuuban, short? gyouban)
        {
            var result = _tensileRepository.GetFICombinedDecisionDetails(chuuban, gyouban);
            return result;
        }
        public void UpdateFICombinedDecision(short tensileDecision, long processPlanCd)
        {

            using (TransactionScope transaction = new TransactionScope())
            {
                _tensileRepository.UpdateFICombinedDecision(tensileDecision, processPlanCd);
                transaction.Complete();
            }
        }
        public string ValidateTensileDetailsOnUpdateBtn(long MITensileCD)
        {
            var result = _tensileRepository.ValidateTensileDetailsOnUpdateBtn(MITensileCD);
            return result;
        }
    }
}
