using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;

namespace TSWebApplication.Application.IServices
{
    public interface IMakerMaterialService
    {
        List<NameValue> GetMaterialTypeList();
        List<NameValue> GetMakersList();
        List<NameValue> GetMukesakiList(string yobina);
        List<MakerMaterialDetails> GetMaterialDetailsList(MakerMaterialDetails details);
        List<MakerMaterialDetails> SaveMaterialDetails(List<MakerMaterialDetails> details);
        List<MakerMaterialDetails> GetListPageData(MakerMaterialDetails details);
        List<MakerMaterialDetails> GetMaterialDetailsByGroupNo(int GroupNo);
        List<MakerMaterialDetails> UpdateMaterialDetails(List<MakerMaterialDetails> details);
        void DeleteMaterialDetails(int groupNo);
    }
}
