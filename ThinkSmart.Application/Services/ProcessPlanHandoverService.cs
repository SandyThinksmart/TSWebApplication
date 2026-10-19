using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Application.IServices;
using TSWebApplication.Infrastructure.IRepositories;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;

namespace TSWebApplication.Application.Services
{
    public class ProcessPlanHandoverService :IProcessPlanHandoverService
    {
        private readonly IProcessPlanHandoverRepository _processPlanHandoverRepository;
        public ProcessPlanHandoverService(IProcessPlanHandoverRepository processPlanHandoverRepository)
        {
            _processPlanHandoverRepository = processPlanHandoverRepository;
        }
            public List<NameValue> GetNextProcessList()
        {
            //var nextProcessNameList = _processPlanHandoverRepository.GetNextProcessList();
            var nextProcessNameList = UIUtils.GetNextProcesKubun();
            return nextProcessNameList;
        }
        public List<GridSearchFields> GetDetailsByChuuban(List<GridSearchFields> processPlanDetails)
        {
            List<GridSearchFields> GridSearchFieldsList = new List<GridSearchFields>();
            var chubanDetailsList = _processPlanHandoverRepository.GetDetailsByChuuban(processPlanDetails);
            foreach (var item in chubanDetailsList)
            {
                item.PROC_CONDITION_STRING = UIUtils.GetProcConditionKubunValue(Convert.ToString(item.HT_CONDITION));
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
                    GridSearchFieldsList.Add(GridSearchFields);
                }
            }
            return chubanDetailsList;
        }
        public string SaveProcessPlanHandoverData(List<GridSearchFields> processPlanDetails)
        {
          return  _processPlanHandoverRepository.SaveProcessPlanHandoverData(processPlanDetails);
        }
        public List<GridSearchFields> DeleteProcessPlanHandoverData(List<GridSearchFields> processPlanDetails)
        {
           return _processPlanHandoverRepository.DeleteProcessPlanHandoverData(processPlanDetails);
        }

    }
}
