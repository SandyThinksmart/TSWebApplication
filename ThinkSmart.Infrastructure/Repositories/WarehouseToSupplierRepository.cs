using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using TSWebApplication.Infrastructure.IRepositories;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;
using TSSortField = TSWebApplication.Domain.Utility.TSSortField;
namespace TSWebApplication.Infrastructure.Repositories
{
    public class WarehouseToSupplierRepository : BaseRepository, IWarehouseToSupplierRepository
    {
        private readonly string _connectionString;
        private readonly object _sortField;
        public WarehouseToSupplierRepository(IConfiguration _configuration) : base(_configuration)
        {
            _connectionString = _configuration.GetConnectionString("OracleDBConnection");
        }
        public KoubanDetails GetKoubanDetailsByKouban(string kouban)
        {
            StringBuilder selectQuery = new StringBuilder();
            try
            {
                selectQuery.Append(" SELECT ");
                selectQuery.Append("  KO.FUTOU_CD ");
                selectQuery.Append(" ,KO.MOCHIKOMISAKI_CD ");
                selectQuery.Append(" ,MKR.MAKER_NAME");
                selectQuery.Append(" ,NYUKA.NYUKA_DATE");
                selectQuery.Append(" ,MTL_MKR.MATL_MAKER_NAME");
                selectQuery.Append(" ,INT.INTERNAL_NAME");
                selectQuery.Append(" ,KO.KOUBAN ");
                selectQuery.Append(" ,KO.KOUBAN_CD ");
                selectQuery.Append(" ,MATL.DIM_D");
                selectQuery.Append(" ,MATL.SHAPE");
                selectQuery.Append(" ,MATL.MATERIAL_LENGTH");
                selectQuery.Append(" ,COND.CONDITION_DESCRIPTION");
                selectQuery.Append(" ,MATL.MATL_KUBUN");
                selectQuery.Append(" ,KO.MUKESAKI_CD ");
                selectQuery.Append(" ,COND.CONDITION_DESCRIPTION");
                selectQuery.Append(" ,KO.MUKESAKI_CD ");
                selectQuery.Append(" ,MUK.MUKESAKI_NAME ");
                selectQuery.Append(" ,MATL.DIM_W ");
                selectQuery.Append(" ,MATL.DIM_OTHER ");
                selectQuery.Append(" ,KO.KOUBAN_COMPLETE_DATE ");
                selectQuery.Append(" FROM TRN_KOUBAN KO");
                selectQuery.Append(" LEFT JOIN TRN_PO_DTL PO ON PO.PO_DTL_CD = KO.PO_DTL_CD");
                selectQuery.Append(" LEFT JOIN MST_MATERIAL MATL ON MATL.MATERIAL_CD = PO.MATERIAL_CD");
                selectQuery.Append(" LEFT JOIN MST_MATL_NAME_MAKER MTL_MKR ON MTL_MKR.MATL_NAME_MAKER_CD = MATL.MATL_NAME_MAKER_CD");
                selectQuery.Append(" LEFT JOIN MST_MAKER MKR ON MKR.MAKER_CD = MTL_MKR.MAKER_CD");
                selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INT ON INT.INTERNAL_NAME_CD = MTL_MKR.INTERNAL_NAME_CD");
                selectQuery.Append(" LEFT JOIN MST_CONDITION COND ON COND.CONDITION_CD = MATL.CONDITION_CD");
                selectQuery.Append(" LEFT JOIN MST_MOCHIKOMISAKI MOCH ON MOCH.MOCHIKOMISAKI_CD = KO.MOCHIKOMISAKI_CD");
                selectQuery.Append(" LEFT JOIN MST_FUTOU FUTOU ON FUTOU.FUTOU_CD = KO.FUTOU_CD");
                selectQuery.Append(" LEFT JOIN MST_MUKESAKI MUK ON KO.MUKESAKI_CD = MUK.MUKESAKI_CD");
                selectQuery.Append(" LEFT JOIN ( SELECT MAX(NYUKA_DATE) AS NYUKA_DATE, KOUBAN_CD FROM TRN_MATL_NYUKO_NYUKA GROUP BY KOUBAN_CD) NYUKA ON NYUKA.KOUBAN_CD = KO.KOUBAN_CD");
                selectQuery.Append("  WHERE Lower(Ko.KOUBAN)  = '" + kouban.ToLower() + "' ");
                var result = ExecuteQueryMultiResult<KoubanDetails>(selectQuery.ToString()).FirstOrDefault();
                return result;

            }
            catch (Exception ex)
            {
                ex.Data.Add("sql", selectQuery.ToString());
                throw;
            }
        }

