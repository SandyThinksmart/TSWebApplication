using Microsoft.AspNetCore.Http;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Transactions;
using TSWebApplication.Application.IServices;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Infrastructure.IRepositories;

namespace TSWebApplication.Application.Services
{
    public class HardnessTemperingService : IHardnessTemperingService
    {
        private readonly IHardnessTemperingRepository _hardnessTemperingRepository;
        private readonly IAllCommonRepository _commonRepository;
        private readonly IUserLoginRepository _userLoginRepository;
        private readonly LocalizationServices _localizationServices;
        private readonly IAllCommonService _allCommonService;
        public HardnessTemperingService(IHardnessTemperingRepository hardnessTemperingRepository, IStamppingRepository stamppingRepository, IAllCommonRepository commonRepository, IUserLoginRepository userLoginRepository, IAllCommonService allCommonService,LocalizationServices localizationServices)
        {
            _hardnessTemperingRepository = hardnessTemperingRepository;
            _commonRepository = commonRepository;
            _userLoginRepository = userLoginRepository;
            _allCommonService = allCommonService;
            _localizationServices = localizationServices;
        }

        public HardeningTemperingDetails GetHardnessTemperingDetailsByChuubanGyoban(string chuuban, short? gyouban)
        {
            var result = _hardnessTemperingRepository.GetHardnessTemperingDetailsByChuubanGyoban(chuuban, gyouban);
            return result;
        }

        public HardeningTemperingDetails GetHardnessTemperingDetails(string furnaceNo, string chuuban, short? gyouban, int userid)
        {
            short inputLocationCD = 0;
            if (userid > 0)
            {
                inputLocationCD = Convert.ToInt16(_userLoginRepository.GetUserLocationById(userid));
            }

            var result = _hardnessTemperingRepository.GetHardnessTemperingDetails(furnaceNo, chuuban, gyouban, inputLocationCD);        

                if (result != null)
                {
                if (result.WATER_OIL_TYPE > 0) {
                 result.WATER_OIL_STRING = UIUtils.GetHardnessOilTypeKubunValue(Convert.ToString(result.WATER_OIL_TYPE));
                }
            }
           

            return result;
        }

        public void InsertHardnessTemperingDetails(HardeningTemperingDetails details)
        {
            if (details.USER_ID != null && details.USER_ID > 0)
            {
                details.INPUT_LOCATION_CD = Convert.ToInt16(_userLoginRepository.GetUserLocationById(details.USER_ID.Value));
            }
            //  details.LOCATION_CD = details.INPUT_LOCATION_CD;

            if (details.HARDENING_MACHINE_CD != null && details.HARDENING_MACHINE_CD > 0)
            {
                details.LOCATION_CD = Convert.ToInt16(_commonRepository.GetMachineNameByID(Convert.ToInt64(details.HARDENING_MACHINE_CD.Value))?.LOCATION_CD);
            }
            details.PROCESS_KUBUN_ID = Convert.ToInt32(UIUtils.GetProcessKubun(_localizationServices.GetLocalizedHtmlString("lbl_Refining")));

            using (TransactionScope transaction = new TransactionScope())
            {
                MessageDetails messageDetails = new MessageDetails();
                messageDetails.PROCESS_PLAN_CD = Convert.ToInt64(details.PROCESS_PLAN_CD);
                messageDetails.MACHINE_CD = Convert.ToInt32(details.TEMPERING_MACHINE_CD);
                messageDetails.PROCESS_KUBUN_ID = Convert.ToInt32(details.PROCESS_KUBUN_ID);
                messageDetails.OPERATOR_CD = Convert.ToInt64(details.OPERATOR_CD_TEMPERING);
                messageDetails.MSG_DATE = Convert.ToDateTime(details.END_DATE_TEMPERING);
                messageDetails.MSG_CONTENTS = details.REMARK;
                messageDetails.USER_ID = Convert.ToInt32(details.USER_ID);
                messageDetails.INPUT_LOCATION_CD = Convert.ToInt32(details.INPUT_LOCATION_CD);

                if (!string.IsNullOrEmpty(details.REMARK))
                {
                    messageDetails = _commonRepository.MessageDetailsInsert(messageDetails);

                    if (messageDetails.MSG_CD != Constants.NullValue.LONG && messageDetails.MSG_CD > 0)
                    {
                        details.MSG_CD = Convert.ToInt32(messageDetails.MSG_CD);
                    }
                }

                _hardnessTemperingRepository.InsertHardnessTemperingDetails(details);

                if (details.HT_NEXT_PROCESS != 999 && Convert.ToInt16(details.TABA_NO) > 0)
                {

                    WipStatusDetails wipDetails = new WipStatusDetails();
                    wipDetails = new WipStatusDetails();
                    wipDetails.PROCESS_PLAN_CD = details.PROCESS_PLAN_CD;
                    wipDetails.LOCATION_CD = Convert.ToInt16(details.LOCATION_CD);
                    wipDetails.MSG_CD = details.MSG_CD;
                    wipDetails.FINISHED_BARS = details.NO_OF_UNITS;
                    wipDetails.UNIT = details.UNIT_TYPE;
                    wipDetails.FINISHED_QTY = details.WEIGHT;
                    wipDetails.PROCESS_START_DATE = details.START_DATE_HARDENING;
                    wipDetails.PROCESS_END_DATE = details.END_DATE_HARDENING;
                    wipDetails.PROCESS_KUBUN_CD = details.PROCESS_KUBUN_ID;
                    wipDetails.PRE_PROSESS_DECISION = null;
                    wipDetails.NEXT_PROCESS = details.HT_NEXT_PROCESS;
                    wipDetails.START_OPERATOR_CD = details.OPERATOR_CD_HARDENING;
                    wipDetails.END_OPERATOR_CD = details.OPERATOR_CD_HARDENING;
                    wipDetails.TABA_NO = details.TABA_NO;
                    wipDetails.NIFUDA_CD = null;

                    _commonRepository.DeleteWIPStatusData(wipDetails);
                }
                transaction.Complete();
            }

        }

