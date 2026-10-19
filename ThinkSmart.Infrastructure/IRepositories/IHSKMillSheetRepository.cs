using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;

namespace TSWebApplication.Infrastructure.IRepositories
{
    public interface IHSKMillSheetRepository
    {                                                          
        HSKMillSheetDetails GetHSKMillSheetDetailsByChuuban(string PP_CHUUBAN, short? PP_GYOUBAN);
        MakerMillSheetDetails GetMakerMillSheetChemicalCompositionPopup(string FILENAME);
        HSKMillSheetDetails GetHSKMillSheetProcessPlanCD(string processPlanCD);
        List<HSKMillSheetDetails> GetHSKMillSheetList(List<SearchField> searchFields, string sortCiteria, short locationCD);
        List<HSKMillSheetDetails> GetHSKMillSheetHistoryPopUpList(List<SearchField> searchFields, string sortCiteria, short locationCD);
        int GetProcessPlanCD(string chuuban, short gyouban);
        List<HSKMillSheetDetails> GetHSKMillSheetPostHTInspectionDetails(string processPlanCD);
        public List<HSKMillSheetDetails> GetPostHTInspectionDetailsByPostHTCD(string postHTCD); 
        List<HSKMillSheetDetails> GetHSKMillSheetFinalInspectionDetails(string processPlanCD);
        List<HSKMillSheetDetails> GetFinalInspectionDetailsByPostFinalInspCD(string finalInspCD);
        List<ChemicalCompositionDetails> GetHSKChemicalCompositionDetails(string custName, string prodNameCust);
        List<MakerMillSheetDetails> GetHSKChemicalCompositionAandMakerDetails(string custName, string prodNameCust,string makerMillsheetCD);
        long GetProdOrderNoInspectionHardness(string chuuban, short gyouban, string charge, string matDimD, string matNameMaker);

        long GetProdOrderNoInspectionTensile(string chuuban, short gyouban, string charge, string matDimD, string matNameMaker);
        List<HSKMillSheetDetails> LoadHardnessPattern1DetailNoData(long prodOrderNo, int DetailNo);
        long GetProductCD(string ProdSpecNo1, string ProdSpecNo2, string ProdVer);

        List<HSKMillSheetDetails> GetProductDetails(long Prod_CD);
        List<HSKMillSheetDetails> GetProductMasterDefaultValue(long Prod_CD);
        string GetCondDescFromProdCD(long Prod_CD);
        List<HSKMillSheetDetails> HSKMillSheetGridSearchChuubanLiveCheck(string chuuban, string gyouban);
        List<MakerMillSheetDetails> GetMakerMillsheetDetails(List<SearchField> searchFields, string sortCiteria);
        public void SaveHSKMillSheetDetails(HSKMillSheetDetails request);
    }
}

