using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Application.IServices;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Infrastructure.IRepositories;
using TSWebApplication.Infrastructure.Repositories;

namespace TSWebApplication.Application.Services
{
    public class PoAndReceiptHistoryPopUpServices : IPoAndReceiptHistoryPopUpServices
    {
        private readonly IPoAndReceiptHistoryPopUpRepository _poAndReceiptHistoryPopUpRepository;
        private readonly IAllCommonRepository _allCommonRepository;
        private readonly IUserLoginRepository _userLoginRepository;
        private readonly LocalizationServices _localization;
        private readonly IAllCommonService _allCommonService;

        public PoAndReceiptHistoryPopUpServices(IPoAndReceiptHistoryPopUpRepository poAndReceiptHistoryPopUpRepository, IAllCommonRepository allCommonRepository, IUserLoginRepository userLoginRepository, LocalizationServices localization, IAllCommonService allCommonService)
        {
            _poAndReceiptHistoryPopUpRepository = poAndReceiptHistoryPopUpRepository;
            _allCommonRepository = allCommonRepository;
            _userLoginRepository = userLoginRepository;
            _localization = localization;
            _allCommonService = allCommonService;
        }
        public List<PoAndReceiptPopupDetails> GetMakerWisePurchaseOrderQtyList(int Batch_Roll)
        {
            var obj = _poAndReceiptHistoryPopUpRepository.GetMakerWisePurchaseOrderQtyList(Batch_Roll);
            return obj;
        }
        public List<PoAndReceiptPopupDetails> GetVendorWisePurchaseOrderQtyList(int Batch_Roll)
        {
            var obj = _poAndReceiptHistoryPopUpRepository.GetVendorWisePurchaseOrderQtyList(Batch_Roll);
            return obj;
        }
    }
}
