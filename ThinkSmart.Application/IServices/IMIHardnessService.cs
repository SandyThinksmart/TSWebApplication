using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;

namespace TSWebApplication.Application.IServices
{
    public interface IMIHardnessService
    {
        List<NameValue> SetHardnessKubunsList();
        List<NameValue> GetNextProcessList();
        List<NameValue> SetDecisionList();

        public string GetDecisionValue(string decisionvalue);
        MIHardnessDetails GetHardnessDetailsByChuuban(string chuuban, short? gyouban, string culture_Name);

        public void SaveHardnessData(MIHardnessDetails request);

        public int GetUserLocationById(int userId);
        List<ThabaDetails> GetThabaDetailsByChuuban(string chuuban, short? gyouban, long? MIHardnessCD);

        //MessageDetails MessageDetailsInsert (MessageDetails request);
        List<MIHardnessDetails> GetMIHardnessdetailsList(GridSearchFields gridSearchRequest);

        MIHardnessDetails GetMIHardnessDetailsbyHardnessCD(long? drawingCD, string chuuban, short? gyouban, string culture_Name);

        long UpdateHardnessDetails(MIHardnessDetails request);
        List<ThabaDetails> CheckThabaExistorNot(string chuuban, short? gyouban, long? MIHardnessCD);

        string HardnessDetailsDelete(MIHardnessDetails request);

        public List<MIHardnessDetails> GetThabaDetailsByThabaNumber(string chuuban, short? gyouban, long ThabaNo);
        public List<MIHardnessDetails> GetMechHardnessOnFactoryDailyWorkStatus(GridSearchFields gridSearchRequest);
        public List<MIHardnessDetails> GetMechHardnessOnDailyProcessStatus(GridSearchFields gridSearchRequest);
        int GetRowCountForMechHardnessFactory(GridSearchFields gridresult);
        int GetRowCountForMechHardnessDailyAnalysis(GridSearchFields gridresult);
        public MIHardnessDetails GetFurnanceNo(string chuuban, short? gyouban, int detail_No);
        public List<MIHardnessDetails> GetFICombinedDecisionDetails(string chuuban, short? gyouban);
        void UpdateFICombinedDecision(short hdDecision,long processPlanCd);
        public string ValidateHardnessDetailsOnUpdateBtn(long MIHardnessCD);
    }
}
