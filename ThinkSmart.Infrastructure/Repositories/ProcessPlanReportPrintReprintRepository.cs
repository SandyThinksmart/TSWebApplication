using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Odbc;
using System.IO;
using Microsoft.Extensions.Configuration;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Infrastructure.IRepositories;

namespace TSWebApplication.Infrastructure.Repositories
{
    public class ProcessPlanReportPrintReprintRepository : BaseRepository,IProcessPlanReportPrintReprintRepository
    {
        protected String kubunNumberscoiltobar;
        protected String kubunNumberscoiltocoil;
        protected String kubunNumbersbartobar;

        public ProcessPlanReportPrintReprintRepository(IConfiguration _configuration) : base(_configuration)
        {
            kubunNumberscoiltobar = _configuration.GetSection("coiltobar").GetSection("Numbers").Value;
            kubunNumberscoiltocoil = _configuration.GetSection("coiltocoil").GetSection("Numbers").Value;
            kubunNumbersbartobar = _configuration.GetSection("bartobar").GetSection("Numbers").Value;
            //HOSTconnectionString = _configuration.GetSection("HostConnectionStrings").GetSection("Connection").Value;
            //deploymentLocation = _configuration.GetSection("DeploymentLocation").GetSection("Location").Value;
            //isDeleteHostData = Convert.ToBoolean(_configuration.GetSection("IsDeleteHostData").GetSection("Condition").Value);
            //_emailSettings = emailSettings.Value;
            //_logger = logger;


        }
        public int GetProcessPlanReportCoilToCoilList(ProcessPlanReportSearchRequest request)
        {
            StringBuilder selectQuery = new StringBuilder();
         //   string kubunNumberscoiltocoil = "3";
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" count(*)");
            //selectQuery.Append(" TPP.CHUUBAN");
            //selectQuery.Append(" ,TPP.GYOUBAN");
            //selectQuery.Append(" ,TPP.PRODUCT_CD");
            //selectQuery.Append(" ,TPP.PROCESS_PLAN_CD");
            //selectQuery.Append(" ,TPP.PROD_SHAPE");
            //selectQuery.Append(" ,MP.PRODUCT_KUBUN");

            selectQuery.Append(" FROM TRN_PROCESS_PLAN TPP ");
            selectQuery.Append(" LEFT JOIN MST_PRODUCT MP  ON TPP.PRODUCT_CD=MP.PRODUCT_CD ");
            selectQuery.Append(" WHERE ");

            selectQuery.Append(" TPP.CREATED_DATE BETWEEN TO_DATE('" + request.DeliveryStartDate + "','yyyy/mm/dd') AND  TO_DATE('" + request.DeliveryEndDate + "','yyyy/mm/dd') ");
            selectQuery.Append(" AND MP.product_kubun = "+ kubunNumberscoiltocoil + " ");

            int returnVal = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));

            return returnVal;
        }

        public int GetProcessPlanReportBarToBarList(ProcessPlanReportSearchRequest request)
        {
            StringBuilder selectQuery = new StringBuilder();
           
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" count(*)");
            //selectQuery.Append(" TPP.CHUUBAN");
            //selectQuery.Append(" ,TPP.GYOUBAN");
            //selectQuery.Append(" ,TPP.PRODUCT_CD");
            //selectQuery.Append(" ,TPP.PROCESS_PLAN_CD");
            //selectQuery.Append(" ,TPP.PROD_SHAPE");
            //selectQuery.Append(" ,MP.PRODUCT_KUBUN");

            selectQuery.Append(" FROM TRN_PROCESS_PLAN TPP ");
            selectQuery.Append(" LEFT JOIN MST_PRODUCT MP  ON TPP.PRODUCT_CD=MP.PRODUCT_CD ");
            selectQuery.Append(" WHERE ");

            selectQuery.Append(" TPP.CREATED_DATE BETWEEN TO_DATE('" + request.DeliveryStartDate + "','yyyy/mm/dd') AND  TO_DATE('" + request.DeliveryEndDate + "','yyyy/mm/dd') ");
           
            selectQuery.Append(" AND MP.product_kubun = " + kubunNumbersbartobar + " ");

            int returnVal = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));

            return returnVal;
        }
        public int GetProcessPlanReportCoilToBarList(ProcessPlanReportSearchRequest request)
        {
            StringBuilder selectQuery = new StringBuilder();
          //  string kubunNumberscoiltobar = "1";
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" count(*)");
            //selectQuery.Append(" TPP.CHUUBAN");
            //selectQuery.Append(" ,TPP.GYOUBAN");
            //selectQuery.Append(" ,TPP.PRODUCT_CD");
            //selectQuery.Append(" ,TPP.PROCESS_PLAN_CD");
            //selectQuery.Append(" ,TPP.PROD_SHAPE");
            //selectQuery.Append(" ,MP.PRODUCT_KUBUN");

            selectQuery.Append(" FROM TRN_PROCESS_PLAN TPP ");
            selectQuery.Append(" LEFT JOIN MST_PRODUCT MP  ON TPP.PRODUCT_CD=MP.PRODUCT_CD ");
            selectQuery.Append(" WHERE ");

            selectQuery.Append(" TPP.CREATED_DATE BETWEEN TO_DATE('" + request.DeliveryStartDate + "','yyyy/mm/dd') AND  TO_DATE('" + request.DeliveryEndDate + "','yyyy/mm/dd') ");
            selectQuery.Append(" AND MP.PRODUCT_KUBUN = " + kubunNumberscoiltobar + " ");

            int returnVal = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));

            return returnVal;
        }

        
    }
}
