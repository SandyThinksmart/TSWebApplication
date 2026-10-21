
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using TSWebApplication.Application.IServices;
using TSWebApplication.Infrastructure.IRepositories;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;
using static TSWebApplication.Domain.Utility.Constants;
using TSWebApplication.Infrastructure.Repositories;
using System.Reflection.PortableExecutable;
using System.Globalization;
using TSWebApplication.Domain.Resources;
using System.Data;
using System.ComponentModel.DataAnnotations;

namespace TSWebApplication.Application.Services
{
    public class HeatTreatmentService : IHeatTreatmentService
    {

        private readonly IHeatTreatmentRepository _heatTreatmentRepository;
        private readonly IAllCommonRepository _CommonRepository;
        private readonly LocalizationServices _localization;
        private readonly IAllCommonService _allCommonService;
        public HeatTreatmentService(IHeatTreatmentRepository heatTreatmentRepository, IAllCommonRepository commonRepository, LocalizationServices localization, IAllCommonService allCommonService)
        {
            _heatTreatmentRepository = heatTreatmentRepository;
            _CommonRepository = commonRepository;
            _localization = localization;
            _allCommonService = allCommonService;
        }
        public List<HeatTreatMentDetails> GetHeatTreatmentEditDeleteStartEndDetails(int? HeatTreatMentCd, bool isNull)
        {
            var startDetails = _heatTreatmentRepository.GetHeatTreatmentEditDeleteStartEndDetails(HeatTreatMentCd, isNull);

            return startDetails;
        }
        public List<HeatTreatMentDetails> GetHeatTreatmentStartDetails(int? machineCD, bool isNull)
        {
            var startDetails = _heatTreatmentRepository.GetHeatTreatmentDetails(machineCD, isNull);

            return startDetails;
        }
        public List<HeatTreatMentDetails> GetFurnaceNo(string furnaceNo)
        {
            var startDetails = _heatTreatmentRepository.GetFurnaceNoDetails(furnaceNo);

            return startDetails;
        }

        public bool IsMachineAlreadyStarted(int machineCD)
        {

            return _heatTreatmentRepository.IsMachineStarted(machineCD); ;
        }
        public List<NameValue> GetFilteredEmployeeNameList(string searchText, bool addEmpty)
        {
            var customerList = new List<NameValue>();

            customerList = _heatTreatmentRepository.GetFilteredEmployeeNameList(searchText, addEmpty);
            if (customerList != null && addEmpty)
            {
                var defaulItem = new NameValue { CD = 0, Name = string.Empty };
                customerList.Insert(0, defaulItem);
            }
            return customerList;
        }

        public List<NameValue> GetMachineNameList()
        {
            var machineNameList = _heatTreatmentRepository.GetMachineNameList();
            return machineNameList;
        }
        public List<NameValue> GetHeatTreatmentMachineNameList()
        {
            var machineNameList = _heatTreatmentRepository.GetHeatTreatmentMachineNameList();
            return machineNameList;
        }
        public List<NameValue> GetLocationNameList()
        {
            var locationNameList = _heatTreatmentRepository.GetFactoryLocationNameList();
            return locationNameList;
        }
       
        public List<NameValue> GetProcConditionKubun()
        {
            List<NameValue> ProcConditionKubunList = UIUtils.GetProcConditionKubun();
            return ProcConditionKubunList;
        }

        public List<NameValue> GetMaterialKubun()
        {
            List<NameValue> MatlKubunList = UIUtils.GetMaterialKubun();
            return MatlKubunList;
        }
        public List<NameValue> GetUnitKubunList()
        {
            List<NameValue> UnitKubunList = UIUtils.GetMaterialUnitKubun(true);
            return UnitKubunList;
        }

        public HeatTreatMentDetails GetChuubanListForInvaidCheck(string enteredCChubaans)
        {
            return _heatTreatmentRepository.GetChuubanListForInvaidCheck(enteredCChubaans);
        }

        public List<string> GetCompletedChuubanList(string enteredCChubaans)
        {
            return _heatTreatmentRepository.GetCompletedChuubanList(enteredCChubaans);
        }

