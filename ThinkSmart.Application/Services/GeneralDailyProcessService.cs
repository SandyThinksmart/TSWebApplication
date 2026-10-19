using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Transactions;
using TSWebApplication.Application.IServices;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Infrastructure.IRepositories;
using TSWebApplication.Infrastructure.Repositories;
using static TSWebApplication.Domain.Utility.Constants;

namespace TSWebApplication.Application.Services
{
    public class GeneralDailyProcessService : IGeneralDailyProcessService
    {
        private readonly IGeneralDailyProcessRepository _generalDailyprocessRepository;
        private readonly LocalizationServices _localization;
        private readonly IAllCommonService _allCommonService;
        private readonly IAllCommonRepository _CommonRepository;
        public GeneralDailyProcessService(IGeneralDailyProcessRepository generalDailyprocessRepository, LocalizationServices localization, IAllCommonService allCommonService, IAllCommonRepository commonRepository)
        {
            _generalDailyprocessRepository = generalDailyprocessRepository;
            _localization = localization;
            _allCommonService = allCommonService;
            _CommonRepository = commonRepository;
        }

        public List<GeneralDailyProcessDetails> GetGeneralDailyProcessData(GridSearchFields gridSearchRequest)
        {
            List<SearchField> searchFields = ComposeSearchCriteriaforDailyProcess(gridSearchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);

            var obj = _generalDailyprocessRepository.GetGeneralDailyProcessData(searchFields, selectedSortCriteria, gridSearchRequest);

            return obj;
        }
        public GeneralDailyProcessDetails CheckValidationsOnGeneralProcessStart(GeneralDailyProcessDetails processDetails)
        {

            var obj = _generalDailyprocessRepository.CheckValidationsOnGeneralProcessStart(processDetails);

            return obj;
        }
        public GeneralDailyProcessDetails checkTabaValidationForOkDecision(GeneralDailyProcessDetails processDetails)
        {

            var obj = _generalDailyprocessRepository.checkTabaValidationForOkDecision(processDetails);

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
            if (gridSearchRequest.PROCESS_KUBUN_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PROCK.KUBUN_ID",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.PROCESS_KUBUN_CD)
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.MACHINE_NAME))
            {
                var searchField = new SearchField
                {
                    FieldName = "MAC.MACHINE_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.MACHINE_NAME
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.LOCATION_NAME))
            {
                var searchField = new SearchField
                {
                    FieldName = "LOC.SHORT_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.LOCATION_NAME
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.BEGINNING_START_DATE))
            {
                var searchField = new SearchField
                {
                    FieldName = "GP.GP_START_DATE",
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
                    SearchText1 = gridSearchRequest.START_OPERATOR
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.END_DATE1))
            {
                var searchField = new SearchField
                {
                    FieldName = "GP.GP_END_DATE",
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
                    SearchText1 = gridSearchRequest.END_OPERATOR
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.DECISION > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "GP.GP_DECISION",
                    SearchType = SearchType.Text,
                    SearchText1 = Convert.ToString(gridSearchRequest.DECISION)
                };
                searchFields.Add(searchField);
            }
            return searchFields;
        }

        public List<GeneralDailyProcessDetails> GetProcessKubunList()
        {
            return _generalDailyprocessRepository.GetProcessKubunList();
        }

