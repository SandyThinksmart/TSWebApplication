using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Channels;
using System.Transactions;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Infrastructure.IRepositories;
using static TSWebApplication.Domain.Utility.Constants;

namespace TSWebApplication.Infrastructure.Repositories
{
    public class MakerMaterialRepository :BaseRepository , IMakerMaterialRepository
    {
        public MakerMaterialRepository(IConfiguration _configuration, IHostEnvironment hostEnvironment) : base(_configuration)
        {
            
        }
        public List<NameValue> GetMakersList()
        {
            List<NameValue> result = new List<NameValue>();
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append("SELECT MAKER_CD AS CD ");
            selectQuery.Append(", NVL(SHORT_NAME,MAKER_NAME) AS NAME ");
            selectQuery.Append(" FROM MST_MAKER ");
            selectQuery.Append(" WHERE ");
            selectQuery.Append(" IN_ACTIVE_FLAG=0 ");
            selectQuery.Append(" ORDER BY DISPLAY_PRIORITY ASC ");            
            result = ExecuteQueryMultiResult<NameValue>(selectQuery.ToString());
            return result;
        }
        public List<NameValue> GetMukesakiList(string yobina)
        {
            string _maxRowLimit = Constants.DefaultValues.SEARCH_COMBO_MAX_LIMIT;
            List<NameValue> result = new List<NameValue>();
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append("SELECT MUKESAKI_CD AS CD ");
            selectQuery.Append(", NVL(SHORT_NAME,MUKESAKI_NAME) AS NAME ");
            selectQuery.Append(" FROM MST_MUKESAKI ");
            selectQuery.Append(" WHERE ");
            selectQuery.Append(" IN_ACTIVE_FLAG=0 ");

            if (!string.IsNullOrEmpty(yobina))
            {
                selectQuery.Append(" AND LOWER (YOBINA) LIKE  LOWER('" + yobina + "%')");
            }else
            {
                selectQuery.Append(" AND ROWNUM <= " + _maxRowLimit);
            }
            result = ExecuteQueryMultiResult<NameValue>(selectQuery.ToString());
            return result;
           
        }
        public List<MakerMaterialDetails> GetMaterialList(MakerMaterialDetails details)
        {
            
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT  ");
            selectQuery.Append(" MM.MATL_NAME_MAKER_CD, ");
            selectQuery.Append(" MM.MATL_MAKER_NAME, ");
            selectQuery.Append(" INT.INTERNAL_NAME AS MTL_NAME_INTERNAL, ");
            selectQuery.Append(" HC.DIM_START, ");
            selectQuery.Append(" HC.DIM_END, ");
            selectQuery.Append(" HC.RATE, ");
            selectQuery.Append(" HC.MATERIAL_RATE_CD ");
          
            selectQuery.Append(" FROM MST_MATL_NAME_MAKER MM ");
            selectQuery.Append(" INNER JOIN MST_INTERNAL_NAME INT ON MM.INTERNAL_NAME_CD = INT.INTERNAL_NAME_CD ");
         
            selectQuery.Append(" LEFT OUTER JOIN( ");
            selectQuery.Append(" SELECT * FROM");
            selectQuery.Append(" MST_MATERIAL_RATE ");
            selectQuery.Append(" WHERE ");
            selectQuery.Append(" MUKESAKI_CD = " + details.MUKESAKI_CD);
            selectQuery.Append(" AND APPLY_ROLL_YEAR = " + details.APPLY_ROLL_YEAR);
            selectQuery.Append(" AND APPLY_ROLL_MONTH = " + details.APPLY_ROLL_MONTH);
            selectQuery.Append(" AND APPLY_ROLL_CHANCE = " + details.APPLY_ROLL_CHANCE);
            selectQuery.Append(" AND MATL_KUBUN = " + details.MATL_KUBUN);
            selectQuery.Append(" ) HC ");
            selectQuery.Append(" ON MM.MATL_NAME_MAKER_CD = HC.MATL_NAME_MAKER_CD ");

            selectQuery.Append(" WHERE ");
          
            selectQuery.Append(" MM.IN_ACTIVE_FLAG = 0  ");
            selectQuery.Append(" AND MM.MAKER_CD = " + details.MAKER_CD);
            
            selectQuery.Append(" GROUP BY  MM.MATL_NAME_MAKER_CD, MM.MATL_MAKER_NAME,INT.INTERNAL_NAME, HC.DIM_START, HC.DIM_END, HC.RATE, HC.MATERIAL_RATE_CD");
            selectQuery.Append(" ORDER BY  ");
            selectQuery.Append("  INT.INTERNAL_NAME ASC, MM.MATL_MAKER_NAME ASC ");
            

            var result= ExecuteQueryMultiResult<MakerMaterialDetails>(selectQuery.ToString());
            return result;

        }
        public List<MakerMaterialDetails> SaveMaterialDetails(List<MakerMaterialDetails> details)
        {
            int seqVal = 0;
            int groupNo = 0;
            List <MakerMaterialDetails> result= new List<MakerMaterialDetails>();

            using (TransactionScope transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
               

                foreach (var item in details)
                {
                    if (item.RATE > 0)
                    {
                        StringBuilder selectQuary = new StringBuilder();
                        selectQuary.Append("SELECT MST_MATERIAL_RATE_SEQ.NEXTVAL FROM DUAL");
                        seqVal = Convert.ToInt32(ExecuteScalar(selectQuary.ToString()));

                        if (groupNo == 0)
                        {
                            groupNo = seqVal;
                        }

                        StringBuilder insertQuery = new StringBuilder();
                        insertQuery.Append(" INSERT INTO MST_MATERIAL_RATE  ");
                        insertQuery.Append(" (  ");
                        insertQuery.Append("  MATERIAL_RATE_CD ");
                        insertQuery.Append(" ,MATL_GROUP_NO ");
                        insertQuery.Append(" ,MATL_NAME_MAKER_CD  ");
                        insertQuery.Append(" ,MUKESAKI_CD  ");
                        insertQuery.Append(" ,MATL_KUBUN  ");
                        insertQuery.Append(" ,DIM_START  ");
                        insertQuery.Append(" ,DIM_END  ");
                        insertQuery.Append(" ,RATE  ");
                        insertQuery.Append(" ,APPLY_ROLL_YEAR  ");
                        insertQuery.Append(" ,APPLY_ROLL_MONTH  ");
                        insertQuery.Append(" ,APPLY_ROLL_CHANCE  ");
                        insertQuery.Append(" ,USER_ID ");
                        insertQuery.Append(" ,CREATED_DATE  ");
                        insertQuery.Append(" ,UPDATED_DATE ");
                        insertQuery.Append("  ) ");
                        insertQuery.Append(" VALUES  ");
                        insertQuery.Append(" (  ");
                        insertQuery.Append(" '" + seqVal + "'");
                        insertQuery.Append(", '" + groupNo + "'");
                        insertQuery.Append(", '" + item.MATL_NAME_MAKER_CD + "'");
                        insertQuery.Append(", '" + item.MUKESAKI_CD + "'");
                        insertQuery.Append(", '" + item.MATL_KUBUN + "'");
                        insertQuery.Append(", '" + item.DIM_START + "'");
                        insertQuery.Append(", '" + item.DIM_END + "'");
                        insertQuery.Append(", '" + item.RATE + "'");
                        insertQuery.Append(", '" + item.APPLY_ROLL_YEAR + "'");
                        insertQuery.Append(", '" + item.APPLY_ROLL_MONTH + "'");
                        insertQuery.Append(", '" + item.APPLY_ROLL_CHANCE + "'");
                        insertQuery.Append(", '" + item.USER_ID + "'");
                        insertQuery.Append(", SYSDATE ");
                        insertQuery.Append(", SYSDATE ");
                        insertQuery.Append(" ) ");
                        ExecuteNonQuery(insertQuery.ToString());
                    }
                }
                result = GetMaterialDetailsBySearchCriteria(details[0]);
                transaction.Complete();
            }
           
            return result;
        }
        public List<MakerMaterialDetails> UpdateMaterialDetails(List<MakerMaterialDetails> details)
        {
            List <MakerMaterialDetails> result = new List<MakerMaterialDetails>();
            using (TransactionScope transaction = new TransactionScope())
            {
                DeleteMaterialDetails(Convert.ToInt32(details[0].MATL_GROUP_NO));
                var resultData = SaveMaterialDetails(details);
                if (details[0].SELECT_EVENT_FLAG == true)
                {
                    result = GetMaterialDetailsByGroupNo(Convert.ToInt32(resultData[0].MATL_GROUP_NO));
                }
                else
                {
                    result = resultData;

                }
               
                transaction.Complete();
            }
            return result;

        }

