using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using TSWebApplication.Application.IServices;
using TSWebApplication.Infrastructure.IRepositories;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Infrastructure.Repositories;
using TSWebApplication.Domain.Resources;
using System.Data;
using System.Globalization;
using System.Linq;


namespace TSWebApplication.Application.Services
{
    public class ColdDrawingWireDrawingService : IColdDrawingWireDrawingService
    {
        private readonly IColdDrawingWireDrawingRepository _ColdDrawingWireDrawingRepository;
        private readonly IAllCommonRepository _CommonRepository;
        private readonly IUserLoginRepository _UserLoginRepository;
        private readonly LocalizationServices _localization;
        private readonly IAllCommonService _allCommonService;

        public ColdDrawingWireDrawingService(IColdDrawingWireDrawingRepository ColdDrawingWireDrawingRepository, IAllCommonRepository allCommonRepository, IUserLoginRepository userLoginRepository, LocalizationServices localization, IAllCommonService allCommonService)
        {
            _ColdDrawingWireDrawingRepository = ColdDrawingWireDrawingRepository;
            _CommonRepository = allCommonRepository;
            _UserLoginRepository = userLoginRepository;
            _localization = localization;
            _allCommonService = allCommonService;
        }

        //public List<ColdDrawingWireDrawingDetails> GetCDList(ColdDrawingWireDrawingGridSearchRequest gridSearchRequest)
        //{
        //    int Location_CD = _UserLoginRepository.GetUserLocationById(Convert.ToInt32(gridSearchRequest.userId));
        //    gridSearchRequest.locationCD = Location_CD;

        //    List<ColdDrawingWireDrawingDetails> result = new List<ColdDrawingWireDrawingDetails>();
        //    List<SearchField> searchFields = ComposeSearchCriteria(gridSearchRequest);
        //    string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.sortFields);
        //    result = _ColdDrawingWireDrawingRepository.GetColdDrawingWireDrawingList(searchFields, selectedSortCriteria, gridSearchRequest.locationCD);
        //    foreach (ColdDrawingWireDrawingDetails record in result)
        //    {
        //        record.DW_DECISION_STRING = UIUtils.GetResultKubunValue(Convert.ToString(record.DW_DECISION));
        //    }
        //    return result;
        //}
      
        public List<NameValue> GetExteriorConditionList()
        {
            var _conditionList = UIUtils.GetExteriorConditionKubunGetKubun();
            return _conditionList;
        }
        public List<NameValue> GetMDustList()
        {
            var _MDustList = UIUtils.GetMagneticDust();
            return _MDustList;
        }
        public List<NameValue> GetDiceInspStartList()
        {
            var _DiceStartList = UIUtils.GetDiceInspStart();
            return _DiceStartList;
        }

        public List<NameValue> GetDiceInspEndList()
        {
            var _DiceEndList = UIUtils.GetDiceInspEnd();
            return _DiceEndList;
        }
        public List<NameValue> GetwindingShapeList()
        {
            var _ShapeList = UIUtils.GetwindingShape();
            return _ShapeList;
        }
        public List<NameValue> GetDieDiaShapeList()
        {
            var _DieShapeList = UIUtils.GetDieShape();
            return _DieShapeList;
        }
        public List<NameValue> GetDecisionList()
        {
            var _DecisionList = UIUtils.GetResultKubun();
            return _DecisionList;
        }
        public List<ThabaDetails> GetThabaDetails(string chuuban, short? gyuban)
        {
            var result= _ColdDrawingWireDrawingRepository.GetThabaDetails(chuuban, gyuban);

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


        public List<NameValue> GetFilteredEmployeeNameList(string searchText, bool addEmpty)
        {
            var customerList = new List<NameValue>();
            customerList = _ColdDrawingWireDrawingRepository.GetFilteredEmployeeNameList(searchText, addEmpty);
            if (customerList != null && addEmpty)
            {
                var defaulItem = new NameValue { CD = 0, Name = string.Empty };
                customerList.Insert(0, defaulItem);
            }
            return customerList;
        }
        public List<NameValue> GetOperatorName(string name, int operatorCD)
        {
            return _CommonRepository.GetOperatorNameList(name, operatorCD);
        }
        public ColdDrawingWireDrawingDetails GetColdDrawingWireDrawingDetailsByDrawingCD(long drawingCD, string cultureName)
        {
            var _drawingDetails = _ColdDrawingWireDrawingRepository.GetColdDrawingWireDrawingDetailsByDrawingCD(drawingCD);
            if (cultureName == "en")
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
            }
            else
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("ja-JP");
            }
            if (_drawingDetails != null)
            {
                if (_drawingDetails.PP_STD_DEL_PERIOD >= 0)
                {
                    _drawingDetails.PP_STD_DEL_PERIOD_STRING = _drawingDetails.PP_STD_DEL_PERIOD + " " + _localization.GetLocalizedHtmlString("lbl_Day");
                }

                if (_drawingDetails.PP_UNIT > 0)
                {
                    _drawingDetails.PP_NO_OF_UNITS_DECODE = Convert.ToDecimal(_drawingDetails.PP_NO_OF_UNITS).ToString("0.####") + UIUtils.GetProductUnitKubunValue(Convert.ToString(_drawingDetails.PP_UNIT));
                }
                if (_drawingDetails.DW_WINDING_SHAPE > 0)
                {
                    _drawingDetails.DW_WINDING_SHAPE1 = Convert.ToDecimal(_drawingDetails.DW_WINDING_SHAPE1).ToString("0.####") + UIUtils.GetColdDrawingWindingShape(Convert.ToString(_drawingDetails.DW_WINDING_SHAPE));
                }

                if (_drawingDetails.MAGARI_KUBUN > 0)
                {
                    _drawingDetails.PP_MAGARI_BEND_TOL = Utils.GetMagariBendTolSpan(UIUtils.Instance.GetMagariKubunValue(Convert.ToString(_drawingDetails.MAGARI_KUBUN)), Convert.ToDecimal(_drawingDetails.MAGARI_BEND_TOL), Convert.ToInt32(_drawingDetails.MAGARI_SPAN));  //Convert.ToDecimal(_stamppingDetails.PP_NO_OF_UNITS).ToString("0.####") + UIUtils.Instance.GetProductUnitKubunValue(Convert.ToString(_stamppingDetails.PP_UNIT));
                }

                //if (_drawingDetails.PP_COLOR_ID > 0)
                //{
                //    _drawingDetails.PP_COLOR = UIUtils.GetColorKubunValue(Convert.ToString(_drawingDetails.PP_COLOR_ID));   // Utils.GetColorName(Convert.ToInt16(_stamppingDetails.PP_COLOR_ID));
                //}
                if (_drawingDetails.COIL_DIA_WINDING > 0)
                {
                    _drawingDetails.PP_COIL_DIA_WINDING = UIUtils.GetCoilDiaWindingKubunValue(Convert.ToString(_drawingDetails.COIL_DIA_WINDING));
                }
                if (_drawingDetails.PP_UNIT_WT_KUBUN > 0)
                {
                    _drawingDetails.PP_UNIT_WT_KUBUN_STRING = Utils.GetUnitWeigth(Convert.ToInt32(_drawingDetails.PP_UNIT_WT_KUBUN), Convert.ToInt32(_drawingDetails.PP_UNIT_WT), Convert.ToInt32(_drawingDetails.UNIT_WT_MIN), Convert.ToInt32(_drawingDetails.UNIT_WT_MAX));

                    if (_drawingDetails.PP_UNIT_WT_KUBUN == 3)
                    {
                        _drawingDetails.PP_UNIT_WT_KUBUN_STRING = Convert.ToString(_drawingDetails.PP_UNIT_WT) + _localization.GetLocalizedHtmlString("lbl_Hone");
                    }


                }
                _drawingDetails.DECISION_OK = Convert.ToInt16(UIUtils.GetResultKubunKey(_localization.GetLocalizedHtmlString("lbl_OK")));
                return _drawingDetails;
            }
            else
            {
                return new ColdDrawingWireDrawingDetails();
            }
        }

