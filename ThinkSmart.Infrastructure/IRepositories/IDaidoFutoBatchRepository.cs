using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;

namespace TSWebApplication.Infrastructure.IRepositories
{
    public interface IDaidoFutoBatchRepository
    {
        List<NameValue> GetFutouList();
        bool ValidateSameDataExist(DaidoFutouNyukoDetails details);
        public void DaidoNyukoEDIBatchDataInsert(DaidoFutouNyukoDetails details, string cultureName, int userID,int inputLocationCd);
        void NyukoDataInsert(int koubanCd,DaidoFutouNyukoDetails details);
        int GetKoubanCd(string kouban, string matlNamemaker, string DimD);
        int GetNyukoNo(int koubanCd, DaidoFutouNyukoDetails details);
        void UpdateNyukoNo(int nyukoNo,DaidoFutouNyukoDetails details);
        bool IsKoubanExists(string kouban);
        bool IsKoubanDeleted(string kouban);
        bool IsMaterialNameMakerExists(string kouban, string matlNameMaker);
        bool IsKoubanMaterialNameMakDimDExists(string kouban, string matlNameMaker, string dimD);
        public DaidoFutouNyukoDetails GetDataCount(DateTime Importdate);

       
    }

}