        public long UpdateHardnessTemperingDetails(HardeningTemperingDetails details)
        {
            long msgCD = 0;
            bool deleteMsg=false;
            if (details.USER_ID != null && details.USER_ID > 0)
            {
                details.INPUT_LOCATION_CD = Convert.ToInt16(_hardnessTemperingRepository.GetUserLocationById(details.USER_ID.Value));
            }
            //  details.LOCATION_CD = details.INPUT_LOCATION_CD;

            if (details.HARDENING_MACHINE_CD != null && details.HARDENING_MACHINE_CD > 0)
            {
                details.LOCATION_CD = Convert.ToInt16(_commonRepository.GetMachineNameByID(Convert.ToInt64(details.HARDENING_MACHINE_CD.Value))?.LOCATION_CD);
            }


            using (TransactionScope transaction = new TransactionScope())
            {
                MessageDetails messageDetails = new MessageDetails();
                messageDetails.PROCESS_PLAN_CD = Convert.ToInt64(details.PROCESS_PLAN_CD);
                messageDetails.MACHINE_CD = Convert.ToInt32(details.TEMPERING_MACHINE_CD);
                messageDetails.MSG_CD = Convert.ToInt64(details.MSG_CD); 
                messageDetails.PROCESS_KUBUN_ID = Convert.ToInt32(UIUtils.GetProcessKubun(_localizationServices.GetLocalizedHtmlString("lbl_Refining")));
                messageDetails.OPERATOR_CD = Convert.ToInt64(details.OPERATOR_CD_TEMPERING);
                messageDetails.MSG_CONTENTS = details.REMARK;
                messageDetails.USER_ID = Convert.ToInt32(details.USER_ID);
                messageDetails.INPUT_LOCATION_CD = Convert.ToInt32(details.INPUT_LOCATION_CD);
                if (details.MSG_CD == Constants.NullValue.INT && !string.IsNullOrEmpty(details.REMARK))
                {
                    
                    messageDetails = _commonRepository.MessageDetailsInsert(messageDetails);

                    if (messageDetails.MSG_CD > 0)
                    {
                        details.MSG_CD = Convert.ToInt32(messageDetails.MSG_CD);
                    }
                }
                else if (details.MSG_CD > 0 && details.REMARK != null)
                {
                    _commonRepository.MessageDetailsUpdate(messageDetails);

                }
                else if (details.MSG_CD > 0 && details.REMARK == null)
                {
                    long messageCD = Convert.ToInt64(details.MSG_CD);
                    details.MSG_CD = Constants.NullValue.LONG;
                    msgCD = _hardnessTemperingRepository.UpdateHardnessTemperingDetails(details);
                    _commonRepository.MessageDetailsDelete(messageCD);
                    deleteMsg = true;
                }
                if (deleteMsg == false)
                {
                    msgCD = _hardnessTemperingRepository.UpdateHardnessTemperingDetails(details);
                }

                if (details.HT_NEXT_PROCESS != 999 && Convert.ToInt16(details.TABA_NO) > 0)
                {
                    WipStatusDetails wipDetails = new WipStatusDetails();
                    wipDetails = new WipStatusDetails();
                    wipDetails.PROCESS_PLAN_CD = details.PROCESS_PLAN_CD;
                    wipDetails.LOCATION_CD = Convert.ToInt16(details.LOCATION_CD);
                    wipDetails.MSG_CD = details.MSG_CD;
                    wipDetails.FINISHED_BARS = details.NO_OF_UNITS;
                    wipDetails.UNIT = details.UNIT_TYPE;
                    wipDetails.FINISHED_QTY = details.WEIGHT;
                    wipDetails.PROCESS_START_DATE = details.START_DATE_HARDENING;
                    wipDetails.PROCESS_END_DATE = details.END_DATE_HARDENING;
                    wipDetails.PROCESS_KUBUN_CD = Convert.ToInt32(UIUtils.GetProcessKubun(_localizationServices.GetLocalizedHtmlString("lbl_Refining"))); ;
                    wipDetails.PRE_PROSESS_DECISION = null;
                    wipDetails.NEXT_PROCESS = details.HT_NEXT_PROCESS;
                    wipDetails.START_OPERATOR_CD = details.OPERATOR_CD_HARDENING;
                    wipDetails.END_OPERATOR_CD = details.OPERATOR_CD_HARDENING;
                    wipDetails.TABA_NO = details.TABA_NO;
                    wipDetails.NIFUDA_CD = null;

                    _commonRepository.DeleteWIPStatusData(wipDetails);
                }

                transaction.Complete();
            }
            return msgCD;
        }