        public MaterialTensouDetails saveTensouDetails(MaterialTensouDetails matlTensou)
        {
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append("SELECT * FROM TRN_MATL_TRANSFER ");
            selectQuery.Append("WHERE TRANSFER_CD=(SELECT MAX(TRANSFER_CD)  FROM TRN_MATL_TRANSFER)");

            StringBuilder insertQuery = new StringBuilder();
            try
            {
                insertQuery.Append(" INSERT INTO TRN_MATL_TRANSFER ( ");
                insertQuery.Append(" KOUBAN_CD ");
                insertQuery.Append(" ,FUTOU_CD ");
                insertQuery.Append(" ,MOCHIKOMISAKI_CD ");
                insertQuery.Append(" ,TRANSFER_DATE ");
                insertQuery.Append(" ,CHARGE ");
                insertQuery.Append(" ,NO_OF_UNITS ");
                insertQuery.Append(" ,UNIT ");
                insertQuery.Append(" ,QUANTITY ");
                insertQuery.Append(" ,CHUUBAN ");
                insertQuery.Append(" ,SLIP_NUMBER ");
                insertQuery.Append(" ,REMARKS ");
                insertQuery.Append(" ,USER_ID ");
                insertQuery.Append(" ,INPUT_LOCATION_CD ");
                insertQuery.Append(" ,CREATED_DATE ");
                insertQuery.Append(" ,UPDATED_DATE");
                insertQuery.Append(") VALUES (");
                insertQuery.Append(matlTensou.KOUBAN_CD);
                insertQuery.Append(",'" + matlTensou.FUTOU_CD + "'");
                insertQuery.Append(",'" + matlTensou.MOCHIKOMISAKI_CD + "'");
                insertQuery.Append(", TO_DATE('" + Convert.ToDateTime(matlTensou.TRANSFER_DATE).ToString("yyyy/MM/dd") + "', 'YYYY/MM/DD')");
                insertQuery.Append(",'" + matlTensou.CHARGE + "'");
                insertQuery.Append(",'" + matlTensou.NO_OF_UNITS + "'");
                insertQuery.Append(",'" + matlTensou.UNIT + "'");
                insertQuery.Append(",'" + matlTensou.QUANTITY + "'");
                insertQuery.Append(",'" + matlTensou.CHUUBAN + "'");
                insertQuery.Append(" ,'" + matlTensou.SLIP_NUMBER + "'");
                insertQuery.Append(" ,'" + matlTensou.REMARKS + "'");
                insertQuery.Append(",'" + matlTensou.USER_ID + "'");
                insertQuery.Append(",'" + matlTensou.INPUT_LOCATION_CD + "'");
                insertQuery.Append(", SYSDATE ");
                insertQuery.Append(", SYSDATE ");
                insertQuery.Append(")");
                ExecuteNonQuery(insertQuery.ToString());
                return ExecuteQueryMultiResult<MaterialTensouDetails>(selectQuery.ToString()).FirstOrDefault();

            }
            catch (Exception ex)
            {
                ex.Data.Add("sql", insertQuery.ToString() + Environment.NewLine + insertQuery.ToString());
                throw;
            }
        }

