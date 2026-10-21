using MailKit;
using Org.BouncyCastle.Asn1.Ocsp;
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
using System.Linq;
using static TSWebApplication.Domain.Utility.Constants;


namespace TSWebApplication.Application.Services
{
    public class WIPTensouService : IWIPTensouService
    {
        private readonly IWIPTensouRepository _WIPTensouRepository;

        private bool isDefaultIncluded;

        private readonly IUserLoginRepository _userLogin;
        private readonly IAllCommonRepository _AllCommonRepository;
        private readonly LocalizationServices _localization;

        public WIPTensouService(IWIPTensouRepository WIPTensouRepository, IUserLoginRepository userLogin, IAllCommonRepository AllCommonRepository, LocalizationServices localization)
        {
            _userLogin = userLogin;
            _WIPTensouRepository = WIPTensouRepository;
            _AllCommonRepository = AllCommonRepository;
            _localization = localization;

            // _localization = localization;
            // _hikinukibartobarService = hikinukibartobarService;
        }

        public WIPTensouDetails GetTensouDetailsByChuubanGyouban(string nifuda, string culture)
        {
            var obj = _WIPTensouRepository.GetTensouDetailsByChuubanGyouban(nifuda);
            if (culture == "en")
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
            }
            else
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("ja-JP");
            }
            if (obj != null)
            {
                if (obj.PP_STD_DEL_PERIOD > 0)
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

                if (obj.PP_COLOR_ID != null)
                {
                    obj.PP_COLOR = obj.PP_COLOR_ID;

                }

                if (obj.COIL_DIA_WINDING != null)
                {
                    // obj.PP_COIL_DIA_WINDING = obj.PP_COIL_DIA_WINDING;
                    obj.PP_COIL_DIA_WINDING = UIUtils.GetCoilDiaWindingKubunValue(Convert.ToString(obj.COIL_DIA_WINDING)); //  Utils.GetCoilDiaWindingValue(Convert.ToInt16(_stamppingDetails.PP_COIL_DIA_WINDING));
                }
                if (obj.DECISION != 0)
                {
                    obj.DECISION_STRING = UIUtils.GetResultKubunValue(Convert.ToString(obj.DECISION));
                }

                if (obj.PP_UNIT_WT_KUBUN > 0)
                {
                    obj.PP_UNIT_WT_KUBUN_STRING = Utils.GetUnitWeigth(Convert.ToInt32(obj.PP_UNIT_WT_KUBUN), Convert.ToInt32(obj.PP_UNIT_WT), Convert.ToInt32(obj.UNIT_WT_MIN), Convert.ToInt32(obj.UNIT_WT_MAX));

                    if (obj.PP_UNIT_WT_KUBUN == 3)
                    {
                        obj.PP_UNIT_WT_KUBUN_STRING = Convert.ToString(obj.PP_UNIT_WT) + _localization.GetLocalizedHtmlString("lbl_Hone");
                    }


                }

                obj.DECISION_OK = UIUtils.GetResultKubunKey(_localization.GetLocalizedHtmlString("lbl_OK"));
                var messageList = _AllCommonRepository.GetMessageDetailsByProcessPlanCD(Convert.ToInt64(obj.PP_PROCESS_PLAN_CD), true);
                obj.DW_MESSAGE_LIST = messageList;
            }

