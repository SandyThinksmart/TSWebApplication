using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using TSWebApplication.Application.IServices;
using TSWebApplication.Infrastructure.IRepositories;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;
using System.Globalization;
using TSWebApplication.Domain.Resources;
using System.Data;
using TSWebApplication.Infrastructure.Repositories;

namespace TSWebApplication.Application.Services
{
    public class StamppingService : IStamppingService
    {
        private readonly IStamppingRepository _stamppingRepository;
        private readonly IAllCommonRepository _CommonRepository;
        private readonly IUserLoginRepository _UserLoginRepository;
        private readonly LocalizationServices _localization;
        private readonly IAllCommonService _allCommonService;
        public StamppingService(IStamppingRepository stamppingRepository, IAllCommonRepository allCommonRepository, IUserLoginRepository userLoginRepository,LocalizationServices localization,IAllCommonService allCommonService)
        {
            _stamppingRepository = stamppingRepository;
            _CommonRepository = allCommonRepository;
            _UserLoginRepository = userLoginRepository;
            _localization = localization;
            _allCommonService = allCommonService;  
        }
        public List<StamppingDetails> GetStamppingList(GridSearchFields gridSearchRequest)
        {
            short LocationCD = Convert.ToInt16(_UserLoginRepository.GetUserLocationById(Convert.ToInt32(gridSearchRequest.USER_ID)));
            gridSearchRequest.LOCATION_CD = LocationCD;

            List<StamppingDetails> stamppingDetails = new List<StamppingDetails>();
            List<SearchField> searchFields = ComposeSearchCriteria(gridSearchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);
            stamppingDetails = _stamppingRepository.GetStamppingList(searchFields, selectedSortCriteria, gridSearchRequest.LOCATION_CD);
            return stamppingDetails;
        }

        public List<NameValue> GetFilteredEmployeeNameList(string searchText, bool addEmpty)
        {
            var customerList = new List<NameValue>();
            customerList = _stamppingRepository.GetFilteredEmployeeNameList(searchText, addEmpty);
            if (customerList != null && addEmpty)
            {
                var defaulItem = new NameValue { CD = 0, Name = string.Empty };
                customerList.Insert(0, defaulItem);
            }
            return customerList;
        }

        public StamppingDetails GetStamppingDetailsByStamppingCD(long stamppingCD, string cultureName)
        {
            var _stamppingDetails = _stamppingRepository.GetStamppingDetailsByStamppingCD(stamppingCD);
            if (_stamppingDetails != null)
            {
                if (cultureName == "en")
                {
                    System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
                }
                else
                {
                    System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("ja-JP");
                }
                if (_stamppingDetails.PP_STD_DEL_PERIOD >= 0)
                {
                    _stamppingDetails.PP_STD_DEL_PERIOD_STRING = _stamppingDetails.PP_STD_DEL_PERIOD + " " + _localization.GetLocalizedHtmlString("lbl_Day");
                }

                if (_stamppingDetails.PP_UNIT > 0)
                {
                    _stamppingDetails.PP_NO_OF_UNITS_DECODE = Convert.ToDecimal(_stamppingDetails.PP_NO_OF_UNITS).ToString("0.####") + UIUtils.GetProductUnitKubunValue(Convert.ToString(_stamppingDetails.PP_UNIT));
                }
                if (_stamppingDetails.PP_UNIT_WT_KUBUN > 0)
                {
                    _stamppingDetails.PP_UNIT_WT_KUBUN_STRING = Utils.GetUnitWeigth(Convert.ToInt32(_stamppingDetails.PP_UNIT_WT_KUBUN), Convert.ToInt32(_stamppingDetails.PP_UNIT_WT), Convert.ToInt32(_stamppingDetails.UNIT_WT_MIN), Convert.ToInt32(_stamppingDetails.UNIT_WT_MAX));

                    if (_stamppingDetails.PP_UNIT_WT_KUBUN == 3)
                    {
                        _stamppingDetails.PP_UNIT_WT_KUBUN_STRING = Convert.ToString(_stamppingDetails.PP_UNIT_WT) + _localization.GetLocalizedHtmlString("lbl_Hone");
                    }


                }

                if (_stamppingDetails.MAGARI_KUBUN > 0)
                {
                    _stamppingDetails.PP_MAGARI_BEND_TOL = Utils.GetMagariBendTolSpan(UIUtils.Instance.GetMagariKubunValue(Convert.ToString(_stamppingDetails.MAGARI_KUBUN)), Convert.ToDecimal(_stamppingDetails.MAGARI_BEND_TOL), Convert.ToInt32(_stamppingDetails.MAGARI_SPAN));  //Convert.ToDecimal(_stamppingDetails.PP_NO_OF_UNITS).ToString("0.####") + UIUtils.Instance.GetProductUnitKubunValue(Convert.ToString(_stamppingDetails.PP_UNIT));
                }

                //if (_stamppingDetails.PP_COLOR_ID > 0)
                //{
                //    _stamppingDetails.PP_COLOR = UIUtils.GetColorKubunValue(Convert.ToString(_stamppingDetails.PP_COLOR_ID));   // Utils.GetColorName(Convert.ToInt16(_stamppingDetails.PP_COLOR_ID));

                //}
                if (_stamppingDetails.PP_COIL_DIA_WINDING != null)
                {
                    _stamppingDetails.PP_COIL_DIA_WINDING = UIUtils.GetCoilDiaWindingKubunValue(Convert.ToString(_stamppingDetails.PP_COIL_DIA_WINDING)); //  Utils.GetCoilDiaWindingValue(Convert.ToInt16(_stamppingDetails.PP_COIL_DIA_WINDING));
                }
                return _stamppingDetails;
            }
            else
            {
                return new StamppingDetails();
            }
        }
        public List<NameValue> GetNextProcessList()
        {
            var _nextProcessList = UIUtils.GetNextProcesKubun();
            return _nextProcessList;
        }

