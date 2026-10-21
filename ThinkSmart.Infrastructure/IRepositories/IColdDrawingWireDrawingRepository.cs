using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;

namespace TSWebApplication.Infrastructure.IRepositories
{
    public interface IColdDrawingWireDrawingRepository
    {
        List<ColdDrawingWireDrawingDetails> GetColdDrawingWireDrawingList(List<SearchField> searchFields, string sortCiteria, short? locationCD);
        ColdDrawingWireDrawingDetails GetColdDrawingWireDrawingDetailsByChuuban(string chuuban, short? gyouban, int? machineCD);
          
        void InsertColdDrawingWireDrawingDetails(ColdDrawingWireDrawingDetails request);
      //  int GetUserLocationById(int userID);
        long UpdateColdDrawingWireDrawingDetails(ColdDrawingWireDrawingDetails request);
        List<ColdDrawingWireDrawingDetails> GetColdDrawingWireDrawing24MachineList(string machineCd);
        ColdDrawingWireDrawingDetails GetColdDrawingWireDrawingDetailsByDrawingCD(long drawingCD);
        List<NameValue> GetFilteredEmployeeNameList(string searchText, bool addEmpty);
        void DeleteColdDrawingWireDrawingDetails(long drawingCD);
        List<ThabaDetails> GetThabaDetails(string chuuban, short? gyuban);

        List<ColdDrawingWireDrawingDetails> GetColdDrawingList(int locationCd);
        public List<ColdDrawingWireDrawingDetails> GetThabaDetailsByThabaNumber(string chuuban, short? gyouban, string Thaba_No_String);

        public List<ColdDrawingWireDrawingDetails> GetWireDrawingDailyReportOnFactoryDailyWorkStatus(List<SearchField> searchFields, string sortCriteria,int rowLimit);
        public List<ColdDrawingWireDrawingDetails> GetDailyProcessColdDrawingW(List<SearchField> searchFields, string sortCriteria, GridSearchFields gridresult);

        int GetRowCountForWireDrawingFactory(List<SearchField> searchFields);
        int GetRowCountForColdDrawingCoilToCoilDailyAnalysis(List<SearchField> searchFields);

    }
}