        public List<HeatTreatMentDetails> SetList(List<string> chubaanNos)
        {
            List<ProductOrderCompletion> productOrderDetailsList = new List<ProductOrderCompletion>();
            List<HeatTreatMentDetails> HeatTreatmentDetailsList = new List<HeatTreatMentDetails>();
            productOrderDetailsList = _heatTreatmentRepository.GetSearchedProductOrderDetails(chubaanNos, 0);
            if (productOrderDetailsList != null && productOrderDetailsList.Count > 0)
            {
                foreach (var item in productOrderDetailsList)
                {
                    var heatTreatmentDetails = new HeatTreatMentDetails();

                    heatTreatmentDetails.CUSTOMER_NAME = item.CUSTOMER;
                    heatTreatmentDetails.CHUUBAN = item.CHUUBAN;
                    heatTreatmentDetails.PROD_NAME_CUST = item.PROD_NAME_CUST;
                    heatTreatmentDetails.CHARGE = item.CHARGE;
                    heatTreatmentDetails.MATL_DIM_STRING = item.MATL_DIM_STRING;
                    heatTreatmentDetails.PROD_DIM_STRING = item.PROD_DIM_STRING;
                    heatTreatmentDetails.PROCESS_PLAN_CD = Convert.ToInt64(item.PROCESS_PLAN_CD);

                    HeatTreatmentDetailsList.Add(heatTreatmentDetails);
                }
            }
            return HeatTreatmentDetailsList;
        }

        public bool IsFurProcFurNoIsExist(int machineCD, string furnaceNo)
        {
            return _heatTreatmentRepository.IsFurProcFurNoIsExist(machineCD, furnaceNo);
        }