        public long UpdateHTQualityDetails(HardeningTemperingDetails details)
        {
            long msgCD = 0;
            bool deleteMsg = false;
            //DateTime now = DateTime.Now;
            //string time24 = details.INSPECTION_DATE.ToShortDateString() + " " + now.ToString("HH:mm:ss");

            //details.INSPECTION_DATE = Convert.ToDateTime(time24);

            details.PROCESS_KUBUN_ID = Convert.ToInt32(UIUtils.GetProcessKubun(_localizationServices.GetLocalizedHtmlString("lbl_ProcessTemperature_Inspection")));
            if (details.USER_ID != null && details.USER_ID > 0)
            {
                details.INPUT_LOCATION_CD = Convert.ToInt16(_hardnessTemperingRepository.GetUserLocationById(details.USER_ID.Value));
            }
            

            using (TransactionScope transaction = new TransactionScope())
            {
                //if (details.HTQ_NEXT_PROCESS != 999)
                //{
                //    WipStatusDetails wipDetails = new WipStatusDetails();
                //    wipDetails = new WipStatusDetails();
                //    wipDetails.PROCESS_PLAN_CD = details.PROCESS_PLAN_CD;
                //    wipDetails.LOCATION_CD = Convert.ToInt16(details.INPUT_LOCATION_CD);
                //    wipDetails.MSG_CD = details.MSG_CD;
                //    wipDetails.FINISHED_BARS = null;
                //    wipDetails.UNIT = null;
                //    wipDetails.FINISHED_QTY = null;
                //    wipDetails.PROCESS_START_DATE = details.INSPECTION_DATE;
                //    wipDetails.PROCESS_END_DATE = details.INSPECTION_DATE;
                //    wipDetails.PROCESS_KUBUN_CD = details.PROCESS_KUBUN_ID;
                //    wipDetails.PRE_PROSESS_DECISION = details.DECISION;
                //    wipDetails.NEXT_PROCESS = details.HTQ_NEXT_PROCESS;
                //    wipDetails.START_OPERATOR_CD = details.INSPECTED_BY_CD;
                //    wipDetails.END_OPERATOR_CD = details.INSPECTED_BY_CD;
                //    wipDetails.TABA_NO = 0;
                //    wipDetails.NIFUDA_CD = null;

                //    _commonRepository.InsertWIPStatusData(wipDetails);
                //}

                MessageDetails messageDetails = new MessageDetails();
                messageDetails.PROCESS_PLAN_CD = Convert.ToInt64(details.PROCESS_PLAN_CD);
                messageDetails.PROCESS_KUBUN_ID = details.PROCESS_KUBUN_ID;
                messageDetails.OPERATOR_CD = Convert.ToInt64(details.INSPECTED_BY_CD);
                messageDetails.MSG_CONTENTS = details.REMARK;
                messageDetails.USER_ID = Convert.ToInt32(details.USER_ID);
                messageDetails.INPUT_LOCATION_CD = Convert.ToInt32(details.INPUT_LOCATION_CD);

                if (details.MSG_CD == Constants.NullValue.INT && !string.IsNullOrEmpty(details.REMARK))
                {
                    messageDetails = _commonRepository.MessageDetailsInsert(messageDetails);

                    if (messageDetails.MSG_CD > 0)
                    {
                        details.MSG_CD = Convert.ToInt32(messageDetails.MSG_CD);
                    }

                }
                else if (details.MSG_CD > 0 && !string.IsNullOrEmpty(details.REMARK))//&& details.REMARK != details.HT_REMARK_PREVIOUS
                {
                    messageDetails.MSG_CD = Convert.ToInt64(details.MSG_CD);
                    _commonRepository.MessageDetailsUpdate(messageDetails);
                }
                else if (details.MSG_CD > 0 && string.IsNullOrEmpty(details.REMARK))
                {
                    long messageCD = Convert.ToInt64(details.MSG_CD);
                    details.MSG_CD = Constants.NullValue.LONG;
                    msgCD = _hardnessTemperingRepository.UpdateHTQualityDetails(details);
                    _commonRepository.MessageDetailsDelete(messageCD);
                    deleteMsg = true;
                }
                if (details.HT_QUALITY_CD > 0 && deleteMsg == false)
                {
                    msgCD =  _hardnessTemperingRepository.UpdateHTQualityDetails(details);
                }
                transaction.Complete();
            }
            return msgCD;
        }
        public void DeleteHTQualityDetails(long htQualityCD, long? msgCD)
        {
            if (htQualityCD > 0)
                using (TransactionScope transaction = new TransactionScope())
                {
                    _hardnessTemperingRepository.DeleteHTQualityDetails(htQualityCD);
                    if (Convert.ToInt64(msgCD) > 0)
                    {
                        _commonRepository.MessageDetailsDelete(Convert.ToInt64(msgCD));
                    }

                    transaction.Complete();

                }
        }

        public void DeleteHardnessTemperingDetails(HardeningTemperingDetails request)
        {
            if (request.HARDENING_TEMPERING_CD > 0)
                using (TransactionScope transaction = new TransactionScope())
                {
                    WipStatusDetails wipDetails = new WipStatusDetails();
                    wipDetails = new WipStatusDetails();
                    wipDetails.PROCESS_PLAN_CD = request.PROCESS_PLAN_CD;
                    wipDetails.TABA_NO = request.TABA_NO;

                    _hardnessTemperingRepository.DeleteHardnessTemperingDetails(request.HARDENING_TEMPERING_CD);
                    if (Convert.ToInt16(request.TABA_NO) > 0)
                    {
                        _commonRepository.DeleteWIPStatusData(wipDetails);
                    }

                    if (Convert.ToInt64(request.MSG_CD) > 0)
                    {
                        _commonRepository.MessageDetailsDelete(Convert.ToInt64(request.MSG_CD));
                    }

                    transaction.Complete();

                }
        }

