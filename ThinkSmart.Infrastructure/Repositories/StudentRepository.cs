using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using TSWebApplication.Infrastructure.IRepositories;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;

namespace TSWebApplication.Infrastructure.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly string _connectionString;
        public StudentRepository(IConfiguration _configuration)
        {
            _connectionString = _configuration.GetConnectionString("OracleDBConnection");
        }


        public IEnumerable<Student> GetAllStudent()
        {
            List<Student> studentList = new List<Student>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select ID, Name,EmailId from Student";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Student stu = new Student
                        {
                            Id = Convert.ToInt32(rdr["Id"]),
                            Name = rdr["Name"].ToString(),
                            Email = rdr["Email"].ToString()
                        };
                        studentList.Add(stu);
                    }
                }
            }
            return studentList;
        }

        public List<StamppingDetails> GetStamppingList()
        {
            List<StamppingDetails> result = new List<StamppingDetails>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = " SELECT GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN, trim(LEADING ' ' FROM To_Char(PP.GYOUBAN,'00'))) AS PP_CHUUBAN_GYOUBAN,PP.PROCESS_PLAN_CD AS PP_PROCESS_PLAN_CD,PP.PROD_SPEC_NO AS PP_PROD_SPEC_NO ,CST.SHORT_NAME AS PP_CUSTOMER_NAME,PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST,PP.PROD_DIM_D AS PP_PROD_DIM_D ,GET_DIM_D_W_SHAPE_OTHER(PP.PROD_DIM_D,PP.PROD_DIM_W,NULL,PP.PROD_DIM_OTHER) AS PP_PROD_DIM_D_STRING ,PP.PROD_SHAPE AS PP_PROD_SHAPE,PP.PROD_LENGTH AS PP_PROD_LENGTH,PP.MATL_NAME_MAK AS PP_MATL_NAME_MAK ,PP.MATL_DIM_D AS PP_MATL_DIM_D ,GET_DIM_D_W_SHAPE_OTHER(PP.MATL_DIM_D,PP.MATL_DIM_W ,PP.MATL_SHAPE,PP.MATL_DIM_OTHER) AS PP_MATL_DIM_D_STRING ,PP.CHARGE AS PP_CHARGE,ST.ST_TABA_NO,TO_CHAR( ST.ST_START_DATE,'yyyy/mm/dd HH24:MI' ) AS ST_START_DATE_STRING,ST.ST_START_OPERATOR_CD AS ST_START_OPERATOR_CD,(SELECT EM.SHORT_NAME FROM MST_EMPLOYEE EM  WHERE EM.EMPLOYEE_CD = ST_START_OPERATOR_CD) AS ST_START_OPERATOR_NAME,ST.ST_METALLIC_PLATE_CODE AS ST_METALLIC_PLATE_CODE,ST.ST_OK_BARS,ST.UNIT_TYPE AS UNIT_TYPE,ST.ST_OK_QTY,ST.ST_NG_BARS1,ST.ST_NG_BARS2,ST.ST_NG_BARS3,ST.ST_NG_DETAILS1,ST.ST_NG_DETAILS2,ST.ST_NG_DETAILS3,ST.MACHINE_CD AS MACHINE_CD ,ST.STAMPING_CD AS STAMPING_CD ,(SELECT COUNT(PP.PROCESS_PLAN_CD) FROM TRN_PROCESS_PLAN PP INNER JOIN TRN_STAMPING ST  ON PP.PROCESS_PLAN_CD = ST.PROCESS_PLAN_CD INNER JOIN MST_EMPLOYEE EM ON ST.ST_START_OPERATOR_CD = EM.EMPLOYEE_CD WHERE ST.ST_END_OPERATOR_CD IS NOT NULL )  AS ROW_COUNT  FROM TRN_PROCESS_PLAN PP  INNER JOIN TRN_STAMPING ST   ON PP.PROCESS_PLAN_CD = ST.PROCESS_PLAN_CD  INNER JOIN MST_EMPLOYEE EMP  ON ST.ST_START_OPERATOR_CD = EMP.EMPLOYEE_CD  LEFT JOIN MST_CUSTOMER CST ON PP.CUSTOMER_CD = CST.CUSTOMER_CD  LEFT JOIN MST_INTERNAL_NAME INTE ON INTE.INTERNAL_NAME_CD = PP.INTERNAL_NAME_CD  WHERE ST.LOCATION_CD =13 AND ST.ST_END_OPERATOR_CD IS NOT NULL  AND ROWNUM <=  (CASE WHEN (ROWNUM >100)  THEN 100 ELSE ROWNUM END)ORDER BY PP.CHUUBAN ASC, ST.ST_TABA_NO ASC ";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    var dataTable = new DataTable();
                    dataTable.Load(rdr);
                    result = CommonConvertMethods.ConvertDataTable<StamppingDetails>(dataTable);
                }
            }
            return result;
        }

      

    }
}
