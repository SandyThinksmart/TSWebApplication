using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Application.IServices;
using TSWebApplication.Infrastructure.IRepositories;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;
using System.Transactions;
using TSWebApplication.Infrastructure.Repositories;
using TSWebApplication.Domain.Resources;
using System.Globalization;
using static System.Net.Mime.MediaTypeNames;

namespace TSWebApplication.Application.Services
{
    public class WIPTensouAcceptanceService : IWIPTensouAcceptanceService
    {
        private readonly IWIPTensouAcceptanceRepository _WIPTensouAcceptanceRepository;
        private readonly IUserLoginRepository _userLogin;
        private readonly IAllCommonRepository _AllCommonRepository;
        private readonly LocalizationServices _localization;
        public WIPTensouAcceptanceService(IWIPTensouAcceptanceRepository WIPTensouAcceptanceRepository, IUserLoginRepository userLogin, IAllCommonRepository AllCommonRepository, LocalizationServices localization)
        {
            _userLogin = userLogin;
            _WIPTensouAcceptanceRepository = WIPTensouAcceptanceRepository;
            _AllCommonRepository = AllCommonRepository;
            _localization = localization;

        }

        public List<NameValue> GetAcceptanceLocation(int USER_ID)
        {
            var result = _userLogin.GetUserLocationNameById(USER_ID);
            return result;

        }

        public List<NameValue> GetNextProcessList()
        {
            var _nextProcessList = UIUtils.GetNextProcesKubun();
            return _nextProcessList;
        }

        public List<WIPTensouDetails> GetWIPTransferAcceptanceDetailsByTrasferCD(string tensouCD)
        {
            var _WIPTensouDetails = _WIPTensouAcceptanceRepository.GetWIPTransferAcceptanceDetailsByTrasferCD(tensouCD);
            foreach (var item in _WIPTensouDetails)
            {
                item.UNIT_TYPE_STRING = UIUtils.GetProductUnitKubunValue(Convert.ToString(item.UNIT_TYPE));
            }

            return _WIPTensouDetails;
        }

        public Dictionary<string, List<string>> GetValidTensouNo(string WIPTensouCD)
        {
            var _WIPTensouNo = _WIPTensouAcceptanceRepository.GetValidTensouNo(WIPTensouCD);
            return _WIPTensouNo;
        }

        public Dictionary<string, List<string>> GetCompleteChuuban(string completeTransferNo)
        {
            var _WIPTensouNo = _WIPTensouAcceptanceRepository.GetCompleteChuuban(completeTransferNo);
            return _WIPTensouNo;
        }
        public Dictionary<string, List<string>> TransferNoAccepted(string transferNoAccepted)
        {
            var _WIPTensouNo = _WIPTensouAcceptanceRepository.TransferNoAccepted(transferNoAccepted);
            return _WIPTensouNo;
        }

