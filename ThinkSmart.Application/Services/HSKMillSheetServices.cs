using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Transactions;
using TSWebApplication.Application.IServices;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Infrastructure.IRepositories;
using TSWebApplication.Infrastructure.Repositories;
using static TSWebApplication.Domain.Utility.Enums;

namespace TSWebApplication.Application.Services
{
    public class HSKMillSheetServices : IHSKMillSheetServices
    {
        private readonly IHSKMillSheetRepository _hskmillsheetRepository;
        private readonly IAllCommonRepository _CommonRepository;
        private readonly IUserLoginRepository _UserLoginRepository;
        private readonly LocalizationServices _localization;
        private readonly IAllCommonService _allCommonService;
        public HSKMillSheetServices(IHSKMillSheetRepository hskmillsheetRepository, IAllCommonRepository allCommonRepository, IUserLoginRepository userLoginRepository, LocalizationServices localization, IAllCommonService allCommonService)
        {
            _hskmillsheetRepository = hskmillsheetRepository;
            _CommonRepository = allCommonRepository;
            _UserLoginRepository = userLoginRepository;
            _localization = localization;
            _allCommonService = allCommonService;
        }
        public HSKMillSheetDetails GetHSKMillSheetDetailsByChuuban(string PP_CHUUBAN, short? PP_GYOUBAN, string cultureName)
        {

            if (cultureName == "en")
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
            }
            else
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("ja-JP");
            }

            var obj = _hskmillsheetRepository.GetHSKMillSheetDetailsByChuuban(PP_CHUUBAN, PP_GYOUBAN);

            //if (obj != null)
            //{
            //    if (obj.PP_STD_DEL_PERIOD > 0)
            //    {
            //        obj.PP_STD_DEL_PERIOD_STRING = obj.PP_STD_DEL_PERIOD + " " + _localization.GetLocalizedHtmlString("lbl_Day");
            //    }

            //    if (obj.PP_UNIT > 0)
            //    {
            //        obj.PP_NO_OF_UNITS_DECODE = Convert.ToDecimal(obj.PP_NO_OF_UNITS).ToString("0.####") + UIUtils.GetProductUnitKubunValue(Convert.ToString(obj.PP_UNIT));
            //    }

            //    //if (obj.DW_START_OPERATOR_NAME != null)    // Stat Data :  Bind DW No Of Unit
            //    //{
            //    //    obj.DW_NO_OF_COILS = Convert.ToDecimal(obj.PP_NO_OF_UNITS).ToString("0.####") + UIUtils.Instance.GetProductUnitKubunValue(Convert.ToString(obj.PP_UNIT));
            //    //}


            //    if (obj.MAGARI_KUBUN > 0)
            //    {

            //        obj.PP_MAGARI_BEND_TOL = Utils.GetMagariBendTolSpan(UIUtils.Instance.GetMagariKubunValue(Convert.ToString(obj.MAGARI_KUBUN)), Convert.ToInt32(obj.MAGARI_BEND_TOL), Convert.ToInt32(obj.MAGARI_SPAN));  //Convert.ToDecimal(obj.PP_NO_OF_UNITS).ToString("0.####") + UIUtils.Instance.GetProductUnitKubunValue(Convert.ToString(obj.PP_UNIT));

            //    }

            //    if (obj.PP_COLOR_ID > 0)
            //    {
            //        obj.PP_COLOR = Utils.GetColorName(Convert.ToInt16(obj.PP_COLOR_ID));
            //    }

            //    if (!string.IsNullOrEmpty(obj.PP_COIL_DIA_WINDING))
            //    {
            //        obj.PP_COIL_DIA_WINDING = UIUtils.GetCoilDiaWindingKubunValue(Convert.ToString(obj.PP_COIL_DIA_WINDING));
            //    }

            //    var messageList = _CommonRepository.GetMessageDetailsByProcessPlanCD(Convert.ToInt64(obj.PP_PROCESS_PLAN_CD), true);
            //    obj.DW_MESSAGE_LIST = messageList;