        public List<HeatTreatMentDetails> SaveStartData(List<HeatTreatMentDetails> requestData)
        {
            List < HeatTreatMentDetails > result = new List<HeatTreatMentDetails>();
            using (TransactionScope transaction = new TransactionScope())
            {
                foreach (var request in requestData)
                {
                    if (request.USER_ID != null && request.USER_ID > 0)
                    {
                        request.INPUT_LOCATION_CD = Convert.ToInt16(_CommonRepository.GetUserLocationById(request.USER_ID.Value));
                    }
                    if (request.HT_NEXT_PROCESS == 0)
                    {
                        request.HT_NEXT_PROCESS = null;
                    }


                    //  request.LOCATION_CD = Convert.ToInt16(request.INPUT_LOCATION_CD);

                    if (request.MACHINE_CD != null && request.MACHINE_CD > 0)
                    {
                        request.LOCATION_CD = Convert.ToInt16(_CommonRepository.GetMachineNameByID(Convert.ToInt64(request.MACHINE_CD.Value))?.LOCATION_CD);
                    }
                    request.PROCESS_KUBUN_ID = Convert.ToInt32(UIUtils.GetProcessKubun(_localization.GetLocalizedHtmlString("lbl_ProcessHeatTreatment")));
                    if (request.MSG_CD == Constants.NullValue.INT && !string.IsNullOrEmpty(request.HT_REMARKS))
                    {
                        MessageDetails messageDetails = new MessageDetails();
                        messageDetails.PROCESS_PLAN_CD = Convert.ToInt64(request.PROCESS_PLAN_CD);
                        messageDetails.MACHINE_CD = Convert.ToInt32(request.MACHINE_CD);
                        messageDetails.PROCESS_KUBUN_ID = Convert.ToInt32(UIUtils.GetProcessKubun(_localization.GetLocalizedHtmlString("lbl_ProcessHeatTreatment")));
                        messageDetails.OPERATOR_CD = Convert.ToInt64(request.OFFLOAD_CONF_BY);
                        messageDetails.MSG_CONTENTS = request.HT_REMARKS;
                        messageDetails.MSG_CD = Convert.ToInt32(request.MSG_CD);
                        messageDetails.USER_ID = Convert.ToInt32(request.USER_ID);
                        messageDetails.INPUT_LOCATION_CD = Convert.ToInt32(request.INPUT_LOCATION_CD);

                        messageDetails = _CommonRepository.MessageDetailsInsert(messageDetails);

                        if (messageDetails.MSG_CD != Constants.NullValue.INT && messageDetails.MSG_CD > 0)
                        {
                            request.MSG_CD = Convert.ToInt32(messageDetails.MSG_CD);
                        }
                    }
                    result= _heatTreatmentRepository.InsertStartData(request);

                    if (request.HT_NEXT_PROCESS != 999)
                    {
                        if (request.HT_TABA_NO_1 != null || request.HT_TABA_NO_2 != null || request.HT_TABA_NO_3 != null || request.HT_TABA_NO_4 != null)
                        {
                            WipStatusDetails wipDetails = new WipStatusDetails();

                            wipDetails = new WipStatusDetails();
                            wipDetails.PROCESS_PLAN_CD = request.PROCESS_PLAN_CD;
                            wipDetails.LOCATION_CD = Convert.ToInt16(request.LOCATION_CD);
                            wipDetails.MSG_CD = request.MSG_CD;
                            wipDetails.FINISHED_BARS = request.HT_NO_OF_UNITS;
                            wipDetails.UNIT = request.UNIT_TYPE;
                            wipDetails.FINISHED_QTY = request.HT_WEIGHT;
                            wipDetails.PROCESS_START_DATE = request.HT_START_DATE;
                            wipDetails.PROCESS_END_DATE = null;
                            wipDetails.PROCESS_KUBUN_CD = request.PROCESS_KUBUN_ID;
                            wipDetails.PRE_PROSESS_DECISION = null;
                            wipDetails.NEXT_PROCESS = request.HT_NEXT_PROCESS;
                            wipDetails.START_OPERATOR_CD = Convert.ToInt32(request.SETUP_CONF_BY);
                            wipDetails.END_OPERATOR_CD = null;
                            wipDetails.TABA_NO = 0;
                            wipDetails.NIFUDA_CD = null;
                            if (request.HT_TABA_NO_1 != null && request.HT_TABA_NO_2 != null || request.HT_TABA_NO_3 != null || request.HT_TABA_NO_4 != null)
                            {
                                wipDetails.TABA_GROUP_NO = request.HT_TABA_NO_1;
                            }
                            if (request.HT_TABA_NO_1 != null)
                            {
                                wipDetails.TABA_NO = request.HT_TABA_NO_1;
                                _CommonRepository.DeleteWIPStatusData(wipDetails);
                            }
                            
                           

                            if (request.HT_TABA_NO_2 > 0)
                            {

                                wipDetails.FINISHED_QTY = null;
                                wipDetails.FINISHED_BARS = null;
                                wipDetails.UNIT = null;
                                wipDetails.TABA_NO = request.HT_TABA_NO_2;
                                _CommonRepository.DeleteWIPStatusData(wipDetails);
                            }
                            if (request.HT_TABA_NO_3 > 0)
                            {

                                wipDetails.FINISHED_QTY = null;
                                wipDetails.FINISHED_BARS = null;
                                wipDetails.UNIT = null;
                                wipDetails.TABA_NO = request.HT_TABA_NO_3;
                                _CommonRepository.DeleteWIPStatusData(wipDetails);
                            }
                            if (request.HT_TABA_NO_4 > 0)
                            {

                                wipDetails.FINISHED_QTY = null;
                                wipDetails.FINISHED_BARS = null;
                                wipDetails.UNIT = null;
                                wipDetails.TABA_NO = request.HT_TABA_NO_4;
                                _CommonRepository.DeleteWIPStatusData(wipDetails);
                            }
                        }
                    }
                }
                // var result = _heatTreatmentRepository.GetHeatTreatmentDetails(requestData[0].FURNACE_NO, null, true);
                transaction.Complete();
                return result;
            }
        }
        public long UpdateEndData(List<HeatTreatMentDetails> requestData)
        {
            long msgCd = 0;
            
            using (TransactionScope transaction = new TransactionScope())
            {
                foreach (var request in requestData)
                {
                    if (request.USER_ID != null && request.USER_ID > 0)
                    {
                        request.INPUT_LOCATION_CD = Convert.ToInt16(_CommonRepository.GetUserLocationById(request.USER_ID.Value));
                    }
                    // request.LOCATION_CD = Convert.ToInt16(request.INPUT_LOCATION_CD);

                    if (request.HT_NEXT_PROCESS == 0)
                    {
                        request.HT_NEXT_PROCESS = null;
                    }

                    if (request.MACHINE_CD != null && request.MACHINE_CD > 0)
                    {
                        request.LOCATION_CD = Convert.ToInt16(_CommonRepository.GetMachineNameByID(request.MACHINE_CD.Value)?.LOCATION_CD);
                    }
                    MessageDetails messageDetails = new MessageDetails();
                    messageDetails.PROCESS_PLAN_CD = Convert.ToInt64(request.PROCESS_PLAN_CD);
                    messageDetails.MACHINE_CD = Convert.ToInt32(request.MACHINE_CD);
                    messageDetails.PROCESS_KUBUN_ID = Convert.ToInt32(UIUtils.GetProcessKubun(_localization.GetLocalizedHtmlString("lbl_ProcessHeatTreatment")));
                    messageDetails.OPERATOR_CD = Convert.ToInt64(request.OFFLOAD_CONF_BY);
                    messageDetails.MSG_CONTENTS = request.HT_REMARKS;
                    messageDetails.MSG_CD = Convert.ToInt32(request.MSG_CD);
                    messageDetails.USER_ID = Convert.ToInt32(request.USER_ID);
                    messageDetails.INPUT_LOCATION_CD = Convert.ToInt32(request.INPUT_LOCATION_CD);
                    if (request.MSG_CD == Constants.NullValue.INT && !string.IsNullOrEmpty(request.HT_REMARKS))
                    {
                        messageDetails = _CommonRepository.MessageDetailsInsert(messageDetails);

                        if (messageDetails.MSG_CD != Constants.NullValue.INT && messageDetails.MSG_CD > 0)
                        {
                            request.MSG_CD = Convert.ToInt32(messageDetails.MSG_CD);
                        }
                    }
                    // If REMARKS was already present, and it is updated,  
                    // then UPDATE the TRN_MESSAGE
                    else if (request.MSG_CD > 0 && request.HT_REMARKS != null)
                    {
                         _CommonRepository.MessageDetailsUpdate(messageDetails);
                    }
                    // If REMARKS was already present, but it is delete now,
                    // then DELETE the TRN_MESSAGE
                    else if (request.MSG_CD > 0 && request.HT_REMARKS == null)
                    {
                        long messageCD = Convert.ToInt64(request.MSG_CD);
                        request.MSG_CD = Constants.NullValue.LONG;

                        _CommonRepository.MessageDetailsDelete(messageCD);
                    }                    
                    msgCd = _heatTreatmentRepository.UpdateEndData(request);

                    if (request.HT_NEXT_PROCESS != 999)
                    {
                        if (request.HT_TABA_NO_1 != null || request.HT_TABA_NO_2 != null || request.HT_TABA_NO_3 != null || request.HT_TABA_NO_4 != null)
                        {

                            WipStatusDetails wipDetails = new WipStatusDetails();
                            wipDetails = new WipStatusDetails();
                            wipDetails.PROCESS_PLAN_CD = request.PROCESS_PLAN_CD;
                            wipDetails.LOCATION_CD = Convert.ToInt16(request.LOCATION_CD);
                            wipDetails.MSG_CD = messageDetails.MSG_CD;
                            wipDetails.FINISHED_BARS = request.HT_NO_OF_UNITS;
                            wipDetails.UNIT = request.UNIT_TYPE;
                            wipDetails.FINISHED_QTY = request.HT_WEIGHT;
                            wipDetails.PROCESS_START_DATE = request.HT_START_DATE;
                            wipDetails.PROCESS_END_DATE = request.HT_END_DATE;
                            wipDetails.PROCESS_KUBUN_CD = Convert.ToInt32(UIUtils.GetProcessKubun(_localization.GetLocalizedHtmlString("lbl_ProcessHeatTreatment")));
                            wipDetails.PRE_PROSESS_DECISION = null;
                            wipDetails.NEXT_PROCESS = request.HT_NEXT_PROCESS;
                            wipDetails.START_OPERATOR_CD = Convert.ToInt32(request.SETUP_CONF_BY);
                            wipDetails.END_OPERATOR_CD = request.OFFLOADED_BY;
                            wipDetails.TABA_NO = 0;

                            wipDetails.NIFUDA_CD = null;
                            if (request.HT_TABA_NO_1 != null && request.HT_TABA_NO_2 != null || request.HT_TABA_NO_3 != null || request.HT_TABA_NO_4 != null)
                            {
                                wipDetails.TABA_GROUP_NO = request.HT_TABA_NO_1;
                            }

                            if (request.HT_TABA_NO_1 != null)
                            {
                                wipDetails.TABA_NO = request.HT_TABA_NO_1;
                                _CommonRepository.DeleteWIPStatusData(wipDetails);
                            }
                                                     

                            if (request.HT_TABA_NO_2 > 0)
                            {
                                wipDetails.FINISHED_BARS = null;
                                wipDetails.UNIT = null;
                                wipDetails.FINISHED_QTY = null;
                                wipDetails.TABA_NO = request.HT_TABA_NO_2;
                                _CommonRepository.DeleteWIPStatusData(wipDetails);
                            }
                            if (request.HT_TABA_NO_3 > 0)
                            {
                                wipDetails.FINISHED_BARS = null;
                                wipDetails.UNIT = null;
                                wipDetails.FINISHED_QTY = null;
                                wipDetails.TABA_NO = request.HT_TABA_NO_3;
                                _CommonRepository.DeleteWIPStatusData(wipDetails);
                            }
                            if (request.HT_TABA_NO_4 > 0)
                            {
                                wipDetails.FINISHED_BARS = null;
                                wipDetails.UNIT = null;
                                wipDetails.FINISHED_QTY = null;
                                wipDetails.TABA_NO = request.HT_TABA_NO_4;
                                _CommonRepository.DeleteWIPStatusData(wipDetails);
                            }
                        }
                    }

                }
                transaction.Complete();
            }
            return msgCd;
        }
        public void Delete(List<HeatTreatMentDetails> request)
        {
            WipStatusDetails wipDetails = new WipStatusDetails();
            wipDetails = new WipStatusDetails();
            wipDetails.PROCESS_PLAN_CD = request[0].PROCESS_PLAN_CD;
            using (TransactionScope transaction = new TransactionScope())
            {
                _heatTreatmentRepository.Delete(Convert.ToInt32(request[0].HEAT_TREATMENT_NUMBER), request[0].FURNACE_NO);

                foreach (var record in request)
                {
                    
                    if (record.MSG_CD > 0)
                    {
                        _CommonRepository.MessageDetailsDelete(Convert.ToInt64(record.MSG_CD));
                    }
                  //  _CommonRepository.DeleteWIPStatusData(record.PROCESS_PLAN_CD, 0);

                    if (record.HT_TABA_NO_1 > 0)
                    {
                        wipDetails.TABA_NO = record.HT_TABA_NO_1;
                        
                        _CommonRepository.DeleteWIPStatusData(wipDetails);
                    }
                    if (record.HT_TABA_NO_2 > 0)
                    {
                        wipDetails.TABA_NO = record.HT_TABA_NO_2;
                        _CommonRepository.DeleteWIPStatusData(wipDetails);
                    }
                    if (record.HT_TABA_NO_3 > 0)
                    {
                        wipDetails.TABA_NO = record.HT_TABA_NO_3;
                        _CommonRepository.DeleteWIPStatusData(wipDetails);
                    }
                    if (record.HT_TABA_NO_4 > 0)
                    {
                        wipDetails.TABA_NO = record.HT_TABA_NO_4;
                        _CommonRepository.DeleteWIPStatusData(wipDetails);
                    }
                }
                transaction.Complete();
            }
        }