        public MaterialTensouDetails updateTensouDetails(MaterialTensouDetails matlTensou)
        {
           // ExecuteNonQuery("UPDATE TRN_MATL_TRANSFER SET REMARKS='TEST' WHERE TRANSFER_CD=21");

            StringBuilder selectQuery = new StringBuilder();
            string TRANSFER_DATE = matlTensou.TRANSFER_DATE.ToString("yyyy/MM/dd");
            selectQuery.Append("SELECT * FROM TRN_MATL_TRANSFER ");
            selectQuery.Append("WHERE TRANSFER_CD=" + matlTensou.TRANSFER_CD);

            StringBuilder updateQuery = new StringBuilder();

            try
            {
                updateQuery.Append("  UPDATE TRN_MATL_TRANSFER SET ");
                updateQuery.Append(" KOUBAN_CD = '"+matlTensou.KOUBAN_CD+ "'");
                updateQuery.Append(" ,FUTOU_CD ='"+ matlTensou.FUTOU_CD + "'");
                updateQuery.Append(" ,MOCHIKOMISAKI_CD ='"+ matlTensou.MOCHIKOMISAKI_CD + "'");
                updateQuery.Append(" ,TRANSFER_DATE = TO_DATE('" + TRANSFER_DATE + "','yyyy/mm/dd')");
                updateQuery.Append(" ,CHARGE ='"+ matlTensou.CHARGE+"'");
                updateQuery.Append(" ,NO_OF_UNITS ='"+ matlTensou.NO_OF_UNITS + "'");
                updateQuery.Append(" ,UNIT ='"+ matlTensou.UNIT + "'");
                updateQuery.Append(" ,QUANTITY ='"+ matlTensou.QUANTITY + "'");
                updateQuery.Append(" ,CHUUBAN ='"+ matlTensou.CHUUBAN + "'");
                updateQuery.Append(" ,SLIP_NUMBER ='"+ matlTensou.SLIP_NUMBER + "'");
                updateQuery.Append(" ,REMARKS ='"+ matlTensou.REMARKS + "'");
                updateQuery.Append(" ,USER_ID ='"+ matlTensou.USER_ID + "'");
                updateQuery.Append(" ,INPUT_LOCATION_CD ='"+ matlTensou.INPUT_LOCATION_CD + "'");
                updateQuery.Append(", UPDATED_DATE = SYSDATE ");
                updateQuery.Append("  WHERE TRANSFER_CD ='"+ matlTensou.TRANSFER_CD + "' ");
                ExecuteNonQuery(updateQuery.ToString());

                var result = ExecuteQueryMultiResult<MaterialTensouDetails>(selectQuery.ToString()).FirstOrDefault();
                return result;
                
            }
            catch (Exception ex)
            {
                ex.Data.Add("sql", updateQuery.ToString());
                throw;
            }

        }