            //    obj.PP_ELONGATION = Helper.GetProductPatternFormat(obj.ELONGATION_PATTERN1, obj.ELONGATION_PATTERN1_MIN_MAX, Convert.ToDecimal(obj.ELONGATION_PATTERN2_MIN), Convert.ToDecimal(obj.ELONGATION_PATTERN2_MAX), obj.ELONGATION_PATTERN3_ACTUAL);
            //    obj.PP_TENSILE_STRENGTH = Helper.GetProductPatternFormat(obj.TENSILE_STRN_PATTERN1, obj.TENSILE_STRN_PATTERN1_MIN_MAX, obj.TENSILE_STRN_PATTERN2_MIN, obj.TENSILE_STRN_PATTERN2_MAX, obj.TENSILE_STRN_PATTERN3_ACTUAL);
            //    obj.PP_HARDNESS = Helper.GetHardnessProductPatternFormat(obj.HARDNESS_PATTERN1_CODE, obj.HARDNESS_PATTERN1, obj.HARDNESS_PATTERN1_MIN_MAX, obj.HARDNESS_PATTERN2_CODE, obj.HARDNESS_PATTERN2_MIN, obj.HARDNESS_PATTERN2_MAX, obj.HARDNESS_PATTERN3_ACTUAL);
            //    obj.PP_DECARB = Helper.GetDecarbProductPatternFormat(obj.DECARB_1, obj.DECARB_VAL_1, obj.DECARB_2, obj.DECARB_VAL_2, obj.DECARB_3, obj.DECARB_VAL_3);
            //    obj.PP_YEILD = Helper.GetProductPatternFormat(obj.YEILD_PATTERN1, obj.YEILD_PATTERN1_MIN_MAX, obj.YEILD_PATTERN2_MIN, obj.YEILD_PATTERN2_MAX, obj.YEILD_PATTERN3_ACTUAL);
            //    obj.PP_PRESS = Helper.GetProductPatternFormat(obj.PRESS_PATTERN1, obj.PRESS_PATTERN1_MIN_MAX, obj.PRESS_PATTERN2_MIN, obj.PRESS_PATTERN2_MAX, obj.PRESS_PATTERN3_ACTUAL);

            //}