        public ColdDrawingWireDrawingDetails GetColdDrawingWireDrawingDetailsByChuuban(string chubaan, short? gyouban, int? machineCD, string cultureName)
        {
            if (cultureName == "en")
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
            }
            else
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("ja-JP");
            }

            var obj = _ColdDrawingWireDrawingRepository.GetColdDrawingWireDrawingDetailsByChuuban(chubaan, gyouban, machineCD);

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
                if (obj.MAGARI_KUBUN > 0)
                {
                    obj.PP_MAGARI_BEND_TOL = Utils.GetMagariBendTolSpan(UIUtils.Instance.GetMagariKubunValue(Convert.ToString(obj.MAGARI_KUBUN)), Convert.ToDecimal(obj.MAGARI_BEND_TOL), Convert.ToInt32(obj.MAGARI_SPAN));  //Convert.ToDecimal(obj.PP_NO_OF_UNITS).ToString("0.####") + UIUtils.Instance.GetProductUnitKubunValue(Convert.ToString(obj.PP_UNIT));
                }
                if (obj.PP_UNIT_WT_KUBUN > 0)
                {
                    obj.PP_UNIT_WT_KUBUN_STRING = Utils.GetUnitWeigth(Convert.ToInt32(obj.PP_UNIT_WT_KUBUN), Convert.ToInt32(obj.PP_UNIT_WT), Convert.ToInt32(obj.UNIT_WT_MIN), Convert.ToInt32(obj.UNIT_WT_MAX));

                    if (obj.PP_UNIT_WT_KUBUN == 3)
                    {
                        obj.PP_UNIT_WT_KUBUN_STRING = Convert.ToString(obj.PP_UNIT_WT) + _localization.GetLocalizedHtmlString("lbl_Hone");
                    }


                }

                //if (obj.PP_COLOR_ID > 0)
                //{
                //    obj.PP_COLOR = UIUtils.GetColorKubunValue(Convert.ToString(obj.PP_COLOR_ID));
                //}
                obj.DECISION_OK = Convert.ToInt16(UIUtils.GetResultKubunKey(_localization.GetLocalizedHtmlString("lbl_OK")));
                obj.PP_COIL_DIA_WINDING = UIUtils.GetCoilDiaWindingKubunValue(Convert.ToString(obj.COIL_DIA_WINDING));

                var messageList = _CommonRepository.GetMessageDetailsByProcessPlanCD(Convert.ToInt64(obj.PP_PROCESS_PLAN_CD), true);
                obj.DW_MESSAGE_LIST = messageList;

                obj.PP_ELONGATION = Helper.GetProductPatternFormat(obj.ELONGATION_PATTERN1.HasValue ? obj.ELONGATION_PATTERN1.Value : 0, obj.ELONGATION_PATTERN1_MIN_MAX, Convert.ToDecimal(obj.ELONGATION_PATTERN2_MIN), Convert.ToDecimal(obj.ELONGATION_PATTERN2_MAX), obj.ELONGATION_PATTERN3_ACTUAL);
                obj.PP_TENSILE_STRENGTH = Helper.GetProductPatternFormat(Convert.ToSingle(obj.TENSILE_STRN_PATTERN1), obj.TENSILE_STRN_PATTERN1_MIN_MAX, obj.TENSILE_STRN_PATTERN2_MIN, obj.TENSILE_STRN_PATTERN2_MAX, obj.TENSILE_STRN_PATTERN3_ACTUAL);
                obj.PP_HARDNESS = Helper.GetHardnessProductPatternFormat(obj.HARDNESS_PATTERN1_CODE, obj.HARDNESS_PATTERN1, obj.HARDNESS_PATTERN1_MIN_MAX, obj.HARDNESS_PATTERN2_CODE, obj.HARDNESS_PATTERN2_MIN, obj.HARDNESS_PATTERN2_MAX, obj.HARDNESS_PATTERN3_ACTUAL);
                obj.PP_DECARB = Helper.GetDecarbProductPatternFormat(obj.DECARB_1,Convert.ToSingle(obj.DECARB_VAL_1), obj.DECARB_2, Convert.ToSingle(obj.DECARB_VAL_2), obj.DECARB_3, Convert.ToSingle(obj.DECARB_VAL_3));
                obj.PP_YEILD = Helper.GetProductPatternFormat(Convert.ToSingle(obj.YEILD_PATTERN1), obj.YEILD_PATTERN1_MIN_MAX, obj.YEILD_PATTERN2_MIN, obj.YEILD_PATTERN2_MAX, obj.YEILD_PATTERN3_ACTUAL);
                obj.PP_PRESS = Helper.GetProductPatternFormat(Convert.ToSingle(obj.PRESS_PATTERN1), obj.PRESS_PATTERN1_MIN_MAX, obj.PRESS_PATTERN2_MIN, obj.PRESS_PATTERN2_MAX, obj.PRESS_PATTERN3_ACTUAL);

            }

            return obj;
        }
        public List<ColdDrawingWireDrawingDetails> GetColdDrawingWireDrawingdetailList(GridSearchFields gridSearchRequest)
        {
            short input_Location_CD = Convert.ToInt16(_UserLoginRepository.GetUserLocationById(Convert.ToInt32(gridSearchRequest.USER_ID)));
            gridSearchRequest.LOCATION_CD = input_Location_CD;

            List<ColdDrawingWireDrawingDetails> ColdDrawingWireDrawingDetails = new List<ColdDrawingWireDrawingDetails>();
            List<SearchField> searchFields = ComposeSearchCriteria(gridSearchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);
            ColdDrawingWireDrawingDetails = _ColdDrawingWireDrawingRepository.GetColdDrawingWireDrawingList(searchFields, selectedSortCriteria, gridSearchRequest.LOCATION_CD);
            foreach (ColdDrawingWireDrawingDetails record in ColdDrawingWireDrawingDetails)
            {
               
                if (record.UNIT_TYPE > 0)
                {
                    record.PP_NO_OF_UNITS_DECODE = Convert.ToDecimal(record.DW_FINISHED_COILS).ToString("0.####") + UIUtils.GetProductUnitKubunValue(Convert.ToString(record.UNIT_TYPE));
                }
            }

            return ColdDrawingWireDrawingDetails;
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
                    FieldName = "CONCAT(PP.CHUUBAN,CASE WHEN LENGTH(TRIM(PP.GYOUBAN))=1 THEN '0'||PP.GYOUBAN END)",//"PP.CHUUBAN",
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

            if (!string.IsNullOrEmpty(gridSearchRequest.START_DATETIME))
            {
                var searchField = new SearchField
                {
                    FieldName = "TO_DATE((TO_CHAR(ST.ST_END_DATE, 'mm/dd/yyyy')), 'mm/dd/yyyy')",
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
                    SearchText1 = Convert.ToString(gridSearchRequest.EMPLOYEE_CD)
                };

                searchFields.Add(searchField);
            }
            return searchFields;
        }
        public ColdDrawingWireDrawingDetails InsertColdDrawingWireDrawingDetails(ColdDrawingWireDrawingDetails request)
        {
            ColdDrawingWireDrawingDetails result = new ColdDrawingWireDrawingDetails();
            request.PROCESS_KUBUN_CD = Convert.ToInt32(UIUtils.GetProcessKubun(_localization.GetLocalizedHtmlString("lbl_ProcessDrawingCtoC")));
            if (request.USER_ID != null && request.USER_ID > 0)
            {
                request.INPUT_LOCATION_CD = _UserLoginRepository.GetUserLocationById(request.USER_ID.Value);
            }
            // request.LOCATION_CD = request.INPUT_LOCATION_CD;
            if (request.MACHINE_CD > 0)
            {
                request.LOCATION_CD = _CommonRepository.GetMachineNameByID(Convert.ToInt64(request.MACHINE_CD)).LOCATION_CD;
            }
            using (TransactionScope transaction = new TransactionScope())
            {
                WipStatusDetails wipDetails = new WipStatusDetails();

                wipDetails = new WipStatusDetails();
                wipDetails.PROCESS_PLAN_CD = request.PROCESS_PLAN_CD;
                wipDetails.LOCATION_CD = Convert.ToInt16(request.LOCATION_CD);
                wipDetails.MSG_CD = null;
                wipDetails.FINISHED_BARS = null;
                wipDetails.UNIT = null;
                wipDetails.FINISHED_QTY = null;
                wipDetails.PROCESS_START_DATE = request.DW_START_DATE;
                wipDetails.PROCESS_END_DATE = null;
                wipDetails.PROCESS_KUBUN_CD = request.PROCESS_KUBUN_CD;
                wipDetails.PRE_PROSESS_DECISION = null;
                wipDetails.NEXT_PROCESS = null;
                wipDetails.START_OPERATOR_CD = request.DW_START_OPERATOR_CD;
                wipDetails.END_OPERATOR_CD = null;
                if(request.DW_TABA_NO_1 != null)
                {
                    wipDetails.TABA_NO=Convert.ToInt16(request.DW_TABA_NO_1);
                }
                else
                {
                    wipDetails.TABA_NO = 0;
                }
           
                wipDetails.NIFUDA_CD = null;
                if (request.DW_TABA_NO_1 != null && request.DW_TABA_NO_2 != null || request.DW_TABA_NO_3 != null || request.DW_TABA_NO_4 != null)
                {
                    wipDetails.TABA_GROUP_NO = request.DW_TABA_NO_1;
                }

                _CommonRepository.DeleteWIPStatusData(wipDetails);

                if (request.DW_TABA_NO_2 > 0)
                {
                    wipDetails.TABA_NO = request.DW_TABA_NO_2;
                    _CommonRepository.DeleteWIPStatusData(wipDetails);
                }
                if (request.DW_TABA_NO_3 > 0)
                {
                    wipDetails.TABA_NO = request.DW_TABA_NO_3;
                    _CommonRepository.DeleteWIPStatusData(wipDetails);
                }
                if (request.DW_TABA_NO_4 > 0)
                {
                    wipDetails.TABA_NO = request.DW_TABA_NO_4;
                    _CommonRepository.DeleteWIPStatusData(wipDetails);
                }

               // _CommonRepository.InsertWIPStatusData(wipDetails);


                _ColdDrawingWireDrawingRepository.InsertColdDrawingWireDrawingDetails(request);

                result = GetColdDrawingWireDrawingDetailsByChuuban(request.PP_CHUUBAN, request.PP_GYOUBAN, request.MACHINE_CD, request.CULTURE_NAME);
                transaction.Complete();
            }
            return result;
        }
        public void DeleteColdDrawingWireDrawingDetails(ColdDrawingWireDrawingDetails request)
        {
            WipStatusDetails wipDetails = new WipStatusDetails();
            wipDetails = new WipStatusDetails();
            wipDetails.PROCESS_PLAN_CD = request.PROCESS_PLAN_CD;
           
            using (TransactionScope transaction = new TransactionScope())
            {
                if (request.DRAWING_CD > 0)
                {
                    _ColdDrawingWireDrawingRepository.DeleteColdDrawingWireDrawingDetails(request.DRAWING_CD);
                }
              
                if (request.DW_TABA_NO_1 > 0)
                {
                    wipDetails.TABA_NO = Convert.ToInt16(request.DW_TABA_NO_1);
                    _CommonRepository.DeleteWIPStatusData(wipDetails);
                }
                if (request.DW_TABA_NO_2 > 0)
                {
                    wipDetails.TABA_NO = Convert.ToInt16(request.DW_TABA_NO_2);
                    _CommonRepository.DeleteWIPStatusData(wipDetails);
                }
                if (request.DW_TABA_NO_3 > 0)
                {
                    wipDetails.TABA_NO = Convert.ToInt16(request.DW_TABA_NO_3);
                    _CommonRepository.DeleteWIPStatusData(wipDetails);
                }
                if (request.DW_TABA_NO_4 > 0)
                {
                    wipDetails.TABA_NO = Convert.ToInt16(request.DW_TABA_NO_4);
                    _CommonRepository.DeleteWIPStatusData(wipDetails);
                }
            
                if (request.MSG_CD > 0)
                {
                    _CommonRepository.MessageDetailsDelete(Convert.ToInt64(request.MSG_CD));
                }
                
                transaction.Complete();
            }
        }
        public int GetUserLocationById(int userID)
        {
            return _UserLoginRepository.GetUserLocationById(userID);
        }

        public List<ColdDrawingWireDrawingDetails> GetColdDrawingWireDrawing24MachineList(string machineCd)
        {
            var machineDetails = _ColdDrawingWireDrawingRepository.GetColdDrawingWireDrawing24MachineList(machineCd);

            foreach (ColdDrawingWireDrawingDetails record in machineDetails)
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
                if (_localization.GetLocalizedHtmlString("lbl_ReHeatTreatment") == decisionName)
                {
                    record.DW_DECISION = 5;
                }
                if (_localization.GetLocalizedHtmlString("lbl_ReDrawing") == decisionName)
                {
                    record.DW_DECISION = 5;
                }
                if (_localization.GetLocalizedHtmlString("lbl_Restraightening") == decisionName)
                {
                    record.DW_DECISION = 5;
                }
                if (_localization.GetLocalizedHtmlString("lbl_Re_Chamfering") == decisionName)
                {
                    record.DW_DECISION = 5;
                }
                if (_localization.GetLocalizedHtmlString("lbl_ReEc") == decisionName)
                {
                    record.DW_DECISION = 5;
                } 

            }




            return machineDetails;
        }
        public long UpdateColdDrawingWireDrawingDetails(ColdDrawingWireDrawingDetails request)
        {
            long msgCd = 0;
            ColdDrawingWireDrawingDetails result = new ColdDrawingWireDrawingDetails();
            request.PROCESS_KUBUN_CD = Convert.ToInt32(UIUtils.GetProcessKubun(_localization.GetLocalizedHtmlString("lbl_ProcessDrawingCtoC")));
            if (request.USER_ID != null && request.USER_ID > 0)
            {
                request.INPUT_LOCATION_CD = _UserLoginRepository.GetUserLocationById(request.USER_ID.Value);
            }
            //  request.LOCATION_CD = request.INPUT_LOCATION_CD;
            if (request.MACHINE_CD > 0)
            {
                request.LOCATION_CD = Convert.ToInt32(_CommonRepository.GetMachineNameByID(request.MACHINE_CD)?.LOCATION_CD);
            }
            using (TransactionScope transaction = new TransactionScope())
            {
                MessageDetails messageDetails = new MessageDetails();
                messageDetails.PROCESS_PLAN_CD = Convert.ToInt64(request.PROCESS_PLAN_CD);
                messageDetails.MACHINE_CD = Convert.ToInt32(request.MACHINE_CD);
                messageDetails.PROCESS_KUBUN_ID = Convert.ToInt32(UIUtils.GetProcessKubun(_localization.GetLocalizedHtmlString("lbl_ProcessDrawingCtoC")));
                messageDetails.OPERATOR_CD = Convert.ToInt64(request.DW_END_OPERATOR_CD);
                messageDetails.MSG_CONTENTS = request.DW_REMARKS;
                messageDetails.MSG_CD = Convert.ToInt32(request.MSG_CD);
                messageDetails.USER_ID = Convert.ToInt32(request.USER_ID);
                messageDetails.INPUT_LOCATION_CD = Convert.ToInt32(request.INPUT_LOCATION_CD);

                if (request.MSG_CD == null && !string.IsNullOrEmpty(request.DW_REMARKS))
                {
                    messageDetails = _CommonRepository.MessageDetailsInsert(messageDetails);

                    if (messageDetails.MSG_CD != Constants.NullValue.LONG && messageDetails.MSG_CD > 0)
                    {
                        request.MSG_CD = Convert.ToInt32(messageDetails.MSG_CD);
                    }                  
                }
              
                else if (request.MSG_CD > 0 && request.DW_REMARKS != null)
                {
                    _CommonRepository.MessageDetailsUpdate(messageDetails);                  
                }
              
                else if (request.MSG_CD > 0 && request.DW_REMARKS == null)
                {
                    long messageCD = Convert.ToInt64(request.MSG_CD);
                    request.MSG_CD = Constants.NullValue.LONG;

                    _CommonRepository.MessageDetailsDelete(messageCD);
                }
                
                msgCd = _ColdDrawingWireDrawingRepository.UpdateColdDrawingWireDrawingDetails(request);

                 if (request.DW_NEXT_PROCESS !=999)
                {

                WipStatusDetails wipDetails = new WipStatusDetails();

                wipDetails = new WipStatusDetails();
                wipDetails.PROCESS_PLAN_CD = request.PROCESS_PLAN_CD;
                wipDetails.LOCATION_CD = Convert.ToInt16(request.LOCATION_CD);
                wipDetails.MSG_CD = request.MSG_CD;
                wipDetails.FINISHED_BARS =Convert.ToSingle(request.DW_FINISHED_COILS);
                wipDetails.UNIT = 4 ;
                wipDetails.FINISHED_QTY = request.DW_FINISHED_QTY;
                wipDetails.PROCESS_START_DATE = request.DW_START_DATE;
                wipDetails.PROCESS_END_DATE = request.DW_END_DATE;
                wipDetails.PROCESS_KUBUN_CD = Convert.ToInt32(UIUtils.GetProcessKubun(_localization.GetLocalizedHtmlString("lbl_ProcessDrawingCtoC")));
                wipDetails.PRE_PROSESS_DECISION = request.DW_DECISION;
                wipDetails.NEXT_PROCESS = Convert.ToInt16(request.DW_NEXT_PROCESS);
                wipDetails.START_OPERATOR_CD = request.DW_START_OPERATOR_CD;
                wipDetails.END_OPERATOR_CD = request.DW_END_OPERATOR_CD;
                wipDetails.TABA_NO = request.DW_TABA_NO_1;
                wipDetails.NIFUDA_CD = null;
                if (request.DW_TABA_NO_1 != null && request.DW_TABA_NO_2 != null || request.DW_TABA_NO_3 != null || request.DW_TABA_NO_4 != null)
                {
                    wipDetails.TABA_GROUP_NO = request.DW_TABA_NO_1;
                }

                _CommonRepository.DeleteWIPStatusData(wipDetails);
                    
                if (request.DW_TABA_NO_2 > 0)
                {

                    wipDetails.FINISHED_QTY = null;
                    wipDetails.FINISHED_BARS = null;
                    wipDetails.UNIT = null;
                    wipDetails.TABA_NO = request.DW_TABA_NO_2;
                    _CommonRepository.DeleteWIPStatusData(wipDetails);
                }
                if (request.DW_TABA_NO_3 > 0)
                {

                    wipDetails.FINISHED_QTY = null;
                    wipDetails.FINISHED_BARS = null;
                    wipDetails.UNIT = null;
                    wipDetails.TABA_NO = request.DW_TABA_NO_3;
                    _CommonRepository.DeleteWIPStatusData(wipDetails);
                }
                if (request.DW_TABA_NO_4 > 0)
                {

                    wipDetails.FINISHED_QTY = null;
                    wipDetails.FINISHED_BARS = null;
                    wipDetails.UNIT = null;
                    wipDetails.TABA_NO = request.DW_TABA_NO_4;
                    _CommonRepository.DeleteWIPStatusData(wipDetails);
                }
             }

                transaction.Complete();
            }
            return msgCd;
        }

        public List<ColdDrawingWireDrawingDetails> GetColdDrawingList(int locationCd)
        {
            return _ColdDrawingWireDrawingRepository.GetColdDrawingList(locationCd);
        }

        public string GetThabaDetailsByThabaNumber(GridSearchFields request)
        {
            string duplicateThaba = string.Empty;
            var result = _ColdDrawingWireDrawingRepository.GetThabaDetailsByThabaNumber(request.CHUUBAN, request.GYOUBAN, request.TABA_NO_STRING);
           

            if(request.TABA_NO_1 > 0)
            {
                var resultList = result.AsEnumerable().OrderBy(x => x.DRAWING_CD).Where(x => x.TABA_NO == Convert.ToString(request.TABA_NO_1)).Select(r => r).FirstOrDefault(); //.Where(x => x.DRAWING_CD == request.DRAWING_CD)
                if (resultList != null)
                {
                    if (request.DRAWING_CD != resultList.DRAWING_CD && request.ISCHECKED == 0)
                    {
                        duplicateThaba = Convert.ToString(request.TABA_NO_1);
                    }
                }
            }
            if (request.TABA_NO_2 > 0)
            {
                var resultList = result.AsEnumerable().OrderBy(x => x.DRAWING_CD).Where(x => x.TABA_NO == Convert.ToString(request.TABA_NO_2)).Select(r => r).FirstOrDefault(); //.Where(x => x.DRAWING_CD == request.DRAWING_CD)
                if (resultList != null)
                {
                    if (request.DRAWING_CD != resultList.DRAWING_CD && request.ISCHECKED == 0)
                    {
                        if (duplicateThaba == string.Empty)
                        {
                            duplicateThaba = Convert.ToString(request.TABA_NO_2);
                        }else
                        {
                            duplicateThaba = duplicateThaba + ", " + Convert.ToString(request.TABA_NO_2);
                        }
                    }
                }
            }
            if (request.TABA_NO_3 > 0)
            {
                var resultList = result.AsEnumerable().OrderBy(x => x.DRAWING_CD).Where(x => x.TABA_NO == Convert.ToString(request.TABA_NO_3)).Select(r => r).FirstOrDefault(); //.Where(x => x.DRAWING_CD == request.DRAWING_CD)
                if (resultList != null)
                {
                    if (request.DRAWING_CD != resultList.DRAWING_CD && request.ISCHECKED == 0)
                    {
                        if (duplicateThaba == string.Empty)
                        {
                            duplicateThaba = Convert.ToString(request.TABA_NO_3);
                        }
                        else
                        {
                            duplicateThaba = duplicateThaba + ", " + Convert.ToString(request.TABA_NO_3);
                        }
                    }
                }
            }
            if (request.TABA_NO_4 > 0)
            {
                var resultList = result.AsEnumerable().OrderBy(x => x.DRAWING_CD).Where(x => x.TABA_NO == Convert.ToString(request.TABA_NO_4)).Select(r => r).FirstOrDefault(); //.Where(x => x.DRAWING_CD == request.DRAWING_CD)
                if (resultList != null)
                {
                    if (request.DRAWING_CD != resultList.DRAWING_CD && request.ISCHECKED == 0)
                    {
                        if (duplicateThaba == string.Empty)
                        {
                            duplicateThaba = Convert.ToString(request.TABA_NO_4);
                        }
                        else
                        {
                            duplicateThaba = duplicateThaba + ", " + Convert.ToString(request.TABA_NO_4);
                        }
                    }
                }
            }
            //foreach (ColdDrawingWireDrawingDetails record in resultList)
            //{

            //    if (Convert.ToString(request.TABA_NO)!=record.TABA_NO && record.DRAWING_CD!=request.DRAWING_CD && request.ISCHECKED==0)
            //    {
            //        if (duplicateThaba == string.Empty)
            //        {
            //            duplicateThaba = record.TABA_NO;
            //        }else
            //        {
            //            duplicateThaba = duplicateThaba + ", " + record.TABA_NO;
            //        }

            //    }
            //}

            //foreach (ColdDrawingWireDrawingDetails record in result)
            //{
            //    string decisionName = UIUtils.GetResultKubunValue(Convert.ToString(record.DECISION));
            //    if (_localization.GetLocalizedHtmlString("lbl_OK") == decisionName)
            //    {
            //        record.DW_DECISION = 1;
            //    }
            //    if (_localization.GetLocalizedHtmlString("lbl_NG") == decisionName)
            //    {
            //        record.DW_DECISION = 2;
            //    }
            //    if (_localization.GetLocalizedHtmlString("lbl_Hold") == decisionName)
            //    {
            //        record.DW_DECISION = 3;
            //    }
            //    if (_localization.GetLocalizedHtmlString("lbl_DecisionKubun4of4") == decisionName)
            //    {
            //        record.DW_DECISION = 4;
            //    }
            //}
            //if (result != null)
            //{
            //    ColdDrawingWireDrawingDetails getData = result.AsEnumerable().Select(x => x).OrderBy(r => r.DRAWING_CD).FirstOrDefault();

            //    if (result.Count > 1 && request.ISCHECKED == 0)
            //    {
            //        if (getData.DRAWING_CD == request.DRAWING_CD)
            //        {
            //            duplicateThaba = string.Empty;
            //        }
            //        else
            //        {
            //            List<short> list = new List<short>();
            //            list.Add(Convert.ToInt16(request.TABA_NO_1));
            //            if (request.TABA_NO_2 != null)
            //            {
            //                list.Add(Convert.ToInt16(request.TABA_NO_2));
            //            }
            //            if (request.TABA_NO_3 != null)
            //            {
            //                list.Add(Convert.ToInt16(request.TABA_NO_3));
            //            }
            //            if (request.TABA_NO_4 != null)
            //            {
            //                list.Add(Convert.ToInt16(request.TABA_NO_4));
            //            }

            //            List<ColdDrawingWireDrawingDetails> details = new List<ColdDrawingWireDrawingDetails>();

            //            details = result.AsEnumerable().Where(x => x.DRAWING_CD != request.DRAWING_CD && list.Contains(Convert.ToInt16(x.DW_TABA_NO_1)) || list.Contains(Convert.ToInt16(x.DW_TABA_NO_2)) || list.Contains(Convert.ToInt16(x.DW_TABA_NO_3)) || list.Contains(Convert.ToInt16(x.DW_TABA_NO_4))).Select(x => x).ToList();
            //            details = result.AsEnumerable().Where(x => x.DRAWING_CD != request.DRAWING_CD).Select(x => x).ToList();

            //            foreach (var item in details)
            //            {
            //                if (item.DW_TABA_NO_1 > 0)
            //                {
            //                    if (item.DW_TABA_NO_1 == request.TABA_NO_1 || item.DW_TABA_NO_1 == request.TABA_NO_2 || item.DW_TABA_NO_1 == request.TABA_NO_3 || item.DW_TABA_NO_1 == request.TABA_NO_4)
            //                    {
            //                        if (string.IsNullOrEmpty(duplicateThaba))
            //                        {
            //                            duplicateThaba = Convert.ToString(item.DW_TABA_NO_1);
            //                        }
            //                        else
            //                        {
            //                            duplicateThaba = duplicateThaba + "," + Convert.ToString(item.DW_TABA_NO_1);
            //                        }
            //                    }
            //                }
            //                if (item.DW_TABA_NO_2 > 0)
            //                {
            //                    if (item.DW_TABA_NO_2 == request.TABA_NO_1 || item.DW_TABA_NO_2 == request.TABA_NO_2 || item.DW_TABA_NO_2 == request.TABA_NO_3 || item.DW_TABA_NO_2 == request.TABA_NO_4)
            //                    {
            //                        if (string.IsNullOrEmpty(duplicateThaba))
            //                        {
            //                            duplicateThaba = Convert.ToString(item.DW_TABA_NO_2);
            //                        }
            //                        else
            //                        {
            //                            duplicateThaba = duplicateThaba + "," + Convert.ToString(item.DW_TABA_NO_2);
            //                        }
            //                    }
            //                }
            //                if (item.DW_TABA_NO_3 > 0)
            //                {
            //                    if (item.DW_TABA_NO_3 == request.TABA_NO_1 || item.DW_TABA_NO_3 == request.TABA_NO_2 || item.DW_TABA_NO_3 == request.TABA_NO_3 || item.DW_TABA_NO_3 == request.TABA_NO_4)
            //                    {
            //                        if (string.IsNullOrEmpty(duplicateThaba))
            //                        {
            //                            duplicateThaba = Convert.ToString(item.DW_TABA_NO_3);
            //                        }
            //                        else
            //                        {
            //                            duplicateThaba = duplicateThaba + "," + Convert.ToString(item.DW_TABA_NO_3);
            //                        }
            //                    }
            //                }
            //                if (item.DW_TABA_NO_4 > 0)
            //                {
            //                    if (item.DW_TABA_NO_4 == request.TABA_NO_1 || item.DW_TABA_NO_4 == request.TABA_NO_2 || item.DW_TABA_NO_4 == request.TABA_NO_3 || item.DW_TABA_NO_4 == request.TABA_NO_4)
            //                    {
            //                        if (string.IsNullOrEmpty(duplicateThaba))
            //                        {
            //                            duplicateThaba = Convert.ToString(item.DW_TABA_NO_4);
            //                        }
            //                        else
            //                        {
            //                            duplicateThaba = duplicateThaba + "," + Convert.ToString(item.DW_TABA_NO_4);
            //                        }
            //                    }
            //                }
            //            }
            //            if (!string.IsNullOrEmpty(duplicateThaba))
            //            {
            //                string[] value = duplicateThaba.Split(',');
            //                var distinctItems = value.Distinct();
            //                duplicateThaba = string.Join(", ", distinctItems);
            //            }


            //        }


            //    }
            //    else if (result.Count == 1 && request.ISCHECKED == 0 && result[0].DRAWING_CD != request.DRAWING_CD)
            //    {
            //        if (result[0].DW_TABA_NO_1 == request.TABA_NO_1 || result[0].DW_TABA_NO_1 == request.TABA_NO_2 || result[0].DW_TABA_NO_1 == request.TABA_NO_3 || result[0].DW_TABA_NO_1 == request.TABA_NO_4)
            //        {
            //            if (string.IsNullOrEmpty(duplicateThaba))
            //            {
            //                duplicateThaba = Convert.ToString(result[0].DW_TABA_NO_1);
            //            }
            //            else
            //            {
            //                duplicateThaba = duplicateThaba + "," + Convert.ToString(result[0].DW_TABA_NO_1);
            //            }
            //        }
            //        if (Convert.ToInt16(result[0].DW_TABA_NO_2) > 0)
            //        {
            //            if (result[0].DW_TABA_NO_2 == request.TABA_NO_1 || result[0].DW_TABA_NO_2 == request.TABA_NO_2 || result[0].DW_TABA_NO_2 == request.TABA_NO_3 || result[0].DW_TABA_NO_2 == request.TABA_NO_4)
            //            {
            //                if (string.IsNullOrEmpty(duplicateThaba))
            //                {
            //                    duplicateThaba = Convert.ToString(result[0].DW_TABA_NO_2);
            //                }
            //                else
            //                {
            //                    duplicateThaba = duplicateThaba + "," + Convert.ToString(result[0].DW_TABA_NO_2);
            //                }
            //            }
            //        }
            //        if (Convert.ToInt16(result[0].DW_TABA_NO_3) > 0)
            //        {
            //            if (Convert.ToInt16(result[0].DW_TABA_NO_3) > 0 && result[0].DW_TABA_NO_3 == request.TABA_NO_1 || result[0].DW_TABA_NO_3 == request.TABA_NO_2 || result[0].DW_TABA_NO_3 == request.TABA_NO_3 || result[0].DW_TABA_NO_3 == request.TABA_NO_4)
            //            {
            //                if (string.IsNullOrEmpty(duplicateThaba))
            //                {
            //                    duplicateThaba = Convert.ToString(result[0].DW_TABA_NO_3);
            //                }
            //                else
            //                {
            //                    duplicateThaba = duplicateThaba + "," + Convert.ToString(result[0].DW_TABA_NO_3);
            //                }
            //            }
            //        }
            //        if (Convert.ToInt16(result[0].DW_TABA_NO_4) > 0)
            //        {
            //            if (Convert.ToInt16(result[0].DW_TABA_NO_4) > 0 && result[0].DW_TABA_NO_4 == request.TABA_NO_1 || result[0].DW_TABA_NO_4 == request.TABA_NO_2 || result[0].DW_TABA_NO_4 == request.TABA_NO_3 || result[0].DW_TABA_NO_4 == request.TABA_NO_4)
            //            {
            //                if (string.IsNullOrEmpty(duplicateThaba))
            //                {
            //                    duplicateThaba = Convert.ToString(result[0].DW_TABA_NO_4);
            //                }
            //                else
            //                {
            //                    duplicateThaba = duplicateThaba + "," + Convert.ToString(result[0].DW_TABA_NO_4);
            //                }
            //            }
            //        }
            //        if (!string.IsNullOrEmpty(duplicateThaba))
            //        {
            //            string[] value = duplicateThaba.Split(',');
            //            var distinctItems = value.Distinct();
            //            duplicateThaba = string.Join(", ", distinctItems);
            //        }

            //    }
            //    else
            //    {
            //        duplicateThaba = string.Empty;
            //    }


            //}
            //else
            //{
            //    duplicateThaba = string.Empty;
            //}
            return duplicateThaba;
        }

        public List<ColdDrawingWireDrawingDetails> GetWireDrawingDailyReportOnFactoryDailyWorkStatus(GridSearchFields gridSearchRequest)
        {
            List<SearchField> searchFields = ComposeSearchCriteriaforFactoryDailyWorkStatus(gridSearchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);
            int rowLimit = gridSearchRequest.ROW_LIMIT;
            var result = _ColdDrawingWireDrawingRepository.GetWireDrawingDailyReportOnFactoryDailyWorkStatus(searchFields, selectedSortCriteria, rowLimit);           
            return result;
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


        public List<ColdDrawingWireDrawingDetails> GetDailyProcessAnalysisColdDrawingW(GridSearchFields gridSearchRequest)
        {

            List<SearchField> searchFields = ComposeSearchCriteriaforDailyProcessColdDrawingWireDrawingReport(gridSearchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);
            var result = _ColdDrawingWireDrawingRepository.GetDailyProcessColdDrawingW(searchFields, selectedSortCriteria, gridSearchRequest);
            return result;
        }


        private List<SearchField> ComposeSearchCriteriaforDailyProcessColdDrawingWireDrawingReport(GridSearchFields gridSearchRequest)
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
                    FieldName = "INTE.INTERNAL_NAME",
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
                    FieldName = "CON.CONDITION_CD",
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
            //Cold_Drawing_Wire search
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
            if (!string.IsNullOrEmpty(gridSearchRequest.ROLLING_NUMBER))
            {
                var searchField = new SearchField
                {
                    FieldName = "DW.ROLLING_NUMBER",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.ROLLING_NUMBER.ToLower().Trim()
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
                    FieldName = "EMP2.SHORT_NAME",
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


             
        public int GetRowCountForWireDrawingFactory(GridSearchFields gridSearchRequest)
        {
            List<SearchField> searchFields = ComposeSearchCriteriaforFactoryDailyWorkStatus(gridSearchRequest);
            int count = _ColdDrawingWireDrawingRepository.GetRowCountForWireDrawingFactory(searchFields);
            return count;
        }

        public int GetRowCountForColdDrawingCoilToCoilDailyAnalysis(GridSearchFields gridSearchRequest)
        {
            List<SearchField> searchFields = ComposeSearchCriteriaforDailyProcessColdDrawingWireDrawingReport(gridSearchRequest);
            int count = _ColdDrawingWireDrawingRepository.GetRowCountForColdDrawingCoilToCoilDailyAnalysis(searchFields);
            return count;
        }
    }


}
