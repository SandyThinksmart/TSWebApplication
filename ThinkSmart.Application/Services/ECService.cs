using Org.BouncyCastle.Asn1.Cmp;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.RegularExpressions;
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
    public class ECService : IECService
    {
        private readonly IECRepository _eCRepository;
        private readonly IAllCommonRepository _commonRepository;
        private readonly LocalizationServices _localization;
        private readonly IAllCommonService _allCommonService;
        public ECService(IECRepository eCRepository, IAllCommonRepository commonRepository, LocalizationServices localization, IAllCommonService allCommonService)
        {
            _eCRepository = eCRepository;
            _commonRepository = commonRepository;
            _localization = localization;
            _allCommonService = allCommonService;
        }

        public ECDetails GetECDetailsByChuuban(string nifuda, short? mechineCD, string cultureName)
        {
            if (cultureName =="en")
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
            }
            else
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("ja-JP");
            }
            var obj = _eCRepository.GetECDetailsByChuuban(nifuda, mechineCD);
            if (obj != null)
            {

                if (obj.PP_STD_DEL_PERIOD >= 0)
                {
                    obj.PP_STD_DEL_PERIOD_STRING = obj.PP_STD_DEL_PERIOD + " " + _localization.GetLocalizedHtmlString("lbl_Day");
                }
                if (obj.PP_UNIT != null)
                {
                    obj.PP_NO_OF_UNITS_STRING = Convert.ToDecimal(obj.PP_NO_OF_UNITS).ToString("0.####") + "" + UIUtils.GetProductUnitKubunValue(Convert.ToString(obj.PP_UNIT));
                }

                if (obj.MAGARI_KUBUN != null)
                {
                    obj.PP_MAGARI_BEND_TOL = Utils.GetMagariBendTolSpan(UIUtils.Instance.GetMagariKubunValue(Convert.ToString(obj.MAGARI_KUBUN)), Convert.ToDecimal(obj.MAGARI_BEND_TOL), Convert.ToInt32(obj.MAGARI_SPAN));
                }

                if (obj.PP_UNIT_WT_KUBUN > 0)
                {
                    obj.PP_UNIT_WT_STRING = Utils.GetUnitWeigth(Convert.ToInt16(obj.PP_UNIT_WT_KUBUN), Convert.ToInt32(obj.PP_UNIT_WT_STRING), Convert.ToInt16(obj.PP_UNIT_WT_MIN), obj.PP_UNIT_WT_MAX);

                }

                if (obj.PP_COIL_DIA_WINDING != null)
                {
                  
                    obj.PP_COIL_DIA_WINDING_STRING = UIUtils.GetCoilDiaWindingKubunValue(Convert.ToString(obj.PP_COIL_DIA_WINDING));
                }
                obj.DECISION_OK = Convert.ToInt16(UIUtils.GetResultKubunKey(_localization.GetLocalizedHtmlString("lbl_OK")));
                var messageList = _commonRepository.GetMessageDetailsByProcessPlanCD(Convert.ToInt64(obj.PP_PROCESS_PLAN_CD), true);
                obj.EC_MESSAGE_LIST = messageList;

                obj.PP_TENSILE_STRENGTH = Helper.GetProductPatternFormat(Convert.ToSingle(obj.TENSILE_STRN_PATTERN1), obj.TENSILE_STRN_PATTERN1_MIN_MAX, obj.TENSILE_STRN_PATTERN2_MIN, obj.TENSILE_STRN_PATTERN2_MAX, obj.TENSILE_STRN_PATTERN3_ACTUAL);
                obj.PP_YEILD = Helper.GetProductPatternFormat(Convert.ToSingle(obj.YEILD_PATTERN1), obj.YEILD_PATTERN1_MIN_MAX, obj.YEILD_PATTERN2_MIN, obj.YEILD_PATTERN2_MAX, obj.YEILD_PATTERN3_ACTUAL);
                obj.PP_ELONGATION = Helper.GetProductPatternFormat(Convert.ToSingle(obj.ELONGATION_PATTERN1), obj.ELONGATION_PATTERN1_MIN_MAX,Convert.ToInt16(obj.ELONGATION_PATTERN2_MIN),Convert.ToInt16(obj.ELONGATION_PATTERN2_MAX), obj.ELONGATION_PATTERN3_ACTUAL);
                obj.PP_PRESS = Helper.GetProductPatternFormat(Convert.ToSingle(obj.PRESS_PATTERN1), obj.PRESS_PATTERN1_MIN_MAX, obj.PRESS_PATTERN2_MIN, obj.PRESS_PATTERN2_MAX, obj.PRESS_PATTERN3_ACTUAL);
                obj.PP_HARDNESS = Helper.GetHardnessProductPatternFormat(obj.HARDNESS_PATTERN1_CODE, obj.HARDNESS_PATTERN1, obj.HARDNESS_PATTERN1_MIN_MAX, obj.HARDNESS_PATTERN2_CODE, obj.HARDNESS_PATTERN2_MIN, obj.HARDNESS_PATTERN2_MAX, obj.HARDNESS_PATTERN3_ACTUAL);
                obj.PP_DECARB = Helper.GetDecarbProductPatternFormat(obj.DECARB_1, Convert.ToSingle(obj.DECARB_VAL_1), obj.DECARB_2, Convert.ToSingle(obj.DECARB_VAL_2), obj.DECARB_3, Convert.ToSingle(obj.DECARB_VAL_3));
            }
            return obj;
        }
        public int IsFIAlreadyDone(long nifudaCD, short thabaNo)
        {
            return _eCRepository.FIAlreadyDone(nifudaCD, thabaNo);
        }
        public List<ECDetails> IsFinalInspectionCompleted(long nifudaCD, short thabaNo, int ecCD)
        {
            return _eCRepository.IsFICompleted(nifudaCD, thabaNo,ecCD);
        }
        public List<ECDetails> GetEC24MachineDetails(string machineCd, string cultureName)
        {
            var obj = _eCRepository.GetEC24MachineList(machineCd, cultureName);
            if (cultureName == "en")
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
            }
            else
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("ja-JP");
            }

            foreach (ECDetails record in obj)
            {
                string decisionName = UIUtils.GetResultKubunValue(Convert.ToString(record.EC_DECISION));
                if (_localization.GetLocalizedHtmlString("lbl_OK") == decisionName)
                {
                    record.EC_DECISION = 1;
                }
                if (_localization.GetLocalizedHtmlString("lbl_NG") == decisionName)
                {
                    record.EC_DECISION = 2;
                }
                if (_localization.GetLocalizedHtmlString("lbl_Hold") == decisionName)
                {
                    record.EC_DECISION = 3;
                }
                if (_localization.GetLocalizedHtmlString("lbl_DecisionKubun4of4") == decisionName)
                {
                    record.EC_DECISION = 4;
                }
                if (_localization.GetLocalizedHtmlString("lbl_ReHeatTreatment") == decisionName || _localization.GetLocalizedHtmlString("lbl_ReDrawing") == decisionName ||
               _localization.GetLocalizedHtmlString("lbl_Restraightening") == decisionName || _localization.GetLocalizedHtmlString("lbl_Re_Chamfering") == decisionName || _localization.GetLocalizedHtmlString("lbl_ReEc") == decisionName)
                {
                    record.EC_DECISION = 5;
                }
                record.PP_NO_OF_UNITS_STRING = record.PP_NO_OF_UNITS != null ? record.PP_NO_OF_UNITS + UIUtils.GetProductUnitKubunValue(Convert.ToString(record.PP_UNIT)) : string.Empty;
                record.EC_OK_BARS_STRING = record.EC_OK_BARS != null ? record.EC_OK_BARS + UIUtils.GetProductUnitKubunValue(Convert.ToString(record.UNIT_TYPE)) : string.Empty;
            }
            return obj;
        }

        public List<ThabaDetails> GetThabaDetailsByChuuban(string chubaan, short? gyouban)
        {
     
            var result= _eCRepository.GetThabaDetails(chubaan, gyouban);

            foreach (ThabaDetails record in result)
            {

              string decisionName=  UIUtils.GetResultKubunValue(Convert.ToString(record.DECISION));
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

        public ThabaDetails GetThabaDetailsByThabaNumber(long processPlanCD, short thabaNo)
        {


            var result = _eCRepository.GetThabaDetailsByThabaNumber(processPlanCD, thabaNo);
            return result;
        }
        public ECDetails GetECHistoryByChuuban(string chuuban, short? gyouban, string cultureName)
        {
            ECDetails result = new ECDetails();
            result = _eCRepository.GetECProcessPlanDetailsByChuuban(chuuban, gyouban);
            if (cultureName == "en")
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
            }
            else
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("ja-JP");
            }

            if (result != null)
            {
                if (result.MAGARI_KUBUN != null)
                {
                    result.PP_MAGARI_BEND_TOL = Utils.GetMagariBendTolSpan(UIUtils.Instance.GetMagariKubunValue(Convert.ToString(result.MAGARI_KUBUN)), Convert.ToDecimal(result.MAGARI_BEND_TOL), Convert.ToInt32(result.MAGARI_SPAN));  //Convert.ToDecimal(obj.PP_NO_OF_UNITS).ToString("0.####") + UIUtils.Instance.GetProductUnitKubunValue(Convert.ToString(obj.PP_UNIT));
                }
                if (result.PP_STD_DEL_PERIOD >= 0)
                {
                    result.PP_STD_DEL_PERIOD_STRING = result.PP_STD_DEL_PERIOD + " " + _localization.GetLocalizedHtmlString("lbl_Day");
                }
                if (result.MAGARI_KUBUN != null)
                {
                    result.PP_MAGARI_BEND_TOL = Utils.GetMagariBendTolSpan(UIUtils.Instance.GetMagariKubunValue(Convert.ToString(result.MAGARI_KUBUN)), Convert.ToDecimal(result.MAGARI_BEND_TOL), Convert.ToInt32(result.MAGARI_SPAN));  //Convert.ToDecimal(obj.PP_NO_OF_UNITS).ToString("0.####") + UIUtils.Instance.GetProductUnitKubunValue(Convert.ToString(obj.PP_UNIT));
                }

                if (result.PP_UNIT_WT_KUBUN > 0)
                {
                    result.PP_UNIT_WT_STRING = Utils.GetUnitWeigth(Convert.ToInt16(result.PP_UNIT_WT_KUBUN), Convert.ToInt32(result.PP_UNIT_WT_STRING), Convert.ToInt16(result.PP_UNIT_WT_MIN), result.PP_UNIT_WT_MAX);

                }

                if (result.PP_COIL_DIA_WINDING != null)
                {
                    result.PP_COIL_DIA_WINDING_STRING = UIUtils.GetCoilDiaWindingKubunValue(Convert.ToString(result.PP_COIL_DIA_WINDING));
                }
                if (result.PP_UNIT != null)
                {
                    result.PP_NO_OF_UNITS_DECODE = Convert.ToDecimal(result.PP_NO_OF_UNITS).ToString("0.####") + UIUtils.GetProductUnitKubunValue(Convert.ToString(result.PP_UNIT));
                }

            }
            var historyList = BindHistoryGrid(chuuban, gyouban, cultureName);
            foreach (ECDetails record in historyList)
            {

                if (record.PP_NO_OF_UNITS != null)
                {
                    record.PP_NO_OF_UNITS_DECODE = Convert.ToDecimal(record.PP_NO_OF_UNITS).ToString("0.####") + UIUtils.GetProductUnitKubunValue(Convert.ToString(record.PP_UNIT));
                }

                record.NG_DETAILS = string.Empty;

                record.EC_OK_BARS_STRING = record.EC_OK_BARS + UIUtils.GetProductUnitKubunValue(Convert.ToString(record.UNIT_TYPE));

                // record.EC_VISUAL_STRING = UIUtils.GetTrialResultKubun(Convert.ToString(record.EC_VISUAL_CHK));
                record.EC_VISUAL_STRING = UIUtils.GetFIResultKubun(Convert.ToString(record.EC_VISUAL_CHK));
                record.EC_DECISION_STRING = UIUtils.GetResultKubunValue(Convert.ToString(record.EC_DECISION));
                record.TP_TEST_STRING = UIUtils.GetTPTestListValue(Convert.ToString(record.TP_TEST));  //UIUtils.GetTPTestKubunValue(Convert.ToString(record.TP_TEST));

                record.FI_DIM_DECISION_ST_STRING = UIUtils.GetFIResultKubun(Convert.ToString(record.FI_DIM_DECISION));
                record.FI_LENGTH_DECISION_STRING = UIUtils.GetFIResultKubun(Convert.ToString(record.FI_LENGTH_DECISION));

                record.FI_COLOR_DECISION_STRING = UIUtils.GetFIResultKubun(Convert.ToString(record.FI_COLOR_DECISION));
                record.FI_CHAMFERING_DECISION_STRING = UIUtils.GetFIResultKubun(Convert.ToString(record.FI_CHAMFERING_DECISION));
                record.EXT_TEXTURE_DECISION = UIUtils.GetFIResultKubun(Convert.ToString(record.FI_EXT_TEXTURE_DECISION));
                record.FI_COMBINED_DECISION_STRING = UIUtils.GetTrialResultKubun(Convert.ToString(record.COMBINED_DECISION));
                record.FI_NEXT_PROCESS_STRING = UIUtils.GetTrialResultKubun(Convert.ToString(record.FI_NEXT_PROCESS));


            }
            if (historyList.Count > 0)
            {
                result.PROCESS_HISTORY = historyList;
            }
            return result;
        }
        public List<ECDetails> BindHistoryGrid(string chubban, short? gyouban, string cultureName)
        {
            return _eCRepository.GetProcessHistoryList(chubban, gyouban, cultureName);
        }
       
        public ECDetails InsertECDetails(ECDetails ecDetails)
        {
            using (TransactionScope transaction = new TransactionScope())
            {

                if (ecDetails.USER_ID != null && ecDetails.USER_ID > 0)
                {
                    ecDetails.INPUT_LOCATION_CD = _commonRepository.GetUserLocationById(ecDetails.USER_ID.Value);
                }
                //   ecDetails.LOCATION_CD = Convert.ToInt32(ecDetails.INPUT_LOCATION_CD);
                if (ecDetails.MACHINE_CD != null && ecDetails.MACHINE_CD > 0)
                {
                    ecDetails.LOCATION_CD = Convert.ToInt32(_commonRepository.GetMachineNameByID(ecDetails.MACHINE_CD.Value)?.LOCATION_CD);
                }
                ecDetails.PROCESS_KUBUN_ID = Convert.ToInt16(UIUtils.GetProcessKubun(_localization.GetLocalizedHtmlString("lbl_ProcessEC")));

                WipStatusDetails wipDetails = new WipStatusDetails();

                wipDetails = new WipStatusDetails();
                wipDetails.PROCESS_PLAN_CD = ecDetails.PROCESS_PLAN_CD;
                wipDetails.LOCATION_CD = Convert.ToInt16(ecDetails.LOCATION_CD);
                wipDetails.MSG_CD = null;
                wipDetails.FINISHED_BARS = null;
                wipDetails.UNIT = null;
                wipDetails.FINISHED_QTY = null;
                wipDetails.PROCESS_START_DATE = ecDetails.EC_START_DATE;
                wipDetails.PROCESS_END_DATE = null;
                wipDetails.PROCESS_KUBUN_CD = ecDetails.PROCESS_KUBUN_ID;
                wipDetails.PRE_PROSESS_DECISION = null;
                wipDetails.NEXT_PROCESS = null;
                wipDetails.START_OPERATOR_CD = ecDetails.EC_START_OPERATOR_CD > 0 ? ecDetails.EC_START_OPERATOR_CD : null;
                wipDetails.END_OPERATOR_CD = null;
                wipDetails.TABA_NO = ecDetails.EC_TABA_NO;
                wipDetails.NIFUDA_CD = ecDetails.NIFUDA_CD;

                _commonRepository.DeleteWIPStatusData(wipDetails);

                var result = _eCRepository.InsertECDetails(ecDetails);
                if (result != null)
                {
                    if (result.PP_UNIT != null)
                    {
                        result.PP_NO_OF_UNITS_STRING = Convert.ToDecimal(result.PP_NO_OF_UNITS).ToString("0.####") + " " + UIUtils.GetProductUnitKubunValue(Convert.ToString(result.PP_UNIT));
                    }

                    if (result.MAGARI_KUBUN != null)
                    {
                        result.PP_MAGARI_BEND_TOL = Utils.GetMagariBendTolSpan(UIUtils.Instance.GetMagariKubunValue(Convert.ToString(result.MAGARI_KUBUN)), Convert.ToDecimal(result.MAGARI_BEND_TOL), Convert.ToInt32(result.MAGARI_SPAN));  //Convert.ToDecimal(obj.PP_NO_OF_UNITS).ToString("0.####") + UIUtils.Instance.GetProductUnitKubunValue(Convert.ToString(obj.PP_UNIT));
                    }

                    if (result.PP_STD_DEL_PERIOD >= 0)
                    {
                        result.PP_STD_DEL_PERIOD_STRING = result.PP_STD_DEL_PERIOD + " " + _localization.GetLocalizedHtmlString("lbl_Day");
                    }
                    
                    if (result.PP_UNIT_WT_KUBUN > 0)
                    {
                        result.PP_UNIT_WT_STRING = Utils.GetUnitWeigth(Convert.ToInt16(result.PP_UNIT_WT_KUBUN), Convert.ToInt32(result.PP_UNIT_WT_STRING), Convert.ToInt16(result.PP_UNIT_WT_MIN), result.PP_UNIT_WT_MAX);

                    }
                }
                transaction.Complete();
                return result;
            }
        }

        public long UpdateECDetails(ECDetails request)
        {
            long msgCD=0;
            bool deleteMsg = false;

            int decisionVal = UIUtils.GetResultKubunKey(_localization.GetLocalizedHtmlString("lbl_OK"));
            
            if (request.USER_ID != null && request.USER_ID > 0)
            {
                request.INPUT_LOCATION_CD = _commonRepository.GetUserLocationById(request.USER_ID.Value);
            }
            // request.LOCATION_CD = Convert.ToInt32(request.INPUT_LOCATION_CD);
            if (request.MACHINE_CD != null && request.MACHINE_CD > 0)
            {
                request.LOCATION_CD = Convert.ToInt32(_commonRepository.GetMachineNameByID(request.MACHINE_CD.Value)?.LOCATION_CD);
            }
            using (TransactionScope transaction = new TransactionScope())
            {
                
                var finalInspectionDetails = new FinalInspectionDetails();
                finalInspectionDetails.PROCESS_PLAN_CD = request.PROCESS_PLAN_CD;
                finalInspectionDetails.NIFUDA_CD = request.NIFUDA_CD;
                finalInspectionDetails.INSPECTED_BY = Convert.ToInt32(request.EC_END_OPERATOR_CD);
                finalInspectionDetails.FI_DIM1 = Convert.ToDecimal(request.EC_ACT_DIM1);
                finalInspectionDetails.FI_DIM2 = Convert.ToDecimal(request.EC_ACT_DIM2);
                finalInspectionDetails.FI_DIM3 = request.EC_ACT_DIM3;
                finalInspectionDetails.FI_DIM4 = request.EC_ACT_DIM4;
                finalInspectionDetails.FI_ACT_LENGTH = Convert.ToDecimal(request.EC_ACT_LENGTH);
                finalInspectionDetails.FI_TABA_NO = request.EC_TABA_NO;
                finalInspectionDetails.NO_OF_BARS = Convert.ToDecimal(request.EC_OK_BARS);
                finalInspectionDetails.UNIT_TYPE = request.UNIT_TYPE;
                finalInspectionDetails.FI_QTY = Convert.ToDecimal(request.EC_OK_QTY);
                finalInspectionDetails.FI_DIM_DECISION = request.FI_DIM_DECISION;
                finalInspectionDetails.FI_LENGTH_DECISION = request.FI_LENGTH_DECISION;
                finalInspectionDetails.COMBINED_DECISION = request.COMBINED_DECISION;
                finalInspectionDetails.FI_NEXT_PROCESS = Convert.ToInt16(request.FI_NEXT_PROCESS);
                finalInspectionDetails.FI_COLOR_DECISION = request.FI_COLOR_DECISION;
                finalInspectionDetails.FI_CHAMFERING_DECISION = request.FI_CHAMFERING_DECISION;
                finalInspectionDetails.FI_EXT_TEXTURE_DECISION = request.FI_EXT_TEXTURE_DECISION;
                finalInspectionDetails.USER_ID = Convert.ToInt32(request.USER_ID);
                finalInspectionDetails.LOCATION_CD = Convert.ToInt32(request.LOCATION_CD);
                finalInspectionDetails.INPUT_LOCATION_CD = Convert.ToInt32(request.INPUT_LOCATION_CD);
                finalInspectionDetails.INSP_PROCESS_KUBUN_CD= Convert.ToInt16(UIUtils.GetProcessKubun(_localization.GetLocalizedHtmlString("lbl_ProcessCompletedInspection")));
                finalInspectionDetails.EC_CD = request.EC_CD;
                //IF FI checkbox is checkedthen first save fi.
                if (Convert.ToInt16(request.FINAL_INSP) == 1 && request.IS_FI_CHECK == 1)
                {
                   finalInspectionDetails = _eCRepository.Insert(finalInspectionDetails);
                }
                else if (Convert.ToInt16(request.FINAL_INSP) == 0 && request.IS_FI_CHECK == 1)
                {
                    _eCRepository.DeleteFinalInspection(Convert.ToInt64(request.EC_CD));
                    request.FINAL_INSP = 0;
                }
                else if (Convert.ToInt16(request.FINAL_INSP) > 0 && request.IS_FI_CHECK == 0)
                {
                    _eCRepository.UpdateFinalInspection(finalInspectionDetails);
                }

                MessageDetails messageDetails = new MessageDetails();
                messageDetails.PROCESS_PLAN_CD = Convert.ToInt64(request.PROCESS_PLAN_CD);
                messageDetails.MACHINE_CD = Convert.ToInt32(request.MACHINE_CD);
                messageDetails.PROCESS_KUBUN_ID = Convert.ToInt32(UIUtils.GetProcessKubun(_localization.GetLocalizedHtmlString("lbl_ProcessEC")));
                messageDetails.OPERATOR_CD = Convert.ToInt64(request.EC_END_OPERATOR_CD);
                messageDetails.MSG_CONTENTS = request.EC_REMARKS;
                messageDetails.USER_ID = Convert.ToInt32(request.USER_ID);
                messageDetails.LOCATION_CD = request.LOCATION_CD;
                messageDetails.INPUT_LOCATION_CD = request.INPUT_LOCATION_CD;
                if (!string.IsNullOrEmpty(request.EC_REMARKS) && request.MSG_CD == null)
                {
                   
                    messageDetails = _commonRepository.MessageDetailsInsert(messageDetails);


                    if (messageDetails.MSG_CD != Constants.NullValue.LONG && messageDetails.MSG_CD > 0)
                    {
                        request.MSG_CD = messageDetails.MSG_CD;
                    }
                                     
                }
             
                else if (request.MSG_CD > 0 && !string.IsNullOrEmpty(request.EC_REMARKS))
                {
                    messageDetails.MSG_CD = Convert.ToInt64(request.MSG_CD);
                    _commonRepository.MessageDetailsUpdate(messageDetails);

                }
                else if (request.MSG_CD > 0 && string.IsNullOrEmpty(request.EC_REMARKS))
                {
                    
                    long messageCD = Convert.ToInt64(request.MSG_CD);
                    request.MSG_CD = Constants.NullValue.LONG;
                    
                    msgCD = _eCRepository.UpdateECDetails(request);
                    _commonRepository.MessageDetailsDelete(messageCD);

                    deleteMsg = true;
                }

                if (deleteMsg == false)
                {
                    msgCD = _eCRepository.UpdateECDetails(request);
                }
               


                if (request.EC_NEXT_PROCESS != 999)
                {

                    WipStatusDetails wipDetails = new WipStatusDetails();
                    wipDetails = new WipStatusDetails();
                    wipDetails.PROCESS_PLAN_CD = request.PROCESS_PLAN_CD;
                    wipDetails.LOCATION_CD = Convert.ToInt16(request.LOCATION_CD);
                    if (msgCD > 0)
                    {
                        wipDetails.MSG_CD = msgCD;
                    }
                    wipDetails.FINISHED_BARS = request.EC_OK_BARS;
                    wipDetails.UNIT = request.UNIT_TYPE;
                    wipDetails.FINISHED_QTY = request.EC_OK_QTY;
                    wipDetails.PROCESS_START_DATE = request.EC_START_DATE;
                    wipDetails.PROCESS_END_DATE = request.EC_END_DATE;
                   
                   
                    if (request.FINAL_INSP == 1)
                    {
                        wipDetails.PROCESS_KUBUN_CD = Convert.ToInt32(UIUtils.GetProcessKubun(_localization.GetLocalizedHtmlString("lbl_ProcessCompletedInspection")));
                        wipDetails.PRE_PROSESS_DECISION = request.COMBINED_DECISION;
                        wipDetails.NEXT_PROCESS = request.FI_NEXT_PROCESS;
                    }
                    else
                    {
                        wipDetails.PROCESS_KUBUN_CD = Convert.ToInt32(UIUtils.GetProcessKubun(_localization.GetLocalizedHtmlString("lbl_ProcessEC")));
                        wipDetails.PRE_PROSESS_DECISION = request.EC_DECISION;
                        wipDetails.NEXT_PROCESS = request.EC_NEXT_PROCESS;
                    }

                    wipDetails.START_OPERATOR_CD = request.EC_START_OPERATOR_CD > 0 ? request.EC_START_OPERATOR_CD : null;
                    wipDetails.END_OPERATOR_CD = request.EC_END_OPERATOR_CD > 0 ? request.EC_END_OPERATOR_CD : null; ;
                    wipDetails.TABA_NO = request.EC_TABA_NO;
                    wipDetails.NIFUDA_CD = request.NIFUDA_CD;

                    _commonRepository.DeleteWIPStatusData(wipDetails);
                }

                transaction.Complete();
               
            }
            return msgCD;
        }

        public void Delete(ECDetails request)
        {
            using (TransactionScope transactionScope = new TransactionScope())
            {
                WipStatusDetails wipDetails = new WipStatusDetails();
                wipDetails = new WipStatusDetails();
                wipDetails.PROCESS_PLAN_CD = request.PROCESS_PLAN_CD;
                wipDetails.TABA_NO = request.EC_TABA_NO;
                if (request.EC_CD > 0)
                {
                    _eCRepository.DeleteFinalInspection(Convert.ToInt64(request.EC_CD));
                    _eCRepository.Delete(Convert.ToInt64(request.EC_CD));
                   
                }
                _commonRepository.DeleteWIPStatusData(wipDetails);
                if (request.MSG_CD > 0)
                {
                    _commonRepository.MessageDetailsDelete(Convert.ToInt64(request.MSG_CD));
                }
               
                transactionScope.Complete();
            }

        }
        public string GetFinalInspByECCD(long ecCD)
        {
            return  _eCRepository.GetFinalInspData(ecCD);
        }
        public ECDetails GetECDetailsByECCD(long ecCD, string cultureName)
        {
            if (cultureName == "en")
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
            }
            else
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("ja-JP");
            }

            var obj = _eCRepository.GetECDetailsByECCD(ecCD);

            if (obj != null)
            {

                if (obj.PP_STD_DEL_PERIOD >= 0)
                {
                    obj.PP_STD_DEL_PERIOD_STRING = obj.PP_STD_DEL_PERIOD + " " + _localization.GetLocalizedHtmlString("lbl_Day");
                }

                if (obj.PP_UNIT > 0)
                {
                    obj.PP_NO_OF_UNITS_STRING = Convert.ToDecimal(obj.PP_NO_OF_UNITS).ToString("0.####") + UIUtils.GetProductUnitKubunValue(Convert.ToString(obj.PP_UNIT));
                }

                if (obj.PP_UNIT_WT_KUBUN > 0)
                {
                    obj.PP_UNIT_WT_STRING = Utils.GetUnitWeigth(Convert.ToInt16(obj.PP_UNIT_WT_KUBUN), Convert.ToInt32(obj.PP_UNIT_WT_STRING), Convert.ToInt16(obj.PP_UNIT_WT_MIN), obj.PP_UNIT_WT_MAX);

                }

                if (obj.MAGARI_KUBUN > 0)
                {
                    obj.PP_MAGARI_BEND_TOL = Utils.GetMagariBendTolSpan(UIUtils.Instance.GetMagariKubunValue(Convert.ToString(obj.MAGARI_KUBUN)), Convert.ToDecimal(obj.MAGARI_BEND_TOL), Convert.ToInt32(obj.MAGARI_SPAN));
                }

                //if (obj.PP_COLOR_ID > 0)
                //{
                //    obj.PP_COLOR = UIUtils.GetColorKubunValue(Convert.ToString(obj.PP_COLOR_ID));   // Utils.GetColorName(Convert.ToInt16(_stamppingDetails.PP_COLOR_ID));
                //}
                if (obj.PP_COIL_DIA_WINDING > 0)
                {
                    obj.PP_COIL_DIA_WINDING_STRING = UIUtils.GetCoilDiaWindingKubunValue(Convert.ToString(obj.PP_COIL_DIA_WINDING));
                }
                obj.DECISION_OK = Convert.ToInt16(UIUtils.GetResultKubunKey(_localization.GetLocalizedHtmlString("lbl_OK")));

            }

            return obj;
        }

        public List<ECDetails> GetECList(GridSearchFields gridSearchRequest)
        {
            short locationCD = Convert.ToInt16(_commonRepository.GetUserLocationById(Convert.ToInt32(gridSearchRequest.USER_ID)));
            List<ECDetails> result = new List<ECDetails>();
            List<SearchField> searchFields = ComposeSearchCriteria(gridSearchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);
            result = _eCRepository.GetECDetailsList(searchFields, selectedSortCriteria, locationCD);
            return result;
        }
        public List<ECDetails> GetECDailyReportOnFactoryDailyWorkStatus(GridSearchFields gridSearchRequest)
        {           
            List<SearchField> searchFields = ComposeSearchCriteriaforFactory(gridSearchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);
            int rowLimit = gridSearchRequest.ROW_LIMIT;
            var obj = _eCRepository.GetECDailyReportOnFactoryDailyWorkStatus(searchFields, selectedSortCriteria, rowLimit);            
            return obj;
        }
        private List<SearchField> ComposeSearchCriteriaforFactory(GridSearchFields gridSearchRequest)
        {
            var searchFields = new List<SearchField>();
            if (gridSearchRequest.MACHINE_CD > 0 )
            {
                var searchField = new SearchField
                {
                    FieldName = "EC.MACHINE_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MACHINE_CD)
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.LOCATION_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "EC.LOCATION_CD",
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
                    FieldName = "EC.EC_END_OPERATOR_CD",
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
                    SearchText1 ="'" + gridSearchRequest.PROD_SPEC_NO1 +"'"
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_SPEC_NO2))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_SPEC_NO2",
                    SearchType = SearchType.Number,
                    SearchText1 = "'" + gridSearchRequest.PROD_SPEC_NO2 +"'"
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
                    SearchText1 =  Convert.ToString(gridSearchRequest.PROD_DIM_D_MIN),
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
            else if (gridSearchRequest.PROD_LENGTH_MIN > 0  && gridSearchRequest.PROD_LENGTH_MAX >0)
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
                        FieldName = "EC.EC_END_DATE",
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
                        FieldName = "EC.EC_END_DATE",
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
                    FieldName = "EC.EC_END_DATE",
                    SearchType = SearchType.DateTime,
                    SearchText1 = gridSearchRequest.INSPECTION_DATE1,
                    SearchText2 = gridSearchRequest.INSPECTION_DATE2
                };
                searchFields.Add(searchField);
            }

            return searchFields;
        }

        private List<SearchField> ComposeSearchCriteria(GridSearchFields gridSearchRequest)
        {
            var searchFields = new List<SearchField>();

            if (!string.IsNullOrEmpty(gridSearchRequest.CHUUBAN))
            {

                var searchField = new SearchField
                {
                    FieldName = "CONCAT(PP.CHUUBAN,CASE WHEN LENGTH(TRIM(PP.GYOUBAN))=1 THEN '0'||PP.GYOUBAN END)",//"PP.CHUUBAN",
                    SearchType = SearchType.Text,

                    SearchText1 = gridSearchRequest.CHUUBAN

                };

                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.CUSTOMER_NAME))
            {
                var searchField = new SearchField
                {
                    FieldName = "CST.SHORT_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.CUSTOMER_NAME.ToLower().Trim()
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
                    SearchCondition =  SearchCondition.Equal.ToString(),
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
                    SearchText1 = gridSearchRequest.PROD_DIM_D_MIN.ToString(),
                    SearchText2 = gridSearchRequest.PROD_DIM_D_MAX.ToString()

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
                    SearchText1 = gridSearchRequest.PROD_LENGTH_MIN.ToString(),
                    SearchText2 = gridSearchRequest.PROD_LENGTH_MAX.ToString()
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.START_DATETIME))
            {
                var searchField = new SearchField
                {
                    FieldName = "ST.STR_START_DATE",
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


        public string IsCheckEmployeeActiveInctiveByID(long opratorCd)
        {
            var data = (opratorCd > 0) ? _eCRepository.GetOperatorDetailsById(opratorCd) : null;
            if (data == null)
            {
                return null;
            }
            else
            {
                string inActive = data.IN_ACTIVE_FLAG == 1 ? "inactive" : "active";
                return inActive;   
            }
        }

        public List<ECDetails> GetDailyProcessAnalysisECDeatils(GridSearchFields gridSearchRequest)
        {

            List<SearchField> searchFields = ComposeSearchCriteriaforDailyProcessECDetails(gridSearchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);
            var result = _eCRepository.GetDailyProcessECDetails(searchFields, selectedSortCriteria, gridSearchRequest);
            if (result != null)
            {
                foreach (var item in result)
                {
                    if(Convert.ToInt32(item.EC_NG_BAR_TOTAL) > 0)
                    {
                        item.EC_NG_PERCENTAGE_STRING = Convert.ToString(Convert.ToDecimal(item.EC_NG_BAR_TOTAL / (item.EC_OK_BARS + item.EC_NG_BAR_TOTAL) * 100).ToString("0.00"));
                    }
                }
            }
            return result;
        }

        private List<SearchField> ComposeSearchCriteriaforDailyProcessECDetails(GridSearchFields gridSearchRequest)
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
                    SearchText1 ="'" + gridSearchRequest.PROD_SPEC_NO2 + "'"
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(Convert.ToString(gridSearchRequest.PROD_VER)))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.VER",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.PROD_VER)
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

            //EC search
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
                    FieldName = "EC.EC_START_DATE",
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
                    FieldName = "EC.EC_METALLIC_PLATE_CODE",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.METALLIC_PLATE_CODE_STRING.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.FURNACE_NO))
            {
                var searchField = new SearchField
                {
                    FieldName = "EC.FURNACE_NO",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.FURNACE_NO.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.END_DATE1))
            {
                var searchField = new SearchField
                {
                    FieldName = "EC.EC_END_DATE",
                    SearchType = SearchType.DateTime,
                    SearchText1 = gridSearchRequest.END_DATE1,
                    SearchText2 = gridSearchRequest.END_DATE2
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(Convert.ToString(gridSearchRequest.END_OPERATOR)))
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
                    FieldName = "EC.EC_DECISION",
                    SearchType = SearchType.Text,
                    SearchText1 = Convert.ToString(gridSearchRequest.DECISION)
                };
                searchFields.Add(searchField);
            }
            return searchFields;
        }
        public List<NameValue> GetTPTestList()
        {
            var _tpTestList = UIUtils.GetTPTestKubunList();
            return _tpTestList;
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
        public List<NameValue> GetFIResultList()
        {
            var _TrialResultList = UIUtils.GetFIResultKubun();
            return _TrialResultList;
        }
        public List<NameValue> GetProductUnitKubun()
        {
            var _prod_kubun = UIUtils.Instance.GetProductUnitKubun();
            return _prod_kubun;
        }

        public int GetRowCountForECFactory(GridSearchFields gridSearchRequest)
        {
            List<SearchField> searchFields = ComposeSearchCriteriaforFactory(gridSearchRequest);
            int count = _eCRepository.GetRowCountForECFactory(searchFields);
            return count;
        }

        public int GetRowCountForECDailyAnalysis(GridSearchFields gridSearchRequest)
        {
            List<SearchField> searchFields = ComposeSearchCriteriaforDailyProcessECDetails(gridSearchRequest);
            int count = _eCRepository.GetRowCountForECDailyAnalysis(searchFields);
            return count;
        }
        public string GetHardnessDescion(long processPlanCD, string cultureName)
        {
             
            //ResultDecisionFIGet
            string decision = string.Empty;
            if (cultureName == "en")
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
            }
            else
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("ja-JP");
            }
            var result = _eCRepository.GetHardnessDescion(processPlanCD);
            if (result.Count == 0)
            {
                decision = Convert.ToString(_localization.GetLocalizedHtmlString("lbl_NA1"));
            }
            else
            {
                foreach(var item in result)
                {
                    item.DECISION_STRING = UIUtils.GetResultKubunValue(Convert.ToString(item.DECISION));
                }
                //decision = result.GroupBy(r => r.TABA_NO).All(g => g.Any(x => x.DECISION == decisionOK)) ? _localization.GetLocalizedHtmlString("lbl_OK") : result.GroupBy(r => r.TABA_NO).Any(g => g.Any(x => x.DECISION == decisionNG) &&
                //               !g.Any(x => x.DECISION == decisionOK)) ? _localization.GetLocalizedHtmlString("lbl_NG") : _localization.GetLocalizedHtmlString("lbl_Hold");
                
                decision = result.Any(x => x.DECISION_STRING != _localization.GetLocalizedHtmlString("lbl_OK")) ? result.First(x => x.DECISION_STRING != _localization.GetLocalizedHtmlString("lbl_OK")).DECISION_STRING: _localization.GetLocalizedHtmlString("lbl_OK");
            }
            return decision;
        }
        public string GetTensileDescion(long processPlanCD, string cultureName)
        {
            string decision = string.Empty;
            if (cultureName == "en")
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
            }
            else
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("ja-JP");
            }

            var result =  _eCRepository.GetTensileDescion(processPlanCD);
            if (result.Count == 0)
            {
                decision = Convert.ToString(_localization.GetLocalizedHtmlString("lbl_NA1"));
            }
            else
            {
                foreach (var item in result)
                {
                    item.DECISION_STRING = UIUtils.GetResultKubunValue(Convert.ToString(item.DECISION));
                }

                decision = result.Any(x => x.DECISION_STRING != _localization.GetLocalizedHtmlString("lbl_OK")) ? result.First(x => x.DECISION_STRING != _localization.GetLocalizedHtmlString("lbl_OK")).DECISION_STRING : _localization.GetLocalizedHtmlString("lbl_OK");
            }
            return decision;

        }

    }
}
