using Org.BouncyCastle.Crypto.Operators;
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
using Org.BouncyCastle.Asn1.Ocsp;
using System.Globalization;

namespace TSWebApplication.Application.Services
{
    public class HikinukiBarToBarService : IHikinukiBarToBarService
    {

        private readonly IHikinukiBarToBarRepository _hikinukibartobarRepository;
        private readonly IAllCommonRepository _CommonRepository;
        private readonly IUserLoginRepository _UserLoginRepository;
        private readonly LocalizationServices _localization;
        private readonly IAllCommonService _allCommonService;
        public HikinukiBarToBarService(IHikinukiBarToBarRepository hikinukibartobarRepository, IAllCommonRepository commonRepository, IUserLoginRepository userLoginRepository, LocalizationServices localization, IAllCommonService allCommonService)
        {
            _hikinukibartobarRepository = hikinukibartobarRepository;
            _CommonRepository = commonRepository;
            _UserLoginRepository = userLoginRepository;
            _localization = localization;
            _allCommonService = allCommonService;
        }
        public List<HikinukiBarToBarDetails> GetHikinukiBarToBarList(GridSearchFields gridSearchRequest)
        {
            short locationCD = Convert.ToInt16(_CommonRepository.GetUserLocationById(Convert.ToInt32(gridSearchRequest.USER_ID)));
            List<HikinukiBarToBarDetails> hikinukibartobarDetails = new List<HikinukiBarToBarDetails>();
            List<SearchField> searchFields = ComposeSearchCriteria(gridSearchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);
            hikinukibartobarDetails = _hikinukibartobarRepository.GetHikinukiBarToBarList(searchFields, selectedSortCriteria, locationCD);

            foreach (HikinukiBarToBarDetails record in hikinukibartobarDetails)
            {
                record.DW_DECISION_STRING = UIUtils.GetResultKubunValue(Convert.ToString(record.DW_DECISION));
            }
            return hikinukibartobarDetails;
        }
        private List<SearchField> ComposeSearchCriteria(GridSearchFields gridSearchRequest)
        {
            var searchFields = new List<SearchField>();

            if (!string.IsNullOrEmpty(gridSearchRequest.CHUUBAN))
            {
                string chuuban = string.Empty;
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
                    //SearchType = SearchType.Text,

                    //SearchText1 = chuuban.Trim()
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
                    FieldName = "DW.DW_TABA_NO_",
                    SearchType = SearchType.Text,
                    SearchText1 = Convert.ToString(gridSearchRequest.TABA_NO)
                };

                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.CUSTOMER_NAME))
            {
                var searchField = new SearchField
                {
                    FieldName = "CUS.SHORT_NAME",
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
                    FieldName = "INTE.INTERNAL_NAME",
                    SearchType = SearchType.Text,
                    SearchCondition = SearchCondition.Equal.ToString(),
                    SearchText1 = gridSearchRequest.INTERNAL_NAME_STRING
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
            else if (gridSearchRequest.PROD_DIM_D_MIN > 0 || gridSearchRequest.PROD_DIM_D_MAX > 0)
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
            
            else if (gridSearchRequest.PROD_LENGTH_MIN > 0 || gridSearchRequest.PROD_LENGTH_MAX > 0)
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

        public List<ThabaDetails> GetThabaDetailsByChuuban(string chuuban, int gyouban)
        {
            var result = _hikinukibartobarRepository.GetThabaDetails(chuuban, gyouban);

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

        public HikinukiBarToBarDetails GetHikinukibartobarDetailsByChuuban(string chubaan, short? gyouban, int? mechineCD, String cultureName)
        {
            var obj = _hikinukibartobarRepository.GetHikinukiBarToBarDetailsByChuuban(chubaan, gyouban, mechineCD);
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

                if (obj.PP_UNIT_WT_KUBUN > 0)
                {
                    obj.PP_UNIT_WT_STRING = Utils.GetUnitWeigth(obj.PP_UNIT_WT_KUBUN, Convert.ToInt32(obj.PP_UNIT_WT_STRING), Convert.ToInt16(obj.PP_UNIT_WT_MIN), obj.PP_UNIT_WT_MAX);

                }
                if (obj.COIL_DIA_WINDING != null)
                {
                    // obj.PP_COIL_DIA_WINDING = obj.PP_COIL_DIA_WINDING;
                    obj.PP_COIL_DIA_WINDING = UIUtils.GetCoilDiaWindingKubunValue(Convert.ToString(obj.COIL_DIA_WINDING)); //  Utils.GetCoilDiaWindingValue(Convert.ToInt16(_stamppingDetails.PP_COIL_DIA_WINDING));
                }
                if (obj.DW_DECISION != null)
                {
                    obj.DW_DECISION_STRING = UIUtils.GetResultKubunValue(Convert.ToString(obj.DW_DECISION));
                }
                obj.DECISION_OK = UIUtils.GetResultKubunKey(_localization.GetLocalizedHtmlString("lbl_OK")); 
                var messageList = _CommonRepository.GetMessageDetailsByProcessPlanCD(Convert.ToInt64(obj.PP_PROCESS_PLAN_CD), true);
                obj.DW_MESSAGE_LIST = messageList;

                obj.PP_ELONGATION = Helper.GetProductPatternFormat(obj.ELONGATION_PATTERN1.HasValue ? obj.ELONGATION_PATTERN1.Value : 0, obj.ELONGATION_PATTERN1_MIN_MAX.HasValue ? obj.ELONGATION_PATTERN1_MIN_MAX.Value : 0, Convert.ToDecimal(obj.ELONGATION_PATTERN2_MIN), Convert.ToDecimal(obj.ELONGATION_PATTERN2_MAX), obj.ELONGATION_PATTERN3_ACTUAL);
                obj.PP_TENSILE_STRENGTH = Helper.GetProductPatternFormat(Convert.ToSingle(obj.TENSILE_STRN_PATTERN1), obj.TENSILE_STRN_PATTERN1_MIN_MAX, obj.TENSILE_STRN_PATTERN2_MIN, obj.TENSILE_STRN_PATTERN2_MAX, obj.TENSILE_STRN_PATTERN3_ACTUAL);
                obj.PP_HARDNESS = Helper.GetHardnessProductPatternFormat(obj.HARDNESS_PATTERN1_CODE, obj.HARDNESS_PATTERN1, obj.HARDNESS_PATTERN1_MIN_MAX, obj.HARDNESS_PATTERN2_CODE, obj.HARDNESS_PATTERN2_MIN, obj.HARDNESS_PATTERN2_MAX, obj.HARDNESS_PATTERN3_ACTUAL);
                obj.PP_DECARB = Helper.GetDecarbProductPatternFormat(obj.DECARB_1,Convert.ToSingle(obj.DECARB_VAL_1), obj.DECARB_2,Convert.ToSingle(obj.DECARB_VAL_2), obj.DECARB_3,Convert.ToSingle(obj.DECARB_VAL_3));
                obj.PP_YEILD = Helper.GetProductPatternFormat(Convert.ToSingle(obj.YEILD_PATTERN1), obj.YEILD_PATTERN1_MIN_MAX, obj.YEILD_PATTERN2_MIN, obj.YEILD_PATTERN2_MAX, obj.YEILD_PATTERN3_ACTUAL);
                obj.PP_PRESS = Helper.GetProductPatternFormat(Convert.ToSingle(obj.PRESS_PATTERN1), obj.PRESS_PATTERN1_MIN_MAX, obj.PRESS_PATTERN2_MIN, obj.PRESS_PATTERN2_MAX, obj.PRESS_PATTERN3_ACTUAL);
            }
            return obj;
        }

        public List<HikinukiBarToBarDetails> GetHikinukiBarToBar24MachineList(string machineCd, string cultureName)
        {
            var result = _hikinukibartobarRepository.GetHikinukiBarToBar24MachineList(machineCd);
            if (cultureName == "en")
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
            }
            else
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("ja-JP");
            }
            foreach (HikinukiBarToBarDetails record in result)
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

                record.UNIT_TYPE_STRING = UIUtils.GetMaterialUnitKubunValue(Convert.ToString(record.UNIT_TYPE));
                record.DIE_SHAPE_STRING = UIUtils.GetDieShapeList(Convert.ToString(record.DIE_SHAPE));
                //GetDieShapeList
                record.DW_BEND_STATUS_DECODE = UIUtils.GetBendStateKubunValue(Convert.ToString(record.DW_BEND_STATUS));
                record.HT_CONDITION_STRING = record.HT_CONDITION;
                record.DW_EXT_TEXTURE_STATUS_DECODE = UIUtils.GetExteriorCondition(Convert.ToString(record.DW_EXT_TEXTURE_STATUS));
                record.PP_NO_OF_UNITS_DECODE = record.PP_NO_OF_UNITS != null ? record.PP_NO_OF_UNITS + UIUtils.GetProductUnitKubunValue(Convert.ToString(record.PP_UNIT)) : string.Empty;
            }
            return result;
        }

        public HikinukiBarToBarDetails SaveHikinukiBarToBar(HikinukiBarToBarDetails request)
        {

            HikinukiBarToBarDetails obj = new HikinukiBarToBarDetails();
            request.PROCESS_KUBUN_CD = Convert.ToInt32(UIUtils.GetProcessKubun(_localization.GetLocalizedHtmlString("lbl_ProcessDrawingBtoB")));
            if (request.USER_ID != null && request.USER_ID > 0)
            {
                request.INPUT_LOCATION_CD = _UserLoginRepository.GetUserLocationById(request.USER_ID.Value);
            }
            // request.LOCATION_CD = request.INPUT_LOCATION_CD;
            if (request.MACHINE_CD != null && request.MACHINE_CD > 0)
            {
                request.LOCATION_CD = _CommonRepository.GetMachineNameByID(Convert.ToInt64(request.MACHINE_CD)).LOCATION_CD;
            }


            WipStatusDetails wipDetails = new WipStatusDetails();
            wipDetails = new WipStatusDetails();
            wipDetails.PROCESS_PLAN_CD = Convert.ToInt32(request.PROCESS_PLAN_CD);
            wipDetails.LOCATION_CD = Convert.ToInt16(request.LOCATION_CD);
            wipDetails.MSG_CD = null;
            wipDetails.FINISHED_BARS = 0; //null;
            wipDetails.UNIT = null;
            wipDetails.FINISHED_QTY = null;
            wipDetails.PROCESS_START_DATE = Convert.ToDateTime(request.DW_START_DATE);
            wipDetails.PROCESS_END_DATE = null;
            wipDetails.PROCESS_KUBUN_CD = request.PROCESS_KUBUN_CD;
            wipDetails.PRE_PROSESS_DECISION = null;
            wipDetails.NEXT_PROCESS = null;
            wipDetails.START_OPERATOR_CD = request.DW_START_OPERATOR_CD > 0 ? request.DW_START_OPERATOR_CD : null;
            wipDetails.END_OPERATOR_CD = null;
            if (request.DW_TABA_NO_ != null)
            {
                wipDetails.TABA_NO = Convert.ToInt16(request.DW_TABA_NO_);
            }
            else
            {
                wipDetails.TABA_NO = 0;
            }


            using (TransactionScope transaction = new TransactionScope())
            {
                _CommonRepository.DeleteWIPStatusData(wipDetails);

                _hikinukibartobarRepository.SaveHikinukiBarToBar(request);

                // _CommonRepository.InsertWIPStatusData(wipDetails);

                transaction.Complete();

            }
            obj = _hikinukibartobarRepository.GetHikinukiBarToBarDetailsByChuuban(request.PP_CHUUBAN, request.PP_GYOUBAN, request.MACHINE_CD);

            if ( request.CULTURE_NAME== "en")
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

            if (obj.DW_DECISION > 0)
            {
                string str = UIUtils.GetResultKubunValue(Convert.ToString(obj.DW_DECISION));
                obj.DW_DECISION = Convert.ToInt32(str);
            }

            if (obj.MAGARI_KUBUN != null)
            {
                obj.PP_MAGARI_BEND_TOL = Utils.GetMagariBendTolSpan(UIUtils.Instance.GetMagariKubunValue(Convert.ToString(obj.MAGARI_KUBUN)), Convert.ToDecimal(obj.MAGARI_BEND_TOL), Convert.ToInt32(obj.MAGARI_SPAN));  //Convert.ToDecimal(obj.PP_NO_OF_UNITS).ToString("0.####") + UIUtils.Instance.GetProductUnitKubunValue(Convert.ToString(obj.PP_UNIT));
            }

            if (obj.PP_UNIT_WT_KUBUN > 0)
            {
                obj.PP_UNIT_WT_STRING = Utils.GetUnitWeigth(obj.PP_UNIT_WT_KUBUN, Convert.ToInt32(obj.PP_UNIT_WT_STRING), Convert.ToInt16(obj.PP_UNIT_WT_MIN), obj.PP_UNIT_WT_MAX);

            }

            if (obj.COIL_DIA_WINDING != null)
            {
                obj.PP_COIL_DIA_WINDING = UIUtils.GetCoilDiaWindingKubunValue(Convert.ToString(obj.COIL_DIA_WINDING)); //  Utils.GetCoilDiaWindingValue(Convert.ToInt16(_stamppingDetails.PP_COIL_DIA_WINDING));
            }
            obj.DECISION_OK = UIUtils.GetResultKubunKey(_localization.GetLocalizedHtmlString("lbl_OK"));
            obj.PP_ELONGATION = Helper.GetProductPatternFormat(obj.ELONGATION_PATTERN1.HasValue ? obj.ELONGATION_PATTERN1.Value : 0, obj.ELONGATION_PATTERN1_MIN_MAX.HasValue ? obj.ELONGATION_PATTERN1_MIN_MAX.Value : 0, Convert.ToDecimal(obj.ELONGATION_PATTERN2_MIN), Convert.ToDecimal(obj.ELONGATION_PATTERN2_MAX), obj.ELONGATION_PATTERN3_ACTUAL);
            obj.PP_TENSILE_STRENGTH = Helper.GetProductPatternFormat(Convert.ToSingle(obj.TENSILE_STRN_PATTERN1), obj.TENSILE_STRN_PATTERN1_MIN_MAX, obj.TENSILE_STRN_PATTERN2_MIN, obj.TENSILE_STRN_PATTERN2_MAX, obj.TENSILE_STRN_PATTERN3_ACTUAL);
            obj.PP_HARDNESS = Helper.GetHardnessProductPatternFormat(obj.HARDNESS_PATTERN1_CODE, obj.HARDNESS_PATTERN1, obj.HARDNESS_PATTERN1_MIN_MAX, obj.HARDNESS_PATTERN2_CODE, obj.HARDNESS_PATTERN2_MIN, obj.HARDNESS_PATTERN2_MAX, obj.HARDNESS_PATTERN3_ACTUAL);
            obj.PP_DECARB = Helper.GetDecarbProductPatternFormat(obj.DECARB_1,Convert.ToSingle(obj.DECARB_VAL_1), obj.DECARB_2,Convert.ToSingle(obj.DECARB_VAL_2), obj.DECARB_3,Convert.ToSingle(obj.DECARB_VAL_3));
            obj.PP_YEILD = Helper.GetProductPatternFormat(Convert.ToSingle(obj.YEILD_PATTERN1), obj.YEILD_PATTERN1_MIN_MAX, obj.YEILD_PATTERN2_MIN, obj.YEILD_PATTERN2_MAX, obj.YEILD_PATTERN3_ACTUAL);
            obj.PP_PRESS = Helper.GetProductPatternFormat(Convert.ToSingle(obj.PRESS_PATTERN1), obj.PRESS_PATTERN1_MIN_MAX, obj.PRESS_PATTERN2_MIN, obj.PRESS_PATTERN2_MAX, obj.PRESS_PATTERN3_ACTUAL);


            return obj;
        }

        public long HikinukiBarToBarDetailsUpdate(HikinukiBarToBarDetails request)
        {
            long msgCd = 0;
            bool deleteMsg = false;
            request.PROCESS_KUBUN_CD = Convert.ToInt32(UIUtils.GetProcessKubun(_localization.GetLocalizedHtmlString("lbl_ProcessDrawingBtoB")));

            if (request.USER_ID != null && request.USER_ID > 0)
            {
                request.INPUT_LOCATION_CD = _UserLoginRepository.GetUserLocationById(request.USER_ID.Value);
            }
            // request.LOCATION_CD = request.INPUT_LOCATION_CD;
            if (request.MACHINE_CD != null && request.MACHINE_CD > 0)
            {
                request.LOCATION_CD = _CommonRepository.GetMachineNameByID(Convert.ToInt64(request.MACHINE_CD))?.LOCATION_CD;
            }
            using (TransactionScope transaction = new TransactionScope())
            {
                MessageDetails messageDetails = new MessageDetails();
                messageDetails.PROCESS_PLAN_CD = Convert.ToInt64(request.PROCESS_PLAN_CD);
                messageDetails.MACHINE_CD = Convert.ToInt32(request.MACHINE_CD);
                messageDetails.PROCESS_KUBUN_ID = Convert.ToInt32(UIUtils.GetProcessKubun(_localization.GetLocalizedHtmlString("lbl_ProcessDrawingBtoB")));
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
                    
                    msgCd = _hikinukibartobarRepository.HikinukiBarToBarDetailsUpdate(request);
                    _CommonRepository.MessageDetailsDelete(messageCD);
                    deleteMsg = true;
                    //if (request.DRAWING_CD > 0)
                    //{
                    //    msgCd = _hikinukibartobarRepository.HikinukiBarToBarDetailsUpdate(request);
                    //}
                }
               if (deleteMsg == false)
                {
                    msgCd = _hikinukibartobarRepository.HikinukiBarToBarDetailsUpdate(request);
                }
                    
                if (request.DW_NEXT_PROCESS != 999)
                {

                    WipStatusDetails wipDetails = new WipStatusDetails();
                    wipDetails = new WipStatusDetails();
                    wipDetails.PROCESS_PLAN_CD = Convert.ToInt32(request.PROCESS_PLAN_CD);
                    wipDetails.LOCATION_CD = Convert.ToInt16(request.LOCATION_CD);
                    wipDetails.MSG_CD = request.MSG_CD > 0 ? request.MSG_CD : null;
                    wipDetails.FINISHED_BARS = request.DW_FINISHED_BARS;
                    wipDetails.UNIT = request.UNIT_TYPE;
                    wipDetails.FINISHED_QTY = request.DW_FINISHED_QTY;
                    wipDetails.PROCESS_START_DATE = Convert.ToDateTime(request.DW_START_DATE);
                    wipDetails.PROCESS_END_DATE = Convert.ToDateTime(request.DW_END_DATE);
                    wipDetails.PROCESS_KUBUN_CD = request.PROCESS_KUBUN_CD;
                    wipDetails.PRE_PROSESS_DECISION = request.DW_DECISION;
                    wipDetails.NEXT_PROCESS = Convert.ToInt16(request.DW_NEXT_PROCESS);
                    wipDetails.START_OPERATOR_CD = request.DW_START_OPERATOR_CD > 0 ? request.DW_START_OPERATOR_CD : null;
                    wipDetails.END_OPERATOR_CD = request.DW_END_OPERATOR_CD;
                    wipDetails.TABA_NO = Convert.ToInt16(request.DW_TABA_NO_);
                    // _CommonRepository.InsertWIPStatusData(wipDetails);
                    _CommonRepository.DeleteWIPStatusData(wipDetails);
                }



                transaction.Complete();
            }
            return msgCd;
        }

        public HikinukiBarToBarDetails GetHikinukiDetailsDrawingByCD(long drawingCD, string cultureName)
        {
            var obj = _hikinukibartobarRepository.GetHikinukiBarToBarDetailsByDrawingCD(drawingCD);
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

                if (obj.PP_UNIT_WT_KUBUN > 0)
                {
                    obj.PP_UNIT_WT_STRING = Utils.GetUnitWeigth(obj.PP_UNIT_WT_KUBUN, Convert.ToInt32(obj.PP_UNIT_WT_STRING), Convert.ToInt16(obj.PP_UNIT_WT_MIN), obj.PP_UNIT_WT_MAX);

                }
                obj.DECISION_OK = Convert.ToInt16(UIUtils.GetResultKubunKey(_localization.GetLocalizedHtmlString("lbl_OK")));
                obj.PP_COIL_DIA_WINDING = UIUtils.GetCoilDiaWindingKubunValue(Convert.ToString(obj.COIL_DIA_WINDING)); //  Utils.GetCoilDiaWindingValue(Convert.ToInt16(_stamppingDetails.PP_COIL_DIA_WINDING));
                obj.DW_DECISION_STRING = UIUtils.GetResultKubunValue(Convert.ToString(obj.DW_DECISION));

                var messageList = _CommonRepository.GetMessageDetailsByProcessPlanCD(Convert.ToInt64(obj.PP_PROCESS_PLAN_CD), true);
                obj.DW_MESSAGE_LIST = messageList;

                obj.PP_ELONGATION = Helper.GetProductPatternFormat(obj.ELONGATION_PATTERN1.HasValue ? obj.ELONGATION_PATTERN1.Value : 0, obj.ELONGATION_PATTERN1_MIN_MAX.HasValue ? obj.ELONGATION_PATTERN1_MIN_MAX.Value : 0, Convert.ToDecimal(obj.ELONGATION_PATTERN2_MIN), Convert.ToDecimal(obj.ELONGATION_PATTERN2_MAX), obj.ELONGATION_PATTERN3_ACTUAL);
                obj.PP_TENSILE_STRENGTH = Helper.GetProductPatternFormat(Convert.ToSingle(obj.TENSILE_STRN_PATTERN1), obj.TENSILE_STRN_PATTERN1_MIN_MAX, obj.TENSILE_STRN_PATTERN2_MIN, obj.TENSILE_STRN_PATTERN2_MAX, obj.TENSILE_STRN_PATTERN3_ACTUAL);
                obj.PP_HARDNESS = Helper.GetHardnessProductPatternFormat(obj.HARDNESS_PATTERN1_CODE, obj.HARDNESS_PATTERN1, obj.HARDNESS_PATTERN1_MIN_MAX, obj.HARDNESS_PATTERN2_CODE, obj.HARDNESS_PATTERN2_MIN, obj.HARDNESS_PATTERN2_MAX, obj.HARDNESS_PATTERN3_ACTUAL);
                obj.PP_DECARB = Helper.GetDecarbProductPatternFormat(obj.DECARB_1,Convert.ToSingle(obj.DECARB_VAL_1), obj.DECARB_2, Convert.ToSingle(obj.DECARB_VAL_2), obj.DECARB_3, Convert.ToSingle(obj.DECARB_VAL_3));
                obj.PP_YEILD = Helper.GetProductPatternFormat(Convert.ToSingle(obj.YEILD_PATTERN1), obj.YEILD_PATTERN1_MIN_MAX, obj.YEILD_PATTERN2_MIN, obj.YEILD_PATTERN2_MAX, obj.YEILD_PATTERN3_ACTUAL);
                obj.PP_PRESS = Helper.GetProductPatternFormat(Convert.ToSingle(obj.PRESS_PATTERN1), obj.PRESS_PATTERN1_MIN_MAX, obj.PRESS_PATTERN2_MIN, obj.PRESS_PATTERN2_MAX, obj.PRESS_PATTERN3_ACTUAL);
            }
            return obj;
        }

        public void Delete(HikinukiBarToBarDetails request)
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                WipStatusDetails wipDetails = new WipStatusDetails();
                wipDetails = new WipStatusDetails();
                wipDetails.PROCESS_PLAN_CD = request.PP_PROCESS_PLAN_CD;
                wipDetails.TABA_NO = Convert.ToInt16(request.DW_TABA_NO_);

                if (request.DRAWING_CD > 0)
                {
                    _hikinukibartobarRepository.Delete(Convert.ToInt64(request.DRAWING_CD));
                }
                _CommonRepository.DeleteWIPStatusData(wipDetails);

                if (request.MSG_CD > 0)
                {
                    _CommonRepository.MessageDetailsDelete(Convert.ToInt64(request.MSG_CD));
                }
               
                transaction.Complete();
            }
        }


        public List<HikinukiBarToBarDetails> GetDrawingBarToBarDataOnFactoryDailyWorkStatus(GridSearchFields gridSearchRequest)
        {
            //List<ECDetails> result = new List<ECDetails>();
            List<SearchField> searchFields = ComposeSearchCriteriaforFactory(gridSearchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);
            int rowLimit = gridSearchRequest.ROW_LIMIT;
            var obj = _hikinukibartobarRepository.GetDrawingBarToBarDataOnFactoryDailyWorkStatus(searchFields, selectedSortCriteria, rowLimit, gridSearchRequest.CULTURE_NAME);
            return obj;
        }

        private List<SearchField> ComposeSearchCriteriaforFactory(GridSearchFields gridSearchRequest)
        {
            var searchFields = new List<SearchField>();
            if (gridSearchRequest.MACHINE_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "DRAWINGD.MACHINE_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MACHINE_CD)
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.LOCATION_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "DRAWINGD.LOCATION_CD",
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
                    FieldName = "DRAWINGD.DW_END_OPERATOR_CD",
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
            if (!string.IsNullOrEmpty(gridSearchRequest.SHAPE))
            {
                var searchField = new SearchField
                {
                    FieldName = " PP.PROD_SHAPE",
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
                    FieldName = "PP.PROD_LENGTH",
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
                        FieldName = "DRAWINGD.DW_END_DATE",
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
                        FieldName = "DRAWINGD.DW_END_DATE",
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
                    FieldName = "DRAWINGD.DW_END_DATE",
                    SearchType = SearchType.DateTime,
                    SearchText1 = gridSearchRequest.INSPECTION_DATE1,
                    SearchText2 = gridSearchRequest.INSPECTION_DATE2
                };
                searchFields.Add(searchField);
            }

            return searchFields;
        }
        public List<HikinukiBarToBarDetails> GetDailyProcessAnalysisHikinukiBarToBar(GridSearchFields gridSearchRequest)
        {

            List<SearchField> searchFields = ComposeSearchCriteriaforDailyProcessHikinukiBarToBar(gridSearchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);
            var result = _hikinukibartobarRepository.GetDailyProcessHikinukiBarToBar(searchFields, selectedSortCriteria, gridSearchRequest);
            return result;
        }

        
        private List<SearchField> ComposeSearchCriteriaforDailyProcessHikinukiBarToBar(GridSearchFields gridSearchRequest)
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

            //Drawing D search
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
                    FieldName = "DD.DW_START_DATE",
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
            if (!string.IsNullOrEmpty(gridSearchRequest.METALLIC_PLATE_CODE_STRING))
            {
                var searchField = new SearchField
                {
                    FieldName = "DD.DW_METALLIC_PLATE_CODE",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.METALLIC_PLATE_CODE_STRING.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.DIE_NO))
            {
                var searchField = new SearchField
                {
                    FieldName = "DD.DIE_NO",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.DIE_NO.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.HT_CONDITION ))
            {
                var searchField = new SearchField
                {
                    FieldName = "DD.HT_CONDITION",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.HT_CONDITION
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.END_DATE1))
            {
                var searchField = new SearchField
                {
                    FieldName = "DD.DW_END_DATE",
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
                    FieldName = "DD.DW_DECISION",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.DECISION)
                };
                searchFields.Add(searchField);
            }
            return searchFields;
        }
        public List<NameValue> GetDecisionList()
        {
            var _DecisionList = UIUtils.GetResultKubun();
            return _DecisionList;
        }
        public List<NameValue> GetDieDiaShapeList()
        {
            var _DieShapeList = UIUtils.GetDieShape();
            return _DieShapeList;
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
       
        public List<NameValue> GetHTMaterialList()
        {
            List<NameValue> ProcConditionKubunList = UIUtils.GetProcConditionKubun();
            return ProcConditionKubunList;
        }
        public List<NameValue> GetUnitTypeList()
        {
            var _UnitTypeList = UIUtils.GetUnitTypetKubun();
            return _UnitTypeList;
        }
        public HikinukiBarToBarDetails GetThabaDetailsByThabaNumber(string chuuban, short? gyouban, short? ThabaNo)
        {
            var result = _hikinukibartobarRepository.GetThabaDetailsByThabaNumber(chuuban, gyouban, ThabaNo);
            return result;
        }

       
        public int GetRowCountForHikinukiBarToBarFactory(GridSearchFields gridSearchRequest)
        {
            List<SearchField> searchFields = ComposeSearchCriteriaforFactory(gridSearchRequest);
            int count = _hikinukibartobarRepository.GetRowCountForHikinukiBarToBarFactory(searchFields);
            return count;
        }

        public int GetRowCountForHikinukiBarToBarDailyAnalysis(GridSearchFields gridSearchRequest)
        {
            List<SearchField> searchFields = ComposeSearchCriteriaforDailyProcessHikinukiBarToBar(gridSearchRequest);
            int count = _hikinukibartobarRepository.GetRowCountForHikinukiBarToBarDailyAnalysis(searchFields);
            return count;
        }
        public List<NameValue> GetHikinukiBarToBarMachineList()
        {
            var machineNameList = _hikinukibartobarRepository.GetHikinukiBarToBarMachineList();
            return machineNameList;
        }
    }
}
