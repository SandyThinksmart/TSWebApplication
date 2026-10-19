using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;

namespace TSWebApplication.Application.IServices
{
    public interface IHikinukiBarToBarService
    {

        List<HikinukiBarToBarDetails> GetHikinukiBarToBarList(GridSearchFields gridSearchRequest);
        List<ThabaDetails> GetThabaDetailsByChuuban(string chuuban, int gyouban);
        HikinukiBarToBarDetails GetHikinukibartobarDetailsByChuuban(string chubaan, short? gyouban, int? mechineCD, string culterName);
        List<HikinukiBarToBarDetails> GetHikinukiBarToBar24MachineList(string machineCd, string cultureName);
        HikinukiBarToBarDetails SaveHikinukiBarToBar(HikinukiBarToBarDetails request);
        long HikinukiBarToBarDetailsUpdate(HikinukiBarToBarDetails request);
        HikinukiBarToBarDetails GetHikinukiDetailsDrawingByCD(long drawingCD, string cultureName);
        void Delete(HikinukiBarToBarDetails request);
        List<HikinukiBarToBarDetails> GetDrawingBarToBarDataOnFactoryDailyWorkStatus(GridSearchFields gridSearchRequest);
        public List<HikinukiBarToBarDetails> GetDailyProcessAnalysisHikinukiBarToBar(GridSearchFields gridSearchRequest);
        List<NameValue> GetDecisionList();
        List<NameValue> GetDieDiaShapeList();
        List<NameValue> GetExteriorConditionList();
        List<NameValue> GetBendStatusList();
        List<NameValue> GetHTMaterialList();
        List<NameValue> GetUnitTypeList();
        public HikinukiBarToBarDetails GetThabaDetailsByThabaNumber(string chuuban, short? gyouban, short? ThabaNo);

        int GetRowCountForHikinukiBarToBarFactory(GridSearchFields gridresult);
        int GetRowCountForHikinukiBarToBarDailyAnalysis(GridSearchFields gridresult);

        List<NameValue> GetHikinukiBarToBarMachineList();

    }
}
