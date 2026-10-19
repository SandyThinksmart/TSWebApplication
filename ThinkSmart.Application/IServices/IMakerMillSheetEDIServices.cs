using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;

namespace TSWebApplication.Application.IServices
{
    public interface IMakerMillSheetEDIServices
    {
        //MakerMillSheetDetails ReadCSVFile(string filename);
        void MillSheetEDILogDataInsert(List<MakerMillSheetEDIDetails> makerMillSheetDetails, string cultureName, int userID);

        public List<MakerMillSheetEDIDetails> GetMakerMillSheetProcessRptData(int runID, DateTime printReportDate);

        public (int RunID, List<MakerMillSheetEDIDetails> ErrorData) GetMakerMillSheetErrorRptData(int runID, DateTime printReportDate);
       
    }
}
