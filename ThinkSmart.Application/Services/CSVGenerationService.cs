using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using TSWebApplication.Application.IServices;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Infrastructure.IRepositories;
using TSWebApplication.Infrastructure.Repositories;

namespace TSWebApplication.Application.Services
{
    public class CSVGenerationService : ICSVGenerationService
    {
        private readonly ICSVGenerationRepository _csvRepository;
        private readonly LocalizationServices _localization;
        public CSVGenerationService(ICSVGenerationRepository csvGenerationsitory, LocalizationServices localization)
        {
            _localization = localization;
            _csvRepository = csvGenerationsitory;
        }
        public List<OutputCSVDetails> GetCSVGroupTitle()
        {
            return _csvRepository.GetGroupTitleList();

        }
        public  List<OutputCSVDetails> GetCSVOutputTitleList(string groupTitleName)
        {
         return  _csvRepository.GetCSVTitleList(groupTitleName);

        }
        public List<OutputCSVDetails> GetConditionsList(int outputCD)
        {
            return _csvRepository.GetConditionsList(outputCD);
        }
       
        public DataTable GetCSVData(string query)
        {
            return _csvRepository.GetData(query);
        }
        public DataTable GetCSVProcedureData(StoredProcedureDetails pDetails)
        {
            return _csvRepository.GetCSVProcedureData(pDetails);
        }
        public List<NameValue> GetLocationNameList()
        {
            var locationNameList = _csvRepository.GetLocationNameList();
            return locationNameList;
        }
        public List<NameValue> GetNextProcessList()
        {
            int nextProcessCD = Convert.ToInt32(UIUtils.GetProcessKubun(_localization.GetLocalizedHtmlString("lbl_ProcessWIP_Forwarding")));
            var locationNameList = _csvRepository.GetNextProcessList(nextProcessCD);
            return locationNameList;
        }
    }
}
