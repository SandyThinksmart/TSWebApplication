using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;

namespace TSWebApplication.Infrastructure.IRepositories
{
    public interface IMakerMaterialRepository
    {
        List<NameValue> GetMakersList();
        List<NameValue> GetMukesakiList(string yobina);

        List<MakerMaterialDetails> GetMaterialList(MakerMaterialDetails details);
        List<MakerMaterialDetails> SaveMaterialDetails(List<MakerMaterialDetails> details);
        List<MakerMaterialDetails> GetListData(MakerMaterialDetails details, List<SearchField> searchCriteria, string sortCriteria);
        List<MakerMaterialDetails> GetMaterialDetailsBySearchCriteria(MakerMaterialDetails details);
        List<MakerMaterialDetails> UpdateMaterialDetails(List<MakerMaterialDetails> details);
        void DeleteMaterialDetails(int groupNo);
        List<MakerMaterialDetails> GetMaterialDetailsByGroupNo(int groupNo);
    }
}
