using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Application.IServices;
using TSWebApplication.Application.Services;
using TSWebApplication.Infrastructure.IRepositories;
using TSWebApplication.Infrastructure.Repositories;

namespace TSWebApplication.Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<IStamppingService, StamppingService>();
            services.AddScoped<IChamfferService, ChamfferService>();
            services.AddScoped<IStraighteningService, StraighteningService>();
            services.AddScoped<IHikinukiBarToBarService,HikinukiBarToBarService>();
            services.AddScoped<IColdDrawingWireDrawingService, ColdDrawingWireDrawingService>();
            services.AddScoped<ITensileService, TensileService>();
            services.AddSingleton<IEmailSenderService,EmailSenderService>();
            services.AddScoped<IStraightening2Service, Straightening2Service>();
            services.AddScoped<IECService,ECService>();
            services.AddScoped<IUserLoginServices, UserLoginServices>();
            services.AddScoped<IHikinukiCoilToBarService, HikinukiCoilToBarService>();
            services.AddScoped<IMIHardnessService, MIHardnessService>();
            services.AddScoped<IWIPTensouService, WIPTensouService>();
            services.AddScoped<IAllCommonService, AllCommonService>();
            services.AddScoped<IHeatTreatmentService, HeatTreatmentService>();
            services.AddScoped<IPostHTInspectionService,PostHTInspectionServices>();
            services.AddScoped<IHardnessTemperingService,HardnessTemperingService>();
            services.AddScoped<IWarehouseToSupplierService, WarehouseToSupplierService>();
            services.AddScoped<IWIPTensouAcceptanceService,WIPTensouAcceptanceService>();
            services.AddScoped<IFinalInspectionService, FinalInspectionService>();
            services.AddScoped<IProcessPlanHandoverService, ProcessPlanHandoverService>();
            services.AddScoped<IProcessPlanDetails, ProcessPlanServices> ();
            services.AddScoped<INextProcessWaitingStatusSearchService, NextProcessWaitingStatusSearchService>();
            services.AddScoped<ISalesProductionStatusSearchService, SalesProductionStatusSearchService>();
            services.AddScoped<IProcessPlanHandoverHostService, ProcessPlanHandoverHostService>();
            services.AddScoped<IGeneralDailyProcessService, GeneralDailyProcessService>();
            services.AddScoped<IProcessDisplayConfigService, ProcessDisplayConfigService>();
            services.AddScoped<IProcessTypeRemarkUpdationService, ProcessTypeRemarkUpdationService>();
            services.AddScoped<IStoredProcedureDemoServices, StoredProcedureDemoServices>();
            services.AddScoped<ICopyTagFromHostToOracleScreenService, CopyTagFromHostToOracleScreenService>();
            services.AddScoped<ISessionHandlerServices, SessionHandlerServices>();
            services.AddScoped<IHSKMillSheetServices, HSKMillSheetServices>();
            services.AddScoped<IRollPoForecastService, RollPoForecastService>();
            services.AddScoped<IMakerMaterialService, MakerMaterialService>();
            services.AddScoped<IMakerMillSheetServices, MakerMillSheetServices>();
            services.AddScoped<IRollPoMainAnalysisServices, RollPoMainAnalysisServices>();
            services.AddScoped<IMatlNameWiseOrderQtyService, MatlNameWiseOrderQtyService>();
            services.AddScoped<IForecastStatusPopupService, ForecastStatusPopupService>();
            services.AddScoped<IProdSpecwiseSalesStatusService, ProdSpecwiseSalesStatusService>();
            services.AddScoped<IRollForcastPopupServices, RollForcastPopupServices>();
            services.AddScoped<IProdSpecwiseSalesStatusService, ProdSpecwiseSalesStatusService>();
            services.AddScoped<IWIPProcessDataServices, WIPProcessDataServices>();
            services.AddScoped<IPoAndReceiptHistoryPopUpServices, PoAndReceiptHistoryPopUpServices>();
            services.AddScoped<ICustomerWiseProdStockShukkaService, CustomerWiseProdStockShukkaService>();
            services.AddScoped<IAnalysePOQuantityServices, AnalysePOQuantityServices>();
            services.AddScoped<IReportsService, ReportsService>();
            services.AddScoped<ICSVGenerationService, CSVGenerationService>();
            services.AddScoped<IDaidoFutoBatchService, DaidoFutoBatchService>();
            services.AddScoped<IYieldSearchService, YieldSearchService>();
            services.AddScoped<IMakerMillSheetEDIServices, MakerMillSheetEDIService>();
            services.AddScoped<IVehicleMasterService, VehicleMasterService>(); 
            services.AddScoped<IMakerMillSheetJfeEDIService, MakerMillSheetJfeEDIService>();
            services.AddScoped<IProductTensouService, ProductTensouService>();
            services.AddScoped<IJFENyukoNyukaBatchService, JFENyukoNyukaBatchService>();
            services.AddScoped<IWIPTagSplitService, WIPTagSplitService>();
            services.AddScoped<IWIPTagCombineService, WIPTagCombineService>();

            return services;
        }
    }
}
