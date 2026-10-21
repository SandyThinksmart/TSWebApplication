using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;


namespace TSWebApplication.Application.IServices

{
    public interface IPostHTInspectionService
    {
        List<NameValue> GetDecisionList();
        List<NameValue> GetNextProcessList();
        List<NameValue> GetStackPositionList();
        List<NameValue> GetMachineName();
        List<PostHTDetails> GetChubaanDetails(List<string> chubaanNos);
        List<PostHTDetails> savePostHTInspectionDetails(List<PostHTDetails> request);
        int GetMaxGruopNo();
        PostHTDetails updatePostHTInspectionDetails(List<PostHTDetails> requestData);
        List<PostHTDetails> getPostHTDetailsByInternalQCD(long groupNo);
        void PostHTDetailsDelete(long groupNo);
        //void GetPostInspList();
        List<PostHTDetails> GetPostInspList(List<SearchField> searchCriteria, GridSearchFields searchRequest);
          PostHTDetails GetChuubanListForInvaidCheck(string enteredCChubaans);
        List<string> GetCompletedChuubanList(string enteredCChubaans);
        int checkvalidFurnaceNo(string furnaceNo);
        List<PostHTDetails> getFurnaceDetails(string furnaceNo);
        public List<PostHTDetails> GetQATestHardnessOnFactoryDailyWorkStatus(GridSearchFields gridresult);

        List<PostHTDetails> GetQATestHardnessDetailsOnDailyProcessAnalysis(GridSearchFields gridresult);
        List<NameValue> GetProcConditionKubun();
        List<NameValue> GetInOutKubun();
        List<NameValue> GetOrientationKubun();
        List<PostHTDetails> GetFurnaceNo(string furnaceNo, bool isNull);

        int GetRowCountForQATestHardnessFactory(GridSearchFields gridresult);
        int GetRowCountForPostHTHardnessDailyAnalysis(GridSearchFields gridresult);
        int PostHTInspectionCheckMilkSheetData(long groupNo);
    }
}
