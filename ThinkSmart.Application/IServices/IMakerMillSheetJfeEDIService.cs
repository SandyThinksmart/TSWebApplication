using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Domain.Model;

namespace TSWebApplication.Application.IServices
{
    public interface IMakerMillSheetJfeEDIService
    {
        void MillSheetJfeEDILogDataInsert(List<MakerMillSheetEDIDetails> makerMillSheetDetails, string cultureName, int userID);

        public List<MakerMillSheetEDIDetails> GetMakerMillSheetProcessRptData(int runID, DateTime printReportDate);

        public (int RunID, List<MakerMillSheetEDIDetails> ErrorData) GetMakerMillSheetErrorRptData(int runID, DateTime printReportDate);
    }
}