        //save data acceptance
        public WIPTensouDetails SaveWIPTensouAcceptanceDetails(WIPTensouDetails request)
        {
            WIPTensouDetails obj = new WIPTensouDetails();
            var kubunCD = Convert.ToInt32(UIUtils.GetProcessKubun(_localization.GetLocalizedHtmlString("lbl_ProcessWIP_Accepting_Transfer")));
            request.PROCESS_KUBUN_ID = kubunCD;
            using (TransactionScope transaction = new TransactionScope())
            {
                if (request.USER_ID != null && request.USER_ID > 0)
                {
                    request.INPUT_LOCATION_CD = _userLogin.GetUserLocationById(request.USER_ID.Value);
                    request.LOCATION_CD = request.INPUT_LOCATION_CD;
                }

                request.WIP_TRANSFER_GROUP_NO = _WIPTensouAcceptanceRepository.GetMaxGroupNo();
                if (obj.PP_UNIT != null)
                {
                    obj.PP_NO_OF_UNITS_DECODE = Convert.ToDecimal(obj.PP_NO_OF_UNITS).ToString("0.####") + UIUtils.GetProductUnitKubunValue(Convert.ToString(obj.PP_UNIT));
                }

                //Save in TRN_Message Table
                if (!string.IsNullOrEmpty(request.MSG_CONTENTS))
                {
                    MessageDetails messageDetails = new MessageDetails();
                    messageDetails.PROCESS_PLAN_CD = Convert.ToInt64(request.PROCESS_PLAN_CD);
                    messageDetails.PROCESS_KUBUN_ID = kubunCD;
                    messageDetails.OPERATOR_CD = null;
                    messageDetails.MSG_CONTENTS = request.MSG_CONTENTS;
                    messageDetails.MSG_CD = Convert.ToInt32(request.MSG_CD);
                    messageDetails.USER_ID = Convert.ToInt32(request.USER_ID);
                    messageDetails.INPUT_LOCATION_CD = Convert.ToInt32(request.INPUT_LOCATION_CD);

                    if (request.MSG_CD == null || request.MSG_CD == 0 && !string.IsNullOrWhiteSpace(request.MSG_CONTENTS))
                    {
                        messageDetails = _AllCommonRepository.MessageDetailsInsert(messageDetails);

                        if (messageDetails.MSG_CD != Constants.NullValue.LONG && messageDetails.MSG_CD > 0)
                        {
                            request.MSG_CD = Convert.ToInt32(messageDetails.MSG_CD);
                        }
                    }
                    else if (request.MSG_CD > 0 && request.MSG_CONTENTS != null)
                    {
                        request.MSG_CD = _WIPTensouAcceptanceRepository.GetMsgCdByMsgContent(request.MSG_CONTENTS);
                        if (request.MSG_CD == 0 || request.MSG_CD == null)
                        {
                            messageDetails = _AllCommonRepository.MessageDetailsInsert(messageDetails);
                            if (messageDetails != null)
                            {
                                request.MSG_CD = Convert.ToInt32(messageDetails.MSG_CD);
                            }
                        }
                        //_AllCommonRepository.MessageDetailsUpdate(messageDetails);
                    }

                }
                else
                {
                    request.MSG_CD = null;
                }

                // Update table - TRN_WIP_AND_PROD_TRANSFER
                _WIPTensouAcceptanceRepository.SaveWIPTensouAcceptanceDetails(request);

                //if (request.WIP_TRANSFER_CD >= 0)
                //{
                // _WIPTensouAcceptanceRepository.WIPTensouTableTensouDoneDateUpdate(Convert.ToUInt16(request.WIP_TRANSFER_CD));
                //}

                //wipSatus Save
                if (request.NEXT_PROSESS != 999)
                {
                    var now = DateTime.Now;
                    string time = now.ToString("HH:mm:ss");
                    string dateTime = request.TRANSFER_DATE_STRING + " " + time;

                    WipStatusDetails wipDetails = new WipStatusDetails();
                    wipDetails = new WipStatusDetails();

                    wipDetails.PROCESS_PLAN_CD = Convert.ToInt32(request.PROCESS_PLAN_CD);
                    wipDetails.LOCATION_CD = Convert.ToInt16(request.LOCATION_CD_TO);

                    if (request.MSG_CD == 0)
                    {
                        wipDetails.MSG_CD = null;
                    }
                    else
                    {
                        wipDetails.MSG_CD = request.MSG_CD;
                    }
                    wipDetails.FINISHED_BARS = request.NO_OF_UNITS;
                    wipDetails.UNIT = request.UNIT_TYPE;
                    wipDetails.FINISHED_QTY = Convert.ToDouble(request.QTY);
                    wipDetails.PROCESS_START_DATE = Convert.ToDateTime(dateTime);
                    wipDetails.PROCESS_END_DATE = Convert.ToDateTime(dateTime);
                    wipDetails.PROCESS_KUBUN_CD = kubunCD;
                    wipDetails.PRE_PROSESS_DECISION = null;
                    wipDetails.NEXT_PROCESS = Convert.ToInt16(request.NEXT_PROSESS);
                    wipDetails.START_OPERATOR_CD = null;
                    wipDetails.END_OPERATOR_CD = null;
                    wipDetails.TABA_NO = Convert.ToInt16(request.TABA_NO_1);
                    wipDetails.NIFUDA_CD = null;

                    if (request.TABA_NO_1 != null && request.TABA_NO_2 != null || request.TABA_NO_3 != null || request.TABA_NO_4 != null)
                    {
                        wipDetails.TABA_GROUP_NO = Convert.ToInt16(request.TABA_NO_1);
                    }

                    if (request.TABA_NO_1 > 0)
                    {
                        _AllCommonRepository.DeleteWIPStatusData(wipDetails);
                    }
                    if (request.TABA_NO_2 > 0)
                    {
                        wipDetails.FINISHED_QTY = null;
                        wipDetails.FINISHED_BARS = null;
                        wipDetails.UNIT = null;
                        wipDetails.TABA_NO = Convert.ToInt16(request.TABA_NO_2);
                        _AllCommonRepository.DeleteWIPStatusData(wipDetails);
                    }
                    if (request.TABA_NO_3 > 0)
                    {
                        wipDetails.FINISHED_QTY = null;
                        wipDetails.FINISHED_BARS = null;
                        wipDetails.UNIT = null;
                        wipDetails.TABA_NO = Convert.ToInt16(request.TABA_NO_3);
                        _AllCommonRepository.DeleteWIPStatusData(wipDetails);
                    }
                    if (request.TABA_NO_4 > 0)
                    {

                        wipDetails.FINISHED_QTY = null;
                        wipDetails.FINISHED_BARS = null;
                        wipDetails.UNIT = null;
                        wipDetails.TABA_NO = Convert.ToInt16(request.TABA_NO_4);
                        _AllCommonRepository.DeleteWIPStatusData(wipDetails);
                    }

                }
                transaction.Complete();
            }

            return obj;
        }