        public List<NameValue> GetStamp()
        {
            var _nextStampList = UIUtils.GetStampKubun();
            return _nextStampList;
        }

        public void DeleteStamppingDetails(StamppingDetails request)
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                WipStatusDetails wipDetails = new WipStatusDetails();
                wipDetails = new WipStatusDetails();
                wipDetails.PROCESS_PLAN_CD = request.PROCESS_PLAN_CD;
                wipDetails.TABA_NO = Convert.ToInt16(request.ST_TABA_NO);

                if (request.STAMPING_CD > 0)
                {
                    _stamppingRepository.DeleteStamppingDetails(request.STAMPING_CD);
                }                
                _CommonRepository.DeleteWIPStatusData(wipDetails);

                if (request.MSG_CD > 0)
            {
                _CommonRepository.MessageDetailsDelete(Convert.ToInt64(request.MSG_CD));
            }
                transaction.Complete();
            }
        }

        public long UpdateStamppingDetails(StamppingDetails request)
        {
            long msgCd = 0;
            bool deleteMsg = false;            
            request.PROCESS_KUBUN_CD = Convert.ToInt32(UIUtils.GetProcessKubun(_localization.GetLocalizedHtmlString("lbl_ProcessStamping")));
            if (request.USER_ID != null && request.USER_ID > 0)
            {
                request.INPUT_LOCATION_CD = _UserLoginRepository.GetUserLocationById(request.USER_ID);
               // request.LOCATION_CD= _UserLoginRepository.GetUserLocationById(request.USER_ID);
            }
            if (request.MACHINE_CD != null && request.MACHINE_CD > 0)
            {
                request.LOCATION_CD = Convert.ToInt32(_CommonRepository.GetMachineNameByID(request.MACHINE_CD)?.LOCATION_CD);
            }
            using (TransactionScope transaction = new TransactionScope())
            {
                MessageDetails messageDetails = new MessageDetails();
                messageDetails.PROCESS_PLAN_CD = Convert.ToInt64(request.PROCESS_PLAN_CD);
                messageDetails.MACHINE_CD = Convert.ToInt32(request.MACHINE_CD);
                messageDetails.PROCESS_KUBUN_ID = Convert.ToInt32(UIUtils.GetProcessKubun(_localization.GetLocalizedHtmlString("lbl_ProcessStamping")));
                messageDetails.OPERATOR_CD = Convert.ToInt64(request.ST_END_OPERATOR_CD);
                messageDetails.MSG_CONTENTS = request.ST_REMARKS;
                messageDetails.MSG_CD = Convert.ToInt32(request.MSG_CD);
                messageDetails.USER_ID = Convert.ToInt32(request.USER_ID);
                messageDetails.INPUT_LOCATION_CD = Convert.ToInt32(request.INPUT_LOCATION_CD);

                if (request.MSG_CD == Constants.NullValue.INT && !string.IsNullOrEmpty(request.ST_REMARKS))
                {
                   
                        messageDetails = _CommonRepository.MessageDetailsInsert(messageDetails);

                        if (messageDetails.MSG_CD != Constants.NullValue.INT && messageDetails.MSG_CD > 0)
                        {
                            request.MSG_CD = Convert.ToInt32(messageDetails.MSG_CD);
                        }
                                       
                }
                // If REMARKS was already present, and it is updated,  
                // then UPDATE the TRN_MESSAGE
                else if (request.MSG_CD > 0 && request.ST_REMARKS != string.Empty)
                {
                   
                    _CommonRepository.MessageDetailsUpdate(messageDetails);

                   
                }
                // If REMARKS was already present, but it is delete now,
                // then DELETE the TRN_MESSAGE
                else if (request.MSG_CD > 0 && request.ST_REMARKS == string.Empty)
                {
                    long messageCD = Convert.ToInt64(request.MSG_CD);
                    request.MSG_CD = null;
                                        
                    msgCd = _stamppingRepository.UpdateStamppingDetails(request);
                    _CommonRepository.MessageDetailsDelete(messageCD);
                    deleteMsg = true;
                }
                if (deleteMsg == false)
                {
                    msgCd = _stamppingRepository.UpdateStamppingDetails(request);
                }
                if (request.ST_NEXT_PROCESS != 999)
                {

                    WipStatusDetails wipDetails = new WipStatusDetails();

                    wipDetails = new WipStatusDetails();
                    wipDetails.PROCESS_PLAN_CD = request.PROCESS_PLAN_CD;
                    wipDetails.LOCATION_CD = Convert.ToInt16(request.LOCATION_CD);
                    wipDetails.MSG_CD = request.MSG_CD;
                    wipDetails.FINISHED_BARS = request.ST_OK_BARS;
                    wipDetails.UNIT = request.UNIT_TYPE;
                    wipDetails.FINISHED_QTY = request.ST_OK_QTY;
                    wipDetails.PROCESS_START_DATE = request.ST_START_DATE;
                    wipDetails.PROCESS_END_DATE = request.ST_END_DATE;
                    wipDetails.PROCESS_KUBUN_CD = request.PROCESS_KUBUN_CD;
                    wipDetails.PRE_PROSESS_DECISION = request.ST_DECISION;
                    wipDetails.NEXT_PROCESS = Convert.ToInt16(request.ST_NEXT_PROCESS);
                    wipDetails.START_OPERATOR_CD = request.ST_START_OPERATOR_CD;
                    wipDetails.END_OPERATOR_CD = request.ST_END_OPERATOR_CD;
                    wipDetails.TABA_NO = request.ST_TABA_NO;
                    wipDetails.NIFUDA_CD = null;

                    //_CommonRepository.InsertWIPStatusData(wipDetails);
                    _CommonRepository.DeleteWIPStatusData(wipDetails);
                }

                transaction.Complete();
            }
            
            return msgCd;
        }

        public List<NameValue> GetMachineNameList()
        {
            var machineNameList = _stamppingRepository.GetMachineNameList();
            return machineNameList;
        }
        public List<NameValue> GetLocationNameList()
        {
            var locationNameList = _stamppingRepository.GetLocationNameList();
            return locationNameList;
        }
        public List<NameValue> GetCustomerNameList()
        {
            var customerNameList = _stamppingRepository.GetCustomerNameList();
            return customerNameList;
        }

        public StamppingDetails GetStamppingDetailsByChuuban(string PP_CHUUBAN, short? PP_GYOUBAN, int MACHINE_CD, string NIFUDA_NO, string cultureName)
        {

            if (cultureName == "en")
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
            }
            else
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("ja-JP");
            }

            var obj = _stamppingRepository.GetStamppingDetailsByChuuban(PP_CHUUBAN, PP_GYOUBAN, MACHINE_CD, NIFUDA_NO);

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
                if (obj.PP_UNIT_WT_KUBUN > 0)
                {
                    obj.PP_UNIT_WT_KUBUN_STRING = Utils.GetUnitWeigth(Convert.ToInt32(obj.PP_UNIT_WT_KUBUN), Convert.ToInt32(obj.PP_UNIT_WT), Convert.ToInt32(obj.UNIT_WT_MIN), Convert.ToInt32(obj.UNIT_WT_MAX));

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
                    
                    obj.PP_MAGARI_BEND_TOL = Utils.GetMagariBendTolSpan(UIUtils.Instance.GetMagariKubunValue(Convert.ToString(obj.MAGARI_KUBUN)), Convert.ToInt32(obj.MAGARI_BEND_TOL), Convert.ToInt32(obj.MAGARI_SPAN));  //Convert.ToDecimal(obj.PP_NO_OF_UNITS).ToString("0.####") + UIUtils.Instance.GetProductUnitKubunValue(Convert.ToString(obj.PP_UNIT));
                    
                }

                //if (obj.PP_COLOR_ID > 0)
                //{
                //    obj.PP_COLOR = Utils.GetColorName(Convert.ToInt16(obj.PP_COLOR_ID));
                //}

                if (!string.IsNullOrEmpty(obj.PP_COIL_DIA_WINDING))
                {
                    obj.PP_COIL_DIA_WINDING = UIUtils.GetCoilDiaWindingKubunValue(Convert.ToString(obj.PP_COIL_DIA_WINDING));
                }

                var messageList = _CommonRepository.GetMessageDetailsByProcessPlanCD(Convert.ToInt64(obj.PP_PROCESS_PLAN_CD), true);
                obj.DW_MESSAGE_LIST = messageList;

                obj.PP_ELONGATION = Helper.GetProductPatternFormat(obj.ELONGATION_PATTERN1.HasValue ? obj.ELONGATION_PATTERN1.Value : 0, obj.ELONGATION_PATTERN1_MIN_MAX, Convert.ToDecimal(obj.ELONGATION_PATTERN2_MIN), Convert.ToDecimal(obj.ELONGATION_PATTERN2_MAX), obj.ELONGATION_PATTERN3_ACTUAL);
                obj.PP_TENSILE_STRENGTH = Helper.GetProductPatternFormat(Convert.ToSingle(obj.TENSILE_STRN_PATTERN1), obj.TENSILE_STRN_PATTERN1_MIN_MAX, obj.TENSILE_STRN_PATTERN2_MIN, obj.TENSILE_STRN_PATTERN2_MAX, obj.TENSILE_STRN_PATTERN3_ACTUAL);
                obj.PP_HARDNESS = Helper.GetHardnessProductPatternFormat(obj.HARDNESS_PATTERN1_CODE, obj.HARDNESS_PATTERN1, obj.HARDNESS_PATTERN1_MIN_MAX, obj.HARDNESS_PATTERN2_CODE, obj.HARDNESS_PATTERN2_MIN, obj.HARDNESS_PATTERN2_MAX, obj.HARDNESS_PATTERN3_ACTUAL);
                obj.PP_DECARB = Helper.GetDecarbProductPatternFormat(obj.DECARB_1, Convert.ToSingle(obj.DECARB_VAL_1), obj.DECARB_2, Convert.ToSingle(obj.DECARB_VAL_2), obj.DECARB_3, Convert.ToSingle(obj.DECARB_VAL_3));
                obj.PP_YEILD = Helper.GetProductPatternFormat(Convert.ToSingle(obj.YEILD_PATTERN1), obj.YEILD_PATTERN1_MIN_MAX, obj.YEILD_PATTERN2_MIN, obj.YEILD_PATTERN2_MAX, obj.YEILD_PATTERN3_ACTUAL);
                obj.PP_PRESS = Helper.GetProductPatternFormat(Convert.ToSingle(obj.PRESS_PATTERN1), obj.PRESS_PATTERN1_MIN_MAX, obj.PRESS_PATTERN2_MIN, obj.PRESS_PATTERN2_MAX, obj.PRESS_PATTERN3_ACTUAL);

            }

            return obj;
        }

        public List<ThabaDetails> GetThabaDetails(string chuuban, short? gyuban)
        {
            var result =  _stamppingRepository.GetThabaDetails(chuuban, gyuban);

            foreach (ThabaDetails record in result)
            {

                string decisionName = UIUtils.GetResultKubunValue(Convert.ToString(record.DECISION));
                if (_localization.GetLocalizedHtmlString("lbl_OK") == decisionName)
                {
                    record.DECISION = 1;
                }
                if (_localization.GetLocalizedHtmlString("lbl_NG") == decisionName)
                {
                    record.DECISION = 2;
                }
                if (_localization.GetLocalizedHtmlString("lbl_Hold") == decisionName)
                {
                    record.DECISION = 3;
                }
                if (_localization.GetLocalizedHtmlString("lbl_DecisionKubun4of4") == decisionName)
                {
                    record.DECISION = 4;
                }
                if (_localization.GetLocalizedHtmlString("lbl_ReHeatTreatment") == decisionName)
                {
                    record.DECISION = 5;
                }
                if (_localization.GetLocalizedHtmlString("lbl_ReDrawing") == decisionName)
                {
                    record.DECISION = 5;
                }
                if (_localization.GetLocalizedHtmlString("lbl_Restraightening") == decisionName)
                {
                    record.DECISION = 5;
                }
                if (_localization.GetLocalizedHtmlString("lbl_Re_Chamfering") == decisionName)
                {
                    record.DECISION = 5;
                }
                if (_localization.GetLocalizedHtmlString("lbl_ReEc") == decisionName)
                {
                    record.DECISION = 5;
                }

            }
            return result;
        }

        public List<StamppingDetails> GetThabaDetailsByThabaNumber(string chuuban, short? gyouban, short? Thaba_No)
        {
            var result = _stamppingRepository.GetThabaDetailsByThabaNumber(chuuban, gyouban, Thaba_No);
            return result;
        }

        public List<StamppingDetails> GetStampping24MachineList(string machineCd, string cultureName)
        {
            var machineDetails = _stamppingRepository.GetStampping24MachineList(machineCd);
            if (cultureName == "en")
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
            }
            else
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("ja-JP");
            }
            if (machineDetails != null)
            {
                foreach (StamppingDetails record in machineDetails)
                {
                    string decisionName = UIUtils.GetResultKubunValue(Convert.ToString(record.ST_DECISION));
                    if (_localization.GetLocalizedHtmlString("lbl_OK") == decisionName)
                    {
                        record.ST_DECISION = 1;
                    }
                    if (_localization.GetLocalizedHtmlString("lbl_NG") == decisionName)
                    {
                        record.ST_DECISION = 2;
                    }
                    if (_localization.GetLocalizedHtmlString("lbl_Hold") == decisionName)
                    {
                        record.ST_DECISION = 3;
                    }
                    if (_localization.GetLocalizedHtmlString("lbl_DecisionKubun4of4") == decisionName)
                    {
                        record.ST_DECISION = 4;
                    }
                    if (_localization.GetLocalizedHtmlString("lbl_ReHeatTreatment") == decisionName)
                    {
                        record.ST_DECISION = 5;
                    }
                    if (_localization.GetLocalizedHtmlString("lbl_ReDrawing") == decisionName)
                    {
                        record.ST_DECISION = 5;
                    }
                    if (_localization.GetLocalizedHtmlString("lbl_Restraightening") == decisionName)
                    {
                        record.ST_DECISION = 5;
                    }
                    if (_localization.GetLocalizedHtmlString("lbl_Re_Chamfering") == decisionName)
                    {
                        record.ST_DECISION = 5;
                    }
                    if (_localization.GetLocalizedHtmlString("lbl_ReEc") == decisionName)
                    {
                        record.ST_DECISION = 5;
                    }
                    record.ST_UNIT_TYPE_STRING = UIUtils.GetProductUnitKubunValue(Convert.ToString(record.ST_UNIT_TYPE));
                    record.STAMP_DETAILS = UIUtils.GetStampbykubun(record.STAMP_DETAILS);
                }

            }
            return machineDetails;
        }

        public string GetEmployeeNameByID(long employeeCd)
        {
            return (employeeCd > 0) ? _stamppingRepository.GetEmployeeNameByID(employeeCd) : null;
        }

        public StamppingDetails InsertStamppingDetails(StamppingDetails request)
        {
            StamppingDetails result = new StamppingDetails();
            using (TransactionScope transaction = new TransactionScope())
            {
              
            request.PROCESS_KUBUN_CD = Convert.ToInt32(UIUtils.GetProcessKubun(_localization.GetLocalizedHtmlString("lbl_ProcessStamping")));
            if (request.USER_ID != null && request.USER_ID > 0)
            {
                request.INPUT_LOCATION_CD = _UserLoginRepository.GetUserLocationById(request.USER_ID);
               // request.LOCATION_CD= _UserLoginRepository.GetUserLocationById(request.USER_ID);
                }
                if (request.MACHINE_CD != null && request.MACHINE_CD > 0)
                {
                    request.LOCATION_CD = _CommonRepository.GetMachineNameByID(Convert.ToInt64(request.MACHINE_CD)).LOCATION_CD;
                }
               

            WipStatusDetails wipDetails = new WipStatusDetails();

            wipDetails = new WipStatusDetails();
            wipDetails.PROCESS_PLAN_CD = request.PROCESS_PLAN_CD;
            wipDetails.LOCATION_CD = Convert.ToInt16(request.LOCATION_CD);
            wipDetails.MSG_CD = null;
            wipDetails.FINISHED_BARS = null;
            wipDetails.UNIT = null;
            wipDetails.FINISHED_QTY = null;
            wipDetails.PROCESS_START_DATE = request.ST_START_DATE;
            wipDetails.PROCESS_END_DATE = null;
            wipDetails.PROCESS_KUBUN_CD = request.PROCESS_KUBUN_CD;
            wipDetails.PRE_PROSESS_DECISION = null;
            wipDetails.NEXT_PROCESS = null;
            wipDetails.START_OPERATOR_CD = request.ST_START_OPERATOR_CD;
            wipDetails.END_OPERATOR_CD = null;
            if (request.ST_TABA_NO != null)
            {
                wipDetails.TABA_NO = Convert.ToInt16(request.ST_TABA_NO);
            }
            else
            {
                wipDetails.TABA_NO = 0;
            }
            wipDetails.NIFUDA_CD = null;

            _CommonRepository.DeleteWIPStatusData(wipDetails);

                 _stamppingRepository.InsertStamppingDetails(request);
            result = GetStamppingDetailsByChuuban(request.PP_CHUUBAN, request.PP_GYOUBAN, request.MACHINE_CD, request.NIFUDA_NO, request.CULTURE_NAME);
                transaction.Complete();

            }
            return result;
        }

        public int GetUserLocationById(int userID)
        {
            return _UserLoginRepository.GetUserLocationById(userID);
        }

        private List<SearchField> ComposeSearchCriteria(GridSearchFields gridSearchRequest)
        {
            var searchFields = new List<SearchField>();

            if (!string.IsNullOrEmpty(gridSearchRequest.CHUUBAN))
            {
                string chuuban = string.Empty;               

                var searchField = new SearchField
                {
                    FieldName = "CONCAT(PP.CHUUBAN_YEAR, CONCAT(PP.CHUUBAN, CASE WHEN LENGTH(TRIM(PP.GYOUBAN)) = 1 THEN '0' || PP.GYOUBAN END))",
                               
                    SearchType = SearchType.Text,

                    SearchText1 = gridSearchRequest.CHUUBAN

                };

                searchFields.Add(searchField);
            }
            if (gridSearchRequest.TABA_NO > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "ST.ST_TABA_NO",
                    SearchType = SearchType.Text,
                    SearchText1 = Convert.ToString(gridSearchRequest.TABA_NO)
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
            return searchFields;
        }
        public List<NameValue> GetDecisionList()
        {
            var _DecisionList = UIUtils.GetResultKubun();
            return _DecisionList;
        }
        public List<NameValue> GetTrialResultList()
        {
            var _TrialResultList = UIUtils.GetTrialResultKubun();
            return _TrialResultList;
        }
        public List<NameValue> GetUnitTypeList()
        {
            var _UnitTypeList = UIUtils.GetUnitTypetKubun();
            return _UnitTypeList;
        }
        public List<StamppingDetails> GetStamppingDailyReportOnFactoryDailyWorkStatus(GridSearchFields gridSearchRequest)
        {
            List<SearchField> searchFields = ComposeSearchCriteriaforFactoryDailyWorkStatus(gridSearchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);
            int rowLimit = gridSearchRequest.ROW_LIMIT;
            var result = _stamppingRepository.GetStamppingDailyReportOnFactoryDailyWorkStatus(searchFields, selectedSortCriteria, rowLimit, gridSearchRequest.CULTURE_NAME);           
            return result;
        }

        private List<SearchField> ComposeSearchCriteriaforFactoryDailyWorkStatus(GridSearchFields gridSearchRequest)
        {
            var searchFields = new List<SearchField>();
            if (gridSearchRequest.MACHINE_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "ST.MACHINE_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MACHINE_CD)
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.LOCATION_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "ST.LOCATION_CD",
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
                    FieldName = "ST.ST_END_OPERATOR_CD",
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
                    SearchText1 ="'" + Convert.ToString(gridSearchRequest.PROD_SPEC_NO1) + "'"
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_SPEC_NO2))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_SPEC_NO2",
                    SearchType = SearchType.Number,
                    SearchText1 ="'" + Convert.ToString(gridSearchRequest.PROD_SPEC_NO2) + "'"
                };
                searchFields.Add(searchField);
            }
            if (string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE2))
            {
                if (!string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE1))
                {
                    var searchField = new SearchField
                    {
                        FieldName = "ST.ST_END_DATE",
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
                        FieldName = "ST.ST_END_DATE",
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
                    FieldName = "ST.ST_END_DATE",
                    SearchType = SearchType.DateTime,
                    SearchText1 = gridSearchRequest.INSPECTION_DATE1,
                    SearchText2 = gridSearchRequest.INSPECTION_DATE2
                };
                searchFields.Add(searchField);
            }
           
            return searchFields;
        }

        public List<StamppingDetails> GetDailyProcessAnalysisStamppingReport(GridSearchFields gridSearchRequest)
        {

            List<SearchField> searchFields = ComposeSearchCriteriaforDailyProcessStamppingReport(gridSearchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);
            var result = _stamppingRepository.GetDailyProcessStamppingReport(searchFields, selectedSortCriteria, gridSearchRequest);            
            return result;
        }
        private List<SearchField> ComposeSearchCriteriaforDailyProcessStamppingReport(GridSearchFields gridSearchRequest)
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
            //stampping search
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
                    FieldName = "ST.ST_START_DATE",
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
                    FieldName = "ST.ST_METALLIC_PLATE_CODE",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.METALLIC_PLATE_CODE_STRING.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
           
            if (!string.IsNullOrEmpty(gridSearchRequest.END_DATE1))
            {
                var searchField = new SearchField
                {
                    FieldName = "ST.ST_END_DATE",
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
                    FieldName = "ST.ST_DECISION",
                    SearchType = SearchType.Text,
                    SearchText1 = Convert.ToString(gridSearchRequest.DECISION)
                };
                searchFields.Add(searchField);
            }
            return searchFields;
        }
                            
        public int GetRowCountForStamppingFactory(GridSearchFields gridSearchRequest)
        {
            List<SearchField> searchFields = ComposeSearchCriteriaforFactoryDailyWorkStatus(gridSearchRequest);
            int count = _stamppingRepository.GetRowCountForStamppingFactory(searchFields);
            return count;
        }

        public int GetRowCountForStamppingDailyAnalysis(GridSearchFields gridSearchRequest)
        {
            List<SearchField> searchFields = ComposeSearchCriteriaforDailyProcessStamppingReport(gridSearchRequest);
            int count = _stamppingRepository.GetRowCountForStamppingDailyAnalysis(searchFields);
            return count;
        }

        
    }
}