            return obj;
        }


        public long GetProductCD(string ProdSpecNo1, string ProdSpecNo2, string ProdVer)
        {
            return _hskmillsheetRepository.GetProductCD(ProdSpecNo1, ProdSpecNo2, ProdVer);
        }

        public MakerMillSheetDetails MakerMillSheetChemicalCompositionPopup(string FILENAME)
        {

            var obj = _hskmillsheetRepository.GetMakerMillSheetChemicalCompositionPopup(FILENAME);
            return obj;
        }

        public List<ChemicalCompositionDetails> GetHSKChemicalCompositionDetails(string custName, string prodNameCust)
        {

            var ChemicalCompositionList = _hskmillsheetRepository.GetHSKChemicalCompositionDetails(custName, prodNameCust);
            return ChemicalCompositionList;
        }

        public List<MakerMillSheetDetails> GetHSKChemicalCompositionAandMakerDetails(string custName, string prodNameCust, string makerMillsheetCD)
        {

            List<ChemicalCompositionDetails> ChemicalCompositionList = new List<ChemicalCompositionDetails>();
            List<MakerMillSheetDetails> MakerChemicalCompositionList = new List<MakerMillSheetDetails>();
            //return _hskmillsheetRepository.GetHSKChemicalCompositionDetails(custName, prodNameCust);
            ChemicalCompositionList = _hskmillsheetRepository.GetHSKChemicalCompositionDetails(custName, prodNameCust);
            MakerChemicalCompositionList = _hskmillsheetRepository.GetHSKChemicalCompositionAandMakerDetails(custName, prodNameCust, makerMillsheetCD);

            //   var List = ChemicalCompositionList.Concat(MakerChemicalCompositionList).ToList();
            //   MakerChemicalCompositionList.AddRange(ChemicalCompositionList);

            var mergedList = ChemicalCompositionList
           .Select(x => new MakerMillSheetDetails
           {
               CCNAME1 = x.ELEMENT_SYMBOL_1,
               CCNAME2 = x.ELEMENT_SYMBOL_2,
               CCNAME3 = x.ELEMENT_SYMBOL_3,
               CCNAME4 = x.ELEMENT_SYMBOL_4,
               CCNAME5 = x.ELEMENT_SYMBOL_5,
               CCNAME6 = x.ELEMENT_SYMBOL_6,
               CCNAME7 = x.ELEMENT_SYMBOL_7,
               CCNAME8 = x.ELEMENT_SYMBOL_8,
               CCNAME9 = x.ELEMENT_SYMBOL_9,
               CCNAME10 = x.ELEMENT_SYMBOL_10,
               CCNAME11 = x.ELEMENT_SYMBOL_11,
               CCNAME12 = x.ELEMENT_SYMBOL_12,
               CCNAME13 = x.ELEMENT_SYMBOL_13,
               CCNAME14 = x.ELEMENT_SYMBOL_14,
               CCNAME15 = x.ELEMENT_SYMBOL_15,
               CCRANGE1 = x.MAGNIFICATION_1,
               CCRANGE2 = x.MAGNIFICATION_2,
               CCRANGE3 = x.MAGNIFICATION_3,
               CCRANGE4 = x.MAGNIFICATION_4,
               CCRANGE5 = x.MAGNIFICATION_5,
               CCRANGE6 = x.MAGNIFICATION_6,
               CCRANGE7 = x.MAGNIFICATION_7,
               CCRANGE8 = x.MAGNIFICATION_8,
               CCRANGE9 = x.MAGNIFICATION_9,
               CCRANGE10 = x.MAGNIFICATION_10,
               CCRANGE11 = x.MAGNIFICATION_11,
               CCRANGE12 = x.MAGNIFICATION_12,
               CCRANGE13 = x.MAGNIFICATION_13,
               CCRANGE14 = x.MAGNIFICATION_14,
               CCRANGE15 = x.MAGNIFICATION_15,
               CCMIN1 = Convert.ToInt32(x.ES_MIN_1),
               CCMIN2 = Convert.ToInt32(x.ES_MIN_2),
               CCMIN3 = Convert.ToInt32(x.ES_MIN_3),
               CCMIN4 = Convert.ToInt32(x.ES_MIN_4),
               CCMIN5 = Convert.ToInt32(x.ES_MIN_5),
               CCMIN6 = Convert.ToInt32(x.ES_MIN_6),
               CCMIN7 = Convert.ToInt32(x.ES_MIN_7),
               CCMIN8 = Convert.ToInt32(x.ES_MIN_8),
               CCMIN9 = Convert.ToInt32(x.ES_MIN_9),
               CCMIN10 = Convert.ToInt32(x.ES_MIN_10),
               CCMIN11 = Convert.ToInt32(x.ES_MIN_11),
               CCMIN12 = Convert.ToInt32(x.ES_MIN_12),
               CCMIN13 = Convert.ToInt32(x.ES_MIN_13),
               CCMIN14 = Convert.ToInt32(x.ES_MIN_14),
               CCMIN15 = Convert.ToInt32(x.ES_MIN_15),

               CCMAX1 = Convert.ToInt32(x.ES_MAX_1),
               CCMAX2 = Convert.ToInt32(x.ES_MAX_2),
               CCMAX3 = Convert.ToInt32(x.ES_MAX_3),
               CCMAX4 = Convert.ToInt32(x.ES_MAX_4),
               CCMAX5 = Convert.ToInt32(x.ES_MAX_5),
               CCMAX6 = Convert.ToInt32(x.ES_MAX_6),
               CCMAX7 = Convert.ToInt32(x.ES_MAX_7),
               CCMAX8 = Convert.ToInt32(x.ES_MAX_8),
               CCMAX9 = Convert.ToInt32(x.ES_MAX_9),
               CCMAX10 = Convert.ToInt32(x.ES_MAX_10),
               CCMAX11 = Convert.ToInt32(x.ES_MAX_11),
               CCMAX12 = Convert.ToInt32(x.ES_MAX_12),
               CCMAX13 = Convert.ToInt32(x.ES_MAX_13),
               CCMAX14 = Convert.ToInt32(x.ES_MAX_14),
               CCMAX15 = Convert.ToInt32(x.ES_MAX_15),

           })
           .Concat(MakerChemicalCompositionList)
            //.GroupBy(x => x.ElementName)
            //.Select(g => g.First())
            .ToList();



            return mergedList;
        }


        public List<HSKMillSheetDetails> GetHSKMillSheetList(GridSearchFields gridSearchRequest)
        {
            short locationCD = Convert.ToInt16(_CommonRepository.GetUserLocationById(Convert.ToInt32(gridSearchRequest.USER_ID)));
            List<HSKMillSheetDetails> hskmillsheetRepository = new List<HSKMillSheetDetails>();
            List<SearchField> searchFields = ComposeSearchCriteria(gridSearchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);
            hskmillsheetRepository = _hskmillsheetRepository.GetHSKMillSheetList(searchFields, selectedSortCriteria, locationCD);
            return hskmillsheetRepository;
        }

        private List<SearchField> ComposeSearchCriteria(GridSearchFields gridSearchRequest)
        {
            var searchFields = new List<SearchField>();

            if (!string.IsNullOrEmpty(gridSearchRequest.CHUUBAN))
            {
                string chuuban = string.Empty;

                var searchField = new SearchField
                {
                    FieldName = "CONCAT(PP.CHUUBAN,CASE WHEN LENGTH(TRIM(PP.GYOUBAN))=1 THEN '0'||PP.GYOUBAN END)",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.CHUUBAN

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
            if (!string.IsNullOrEmpty(gridSearchRequest.RECEIVER_NAME))
            {
                var searchField = new SearchField
                {
                    FieldName = "HMS.RECEIVER_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.RECEIVER_NAME
                };

                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_NAME_CUST))
            {
                var searchField = new SearchField
                {
                    FieldName = "HMS.PROD_NAME_CUST",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.PROD_NAME_CUST
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.HT_CONDITION))
            {
                var searchField = new SearchField
                {
                    FieldName = "CND.CONDITION_DESCRIPTION",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.HT_CONDITION
                };
                searchFields.Add(searchField);
            }


            if (!string.IsNullOrEmpty(gridSearchRequest.SHAPE))
            {
                var searchField = new SearchField
                {
                    FieldName = "HMS.PROD_SHAPE",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.SHAPE
                };
                searchFields.Add(searchField);
            }

            if (gridSearchRequest.PROD_DIM_D_MAX == 0)
            {

                if (gridSearchRequest.PROD_DIM_D_MIN > 0)
                {
                    var searchField = new SearchField
                    {
                        FieldName = "HMS.PROD_DIM_D",
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
                    FieldName = "HMS.PROD_DIM_D",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.PROD_DIM_D_MIN),
                    SearchText2 = Convert.ToString(gridSearchRequest.PROD_DIM_D_MAX)
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.CHARGE))
            {
                var searchField = new SearchField
                {
                    FieldName = "HMS.CHARGE_NO",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.CHARGE.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }

            if (gridSearchRequest.PROD_LENGTH_MAX == 0)
            {
                if (gridSearchRequest.PROD_LENGTH_MIN > 0)
                {
                    var searchField = new SearchField
                    {
                        FieldName = "HMS.PROD_LENGTH",
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
                    FieldName = "HMS.PROD_LENGTH",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.PROD_LENGTH_MIN),
                    SearchText2 = Convert.ToString(gridSearchRequest.PROD_LENGTH_MAX)
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.MAKER_NAME))
            {
                var searchField = new SearchField
                {
                    FieldName = "M.MAKER_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.MAKER_NAME.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }

            return searchFields;
        }

        public List<HSKMillSheetDetails> GetHSKMillSheetHistoryPopUp(GridSearchFields gridSearchRequest)
        {
            short locationCD = Convert.ToInt16(_CommonRepository.GetUserLocationById(Convert.ToInt32(gridSearchRequest.USER_ID)));
            List<HSKMillSheetDetails> hskmillsheetRepository = new List<HSKMillSheetDetails>();
            List<SearchField> searchFields = ComposeSearchHistoryPopUpCriteria(gridSearchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);
            hskmillsheetRepository = _hskmillsheetRepository.GetHSKMillSheetHistoryPopUpList(searchFields, selectedSortCriteria, locationCD);
            return hskmillsheetRepository;
        }

        private List<SearchField> ComposeSearchHistoryPopUpCriteria(GridSearchFields gridSearchRequest)
        {
            var searchFields = new List<SearchField>();

            if (!string.IsNullOrEmpty(gridSearchRequest.CHUUBAN))
            {
                string chuuban = string.Empty;

                var searchField = new SearchField
                {
                    FieldName = "CONCAT(PP.CHUUBAN,CASE WHEN LENGTH(TRIM(PP.GYOUBAN))=1 THEN '0'||PP.GYOUBAN END)",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.CHUUBAN

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


            if (!string.IsNullOrEmpty(gridSearchRequest.DELIVERY_DATE_MIN))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.DELIVERY_DATE",
                    SearchType = SearchType.Date,
                    SearchText1 = gridSearchRequest.DELIVERY_DATE_MIN,
                    SearchText2 = gridSearchRequest.DELIVERY_DATE_MAX
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_SHAPE))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_SHAPE",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.SHAPE
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



            return searchFields;
        }

        public int GetProcessPlanCD(string chuuban, short gyouban)
        {
            return _hskmillsheetRepository.GetProcessPlanCD(chuuban, gyouban);
        }

        public string GetCondDescFromProdCD(long Prod_CD)
        {
            return _hskmillsheetRepository.GetCondDescFromProdCD(Prod_CD);
        }
        public long GetProdOrderNoInspectionHardness(string chuuban, short gyouban, string charge, string matDimD, string matNameMaker)
        {
            return _hskmillsheetRepository.GetProdOrderNoInspectionHardness(chuuban, gyouban, charge, matDimD, matNameMaker);
        }

        public long GetProdOrderNoInspectionTensile(string chuuban, short gyouban, string charge, string matDimD, string matNameMaker)
        {
            return _hskmillsheetRepository.GetProdOrderNoInspectionTensile(chuuban, gyouban, charge, matDimD, matNameMaker);
        }

        public List<HSKMillSheetDetails> GetHSKMillSheetPostHTInspectionDetails(string processPlanCD)
        {
            return _hskmillsheetRepository.GetHSKMillSheetPostHTInspectionDetails(processPlanCD);
        }
        public HSKMillSheetDetails GetHSKMillSheetProcessPlanCD(string processPlanCD)
        {
            return _hskmillsheetRepository.GetHSKMillSheetProcessPlanCD(processPlanCD);
        }

        public List<HSKMillSheetDetails> GetPostHTInspectionDetailsByPostHTCD(string postHTCD)
        {
            return _hskmillsheetRepository.GetPostHTInspectionDetailsByPostHTCD(postHTCD);
        }
        public List<HSKMillSheetDetails> GetHSKMillSheetFinalInspectionDetails(string processPlanCD)
        {
            return _hskmillsheetRepository.GetHSKMillSheetFinalInspectionDetails(processPlanCD);
        }
        public List<HSKMillSheetDetails> GetFinalInspectionDetailsByPostFinalInspCD(string finalInspCD)
        {
            return _hskmillsheetRepository.GetFinalInspectionDetailsByPostFinalInspCD(finalInspCD);
        }

        public List<HSKMillSheetDetails> LoadHardnessPattern1DetailNoData(long prodOrderNo, int DetailNo)
        {
            List<HSKMillSheetDetails> result = new List<HSKMillSheetDetails>();

            result = _hskmillsheetRepository.LoadHardnessPattern1DetailNoData(prodOrderNo, DetailNo);
            for (int i = 0; i < result.Count; i++)
            {
                //result[i].CH_DECISION_STRING = UIUtils.GetResultKubunValue(Convert.ToString(result[i].CH_DECISION));
            }
            return result;


            //return _hskmillsheetRepository.LoadHardnessPattern1DetailNoData(prodOrderNo, DetailNo);
        }

        public List<HSKMillSheetDetails> HSKMillSheetGridSearchChuubanLiveCheck(string chuuban, string gyouban)
        {
            return _hskmillsheetRepository.HSKMillSheetGridSearchChuubanLiveCheck(chuuban, gyouban);
        }
        public List<HSKMillSheetDetails> GetProductDetails(long Prod_CD)
        {
            return _hskmillsheetRepository.GetProductDetails(Prod_CD);
        }

        public List<HSKMillSheetDetails> GetProductMasterDefaultValue(long Prod_CD)
        {
            return _hskmillsheetRepository.GetProductMasterDefaultValue(Prod_CD);
        }

        public List<MakerMillSheetDetails> GetMakerMillsheetDetails(GridSearchFields request)
        {

            List<ProcessPlanForYieldSearchDetails> yieldSearchDetails = new List<ProcessPlanForYieldSearchDetails>();
            List<SearchField> searchFields = ComposeMakerMillSheetSearchCriteria(request);
            string selectedSortCriteria = Helper.ParseSortCriteria(request.SORT_FIELDS);
            if (request.CULTURE_NAME == "en")
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
            }
            else
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("ja-JP");
            }
            
            return _hskmillsheetRepository.GetMakerMillsheetDetails(searchFields, selectedSortCriteria);


        }

        private List<SearchField> ComposeMakerMillSheetSearchCriteria(GridSearchFields request)
        {
            var searchFields = new List<SearchField>();

          
            if (!string.IsNullOrEmpty(request.CHARGE))
            {
                var searchField = new SearchField
                {
                    FieldName = "MM.CHARGE",
                    SearchType = SearchType.Text,
                    SearchText1 = request.CHARGE
                };

                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(request.MATL_NAME_MAKER))
            {
                var searchField = new SearchField
                {
                    FieldName = "MM.MATL_NAME_MAKER",
                    SearchType = SearchType.Text,
                    SearchText1 = request.MATL_NAME_MAKER
                };

                searchFields.Add(searchField);
            }
                    
           
            if (request.DIM_D2 == 0)
            {

                if (request.DIM_D1 > 0)
                {
                    var searchField = new SearchField
                    {
                        FieldName = "MM.DIM_D",
                        SearchType = SearchType.Number,
                        SearchText1 = Convert.ToString(request.DIM_D1)
                    };
                    searchFields.Add(searchField);
                }
            }
            else if (request.DIM_D1 > 0 || request.DIM_D2 > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "MM.DIM_D",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(request.DIM_D1),
                    SearchText2 = Convert.ToString(request.DIM_D2)
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(request.CREATED_DATE1))
            {
                var searchField = new SearchField
                {
                    FieldName = "TO_DATE(TO_CHAR(MM.CREATED_DATE))",
                    SearchType = SearchType.Date,
                    SearchText1 = request.CREATED_DATE1,
                    SearchText2 = request.CREATED_DATE2
                };
                searchFields.Add(searchField);
            }

            return searchFields;
        }



        public void SaveHSKMillSheetDetails(HSKMillSheetDetails request)
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                if (request.USER_ID != null && request.USER_ID > 0)
                {
                   // request.INPUT_LOCATION_CD = _UserLoginRepository.GetUserLocationById(request.USER_ID);
                    request.LOCATION_CD = request.INPUT_LOCATION_CD;
                }
                //request.PROCESS_KUBUN_CD = Convert.ToInt16(UIUtils.GetProcessKubun(_localization.GetLocalizedHtmlString("lbl_ProcessMechanical_Inspection_Hardness")));
                //if (request.MSG_CD == Constants.NullValue.INT && !string.IsNullOrEmpty(request.REMARKS))
                //{
                //    MessageDetails messageDetails = new MessageDetails();
                //    messageDetails.PROCESS_PLAN_CD = Convert.ToInt64(request.PROCESS_PLAN_CD);

                //    messageDetails.PROCESS_KUBUN_ID = Convert.ToInt16(UIUtils.GetProcessKubun(_localization.GetLocalizedHtmlString("lbl_ProcessMechanical_Inspection_Hardness")));
                //    messageDetails.OPERATOR_CD = Convert.ToInt64(request.HD_TEST_OPERATOR_CD);
                //    messageDetails.MSG_CONTENTS = request.REMARKS;
                //    messageDetails.USER_ID = Convert.ToInt32(request.USER_ID);
                //    //     messageDetails.LOCATION_CD = Convert.ToInt32(request.LOCATION_CD);
                //    messageDetails.INPUT_LOCATION_CD = Convert.ToInt32(request.INPUT_LOCATION_CD);


                //    messageDetails = _AllCommonRepository.MessageDetailsInsert(messageDetails);

                //    if (messageDetails != null)
                //    {
                //        request.MSG_CD = Convert.ToInt32(messageDetails.MSG_CD);
                //    }


                //}
                _hskmillsheetRepository.SaveHSKMillSheetDetails(request);

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
    }
}
