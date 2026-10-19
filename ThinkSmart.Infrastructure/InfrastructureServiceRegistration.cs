using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TSWebApplication.Infrastructure.IRepositories;
using TSWebApplication.Infrastructure.Repositories;

namespace TSWebApplication.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<IStamppingRepository, StamppingRepository>();
            services.AddScoped<IChamfferRepository, ChamfferRepository>();
            services.AddScoped<IStraighteningRepository, StraighteningRepository>();
            services.AddScoped<IHikinukiBarToBarRepository,HikinukiBarToBarRepository>();
            services.AddScoped<IColdDrawingWireDrawingRepository, ColdDrawingWireDrawingRepository>();
            services.AddScoped<ITensileRepository, TensileRepository>();
            services.AddScoped<IStraightening2Repository, Straightening2Repository>();
            services.AddScoped<IECRepository, ECRepository>();
            services.AddScoped<IUserLoginRepository, UserLoginRepository>();
            services.AddScoped<IHikinukiCoilToBarRepository, HikinukiCoilToBarRepository>();
            services.AddScoped<IMIHardnessRepository, MIHardnessRepository>();
            services.AddScoped<IWIPTensouRepository, WIPTensouRepository>();
            services.AddScoped<IWIPTensouAcceptanceRepository, WIPTensouAcceptanceRepository>();
            services.AddScoped<IAllCommonRepository, AllCommonRepository>();
            services.AddScoped<IHeatTreatmentRepository, HeatTreatmentRepository>();
            services.AddScoped<IPostHTInspectionRepository,PostHTInspectionRepository>();
            services.AddScoped<IHardnessTemperingRepository, HardnessTemperingRepository>();
            services.AddScoped<IWarehouseToSupplierRepository, WarehouseToSupplierRepository>();
            services.AddScoped<IFinalInspectionRepository, FinalInspectionRepository>();
            services.AddScoped<IProcessPlanHandoverRepository, ProcessPlanHandoverRepository>();
            services.AddScoped<IProcessPlanReportPrintReprintRepository, ProcessPlanReportPrintReprintRepository>();
            services.AddScoped<INextProcessWaitingStatusSearchRepository, NextProcessWaitingStatusSearchRepository>();
            services.AddScoped<ISalesProductionStatusSearchRepository, SalesProductionStatusSearchRepository>();
            services.AddScoped<IProcessPlanHandoverHostRepository, ProcessPlanHandoverHostRepository>();
            services.AddScoped<IGeneralDailyProcessRepository, GeneralDailyProcessRepository>();
            services.AddScoped<IProcessDisplayConfigRepository, ProcessDisplayConfigRepository>();
            services.AddScoped<IProcessTypeRemarkUpdationRepository, ProcessTypeRemarkUpdationRepository>();
            services.AddScoped<IStoredProcedureDemoRepository, StoredProcedureDemoRepository>();
            services.AddScoped<ICopyTagFromHostToOracleScreenRepository, CopyTagFromHostToOracleScreenRepository>();
            services.AddScoped<ISessionHandlerRepository, SessionHandlerRepository>();
            services.AddScoped<IHSKMillSheetRepository, HSKMillSheetRepository>();
            services.AddScoped<IRollPoForecastRepository, RollPoForecastRepository>();
            services.AddScoped<IMakerMaterialRepository, MakerMaterialRepository>();
            services.AddScoped<IMakerMillSheetRepository, MakerMillSheetRepository>();
            services.AddScoped<IRollPoMainAnalysisRepository, RollPoMainAnalysisRepository>();
            services.AddScoped<IMatlNameWiseOrderQtyRepository, MatlNameWiseOrderQtyRepository>();
            services.AddScoped<IForecastStatusPopupRepository, ForecastStatusPopupRepository>();
            services.AddScoped<IProdSpecwiseSalesStatusRepository, ProdSpecwiseSalesStatusRepository>();
            services.AddScoped<IRollForcastPopupRepository, RollForcastPopupRepository>();
            services.AddScoped<IProdSpecwiseSalesStatusRepository, ProdSpecwiseSalesStatusRepository>();
            services.AddScoped<IWIPProcessDataRepository, WIPProcessDataRepository>();
            services.AddScoped<IPoAndReceiptHistoryPopUpRepository, PoAndReceiptHistoryPopUpRepository>();
            services.AddScoped<ICustomerWiseProdStockShukkaRepository, CustomerWiseProdStockShukkaRepository>();
            services.AddScoped<IAnalysePOQuantityRepository, AnalysePOQuantityRepository>();
            services.AddScoped<IReportsRepository, ReportsRepository>();
            services.AddScoped<ICSVGenerationRepository, CSVGenerationRepository>(); 
            services.AddScoped<IDaidoFutoBatchRepository, DaidoFutoBatchRepository>();
            services.AddScoped<IYieldSearchRepository, YieldSearchRepository>();
            services.AddScoped<IMakerMillSheetEDIRepository, MakerMillSheetEDIRepository>(); 
            services.AddScoped<IVehicleMasterRepository, VehicleMasterRepository>();
            services.AddScoped<IMakerMillSheetJfeEDIRepository, MakerMillSheetJfeEDIRepository>();
            services.AddScoped<IProductTensouRepository, ProductTensouRepository>();
            services.AddScoped<IJFENyukoNyukaBatchRepository, JFENyukoNyukaBatchRepository>();
            services.AddScoped<IWIPTagSplitRepository, WIPTagSplitRepository>();
            services.AddScoped<IWIPTagCombineRepository, WIPTagCombineRepository>();

            return services;
        }
    }
}