            return obj;

        }

        public List<NameValue> GetNextProcessList()
        {
            var _nextProcessList = UIUtils.GetNextProcesKubun();
            return _nextProcessList;
        }

        public List<NameValue> GetProductUnitKubun()
        {
            var _prod_kubun = UIUtils.Instance.GetProductUnitKubun();
            return _prod_kubun;
        }

        public List<NameValue> GetTensouLocation(int USER_ID)
        {
            //    int userID = (int)Convert.ToInt64(USER_ID);
            var result = _userLogin.GetUserLocationNameById(USER_ID);
            return result;

        }

        public List<WIPTensouDetails> SaveWIPTensouDetails(List<WIPTensouDetails> lstGridData)
        {
            List<WIPTensouDetails> obj = new List<WIPTensouDetails>();
            WipStatusDetails wipDetails = new WipStatusDetails();
            var groupNo = _WIPTensouRepository.GetMaxGroupNo();
            int i = 0;

            foreach (var item in lstGridData)
            {
                var kubunCD = Convert.ToInt32(UIUtils.GetProcessKubun(_localization.GetLocalizedHtmlString("lbl_ProcessWIP_Forwarding")));
                if (i < lstGridData.Count)
                {
                    if (item.USER_ID != null && item.USER_ID > 0)
                    {
                        item.INPUT_LOCATION_CD = _userLogin.GetUserLocationById(item.USER_ID.Value);
                        item.LOCATION_CD = item.INPUT_LOCATION_CD;
                    }

                    if (item.USER_ID != null && item.USER_ID > 0)
                    {
                        item.INPUT_LOCATION_CD = _userLogin.GetUserLocationById(item.USER_ID.Value);
                        item.LOCATION_CD = item.INPUT_LOCATION_CD;
                    }

                    if (item.PP_UNIT != null)
                    {
                        item.PP_NO_OF_UNITS_DECODE = Convert.ToDecimal(item.PP_NO_OF_UNITS).ToString("0.####") + UIUtils.GetProductUnitKubunValue(Convert.ToString(item.PP_UNIT));
                    }

                    //thaba null then save null value.
                    if (item.TABA_NO_2 == 0)
                    {
                        item.TABA_NO_2 = null;
                    }

                    if (item.TABA_NO_3 == 0)
                    {
                        item.TABA_NO_3 = null;
                    }
                    if (item.TABA_NO_4 == 0)
                    {
                        item.TABA_NO_4 = null;
                    }

                    using (TransactionScope transaction = new TransactionScope())
                    {
                        //message insert

                        if ((item.MSG_CD == null || item.MSG_CD == 0) && item.MSG_CONTENTS != null)
                        {

                            MessageDetails messageDetails = new MessageDetails();
                            messageDetails.PROCESS_PLAN_CD = Convert.ToInt64(item.PROCESS_PLAN_CD);
                            messageDetails.PROCESS_KUBUN_ID = kubunCD;
                            messageDetails.OPERATOR_CD = null;
                            messageDetails.MSG_CONTENTS = item.MSG_CONTENTS;
                            messageDetails.USER_ID = Convert.ToInt32(item.USER_ID);
                            messageDetails.INPUT_LOCATION_CD = item.INPUT_LOCATION_CD;

                            messageDetails = _AllCommonRepository.MessageDetailsInsert(messageDetails);

                            if (messageDetails != null)
                            {
                                item.MSG_CD = Convert.ToInt32(messageDetails.MSG_CD);
                            }
                        }

                        item.SERIAL_NUMBER = Convert.ToInt16(i + 1);
                        item.WIP_TRANSFER_GROUP_NO = groupNo + 1;
                        item.PROCESS_KUBUN_ID = kubunCD;
                        //WIP Tensou Insert
                        int TransferGroupNo = _WIPTensouRepository.SaveWIPTensouDetails(item);
                        obj = _WIPTensouRepository.GetTransferCD(TransferGroupNo);


                        //wipSatus Save
                        wipDetails = new WipStatusDetails();
                        if (item.NEXT_PROSESS != 999)
                        {
                            var now = DateTime.Now;
                            string time = now.ToString("HH:mm:ss");
                            string dateTime = item.TRANSFER_DATE_STRING + " " + time;

                            wipDetails.PROCESS_PLAN_CD = Convert.ToInt32(item.PROCESS_PLAN_CD);
                            wipDetails.LOCATION_CD = Convert.ToInt16(item.LOCATION_CD_TO);
                            if (item.MSG_CD == 0)
                            {
                                wipDetails.MSG_CD = null;
                            }
                            else
                            {
                                wipDetails.MSG_CD = item.MSG_CD;
                            }
                            wipDetails.FINISHED_BARS = item.NO_OF_UNITS;
                            wipDetails.UNIT = item.UNIT_TYPE;
                            wipDetails.FINISHED_QTY = Convert.ToDouble(item.QTY);

                            // DateTime Date = DateTime.Now; // Example input
                            wipDetails.PROCESS_START_DATE = Convert.ToDateTime(dateTime); //Convert.ToDateTime(item.TRANSFER_DATE_STRING);
                            wipDetails.PROCESS_END_DATE = Convert.ToDateTime(dateTime);//Convert.ToDateTime(item.TRANSFER_DATE_STRING);
                            wipDetails.PROCESS_KUBUN_CD = kubunCD;
                            wipDetails.PRE_PROSESS_DECISION = null;
                            wipDetails.NEXT_PROCESS = Convert.ToInt16(item.NEXT_PROSESS);
                            wipDetails.START_OPERATOR_CD = null;
                            wipDetails.END_OPERATOR_CD = null;
                            wipDetails.TABA_NO = Convert.ToInt16(item.TABA_NO_1);
                            wipDetails.NIFUDA_CD = null;

                        }

                        if (item.TABA_NO_1 != null && item.TABA_NO_2 != null || item.TABA_NO_3 != null || item.TABA_NO_4 != null)
                        {
                            wipDetails.TABA_GROUP_NO = Convert.ToInt16(item.TABA_NO_1);
                        }

                        if (item.TABA_NO_1 > 0)
                        {
                            _AllCommonRepository.DeleteWIPStatusData(wipDetails);
                        }

                        if (item.TABA_NO_2 > 0)
                        {

                            wipDetails.FINISHED_QTY = null;
                            wipDetails.FINISHED_BARS = null;
                            wipDetails.UNIT = null;
                            wipDetails.TABA_NO = Convert.ToInt16(item.TABA_NO_2);
                            _AllCommonRepository.DeleteWIPStatusData(wipDetails);
                        }
                        if (item.TABA_NO_3 > 0)
                        {

                            wipDetails.FINISHED_QTY = null;
                            wipDetails.FINISHED_BARS = null;
                            wipDetails.UNIT = null;
                            wipDetails.TABA_NO = Convert.ToInt16(item.TABA_NO_3);
                            _AllCommonRepository.DeleteWIPStatusData(wipDetails);
                        }
                        if (item.TABA_NO_4 > 0)
                        {
                            wipDetails.FINISHED_QTY = null;
                            wipDetails.FINISHED_BARS = null;
                            wipDetails.UNIT = null;
                            wipDetails.TABA_NO = Convert.ToInt16(item.TABA_NO_4);
                            _AllCommonRepository.DeleteWIPStatusData(wipDetails);
                        }
                        // }

                        // _AllCommonRepository.InsertWIPStatusData(wipDetails);
                        transaction.Complete();

                    }
                }
                i++;
            }

            return obj;
        }

        public List<WIPTensouDetails> GetTensouDataSearchList(GridSearchFields gridSearchRequest)
        {
            string Location_CD = Convert.ToString(_userLogin.GetUserLocationById(Convert.ToInt32(gridSearchRequest.USER_ID)));
            gridSearchRequest.LOCATION_CD = Convert.ToInt16(Location_CD);

            List<WIPTensouDetails> WIPTensouDetails = new List<WIPTensouDetails>();
            List<SearchField> searchFields = ComposeSearchCriteria(gridSearchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);
            WIPTensouDetails = _WIPTensouRepository.GetWIPTensouList(searchFields, selectedSortCriteria, Convert.ToInt32(gridSearchRequest.LOCATION_CD));
            return WIPTensouDetails;
        }

        private List<SearchField> ComposeSearchCriteria(GridSearchFields gridSearchRequest)
        {
            var searchFields = new List<SearchField>();

            //if (!string.IsNullOrEmpty(gridSearchRequest.CHUUBAN))
            //{
            //    string chuuban = string.Empty;
            //    if (gridSearchRequest.CHUUBAN.Length > 9)
            //    {
            //        chuuban = gridSearchRequest.CHUUBAN.Substring(0, 9);
            //    }
            //    else
            //    {
            //        chuuban = gridSearchRequest.CHUUBAN.Trim();
            //    }

            //    var searchField = new SearchField
            //    {
            //        FieldName = "PP.CHUUBAN",
            //        SearchType = SearchType.Text,

            //        SearchText1 = chuuban.Trim()

            //    };

            //    searchFields.Add(searchField);
            //}

            if (!string.IsNullOrEmpty(gridSearchRequest.CHUUBAN))
            {
                var searchField = new SearchField
                {
                    FieldName = "CONCAT(PP.CHUUBAN_YEAR, CONCAT(PP.CHUUBAN ,CASE WHEN LENGTH(TRIM(PP.GYOUBAN))=1 THEN '0'||PP.GYOUBAN END))",//"PP.CHUUBAN",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.CHUUBAN.Trim()
                };
                searchFields.Add(searchField);

            }

            if (!string.IsNullOrEmpty(gridSearchRequest.CUSTOMER_NAME))
            {
                var searchField = new SearchField
                {
                    FieldName = "CUS.SHORT_NAME",
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
                    FieldName = "MIN.INTERNAL_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.INTERNAL_NAME_STRING.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }

            //if (!string.IsNullOrEmpty(gridSearchRequest.PP_DIM_TOL_MIN))
            //{
            //    var searchField = new SearchField
            //    {
            //        FieldName = "PP.PROD_DIM_D",
            //        SearchType = SearchType.Number,
            //        SearchText1 = Convert.ToString(gridSearchRequest.PP_DIM_TOL_MIN),
            //        SearchText2 = Convert.ToString(gridSearchRequest.PP_DIM_TOL_MAX)

            //    };
            //    searchFields.Add(searchField);
            //}
            if (gridSearchRequest.PP_DIM_TOL_MAX == "")
            {
                if (gridSearchRequest.PP_DIM_TOL_MIN != "")
                {
                    var searchField = new SearchField
                    {
                        FieldName = "PP.PROD_DIM_D",
                        SearchType = SearchType.Number,
                        SearchText1 = Convert.ToString(gridSearchRequest.PP_DIM_TOL_MIN)
                    };
                    searchFields.Add(searchField);
                }
            }
            else if (gridSearchRequest.PP_DIM_TOL_MIN == "")
            {
                if (gridSearchRequest.PP_DIM_TOL_MAX != "")
                {
                    var searchField = new SearchField
                    {
                        FieldName = "PP.PROD_DIM_D",
                        SearchType = SearchType.Number,
                        SearchText1 = Convert.ToString(gridSearchRequest.PP_DIM_TOL_MAX)
                    };
                    searchFields.Add(searchField);
                }
            }
            else if (!string.IsNullOrEmpty(gridSearchRequest.PP_DIM_TOL_MIN) && !string.IsNullOrEmpty(gridSearchRequest.PP_DIM_TOL_MAX))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_DIM_D",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.PP_DIM_TOL_MIN),
                    SearchText2 = Convert.ToString(gridSearchRequest.PP_DIM_TOL_MAX)

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

            //if (!string.IsNullOrEmpty(gridSearchRequest.TRANSFER_DATE_START))
            //{
            //    var searchField = new SearchField
            //    {
            //        FieldName = "WIP.TRANSFER_DATE",
            //        SearchType = SearchType.Date,
            //        SearchText1 = gridSearchRequest.TRANSFER_DATE_START,
            //        SearchText2 = gridSearchRequest.TRANSFER_DATE_END
            //    };
            //    searchFields.Add(searchField);
            //}
            if (string.IsNullOrEmpty(gridSearchRequest.TRANSFER_DATE_END))
            {
                if (!string.IsNullOrEmpty(gridSearchRequest.TRANSFER_DATE_START))
                {
                    var searchField = new SearchField
                    {
                        FieldName = "WIP.TRANSFER_DATE",
                        SearchType = SearchType.Date,
                        SearchText1 = gridSearchRequest.TRANSFER_DATE_START
                    };
                    searchFields.Add(searchField);
                }
            }
            else if (string.IsNullOrEmpty(gridSearchRequest.TRANSFER_DATE_START))
            {
                if (!string.IsNullOrEmpty(gridSearchRequest.TRANSFER_DATE_END))
                {
                    var searchField = new SearchField
                    {
                        FieldName = "WIP.TRANSFER_DATE",
                        SearchType = SearchType.Date,
                        SearchText1 = gridSearchRequest.TRANSFER_DATE_END
                    };
                    searchFields.Add(searchField);
                }
            }
            else if (!string.IsNullOrEmpty(gridSearchRequest.TRANSFER_DATE_START) && !string.IsNullOrEmpty(gridSearchRequest.TRANSFER_DATE_END))
            {
                var searchField = new SearchField
                {
                    FieldName = "WIP.TRANSFER_DATE",
                    SearchType = SearchType.Date,
                    SearchText1 = gridSearchRequest.TRANSFER_DATE_START,
                    SearchText2 = gridSearchRequest.TRANSFER_DATE_END
                };
                searchFields.Add(searchField);
            }

            return searchFields;
        }


        public List<WIPTensouDetails> GetWIPTransferDetailsByTrasferCD(long WIP_TRANSFER_CD, string culture)
        {
            List<WIPTensouDetails> _WIPTensouDetails = new List<WIPTensouDetails>();
            int WIP_TRANSFER_GROUP_NO = _WIPTensouRepository.GetMaxGroupNo();
            _WIPTensouDetails = _WIPTensouRepository.GetWIPTransferDetailsByTrasferCD(Convert.ToInt32(WIP_TRANSFER_CD));
            if (culture == "en")
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
            }
            else
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("ja-JP");
            }
            for (int i = 0; i < _WIPTensouDetails.Count; i++)
            {
                if (_WIPTensouDetails[i].PP_UNIT != null)
                {
                    _WIPTensouDetails[i].PP_NO_OF_UNITS_DECODE = Convert.ToDecimal(_WIPTensouDetails[i].PP_NO_OF_UNITS).ToString("0.####") + UIUtils.GetProductUnitKubunValue(Convert.ToString(_WIPTensouDetails[i].PP_UNIT));
                }
                if (_WIPTensouDetails[i].PP_STD_DEL_PERIOD > 0)
                {
                    _WIPTensouDetails[i].PP_STD_DEL_PERIOD_STRING = _WIPTensouDetails[i].PP_STD_DEL_PERIOD + " " + _localization.GetLocalizedHtmlString("lbl_Day");
                }

                if (_WIPTensouDetails[i].MAGARI_KUBUN != null)
                {
                    _WIPTensouDetails[i].PP_MAGARI_BEND_TOL = Utils.GetMagariBendTolSpan(UIUtils.Instance.GetMagariKubunValue(Convert.ToString(_WIPTensouDetails[i].MAGARI_KUBUN)), Convert.ToDecimal(_WIPTensouDetails[i].MAGARI_BEND_TOL), Convert.ToInt32(_WIPTensouDetails[i].MAGARI_SPAN));  //Convert.ToDecimal(obj.PP_NO_OF_UNITS).ToString("0.####") + UIUtils.Instance.GetProductUnitKubunValue(Convert.ToString(obj.PP_UNIT));
                }

                if (_WIPTensouDetails[i].PP_COLOR_ID != null)
                {
                    _WIPTensouDetails[i].PP_COLOR = _WIPTensouDetails[i].PP_COLOR_ID;

                }

                if (_WIPTensouDetails[i].COIL_DIA_WINDING != null)
                {
                    _WIPTensouDetails[i].PP_COIL_DIA_WINDING = UIUtils.GetCoilDiaWindingKubunValue(Convert.ToString(_WIPTensouDetails[i].COIL_DIA_WINDING)); //  Utils.GetCoilDiaWindingValue(Convert.ToInt16(_stamppingDetails.PP_COIL_DIA_WINDING));
                }

                if (_WIPTensouDetails[i].MSG_CD != null)
                {
                    _WIPTensouDetails[i].MSG_CONTENTS = _WIPTensouRepository.GetMessageFromMsgID(Convert.ToInt32(_WIPTensouDetails[i].MSG_CD));
                }
                else
                {
                    _WIPTensouDetails[i].MSG_CONTENTS = null;
                }

            }

            //int msgCD = _WIPTensouDetails.MSG_CD;
            return _WIPTensouDetails;
        }

        public List<WIPTensouDetails> IsAcceptanceCompleted(long WIP_TRANSFER_CD, string culture)
        {
            List<WIPTensouDetails> _WIPTensouDetails = new List<WIPTensouDetails>();
            _WIPTensouDetails = _WIPTensouRepository.IsAcceptanceCompleted(Convert.ToInt32(WIP_TRANSFER_CD));
            if (culture == "en")
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
            }
            else
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("ja-JP");
            }
            return _WIPTensouDetails;
        }

        public List<WIPTensouDetails> GetDataOnGridView(GridSearchFields gridSearchRequest)
        {
            string Location_CD = Convert.ToString(_userLogin.GetUserLocationById(Convert.ToInt32(gridSearchRequest.USER_ID)));
            gridSearchRequest.LOCATION_CD = Convert.ToInt16(Location_CD);

            List<WIPTensouDetails> result = new List<WIPTensouDetails>();
            List<SearchField> searchFields = ComposeSearchCriteria(gridSearchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);
            result = _WIPTensouRepository.GetDataOnGridView(searchFields, selectedSortCriteria, Convert.ToInt32(gridSearchRequest.LOCATION_CD));
            //foreach (WIPTensouDetails record in result)
            //{
            //    record.DW_DECISION_STRING = UIUtils.GetResultKubunValue(Convert.ToString(record.DW_DECISION));
            //}

            return result;
        }

        //delete
        public int WIPTensouDetailsDelete(List<WIPTensouDetails> request)
        {
            int i = 1;
            WIPTensouDetails obj = new WIPTensouDetails();
            WipStatusDetails wipDetails = new WipStatusDetails();
            wipDetails = new WipStatusDetails();
           
            foreach (var item in request)
            {
                using (TransactionScope transaction = new TransactionScope())
                {
                    if (item.WIP_TRANSFER_GROUP_NO >= 0)
                    {
                        i = _WIPTensouRepository.WIPTensouDetailsDelete(item.WIP_TRANSFER_GROUP_NO);
                    }
                    if (item.MSG_CD == null || item.MSG_CD == 0 && item.MSG_CONTENTS != null)
                    {
                        item.MSG_CD = _WIPTensouRepository.GetMsgCdByMsgContent(item.MSG_CONTENTS);
                    }
                    else
                    {
                        if (item.MSG_CD > 0)
                        {
                            _AllCommonRepository.MessageDetailsDelete(Convert.ToInt64(item.MSG_CD));
                        }
                    }

                    wipDetails.PROCESS_PLAN_CD = item.PROCESS_PLAN_CD;
                   
                    if (item.TABA_NO_1 > 0)
                    {
                        wipDetails.TABA_NO = Convert.ToInt16(item.TABA_NO_1);
                        _AllCommonRepository.DeleteWIPStatusData(wipDetails);
                    }
                    if (item.TABA_NO_2 > 0)
                    {
                        wipDetails.TABA_NO = Convert.ToInt16(item.TABA_NO_2);
                        _AllCommonRepository.DeleteWIPStatusData(wipDetails);
                    }
                    if (item.TABA_NO_3 > 0)
                    {
                        wipDetails.TABA_NO = Convert.ToInt16(item.TABA_NO_3);
                        _AllCommonRepository.DeleteWIPStatusData(wipDetails);
                    }
                    if (item.TABA_NO_4 > 0)
                    {
                        wipDetails.TABA_NO = Convert.ToInt16(item.TABA_NO_4);
                        _AllCommonRepository.DeleteWIPStatusData(wipDetails);
                    }

                    transaction.Complete();
                }
            }

            return i;
        }

        //update
        public List<WIPTensouDetails> WIPTensouDetailsUpdate(List<WIPTensouDetails> lstGridData)
        {
            var kubunCD = Convert.ToInt32(UIUtils.GetProcessKubun(_localization.GetLocalizedHtmlString("lbl_ProcessWIP_Forwarding")));
            WIPTensouDetails result = new WIPTensouDetails();

            List<WIPTensouDetails> obj = new List<WIPTensouDetails>();

            var groupNo = _WIPTensouRepository.GetMaxGroupNo();
            int i = 0;

            foreach (var item in lstGridData)
            {
                if (i < lstGridData.Count)
                {
                    if (item.USER_ID != null && item.USER_ID > 0)
                    {
                        item.INPUT_LOCATION_CD = _userLogin.GetUserLocationById(item.USER_ID.Value);
                        item.LOCATION_CD = item.INPUT_LOCATION_CD;
                    }

                    if (item.USER_ID != null && item.USER_ID > 0)
                    {
                        item.INPUT_LOCATION_CD = _userLogin.GetUserLocationById(item.USER_ID.Value);
                        item.LOCATION_CD = item.INPUT_LOCATION_CD;
                    }
                    if (item.PP_UNIT != null)
                    {
                        item.PP_NO_OF_UNITS_DECODE = Convert.ToDecimal(item.PP_NO_OF_UNITS).ToString("0.####") + UIUtils.GetProductUnitKubunValue(Convert.ToString(item.PP_UNIT));
                    }

                    if (item.MSG_CD == 0 || item.MSG_CD == null && item.MSG_CONTENTS != null)
                    {
                        item.MSG_CD = _WIPTensouRepository.GetMsgCdByMsgContent(item.MSG_CONTENTS);
                    }

                    if (item.PROCESS_KUBUN_ID == null)
                    {
                        item.PROCESS_KUBUN_ID = kubunCD;
                    }
                    //message insert
                    using (TransactionScope transaction = new TransactionScope())
                    {
                        if (item.MSG_CD == 0 || item.MSG_CD == null)
                        {

                            MessageDetails messageDetails = new MessageDetails();
                            messageDetails.PROCESS_PLAN_CD = Convert.ToInt64(item.PROCESS_PLAN_CD);
                            messageDetails.PROCESS_KUBUN_ID = kubunCD;
                            messageDetails.OPERATOR_CD = null;
                            messageDetails.MSG_CONTENTS = item.MSG_CONTENTS;
                            messageDetails.USER_ID = Convert.ToInt32(item.USER_ID);
                            messageDetails.INPUT_LOCATION_CD = Convert.ToInt32(item.INPUT_LOCATION_CD);

                            if (messageDetails.MSG_CONTENTS != null)
                            {
                                messageDetails = _AllCommonRepository.MessageDetailsInsert(messageDetails);
                            }

                            if (messageDetails != null)
                            {
                                item.MSG_CD = Convert.ToInt32(messageDetails.MSG_CD);
                            }

                        }
                        else
                        {

                            MessageDetails messageDetails = new MessageDetails();
                            messageDetails.PROCESS_PLAN_CD = Convert.ToInt64(item.PROCESS_PLAN_CD);
                            messageDetails.PROCESS_KUBUN_ID = kubunCD;
                            messageDetails.OPERATOR_CD = null;
                            messageDetails.MSG_CONTENTS = item.MSG_CONTENTS;
                            messageDetails.USER_ID = Convert.ToInt32(item.USER_ID);
                            messageDetails.INPUT_LOCATION_CD = Convert.ToInt32(item.INPUT_LOCATION_CD);

                            messageDetails = _AllCommonRepository.MessageDetailsUpdate(messageDetails);

                        }



                        //Update Tensou
                        _WIPTensouRepository.WIPTensouDetailsUpdate(item);
                        obj = _WIPTensouRepository.GetTransferCD(item.WIP_TRANSFER_GROUP_NO);
                        // item.SERIAL_NUMBER = Convert.ToInt16(i + 1);
                        //    item.WIP_TRANSFER_GROUP_NO = groupNo + 1;


                        //wipSatus Save
                        if (item.NEXT_PROSESS != 999)
                        {
                            var now = DateTime.Now;
                            string time = now.ToString("HH:mm:ss");
                            string dateTime = item.TRANSFER_DATE_STRING + " " + time;

                            WipStatusDetails wipDetails = new WipStatusDetails();
                            wipDetails = new WipStatusDetails();

                            wipDetails.PROCESS_PLAN_CD = Convert.ToInt32(item.PROCESS_PLAN_CD);
                            wipDetails.LOCATION_CD = Convert.ToInt16(item.LOCATION_CD_TO);
                            wipDetails.MSG_CD = item.MSG_CD;
                            wipDetails.FINISHED_BARS = item.NO_OF_UNITS;
                            wipDetails.UNIT = item.UNIT_TYPE;
                            wipDetails.FINISHED_QTY = Convert.ToDouble(item.QTY);
                            wipDetails.PROCESS_START_DATE = Convert.ToDateTime(dateTime); //Convert.ToDateTime(item.TRANSFER_DATE_STRING);
                            wipDetails.PROCESS_END_DATE = Convert.ToDateTime(dateTime); //Convert.ToDateTime(item.TRANSFER_DATE_STRING);
                            wipDetails.PROCESS_KUBUN_CD = kubunCD;
                            wipDetails.PRE_PROSESS_DECISION = null;
                            wipDetails.NEXT_PROCESS = Convert.ToInt16(item.NEXT_PROSESS);
                            wipDetails.START_OPERATOR_CD = null;
                            wipDetails.END_OPERATOR_CD = null;
                            wipDetails.TABA_NO = Convert.ToInt16(item.TABA_NO_1);
                            wipDetails.NIFUDA_CD = null;

                            if (item.TABA_NO_1 != null && item.TABA_NO_2 != null || item.TABA_NO_3 != null || item.TABA_NO_4 != null)
                            {
                                wipDetails.TABA_GROUP_NO = Convert.ToInt16(item.TABA_NO_1);
                            }

                            if (item.TABA_NO_1 > 0)
                            {
                                _AllCommonRepository.DeleteWIPStatusData(wipDetails);
                            }
                            if (item.TABA_NO_2 > 0)
                            {
                                wipDetails.FINISHED_QTY = null;
                                wipDetails.FINISHED_BARS = null;
                                wipDetails.UNIT = null;
                                wipDetails.TABA_NO = Convert.ToInt16(item.TABA_NO_2);
                                _AllCommonRepository.DeleteWIPStatusData(wipDetails);
                            }
                            if (item.TABA_NO_3 > 0)
                            {

                                wipDetails.FINISHED_QTY = null;
                                wipDetails.FINISHED_BARS = null;
                                wipDetails.UNIT = null;
                                wipDetails.TABA_NO = Convert.ToInt16(item.TABA_NO_3);
                                _AllCommonRepository.DeleteWIPStatusData(wipDetails);
                            }
                            if (item.TABA_NO_4 > 0)
                            {

                                wipDetails.FINISHED_QTY = null;
                                wipDetails.FINISHED_BARS = null;
                                wipDetails.UNIT = null;
                                wipDetails.TABA_NO = Convert.ToInt16(item.TABA_NO_4);
                                _AllCommonRepository.DeleteWIPStatusData(wipDetails);
                            }
                        }

                        transaction.Complete();
                    }

                }


            }
            i++;
            return obj;
        }

        public int GetWIPRequestReportData(string transferDate, long userId)
        {
            return _WIPTensouRepository.GetWIPRequestReportData(transferDate, userId);
        }

        public int GetWIPAcceptancetReportData(WIPTensouDetails details)
        {
            return _WIPTensouRepository.GetWIPAcceptanceReportData(details);
        }


        public List<NameValue> GetForwordingLocation()
        {
            var result = _userLogin.GetForwordingLocation();
            return result;
        }

        public List<NameValue> GetVehicleName()
        {
            var result = _userLogin.GetVehicleName();
            return result;
        }

        public int GetRowCountForTransferReport(GridSearchFields gridSearchRequest)
        {
            List<SearchField> searchFields = ComposeSearchCriteriaforTransferDetails(gridSearchRequest);
            int count = _WIPTensouRepository.GetRowCountForTransferReport(searchFields);
            return count;
        }

        //private List<SearchField> ComposeSearchCriteriaforTransferReport(GridSearchFields gridSearchRequest)
        //{
        //    var searchFields = new List<SearchField>();

        //    var locationOrGroup = new List<SearchField>();
        //    //if (gridSearchRequest.MACHINE_CD > 0)
        //    //{
        //    //    var searchField = new SearchField
        //    //    {
        //    //        FieldName = "GP.MACHINE_CD",
        //    //        SearchType = SearchType.Number,
        //    //        SearchText1 = Convert.ToString(gridSearchRequest.MACHINE_CD)
        //    //    };
        //    //    searchFields.Add(searchField);
        //    //}
        //    if (gridSearchRequest.LOCATION_CD > 0)
        //    {
        //        locationOrGroup.Add(new SearchField
        //        {
        //            FieldName = "WIP.LOCATION_CD_FROM ",
        //            SearchType = SearchType.Number,
        //            SearchText1 = Convert.ToString(gridSearchRequest.LOCATION_CD)
        //        });

        //        locationOrGroup.Add(new SearchField
        //        {
        //            FieldName = "WIP.LOCATION_CD_TO ",
        //            SearchType = SearchType.Number,
        //            SearchText1 = Convert.ToString(gridSearchRequest.LOCATION_CD)
        //        });

        //        locationOrGroup.Add(new SearchField
        //        {
        //            FieldName = "ACC.INPUT_LOCATION_CD ",
        //            SearchType = SearchType.Number,
        //            SearchText1 = Convert.ToString(gridSearchRequest.LOCATION_CD)
        //        });

        //        searchFields.Add(new SearchField
        //        {
        //            FieldName = "(" + string.Join(" OR ", locationOrGroup.Select(f => f.FieldName + " = " + f.SearchText1)) + ")",
        //            SearchType = SearchType.Custom
        //        });

        //    }



        //    if (gridSearchRequest.PROCESS_CD > 0)
        //    {
        //        var searchField = new SearchField
        //        {
        //            FieldName = "WIP.PROCESS_KUBUN_CD",
        //            SearchType = SearchType.Number,
        //            SearchText1 = Convert.ToString(gridSearchRequest.PROCESS_CD)
        //        };
        //        searchFields.Add(searchField);
        //    }
        //    if (gridSearchRequest.CUSTOMER_CD > 0)
        //    {
        //        var searchField = new SearchField
        //        {
        //            FieldName = "PP.CUSTOMER_CD",
        //            SearchType = SearchType.Number,
        //            SearchText1 = Convert.ToString(gridSearchRequest.CUSTOMER_CD)
        //        };
        //        searchFields.Add(searchField);
        //    }

        //    if (!string.IsNullOrEmpty(gridSearchRequest.PROD_NAME_CUST))
        //    {
        //        var searchField = new SearchField
        //        {
        //            FieldName = "PP.PROD_NAME_CUST",
        //            SearchType = SearchType.Text,
        //            SearchText1 = gridSearchRequest.PROD_NAME_CUST
        //        };
        //        searchFields.Add(searchField);
        //    }
        //    if (!string.IsNullOrEmpty(gridSearchRequest.INTERNAL_NAME_STRING))
        //    {
        //        var searchField = new SearchField
        //        {
        //            FieldName = "INTERN.INTERNAL_NAME",
        //            SearchType = SearchType.Text,
        //            SearchText1 = gridSearchRequest.INTERNAL_NAME_STRING
        //        };
        //        searchFields.Add(searchField);
        //    }


        //    if (gridSearchRequest.PROD_DIM_D_MAX == null)
        //    {
        //        if (gridSearchRequest.PROD_DIM_D_MIN > 0)
        //        {
        //            var searchField = new SearchField
        //            {
        //                FieldName = "PP.PROD_DIM_D",
        //                SearchType = SearchType.Number,
        //                SearchText1 = Convert.ToString(gridSearchRequest.PROD_DIM_D_MIN)
        //            };
        //            searchFields.Add(searchField);
        //        }
        //    }
        //    else if (gridSearchRequest.PROD_DIM_D_MIN == null)
        //    {
        //        if (gridSearchRequest.PROD_DIM_D_MAX > 0)
        //        {
        //            var searchField = new SearchField
        //            {
        //                FieldName = "PP.PROD_DIM_D",
        //                SearchType = SearchType.Number,
        //                SearchText1 = Convert.ToString(gridSearchRequest.PROD_DIM_D_MAX)
        //            };
        //            searchFields.Add(searchField);
        //        }
        //    }
        //    else if (gridSearchRequest.PROD_DIM_D_MIN > 0 && gridSearchRequest.PROD_DIM_D_MAX > 0)
        //    {
        //        var searchField = new SearchField
        //        {
        //            FieldName = "PP.PROD_DIM_D",
        //            SearchType = SearchType.Number,
        //            SearchText1 = Convert.ToString(gridSearchRequest.PROD_DIM_D_MIN),
        //            SearchText2 = Convert.ToString(gridSearchRequest.PROD_DIM_D_MAX)

        //        };
        //        searchFields.Add(searchField);
        //    }
        //    if (gridSearchRequest.PROD_LENGTH_MAX == null)
        //    {
        //        if (gridSearchRequest.PROD_LENGTH_MIN > 0)
        //        {
        //            var searchField = new SearchField
        //            {
        //                FieldName = "PP.PROD_LENGTH",
        //                SearchType = SearchType.Number,
        //                SearchText1 = Convert.ToString(gridSearchRequest.PROD_LENGTH_MIN)
        //            };
        //            searchFields.Add(searchField);
        //        }
        //    }
        //    else if (gridSearchRequest.PROD_LENGTH_MIN == null)
        //    {
        //        if (gridSearchRequest.PROD_LENGTH_MAX > 0)
        //        {
        //            var searchField = new SearchField
        //            {
        //                FieldName = "PP.PROD_LENGTH",
        //                SearchType = SearchType.Number,
        //                SearchText1 = Convert.ToString(gridSearchRequest.PROD_LENGTH_MAX)
        //            };
        //            searchFields.Add(searchField);
        //        }
        //    }
        //    else if (gridSearchRequest.PROD_LENGTH_MIN > 0 && gridSearchRequest.PROD_LENGTH_MAX > 0)
        //    {
        //        var searchField = new SearchField
        //        {
        //            FieldName = "PP.PROD_LENGTH",
        //            SearchType = SearchType.Number,
        //            SearchText1 = Convert.ToString(gridSearchRequest.PROD_LENGTH_MIN),
        //            SearchText2 = Convert.ToString(gridSearchRequest.PROD_LENGTH_MAX)
        //        };
        //        searchFields.Add(searchField);
        //    }

        //    if (!string.IsNullOrEmpty(gridSearchRequest.SHAPE))
        //    {
        //        var searchField = new SearchField
        //        {
        //            FieldName = "PP.PROD_SHAPE",
        //            SearchType = SearchType.Text,
        //            SearchText1 = gridSearchRequest.SHAPE
        //        };
        //        searchFields.Add(searchField);
        //    }
        //    if (!string.IsNullOrEmpty(gridSearchRequest.CHARGE))
        //    {
        //        var searchField = new SearchField
        //        {
        //            FieldName = "PP.CHARGE",
        //            SearchType = SearchType.Text,
        //            SearchText1 = gridSearchRequest.CHARGE
        //        };
        //        searchFields.Add(searchField);
        //    }
        //    //if (!string.IsNullOrEmpty(gridSearchRequest.CHUUBAN))
        //    //{
        //    //    var searchField = new SearchField
        //    //    {
        //    //        FieldName = "CONCAT(PP.CHUUBAN,CASE WHEN LENGTH(TRIM(PP.GYOUBAN))=1 THEN '0'||PP.GYOUBAN END)",
        //    //        SearchType = SearchType.Text,
        //    //        SearchText1 = gridSearchRequest.CHUUBAN
        //    //    };
        //    //    searchFields.Add(searchField);
        //    //}
        //    if (!string.IsNullOrEmpty(gridSearchRequest.CHUUBAN))
        //    {
        //        var searchField = new SearchField
        //        {
        //            FieldName = "PP.CHUUBAN_YEAR||PP.CHUUBAN",
        //            SearchType = SearchType.Text,
        //            SearchText1 = gridSearchRequest.CHUUBAN
        //        };
        //        searchFields.Add(searchField);
        //    }
        //    if (gridSearchRequest.GYOUBAN > 0)
        //    {
        //        var searchField = new SearchField
        //        {
        //            FieldName = "PP.GYOUBAN",
        //            SearchType = SearchType.Number,
        //            SearchText1 = Convert.ToString(gridSearchRequest.GYOUBAN)
        //        };
        //        searchFields.Add(searchField);
        //    }
        //    if (!string.IsNullOrEmpty(gridSearchRequest.PROD_SPEC_NO1))
        //    {
        //        var searchField = new SearchField
        //        {
        //            FieldName = "PP.PROD_SPEC_NO1",
        //            SearchType = SearchType.Number,
        //            SearchText1 = "'" + gridSearchRequest.PROD_SPEC_NO1 + "'"
        //        };
        //        searchFields.Add(searchField);
        //    }
        //    if (!string.IsNullOrEmpty(gridSearchRequest.PROD_SPEC_NO2))
        //    {
        //        var searchField = new SearchField
        //        {
        //            FieldName = "PP.PROD_SPEC_NO2",
        //            SearchType = SearchType.Number,
        //            SearchText1 = "'" + gridSearchRequest.PROD_SPEC_NO2 + "'"
        //        };
        //        searchFields.Add(searchField);
        //    }
        //    if (string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE2))
        //    {
        //        if (!string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE1))
        //        {
        //            var searchField = new SearchField
        //            {
        //                FieldName = "WIP.TRANSFER_DATE",
        //                SearchType = SearchType.Date,
        //                SearchText1 = gridSearchRequest.INSPECTION_DATE1
        //            };
        //            searchFields.Add(searchField);
        //        }
        //    }
        //    else if (string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE1))
        //    {
        //        if (!string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE2))
        //        {
        //            var searchField = new SearchField
        //            {
        //                FieldName = "WIP.TRANSFER_DATE",
        //                SearchType = SearchType.Date,
        //                SearchText1 = gridSearchRequest.INSPECTION_DATE2
        //            };
        //            searchFields.Add(searchField);
        //        }
        //    }
        //    else if (!string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE1) && !string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE2))
        //    {
        //        var searchField = new SearchField
        //        {
        //            FieldName = "WIP.TRANSFER_DATE",
        //            SearchType = SearchType.Date,
        //            SearchText1 = gridSearchRequest.INSPECTION_DATE1,
        //            SearchText2 = gridSearchRequest.INSPECTION_DATE2
        //        };
        //        searchFields.Add(searchField);
        //    }

        //    return searchFields;
        //}

        public List<WIPTensouDetails> GetTransferDetails(GridSearchFields gridSearchRequest)
        {
            List<SearchField> searchFields = ComposeSearchCriteriaforTransferDetails(gridSearchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);
            int rowLimit = gridSearchRequest.ROW_LIMIT;
            var result = _WIPTensouRepository.GetTransferDetails(searchFields, selectedSortCriteria, rowLimit);

            //foreach (WIPTensouDetails record in result)
            //{
            //    {
            //        record.PP_NO_OF_UNITS_DECODE = Convert.ToDecimal(record.NO_OF_UNITS).ToString("0.####") + UIUtils.GetProductUnitKubunValue(Convert.ToString(record.UNIT_TYPE));

            //    }
            //}
            return result;
        }

        private List<SearchField> ComposeSearchCriteriaforTransferDetails(GridSearchFields gridSearchRequest)
        {
            var searchFields = new List<SearchField>();

            var locationOrGroup = new List<SearchField>();
            var dateOrGroup = new List<SearchField>();

            if (gridSearchRequest.LOCATION_CD > 0)
            {
                locationOrGroup.Add(new SearchField
                {
                    FieldName = "WIP.LOCATION_CD_FROM",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.LOCATION_CD)
                });

                locationOrGroup.Add(new SearchField
                {
                    FieldName = "WIP.LOCATION_CD_TO",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.LOCATION_CD)
                });

                searchFields.Add(new SearchField
                {
                    FieldName = "(" + string.Join(" OR ", locationOrGroup.Select(f => f.FieldName + " = " + f.SearchText1)) + ")",
                    SearchType = SearchType.Custom
                });

            }
            if (gridSearchRequest.PROCESS_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "WIP.PROCESS_KUBUN_CD",
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
            //if (string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE2))
            //{
            //    if (!string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE1))
            //    {
            //        var searchField = new SearchField
            //        {
            //            FieldName = "WIP.TRANSFER_DATE",
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
            //            FieldName = "WIP.TRANSFER_DATE",
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
            //        FieldName = "WIP.TRANSFER_DATE",
            //        SearchType = SearchType.DateTime,
            //        SearchText1 = gridSearchRequest.INSPECTION_DATE1,
            //        SearchText2 = gridSearchRequest.INSPECTION_DATE2
            //    };
            //    searchFields.Add(searchField);
            //}

            if (!string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE1) && !string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE2))
            {
                dateOrGroup.Add(new SearchField
                {
                    FieldName = "WIP.ACCEPT_PLANNED_DATE",
                    SearchType = SearchType.DateTime,
                    SearchText1 = DateTime.Parse(gridSearchRequest.INSPECTION_DATE1).ToString("yyyy/MM/dd"),
                    SearchText2 = DateTime.Parse(gridSearchRequest.INSPECTION_DATE2).ToString("yyyy/MM/dd")
                });

                dateOrGroup.Add(new SearchField
                {
                    FieldName = "WIP.TRANSFER_DATE",
                    SearchType = SearchType.DateTime,
                    SearchText1 = DateTime.Parse(gridSearchRequest.INSPECTION_DATE1).ToString("yyyy/MM/dd"),
                    SearchText2 = DateTime.Parse(gridSearchRequest.INSPECTION_DATE2).ToString("yyyy/MM/dd")
                });

                searchFields.Add(new SearchField
                {
                    FieldName = "(" + string.Join(" OR ", dateOrGroup.Select(f =>
                        $"{f.FieldName} BETWEEN " +
                        $"TO_DATE('{f.SearchText1} 00:00','yyyy/MM/dd hh24:mi') " +
                        $"AND TO_DATE('{f.SearchText2} 23:59','yyyy/MM/dd hh24:mi')"
                    )) + ")",
                    SearchType = SearchType.Custom
                });
            }
            return searchFields;
        }
    }
}