        public void InsertHardnessTemperingTempDetails(List<HardeningTemperingDetails> details)
        {
            using (TransactionScope transaction = new TransactionScope())
            {

                foreach (var item in details)
                {
                  if (item.USER_ID != null && item.USER_ID > 0)
                  {
                      item.INPUT_LOCATION_CD = Convert.ToInt16(_userLoginRepository.GetUserLocationById(item.USER_ID.Value));
                  }
                    //    item.LOCATION_CD = item.INPUT_LOCATION_CD;

                    if (item.HARDENING_MACHINE_CD != null && item.HARDENING_MACHINE_CD > 0)
                    {
                        item.LOCATION_CD = Convert.ToInt16(_commonRepository.GetMachineNameByID(Convert.ToInt64(item.HARDENING_MACHINE_CD.Value))?.LOCATION_CD);
                    }
                    item.PROCESS_KUBUN_ID = Convert.ToInt32(UIUtils.GetProcessKubun(_localizationServices.GetLocalizedHtmlString("lbl_Refining")));
                                    
                   _hardnessTemperingRepository.InsertHardnessTemperingTempDetails(item);
           
                //WipStatusDetails wipDetails = new WipStatusDetails();

                //wipDetails = new WipStatusDetails();
                //wipDetails.PROCESS_PLAN_CD = item.PROCESS_PLAN_CD;
                //wipDetails.LOCATION_CD = Convert.ToInt16(item.LOCATION_CD);
                //wipDetails.MSG_CD = item.MSG_CD;
                //wipDetails.FINISHED_BARS = item.NO_OF_UNITS;
                //wipDetails.UNIT = item.UNIT_TYPE;
                //wipDetails.FINISHED_QTY = item.WEIGHT;
                //wipDetails.PROCESS_START_DATE = item.START_DATE_HARDENING;
                //wipDetails.PROCESS_END_DATE = item.END_DATE_HARDENING;
                //wipDetails.PROCESS_KUBUN_CD = item.PROCESS_KUBUN_ID;
                //wipDetails.PRE_PROSESS_DECISION = null;
                //wipDetails.NEXT_PROCESS = item.HT_NEXT_PROCESS;
                //wipDetails.START_OPERATOR_CD = item.OPERATOR_CD_HARDENING;
                //wipDetails.END_OPERATOR_CD = item.OPERATOR_CD_HARDENING;
                //wipDetails.TABA_NO = 0;
                //wipDetails.NIFUDA_CD = null;


                //_commonRepository.InsertWIPStatusData(wipDetails);
                }
                transaction.Complete();
            }
        }

        public void InsertHTQualityDetails(HardeningTemperingDetails details)
        {
            //DateTime now = DateTime.Now;
            //string time24 = details.INSPECTION_DATE.ToShortDateString() + " " + now.ToString("HH:mm:ss");
            //details.INSPECTION_DATE = Convert.ToDateTime(time24);

            details.PROCESS_KUBUN_ID = Convert.ToInt32(UIUtils.GetProcessKubun(_localizationServices.GetLocalizedHtmlString("lbl_ProcessTemperature_Inspection")));
            if (details.USER_ID != null && details.USER_ID > 0)
            {
                details.INPUT_LOCATION_CD = Convert.ToInt16(_userLoginRepository.GetUserLocationById(Convert.ToInt32( details.USER_ID)));
                
            }
            details.LOCATION_CD = details.INPUT_LOCATION_CD;
            using (TransactionScope transaction = new TransactionScope())
            {
                if (!string.IsNullOrEmpty(details.REMARK))
                {
                    MessageDetails messageDetails = new MessageDetails();
                    messageDetails.PROCESS_PLAN_CD = Convert.ToInt64(details.PROCESS_PLAN_CD);
                   // messageDetails.MACHINE_CD = 0;
                    messageDetails.PROCESS_KUBUN_ID = details.PROCESS_KUBUN_ID;
                    messageDetails.OPERATOR_CD = Convert.ToInt64(details.INSPECTED_BY_CD);
                    messageDetails.MSG_CONTENTS = details.REMARK;
                    messageDetails.USER_ID = Convert.ToInt32(details.USER_ID);
                    messageDetails.INPUT_LOCATION_CD = Convert.ToInt32(details.INPUT_LOCATION_CD);

                    messageDetails = _commonRepository.MessageDetailsInsert(messageDetails);

                    if (messageDetails.MSG_CD != Constants.NullValue.INT && messageDetails.MSG_CD > 0)
                    {
                        details.MSG_CD = Convert.ToInt32(messageDetails.MSG_CD);
                    }
                  
                }
                //if (details.HTQ_NEXT_PROCESS != 999)
                //{
                //    WipStatusDetails wipDetails = new WipStatusDetails();
                //    wipDetails = new WipStatusDetails();
                //    wipDetails.PROCESS_PLAN_CD = details.PROCESS_PLAN_CD;
                //    wipDetails.LOCATION_CD = Convert.ToInt16(details.LOCATION_CD);
                //    wipDetails.MSG_CD = details.MSG_CD;
                //    wipDetails.FINISHED_BARS = details.NO_OF_UNITS;
                //    wipDetails.UNIT = details.UNIT_TYPE;
                //    wipDetails.FINISHED_QTY = details.WEIGHT;
                //    wipDetails.PROCESS_START_DATE = details.INSPECTION_DATE;
                //    wipDetails.PROCESS_END_DATE = details.INSPECTION_DATE;
                //    wipDetails.PROCESS_KUBUN_CD = details.PROCESS_KUBUN_ID;
                //    wipDetails.PRE_PROSESS_DECISION = details.DECISION;
                //    wipDetails.NEXT_PROCESS = details.HTQ_NEXT_PROCESS;
                //    wipDetails.START_OPERATOR_CD = details.INSPECTED_BY_CD;
                //    wipDetails.END_OPERATOR_CD = details.INSPECTED_BY_CD;
                //    wipDetails.TABA_NO = 0;
                //    wipDetails.NIFUDA_CD = null;
                //    _commonRepository.InsertWIPStatusData(wipDetails);
                //}
                _hardnessTemperingRepository.InsertHTQualityDetails(details);
                transaction.Complete();
            }
        }

