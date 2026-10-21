using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using TSWebApplication.Application.IServices;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Infrastructure.IRepositories;
using TSWebApplication.Infrastructure.Repositories;

namespace TSWebApplication.Application.Services
{
    public class ProcessPlanHandoverHostService : IProcessPlanHandoverHostService
    {
        private readonly IProcessPlanHandoverHostRepository _processPlanHandoverHostRepository;
        private readonly IAllCommonRepository _CommonRepository;
        public ProcessPlanHandoverHostService(IProcessPlanHandoverHostRepository processPlanHandoverHostRepository, IAllCommonRepository commonRepository)
        {
            _processPlanHandoverHostRepository = processPlanHandoverHostRepository;
            _CommonRepository = commonRepository;
        }
        public List<NameValue> GetNextProcessList()
        {
            //var nextProcessNameList = _processPlanHandoverRepository.GetNextProcessList();
            var nextProcessNameList = UIUtils.GetNextProcesKubun();
            return nextProcessNameList;
        }
        public List<GridSearchFields> ValidateChuuban(List<GridSearchFields> processPlanDetails)
        {
            List<GridSearchFields> GridSearchFieldsList = new List<GridSearchFields>();
           
            var chubanDetailsList = _processPlanHandoverHostRepository.ValidateChuuban(processPlanDetails);
            foreach (var item in chubanDetailsList)
            {
               // item.PROC_CONDITION_STRING = UIUtils.GetProcConditionKubunValue(Convert.ToString(item.HT_CONDITION));
                item.UNIT_TYPE_STRING = UIUtils.GetProductUnitKubunValue(Convert.ToString(item.UNIT));
               
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
                    GridSearchFieldsList.Add(GridSearchFields);
                }
            }
            return chubanDetailsList;

        }
        public List<GridSearchFields> GetProcessPlanDetails(List<GridSearchFields> processPlanDetails)
        {
            List<GridSearchFields> GridSearchFieldsList = new List<GridSearchFields>();

            var chubanDetailsList = _processPlanHandoverHostRepository.GetProcessPlanDetails(processPlanDetails);
            foreach (var item in chubanDetailsList)
            {
                // item.PROC_CONDITION_STRING = UIUtils.GetProcConditionKubunValue(Convert.ToString(item.HT_CONDITION));
                item.UNIT_TYPE_STRING = UIUtils.GetProductUnitKubunValue(Convert.ToString(item.UNIT));

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
                    GridSearchFieldsList.Add(GridSearchFields);
                }
            }
            return chubanDetailsList;

        }
        public string SaveProcessPlanHandoverData(List<GridSearchFields> processPlanDetails)
        {
            string message = "";
            using (TransactionScope transaction = new TransactionScope())
            {
                message = _processPlanHandoverHostRepository.SaveProcessPlanHandoverData(processPlanDetails);
                transaction.Complete();
            }
            return message;   
        }
        public List<GridSearchFields> DeleteProcessPlanHandoverData(List<GridSearchFields> processPlanDetails)
        {
            return _processPlanHandoverHostRepository.DeleteProcessPlanHandoverData(processPlanDetails);
        }
        
        public List<GridSearchFields> ValidateDataExist(List<GridSearchFields> processPlanDetails)
        {
            return _processPlanHandoverHostRepository.ValidateDataExist(processPlanDetails);
        }
        public List<GridSearchFields> ValidateDataForUpdate(List<GridSearchFields> processPlanDetails)
        {
            return _processPlanHandoverHostRepository.ValidateDataForUpdate(processPlanDetails);
        }
        public string UpdateProcessPlanHandoverData(List<GridSearchFields> processPlanDetails)
        {
            string message = "";
            using (TransactionScope transaction = new TransactionScope())
            {
                message = _processPlanHandoverHostRepository.UpdateProcessPlanHandoverData(processPlanDetails);
                transaction.Complete();
            }
            return message; 
        }
        public List<GridSearchFields> ValidateDataForAddNewBtn(List<GridSearchFields> processPlanDetails)
        {
            return _processPlanHandoverHostRepository.ValidateDataForAddNewBtn(processPlanDetails);
        }
    }
}
