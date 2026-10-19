using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Infrastructure.IRepositories;

namespace TSWebApplication.Infrastructure.Repositories
{

    public class WIPProcessDataRepository : BaseRepository, IWIPProcessDataRepository
    {
        private readonly string _connectionString;
        private readonly LocalizationServices _localization;
        public WIPProcessDataRepository(IConfiguration _configuration, LocalizationServices localization) : base(_configuration)
        {
            _connectionString = _configuration.GetConnectionString("OracleDBConnection");
            _localization = localization;
        }
        public int GetWIPProcessDataList(int locationCd)
        {
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" count(*)");
            selectQuery.Append(" FROM TRN_WIP_STATUS WIP      ");
            selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN  PP ON PP.PROCESS_PLAN_CD = WIP.PROCESS_PLAN_CD ");
            //selectQuery.Append(" INNER JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
            //selectQuery.Append(" INNER JOIN MST_INTERNAL_NAME INTERN ON INTERN.INTERNAL_NAME_CD = PP.INTERNAL_NAME_CD ");
            //selectQuery.Append(" INNER JOIN MST_DEPARTMENT_GROUP  DEP ON DEP.DEPARTMENT_GROUP_CD = PP.DEPARTMENT_GROUP_CD ");
            //selectQuery.Append(" INNER JOIN MST_EMPLOYEE  EMP ON EMP.EMPLOYEE_CD = DEP.EMPLOYEE_CD ");
            //selectQuery.Append(" LEFT JOIN TRN_MESSAGE M ON M.MSG_CD = WIP.MSG_CD ");
            //selectQuery.Append(" LEFT JOIN MST_PROCESS_KUBUN PROCK ON PROCK.KUBUN_ID = WIP.PRE_PROCESS ");
            //selectQuery.Append(" LEFT JOIN MST_PROCESS_KUBUN PROCK1 ON PROCK1.KUBUN_ID = WIP.NEXT_PROCESS ");
            selectQuery.Append(" WHERE");
            selectQuery.Append(" PP.PROCESS_PLAN_CMPL_DATE IS NULL AND");
            selectQuery.Append("  wip.location_cd = " + locationCd + " ");
            int returnVal = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));

            return returnVal;

        }
    }
}