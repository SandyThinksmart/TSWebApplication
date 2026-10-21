using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;

namespace TSWebApplication.Application.IServices
{
    public interface IColdDrawingWireDrawingService

    {
        //List<ColdDrawingWireDrawingDetails> GetColdDrawingWireDrawingList(ColdDrawingWireDrawingGridSearchRequest gridSearchRequest);
        List<ColdDrawingWireDrawingDetails> GetColdDrawingWireDrawingdetailList(GridSearchFields gridSearchRequest);
        ColdDrawingWireDrawingDetails GetColdDrawingWireDrawingDetailsByChuuban(string chuuban, short? gyouban, int? machineCD,string cultureName);

        ColdDrawingWireDrawingDetails InsertColdDrawingWireDrawingDetails(ColdDrawingWireDrawingDetails request);
        int GetUserLocationById(int userID);
        // List<ColdDrawingWireDrawingDetails> GetCDList(ColdDrawingWireDrawingGridSearchRequest gridSearchRequest);


        List<NameValue> GetExteriorConditionList();
        List<NameValue> GetMDustList();
        List<NameValue> GetDiceInspStartList();
        List<NameValue> GetDiceInspEndList();
        List<NameValue> GetwindingShapeList();
        List<NameValue> GetDieDiaShapeList();
        List<NameValue> GetDecisionList();
        long UpdateColdDrawingWireDrawingDetails(ColdDrawingWireDrawingDetails request);
        List<ColdDrawingWireDrawingDetails> GetColdDrawingWireDrawing24MachineList(string machineCd);
        ColdDrawingWireDrawingDetails GetColdDrawingWireDrawingDetailsByDrawingCD(long drawingCD,string culture);
        List<NameValue> GetFilteredEmployeeNameList(string searchText, bool addEmpty);
        void DeleteColdDrawingWireDrawingDetails(ColdDrawingWireDrawingDetails result);
        List<ThabaDetails> GetThabaDetails(string chuuban, short? gyuban);
        List<ColdDrawingWireDrawingDetails> GetColdDrawingList(int locationCd);
        string GetThabaDetailsByThabaNumber(GridSearchFields request);
        
        public List<ColdDrawingWireDrawingDetails> GetWireDrawingDailyReportOnFactoryDailyWorkStatus(GridSearchFields gridresult);
        public List<ColdDrawingWireDrawingDetails> GetDailyProcessAnalysisColdDrawingW(GridSearchFields gridSearchRequest);

        int GetRowCountForWireDrawingFactory(GridSearchFields gridresult);
        int GetRowCountForColdDrawingCoilToCoilDailyAnalysis(GridSearchFields gridresult);
    }
}