        public List<GeneralDailyProcessDetails> GetGeneralProcessOnFactoryDailyWorkStatus(GridSearchFields gridSearchRequest)
        {
            List<SearchField> searchFields = ComposeSearchCriteriaforFactoryDailyWorkStatus(gridSearchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);
            int rowLimit = gridSearchRequest.ROW_LIMIT;
            var result = _generalDailyprocessRepository.GetGeneralProcessOnFactoryDailyWorkStatus(searchFields, selectedSortCriteria, rowLimit);
            return result;
        }
        private List<SearchField> ComposeSearchCriteriaforFactoryDailyWorkStatus(GridSearchFields gridSearchRequest)
        {
            var searchFields = new List<SearchField>();
            if (gridSearchRequest.MACHINE_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "GP.MACHINE_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MACHINE_CD)
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.LOCATION_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "NVL(GP.LOCATION_CD,GP.INPUT_LOCATION_CD)",//change 2025/12/11 as per client requirement
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.LOCATION_CD)
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.PROCESS_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "GP.PROCESS_KUBUN_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.PROCESS_CD)
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
                    FieldName = "GP.GP_END_OPERATOR_CD",
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
                        FieldName = "GP.GP_END_DATE",
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
                        FieldName = "GP.GP_END_DATE",
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
                    FieldName = "GP.GP_END_DATE",
                    SearchType = SearchType.DateTime,
                    SearchText1 = gridSearchRequest.INSPECTION_DATE1,
                    SearchText2 = gridSearchRequest.INSPECTION_DATE2
                };
                searchFields.Add(searchField);
            }

            return searchFields;
        }
        public GeneralDailyProcessDetails CheckValidationsForProcessOperatorMachineCD(GeneralDailyProcessDetails processDetails)
        {

            var obj = _generalDailyprocessRepository.CheckValidationsForProcessOperatorMachineCD(processDetails);

            return obj;
        }
        public string getRemarksOfProcess(GeneralDailyProcessDetails processDetails)
        {

            var obj = _generalDailyprocessRepository.getRemarksOfProcess(processDetails);

            return obj;
        }
        public string saveGeneralProcessPlanEntryData(GeneralDailyProcessDetails processDetails)
        {
            var obj = "";
            using (TransactionScope transaction = new TransactionScope())
            {
                if (processDetails.MACHINE_CD != null && processDetails.MACHINE_CD > 0)
                {
                    MST_MACHINE result =_CommonRepository.GetMachineNameByID(processDetails.MACHINE_CD.Value);
                    if(result !=null)
                    {
                        processDetails.LOCATION_CD= Convert.ToInt16(result.LOCATION_CD);
                    }
                }
                else
                {
                    processDetails.LOCATION_CD = Convert.ToInt16(_generalDailyprocessRepository.GetLocationByUserID(processDetails.USER_ID));
                }
                    obj = _generalDailyprocessRepository.saveGeneralProcessPlanEntryData(processDetails);
                if (processDetails.GP_TABA_NO_1 != null || processDetails.GP_TABA_NO_2 != null || processDetails.GP_TABA_NO_3 != null || processDetails.GP_TABA_NO_4 != null)
                {
                    if (obj == "Data Inserted Successfully")
                    {
                        WipStatusDetails wipDetails = new WipStatusDetails();

                        wipDetails = new WipStatusDetails();
                        wipDetails.PROCESS_PLAN_CD = processDetails.PP_PROCESS_PLAN_CD;
                        wipDetails.LOCATION_CD = Convert.ToInt16(processDetails.LOCATION_CD);
                        //wipDetails.MSG_CD = request.STR_MSG_CD;
                        wipDetails.FINISHED_BARS = processDetails.GP_OK_UNITS;
                        wipDetails.UNIT = processDetails.UNIT_TYPE;
                        wipDetails.FINISHED_QTY = processDetails.GP_QTY;
                        wipDetails.PROCESS_START_DATE = processDetails.GP_START_DATE;
                        wipDetails.PROCESS_END_DATE = processDetails.GP_END_DATE;
                        wipDetails.PROCESS_KUBUN_CD = Convert.ToInt32(processDetails.PROCESS_KUBUN_CD);
                        wipDetails.PRE_PROSESS_DECISION = processDetails.GP_DECISION;
                        wipDetails.NEXT_PROCESS = processDetails.GP_NEXT_PROCESS;
                        wipDetails.START_OPERATOR_CD = processDetails.GP_START_OPERATOR_CD;
                        wipDetails.END_OPERATOR_CD = processDetails.GP_END_OPERATOR_CD;
                        wipDetails.TABA_NO = processDetails.GP_TABA_NO_1;
                        wipDetails.NIFUDA_CD = null;
                        if (processDetails.GP_TABA_NO_1 != null || processDetails.GP_TABA_NO_2 != null || processDetails.GP_TABA_NO_3 != null || processDetails.GP_TABA_NO_4 != null)
                        {
                            wipDetails.TABA_GROUP_NO = processDetails.GP_TABA_NO_1;
                        }

                        _CommonRepository.DeleteWIPStatusData(wipDetails);

                        //if (processDetails.GP_TABA_NO_2 > 0)
                        //{

                        //    wipDetails.FINISHED_QTY = null;
                        //    wipDetails.TABA_NO = processDetails.GP_TABA_NO_2;
                        //    _CommonRepository.InsertWIPStatusData(wipDetails);
                        //}
                        //if (processDetails.GP_TABA_NO_3 > 0)
                        //{

                        //    wipDetails.FINISHED_QTY = null;
                        //    wipDetails.TABA_NO = processDetails.GP_TABA_NO_3;
                        //    _CommonRepository.InsertWIPStatusData(wipDetails);
                        //}
                        //if (processDetails.GP_TABA_NO_4 > 0)
                        //{

                        //    wipDetails.FINISHED_QTY = null;
                        //    wipDetails.TABA_NO = processDetails.GP_TABA_NO_4;
                        //    _CommonRepository.InsertWIPStatusData(wipDetails);
                        //}
                    }
                }
                    
                transaction.Complete();
            }
            return obj;
        }
        public GeneralDailyProcessDetails CheckValidationsOnGeneralProcessEnd(GeneralDailyProcessDetails processDetails)
        {

            var obj = _generalDailyprocessRepository.CheckValidationsOnGeneralProcessEnd(processDetails);

            return obj;
        }
        public GeneralDailyProcessDetails getGeneralDailyProcessStartData(GeneralDailyProcessDetails processDetails)
        {

            var obj = _generalDailyprocessRepository.getGeneralDailyProcessStartData(processDetails);

            return obj;
        }
        //public List<GeneralDailyProcessDetails> GetNextProcessNameList(GeneralDailyProcessDetails processDetails)
        //{

        //    var obj = _generalDailyprocessRepository.GetNextProcessNameList(processDetails);

        //    return obj;
        //}
        public List<NameValue> GetDecisionList()
        {
            var _DecisionList = UIUtils.GetResultKubun();
            return _DecisionList;
        }
        public GeneralDailyProcessDetails CheckValidationsForOperatorAndEndDateTime(GeneralDailyProcessDetails processDetails)
        {

            var obj = _generalDailyprocessRepository.CheckValidationsForOperatorAndEndDateTime(processDetails);

            return obj;
        }
        public string getRemarksForEndProcess(GeneralDailyProcessDetails processDetails)
        {

            var obj = _generalDailyprocessRepository.getRemarksForEndProcess(processDetails);

            return obj;
        }
        public string updateGeneralProcessPlanEndData(GeneralDailyProcessDetails processDetails)
        {
            var obj = "";
            using (TransactionScope transaction = new TransactionScope())
            {
                //if (processDetails.USER_ID != null && processDetails.USER_ID > 0)
                //{
                //    processDetails.LOCATION_CD = Convert.ToInt16(_generalDailyprocessRepository.GetLocationByUserID(processDetails.USER_ID));
                //}
                obj = _generalDailyprocessRepository.updateGeneralProcessPlanEndData(processDetails);
               
                    if (processDetails.GP_TABA_NO_1 != null || processDetails.GP_TABA_NO_2 != null || processDetails.GP_TABA_NO_3 != null || processDetails.GP_TABA_NO_4 != null)
                    {
                        if (obj == "Data Inserted Successfully")
                        {
                            if (processDetails.GP_NEXT_PROCESS != 999)
                            {

                                WipStatusDetails wipDetails = new WipStatusDetails();

                                wipDetails = new WipStatusDetails();
                                wipDetails.PROCESS_PLAN_CD = processDetails.PP_PROCESS_PLAN_CD;
                                wipDetails.LOCATION_CD = Convert.ToInt16(processDetails.LOCATION_CD);
                                //wipDetails.MSG_CD = request.STR_MSG_CD;
                                wipDetails.FINISHED_BARS = processDetails.GP_OK_UNITS;
                                wipDetails.UNIT = processDetails.UNIT_TYPE;
                                wipDetails.FINISHED_QTY = processDetails.GP_QTY;
                                wipDetails.PROCESS_START_DATE = processDetails.GP_START_DATE;
                                wipDetails.PROCESS_END_DATE = processDetails.GP_END_DATE;
                                wipDetails.PROCESS_KUBUN_CD = Convert.ToInt32(processDetails.PROCESS_KUBUN_CD);
                                wipDetails.PRE_PROSESS_DECISION = processDetails.GP_DECISION;
                                wipDetails.NEXT_PROCESS = processDetails.GP_NEXT_PROCESS;
                                wipDetails.START_OPERATOR_CD = processDetails.GP_START_OPERATOR_CD;
                                wipDetails.END_OPERATOR_CD = processDetails.GP_END_OPERATOR_CD;
                                wipDetails.TABA_NO = processDetails.GP_TABA_NO_1;
                                wipDetails.NIFUDA_CD = null;
                                if (processDetails.GP_TABA_NO_1 != null || processDetails.GP_TABA_NO_2 != null || processDetails.GP_TABA_NO_3 != null || processDetails.GP_TABA_NO_4 != null)
                                {
                                    wipDetails.TABA_GROUP_NO = processDetails.GP_TABA_NO_1;
                                }

                                _CommonRepository.DeleteWIPStatusData(wipDetails);

                                //if (processDetails.GP_TABA_NO_2 > 0)
                                //{

                                //    wipDetails.FINISHED_QTY = null;
                                //    wipDetails.TABA_NO = processDetails.GP_TABA_NO_2;
                                //    _CommonRepository.InsertWIPStatusData(wipDetails);
                                //}
                                //if (processDetails.GP_TABA_NO_3 > 0)
                                //{

                                //    wipDetails.FINISHED_QTY = null;
                                //    wipDetails.TABA_NO = processDetails.GP_TABA_NO_3;
                                //    _CommonRepository.InsertWIPStatusData(wipDetails);
                                //}
                                //if (processDetails.GP_TABA_NO_4 > 0)
                                //{

                                //    wipDetails.FINISHED_QTY = null;
                                //    wipDetails.TABA_NO = processDetails.GP_TABA_NO_4;
                                //    _CommonRepository.InsertWIPStatusData(wipDetails);
                                //}


                            }
                        }
                    }
                    
               
               
                transaction.Complete();
            }
            return obj;
        }
        public List<NameValue> GetUnitTypeList()
        {
            var _UnitTypeList = UIUtils.GetUnitTypetKubunForGeneralProcess();
            return _UnitTypeList;
        }
        //public List<GeneralDailyProcessDetails> GetProcessDefectList(GeneralDailyProcessDetails processDetails)
        //{

        //    var obj = _generalDailyprocessRepository.GetProcessDefectList(processDetails);

        //    return obj;
        //}
        public string saveGeneralProcessPlanEntryEndData(GeneralDailyProcessDetails processDetails)
        {
            var obj = "";
            using (TransactionScope transaction = new TransactionScope())
            {
                if (processDetails.MACHINE_CD != null && processDetails.MACHINE_CD > 0)
                {
                    MST_MACHINE result = _CommonRepository.GetMachineNameByID(processDetails.MACHINE_CD.Value);
                    if (result != null)
                    {
                        processDetails.LOCATION_CD = Convert.ToInt16(result.LOCATION_CD);
                    }
                }
                else
                {
                    processDetails.LOCATION_CD = Convert.ToInt16(_generalDailyprocessRepository.GetLocationByUserID(processDetails.USER_ID));
                }
                obj = _generalDailyprocessRepository.saveGeneralProcessPlanEntryEndData(processDetails);
                if (processDetails.GP_TABA_NO_1 != null || processDetails.GP_TABA_NO_2 != null || processDetails.GP_TABA_NO_3 != null || processDetails.GP_TABA_NO_4 != null)
                {
                    if (obj == "Data Inserted Successfully")
                    {
                        if (processDetails.GP_NEXT_PROCESS != 999)
                        {
                            WipStatusDetails wipDetails = new WipStatusDetails();

                            wipDetails = new WipStatusDetails();
                            wipDetails.PROCESS_PLAN_CD = processDetails.PP_PROCESS_PLAN_CD;
                            wipDetails.LOCATION_CD = Convert.ToInt16(processDetails.LOCATION_CD);
                            //wipDetails.MSG_CD = request.STR_MSG_CD;
                            wipDetails.FINISHED_BARS = processDetails.GP_OK_UNITS;
                            wipDetails.UNIT = processDetails.UNIT_TYPE;
                            wipDetails.FINISHED_QTY = processDetails.GP_QTY;
                            wipDetails.PROCESS_START_DATE = processDetails.GP_START_DATE;
                            wipDetails.PROCESS_END_DATE = processDetails.GP_END_DATE;
                            wipDetails.PROCESS_KUBUN_CD = Convert.ToInt32(processDetails.PROCESS_KUBUN_CD);
                            wipDetails.PRE_PROSESS_DECISION = processDetails.GP_DECISION;
                            wipDetails.NEXT_PROCESS = processDetails.GP_NEXT_PROCESS;
                            wipDetails.START_OPERATOR_CD = processDetails.GP_START_OPERATOR_CD;
                            wipDetails.END_OPERATOR_CD = processDetails.GP_END_OPERATOR_CD;
                            wipDetails.TABA_NO = processDetails.GP_TABA_NO_1;
                            wipDetails.NIFUDA_CD = null;
                            if (processDetails.GP_TABA_NO_1 != null || processDetails.GP_TABA_NO_2 != null || processDetails.GP_TABA_NO_3 != null || processDetails.GP_TABA_NO_4 != null)
                            {
                                wipDetails.TABA_GROUP_NO = processDetails.GP_TABA_NO_1;
                            }

                            _CommonRepository.DeleteWIPStatusData(wipDetails);

                            //if (processDetails.GP_TABA_NO_2 > 0)
                            //{

                            //    wipDetails.FINISHED_QTY = null;
                            //    wipDetails.TABA_NO = processDetails.GP_TABA_NO_2;
                            //    _CommonRepository.InsertWIPStatusData(wipDetails);
                            //}
                            //if (processDetails.GP_TABA_NO_3 > 0)
                            //{

                            //    wipDetails.FINISHED_QTY = null;
                            //    wipDetails.TABA_NO = processDetails.GP_TABA_NO_3;
                            //    _CommonRepository.InsertWIPStatusData(wipDetails);
                            //}
                            //if (processDetails.GP_TABA_NO_4 > 0)
                            //{

                            //    wipDetails.FINISHED_QTY = null;
                            //    wipDetails.TABA_NO = processDetails.GP_TABA_NO_4;
                            //    _CommonRepository.InsertWIPStatusData(wipDetails);
                            //}

                        }
                    }
                }
                
                transaction.Complete();
            }
            return obj;
        }
        public GeneralDailyProcessDetails GetUnitsWeightOnGeneralProcessEnd(GeneralDailyProcessDetails processDetails)
        {

            var obj = _generalDailyprocessRepository.GetUnitsWeightOnGeneralProcessEnd(processDetails);

            return obj;
        }
        public List<GeneralDailyProcessDetails> CheckValidationsOnGeneralDailyProcessCancel(GeneralDailyProcessDetails processDetails)
        {

            var obj = _generalDailyprocessRepository.CheckValidationsOnGeneralDailyProcessCancel(processDetails);

            return obj;
        }
        public GeneralDailyProcessDetails GetProcessNameOnMergeScreen(short PROCESS_KUBUN_CD)
        {

            var obj = _generalDailyprocessRepository.GetProcessNameOnMergeScreen(PROCESS_KUBUN_CD);

            return obj;
        }
        public string CheckValidationsProcessMachineOprOnCancel(GeneralDailyProcessDetails processDetails)
        {

            var obj = _generalDailyprocessRepository.CheckValidationsProcessMachineOprOnCancel(processDetails);

            return obj;
        }
        public string DeleteGeneralProcessPlanData(GeneralDailyProcessDetails processDetails)
        {
            var obj = "";
            using (TransactionScope transaction = new TransactionScope())
            {
                 obj = _generalDailyprocessRepository.DeleteGeneralProcessPlanData(processDetails);
            if (obj == "Success")
            {
                    WipStatusDetails wipDetails = new WipStatusDetails();
                    wipDetails = new WipStatusDetails();
                    wipDetails.PROCESS_PLAN_CD = processDetails.PP_PROCESS_PLAN_CD;
                    wipDetails.TABA_NO = Convert.ToInt16(processDetails.GP_TABA_NO_1);
                    if (processDetails.GP_TABA_NO_1 != null || processDetails.GP_TABA_NO_2 != null || processDetails.GP_TABA_NO_3 != null || processDetails.GP_TABA_NO_4 != null)
                    {
                        _CommonRepository.DeleteWIPStatusData(wipDetails);
                    }
            }
                transaction.Complete();
            }
            return obj;
        }
       
        public int GetRowCountForGeneralDailyProcessFactory(GridSearchFields gridSearchRequest)
        {
            List<SearchField> searchFields = ComposeSearchCriteriaforFactoryDailyWorkStatus(gridSearchRequest);
            int count = _generalDailyprocessRepository.GetRowCountForGeneralDailyProcessFactory(searchFields);
            return count;
        }

        public int GetRowCountForGeneralProcessDailyAnalysis(GridSearchFields gridSearchRequest)
        {
            List<SearchField> searchFields = ComposeSearchCriteriaforDailyProcess(gridSearchRequest);
            int count = _generalDailyprocessRepository.GetRowCountForGeneralProcessDailyAnalysis(searchFields);
            return count;
        }
        public int ValidateGeneralProcess(short PROCESS_KUBUN_CD)
        {
            int count = _generalDailyprocessRepository.ValidateGeneralProcess(PROCESS_KUBUN_CD);
            return count;
        }
        public GeneralDailyProcessDetails GetChubbanDetailsForWeight(string PP_CHUUBAN)
        {

            var obj = _generalDailyprocessRepository.GetChubbanDetailsForWeight(PP_CHUUBAN);

            return obj;
        }
        public int GetNextThabaForSpecialChuubanNo(string orderNo)
        {

            var obj = _generalDailyprocessRepository.GetNextThabaForSpecialChuubanNo(orderNo);

            return obj;
        }
        public GeneralDailyProcessDetails GetGeneralProcessDataForCancel(GeneralDailyProcessDetails processDetails)
        {

            var obj = _generalDailyprocessRepository.GetGeneralProcessDataForCancel(processDetails);

            return obj;
        }
        public GeneralDailyProcessDetails GetProcessPlanData(GeneralDailyProcessDetails processDetails)
        {

            var obj = _generalDailyprocessRepository.GetProcessPlanData(processDetails);

            return obj;
        }
        public List<NameValue> GetMachineTypeList(int machineTypeCd)
        {
            var machineNameList = _generalDailyprocessRepository.GetMachineTypeList(machineTypeCd);
            return machineNameList;
        }
        public GeneralDailyProcessDetails ValidateChubbanForProcessStart(string PP_CHUUBAN)
        {
            var obj = _generalDailyprocessRepository.ValidateChubbanForProcessStart(PP_CHUUBAN);
            return obj;
        }
        public string ValidateChubbanForProcessEnd(string PP_CHUUBAN)
        {
            var obj = _generalDailyprocessRepository.ValidateChubbanForProcessEnd(PP_CHUUBAN);
            return obj;
        }
        public string CheckExistingDataValidation(GeneralDailyProcessDetails processDetails)
        {

            var obj = _generalDailyprocessRepository.CheckExistingDataValidation(processDetails);

            return obj;
        }
    }

}
