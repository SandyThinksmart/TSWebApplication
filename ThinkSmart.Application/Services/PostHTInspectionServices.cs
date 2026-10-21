using MailKit.Search;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Transactions;
using TSWebApplication.Application.IServices;
using TSWebApplication.Infrastructure.IRepositories;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Infrastructure.Repositories;
using TSWebApplication.Domain.Resources;
using System.Data;

namespace TSWebApplication.Application.Services
{
    public class PostHTInspectionServices : IPostHTInspectionService
    {
        private readonly IPostHTInspectionRepository _postHTInspectionRepository;

        private readonly IUserLoginRepository _userLogin;
        private readonly IAllCommonRepository _AllCommonRepository;
        private readonly LocalizationServices _localization;
        private readonly IAllCommonService _allCommonService;
        public PostHTInspectionServices(IPostHTInspectionRepository PostHTInspectionRepository, IUserLoginRepository userLogin, IAllCommonRepository AllCommonRepository, LocalizationServices localization, IAllCommonService allCommonService)
        {
            _userLogin = userLogin;
            _postHTInspectionRepository = PostHTInspectionRepository;
            _AllCommonRepository = AllCommonRepository;
            _localization = localization;
            _allCommonService = allCommonService;
        }
        public List<NameValue> GetStackPositionList()
        {
            var _nextProcessList = UIUtils.GetStackPositionKubun();
            return _nextProcessList;
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

        public List<NameValue> GetMachineName()
        {
            return _postHTInspectionRepository.GetMachineNameList();

        }

        public List<PostHTDetails> GetChubaanDetails(List<string> chubaanNos)
        {
            List<ProductOrderCompletion> productOrderDetailsList = new List<ProductOrderCompletion>();
            List<PostHTDetails> PostHTDetailsList = new List<PostHTDetails>();
            productOrderDetailsList = _postHTInspectionRepository.GetProcessPlanDetails(chubaanNos);
            if (productOrderDetailsList != null && productOrderDetailsList.Count > 0)
            {

                foreach (var item in productOrderDetailsList)
                {
                    var postHTDetails = new PostHTDetails();
                    postHTDetails.CHUUBAN = item.CHUUBAN;
                    postHTDetails.CUSTOMER_NAME = item.CUSTOMER;
                    postHTDetails.PROD_NAME = item.PROD_NAME_CUST;
                    postHTDetails.CHARGE = item.CHARGE;
                    postHTDetails.MATL_DIM_D = Convert.ToSingle(item.MATL_DIM_D);
                    postHTDetails.PROD_DIM_D = Convert.ToSingle(item.PP_PROD_DIM_D);
                    //postHTDetails.GET_PP_STD_DMT = Convert.ToInt16(item.DECARB_VAL_1);
                    //postHTDetails.GET_PP_STD_DMF = Convert.ToInt16(item.DECARB_VAL_3);
                    //postHTDetails.MATL_DIM_D = Convert.ToSingle(item.MATL_DIM_D);

                    postHTDetails.PROCESS_PLAN_CD = Convert.ToInt32(item.PROCESS_PLAN_CD);
                    postHTDetails.INTERNAL_QUALITY_GROUP_NO = item.INTERNAL_QUALITY_GROUP_NO;


                    PostHTDetailsList.Add(postHTDetails);
                }
            }
            return PostHTDetailsList;
        }
        public int GetMaxGruopNo()
        {
            var result = _postHTInspectionRepository.GetGroupNoDetails();
            return result;
        }

        public List<PostHTDetails> savePostHTInspectionDetails(List<PostHTDetails> requestData)
        {
            List<PostHTDetails> PostHT = new List<PostHTDetails>();
            using (TransactionScope transaction = new TransactionScope())
            {
                foreach (var request in requestData)
                {
                    if (request.USER_ID != null && request.USER_ID > 0)
                    {
                        request.INPUT_LOCATION_CD = Convert.ToInt16(_userLogin.GetUserLocationById(request.USER_ID.Value));
                        request.LOCATION_CD = request.INPUT_LOCATION_CD;
                    }
                    if (request.FURNACE_NO == null)
                    {
                        request.FURNACE_NO = "0";
                    }
                    if (request.IQ_NEXT_PROCESS == 0)
                    {
                        request.IQ_NEXT_PROCESS = null;
                    }


                    request.PROCESS_KUBUN_ID = Convert.ToInt32(UIUtils.GetProcessKubun(_localization.GetLocalizedHtmlString("lbl_ProcessInternal_Quality_Inspection")));
                    _postHTInspectionRepository.savePostHTInspectionDetails(request);
               
                   PostHT = _postHTInspectionRepository.FurnaceHdfDetails(requestData[0].FURNACE_NO);

                    //if (request.IQ_NEXT_PROCESS != 999)
                    //{
                        //WipStatusDetails wipDetails = new WipStatusDetails();

                        //wipDetails = new WipStatusDetails();
                        //wipDetails.PROCESS_PLAN_CD = request.PROCESS_PLAN_CD;
                        //wipDetails.LOCATION_CD = Convert.ToInt16(request.LOCATION_CD);
                        //wipDetails.MSG_CD = null;
                        //wipDetails.FINISHED_BARS = null;
                        //wipDetails.UNIT = null;
                        //wipDetails.FINISHED_QTY = null;
                        //wipDetails.PROCESS_START_DATE = request.INT_QLTY_TEST_DATE;
                        //wipDetails.PROCESS_END_DATE = null;
                        //wipDetails.PROCESS_KUBUN_CD = Convert.ToInt32(UIUtils.GetProcessKubun(_localization.GetLocalizedHtmlString("lbl_ProcessInternal_Quality_Inspection")));
                        //wipDetails.PRE_PROSESS_DECISION = request.INTERNAL_QUALITY_DECISION;
                        //wipDetails.NEXT_PROCESS = Convert.ToInt16(request.IQ_NEXT_PROCESS);
                        //wipDetails.START_OPERATOR_CD = request.INT_QLTY_TEST_OPERATOR_CD;
                        //wipDetails.END_OPERATOR_CD = null;
                        //wipDetails.TABA_NO = 0;

                        //_AllCommonRepository.InsertWIPStatusData(wipDetails);
                    //}
                }
                transaction.Complete();
            }
            return PostHT;
        }
        public PostHTDetails updatePostHTInspectionDetails(List<PostHTDetails> requestData)
        {
            PostHTDetails PostHT = new PostHTDetails();
            using (TransactionScope transaction = new TransactionScope())
            {
                foreach (var request in requestData)
                {

                    if (request.USER_ID != null && request.USER_ID > 0)
                    {
                        request.INPUT_LOCATION_CD = Convert.ToInt16(_userLogin.GetUserLocationById(request.USER_ID.Value));
                        request.LOCATION_CD = request.INPUT_LOCATION_CD;
                    }
                    if (request.FURNACE_NO == null)
                    {
                        request.FURNACE_NO = "0";
                    }

                    _postHTInspectionRepository.updatePostHTInspectionDetails(request);

                }

                transaction.Complete();
            }
            return PostHT;
        }

        public void PostHTDetailsDelete(long groupNo)
        {
            _postHTInspectionRepository.PostHTDetailsDelete(groupNo);
        }
        public int PostHTInspectionCheckMilkSheetData(long groupNo)
        {
           return _postHTInspectionRepository.PostHTInspectionCheckMilkSheetData(groupNo);
        }
        public List<PostHTDetails> GetPostInspList(List<SearchField> searchCriteria, GridSearchFields searchRequest)
        {
            short LocationCD = Convert.ToInt16(_postHTInspectionRepository.GetUserLocationById(Convert.ToInt32(searchRequest.USER_ID)));
            searchRequest.LOCATION_CD = LocationCD;
            List<PostHTDetails> list = new List<PostHTDetails>();
            //List<SearchField>  = ComposeSearchCriteria(searchRequest);
            string sortCiteria = Helper.ParseSortCriteria(searchRequest.SORT_FIELDS);
            var result = _postHTInspectionRepository.GetPostInspList(searchCriteria, sortCiteria);
            return result;
            //if (list != null && list.Any())
            //{
            //    foreach (var item in list)
            //    {
            //        item.PROC_CONDITION_STRING = UIUtils.GetProcConditionKubunValue(Convert.ToString(item.HT_CONDITION));
            //    }
            //}
            //return list;
        }

        public PostHTDetails GetChuubanListForInvaidCheck(string enteredCChubaans)
        {
            return _postHTInspectionRepository.GetChuubanListForInvaidCheck(enteredCChubaans);
        }
        public List<string> GetCompletedChuubanList(string enteredCChubaans)
        {
            return _postHTInspectionRepository.GetCompletedChuubanList(enteredCChubaans);
        }
        public List<PostHTDetails> getPostHTDetailsByInternalQCD(long groupNo)
        {
            return _postHTInspectionRepository.GetProcPlanCDDetailsByInternalQCD(groupNo);

        }

        public int checkvalidFurnaceNo(string furnaceNo)
        {
            return _postHTInspectionRepository.checkvalidFurnaceNo(furnaceNo);

        }

        public List<PostHTDetails> getFurnaceDetails(string furnaceNo)
        {
            return _postHTInspectionRepository.getFurnaceDetails(furnaceNo);
        }
        public List<PostHTDetails> GetQATestHardnessOnFactoryDailyWorkStatus(GridSearchFields gridSearchRequest)
        {
            List<SearchField> searchFields = ComposeSearchCriteriaforFactoryDailyWorkStatus(gridSearchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);
            int rowLimit = gridSearchRequest.ROW_LIMIT;
            var result = _postHTInspectionRepository.GetQATestHardnessOnFactoryDailyWorkStatus(searchFields, selectedSortCriteria,rowLimit);            
            return result;
        }

        private List<SearchField> ComposeSearchCriteriaforFactoryDailyWorkStatus(GridSearchFields gridSearchRequest)
        {
            var searchFields = new List<SearchField>();
            if (gridSearchRequest.MACHINE_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "IQ.MACHINE_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MACHINE_CD)
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.LOCATION_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "IQ.LOCATION_CD",
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
                    FieldName = "IQ.INT_QLTY_TEST_OPERATOR_CD",
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
            if (string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE2))
            {
                if (!string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE1))
                {
                    var searchField = new SearchField
                    {
                        FieldName = "IQ.INT_QLTY_TEST_DATE",
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
                        FieldName = "IQ.INT_QLTY_TEST_DATE",
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
                    FieldName = "IQ.INT_QLTY_TEST_DATE",
                    SearchType = SearchType.DateTime,
                    SearchText1 = gridSearchRequest.INSPECTION_DATE1,
                    SearchText2 = gridSearchRequest.INSPECTION_DATE2
                };
                searchFields.Add(searchField);
            }


            return searchFields;
        }

        public List<PostHTDetails> GetQATestHardnessDetailsOnDailyProcessAnalysis(GridSearchFields gridSearchRequest)
        {


            List<SearchField> searchFields = ComposeSearchCriteriaforDailyProcess(gridSearchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);

            var obj = _postHTInspectionRepository.GetQATestHardnessDetailsOnDailyProcessAnalysis(searchFields, selectedSortCriteria, gridSearchRequest);            
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
            if (!string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE1))
            {
                var searchField = new SearchField
                {
                    FieldName = "IQ.INT_QLTY_TEST_DATE",
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
                    SearchText1 = gridSearchRequest.EMPLOYEE_NAME.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.HT_CONDITION))
            {
                var searchField = new SearchField
                {
                    FieldName = "IQ.HT_CONDITION",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.HT_CONDITION
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.PATTERN_NO))
            {
                var searchField = new SearchField
                {
                    FieldName = "IQ.PATTERN_NO",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.PATTERN_NO.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.FURNACE_NO))
            {
                var searchField = new SearchField
                {
                    FieldName = "IQ.FURNACE_NO",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.FURNACE_NO.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.DECISION > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "IQ.INTERNAL_QUALITY_DECISION",
                    SearchType = SearchType.Text,
                    SearchText1 = Convert.ToString(gridSearchRequest.DECISION)
                };
                searchFields.Add(searchField);
            }

            return searchFields;
        }
        public List<NameValue> GetProcConditionKubun()
        {
            List<NameValue> ProcConditionKubunList = UIUtils.GetProcConditionKubun();
            return ProcConditionKubunList;
        }
        public List<NameValue> GetInOutKubun()
        {
            List<NameValue> ProcConditionKubunList = UIUtils.Instance.GetInOut();
            return ProcConditionKubunList;
        }
        public List<NameValue> GetOrientationKubun()
        {
            List<NameValue> ProcConditionKubunList = UIUtils.Instance.GetOrientationKubun();
            return ProcConditionKubunList;
        }
        public List<PostHTDetails> GetFurnaceNo(string furnaceNo, bool isNull)
        {
            var startDetails = _postHTInspectionRepository.GetFurnaceNoDetails(furnaceNo, isNull);

            return startDetails;
        }

             
        public int GetRowCountForQATestHardnessFactory(GridSearchFields gridSearchRequest)
        {
            List<SearchField> searchFields = ComposeSearchCriteriaforFactoryDailyWorkStatus(gridSearchRequest);
            int count = _postHTInspectionRepository.GetRowCountForQATestHardnessFactory(searchFields);
            return count;
        }
        public int GetRowCountForPostHTHardnessDailyAnalysis(GridSearchFields gridSearchRequest)
        {
            List<SearchField> searchFields = ComposeSearchCriteriaforDailyProcess(gridSearchRequest);
            int count = _postHTInspectionRepository.GetRowCountForPostHTHardnessDailyAnalysis(searchFields);
            return count;
        }
    }

}
