using Dapper;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection.Metadata;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;

namespace TSWebApplication.Infrastructure.Repositories
{
    public abstract class BaseRepository
    {
        protected string connectionString = null;
        public BaseRepository(IConfiguration _configuration)
        {
            connectionString = _configuration.GetConnectionString("OracleDBConnection");
        }

        protected void Initialize(string connectionString)
        {
            this.connectionString = connectionString;
        }

        protected OracleConnection Connection()
        {

            try
            {
                return new OracleConnection(this.connectionString);
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void ExecuteNonQuery(string query)
        {
            try
            {
                //using OracleConnection dbConnection = Connection();
                //using (OracleCommand cmd = dbConnection.CreateCommand())
                //{
                //    dbConnection.Open();
                //    cmd.CommandText = query;
                //    cmd.ExecuteNonQuery();
                //}
                using (var connection = Connection())
                {
                    connection.Execute(query);

                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("sql", query);
                throw ex;
            }
        }
        protected void ExecuteNonQueryList<T>(string query,List<T> list)
        {
            try
            {
               
                using (var connection = Connection())
                {
                    connection.Execute(query,list);

                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("sql", query);
                throw ex;
            }
        }

        protected object ExecuteScalar(string query)
        {
            object returnVal;
            try
            {
                //using OracleConnection dbConnection = Connection();
                //using (OracleCommand cmd = dbConnection.CreateCommand())
                //{
                //    dbConnection.Open();
                //    cmd.CommandText = query;
                //    returnVal = cmd.ExecuteScalar();
                //}
                using (var connection = Connection())
                {
                    returnVal = connection.ExecuteScalar<object>(query);

                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("sql", query);
                throw ex;
            }
            return returnVal;
        }

        protected T ExecuteQuerySingleResult<T>(string query)
        {
            try
            {
                T returnVal;
            
                //    using OracleConnection dbConnection = Connection();
                //using (OracleCommand cmd = dbConnection.CreateCommand())
                //{
                //    dbConnection.Open();
                //    cmd.CommandText = query;
                //    OracleDataReader rdr = cmd.ExecuteReader();
                //    var dataTable = new DataTable();
                //    dataTable.Load(rdr);
                //    returnVal = CommonConvertMethods.ConvertDataTableToClass<T>(dataTable);
                //}
                using (var connection = Connection())
                {
                    returnVal = connection.QueryFirstOrDefault<T>(query);

                }
                return returnVal;
            }            
            catch (Exception ex)
            {
                ex.Data.Add("sql", query);
                throw ex;
            }

} 

        protected List<T> ExecuteQueryMultiResult<T>(string query)
        {
            List<T> returnVal;
            try
            {
                //  using OracleConnection dbConnection = Connection();
                //using (OracleCommand cmd = dbConnection.CreateCommand())
                //{
                //    dbConnection.Open();
                //    cmd.CommandText = query;
                //    OracleDataReader rdr = cmd.ExecuteReader();
                //    var dataTable = new DataTable();
                //    dataTable.Load(rdr);
                //    returnVal = CommonConvertMethods.ConvertDataTable<T>(dataTable);
                //}
                using (var connection = Connection())
                {
                    var dataTable = connection.Query<T>(query);
                    returnVal = (List<T>)dataTable;
                }


            }
            catch (Exception ex)
            {
                ex.Data.Add("sql", query);
                throw ex;
            }
            return returnVal;
        }


        protected List<T> ExecuteEdiQueryMultiResult<T>(string query)
        {
            try
            {
                using (var connection = Connection())
                {
                    return connection
                        .Query<T>(query)
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("sql", query);
                throw ex;
            }
        }

        protected DataTable ExecuteQueryDataTable(string query)
        {
            var dataTable = new DataTable();
            
            try
            {
                //    using OracleConnection dbConnection = Connection();
                //using (OracleCommand cmd = dbConnection.CreateCommand())
                //{
                //    dbConnection.Open();
                //    cmd.CommandText = query;
                //    OracleDataReader rdr = cmd.ExecuteReader();
                //    dataTable.Load(rdr);
                //}

                using (var connection = Connection())
                {
                    IEnumerable<dynamic> result =(IEnumerable<dynamic>) connection.Query(query);
                     dataTable = ConvertToDataTable(result);
                }

            
            }
            catch (Exception ex)
            {
                ex.Data.Add("sql", query);
                throw ex;
            }
            return dataTable;
        }
        protected void ExecuteStoredProcedure(WipStatusDetails wipDetails)
        {

            var parameters = new DynamicParameters();
            parameters.Add("PROCESSPLAN_CD", wipDetails.PROCESS_PLAN_CD);
            parameters.Add("NIFUDACD", wipDetails.NIFUDA_CD);
            parameters.Add("TABANO", wipDetails.TABA_NO);
            parameters.Add("TABAGROUPNO", wipDetails.TABA_GROUP_NO);
            parameters.Add("LOCATIONCD", wipDetails.LOCATION_CD);
            parameters.Add("MSGCD", wipDetails.MSG_CD);
            parameters.Add("FINISHEDBARS", wipDetails.FINISHED_BARS);
            parameters.Add("UNITS", wipDetails.UNIT);
            parameters.Add("FINISHEDQTY", wipDetails.FINISHED_QTY);
           // parameters.Add("PROCESSSTARTDATE", wipDetails.PROCESS_START_DATE, dbType: DbType.Date, direction: ParameterDirection.Input);
            parameters.Add("PROCESSSTARTDATE",    wipDetails.PROCESS_START_DATE);
            parameters.Add("PROCESSENDDATE", wipDetails.PROCESS_END_DATE);
            parameters.Add("PROCESSKUBUNCD", wipDetails.PROCESS_KUBUN_CD);
            parameters.Add("PREPROSESSDECISION", wipDetails.PRE_PROSESS_DECISION);
            parameters.Add("NEXTPROCESS", wipDetails.NEXT_PROCESS);
            parameters.Add("STARTOPERATORCD", wipDetails.START_OPERATOR_CD);
            parameters.Add("ENDOPERATORCD", wipDetails.END_OPERATOR_CD);
            using (var connection = Connection())
            {
                connection.Execute("GET_WIP_STATUS", parameters, commandType: CommandType.StoredProcedure);
            }
          
            //using OracleConnection dbConnection = Connection();
            //using (OracleCommand cmd = dbConnection.CreateCommand())
            //{
            //    dbConnection.Open();


            //    cmd.CommandText = "SP_Procedure_Without_Data1";
            //    cmd.CommandType = CommandType.StoredProcedure;
            //    cmd.ExecuteNonQuery();

            //}

        }

        protected void ExecuteStoredProcedureWithoutResult(string parameter)
        {
            //           using OracleConnection dbConnection = Connection();
            //using (OracleCommand cmd = dbConnection.CreateCommand())
            //{
            //    dbConnection.Open();


            //    cmd.CommandText = "SP_Procedure_Without_Data2";
            //    cmd.CommandType = CommandType.StoredProcedure;
            //    cmd.Parameters.Add("selectQuery", OracleDbType.Varchar2, ParameterDirection.Input).Value = parameter;
            //cmd.ExecuteNonQuery(); 

            //}



        }
        protected DataTable ExecuteStoredProcedureWithResult(StoredProcedureDetails procedureDetails)
        {
            DataTable dt = new DataTable();
            using OracleConnection dbConnection = Connection();
            using (OracleCommand cmd = dbConnection.CreateCommand())
            {
                dbConnection.Open();


                cmd.CommandText = procedureDetails.PROCEDURE_NAME;
                cmd.CommandType = CommandType.StoredProcedure;
                foreach (var item in procedureDetails.PARAMETERS)
                {
                    if (item.DATA_TYPE=="string")
                    {
                        cmd.Parameters.Add(item.PARAMETER_NAME, OracleDbType.NVarchar2, ParameterDirection.Input).Value = item.PARAMETER_VALUE;// ;
                    }else if (item.DATA_TYPE == "number")
                    {
                        cmd.Parameters.Add(item.PARAMETER_NAME, OracleDbType.Decimal, ParameterDirection.Input).Value = item.PARAMETER_VALUE;
                    }else if (item.DATA_TYPE == "date")
                    {
                        cmd.Parameters.Add(item.PARAMETER_NAME, OracleDbType.Date, ParameterDirection.Input).Value = Convert.ToDateTime(item.PARAMETER_VALUE); 
                    }

                }
                cmd.Parameters.Add("OUTPUT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
                OracleDataAdapter da = new OracleDataAdapter(cmd);

                da.Fill(dt);
                OracleDataReader reader = cmd.ExecuteReader();

            }

            // NOTES : -
            // IF DATE FORMAT YYYY/MM/DD THEN APPLY TRUNC() IN DB SIDE AND DATA TYPE IS DATE IN INPUT PARAMTER.
            // IF DATE IS NULL OR IS NOT NULL CHECKING THEN USE NVARCHAR2 DATA TYPE IN INPUT PARAMTER AND APPLY IF CONDITION FOR IS NULL AND IS NOT NULL CONDITION FOR WHERE CONDIITON AND SEPARATE BOTH QUERY IN DB SIDE.

            return dt;
        }
        protected List<T> ExecuteQuerySalesProduction<T>(string query, int processPlanCD)
        {
            List<T> returnVal;
            try
            {
                using (var connection = Connection())
                {
                    var dataTable = connection.Query<T>(query, new { PROCESS_PLAN = processPlanCD });
                    returnVal = (List<T>)dataTable;
                    
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnVal;
        }

        public static DataTable ConvertToDataTable<T>(IEnumerable<T> itemsList)
        {
            var firstItem = itemsList.FirstOrDefault();

            if (firstItem == null)
            {
                return new DataTable();
            }
            DataTable dt = new DataTable(TypeDescriptor.GetClassName(firstItem));
            object[] obj;
            int i;

            var properties = TypeDescriptor.GetProperties(firstItem);

            long j = Convert.ToInt64(properties.Count)/2;
            int k = 0;

            foreach (PropertyDescriptor property in properties)
            {
                if (k == j)
                {
                    break;
                }
                else
                {
                    dt.Columns.Add(property.Name, BaseType(property.PropertyType));
                    k++;
                }
            }
            
           
            foreach (T item in itemsList)
            {
                obj = new object[j];

                for (i = 0; i < j; i++)
                {
                    obj[i] = properties[i].GetValue(item);
                }
               dt.Rows.Add(obj);
            }
        
            return dt;
        }
        public static Type BaseType(Type oType)
        {
            //If the passed oType is valid, .IsValueType and is logicially nullable, .Get(its)UnderlyingType
            if (oType != null && oType.IsValueType &&
                oType.IsGenericType && oType.GetGenericTypeDefinition() == typeof(Nullable<>)
                )
            {
                return Nullable.GetUnderlyingType(oType);
            }
            //Else the passed oType was null or was not logicially nullable, so simply return the passed oType
            else
            {
                return oType;
            }
        }

        //MMS BATCH Added by Pallavi
        protected void ExecuteStoredProcedureRemQty4MonthAgo(int Roll_RYearBeforeSixMonth, int Roll_RBeforeSixMonth)
        {

            var parameters = new DynamicParameters();
            parameters.Add("P_YEAR", Roll_RYearBeforeSixMonth);
            parameters.Add("P_ROLL_R", Roll_RBeforeSixMonth);

            using (var connection = Connection())
            {
                connection.Execute("SP_UPDATE_REM_QTY_4MONTH_AGO", parameters, commandType: CommandType.StoredProcedure);
            }

           

        }


        //MMS BATCH Added by Pallavi
        protected void ExecuteStoredProcedureRemQtyLast3Months(int Roll_RBeforeSixMonth, int Roll_RYearBeforeSixMonth, int Roll_RBeforeFiveMonth, int Roll_RYearBeforeFiveMonth, int Roll_RBeforeFourMonth, int Roll_RYearBeforeFourMonth)
        {

            var parameters = new DynamicParameters();
            parameters.Add("P_ROLL_3", Roll_RBeforeSixMonth);
            parameters.Add("P_YEAR_3", Roll_RYearBeforeSixMonth);
            parameters.Add("P_ROLL_2", Roll_RBeforeFiveMonth);
            parameters.Add("P_YEAR_2", Roll_RYearBeforeFiveMonth);
            parameters.Add("P_ROLL_1", Roll_RBeforeFourMonth);
            parameters.Add("P_YEAR_1", Roll_RYearBeforeFourMonth);

            using (var connection = Connection())
            {
                connection.Execute("SP_UPDATE_REM_QTY_LAST_3MONTHS", parameters, commandType: CommandType.StoredProcedure);
            }



        }

        //MMS BATCH Added by Pallavi
        protected void ExecuteStoredProcedurePOQty(int Roll_RBeforeSixMonth, int Roll_RYearBeforeSixMonth, int Roll_RBeforeFiveMonth, int Roll_RYearBeforeFiveMonth, int Roll_RBeforeFourMonth, int Roll_RYearBeforeFourMonth, 
            int Roll_RBeforeThreeMonth, int Roll_YearBeforeThreeMonth, int Roll_RBeforeTwoMonth,int Roll_YearBeforeTwoMonth,int Roll_RBeforeOneMonth,int Roll_YearBeforeOneMonth,int Roll_RCurrentMonth,int Roll_YearCurrentMonth,
            int Roll_RAfterOneMonth, int Roll_YearAfterOneMonth)
        {

            var parameters = new DynamicParameters();
            parameters.Add("p_r1_roll", Roll_RBeforeSixMonth);
            parameters.Add("p_r1_year", Roll_RYearBeforeSixMonth);
            parameters.Add("p_r2_roll", Roll_RBeforeFiveMonth);
            parameters.Add("p_r2_year", Roll_RYearBeforeFiveMonth);
            parameters.Add("p_r3_roll", Roll_RBeforeFourMonth);
            parameters.Add("p_r3_year", Roll_RYearBeforeFourMonth);

            parameters.Add("p_r4_roll", Roll_RBeforeThreeMonth);
            parameters.Add("p_r4_year", Roll_YearBeforeThreeMonth);
            parameters.Add("p_r5_roll", Roll_RBeforeTwoMonth);
            parameters.Add("p_r5_year", Roll_YearBeforeTwoMonth);
            parameters.Add("p_r6_roll", Roll_RBeforeOneMonth);
            parameters.Add("p_r6_year", Roll_YearBeforeOneMonth);
            parameters.Add("p_r7_roll", Roll_RCurrentMonth);
            parameters.Add("p_r7_year", Roll_YearCurrentMonth);
            parameters.Add("p_r8_roll", Roll_RAfterOneMonth);
            parameters.Add("p_r8_year", Roll_YearAfterOneMonth);


            using (var connection = Connection())
            {
                connection.Execute("SP_UPDATE_PO_QTY", parameters, commandType: CommandType.StoredProcedure);
            }



        }

        //MMS BATCH Added by Pallavi
        protected void ExecuteStoredProcedureIntStockQty()
        {

            var parameters = new DynamicParameters();
           

            using (var connection = Connection())
            {
                connection.Execute("SP_UPDATE_INT_STOCK_QTY", commandType: CommandType.StoredProcedure);
            }



        }

        //MMS BATCH Added by Pallavi
        protected void ExecuteStoredProcedureExtStockQty()
        {

            var parameters = new DynamicParameters();


            using (var connection = Connection())
            {
                connection.Execute("SP_UPDATE_EXT_STOCK_QTY", commandType: CommandType.StoredProcedure);
            }



        }

        protected void ExecuteStoredProcedureUpdatePoRemQtyPrev(int Roll_RYearBeforeSixMonth, int Roll_RBeforeSixMonth)
        {

            var parameters = new DynamicParameters();
            parameters.Add("P_YEAR", Roll_RYearBeforeSixMonth);
            parameters.Add("P_ROLL_R", Roll_RBeforeSixMonth);

            using (var connection = Connection())
            {
                connection.Execute("SP_UPDATE_PO_REM_QTY_PREV", parameters, commandType: CommandType.StoredProcedure);
            }



        }

        protected void ExecuteStoredProcedureUpdateNyukaQtyLast3Months(string RollYr_BeforeSixMonth, string RollYr_BeforeFiveMonth,string RollYr_BeforeFourMonth)
        {

            var parameters = new DynamicParameters();
            parameters.Add("P_YM_3", RollYr_BeforeSixMonth);
            parameters.Add("P_YM_2", RollYr_BeforeFiveMonth);
            parameters.Add("P_YM_1", RollYr_BeforeFourMonth);

            using (var connection = Connection())
            {
                connection.Execute("SP_UPDATE_NYUKA_QTY_LAST_3MONTHS", parameters, commandType: CommandType.StoredProcedure);
            }



        }

        protected void ExecuteStoredProcedurePoRemQtyLast3Months(int Roll_RBeforeSixMonth, int Roll_RYearBeforeSixMonth, int Roll_RBeforeFiveMonth, int Roll_RYearBeforeFiveMonth, int Roll_RBeforeFourMonth, int Roll_RYearBeforeFourMonth)
        {

            var parameters = new DynamicParameters();
            parameters.Add("P_ROLL_1", Roll_RBeforeSixMonth);
            parameters.Add("P_YEAR_1", Roll_RYearBeforeSixMonth);
            parameters.Add("P_ROLL_2", Roll_RBeforeFiveMonth);
            parameters.Add("P_YEAR_2", Roll_RYearBeforeFiveMonth);
            parameters.Add("P_ROLL_3", Roll_RBeforeFourMonth);
            parameters.Add("P_YEAR_3", Roll_RYearBeforeFourMonth);

            using (var connection = Connection())
            {
                connection.Execute("SP_UPDATE_PO_REM_QTY_LAST_3MONTHS", parameters, commandType: CommandType.StoredProcedure);
            }



        }
    }
}
