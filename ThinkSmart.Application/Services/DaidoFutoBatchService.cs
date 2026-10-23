using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using TSWebApplication.Application.IServices;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Infrastructure.IRepositories;
using TSWebApplication.Infrastructure.Repositories;

namespace TSWebApplication.Application.Services
{
    public class DaidoFutoBatchService : IDaidoFutoBatchService
    {
        private readonly IDaidoFutoBatchRepository _daidoFutoBatchRepository;
        private readonly IAllCommonRepository _CommonRepository;
        private readonly LocalizationServices _localization;
        private readonly IAllCommonService _commonService;
        public DaidoFutoBatchService(IDaidoFutoBatchRepository daidoFutoBatchRepository, IAllCommonRepository commonRepository, LocalizationServices localization, IAllCommonService commonService)
        {
            _daidoFutoBatchRepository = daidoFutoBatchRepository;
            _CommonRepository = commonRepository;
            _localization = localization;
            _commonService = commonService;
        }
        public List<NameValue> GetFutouList()
        {
            var _FutouList = _daidoFutoBatchRepository.GetFutouList();
            return _FutouList;
        }



        public DaidoFutouNyukoDetails DaidoNyukoEDIBatchDataInsert(List<DaidoFutouNyukoDetails> requestData, string cultureName, int userID)
        {
            List<DaidoFutouNyukoDetails> result = new List<DaidoFutouNyukoDetails>();
            DaidoFutouNyukoDetails data = new DaidoFutouNyukoDetails();
            bool checkDataExist = false;
            int inputLocationCd = _CommonRepository.GetUserLocationById(userID);
            DateTime dateTime = _commonService.GetSystemDateTime();
            string sysDate = Convert.ToString(dateTime.ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS));
            using (TransactionScope transaction = new TransactionScope())
            {
                if (cultureName == "en")
                {
                    System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
                }
                else
                {
                    System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("ja-JP");
                }
                foreach (var request in requestData)
                {
                    
                    bool samedataExist = _daidoFutoBatchRepository.ValidateSameDataExist(request);
                    if (samedataExist == true)
                    {
                        request.ERROR = _localization.GetLocalizedHtmlString("valMsg_SameDataExist");
                           
                        request.EXECUTE_RESULT = 2;
                        checkDataExist = true;

                    }
                    bool isTwoKoubanExist = _daidoFutoBatchRepository.IsKoubanMaterialNameMakDimDExists(request.ORDER_KOUBAN, request.STEEL_GRADE, request.DIMD_DECOMPOSITION_SIZE);// STEEL_GRADE=matlNameMaker
                    if (isTwoKoubanExist == true)
                    {
                        request.ERROR = _localization.GetLocalizedHtmlString("valMsg_koubanAlreadyExists");

                        request.EXECUTE_RESULT = 2;
                        checkDataExist = true;
                    }
                    request.PROCESS_DATE_AND_TIME = dateTime;
                    _daidoFutoBatchRepository.DaidoNyukoEDIBatchDataInsert(request, cultureName, userID, inputLocationCd);
                    data.PROCESS_DATE_AND_TIME = dateTime;
                    data.EXECUTE_RESULT = request.EXECUTE_RESULT;
                    if (checkDataExist == false)
                    {
                        if (request.EXECUTE_RESULT == 0)
                        {
                            if (!string.IsNullOrEmpty(request.ORDER_KOUBAN))
                            {
                                if (!string.IsNullOrEmpty(request.STEEL_GRADE))// STEEL_GRADE=matlNameMaker
                                {
                                    int koubanCd = _daidoFutoBatchRepository.GetKoubanCd(request.ORDER_KOUBAN, request.STEEL_GRADE, request.DIMD_DECOMPOSITION_SIZE);

                                    _daidoFutoBatchRepository.NyukoDataInsert(koubanCd, request);

                                    int nyukoNo = _daidoFutoBatchRepository.GetNyukoNo(koubanCd, request);

                                    _daidoFutoBatchRepository.UpdateNyukoNo(nyukoNo, request);
                                }
                            }

                        }
                    }                   
                }
                transaction.Complete();
            }
           return data;

        }

        public DaidoFutouNyukoDetails GetDataCount(DateTime ImportDate)
        {           
            var obj = _daidoFutoBatchRepository.GetDataCount(ImportDate);
            return obj;
        }

       
    }
   

    }
