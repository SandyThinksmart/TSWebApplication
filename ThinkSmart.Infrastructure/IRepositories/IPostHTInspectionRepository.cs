using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;


namespace TSWebApplication.Infrastructure.IRepositories
{
    public interface IPostHTInspectionRepository
    {
        List<ProductOrderCompletion> GetProcessPlanDetails(List<string> chubanList);
        int GetGroupNoDetails();
        List<NameValue> GetMachineNameList();
        void savePostHTInspectionDetails(PostHTDetails request);
        List<PostHTDetails> FurnaceHdfDetails(string furnace_no);
        void updatePostHTInspectionDetails(PostHTDetails request);
        void PostHTDetailsDelete(long groupNo);
        List<PostHTDetails> GetPostInspList(List<SearchField> searchCriteria, string sortCiteria);
        List<PostHTDetails> GetProcPlanCDDetailsByInternalQCD(long groupNo);
        PostHTDetails GetChuubanListForInvaidCheck(string chuubanList);
        List<string> GetCompletedChuubanList(string chuubanList);
        int GetGroupNo();
        int checkvalidFurnaceNo(string furnaceNo);
        List<PostHTDetails> getFurnaceDetails(string furnaceNo);
        public List<PostHTDetails> GetQATestHardnessOnFactoryDailyWorkStatus(List<SearchField> searchFields, string sortCiteria,int rowLimit);
        List<PostHTDetails> GetQATestHardnessDetailsOnDailyProcessAnalysis(List<SearchField> searchFields, string sortCiteria, GridSearchFields gridresult);
        int GetUserLocationById(int userID);
        List<PostHTDetails> GetFurnaceNoDetails(string FURNACE_NO,bool isNull);
        int GetRowCountForQATestHardnessFactory(List<SearchField> searchFields);
        int GetRowCountForPostHTHardnessDailyAnalysis(List<SearchField> searchFields);
        int PostHTInspectionCheckMilkSheetData(long groupNo);
    }
}