        public void DeleteMaterialDetails(int groupNo)
        {

            StringBuilder deleteQuery = new StringBuilder();
            deleteQuery.Append("DELETE FROM MST_MATERIAL_RATE ");
            deleteQuery.Append(" WHERE ");
            deleteQuery.Append(" MATL_GROUP_NO = " + groupNo);
            //deleteQuery.Append(" MUKESAKI_CD = '" + materialDetails.MUKESAKI_CD + "'");
            //deleteQuery.Append(" AND APPLY_ROLL_YEAR = '" + materialDetails.APPLY_ROLL_YEAR + "'");
            //deleteQuery.Append(" AND APPLY_ROLL_MONTH = '" + materialDetails.APPLY_ROLL_MONTH + "'");
            //deleteQuery.Append(" AND APPLY_ROLL_CHANCE = '" + materialDetails.APPLY_ROLL_CHANCE + "'");

            ExecuteNonQuery(deleteQuery.ToString());

        }

        public List<MakerMaterialDetails> GetListData(MakerMaterialDetails details,List<SearchField> searchCriteria, string sortCriteria) 
        {
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT  ");
          
            selectQuery.Append(" HTMATL.MATL_GROUP_NO, ");
            selectQuery.Append(" NVL(MKR.SHORT_NAME,MKR.MAKER_NAME) AS MAKER_NAME, ");
            selectQuery.Append(" CONCAT (CONCAT (HTMATL.APPLY_ROLL_YEAR,'-'), "); 
           
            selectQuery.Append(" CONCAT (CONCAT ( TO_CHAR(HTMATL.APPLY_ROLL_MONTH, 'FM00') ,'-'), HTMATL.APPLY_ROLL_CHANCE)");
            selectQuery.Append(" ) AS APPLICATION_DATE_STRING, ");
            selectQuery.Append(" HTMATL.MATL_KUBUN, ");
            
            selectQuery.Append(" NVL(MSK.SHORT_NAME,MSK.MUKESAKI_NAME) AS MUKESAKI_NAME ");

            selectQuery.Append(" FROM MST_MATERIAL_RATE HTMATL ");
          
            selectQuery.Append(" INNER JOIN MST_MATL_NAME_MAKER MM ON MM.MATL_NAME_MAKER_CD =  HTMATL.MATL_NAME_MAKER_CD ");
      
            selectQuery.Append(" INNER JOIN MST_MAKER MKR ON MM.MAKER_CD =MKR.MAKER_CD ");
            selectQuery.Append(" INNER JOIN MST_MUKESAKI MSK ON MSK.MUKESAKI_CD = HTMATL.MUKESAKI_CD ");
           
            selectQuery.Append(" WHERE ");

            string query = selectQuery.ToString();
            if (searchCriteria != null && searchCriteria.Count > 0)
            {
                string whereCondition = string.Empty;
                string andOperator = string.Empty;

                foreach (var searchField in searchCriteria)
                {
                    whereCondition = whereCondition + andOperator + DynamicSearchProvider.ApplySearchCriteria(searchField, query);
                    andOperator = " AND ";
                }
                query = query + whereCondition;
            }
            selectQuery = new StringBuilder();
            selectQuery.Append(query);
            selectQuery.Append(" GROUP BY ");
            selectQuery.Append(" HTMATL.MATL_GROUP_NO, ");
            selectQuery.Append(" MKR.SHORT_NAME, "); 
            selectQuery.Append(" MKR.MAKER_NAME,  ");
            selectQuery.Append(" HTMATL.APPLY_ROLL_YEAR, ");
            selectQuery.Append(" HTMATL.APPLY_ROLL_MONTH, ");
            selectQuery.Append(" HTMATL.APPLY_ROLL_CHANCE, ");
            selectQuery.Append(" HTMATL.MATL_KUBUN, ");
            selectQuery.Append(" MSK.SHORT_NAME, ");
            selectQuery.Append(" MSK.MUKESAKI_NAME ");
            
            query = string.Empty;
            query= selectQuery.ToString();
            if (!string.IsNullOrEmpty(sortCriteria))
            {
                query = Helper.ApplySortCriteria(sortCriteria, query);
            }
            else
            {
                query += "ORDER BY APPLICATION_DATE_STRING, MAKER_NAME, MUKESAKI_NAME, HTMATL.MATL_KUBUN ";
            }
            var result = ExecuteQueryMultiResult<MakerMaterialDetails>(query.ToString());
            return result;

        }
        public List<MakerMaterialDetails> GetMaterialDetailsBySearchCriteria(MakerMaterialDetails details)
        {
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT  ");
            selectQuery.Append(" MM.MATL_NAME_MAKER_CD, ");
            selectQuery.Append(" MM.MATL_MAKER_NAME, ");
            selectQuery.Append(" INT.INTERNAL_NAME AS MTL_NAME_INTERNAL, ");
            selectQuery.Append(" HC.DIM_START, ");
            selectQuery.Append(" HC.DIM_END, ");
            selectQuery.Append(" HC.RATE, ");
            selectQuery.Append(" HC.MATERIAL_RATE_CD, ");
            selectQuery.Append(" HC.MATL_GROUP_NO ");
            
            selectQuery.Append(" FROM MST_MATL_NAME_MAKER MM ");
            selectQuery.Append(" INNER JOIN MST_INTERNAL_NAME INT ON MM.INTERNAL_NAME_CD = INT.INTERNAL_NAME_CD ");
          
            selectQuery.Append(" LEFT OUTER JOIN( ");
            selectQuery.Append(" SELECT * FROM");
            selectQuery.Append(" MST_MATERIAL_RATE ");
            selectQuery.Append(" WHERE ");
            selectQuery.Append(" MUKESAKI_CD = " + details.MUKESAKI_CD);
            selectQuery.Append(" AND APPLY_ROLL_YEAR = " + details.APPLY_ROLL_YEAR);
            selectQuery.Append(" AND APPLY_ROLL_MONTH = " + details.APPLY_ROLL_MONTH);
            selectQuery.Append(" AND APPLY_ROLL_CHANCE = " + details.APPLY_ROLL_CHANCE);
            selectQuery.Append(" AND MATL_KUBUN = " + details.MATL_KUBUN);
            selectQuery.Append(" ) HC ");
            selectQuery.Append(" ON MM.MATL_NAME_MAKER_CD = HC.MATL_NAME_MAKER_CD ");

            selectQuery.Append(" WHERE ");
            
            selectQuery.Append(" MM.IN_ACTIVE_FLAG = 0  ");
            selectQuery.Append(" AND MM.MAKER_CD = " + details.MAKER_CD);
           
            selectQuery.Append(" GROUP BY  MM.MATL_NAME_MAKER_CD, MM.MATL_MAKER_NAME,INT.INTERNAL_NAME, HC.DIM_START, HC.DIM_END, HC.RATE, HC.MATERIAL_RATE_CD, HC.MATL_GROUP_NO");
            selectQuery.Append(" ORDER BY  ");
            selectQuery.Append("  INT.INTERNAL_NAME ASC, MATL_MAKER_NAME ASC ");


            var result = ExecuteQueryMultiResult<MakerMaterialDetails>(selectQuery.ToString());
            return result;

        }
        public List<MakerMaterialDetails> GetMaterialDetailsByGroupNo(int groupNo)
        {
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT  ");
            selectQuery.Append("  MTLR.MUKESAKI_CD ");
            selectQuery.Append(" ,MKR.MATL_NAME_MAKER_CD ");
            selectQuery.Append(" ,MTLR.APPLY_ROLL_YEAR AS APPLY_ROLL_YEAR ");
            selectQuery.Append(" ,MTLR.APPLY_ROLL_MONTH AS APPLY_ROLL_MONTH ");
            selectQuery.Append(" ,MTLR.APPLY_ROLL_CHANCE ");
            selectQuery.Append(" ,MTLR.DIM_START ");
            selectQuery.Append(" ,MTLR.DIM_END ");
            selectQuery.Append(" ,MTLR.RATE ");
            selectQuery.Append(" ,MTLR.MATL_GROUP_NO ");
            
            selectQuery.Append(" ,MTLR.MATL_KUBUN ");
            selectQuery.Append(" ,MKR.MAKER_CD ");
            selectQuery.Append(" ,MKR.MATL_MAKER_NAME ");
            selectQuery.Append(" ,INT.INTERNAL_NAME AS MTL_NAME_INTERNAL ");

            selectQuery.Append(" FROM MST_MATERIAL_RATE MTLR ");
          
            selectQuery.Append(" INNER JOIN MST_MATL_NAME_MAKER MKR ON MKR.MATL_NAME_MAKER_CD = MTLR.MATL_NAME_MAKER_CD  ");
            selectQuery.Append(" INNER JOIN MST_INTERNAL_NAME INT ON MKR.INTERNAL_NAME_CD = INT.INTERNAL_NAME_CD  ");
            selectQuery.Append(" WHERE ");
            selectQuery.Append(" MATL_GROUP_NO = " + groupNo);
            selectQuery.Append(" GROUP BY ");
            selectQuery.Append("  MTLR.MUKESAKI_CD ");
            selectQuery.Append(" ,MKR.MATL_NAME_MAKER_CD ");
            selectQuery.Append(" ,MTLR.APPLY_ROLL_YEAR ");
            selectQuery.Append(" ,MTLR.APPLY_ROLL_MONTH ");
            selectQuery.Append(" ,MTLR.APPLY_ROLL_CHANCE ");
            selectQuery.Append(" ,MTLR.DIM_START ");
            selectQuery.Append(" ,MTLR.DIM_END ");
            selectQuery.Append(" ,MTLR.RATE ");
            selectQuery.Append(" ,MTLR.MATL_KUBUN ");
            selectQuery.Append(" ,MKR.MAKER_CD ");
            selectQuery.Append(" ,MKR.MATL_MAKER_NAME ");
            selectQuery.Append(" ,INT.INTERNAL_NAME ");
            selectQuery.Append(" ,MTLR.MATL_GROUP_NO ");
            selectQuery.Append(" ORDER BY  ");
            selectQuery.Append("  INT.INTERNAL_NAME ASC, MATL_MAKER_NAME ASC ");
            var result = ExecuteQueryMultiResult<MakerMaterialDetails>(selectQuery.ToString());
            return result;
        }

    }
}
