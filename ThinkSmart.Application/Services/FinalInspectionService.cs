using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Application.IServices;
using TSWebApplication.Infrastructure.IRepositories;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Domain.Resources;
using System.Data;
using TSWebApplication.Infrastructure.Repositories;

namespace TSWebApplication.Application.Services
{
    public class FinalInspectionService : IFinalInspectionService
    {
        private readonly IFinalInspectionRepository _FinalInspectionRepository;
        private readonly LocalizationServices _localization;
        private readonly IAllCommonService _allCommonService;
        private readonly IUserLoginRepository _userLogin;
        // private readonly IStamppingRepository _stamppingRepository;
        public FinalInspectionService(IFinalInspectionRepository FinalInspectionRepository, IStamppingRepository stamppingRepository, LocalizationServices localization, IAllCommonService allCommonService, IUserLoginRepository userLogin)
        {
            _FinalInspectionRepository = FinalInspectionRepository;
            _localization = localization;
            _allCommonService = allCommonService;
            _userLogin = userLogin;
            //_stamppingRepository = stamppingRepository;
        }
        public List<FinalInspectionDetails> GetFinalInspectionDataOnFactoryDailyWorkStatus(GridSearchFields gridSearchRequest)
        {
            //List<ECDetails> result = new List<ECDetails>();
            List<SearchField> searchFields = ComposeSearchCriteriaforFactory(gridSearchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);
            int rowLimit = gridSearchRequest.ROW_LIMIT;
            var obj = _FinalInspectionRepository.GetFinalInspectionDataOnFactoryDailyWorkStatus(searchFields, selectedSortCriteria, rowLimit);
            return obj;
        }
        public List<FinalInspectionDetails> GetFinalInspectionDataOnDailyProcessStatus(GridSearchFields gridSearchRequest)
        {
            //List<ECDetails> result = new List<ECDetails>();
            List<SearchField> searchFields = ComposeSearchCriteriaforDailyProcessAnalysisStatus(gridSearchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);
            var obj = _FinalInspectionRepository.GetFinalInspectionDataOnDailyProcessStatus(searchFields, selectedSortCriteria, gridSearchRequest);
            return obj;
        }
        private List<SearchField> ComposeSearchCriteriaforFactory(GridSearchFields gridSearchRequest)
        {
            var searchFields = new List<SearchField>();
            //if (!string.IsNullOrEmpty(gridSearchRequest.ddlMachine))
            //{
            //    var searchField = new SearchField
            //    {
            //        FieldName = "EC.MACHINE_CD",
            //        SearchType = SearchType.Text,
            //        SearchText1 = gridSearchRequest.ddlMachine
            //    };
            //    searchFields.Add(searchField);
            //}
            if (gridSearchRequest.LOCATION_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "FI.INPUT_LOCATION_CD",//change 2025/12/11 as per client requirement
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
                    FieldName = "FI.INSP_BY",
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
                        FieldName = "FI.INSP_DATE",
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
                        FieldName = "FI.INSP_DATE",
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
                    FieldName = "FI.INSP_DATE",
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
                    SearchText1 =  gridSearchRequest.MATL_SHAPE 
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
                    FieldName = "FI.INSP_DATE",
                    SearchType = SearchType.DateTime,
                    SearchText1 = gridSearchRequest.INSPECTION_DATE1,
                    SearchText2 = gridSearchRequest.INSPECTION_DATE2
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.EMPLOYEE_NAME))
            {
                var searchField = new SearchField
                {
                    FieldName = "EMP2.SHORT_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.EMPLOYEE_NAME
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.START_DATETIME))
            {
                var searchField = new SearchField
                {
                    FieldName = "FI.SPARK_INSP_DATE",
                    SearchType = SearchType.DateTime,
                    SearchText1 = gridSearchRequest.START_DATETIME,
                    SearchText2 = gridSearchRequest.END_DATETIME
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.OPERATOR_NAME))
            {
                var searchField = new SearchField
                {
                    FieldName = "EMP3.SHORT_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.OPERATOR_NAME
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.DECISION > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "FI.COMBINED_DECISION",
                    SearchType = SearchType.Text,
                    SearchText1 = Convert.ToString(gridSearchRequest.DECISION)
                };
                searchFields.Add(searchField);
            }
            return searchFields;
        }

                     
        public int GetRowCountForFinalInspectionFactory(GridSearchFields gridSearchRequest)
        {
            List<SearchField> searchFields = ComposeSearchCriteriaforFactory(gridSearchRequest);
            int count = _FinalInspectionRepository.GetRowCountForFinalInspectionFactory(searchFields);
            return count;
        }
        public int GetRowCountForFinalInspectionDailyAnalysis(GridSearchFields gridSearchRequest)
        {
            List<SearchField> searchFields = ComposeSearchCriteriaforDailyProcessAnalysisStatus(gridSearchRequest);
            int count = _FinalInspectionRepository.GetRowCountForFinalInspectionDailyAnalysis(searchFields);
            return count;
        }
        public List<NameValue> GetLocation(int USER_ID)
        {
            var result = _userLogin.GetUserLocationNameById(USER_ID);
            return result;
        }
        public List<FinalInspectionDetails> LoadFinalInspectionDetails(string startDate, string endDate, string endOperator, int orderBy, string printFlag, int isPrintByPrintFlag, int userID)
        {
            var finalInspectionDetails = _FinalInspectionRepository.GetFinalInspectDetails(startDate, endDate, endOperator, orderBy, printFlag, isPrintByPrintFlag, userID);
            return finalInspectionDetails;

            var chubbanThabaDetails = _FinalInspectionRepository.GetStartChuuban(startDate, endDate, printFlag, isPrintByPrintFlag);

            return chubbanThabaDetails;
        }

        public int GetFinishProductDetails(string startDate, string endDate, string endOperator, int printFlag, int userID)
        {
            int finalInspectionDetails = _FinalInspectionRepository.GetFinishProductDetails(startDate, endDate, endOperator, printFlag,userID);
            return finalInspectionDetails;

        }
        public int GetSimpleFinalInspectionDetails(string startDate, string endDate, string endOperator, int printFlag, int userID)
        {
            int finalInspectionDetails = _FinalInspectionRepository.GetSimpleFinalInspectionDetails(startDate, endDate, endOperator, printFlag, userID);
            return finalInspectionDetails;

        }
        
    }
}
