using Org.BouncyCastle.Asn1.Cmp;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace TSWebApplication.Application.Services
{
    public class ChamfferService : IChamfferService
    {
        private readonly IChamfferRepository _chamfferRepository;
        private readonly LocalizationServices _localization;
        private readonly IAllCommonRepository _allCommonRepository;
        private readonly IAllCommonService _allCommonService;
        public ChamfferService(IChamfferRepository chamfferRepository, LocalizationServices localization, IAllCommonRepository allCommonRepository, IAllCommonService allCommonService)
        {
            _chamfferRepository = chamfferRepository;
            _localization = localization;
            _allCommonRepository = allCommonRepository;
            _allCommonService = allCommonService;

        }

        public List<ChamferDetails> GetChamfferList(GridSearchFields gridSearchRequest)
        {
            short LocationCD = Convert.ToInt16(_chamfferRepository.GetUserLocationById(Convert.ToInt32(gridSearchRequest.USER_ID)));
            gridSearchRequest.LOCATION_CD = LocationCD;

            List<ChamferDetails> result = new List<ChamferDetails>();
            List<SearchField> searchFields = ComposeSearchCriteria(gridSearchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);
            result = _chamfferRepository.GetChamferingList(searchFields, selectedSortCriteria, gridSearchRequest.LOCATION_CD);
            for (int i = 0; i < result.Count; i++)
            {
                result[i].CH_DECISION_STRING = UIUtils.GetResultKubunValue(Convert.ToString(result[i].CH_DECISION));
            }
            return result;
        }

        public ChamferDetails GetChamferingDetailsByCD(long chamferingCD, string cultureName)
        {
            if (cultureName == "en")
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
            }
            else
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("ja-JP");
            }
            var obj = _chamfferRepository.GetChamferingDetailsByCD(chamferingCD);
            if (obj != null)
            {
                if (obj.PP_STD_DEL_PERIOD >= 0)
                {
                    obj.PP_STD_DEL_PERIOD_STRING = obj.PP_STD_DEL_PERIOD + " " + _localization.GetLocalizedHtmlString("lbl_Day");
                }
                if (obj.PP_UNIT != null)
                {
                    obj.PP_NO_OF_UNITS_DECODE = Convert.ToDecimal(obj.PP_NO_OF_UNITS).ToString("0.####") + UIUtils.GetProductUnitKubunValue(Convert.ToString(obj.PP_UNIT));
                }

                if (obj.COIL_DIA_WINDING != null)
                {
                    obj.PP_COIL_DIA_WINDING = UIUtils.GetCoilDiaWindingKubunValue(Convert.ToString(obj.COIL_DIA_WINDING));
                }

                if (obj.PP_UNIT_WT_KUBUN != null)
                {
                    obj.PP_UNIT_WT_KUBUN_STRING = Utils.GetUnitWeigth(Convert.ToInt32(obj.PP_UNIT_WT_KUBUN), Convert.ToInt32(obj.PP_UNIT_WT), Convert.ToInt32(obj.PP_UNIT_WT_MIN), Convert.ToInt32(obj.PP_UNIT_WT_MIN));

                    if (obj.PP_UNIT_WT_KUBUN == 3)
                    {
                        obj.PP_UNIT_WT_KUBUN_STRING = Convert.ToString(obj.PP_UNIT_WT) + _localization.GetLocalizedHtmlString("lbl_Hone");
                    }


                }


                if (obj.MAGARI_KUBUN != null)
                {
                    obj.PP_MAGARI_BEND_TOL = Utils.GetMagariBendTolSpan(UIUtils.Instance.GetMagariKubunValue(Convert.ToString(obj.MAGARI_KUBUN)), Convert.ToDecimal(obj.MAGARI_BEND_TOL), Convert.ToInt32(obj.MAGARI_SPAN));  //Convert.ToDecimal(obj.PP_NO_OF_UNITS).ToString("0.####") + UIUtils.Instance.GetProductUnitKubunValue(Convert.ToString(obj.PP_UNIT));
                }

                if (obj.PP_COLOR_ID != null)
                {
                    obj.PP_COLOR = UIUtils.GetColorKubunValue(Convert.ToString(obj.PP_COLOR_ID));  //Utils.GetColorName(Convert.ToInt16(obj.PP_COLOR_ID));
                }
                obj.DECISION_OK = Convert.ToInt16(UIUtils.GetResultKubunKey(_localization.GetLocalizedHtmlString("lbl_OK")));
                return obj;
            }
            else
            {
                return new ChamferDetails();
            }
        }

        public List<ThabaDetails> GetThabaDetailsByChuuban(string chubaan, string gyouban, long chamferingCD)
        {
            var result = _chamfferRepository.GetThabaDetailsWithOKDecision(chubaan, gyouban, chamferingCD);

            return result;
        }

        public List<ThabaDetails> GetThabaDetailsByChuuban(string chubaan, string gyouban)
        {
            var result = _chamfferRepository.GetThabaDetails(chubaan, gyouban);
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

        public void Delete(ChamferDetails request)
        {
            WipStatusDetails wipDetails = new WipStatusDetails();
            wipDetails = new WipStatusDetails();
            wipDetails.PROCESS_PLAN_CD = request.PROCESS_PLAN_CD;
            using (TransactionScope transaction = new TransactionScope())
            {


                if (request.CHAMFERING_CD > 0)
                {
                    _chamfferRepository.Delete(request.CHAMFERING_CD);
                }
                if (request.CH_TABA_NO_1 > 0)
                {

                    wipDetails.TABA_NO = request.CH_TABA_NO_1;
                    _allCommonRepository.DeleteWIPStatusData(wipDetails);


                }
                if (request.CH_TABA_NO_2 > 0)
                {
                    wipDetails.TABA_NO = request.CH_TABA_NO_2;
                    _allCommonRepository.DeleteWIPStatusData(wipDetails);
                }
                if (request.CH_TABA_NO_3 > 0)
                {
                    wipDetails.TABA_NO = request.CH_TABA_NO_3;
                    _allCommonRepository.DeleteWIPStatusData(wipDetails);
                }
                if (request.CH_TABA_NO_4 > 0)
                {
                    wipDetails.TABA_NO = request.CH_TABA_NO_4;
                    _allCommonRepository.DeleteWIPStatusData(wipDetails);
                }

                if (request.MSG_CD > 0)
                {
                    _allCommonRepository.MessageDetailsDelete(request.MSG_CD);
                }


                transaction.Complete();
            }
        }

        //public void ChamferDetailsUpdate(ChamfferUpdateRequest request)
        //{
        //    if (request.USER_ID != null && request.USER_ID > 0)
        //    {
        //        request.INPUT_LOCATION_CD = _chamfferRepository.GetUserLocationById(request.USER_ID.Value);
        //    }
        //    if (request.MACHINE_CD != null && request.MACHINE_CD > 0)
        //    {
        //        request.LOCATION_CD = _chamfferRepository.GetMachineNameByID(Convert.ToInt64(request.MACHINE_CD.Value))?.LOCATION_CD;
        //    }
        //    using (TransactionScope transaction = new TransactionScope())
        //    {
        //        MessageDetails messageDetails = new MessageDetails();
        //        if (!string.IsNullOrEmpty(request.CH_REMARKS))
        //        {
        //            messageDetails.PROCESS_PLAN_CD = Convert.ToInt64(request.PROCESS_PLAN_CD);
        //            messageDetails.MACHINE_CD = Convert.ToInt32(request.MACHINE_CD);
        //            messageDetails.PROCESS_KUBUN_ID = Convert.ToInt32(Enums.PROCESS_KUBUN_EN.Chamfering);
        //            messageDetails.OPERATOR_CD = Convert.ToInt64(request.CH_END_OPERATOR_CD);
        //            messageDetails.MSG_CONTENTS = request.CH_REMARKS;
        //            messageDetails.USER_ID = Convert.ToInt32(request.USER_ID);
        //            messageDetails.LOCATION_CD = Convert.ToInt32(request.INPUT_LOCATION_CD);
        //            messageDetails.INPUT_LOCATION_CD = Convert.ToInt32(request.INPUT_LOCATION_CD);

        //            messageDetails = _chamfferRepository.MessageDetailsInsert(messageDetails);
        //        }

        //        if (messageDetails.MSG_CD != Constants.NullValue.LONG && messageDetails.MSG_CD > 0)
        //        {
        //            request.CH_MSG_CD = Convert.ToInt32(messageDetails.MSG_CD);
        //        }

        //        _chamfferRepository.ChamferDetailsUpdate(request);
        //        transaction.Complete();
        //    }
        //}

        public long ChamferDetailsUpdate(ChamferDetails request)
        {
            long msgCd = 0;
            bool deleteMsg = false;
            if (request.USER_ID != null && request.USER_ID > 0)
            {
                request.INPUT_LOCATION_CD = _chamfferRepository.GetUserLocationById(request.USER_ID);
            }
            if (request.MACHINE_CD != null && request.MACHINE_CD > 0)
            {
                request.LOCATION_CD = Convert.ToInt32(_chamfferRepository.GetMachineNameByID(request.MACHINE_CD)?.LOCATION_CD);
            }
            using (TransactionScope transaction = new TransactionScope())
            {
                MessageDetails messageDetails = new MessageDetails();
                //if (request.CH_MSG_CD == Constants.NullValue.INT && !string.IsNullOrEmpty(request.CH_REMARKS))
                //{
                messageDetails.PROCESS_PLAN_CD = Convert.ToInt64(request.PROCESS_PLAN_CD);
                messageDetails.MACHINE_CD = Convert.ToInt32(request.MACHINE_CD);
                messageDetails.PROCESS_KUBUN_ID = Convert.ToInt32(UIUtils.GetProcessKubun(_localization.GetLocalizedHtmlString("lbl_ProcessChamferS")));
                messageDetails.OPERATOR_CD = Convert.ToInt64(request.CH_END_OPERATOR_CD);
                messageDetails.MSG_CONTENTS = request.CH_REMARKS;
                messageDetails.USER_ID = Convert.ToInt32(request.USER_ID);
                //messageDetails.LOCATION_CD = Convert.ToInt32(request.INPUT_LOCATION_CD);
                messageDetails.INPUT_LOCATION_CD = Convert.ToInt32(request.INPUT_LOCATION_CD);
                messageDetails.MSG_CD = Convert.ToInt32(request.CH_MSG_CD);

                //messageDetails = _chamfferRepository.MessageDetailsInsert(messageDetails);

                //if (messageDetails.MSG_CD != Constants.NullValue.INT && messageDetails.MSG_CD > 0)
                //{
                //    request.CH_MSG_CD = Convert.ToInt32(messageDetails.MSG_CD);
                //}

                //if (request.CHAMFERING_CD > 0)
                //{
                //    _chamfferRepository.ChamferDetailsUpdate(request);
                //}
                if (request.CH_MSG_CD == Constants.NullValue.INT)
                {

                    if (!string.IsNullOrEmpty(request.CH_REMARKS))
                    {

                        messageDetails = _allCommonRepository.MessageDetailsInsert(messageDetails);

                        if (messageDetails.MSG_CD != Constants.NullValue.INT && messageDetails.MSG_CD > 0)
                        {
                            request.CH_MSG_CD = Convert.ToInt32(messageDetails.MSG_CD);
                        }
                    }

                    if (request.CHAMFERING_CD > 0)
                    {
                        _chamfferRepository.ChamferDetailsUpdate(request);
                    }
                }
                // }

                //if (messageDetails.MSG_CD != Constants.NullValue.LONG && messageDetails.MSG_CD > 0)
                //{
                //    request.CH_MSG_CD = Convert.ToInt32(messageDetails.MSG_CD);
                //}
                // If REMARKS was already present, and it is updated,  
                // then UPDATE the TRN_MESSAGE
                else if (request.CH_MSG_CD > 0 && request.CH_REMARKS != null)
                {
                    _allCommonRepository.MessageDetailsUpdate(messageDetails);

                    if (request.CHAMFERING_CD > 0)
                    {
                        _chamfferRepository.ChamferDetailsUpdate(request);
                    }
                }
                //If REMARKS was already present, but it is delete now,
                // then DELETE the TRN_MESSAGE
                //else if (request.CH_MSG_CD > 0 && request.CH_REMARKS == null)
                //{
                //    long messageCD = Convert.ToInt64(request.CH_MSG_CD);
                //    request.CH_MSG_CD = Constants.NullValue.LONG;

                //    if (request.CHAMFERING_CD > 0)
                //    {
                //        _chamfferRepository.ChamferDetailsUpdate(request);
                //    }
                //    _allCommonRepository.MessageDetailsDelete(messageCD);
                //}
                //else
                //{

                //    if (request.CHAMFERING_CD > 0)
                //    {
                //        _chamfferRepository.ChamferDetailsUpdate(request);
                //    }
                //}
                else if (request.CH_MSG_CD > 0 && request.CH_REMARKS == null)
                {
                    long messageCD = Convert.ToInt64(request.CH_MSG_CD);
                    request.CH_MSG_CD = Constants.NullValue.LONG;


                    if (request.CH_TABA_NO_1 > 0)
                    {
                        _allCommonRepository.DeleteWIPStatusDataPreProcess(Convert.ToInt64(request.PROCESS_PLAN_CD), Convert.ToInt16(request.CH_TABA_NO_1));
                    }
                    if (request.CH_TABA_NO_2 > 0)
                    {
                        _allCommonRepository.DeleteWIPStatusDataPreProcess(Convert.ToInt64(request.PROCESS_PLAN_CD), Convert.ToInt16(request.CH_TABA_NO_2));
                    }
                    if (request.CH_TABA_NO_3 > 0)
                    {
                        _allCommonRepository.DeleteWIPStatusDataPreProcess(Convert.ToInt64(request.PROCESS_PLAN_CD), Convert.ToInt16(request.CH_TABA_NO_3));
                    }
                    if (request.CH_TABA_NO_4 > 0)
                    {
                        _allCommonRepository.DeleteWIPStatusDataPreProcess(Convert.ToInt64(request.PROCESS_PLAN_CD), Convert.ToInt16(request.CH_TABA_NO_4));
                    }





                    if (request.CHAMFERING_CD > 0)
                    {
                        msgCd = _chamfferRepository.ChamferDetailsUpdate(request);
                    }


                    _allCommonRepository.MessageDetailsDelete(messageCD);
                    deleteMsg = true;
                }

                if (deleteMsg == false)
                {
                    msgCd = _chamfferRepository.ChamferDetailsUpdate(request);
                }
                if (request.CH_NEXT_PROCESS != 999)
                {

                    WipStatusDetails wipDetails = new WipStatusDetails();
                    wipDetails = new WipStatusDetails();
                    wipDetails.PROCESS_PLAN_CD = request.PROCESS_PLAN_CD;
                    wipDetails.LOCATION_CD = Convert.ToInt16(request.LOCATION_CD);
                    wipDetails.MSG_CD = request.CH_MSG_CD;
                    wipDetails.FINISHED_BARS = request.CH_FINISHED_BARS;
                    wipDetails.UNIT = request.UNIT_TYPE;
                    wipDetails.FINISHED_QTY = request.CH_FINISHED_QTY;
                    wipDetails.PROCESS_START_DATE = request.CH_START_DATE;
                    wipDetails.PROCESS_END_DATE = request.CH_END_DATE;
                    wipDetails.PROCESS_KUBUN_CD = Convert.ToInt32(UIUtils.GetProcessKubun(_localization.GetLocalizedHtmlString("lbl_ProcessChamferS")));
                    wipDetails.PRE_PROSESS_DECISION = request.CH_DECISION;
                    wipDetails.NEXT_PROCESS = Convert.ToInt16(request.CH_NEXT_PROCESS);
                    wipDetails.START_OPERATOR_CD = request.CH_START_OPERATOR_CD;
                    wipDetails.END_OPERATOR_CD = request.CH_END_OPERATOR_CD;
                    wipDetails.TABA_NO = request.CH_TABA_NO_1;
                    wipDetails.NIFUDA_CD = 0;



                    if (request.CH_TABA_NO_1 > 0)
                    {
                        wipDetails.TABA_NO = request.CH_TABA_NO_1;
                        _allCommonRepository.DeleteWIPStatusData(wipDetails);
                    }
                    if (request.CH_TABA_NO_2 > 0)
                    {
                        wipDetails.TABA_NO = request.CH_TABA_NO_2;
                        _allCommonRepository.DeleteWIPStatusData(wipDetails);
                    }
                    if (request.CH_TABA_NO_3 > 0)
                    {
                        wipDetails.TABA_NO = request.CH_TABA_NO_3;
                        _allCommonRepository.DeleteWIPStatusData(wipDetails);
                    }
                    if (request.CH_TABA_NO_4 > 0)
                    {
                        wipDetails.TABA_NO = request.CH_TABA_NO_4;
                        _allCommonRepository.DeleteWIPStatusData(wipDetails);
                    }





                    //if (request.CH_TABA_NO_1 != null && request.CH_TABA_NO_2 != null || request.CH_TABA_NO_3 != null || request.CH_TABA_NO_4 != null)
                    //{

                    //    wipDetails.TABA_GROUP_NO = request.CH_TABA_NO_1;

                    //}
                    // _allCommonRepository.InsertWIPStatusData(wipDetails);
                    //if (request.CH_TABA_NO_2 > 0)
                    //{

                    //    wipDetails.FINISHED_QTY = null;
                    //    wipDetails.TABA_NO = request.CH_TABA_NO_2;
                    //    _allCommonRepository.InsertWIPStatusData(wipDetails);
                    //}
                    //if (request.CH_TABA_NO_3 > 0)
                    //{

                    //    wipDetails.FINISHED_QTY = null;
                    //    wipDetails.TABA_NO = request.CH_TABA_NO_3;
                    //    _allCommonRepository.InsertWIPStatusData(wipDetails);
                    //}
                    //if (request.CH_TABA_NO_4 > 0)
                    //{

                    //    wipDetails.FINISHED_QTY = null;
                    //    wipDetails.TABA_NO = request.CH_TABA_NO_4;
                    //    _allCommonRepository.InsertWIPStatusData(wipDetails);
                    //}
                }
                transaction.Complete();
            }
            return msgCd;
        }

        public List<ChamferDetails> GetChamfering24MachineList(string machineCd)
        {
            var machineDetails = _chamfferRepository.GetChamfering24MachineList(machineCd);
            foreach (ChamferDetails record in machineDetails)
            {

                string decisionName = UIUtils.GetResultKubunValue(Convert.ToString(record.CH_DECISION));
                if (_localization.GetLocalizedHtmlString("lbl_OK") == decisionName)
                {
                    record.CH_DECISION = 1;
                }
                if (_localization.GetLocalizedHtmlString("lbl_NG") == decisionName)
                {
                    record.CH_DECISION = 2;
                }
                if (_localization.GetLocalizedHtmlString("lbl_Hold") == decisionName)
                {
                    record.CH_DECISION = 3;
                }
                if (_localization.GetLocalizedHtmlString("lbl_DecisionKubun4of4") == decisionName)
                {
                    record.CH_DECISION = 4;
                }

            }

            return machineDetails;
        }

        public ChamferDetails GetChamferDetailsByChuuban(string chubaan, string gyouban, string mechineCD, string nifudaNo, string cultureName)
        {

            if (cultureName == "en")
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
            }
            else
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("ja-JP");
            }


            var obj = _chamfferRepository.GetChamferDetailsByChuubanGyoubanMachine(chubaan, Convert.ToInt16(gyouban), mechineCD, nifudaNo);
            if (obj != null)
            {
                //if (cultureName == "en")
                //{
                //    System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
                //}
                //else
                //{
                //    System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("ja-JP");
                //}



                if (obj.PP_UNIT_WT_KUBUN != null)
                {
                    obj.PP_UNIT_WT_KUBUN_STRING = Utils.GetUnitWeigth(Convert.ToInt32(obj.PP_UNIT_WT_KUBUN), Convert.ToInt32(obj.PP_UNIT_WT), Convert.ToInt32(obj.PP_UNIT_WT_MIN), Convert.ToInt32(obj.PP_UNIT_WT_MAX));

                    if (obj.PP_UNIT_WT_KUBUN == 3)
                    {
                        obj.PP_UNIT_WT_KUBUN_STRING = Convert.ToString(obj.PP_UNIT_WT) + _localization.GetLocalizedHtmlString("lbl_Hone");
                    }


                }


                if (obj.PP_STD_DEL_PERIOD >= 0)
                {
                    obj.PP_STD_DEL_PERIOD_STRING = obj.PP_STD_DEL_PERIOD + " " + _localization.GetLocalizedHtmlString("lbl_Day");
                }
                if (obj.PP_UNIT > 0)
                {
                    obj.PP_NO_OF_UNITS_DECODE = Convert.ToDecimal(obj.PP_NO_OF_UNITS).ToString("0.####") + UIUtils.GetProductUnitKubunValue(Convert.ToString(obj.PP_UNIT));
                }
                if (obj.COIL_DIA_WINDING != null)
                {
                    obj.PP_COIL_DIA_WINDING = UIUtils.GetCoilDiaWindingKubunValue(Convert.ToString(obj.COIL_DIA_WINDING));
                }
                if (obj.MAGARI_KUBUN > 0)
                {
                    obj.PP_MAGARI_BEND_TOL = Utils.GetMagariBendTolSpan(UIUtils.Instance.GetMagariKubunValue(Convert.ToString(obj.MAGARI_KUBUN)), Convert.ToDecimal(obj.MAGARI_BEND_TOL), Convert.ToInt32(obj.MAGARI_SPAN));  //Convert.ToDecimal(obj.PP_NO_OF_UNITS).ToString("0.####") + UIUtils.Instance.GetProductUnitKubunValue(Convert.ToString(obj.PP_UNIT));
                }

                if (obj.MAGARI_KUBUN != null)
                {
                    obj.PP_MAGARI_BEND_TOL = Utils.GetMagariBendTolSpan(UIUtils.Instance.GetMagariKubunValue(Convert.ToString(obj.MAGARI_KUBUN)), Convert.ToDecimal(obj.MAGARI_BEND_TOL), Convert.ToInt32(obj.MAGARI_SPAN));  //Convert.ToDecimal(obj.PP_NO_OF_UNITS).ToString("0.####") + UIUtils.Instance.GetProductUnitKubunValue(Convert.ToString(obj.PP_UNIT));
                }


                //if (obj.PP_COLOR_ID > 0)
                //{
                //    obj.PP_COLOR = UIUtils.GetColorKubunValue(Convert.ToString(obj.PP_COLOR_ID));  //Utils.GetColorName(Convert.ToInt16(obj.PP_COLOR_ID));
                //}
                obj.DECISION_OK = Convert.ToInt16(UIUtils.GetResultKubunKey(_localization.GetLocalizedHtmlString("lbl_OK")));
                var messageList = _chamfferRepository.GetMessageDetailsByProcessPlanCD(Convert.ToInt64(obj.PP_PROCESS_PLAN_CD), true);
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
        public KeyValuePair<string, string> GetMachineNameByID(long machineCd)
        {
            var data = (machineCd > 0) ? _chamfferRepository.GetMachineNameByID(machineCd) : null;
            if (data == null)
            {
                return new KeyValuePair<string, string>(null, null);
            }
            else
            {
                string inActive = data.IN_ACTIVE_FLAG == 1 ? "inactive" : "active";
                return new KeyValuePair<string, string>(data.SHORT_NAME, inActive);
            }
        }

        public ChamferDetails InsertChamferDetails(ChamferDetails request)
        {
            ChamferDetails result = new ChamferDetails();

            using (TransactionScope transaction = new TransactionScope())
            {
                if (request.USER_ID != null && request.USER_ID > 0)
                {
                    request.INPUT_LOCATION_CD = _chamfferRepository.GetUserLocationById(request.USER_ID);
                }
                if (request.MACHINE_CD != null && request.MACHINE_CD > 0)
                {
                    request.LOCATION_CD = _chamfferRepository.GetMachineNameByID(Convert.ToInt64(request.MACHINE_CD)).LOCATION_CD;
                }
                WipStatusDetails wipDetails = new WipStatusDetails();

                wipDetails = new WipStatusDetails();
                wipDetails.PROCESS_PLAN_CD = request.PROCESS_PLAN_CD;
                wipDetails.LOCATION_CD = Convert.ToInt16(request.LOCATION_CD);
                wipDetails.MSG_CD = null;
                wipDetails.FINISHED_BARS = null;
                wipDetails.UNIT = null;
                wipDetails.FINISHED_QTY = null;
                wipDetails.PROCESS_START_DATE = request.CH_START_DATE;
                wipDetails.PROCESS_END_DATE = null;
                wipDetails.PROCESS_KUBUN_CD = Convert.ToInt32(UIUtils.GetProcessKubun(_localization.GetLocalizedHtmlString("lbl_ProcessChamferS"))); //request.PROCESS_KUBUN_CD;
                wipDetails.PRE_PROSESS_DECISION = null;
                wipDetails.NEXT_PROCESS = null;
                wipDetails.START_OPERATOR_CD = request.CH_START_OPERATOR_CD;
                wipDetails.END_OPERATOR_CD = null;
                // wipDetails.TABA_NO = request.CH_TABA_NO_1;
                if (request.CH_TABA_NO_1 != null)
                {
                    wipDetails.TABA_NO = Convert.ToInt16(request.CH_TABA_NO_1);
                }
                else
                {
                    wipDetails.TABA_NO = 0;
                }

                if (request.CH_TABA_NO_1 != null && request.CH_TABA_NO_2 != null || request.CH_TABA_NO_3 != null || request.CH_TABA_NO_4 != null)
                {

                    wipDetails.TABA_GROUP_NO = request.CH_TABA_NO_1;

                }
                _allCommonRepository.DeleteWIPStatusData(wipDetails);

                if (request.CH_TABA_NO_2 > 0)
                {
                    wipDetails.TABA_NO = Convert.ToInt16(request.CH_TABA_NO_2);
                    _allCommonRepository.DeleteWIPStatusData(wipDetails);


                }

                if (request.CH_TABA_NO_3 > 0)
                {
                    wipDetails.TABA_NO = Convert.ToInt16(request.CH_TABA_NO_3);
                    _allCommonRepository.DeleteWIPStatusData(wipDetails);

                }

                if (request.CH_TABA_NO_4 > 0)
                {
                    wipDetails.TABA_NO = Convert.ToInt16(request.CH_TABA_NO_4);
                    _allCommonRepository.DeleteWIPStatusData(wipDetails);

                }
                wipDetails.NIFUDA_CD = null;
                //request.PROCESS_KUBUN_CD = Convert.ToInt32(Enums.PROCESS_KUBUN_EN.Chamfering);
                request.PROCESS_KUBUN_CD = Convert.ToInt32(UIUtils.GetProcessKubun(_localization.GetLocalizedHtmlString("lbl_ProcessChamferS")));
                _chamfferRepository.InsertChamferDetails(request);
                result = GetChamferDetailsByChuuban(request.PP_CHUUBAN, Convert.ToString(request.PP_GYOUBAN), Convert.ToString(request.MACHINE_CD), request.NIFUDA_STRING, request.CULTURE_NAME);
                transaction.Complete();
            }
            return result;
        }
        private List<SearchField> ComposeSearchCriteria(GridSearchFields gridSearchRequest)
        {
            var searchFields = new List<SearchField>();

            if (!string.IsNullOrEmpty(gridSearchRequest.CHUUBAN))
            {
                string chuuban = string.Empty;
                if (gridSearchRequest.CHUUBAN.Length > 9)
                {
                    chuuban = gridSearchRequest.CHUUBAN.Substring(0, 9);
                }
                else
                {
                    chuuban = gridSearchRequest.CHUUBAN.Trim();
                }

                var searchField = new SearchField
                {
                    FieldName = "CONCAT(PP.CHUUBAN_YEAR, CONCAT(PP.CHUUBAN,CASE WHEN LENGTH(TRIM(PP.GYOUBAN))=1 THEN '0'||PP.GYOUBAN END))",
                    SearchType = SearchType.Text,

                    SearchText1 = chuuban.Trim()

                };

                searchFields.Add(searchField);
            }

            if (gridSearchRequest.TABA_NO > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "CH.CH_TABA_NO_1",
                    SearchType = SearchType.Text,
                    SearchText1 = Convert.ToString(gridSearchRequest.TABA_NO)
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
                    SearchText1 = Convert.ToString(gridSearchRequest.PROD_DIM_D_MIN).ToLower().Trim(),
                    SearchText2 = Convert.ToString(gridSearchRequest.PROD_DIM_D_MAX).ToLower().Trim()

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
                    SearchText1 = Convert.ToString(gridSearchRequest.PROD_LENGTH_MIN).ToLower().Trim(),
                    SearchText2 = Convert.ToString(gridSearchRequest.PROD_LENGTH_MAX).ToLower().Trim()
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.START_DATETIME))
            {
                var searchField = new SearchField
                {
                    FieldName = "CH.CH_START_DATE",
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
                    SearchText1 = gridSearchRequest.EMPLOYEE_CD.ToString()
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
        public string GetEmployeeNameByID(long employeeCd)
        {
            return (employeeCd > 0) ? _chamfferRepository.GetEmployeeNameByID(employeeCd) : null;
        }
        public List<NameValue> GetEmployeeNameList(string searchText, bool addEmpty)
        {
            var customerList = new List<NameValue>();
            customerList = _chamfferRepository.GetEmployeeNameList(searchText, addEmpty);
            if (customerList != null && addEmpty)
            {
                var defaulItem = new NameValue { CD = 0, Name = string.Empty };
                customerList.Insert(0, defaulItem);
            }
            return customerList;
        }

        public List<NameValue> GetMachineNameList()
        {
            var machineNameList = _chamfferRepository.GetMachineNameList();
            return machineNameList;
        }

        public List<NameValue> GetDecisionList()
        {
            var _DecisionList = UIUtils.GetResultKubun();
            return _DecisionList;
        }

        public List<NameValue> GetUnitTypeList()
        {
            var _UnitTypeList = UIUtils.GetUnitTypetKubun();
            return _UnitTypeList;
        }

        public string GetThabaDetailsByThabaNumber(ChamferDetails request)
        {
            string duplicateThaba = string.Empty;
            var result = _chamfferRepository.GetThabaDetailsByThabaNumber(request.PP_CHUUBAN, Convert.ToString(request.PP_GYOUBAN), request.TABA_NO_STRING);
            foreach (ChamferDetails record in result)
            {
                string decisionName = UIUtils.GetResultKubunValue(Convert.ToString(record.CH_DECISION));
                if (_localization.GetLocalizedHtmlString("lbl_OK") == decisionName)
                {
                    record.CH_DECISION = 1;
                }
                if (_localization.GetLocalizedHtmlString("lbl_NG") == decisionName)
                {
                    record.CH_DECISION = 2;
                }
                if (_localization.GetLocalizedHtmlString("lbl_Hold") == decisionName)
                {
                    record.CH_DECISION = 3;
                }
                if (_localization.GetLocalizedHtmlString("lbl_DecisionKubun4of4") == decisionName)
                {
                    record.CH_DECISION = 4;
                }
            }
            if (result != null)
            {
                ChamferDetails getData = result.AsEnumerable().Select(x => x).OrderBy(r => r.CHAMFERING_CD).FirstOrDefault();
                bool isTaba1Correct = true;
                bool isTaba2Correct = true;
                bool isTaba3Correct = true;
                bool isTaba4Correct = true;

                if (result.Count > 1 && request.RECHAMFERING == 0)
                {
                    if (request.CH_TABA_NO_1 > 0)
                    {
                        var taba1Data = result.AsEnumerable().Select(x => x).OrderBy(r => r.CHAMFERING_CD).Where(r => r.CH_TABA_NO_1 == request.CH_TABA_NO_1 && r.CHAMFERING_CD == request.CHAMFERING_CD && r.RECHAMFERING == 0).FirstOrDefault();
                        if (taba1Data == null)
                        {
                            isTaba1Correct = false;
                        }
                    }
                    if (request.CH_TABA_NO_2 > 0)
                    {
                        var taba2Data = result.AsEnumerable().Select(x => x).OrderBy(r => r.CHAMFERING_CD).Where(r => r.CH_TABA_NO_2 == request.CH_TABA_NO_2 && r.CHAMFERING_CD == request.CHAMFERING_CD && r.RECHAMFERING == 0).FirstOrDefault();
                        if (taba2Data == null)
                        {
                            isTaba2Correct = false;
                        }
                    }
                    if (request.CH_TABA_NO_3 > 0)
                    {
                        var taba3Data = result.AsEnumerable().Select(x => x).OrderBy(r => r.CHAMFERING_CD).Where(r => r.CH_TABA_NO_3 == request.CH_TABA_NO_3 && r.CHAMFERING_CD == request.CHAMFERING_CD && r.RECHAMFERING == 0).FirstOrDefault();
                        if (taba3Data == null)
                        {
                            isTaba3Correct = false;
                        }
                    }
                    if (request.CH_TABA_NO_4 > 0)
                    {
                        var taba4Data = result.AsEnumerable().Select(x => x).OrderBy(r => r.CHAMFERING_CD).Where(r => r.CH_TABA_NO_4 == request.CH_TABA_NO_4 && r.CHAMFERING_CD == request.CHAMFERING_CD && r.RECHAMFERING == 0).FirstOrDefault();
                        if (taba4Data == null)
                        {
                            isTaba4Correct = false;
                        }
                    }


                    if (isTaba1Correct && isTaba2Correct && isTaba3Correct && isTaba4Correct)
                    {
                        duplicateThaba = string.Empty;
                    }
                    else
                    {
                        List<ChamferDetails> details = new List<ChamferDetails>();

                        details = result.AsEnumerable().Where(x => x.CHAMFERING_CD != request.CHAMFERING_CD).Select(x => x).ToList();

                        foreach (var item in details)
                        {
                            if (item.CH_TABA_NO_1 > 0)
                            {
                                if (item.CH_TABA_NO_1 == request.CH_TABA_NO_1 || item.CH_TABA_NO_1 == request.CH_TABA_NO_2 || item.CH_TABA_NO_1 == request.CH_TABA_NO_3 || item.CH_TABA_NO_1 == request.CH_TABA_NO_4)
                                {
                                    if (string.IsNullOrEmpty(duplicateThaba))
                                    {
                                        duplicateThaba = Convert.ToString(item.CH_TABA_NO_1);
                                    }
                                    else
                                    {
                                        duplicateThaba = duplicateThaba + "," + Convert.ToString(item.CH_TABA_NO_1);
                                    }
                                }
                            }
                            if (item.CH_TABA_NO_2 > 0)
                            {
                                if (item.CH_TABA_NO_2 == request.CH_TABA_NO_1 || item.CH_TABA_NO_2 == request.CH_TABA_NO_2 || item.CH_TABA_NO_2 == request.CH_TABA_NO_3 || item.CH_TABA_NO_2 == request.CH_TABA_NO_4)
                                {
                                    if (string.IsNullOrEmpty(duplicateThaba))
                                    {
                                        duplicateThaba = Convert.ToString(item.CH_TABA_NO_2);
                                    }
                                    else
                                    {
                                        duplicateThaba = duplicateThaba + "," + Convert.ToString(item.CH_TABA_NO_2);
                                    }
                                }
                            }
                            if (item.CH_TABA_NO_3 > 0)
                            {
                                if (item.CH_TABA_NO_3 == request.CH_TABA_NO_1 || item.CH_TABA_NO_3 == request.CH_TABA_NO_2 || item.CH_TABA_NO_3 == request.CH_TABA_NO_3 || item.CH_TABA_NO_3 == request.CH_TABA_NO_4)
                                {
                                    if (string.IsNullOrEmpty(duplicateThaba))
                                    {
                                        duplicateThaba = Convert.ToString(item.CH_TABA_NO_3);
                                    }
                                    else
                                    {
                                        duplicateThaba = duplicateThaba + "," + Convert.ToString(item.CH_TABA_NO_3);
                                    }
                                }
                            }
                            if (item.CH_TABA_NO_4 > 0)
                            {
                                if (item.CH_TABA_NO_4 == request.CH_TABA_NO_1 || item.CH_TABA_NO_4 == request.CH_TABA_NO_2 || item.CH_TABA_NO_4 == request.CH_TABA_NO_3 || item.CH_TABA_NO_4 == request.CH_TABA_NO_4)
                                {
                                    if (string.IsNullOrEmpty(duplicateThaba))
                                    {
                                        duplicateThaba = Convert.ToString(item.CH_TABA_NO_4);
                                    }
                                    else
                                    {
                                        duplicateThaba = duplicateThaba + "," + Convert.ToString(item.CH_TABA_NO_4);
                                    }
                                }
                            }
                            //}
                            if (!string.IsNullOrEmpty(duplicateThaba))
                            {
                                string[] value = duplicateThaba.Split(',');
                                var distinctItems = value.Distinct();
                                duplicateThaba = string.Join(", ", distinctItems);
                            }


                        }
                    }

                }
                else if (result.Count == 1 && request.RECHAMFERING == 0 && result[0].CHAMFERING_CD != request.CHAMFERING_CD)
                {
                    if (result[0].CH_TABA_NO_1 == request.CH_TABA_NO_1 || result[0].CH_TABA_NO_1 == request.CH_TABA_NO_2 || result[0].CH_TABA_NO_1 == request.CH_TABA_NO_3 || result[0].CH_TABA_NO_1 == request.CH_TABA_NO_4)
                    {
                        if (string.IsNullOrEmpty(duplicateThaba))
                        {
                            duplicateThaba = Convert.ToString(result[0].CH_TABA_NO_1);
                        }
                        else
                        {
                            duplicateThaba = duplicateThaba + "," + Convert.ToString(result[0].CH_TABA_NO_1);
                        }
                    }
                    if (Convert.ToInt16(result[0].CH_TABA_NO_2) > 0)
                    {
                        if (result[0].CH_TABA_NO_2 == request.CH_TABA_NO_1 || result[0].CH_TABA_NO_2 == request.CH_TABA_NO_2 || result[0].CH_TABA_NO_2 == request.CH_TABA_NO_3 || result[0].CH_TABA_NO_2 == request.CH_TABA_NO_4)
                        {
                            if (string.IsNullOrEmpty(duplicateThaba))
                            {
                                duplicateThaba = Convert.ToString(result[0].CH_TABA_NO_2);
                            }
                            else
                            {
                                duplicateThaba = duplicateThaba + "," + Convert.ToString(result[0].CH_TABA_NO_2);
                            }
                        }
                    }
                    if (Convert.ToInt16(result[0].CH_TABA_NO_3) > 0)
                    {
                        if (Convert.ToInt16(result[0].CH_TABA_NO_3) > 0 && result[0].CH_TABA_NO_3 == request.CH_TABA_NO_1 || result[0].CH_TABA_NO_3 == request.CH_TABA_NO_2 || result[0].CH_TABA_NO_3 == request.CH_TABA_NO_3 || result[0].CH_TABA_NO_3 == request.CH_TABA_NO_4)
                        {
                            if (string.IsNullOrEmpty(duplicateThaba))
                            {
                                duplicateThaba = Convert.ToString(result[0].CH_TABA_NO_3);
                            }
                            else
                            {
                                duplicateThaba = duplicateThaba + "," + Convert.ToString(result[0].CH_TABA_NO_3);
                            }
                        }
                    }
                    if (Convert.ToInt16(result[0].CH_TABA_NO_4) > 0)
                    {
                        if (Convert.ToInt16(result[0].CH_TABA_NO_4) > 0 && result[0].CH_TABA_NO_4 == request.CH_TABA_NO_1 || result[0].CH_TABA_NO_4 == request.CH_TABA_NO_2 || result[0].CH_TABA_NO_4 == request.CH_TABA_NO_3 || result[0].CH_TABA_NO_4 == request.CH_TABA_NO_4)
                        {
                            if (string.IsNullOrEmpty(duplicateThaba))
                            {
                                duplicateThaba = Convert.ToString(result[0].CH_TABA_NO_4);
                            }
                            else
                            {
                                duplicateThaba = duplicateThaba + "," + Convert.ToString(result[0].CH_TABA_NO_4);
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(duplicateThaba))
                    {
                        string[] value = duplicateThaba.Split(',');
                        var distinctItems = value.Distinct();
                        duplicateThaba = string.Join(", ", distinctItems);
                    }

                }
                else
                {
                    duplicateThaba = string.Empty;
                }


            }
            else
            {
                duplicateThaba = string.Empty;
            }
            return duplicateThaba;
        }
        public List<ChamferDetails> GetChamferDailyReportOnFactoryDailyWorkStatus(GridSearchFields gridSearchRequest)
        {
            List<SearchField> searchFields = ComposeSearchCriteriaforFactoryDailyWorkStatus(gridSearchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);
            int rowLimit = gridSearchRequest.ROW_LIMIT;
            var result = _chamfferRepository.GetChamferDailyReportOnFactoryDailyWorkStatus(searchFields, selectedSortCriteria, rowLimit);
            return result;
        }

        private List<SearchField> ComposeSearchCriteriaforFactoryDailyWorkStatus(GridSearchFields gridSearchRequest)
        {
            var searchFields = new List<SearchField>();
            if (gridSearchRequest.MACHINE_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "CHA.MACHINE_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MACHINE_CD)
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.LOCATION_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "CHA.LOCATION_CD",
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
                    FieldName = "CHA.CH_END_OPERATOR_CD",
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
                        FieldName = "CHA.CH_END_DATE",
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
                        FieldName = "CHA.CH_END_DATE",
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
                    FieldName = "CHA.CH_END_DATE",
                    SearchType = SearchType.DateTime,
                    SearchText1 = gridSearchRequest.INSPECTION_DATE1,
                    SearchText2 = gridSearchRequest.INSPECTION_DATE2
                };
                searchFields.Add(searchField);
            }


            return searchFields;
        }


        public List<ChamferDetails> GetChamferDetailsOnDailyProcessAnalysis(GridSearchFields gridSearchRequest)
        {


            List<SearchField> searchFields = ComposeSearchCriteriaforDailyProcess(gridSearchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);

            var obj = _chamfferRepository.GetChamferDetailsOnDailyProcessAnalysis(searchFields, selectedSortCriteria, gridSearchRequest);

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
                    FieldName = "CH.CH_START_DATE",
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
                    FieldName = "CH.CH_METALLIC_PLATE_CODE",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.METALLIC_PLATE_CODE_STRING.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.FURNACE_NO))
            {
                var searchField = new SearchField
                {
                    FieldName = "CH.FURNACE_NO",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.FURNACE_NO.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.END_DATE1))
            {
                var searchField = new SearchField
                {
                    FieldName = "CH.CH_END_DATE",
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
                    FieldName = "CH.CH_DECISION",
                    SearchType = SearchType.Text,
                    SearchText1 = Convert.ToString(gridSearchRequest.DECISION)
                };
                searchFields.Add(searchField);
            }
            return searchFields;
        }


        public int GetRowCountForChamfferFactory(GridSearchFields gridSearchRequest)
        {
            List<SearchField> searchFields = ComposeSearchCriteriaforFactoryDailyWorkStatus(gridSearchRequest);
            int count = _chamfferRepository.GetRowCountForChamfferFactory(searchFields);
            return count;
        }

        public int GetRowCountForChamfferDailyAnalysis(GridSearchFields gridSearchRequest)
        {
            List<SearchField> searchFields = ComposeSearchCriteriaforDailyProcess(gridSearchRequest);
            int count = _chamfferRepository.GetRowCountForChamfferDailyAnalysis(searchFields);
            return count;
        }

    }

}
