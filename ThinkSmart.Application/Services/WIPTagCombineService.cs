using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using TSWebApplication.Application.IServices;
using TSWebApplication.Domain.Model;
using TSWebApplication.Infrastructure.IRepositories;

namespace TSWebApplication.Application.Services
{
    public class WIPTagCombineService : IWIPTagCombineService
    {
        private readonly IWIPTagCombineRepository _tagCombineRepository;
        private readonly IAllCommonRepository _commonRepository;
        public WIPTagCombineService(IWIPTagCombineRepository tagCombineRepository, IAllCommonRepository commonRepository)
        {
            _tagCombineRepository = tagCombineRepository;
            _commonRepository = commonRepository;
        }
        public WIPTagCombineDetails GetTagDetailsByCombineCD(int combineCD)
        {
            return _tagCombineRepository.GetTagDetailsByCombineCD(combineCD);
        }
        public void DeleteTagCombineData(WIPTagCombineDetails request)
        {
            using (TransactionScope transaction = new TransactionScope())
            {

                if (Convert.ToInt32(request.TABA_COMBINE_CD) > 0)
                {
                    _tagCombineRepository.Delete(Convert.ToInt32(request.TABA_COMBINE_CD));
                }

                WipStatusDetails wipDetails = new WipStatusDetails();
                wipDetails.PROCESS_PLAN_CD = Convert.ToInt32(request.PROCESS_PLAN_CD);

                if (request.COMBINE_SOURCES_TABA_NO_1 > 0)
                {
                    wipDetails.TABA_NO = Convert.ToInt16(request.COMBINE_SOURCES_TABA_NO_1);
                    _commonRepository.DeleteWIPStatusData(wipDetails);
                }
                if (request.COMBINE_SOURCES_TABA_NO_2 > 0)
                {
                    wipDetails.TABA_NO = Convert.ToInt16(request.COMBINE_SOURCES_TABA_NO_2);
                    _commonRepository.DeleteWIPStatusData(wipDetails);
                }
                if (request.COMBINE_SOURCES_TABA_NO_3 > 0)
                {
                    wipDetails.TABA_NO = Convert.ToInt16(request.COMBINE_SOURCES_TABA_NO_3);
                    _commonRepository.DeleteWIPStatusData(wipDetails);
                }
                if (request.COMBINE_SOURCES_TABA_NO_4 > 0)
                {
                    wipDetails.TABA_NO = Convert.ToInt16(request.COMBINE_SOURCES_TABA_NO_4);
                    _commonRepository.DeleteWIPStatusData(wipDetails);
                }

                transaction.Complete(); 
            }
        }
    }
}
