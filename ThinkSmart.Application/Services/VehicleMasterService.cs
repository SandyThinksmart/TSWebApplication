using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using TSWebApplication.Application.IServices;
using TSWebApplication.Infrastructure.IRepositories;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Infrastructure.Repositories;
using System.Reflection.PortableExecutable;
using System.Globalization;
using TSWebApplication.Domain.Resources;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Asn1.Cmp;
using System.Data;

namespace TSWebApplication.Application.Services
{
    public class VehicleMasterService: IVehicleMasterService
    {
        private readonly IVehicleMasterRepository _vehicleMasterRepository;
        private readonly LocalizationServices _localization;
        private readonly IAllCommonRepository _allCommonRepository;
        private readonly IAllCommonService _allCommonService;
        public VehicleMasterService(IVehicleMasterRepository vehicleMasterRepository, LocalizationServices localization, IAllCommonRepository allCommonRepository, IAllCommonService allCommonService)
        {
            _vehicleMasterRepository = vehicleMasterRepository;
            _localization = localization;
            _allCommonRepository = allCommonRepository;
            _allCommonService = allCommonService;

        }
        public long registerVehicle(VehicleDetails request)
        {
            VehicleDetails result = new VehicleDetails();
            return  _vehicleMasterRepository.registerVehicle(request);

        }
        public VehicleDetails GetVehicleById(long vehicleId)
        {
            return _vehicleMasterRepository.GetVehicleById(vehicleId);
        }

        public bool DeleteVehicleDetailsById(long vehicleId)
        {
            return _vehicleMasterRepository.DeleteVehicleDetailsById(vehicleId);
        }
        public bool ChangeVehicleStatus(long vehicleId)
        {
            return _vehicleMasterRepository.ChangeVehicleStatus(vehicleId);
        }
        //public List<VehicleDetails> FetchVehicleDetails()
        //{
        //    return _vehicleMasterRepository.FetchVehicleDetails();
        //}
        public List<VehicleDetails> FetchVehicleDetails(GridSearchFields gridSearchRequest)
        {
            List<SearchField> searchFields = ComposeSearchCriteria(gridSearchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);

            return _vehicleMasterRepository.FetchVehicleDetails(searchFields, selectedSortCriteria);
        }
        public long UpdateVehicleDetails(VehicleDetails request)
        {
            return _vehicleMasterRepository.UpdateVehicleDetails(request);
        }

        private List<SearchField> ComposeSearchCriteria(GridSearchFields gridSearchRequest)
        {
            var searchFields = new List<SearchField>();

                       
            if (gridSearchRequest.VEHICLE_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "VEHICLE_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.VEHICLE_CD)
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.VEHICLE_NAME))
            {
                var searchField = new SearchField
                {
                    FieldName = "VEHICLE_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.VEHICLE_NAME.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }

            
            if (!string.IsNullOrEmpty(gridSearchRequest.SHORT_NAME))
            {
                var searchField = new SearchField
                {
                    FieldName = "SHORT_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.SHORT_NAME.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }

           
            if (!string.IsNullOrEmpty(gridSearchRequest.YOBIGANA))
            {
                var searchField = new SearchField
                {
                    FieldName = "YOBIGANA",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.YOBIGANA.Trim()
                };

                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.YOBINA))
            {
                var searchField = new SearchField
                {
                    FieldName = "YOBINA",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.YOBINA.Trim()
                };

                searchFields.Add(searchField);
            }

            
            if (!string.IsNullOrEmpty(gridSearchRequest.DRIVER_NAME))
            {
                var searchField = new SearchField
                {
                    FieldName = "DRIVER_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.DRIVER_NAME.Trim()

                };
                searchFields.Add(searchField);
            }
            
            return searchFields;
        }
    }
}
