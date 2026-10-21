using System;
using System.Collections.Generic;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;


namespace TSWebApplication.Application.IServices
{
    public interface IWIPTensouService
    {
        WIPTensouDetails GetTensouDetailsByChuubanGyouban(string nifuda ,string culture);
        List<NameValue> GetNextProcessList();
        List<NameValue> GetProductUnitKubun();
        List<NameValue> GetTensouLocation(int USER_ID);
        List<NameValue> GetForwordingLocation();
        List<WIPTensouDetails> SaveWIPTensouDetails(List<WIPTensouDetails> request);
        List<WIPTensouDetails> GetTensouDataSearchList(GridSearchFields request);
        // object GetTensouLocation();
        List<WIPTensouDetails> GetWIPTransferDetailsByTrasferCD(long WIP_TRANSFER_CD,string culture);
        List<WIPTensouDetails> IsAcceptanceCompleted(long WIP_TRANSFER_CD,string culture);
        List<WIPTensouDetails> GetDataOnGridView(GridSearchFields gridSearchRequest);
        int WIPTensouDetailsDelete(List<WIPTensouDetails> request);
        int GetWIPRequestReportData(string transferDate, long userId);
        List<WIPTensouDetails> WIPTensouDetailsUpdate(List<WIPTensouDetails> request);
        int GetWIPAcceptancetReportData(WIPTensouDetails details);
        int GetRowCountForTransferReport(GridSearchFields gridresult);
        List<WIPTensouDetails> GetTransferDetails(GridSearchFields gridresult);
        List<NameValue> GetVehicleName();


    }
}
