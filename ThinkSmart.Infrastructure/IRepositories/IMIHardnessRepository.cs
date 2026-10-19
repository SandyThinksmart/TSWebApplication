using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;

namespace TSWebApplication.Infrastructure.IRepositories
{
    public interface IMIHardnessRepository
    {
        MIHardnessDetails GetHardnessDetailsByChuuban(string chuuban, short? gyouban);
        public void InsertHardnessDetails(MIHardnessDetails hardnessDetails);
     //   public int GetUserLocationById(int userId);
        List<ThabaDetails> GetThabaDetailsByChuuban(string chuuban, short? gyouban, long? MIHardnessCD);
        List<MIHardnessDetails> GetMIHardnessList(List<SearchField> searchFields, string sortCiteria, short? locationCD);
        MIHardnessDetails GetHDdetailsbyHRDCD(long? hardnessCD, string chuuban, short? gyouban);

        long UpdateHardnessDetails(MIHardnessDetails request);
        List<ThabaDetails> CheckThabaExistorNot(string chuuban, short? gyouban, long? MIHardnessCD);
        string DeleteHardnessDetails(long hardnessCD);
        public List<MIHardnessDetails> GetThabaDetailsByThabaNumber(string chuuban, short? gyouban, long ThabaNo);
        public List<MIHardnessDetails> GetMechHardnessOnFactoryDailyWorkStatus(List<SearchField> searchFields, string SortCriteria,int rowLimit);
        public List<MIHardnessDetails> GetMechHardnessOnDailyProcessStatus(List<SearchField> searchFields,string SortCriteria, GridSearchFields gridresult);

        int GetRowCountForMechHardnessFactory(List<SearchField> searchFields);
        int GetRowCountForMechHardnessDailyAnalysis(List<SearchField> searchFields);
        public MIHardnessDetails GetFurnanceNo(string chuuban, short? gyouban, int detail_No);
        List<MIHardnessDetails> GetFICombinedDecisionDetails(string chuuban, short? gyouban);
        void UpdateFICombinedDecision(short hdDecision,long processPlanCd);
        string ValidateHardnessDetailsOnUpdateBtn(long MIHardnessCD);
    }
}
