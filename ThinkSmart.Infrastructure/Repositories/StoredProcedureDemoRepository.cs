using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using TSWebApplication.Infrastructure.IRepositories;

namespace TSWebApplication.Infrastructure.Repositories
{
    public class StoredProcedureDemoRepository : BaseRepository, IStoredProcedureDemoRepository
    {
        public StoredProcedureDemoRepository(IConfiguration _configuration) : base(_configuration)
        {
        }

        public DataTable Trial1()
        {

             //ExecuteStoredProcedure();

            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append("SELECT * FROM TRN_SAMPLE");
            return ExecuteQueryDataTable(selectQuery.ToString());

        }
        public DataTable Trial2()
        {


            string selectQuery1 = "INSERT INTO TRN_SAMPLE SELECT PP.PROCESS_PLAN_CD, PP.PRODUCT_CD, PP.ROOT_VER_PRODUCT_KEY, CUST.SHORT_NAME, REC.SHORT_NAME FROM TRN_PROCESS_PLAN PP LEFT JOIN MST_CUSTOMER CUST ON PP.CUSTOMER_CD =CUST.CUSTOMER_CD LEFT JOIN MST_RECEIVER REC ON PP.RECEIVER_CD = REC.RECEIVER_CD";

            ExecuteStoredProcedureWithoutResult(selectQuery1);

            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append("SELECT * FROM TRN_SAMPLE");
            return ExecuteQueryDataTable(selectQuery.ToString());

        }
        public DataTable Trial3()
        {
            DataTable dt = new DataTable();
           // DataTable dt = ExecuteStoredProcedureWithResult();

            return dt;

        }
    }
}