        public List<HeatTreatMentDetails> GetHeatTreatmentList(GridSearchFields searchRequest)
        {
            short locationCD = Convert.ToInt16(_CommonRepository.GetUserLocationById(Convert.ToInt32(searchRequest.USER_ID)));
            List<HeatTreatMentDetails> list = new List<HeatTreatMentDetails>();
            List<SearchField> searchFields = ComposeSearchCriteria(searchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(searchRequest.SORT_FIELDS);
            list = _heatTreatmentRepository.GetHeatTreatmentDetails(selectedSortCriteria, searchFields, locationCD);
            if (list != null && list.Any())
            {
                foreach (var item in list)
                {
                    item.PROC_CONDITION_STRING = UIUtils.GetProcConditionKubunValue(Convert.ToString(item.HT_CONDITION));
                }
            }
            return list;
        }
        public List<NameValue> GetProductUnitKubun()
        {
            var _prod_kubun = UIUtils.Instance.GetProductUnitKubun();
            return _prod_kubun;
        }
        public List<NameValue> GetMatlType()
        {
            var _prod_kubun = UIUtils.Instance.GetMatlTypeKubun();
            return _prod_kubun;
        }
        public List<HeatTreatMentDetails> GetHeatTreatmentDetailsOnFactoryDailyWorkStatus(GridSearchFields gridSearchRequest)
        {
            List<SearchField> searchFields = ComposeSearchCriteriaforFactory(gridSearchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);
            int rowLimit = gridSearchRequest.ROW_LIMIT;
            var obj = _heatTreatmentRepository.GetHeatTreatmentDetailsOnFactoryDailyWorkStatus(searchFields, selectedSortCriteria, rowLimit);
            return obj;
        }


        private List<SearchField> ComposeSearchCriteria(GridSearchFields gridSearchRequest)
        {
            var searchFields = new List<SearchField>();
            if (gridSearchRequest.MACHINE_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "MAC.MACHINE_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MACHINE_CD)
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.FURNACE_NO))
            {
                var searchField = new SearchField
                {
                    FieldName = "LOWER(HT.FURNACE_NO)",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.FURNACE_NO.ToLower()
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_CONDITION_STRING))
            {
                var searchField = new SearchField
                {
                    FieldName = "HT.HT_CONDITION",
                    SearchType = SearchType.Number,
                    SearchText1 = "'" + Convert.ToString(gridSearchRequest.PROD_CONDITION_STRING) + "'"
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.PATTERN_NO))
            {
                var searchField = new SearchField
                {
                    FieldName = "HT.PATTERN_NO",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.PATTERN_NO.ToLower()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.START_DATETIME))
            {
                var startD = gridSearchRequest.START_DATETIME;
                var EndD = gridSearchRequest.END_DATETIME;
                var searchField = new SearchField
                {

                    FieldName = "HT.HT_START_DATE",
                    SearchType = SearchType.Date,
                    SearchText1 = startD,
                    SearchText2 = EndD
                };

                searchFields.Add(searchField);
            }

            return searchFields;
        }
        private List<SearchField> ComposeSearchCriteriaforFactory(GridSearchFields gridSearchRequest)
        {
            var searchFields = new List<SearchField>();
            if (gridSearchRequest.MACHINE_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "HT.MACHINE_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MACHINE_CD)
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.LOCATION_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "HT.LOCATION_CD",
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
                    FieldName = "HT.OFFLOAD_CONF_BY",
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
                        SearchText1 = Convert.ToString(gridSearchRequest.PROD_DIM_D_MAX)
                    };
                    searchFields.Add(searchField);
                }
            }
            else if (gridSearchRequest.PROD_LENGTH_MAX > 0 && gridSearchRequest.PROD_LENGTH_MIN > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_LENGTH",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.PROD_DIM_D_MIN),
                    SearchText2 = Convert.ToString(gridSearchRequest.PROD_DIM_D_MAX)
                };
                searchFields.Add(searchField);
            }
            if (string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE2))
            {
                if (!string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE1))
                {
                    var searchField = new SearchField
                    {
                        FieldName = "HT.HT_END_DATE",
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
                        FieldName = "HT.HT_END_DATE",
                        SearchType = SearchType.DateTime,
                        SearchText1 = gridSearchRequest.INSPECTION_DATE2
                    };
                    searchFields.Add(searchField);
                }
            }
            else if (!string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE1) && !string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE1))
            {
                var searchField = new SearchField
                {
                    FieldName = "HT.HT_END_DATE",
                    SearchType = SearchType.DateTime,
                    SearchText1 = gridSearchRequest.INSPECTION_DATE1,
                    SearchText2 = gridSearchRequest.INSPECTION_DATE2
                };
                searchFields.Add(searchField);
            }

            return searchFields;
        }

        public List<HeatTreatMentDetails> GetHeatTreatmentDetailsOnDailyProcessAnalysis(GridSearchFields gridSearchRequest)
        {


            List<SearchField> searchFields = ComposeSearchCriteriaforDailyProcess(gridSearchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);

            var obj = _heatTreatmentRepository.GetHeatTreatmentDetailsOnDailyProcessAnalysis(searchFields, selectedSortCriteria, gridSearchRequest);
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

            if (!string.IsNullOrEmpty(gridSearchRequest.FURNACE_NO))
            {
                var searchField = new SearchField
                {
                    FieldName = "HT.FURNACE_NO",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.FURNACE_NO.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.HT_CONDITION))
            {
                var searchField = new SearchField
                {
                    FieldName = "HT.HT_CONDITION",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.HT_CONDITION
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.PATTERN_NO))
            {
                var searchField = new SearchField
                {
                    FieldName = "HT.PATTERN_NO",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.PATTERN_NO.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.BEGINNING_START_DATE))
            {
                var searchField = new SearchField
                {
                    FieldName = "HT.HT_START_DATE",
                    SearchType = SearchType.DateTime,
                    SearchText1 = gridSearchRequest.BEGINNING_START_DATE,
                    SearchText2 = gridSearchRequest.BEGINNING_END_DATE
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.HT_SETUP_CONF_BY_OPERATOR))
            {
                var searchField = new SearchField
                {
                    FieldName = "EMP1.SHORT_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.HT_SETUP_CONF_BY_OPERATOR.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.HT_LOADING_CONF_BY_OPERATOR))
            {
                var searchField = new SearchField
                {
                    FieldName = "EMP2.SHORT_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.HT_LOADING_CONF_BY_OPERATOR.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.END_DATE1))
            {
                var searchField = new SearchField
                {
                    FieldName = "HT.HT_END_DATE",
                    SearchType = SearchType.DateTime,
                    SearchText1 = gridSearchRequest.END_DATE1,
                    SearchText2 = gridSearchRequest.END_DATE2
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.HT_OFFLOADED_BY_OPERATOR))
            {
                var searchField = new SearchField
                {
                    FieldName = "EMP3.SHORT_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.HT_OFFLOADED_BY_OPERATOR.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.HT_OFFLOAD_CONF_BY_OPERATOR))
            {
                var searchField = new SearchField
                {
                    FieldName = "EMP4.SHORT_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.HT_OFFLOAD_CONF_BY_OPERATOR.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.METALLIC_LABEL_NO_ROLL_MONTH))
            {
                var searchField = new SearchField
                {
                    FieldName = "HT.ROLL_MONTH",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.METALLIC_LABEL_NO_ROLL_MONTH.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.METALLIC_PLATE_CODE_STRING))
            {
                var searchField = new SearchField
                {
                    FieldName = "HT.COIL_NO",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.METALLIC_PLATE_CODE_STRING.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }

            return searchFields;
        }
        public int GetMaxHTNumber()
        {
            var result = _heatTreatmentRepository.GetHTNumDetails();
            return result;
        }
       

        public int GetRowCountForHeatTreatmentFactory(GridSearchFields gridSearchRequest)
        {
            List<SearchField> searchFields = ComposeSearchCriteriaforFactory(gridSearchRequest);            
            int count = _heatTreatmentRepository.GetRowCountForHeatTreatmentFactory(searchFields);
            return count;
        }

        public int GetRowCountForHeatTreatmentDailyAnalysis(GridSearchFields gridSearchRequest)
        {
            List<SearchField> searchFields = ComposeSearchCriteriaforDailyProcess(gridSearchRequest);
            int count = _heatTreatmentRepository.GetRowCountForHeatTreatmentDailyAnalysis(searchFields);
            return count;
        }
        public List<NameValue> GetHeatTreatmentMachineList()
        {
            var machineNameList = _heatTreatmentRepository.GetHeatTreatmentMachineList();
            return machineNameList;
        }
    }
}
