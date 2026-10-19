using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using TSWebApplication.Application.IServices;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Infrastructure.IRepositories;
using TSWebApplication.Infrastructure.Repositories;


namespace TSWebApplication.Application.Services
{
    public class CopyTagFromHostToOracleScreenService:ICopyTagFromHostToOracleScreenService
    {
        private readonly ICopyTagFromHostToOracleScreenRepository _copyTagFromHostToOracleScreenRepository;
        private readonly IUserLoginRepository _userLogin;
        private List<GridSearchFields> request;

        public CopyTagFromHostToOracleScreenService(ICopyTagFromHostToOracleScreenRepository copyTagFromHostToOracleScreenRepository, IUserLoginRepository userLogin)
        {

            _copyTagFromHostToOracleScreenRepository = copyTagFromHostToOracleScreenRepository;
            _userLogin = userLogin;
        }
        public List<GridSearchFields> GetDetailsByNifuda(List<GridSearchFields> lstNifudaNo)
        {
            List<GridSearchFields> GridSearchFieldsList = new List<GridSearchFields>();

            var chubanDetailsList = _copyTagFromHostToOracleScreenRepository.GetDetailsByNifuda(lstNifudaNo);
            foreach (var item in chubanDetailsList)
            {
                //item.PROC_CONDITION_STRING = UIUtils.GetProcConditionKubunValue(Convert.ToString(item.HT_CONDITION));
                item.UNIT_TYPE_STRING = UIUtils.GetProductUnitKubunValue(Convert.ToString(item.UNIT));
                item.PROCESS_NAME_STRING = UIUtils.GetNextProcessName(Convert.ToString(item.NEXT_PROCESS_CD));
            }
            if (chubanDetailsList != null && chubanDetailsList.Count > 0)
            {

                foreach (var item in chubanDetailsList)
                {
                    var GridSearchFields = new GridSearchFields();

                    GridSearchFields.CUSTOMER_NAME = item.CUSTOMER_NAME;
                    GridSearchFields.PROD_NAME_CUST = item.PROD_NAME_CUST;
                    GridSearchFields.SHAPE = item.SHAPE;
                    GridSearchFields.PROD_DIM_D = item.PROD_DIM_D;
                    GridSearchFields.PROD_DIM_W = item.PROD_DIM_W;
                    GridSearchFields.PROD_DIM_OTHER_STRING = item.PROD_DIM_OTHER_STRING;
                    GridSearchFields.PROD_LENGTH = item.PROD_LENGTH;
                    GridSearchFields.HT_CONDITION = item.PROC_CONDITION_STRING;
                    GridSearchFields.CHARGE = item.CHARGE;
                    GridSearchFields.NO_OF_UNITS = item.NO_OF_UNITS;
                    GridSearchFields.ACT_ALLOC_QTY = item.ACT_ALLOC_QTY;
                    GridSearchFields.ACT_ALLOC_DATE = item.ACT_ALLOC_DATE;
                    GridSearchFields.PROD_SPEC_NO = item.PROD_SPEC_NO;
                    GridSearchFields.CHUUBAN_GYOUBAN = item.CHUUBAN_GYOUBAN;
                    GridSearchFieldsList.Add(GridSearchFields);
                }
            }
            return chubanDetailsList;
        }

        public string SaveNifudaData(List<GridSearchFields> processPlanDetails)
        {
            string message = "";
            using (TransactionScope transaction = new TransactionScope())
            {
                foreach (var request in processPlanDetails)
            {
                if (request.USER_ID != null && request.USER_ID > 0)
                {
                    request.INPUT_LOCATION_CD = Convert.ToInt16(_userLogin.GetUserLocationById(request.USER_ID.Value));
                    request.LOCATION_CD = request.INPUT_LOCATION_CD;
                }
            }
           
                message= _copyTagFromHostToOracleScreenRepository.SaveNifudaData(processPlanDetails);
                transaction.Complete();
            }
            return message;

        }
        public List<GridSearchFields> DeleteCopyTagFromHostToOracleData(List<GridSearchFields> tagDetails)
        {
            return _copyTagFromHostToOracleScreenRepository.DeleteCopyTagData(tagDetails);
        }
    }
}
