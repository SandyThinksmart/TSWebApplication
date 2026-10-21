using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;

namespace TSWebApplication.Infrastructure.IRepositories
{
    public interface IHikinukiBarToBarRepository
    {
        List<HikinukiBarToBarDetails> GetHikinukiBarToBarList(List<SearchField> searchFields, string sortCiteria, short locationCD);
        List<ThabaDetails> GetThabaDetails(string chuuban, int gyuban);
        HikinukiBarToBarDetails GetHikinukiBarToBarDetailsByChuuban(string chuuban, short? gyouban, int? mechineCD);
        List<HikinukiBarToBarDetails> GetHikinukiBarToBar24MachineList(string machineCd);
        void SaveHikinukiBarToBar(HikinukiBarToBarDetails request);
        //void WIPStatusCheckAndDeleteProcessCDandKubunCD(long? processPlanCD, long PROCESS_KUBUN_CD);
       // void InsertWIPStatusHikinukiBarToBar(HikinukiBarToBarDetails request);
        long HikinukiBarToBarDetailsUpdate(HikinukiBarToBarDetails request);
        HikinukiBarToBarDetails GetHikinukiBarToBarDetailsByDrawingCD(long drawingCD);
        List<HikinukiBarToBarDetails> GetDrawingBarToBarDataOnFactoryDailyWorkStatus(List<SearchField> searchFields, string sortCriteria,int rowLimit,string cultureName);
        public List<HikinukiBarToBarDetails> GetDailyProcessHikinukiBarToBar(List<SearchField> searchFields, string sortCriteria, GridSearchFields gridresult);
        void Delete(long DRAWING_CD);
        public HikinukiBarToBarDetails GetThabaDetailsByThabaNumber(string chuuban, short? gyouban, short? ThabaNo);

        int GetRowCountForHikinukiBarToBarFactory(List<SearchField> searchFields);
        int GetRowCountForHikinukiBarToBarDailyAnalysis(List<SearchField> searchFields);
        List<NameValue> GetHikinukiBarToBarMachineList();
    }
}