        public void WIPTensouAcceptanceDetailsDelete(List<WIPTensouDetails> request)
        {
            WIPTensouDetails obj = new WIPTensouDetails();
            WipStatusDetails wipDetails = new WipStatusDetails();

            foreach (var item in request)
            {
                using (TransactionScope transaction = new TransactionScope())
                {
                    //WIP Acceptance Record delete 
                    if (item.WIP_TRANSFER_CD >= 0)
                    {
                        _WIPTensouAcceptanceRepository.WIPTensouAcceptanceDetailsDelete(Convert.ToString(item.WIP_TRANSFER_CD));
                    } 

                    //msg delete 
                    if (item.MSG_CD == 0 && item.MSG_CONTENTS != null)
                    {
                        item.MSG_CD = _WIPTensouAcceptanceRepository.GetMsgCdByMsgContent(item.MSG_CONTENTS);
                        int msgCDFromTensouTable = _WIPTensouAcceptanceRepository.GetMsgCdFromTensouRequest(Convert.ToInt32(item.MSG_CD));
                        if (msgCDFromTensouTable == 0)
                        {
                            _AllCommonRepository.MessageDetailsDelete(Convert.ToInt64(item.MSG_CD));
                        }
                    }
                    else
                    {
                        int msgCDFromTensouTable = _WIPTensouAcceptanceRepository.GetMsgCdFromTensouRequest(Convert.ToInt32(item.MSG_CD));
                        if (msgCDFromTensouTable == 0)
                        {
                            _AllCommonRepository.MessageDetailsDelete(Convert.ToInt64(item.MSG_CD));
                        }
                    }

                    //Taba Delete in wip status table
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


        }

        public List<WIPTensouDetails> GetWIPTransferAcceptanceDetailsByTrasferCD(string tensouCD, string CultureName)
        {
            var _WIPTensouDetails = _WIPTensouAcceptanceRepository.GetWIPTransferAcceptanceDetailsByTrasferCD(tensouCD);
            if (CultureName == "en")
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
            }
            else
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("ja-JP");
            }
            foreach (var item in _WIPTensouDetails)
            {
                item.UNIT_TYPE_STRING = UIUtils.GetProductUnitKubunValue(Convert.ToString(item.UNIT_TYPE));
            }

            return _WIPTensouDetails;
        }
    }
}