        public List<MaterialTensouDetails> GetMatlTensouList(string sortCriteria, List<SearchField> searchCriteria)
        {
            StringBuilder selectQuery = new StringBuilder();
            List<MaterialTensouDetails> result = new List<MaterialTensouDetails>();
            try
            {
                selectQuery.Append("SELECT ");
                selectQuery.Append("TRANS.TRANSFER_CD,");
                selectQuery.Append("MTL_MKR.MATL_MAKER_NAME,");
                selectQuery.Append("MKR.MAKER_NAME,");
                selectQuery.Append("INT.INTERNAL_NAME,");
                selectQuery.Append("MATL.DIM_D,");
                selectQuery.Append("MATL.SHAPE,");
                selectQuery.Append("MATL.MATERIAL_LENGTH,");
                selectQuery.Append("COND.CONDITION_DESCRIPTION,");
                selectQuery.Append("MATL.MATL_KUBUN,");
                selectQuery.Append("KOB.KOUBAN,");
                selectQuery.Append("TRANS.CHARGE,");
                selectQuery.Append("TRANS.NO_OF_UNITS,");
                selectQuery.Append("TRANS.UNIT,");
                selectQuery.Append("TRANS.QUANTITY,");
                selectQuery.Append("TRANS.TRANSFER_DATE,");
                selectQuery.Append("FUTOU.FUTOU_NAME,");
                selectQuery.Append("MOCH.MKSAKI_NAME,");
                selectQuery.Append(" MTL_MKR.MATL_NAME_MAKER_CD,");
                selectQuery.Append(" FUTOU.FUTOU_CD,");
                selectQuery.Append("MOCH.MOCHIKOMISAKI_CD");
                selectQuery.Append(" FROM TRN_MATL_TRANSFER TRANS");
                selectQuery.Append(" LEFT JOIN TRN_KOUBAN KOB ON TRANS.KOUBAN_CD = KOB.KOUBAN_CD");
                selectQuery.Append(" LEFT JOIN TRN_PO_DTL PO ON PO.PO_DTL_CD = KOB.PO_DTL_CD");
                selectQuery.Append(" LEFT JOIN MST_MATERIAL MATL ON MATL.MATERIAL_CD = PO.MATERIAL_CD");
                selectQuery.Append(" LEFT JOIN MST_MATL_NAME_MAKER MTL_MKR ON MTL_MKR.MATL_NAME_MAKER_CD = MATL.MATL_NAME_MAKER_CD");
                selectQuery.Append(" LEFT JOIN MST_MAKER MKR ON MKR.MAKER_CD = MTL_MKR.MAKER_CD");
                selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INT ON INT.INTERNAL_NAME_CD = MTL_MKR.INTERNAL_NAME_CD");
                selectQuery.Append(" LEFT JOIN MST_CONDITION COND ON COND.CONDITION_CD = MATL.CONDITION_CD");
                selectQuery.Append(" LEFT JOIN MST_MOCHIKOMISAKI MOCH ON MOCH.MOCHIKOMISAKI_CD = TRANS.MOCHIKOMISAKI_CD");
                selectQuery.Append(" LEFT JOIN MST_FUTOU FUTOU ON FUTOU.FUTOU_CD = TRANS.FUTOU_CD");

                var query = selectQuery.ToString();
                if (searchCriteria != null && searchCriteria.Count > 0)
                {
                    string whereCondition = "  WHERE ";

                    string andOperator = string.Empty;

                    foreach (var searchField in searchCriteria)
                    {
                        whereCondition = whereCondition + andOperator + DynamicSearchProvider.ApplySearchCriteria(searchField, query);
                        andOperator = " AND ";
                    }
                    query = query + whereCondition;
                }
                
                if (!string.IsNullOrEmpty(sortCriteria))
                {
                    query = query + " ORDER BY " + sortCriteria;
                    //return query;
                }

               
                selectQuery = new StringBuilder();
                selectQuery.Append(query);
                result = ExecuteQueryMultiResult<MaterialTensouDetails>(query);
                return result;
            }
            catch (Exception ex)
            {
                ex.Data.Add("sql", selectQuery.ToString() + Environment.NewLine + selectQuery.ToString());
                throw;
            }
        }
        public MaterialTensouDetails GetMaterialTransferDetailsByTrasferCD(long TRANSFER_CD)
        {
            StringBuilder selectQuery = new StringBuilder();
            MaterialTensouDetails result = new MaterialTensouDetails();
            try
            {
                selectQuery.Append("SELECT ");
                selectQuery.Append("MTL_MKR.MATL_MAKER_NAME,");
                selectQuery.Append("MKR.MAKER_NAME,");
                selectQuery.Append("INT.INTERNAL_NAME,");
                selectQuery.Append("MATL.DIM_D,");
                selectQuery.Append("MATL.DIM_W,");
                selectQuery.Append("MATL.DIM_OTHER,");
                selectQuery.Append("MATL.SHAPE,");
                selectQuery.Append("MATL.MATERIAL_LENGTH,");
                selectQuery.Append("COND.CONDITION_DESCRIPTION,");
                selectQuery.Append("MATL.MATL_KUBUN,");
                selectQuery.Append("TRANS.KOUBAN_CD,");
                selectQuery.Append("KOB.KOUBAN,");
                selectQuery.Append("TRANS.CHARGE,");
                selectQuery.Append("TRANS.NO_OF_UNITS,");
                selectQuery.Append("TRANS.UNIT,");
                selectQuery.Append("TRANS.QUANTITY,");
                selectQuery.Append("TRANS.CHUUBAN,");
                selectQuery.Append("TRANS.SLIP_NUMBER,");
                selectQuery.Append("TRANS.REMARKS,");
                selectQuery.Append("TRANS.TRANSFER_DATE,");
                selectQuery.Append("FUTOU.SHORT_NAME AS FUTOU_NAME,");
                selectQuery.Append("MOCH.SHORT_NAME AS MKSAKI_NAME,");
                selectQuery.Append("MTL_MKR.MATL_NAME_MAKER_CD,");
                selectQuery.Append("FUTOU.FUTOU_CD,");
                selectQuery.Append("MOCH.MOCHIKOMISAKI_CD,");
                selectQuery.Append("MUK.SHORT_NAME AS MUKESAKI_NAME,");
                selectQuery.Append("NYUKA.NYUKA_DATE");
                selectQuery.Append(" FROM TRN_MATL_TRANSFER TRANS");
                selectQuery.Append(" LEFT JOIN TRN_KOUBAN KOB ON TRANS.KOUBAN_CD = KOB.KOUBAN_CD");
                selectQuery.Append(" LEFT JOIN TRN_PO_DTL PO ON PO.PO_DTL_CD = KOB.PO_DTL_CD");
                selectQuery.Append(" LEFT JOIN MST_MATERIAL MATL ON MATL.MATERIAL_CD = PO.MATERIAL_CD");
                selectQuery.Append(" LEFT JOIN MST_MATL_NAME_MAKER MTL_MKR ON MTL_MKR.MATL_NAME_MAKER_CD = MATL.MATL_NAME_MAKER_CD");
                selectQuery.Append(" LEFT JOIN MST_MAKER MKR ON MKR.MAKER_CD = MTL_MKR.MAKER_CD");
                selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INT ON INT.INTERNAL_NAME_CD = MTL_MKR.INTERNAL_NAME_CD");
                selectQuery.Append(" LEFT JOIN MST_CONDITION COND ON COND.CONDITION_CD = MATL.CONDITION_CD");
                selectQuery.Append(" LEFT JOIN MST_MOCHIKOMISAKI MOCH ON MOCH.MOCHIKOMISAKI_CD = TRANS.MOCHIKOMISAKI_CD");
                selectQuery.Append(" LEFT JOIN MST_MUKESAKI MUK ON KOB.MUKESAKI_CD = MUK.MUKESAKI_CD");
                selectQuery.Append(" LEFT JOIN MST_FUTOU FUTOU ON FUTOU.FUTOU_CD = TRANS.FUTOU_CD");
                selectQuery.Append(" LEFT JOIN ( SELECT MAX(NYUKA_DATE) AS NYUKA_DATE, KOUBAN_CD FROM TRN_MATL_NYUKO_NYUKA GROUP BY KOUBAN_CD) NYUKA ON NYUKA.KOUBAN_CD = KOB.KOUBAN_CD");
                selectQuery.Append(" WHERE TRANSFER_CD = '" + TRANSFER_CD + "'");
                result = ExecuteQuerySingleResult<MaterialTensouDetails>(selectQuery.ToString());
                return result;
            }
            catch (Exception ex)
            {
                ex.Data.Add("sql", selectQuery.ToString() + Environment.NewLine + selectQuery.ToString());
                throw;
            }
        }

       


        }
}
