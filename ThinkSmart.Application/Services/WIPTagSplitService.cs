using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using TSWebApplication.Application.IServices;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Infrastructure.IRepositories;

namespace TSWebApplication.Application.Services
{
    public class WIPTagSplitService : IWIPTagSplitService
    {
        private readonly IWIPTagSplitRepository _tagSplitRepository;
        private readonly IAllCommonRepository _commonRepository;
        public WIPTagSplitService(IWIPTagSplitRepository tagSplitRepository, IAllCommonRepository commonRepository)
        {
            _tagSplitRepository = tagSplitRepository;
            _commonRepository = commonRepository;
        }
        public WIPTagSplitDetails GetTagDetailsByDivisionCD(int divisionCD)
        {
            return _tagSplitRepository.GetTagDetails(divisionCD);
        }
        public void DeleteTagSplitData(WIPTagSplitDetails request)
        {
            using (TransactionScope transaction = new TransactionScope())
            {

                if (Convert.ToInt32(request.TABA_DIVISION_CD) > 0)
                {
                    _tagSplitRepository.Delete(Convert.ToInt32(request.TABA_DIVISION_CD));
                }


                WipStatusDetails wipDetails = new WipStatusDetails();
                wipDetails.PROCESS_PLAN_CD = Convert.ToInt32(request.PROCESS_PLAN_CD);

                if (request.DIVISION_TABA_NO_1 > 0)
                {
                    wipDetails.TABA_NO = Convert.ToInt16(request.DIVISION_TABA_NO_1);
                    _commonRepository.DeleteWIPStatusData(wipDetails);
                }

                if (request.DIVISION_TABA_NO_2 > 0)
                {
                    wipDetails.TABA_NO = Convert.ToInt16(request.DIVISION_TABA_NO_2);
                    _commonRepository.DeleteWIPStatusData(wipDetails);
                }
                if (request.DIVISION_TABA_NO_3 > 0)
                {
                    wipDetails.TABA_NO = Convert.ToInt16(request.DIVISION_TABA_NO_3);
                    _commonRepository.DeleteWIPStatusData(wipDetails);
                }
                if (request.DIVISION_TABA_NO_4 > 0)
                {
                    wipDetails.TABA_NO = Convert.ToInt16(request.DIVISION_TABA_NO_4);
                    _commonRepository.DeleteWIPStatusData(wipDetails);
                }
                transaction.Complete();
            }

        }
        
    }
}