        public List<HardeningTemperingDetails> GetHardnessTemperingList(GridSearchFields searchRequest)
        {
            int Location_CD = _commonRepository.GetUserLocationById(Convert.ToInt32(searchRequest.USER_ID));
            List<SearchField> searchFields = ComposeSearchCriteria(searchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(searchRequest.SORT_FIELDS);
           return _hardnessTemperingRepository.GetHardnessTemperingList(searchFields, selectedSortCriteria, Location_CD);
            
        }

        public List<HardeningTemperingDetails> GetHTQualityList(GridSearchFields searchRequest)
        {
            int Location_CD = _commonRepository.GetUserLocationById(Convert.ToInt32(searchRequest.USER_ID));
            searchRequest.PROCESS_NAME_STRING = "HTQUALITY";
            List<SearchField> searchFields = ComposeSearchCriteria(searchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(searchRequest.SORT_FIELDS);
            return _hardnessTemperingRepository.GetHTQualityList(searchFields, selectedSortCriteria, Location_CD);
           
        }

        public HardeningTemperingDetails GetHardnessTemperingDetailsByHardnessTemperingCD(long hardnessTemperingCD)
        {
            var result = _hardnessTemperingRepository.GetHardnessTemperingDetailsByHardnessTemperingCD(hardnessTemperingCD);
            return result;
        }

        private List<SearchField> ComposeSearchCriteria(GridSearchFields gridSearchRequest)
        {
            if (gridSearchRequest.PROCESS_NAME_STRING == "HTQUALITY")
            {
                var searchFields = new List<SearchField>();
                if (!string.IsNullOrEmpty(gridSearchRequest.CHUUBAN))
                {
                    var searchField = new SearchField
                    {
                        FieldName = "CONCAT(PP.CHUUBAN,CASE WHEN LENGTH(TRIM(PP.GYOUBAN))=1 THEN '0'||PP.GYOUBAN END)",
                        SearchType = SearchType.Text,
                        SearchText1 = gridSearchRequest.CHUUBAN
                    };
                    searchFields.Add(searchField);
                }

                if (!string.IsNullOrEmpty(gridSearchRequest.FURNACE_NO))
                {
                    var searchField = new SearchField
                    {
                        FieldName = "HT.FURNACE_NO",
                        SearchType = SearchType.Text,
                        SearchText1 = gridSearchRequest.FURNACE_NO.ToLower()
                    };
                    searchFields.Add(searchField);
                }

                if (gridSearchRequest.CUSTOMER_CD > 0)
                {
                    var searchField = new SearchField
                    {
                        FieldName = "PP.CUSTOMER_CD",
                        SearchType = SearchType.Text,
                        SearchText1 = Convert.ToString(gridSearchRequest.CUSTOMER_CD)
                    };
                    searchFields.Add(searchField);
                }

                if (!string.IsNullOrEmpty(gridSearchRequest.PROD_NAME_CUST))
                {
                    var searchField = new SearchField
                    {
                        FieldName = "PP.PROD_NAME_CUST",
                        SearchType = SearchType.Text,
                        SearchText1 = gridSearchRequest.PROD_NAME_CUST.ToLower()
                    };
                    searchFields.Add(searchField);
                }

                if (gridSearchRequest.MATL_DIM_D > 0)
                {
                    var searchField = new SearchField
                    {
                        FieldName = "PP.MATL_DIM_D",
                        SearchType = SearchType.Text,
                        SearchText1 = Convert.ToString(gridSearchRequest.MATL_DIM_D)
                    };
                    searchFields.Add(searchField);
                }

                if (gridSearchRequest.PROD_DIM_D > 0)
                {
                    var searchField = new SearchField
                    {
                        FieldName = "PP.PROD_DIM_D",
                        SearchType = SearchType.Text,
                        SearchText1 = Convert.ToString(gridSearchRequest.PROD_DIM_D)
                    };
                    searchFields.Add(searchField);
                }

                if (!string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE1) && !string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE2))
                {
                    var searchField = new SearchField
                    {
                        FieldName = "HTQ.INSPECTION_DATE",
                        SearchType = SearchType.Date,
                        SearchText1 = gridSearchRequest.INSPECTION_DATE1,
                        SearchText2 = gridSearchRequest.INSPECTION_DATE2,

                    };
                    searchFields.Add(searchField);
                }
                else if (!string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE1))
                {
                    var searchField = new SearchField
                    {
                        FieldName = "HTQ.INSPECTION_DATE",
                        SearchType = SearchType.Date,
                        SearchText1 = gridSearchRequest.INSPECTION_DATE1,

                    };
                    searchFields.Add(searchField);
                }
                return searchFields;
            }
            else
            {
                var searchFields = new List<SearchField>();
                if (!string.IsNullOrEmpty(gridSearchRequest.CHUUBAN))
                {
                    var searchField = new SearchField
                    {
                        FieldName = "CONCAT(PP.CHUUBAN,CASE WHEN LENGTH(TRIM(PP.GYOUBAN))=1 THEN '0'||PP.GYOUBAN END)",
                        SearchType = SearchType.Text,
                        SearchText1 = gridSearchRequest.CHUUBAN
                    };
                    searchFields.Add(searchField);
                }

                if (!string.IsNullOrEmpty(gridSearchRequest.FURNACE_NO))
                {
                    var searchField = new SearchField
                    {
                        FieldName = "FURNACE_NO",
                        SearchType = SearchType.Text,
                        SearchText1 = gridSearchRequest.FURNACE_NO.ToLower()
                    };
                    searchFields.Add(searchField);
                }

                if (gridSearchRequest.CUSTOMER_CD > 0)
                {
                    var searchField = new SearchField
                    {
                        FieldName = "PP.CUSTOMER_CD",
                        SearchType = SearchType.Text,
                        SearchText1 = Convert.ToString(gridSearchRequest.CUSTOMER_CD)
                    };
                    searchFields.Add(searchField);
                }

                if (!string.IsNullOrEmpty(gridSearchRequest.PROD_NAME_CUST))
                {
                    var searchField = new SearchField
                    {
                        FieldName = "PP.PROD_NAME_CUST",
                        SearchType = SearchType.Text,
                        SearchText1 = gridSearchRequest.PROD_NAME_CUST.ToLower()
                    };
                    searchFields.Add(searchField);
                }

                if (gridSearchRequest.MATL_DIM_D > 0)
                {
                    var searchField = new SearchField
                    {
                        FieldName = "PP.MATL_DIM_D",
                        SearchType = SearchType.Text,
                        SearchText1 = Convert.ToString(gridSearchRequest.MATL_DIM_D)
                    };
                    searchFields.Add(searchField);
                }

                if (gridSearchRequest.PROD_DIM_D > 0)
                {
                    var searchField = new SearchField
                    {
                        FieldName = "PP.PROD_DIM_D",
                        SearchType = SearchType.Text,
                        SearchText1 = Convert.ToString(gridSearchRequest.PROD_DIM_D)
                    };
                    searchFields.Add(searchField);
                }

                if (!string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE1) && !string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE2))
                {
                    var searchField = new SearchField
                    {
                        FieldName = "HT.START_DATE_HARDENING",
                        SearchType = SearchType.Date,
                        SearchText1 = gridSearchRequest.INSPECTION_DATE1,
                        SearchText2 = gridSearchRequest.INSPECTION_DATE2,

                    };
                    searchFields.Add(searchField);
                }
                else if (!string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE1))
                {
                    var searchField = new SearchField
                    {
                        FieldName = "HT.START_DATE_HARDENING",
                        SearchType = SearchType.Date,
                        SearchText1 = gridSearchRequest.INSPECTION_DATE1,

                    };
                    searchFields.Add(searchField);
                }
                return searchFields;
            }
           
        }

        public List<HardeningTemperingDetails> GetHardnessTempringOnFactoryDailyWorkStatus(GridSearchFields gridSearchRequest)
        {
            List<SearchField> searchFields = ComposeSearchCriteriaforFactoryDailyWorkStatus(gridSearchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);
            int rowLimit = gridSearchRequest.ROW_LIMIT;
            var result = _hardnessTemperingRepository.GetHardnessTempringOnFactoryDailyWorkStatus(searchFields, selectedSortCriteria, rowLimit);           
            return result;
        }

        private List<SearchField> ComposeSearchCriteriaforFactoryDailyWorkStatus(GridSearchFields gridSearchRequest)
        {
            var searchFields = new List<SearchField>();
            var machineOrGroup = new List<SearchField>();
            var dateOrGroup = new List<SearchField>();
            var operatorOrGroup = new List<SearchField>();

            if (gridSearchRequest.LOCATION_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "NVL(HTQ.LOCATION_CD, HT.LOCATION_CD)",
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
            if (gridSearchRequest.MACHINE_CD > 0)
            {
                machineOrGroup.Add(new SearchField
                {
                    FieldName = "HT.HARDENING_MACHINE_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MACHINE_CD)
                });

                machineOrGroup.Add(new SearchField
                {
                    FieldName = "HT.TEMPERING_MACHINE_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MACHINE_CD)
                });
                searchFields.Add(new SearchField
                {
                    FieldName = "(" + string.Join(" OR ", machineOrGroup.Select(f => f.FieldName + " = " + f.SearchText1)) + ")",
                    SearchType = SearchType.Custom
                });
            }
            if (gridSearchRequest.END_OPERATOR_CD > 0)
            {
                operatorOrGroup.Add(new SearchField
                {
                    FieldName = "HT.OPERATOR_CD_HARDENING",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.END_OPERATOR_CD)
                });

                operatorOrGroup.Add(new SearchField
                {
                    FieldName = "HT.OPERATOR_CD_TEMPERING",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.END_OPERATOR_CD)
                });
                operatorOrGroup.Add(new SearchField
                {
                    FieldName = "HTQ.INSPECTED_BY_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.END_OPERATOR_CD)
                });
                searchFields.Add(new SearchField
                {
                    FieldName = "(" + string.Join(" OR ", operatorOrGroup.Select(f => f.FieldName + " = " + f.SearchText1)) + ")",
                    SearchType = SearchType.Custom
                });
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
                    SearchText1 = "'" + Convert.ToString(gridSearchRequest.PROD_SPEC_NO1) + "'"
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


            if (!string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE1) && !string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE2))
            {               
                dateOrGroup.Add(new SearchField
                {
                    FieldName = "HT.START_DATE_HARDENING",
                    SearchType = SearchType.DateTime,
                    SearchText1 = gridSearchRequest.INSPECTION_DATE1,
                    SearchText2 = gridSearchRequest.INSPECTION_DATE2
                });

                dateOrGroup.Add(new SearchField
                {
                    FieldName = "HT.END_DATE_HARDENING",
                    SearchType = SearchType.DateTime,
                    SearchText1 = gridSearchRequest.INSPECTION_DATE1,
                    SearchText2 = gridSearchRequest.INSPECTION_DATE2
                });

                dateOrGroup.Add(new SearchField
                {
                    FieldName = "HT.START_DATE_TEMPERING",
                    SearchType = SearchType.DateTime,
                    SearchText1 = gridSearchRequest.INSPECTION_DATE1,
                    SearchText2 = gridSearchRequest.INSPECTION_DATE2
                });

                dateOrGroup.Add(new SearchField
                {
                    FieldName = "HT.END_DATE_TEMPERING",
                    SearchType = SearchType.DateTime,
                    SearchText1 = gridSearchRequest.INSPECTION_DATE1,
                    SearchText2 = gridSearchRequest.INSPECTION_DATE2
                });

                dateOrGroup.Add(new SearchField
                {
                    FieldName = "HTQ.INSPECTION_DATE",
                    SearchType = SearchType.DateTime,
                    SearchText1 = DateTime.Parse(gridSearchRequest.INSPECTION_DATE1).ToString("yyyy/MM/dd"),
                    SearchText2 = DateTime.Parse(gridSearchRequest.INSPECTION_DATE2).ToString("yyyy/MM/dd")
                });

                //searchFields.Add(new SearchField
                //{
                //    FieldName = "(" + string.Join(" OR ",
                //        dateOrGroup.Select(f =>
                //            $"{f.FieldName} BETWEEN TO_DATE('{f.SearchText1}','yyyy/MM/dd hh24:mi') " +
                //            $"AND TO_DATE('{f.SearchText2}','yyyy/MM/dd hh24:mi')")) + ")",
                //    SearchType = SearchType.Custom
                //});
                searchFields.Add(new SearchField
                {
                    FieldName = "(" + string.Join(" OR ",dateOrGroup.Select(f =>
           {
               // ✅ FIX: DATE FIELD HANDLING
               if (f.FieldName == "HTQ.INSPECTION_DATE")
               {
                   return $"{f.FieldName} BETWEEN " +
                          $"TO_DATE('{f.SearchText1} 00:00','yyyy/MM/dd hh24:mi') " +
                          $"AND TO_DATE('{f.SearchText2} 23:59','yyyy/MM/dd hh24:mi')";
               }

               // DATETIME FIELDS
               return $"{f.FieldName} BETWEEN " +
                      $"TO_DATE('{f.SearchText1}','yyyy/MM/dd hh24:mi') " +
                      $"AND TO_DATE('{f.SearchText2}','yyyy/MM/dd hh24:mi')";
           })
       ) + ")",
                    SearchType = SearchType.Custom
                });
            }

            //if (string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE2))
            //{
            //    if (!string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE1))
            //    {
            //        var searchField = new SearchField
            //        {
            //            FieldName = "NVL(HT.END_DATE_HARDENING,HTQ.INSPECTION_DATE)",
            //            SearchType = SearchType.DateTime,
            //            SearchText1 = gridSearchRequest.INSPECTION_DATE1
            //        };
            //        searchFields.Add(searchField);
            //    }
            //}
            //else if (string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE1))
            //{
            //    if (!string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE2))
            //    {
            //        var searchField = new SearchField
            //        {
            //            FieldName = "NVL(HT.END_DATE_HARDENING,HTQ.INSPECTION_DATE)",
            //            SearchType = SearchType.DateTime,
            //            SearchText1 = gridSearchRequest.INSPECTION_DATE2
            //        };
            //        searchFields.Add(searchField);
            //    }
            //}
            //else if (!string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE1) && !string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE2))
            //{
            //    var searchField = new SearchField
            //    {
            //        FieldName = "NVL(HT.END_DATE_HARDENING,HTQ.INSPECTION_DATE)",
            //        SearchType = SearchType.DateTime,
            //        SearchText1 = gridSearchRequest.INSPECTION_DATE1,
            //        SearchText2 = gridSearchRequest.INSPECTION_DATE2
            //    };
            //    searchFields.Add(searchField);
            //}
            //if (gridSearchRequest.MACHINE_CD > 0)
            //{
            //    var searchField = new SearchField
            //    {
            //        FieldName = "HT.HARDENING_MACHINE_CD",
            //        SearchType = SearchType.Number,
            //        SearchText1 = Convert.ToString(gridSearchRequest.MACHINE_CD)
            //    };
            //    searchFields.Add(searchField);
            //}
            //if (gridSearchRequest.MACHINE_CD > 0)
            //{
            //    var searchField = new SearchField
            //    {
            //        FieldName = "HT.TEMPERING_MACHINE_CD",
            //        SearchType = SearchType.Number,
            //        SearchText1 = Convert.ToString(gridSearchRequest.MACHINE_CD)
            //    };
            //    searchFields.Add(searchField);
            //}

            return searchFields;
        }


        public List<HardeningTemperingDetails> GetHardnessTemperingDetailsOnDailyProcessAnalysis(GridSearchFields gridSearchRequest)
        {


            List<SearchField> searchFields = ComposeSearchCriteriaforDailyProcess(gridSearchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);

            var obj = _hardnessTemperingRepository.GetHardnessTemperingDetailsOnDailyProcessAnalysis(searchFields, selectedSortCriteria, gridSearchRequest);           
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
                    SearchText1 ="'"+ gridSearchRequest.PROD_SPEC_NO1 +"'"
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
            if (!string.IsNullOrEmpty(gridSearchRequest.QUENCHING_MACHINE))
            {
                var searchField = new SearchField
                {
                    FieldName = "MACH.MACHINE_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.QUENCHING_MACHINE.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.TEMPERING_MACHINE))
            {
                var searchField = new SearchField
                {
                    FieldName = "MACT.MACHINE_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.TEMPERING_MACHINE.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.Q_OPERATOR))
            {
                var searchField = new SearchField
                {
                    FieldName = "EMPH.SHORT_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.Q_OPERATOR.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.Q_FURNACE_IN_START_DATE))
            {
                var searchField = new SearchField
                {
                    FieldName = "HT.START_DATE_HARDENING",
                    SearchType = SearchType.DateTime,
                    SearchText1 = gridSearchRequest.Q_FURNACE_IN_START_DATE,
                    SearchText2 = gridSearchRequest.Q_FURNACE_IN_END_DATE
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.Q_FURNACE_OUT_START_DATE))
            {
                var searchField = new SearchField
                {
                    FieldName = "HT.END_DATE_HARDENING",
                    SearchType = SearchType.DateTime,
                    SearchText1 = gridSearchRequest.Q_FURNACE_OUT_START_DATE,
                    SearchText2 = gridSearchRequest.Q_FURNACE_OUT_END_DATE
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.T_OPERATOR))
            {
                var searchField = new SearchField
                {
                    FieldName = "EMPT.SHORT_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.T_OPERATOR.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.T_FURNACE_IN_START_DATE))
            {
                var searchField = new SearchField
                {
                    FieldName = "HT.START_DATE_TEMPERING",
                    SearchType = SearchType.DateTime,
                    SearchText1 = gridSearchRequest.T_FURNACE_IN_START_DATE,
                    SearchText2 = gridSearchRequest.T_FURNACE_IN_END_DATE
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.T_FURNACE_OUT_START_DATE))
            {
                var searchField = new SearchField
                {
                    FieldName = "HT.END_DATE_TEMPERING",
                    SearchType = SearchType.DateTime,
                    SearchText1 = gridSearchRequest.T_FURNACE_OUT_START_DATE,
                    SearchText2 = gridSearchRequest.T_FURNACE_OUT_END_DATE
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE1))
            {
                var searchField = new SearchField
                {
                    FieldName = "HTQ.INSPECTION_DATE",
                    SearchType = SearchType.Date,
                    SearchText1 = gridSearchRequest.INSPECTION_DATE1,
                    SearchText2 = gridSearchRequest.INSPECTION_DATE2
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.OPERATOR_NAME))
            {
                var searchField = new SearchField
                {
                    FieldName = "EMPL.EMPLOYEE_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.OPERATOR_NAME.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.DECISION > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "HTQ.DECISION",
                    SearchType = SearchType.Text,
                    SearchText1 = Convert.ToString(gridSearchRequest.DECISION)
                };
                searchFields.Add(searchField);
            }

            return searchFields;
        }
        public List<NameValue> GetNextProcessList()
        {
            var _nextProcessList = UIUtils.GetNextProcesKubun();
            return _nextProcessList;
        }
        public List<NameValue> GetDecisionList()
        {
            var _DecisionList = UIUtils.GetResultKubun(); // UIUtils.GetHrdResultKubun();
            return _DecisionList;
        }
        public List<NameValue> GetTempTypeList()
        {
            var _TempList = UIUtils.GetTempTypeKubun();
            return _TempList;
        }

        public List<NameValue> GetTextureTypeList()
        {
            var _TempList = UIUtils.GetTextureKubun();
            return _TempList;
        }

        public int GetFurnaceDetails(string furnaceNo)
        {
            return _hardnessTemperingRepository.GetFurnaceNumber(furnaceNo);
        }

        public HardeningTemperingDetails FurnaceIsAlreadyEntered(string furnaceNo)
        {
            return _hardnessTemperingRepository.FurnaceIsAlreadyEntered(furnaceNo);
        }
        public List <HardeningTemperingDetails> GetHardnessTemperingTempDetailsByUser(int userID)
        {
            return _hardnessTemperingRepository.GetHardnessTemperingTempDetailsByUser(userID);
        }

        public HardeningTemperingDetails GetHTQualityDetailsByCD(long HTQualityCD)
        {
            var result = _hardnessTemperingRepository.GetHTQualityDetailsByCD(HTQualityCD);

            if (result != null)
            {
                if (result.WATER_OIL_TYPE > 0)
                {
                    result.WATER_OIL_STRING = UIUtils.GetHardnessOilTypeKubunValue(Convert.ToString(result.WATER_OIL_TYPE));
                }

            }
            return result;

        }

       
        public int GetRowCountForHardnessTemperingtFactory(GridSearchFields gridSearchRequest)
        {
            List<SearchField> searchFields = ComposeSearchCriteriaforFactoryDailyWorkStatus(gridSearchRequest);
            int count = _hardnessTemperingRepository.GetRowCountForHardnessTemperingtFactory(searchFields);
            return count;
        }

        public int GetRowCountForHardnessTemperingDailyAnalysis(GridSearchFields gridSearchRequest)
        {
            List<SearchField> searchFields = ComposeSearchCriteriaforDailyProcess(gridSearchRequest);
            int count = _hardnessTemperingRepository.GetRowCountForHardnessTemperingDailyAnalysis(searchFields);
            return count;
        }
        public List<ThabaDetails> GetThabaDetailsByProcessPlan(int processPlanCD)
        {
            return _hardnessTemperingRepository.GetThabaDetails(processPlanCD);
            
        }
        public bool IsProcessPlanAlreadyExist(GridSearchFields request)
        {
            return _hardnessTemperingRepository.IsProcessPlanSaved(request);
        }


    }
}
