using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;
using TSWebApplication.Domain.Resources;

namespace TSWebApplication.Domain.Utility
{
    public class UIConstants
    {

        private static LocalizationServices _localizationServices;
        private static IHttpContextAccessor _httpContext;
        public static void UIConstantsConfigure(LocalizationServices localizationServices, IHttpContextAccessor httpContext)
        {
            _localizationServices = localizationServices;
            _httpContext = httpContext;
        }
        public const int K_ORDERED = 1;  // in product master old values are 1,2, 3 // in kubun 10,20 , 30
        public const int K_SHIP_AFTER_PRODUCTION = 2;
        public const int K_SPOT_ORDER = 3;

        // Product management kubun
        public const int K_FIRST_GOOD = 10;
        public const int K_REPLACE_GOOD = 20;

        // Storage direction kubun
        public const int K_FACTORY_STORAGE = 10;
        public const int K_BRANCH_STORAGE = 20;
        public const int K_QUICK_DELIVERY = 30;
        public const int K_STRG_MGMT_TYPE_ORDERED = 40;
        public const int K_NO_INSTRUCTION = 99;

        // Special management kubun 
        public const int K_NEW_GOODS = 10;
        public const int K_PROTOTYPE_GOODS = 20;
        public const int K_SPECIALMGMT_GOODS = 30;

        // Storage management kubun 
        public const int K_PLANNEDSTOCK = 10;
        public const int K_QUICKDISPATCH = 20;
        public const int K_SPOTORDER = 30;

        // Process plan kubun
        public const int K_GENERAL = 10;
        public const int K_REPROCESS = 20;
        public const int K_ADDEDPROCESS = 30;
        public const int K_CUTTING = 40;

        // Material type Kubun
        public const int K_MATERIAL_TYPE_COIL = 1;
        public const int K_MATERIAL_TYPE_BAR = 2;
        public const int K_MATERIAL_TYPE_KUROKAWA = 3;

        // Material Maker Kubun
        public const int K_MATERIAL_COIL = 1;
        public const int K_MATERIAL_BAR_KUROKAWA = 2;
        
        // General Special Kubun
        public const int K_GENSPEC_GENERAL = 1;
        public const int K_GENSPEC_SPECIAL = 2;

        // Unit Kubun
        public const int K_UNIT_BAR = 1;
        public const int K_UNIT_BUNDLE = 2;
        public const int K_UNIT_PIECES = 3;
        public const int K_UNIT_COIL = 4;
        public const int K_UNIT_BG = 5;
        public const int K_UNIT_CS = 6;
        public const int K_UNIT_CA = 7;
        public const int K_UNIT_DJ = 8;
        public const int K_UNIT_RO = 9;
        public const int K_UNIT_SE = 10;
        public const int K_UNIT_S = 11;

        // Chemical Specification  Kubun
        public const int K_Chem_C = 1;
        public const int K_Chem_Sl = 2;
        public const int K_Chem_Mn = 3;
        public const int K_Chem_P = 4;
        public const int K_Chem_S = 5;
        public const int K_Chem_Cu = 6;
        public const int K_Chem_Nl = 7;
        public const int K_Chem_Cr = 8;
        public const int K_Chem_Mo = 9;
        public const int K_Chem_NlCr = 10;


        // Payble Days Kubun
        public const int K_DAYS_EARLY = 1;
        public const int K_DAYS_EARLY_OR_LATER = 2;
        public const int K_DAYS_LATER = 3;

        // Kouban reply Kubun
        public const int K_KOUBAN_REPLY_REGULAR = 1;
        public const int K_KOUBAN_REPLY_SPLIT = 2;
        public const int K_KOUBAN_REPLY_WT_DELV_DATE_CHANGE = 3;
        public const int K_KOUBAN_REPLY_DELV_PLACE_CHANGE = 4;
        public const int K_KOUBAN_REPLY_CANCEL = 5;
        public const int K_KOUBAN_REPLY_UPDATE_MISTAKE = 6;
        public const int K_KOUBAN_REPLY_DELETE_MISATKE = 7;
        public const int K_KOUBAN_REPLY_OTHER_CHANGES = 8; //99

        // Billing Kubun
        public const int K_BILLING_SALES = 10;
        public const int K_BILLING_SALES_INTERNAL_PROCESSING = 11;
        public const int K_BILLING_RETURN_HENPIN = 20;
        public const int K_BILLING_RETURN_HENPIN_INTERNAL_PROCESSING = 21;
        public const int K_BILLING_UNIT_PRICE_CORRECTION = 30;
        public const int K_BILLING_UNIT_PRICE_CORRECT_INTERN_P = 31;
        public const int K_BILLING_AMOUNT_CORRECTION = 40;
        public const int K_BILLING_AMT_CORREC_INTERNAL_PROCESSING = 41;
        public const int K_BILLING_WEIGHT_CORRECTION = 50;
        public const int K_BILLING_WT_CORRE_INTERNAL_PROCESSING = 51;
        public const int K_BILLING_DISCOUNT_CORRECTION = 60;
        public const int K_BILLING_DISC_CORREC_INTERNAL_PROCESSING = 61;
        public const int K_BILLING_ACCEPTANCE_INSPECTION_DIFFERENCE = 70;
        public const int K_BILLING_ACCEPTANCE_INSP_DIFF_INTERNAL_PROCESSING = 71;
        public const int K_BILLING_FARE = 80;
        public const int K_BILLING_FARE_INTERNAL_PROCESSING = 81;
        public const int K_BILLING_CUTTING_FARE = 90;
        public const int K_BILLING_CUTTING_FARE_INTERNAL_PROCESSING = 91;
        public const int K_BILLING_DIFFERENCE_IN_CALCULATION = 100;
        public const int K_BILLING_DIFF_IN_CALCULATION_INTERNAL_PROCESSING = 101;
        public const int K_BILLING_COMMISSION = 110;
        public const int K_BILLING_COMM_INTERNAL_PROCESSING = 111;
        public const int K_BILLING_INTEREST_RATE = 120;
        public const int K_BILLING_INTEREST_RATE_INTERNAL_PROCESSING = 121;
        public const int K_BILLING_CLAIM = 130;
        public const int K_BILLING_CLAIM_INTERNAL_PROCESSING = 131;
        //public const int K_BILLING_OTHER = 270;
        //public const int K_BILLING_OTHER_INTERNAL_PROCESSING = 280;

        // Rate Formula kubun
        public const int K_RATE_FORMULA_QTYｘRATE = 1;
        public const int K_RATE_FORMULA_PIECESｘRATE = 2;
        public const int K_RATE_FORMULA_SPECIFIED = 3;

        // Payment mode kubun
        public const int K_PAYMENT_MODE_ACCOUNT_TRN = 1;
        public const int K_PAYMENT_MODE_CASH_TRN = 2;

        // Locality kubun
        public const int K_LOCALITY_DOMESTIC = 1;
        public const int K_LOCALITY_OVERSEAS = 2;

        // TP test kubun
        public const int K_TPTEST_NONE = 1;
        public const int K_TPTEST_150 = 2;
        public const int K_TPTEST_450 = 3;

        // TP test kubun EC
        public const int K_TPTEST_OK = 1;
        public const int K_TPTEST_NA = 2;

        //Hrd Temp kubun
        public const int H_TEMP_WATER = 10;
        public const int H_TEMP_LOW = 20;
        public const int H_TEMP_MIDDLE = 30;
        public const int H_TEMP_HIGHT = 40;

        public const int H_TEXTURE_M = 1;
        public const int H_TEXTURE_M_B = 2;
        public const int H_TEXTURE_M_FE = 3;
        public const int H_TEXTURE_M_B_FE = 4;
        public const int H_TEXTURE_M_B_FE1 = 5;
        public const int H_TEXTURE_UNCONDITIONAL = 6;




        // TO DO : Refer kubun  doc
        // Bend state kubun 
        public const int K_BEND_STATUS_OK = 10;
        public const int K_BEND_STATUS_OK2 = 15;
        public const int K_BEND_STATUS_OK3 = 20;
        public const int K_BEND_STATUS_HOLD = 25;
        public const int K_BEND_STATUS_STRAIGHTENING = 30;
        public const int K_BEND_STATUS_MULTIROLL = 35;
        public const int K_BEND_STATUS_OTHER = 99;

        // Exterior condition kubun
        public const int K_EXT_COND_OK = 10;
        public const int K_EXT_COND_HOLD = 15;
        public const int K_EXT_COND_MATERIAL_SCRATCH = 20;
        public const int K_EXT_COND_HELIX_CORRECTION = 25;
        public const int K_EXT_COND_OUT_OF_DIM = 30;
        public const int K_EXT_COND_SURFACE_CORRECTION = 35;
        // public const int K_EXT_COND_DIMENSSION_CORRECTION = 5;
        public const int K_EXT_COND_RUSTED = 40;
        public const int K_EXT_COND_DICE_SCRATCH = 45;
        public const int K_EXT_COND_JAW_SCRATCH = 50;
        public const int K_EXT_COND_CUTTER_SCRATCH = 55;
        public const int K_EXT_COND_ROLLER_SCRATCH = 60;
        public const int K_EXT_COND_OTHER = 99; //99

        // PO kubun
        public const int K_PO_New = 1;
        public const int K_PO_CHANGE_NOT_REGISTERED = 2;
        public const int K_PO_DELETE_NOT_REGISTERED = 3;
        public const int K_PO_CHANGE_REGISTERED = 4;
        public const int K_PO_DELETE_REGISTERED = 5;

        // Shukka kubun
        public const int K_SHUKKA_GENERAL = 10;
        public const int K_SHUKKA_SUBSTITUTE = 20;

        // Bend Tolerance kubun
        public const int K_BEND_HEXAGON = 1;
        public const int K_BEND_ROUND = 2;
        public const int K_BEND_HEXAGON_HOST = 3;
        public const int K_BEND_ROUND_HOST = 4;

        // Result kubun
        public const int K_RESULT_OK = 10;
        public const int K_RESULT_NG = 20;
        public const int K_RESULT_HOLD = 30;
        public const int K_RESULT_UNTESTED = 35;
        public const int K_RESULT_PROC_ADDITION = 40;
        public const int K_RESULT_REHEATTREATMENT = 60;
        public const int K_RESULT_TEMPAGAIN = 65;
        public const int K_RESULT_REDRAWING = 70;
        public const int K_RESULT_RESTRAIGHTENING = 73;
        public const int K_RESULT_RECHAMFERING = 76;
        public const int K_RESULT_RE_EC = 79;
        public const int K_RESULT_SPARKUNTESTED = 80;
        public const int K_RESULT_PRODRECIEPT = 90;


        // Cutting kubun
        public const int K_CUTTING_CONSTANT = 1;
        public const int K_CUTTING_TWO_CUT = 2;
        public const int K_CUTTING_THREE_CUT = 3;


        // Shape kubun
        public const int K_SHAPE_R = 0;
        public const int K_SHAPE_H = 1;
        public const int K_SHAPE_S = 2;
        public const int K_SHAPE_K = 3;

        // Location kubun
        // TO DO : Update by doc
        public const int K_LOCATION_HEADOFFICE = 1; //10
        public const int K_LOCATION_BRANCHOFFICE = 2; //20
        public const int K_LOCATION_SALESOFFICE = 3; //30
        public const int K_LOCATION_FACTORY = 4; //40

        // Location Type
        public const int K_LOC_HEADOFFICE = 10; //10
        public const int K_LOC_BRANCHOFFICE = 20; //20
        public const int K_LOC_SALESOFFICE = 30; //30
        public const int K_LOC_FACTORY = 40; //40

        // Kouban search kubun
        public const int K_ALL_KOB_REPLY = 1;
        public const int K_BEFORE_KOB_REPLY = 2;
        public const int K_AFTER_KOB_REPLY = 3;

        // Packing kubun
        public const int K_PACKING_PPCRAFT = 1;
        public const int K_PACKING_STEELBOX = 2;
        public const int K_PACKING_WOODENBOX = 3;
        public const int K_PACKING_STEELWIREBINDING = 4;
        public const int K_PACKING_POLYMERWIREBINDING = 5;

        // Shukka inst Special kubun
        public const int K_SPECIFIED = 1;
        public const int K_NOTSPECIFIED = 2;

        // Operational kubun
        public const int K_STOCKING = 1;
        public const int K_SCRAP = 2;
        public const int K_SHIPMENT = 3;

        // Product Shukko kubun 
        public const int K_SHUKKO_GENERAL = 10;
        public const int K_SHUKKO_SPLIT = 15;
        public const int K_SHUKKO_CHANGE = 16;
        public const int K_SHUKKO_USEASMATERIAL = 20;
        public const int K_SHUKKO_SUBCONTRACTOR = 30;
        public const int K_SHUKKO_STOCKTAKING = 80;
        public const int K_SHUKKO_SCRAP = 90;
        public const int K_SHUKKO_OTHER = 99;

        public const int K_SHUKKO_OTHER1 = 21;

        // Product Nyuko kubun 
        public const int K_PROD_NYUKO_FI = 10;
        public const int K_PROD_NYUKO_NOTINSPECTED = 11;
        public const int K_PROD_NYUKO_SPLIT = 15;
        public const int K_PROD_NYUKO_CHANGE = 16;
        public const int K_PROD_NYUKO_USEASPRODUCT = 20;
        public const int K_PROD_NYUKO_SUBCONTRACTOR = 30;
        public const int K_PROD_NYUKO_STOCKTAKING = 80;
        public const int K_PROD_NYUKO_OTHER = 99;

        // Nyuko kubun 
        public const int K_MATL_NYUKO_GENERAL = 10;
        public const int K_MATL_NYUKO_USEASMATERIAL = 20;
        public const int K_MATL_NYUKO_SUBCONTRACTOR = 30;
        public const int K_MATL_NYUKO_STOCKTAKING = 80;
        public const int K_MATL_NYUKO_OTHER = 99;

        // Material Product kubun 
        public const int K_SELF_RECEIVED = 1;
        public const int K_RECEIVED_WITH_PAYMENT = 2;
        public const int K_RECEIVED_FREE = 3;
        public const int K_PURCHASED = 4;
        public const int K_RAW_MATL = 5;

        // Process kubun 
        public const int K_PROCESS_SPLIT = 10;
        public const int K_RECEIVED_SPLIT_REPLACEMENT = 20;
        public const int K_PROCESS_REPLACEMENT = 30;


        // Prodct Unit kubun 
        public const int K_PRODUCT_UNIT = 1;
        public const int K_PRODUCT_UNIT_RANGE = 2;
        public const int K_PRODUCT_UNIT_BUNCH = 3;

        // Report kubun 
        // Check kubun values
        public const int K_NOT_PRINTED = 0;
        public const int K_PRINTED = 1;

        // Product kubun
        //Refer kubun doc 
        public const int K_COILBAR = 1; //10
        public const int K_BARBAR = 2;
        public const int K_COILCOIL = 3;
        public const int K_STRGT_CUT = 4;
        public const int K_OTHER = 5;

        // Temp alloc state kubun
        // confirm values 
        public const int K_INPROGRESS = 0;
        public const int K_HOLD = 1;
        public const int K_PRODUCTION = 2;



        // Payment kubun 
        public const int K_CASH = 10;
        public const int K_CHEQUE = 15;
        public const int K_TRANSFER = 20;
        public const int K_TRANSFERFEE = 25;
        public const int K_BILL = 30;
        public const int K_HANDPRINT = 35;
        public const int K_OFFSETTRADE = 40;
        public const int K_OFFSETTRANSFERFEE = 41;
        public const int K_OFFSETANDOTHERS = 42;
        public const int K_SHIPPINGCOST = 50;
        public const int K_TAXCONSUMPTION = 60;
        public const int K_CONSUMPTIONTAXINSP = 61;
        public const int K_CONSUMPTIONTAXFEE = 62;
        public const int K_PAYMENT_OTHER = 99;

        // Material(Factory) Shukko kubun 
        public const int K_MATL_SHUKKO_GENERAL = 10;
        public const int K_MATL_SHUKKO_USEASPROD = 20; // check this kubun
        public const int K_MATL_SHUKKO_SUBCONTRACTOR = 30;
        public const int K_MATL_SHUKKO_RETURN = 70;
        public const int K_MATL_SHUKKO_STOCKTAKING = 80;
        public const int K_MATL_SHUKKO_OTHER = 99;

        // Nifuda kubun
        public const short K_PRODUCT = 1;
        public const short K_SPOTKW = 2;
        public const short K_SHUKKA = 5;

        // Futou shukko kubun
        public const short K_FUTOUSHUKKO_DIRECTMAKERSELL = 10;
        public const short K_FUTOUSHUKKO_RETURNEDTOMAKER = 20;
        public const short K_FUTOUSHUKKO_DISPOSED = 30;
        public const short K_FUTOUSHUKKO_DELIVEREDTOOTHERCOMPANIES = 80;
        public const short K_FUTOUSHUKKO_OTHER = 99;


        // Payment type kubun 
        public const int K_ONETIME = 1;
        public const int K_INSTALLMENT = 2;

        // Quantity kubun
        public const short K_QTYkUBUN_MOREORLESS = 1;
        public const short K_QTYkUBUN_ORLESS = 2;
        public const short K_QTYkUBUN_ORMORE = 3;

        // Dim kubun
        public const short K_DIM_OK = 1;
        public const short K_DIM_NG = 2;
        public const short K_DIM_UNTESTED = 3;

        // Length kubun
        public const short K_LENGTH_OK = 1;
        public const short K_LENGTH_NG = 2;
        public const short K_LENGTH_UNTESTED = 3;

        // Combined kubun
        public const short K_COMBINED_OK = 1;
        public const short K_COMBINED_UNTESTED = 2;
        public const short K_COMBINED_HOLD = 3;
        public const short K_COMBINED_ADDEDPROCESS = 4;


        // Color kubun
        public const short K_COLOR_OK = 1;
        public const short K_COLOR_NG = 2;
        public const short K_COLOR_UNTESTED = 3;

        // Chamfering kubun
        public const short K_CHAMFERING_OK = 1;
        public const short K_CHAMFERING_NG = 2;
        public const short K_CHAMFERING_UNTESTED = 3;

        // Spark kubun
        public const short K_SPARK_UNTESTED = 0;
        public const short K_SPARK_OK = 1;
        public const short K_SPARK_NG = 2;


        // Ext Texture kubun
        public const short K_EXT_TEXTURE_OK = 1;
        public const short K_EXT_TEXTURE_NG = 2;
        public const short K_EXT_TEXTURE_UNTESTED = 3;

        // Ext Texture kubun
        public const short K_RADIUS_OK = 1;
        public const short K_RADIUS_NG = 2;
        public const short K_RADIUS_UNTESTED = 3;

        //StockStateKubun
        public const short K_STOCK = 1;
        public const short K_WIP = 2;


        //LocationTypeKubun
        public const short K_HEADQUARTERS = 1;
        public const short K_BRANCHOFFICE = 2;
        public const short K_SALESOFFICE = 3;
        public const short K_FACTORY = 4;

        //Process Defect type Koubun   by Sandeep Hajare
        public const short K_AcidWash = 10;
        public const short K_HeatTreatment = 20;
        public const short K_Refining = 30;
        public const short K_Attaching = 100;
        public const short K_ShotBlast = 110;
        public const short K_Drawing = 120;
        public const short K_DrawingBtoB = 121;
        public const short K_DrawingCtoB = 122;
        public const short K_UnderDrawing = 123;
        public const short K_DrawingCtoC = 124;
        public const short K_Straightening = 150;
        public const short K_MultiStraightening = 151;
        public const short K_PressStraightening = 152;


        //SymbolCurrencyKubun
        public const short K_Lek = 1;
        public const short K_Doll = 3;
        public const short K_ƒ = 4;
        public const short K_pdot = 6;
        public const short K_BZ = 7;
        public const short K_b = 8;
        public const short K_KM = 9;
        public const short K_p = 10;
        public const short K_лв = 11;
        public const short K_R = 12;
        public const short K_Yen = 14;
        public const short K_syn = 15;

        public const short K_kn = 16;
        public const short K_Pline = 17;
        public const short K_Kč = 18;
        public const short K_kr = 19;
        public const short K_RD = 20;
        public const short K_FC = 21;
        public const short K_EC = 22;
        public const short K_CE = 24;
        public const short K_Q = 25;
        public const short K_L = 26;

        public const short K_Ft = 27;
        public const short K_Rs = 28;
        public const short K_Rp = 29;
        public const short K_thirtyone = 31;
        public const short K_J = 32;
        public const short K_W = 33;
        public const short K_k = 34;
        public const short K_Ls = 35;
        public const short K_Lt = 36;
        public const short K_ден = 37;

        public const short K_RM = 38;
        public const short K_Rss = 39;
        public const short K_te = 40;
        public const short K_MT = 41;
        public const short K_C = 42;
        public const short K_N = 43;
        public const short K_B = 44;
        public const short K_Gs = 45;
        public const short K_S = 46;
        public const short K_zł = 47;

        public const short K_lei = 48;
        public const short K_Дин = 50;
        public const short K_S51 = 51;
        public const short K_CHF = 52;
        public const short K_NT = 53;
        public const short K_Bit = 54;
        public const short K_TT = 55;
        public const short K_57 = 57;
        public const short K_U58 = 58;
        public const short K_Bs = 59;
        public const short K_d60 = 60;
        public const short K_z61 = 61;


        //ProcessTemplateKubun
        public const short K_PROCESSTEMPLATE_GENERAL = 10;
        public const short K_PROCESSTEMPLATE_RETURN = 20;
        public const short K_PROCESSTEMPLATE_ADDEDPROCESS = 30;
        public const short K_PROCESSTEMPLATE_CUTTING = 40;

        //PCongiguratioMKubun
        public const short K_STRING = 1;
        public const short K_INTEGER = 2;
        public const short K_DECIMAL = 3;


        //CoilDiaWindingKubun
        public const short K_COIL_DIA_WINDING_LEFT = 1;
        public const short K_COIL_DIA_WINDING_RIGHT = 2;
        public const short K_COIL_DIA_WINDING_OTHER = 3;

        // Color kubun
        public const short K_COLOR_BLUE = 1;
        public const short K_COLOR_RED = 2;
        public const short K_COLOR_ORANGE = 3;
        public const short K_COLOR_YELLOW = 4;
        public const short K_COLOR_YELLOWGREEN = 5;
        public const short K_COLOR_CREAM = 6;
        public const short K_COLOR_GREEN = 7;
        public const short K_COLOR_GRAY = 8;
        public const short K_COLOR_BLACK = 9;
        public const short K_COLOR_GOLD = 10;
        public const short K_COLOR_CON = 11;
        public const short K_COLOR_SILVER = 12;
        public const short K_COLOR_WHITE = 13;
        public const short K_COLOR_SKYBLUE = 14;
        public const short K_COLOR_BROWN = 15;
        public const short K_COLOR_DARKGREY = 16;
        public const short K_COLOR_LIGHTGRAY = 17; // grap is repeat
        public const short K_COLOR_SKINCOLOR = 18;
        public const short K_COLOR_PINK = 19;
        public const short K_COLOR_LIGHTBLUE = 20;
        public const short K_COLOR_LIGHTGREEN = 21;
        public const short K_COLOR_METALLIC = 22;

        // NyukoShukko kubun
        public const short K_NYUKO = 1;
        public const short K_SHUKKASHUKKO = 2;

        //NG Reason Kubun
        public const short K_NG_CUTTER_SCRATCH = 1;
        public const short K_NG_DICE_SCRATCHES = 2;
        public const short K_NG_ROLLER_SCRATCH = 3;
        public const short K_NG_MACHINE_TROUBLE = 4;
        public const short K_NG_BEND = 5;
        public const short K_NG_SCRATCH = 6;
        public const short K_NG_DIM_HIGH = 7;
        public const short K_NG_DIM_LOW = 8;
        public const short K_NG_LENGTH_LONG = 9;
        public const short K_NG_LENGTH_SHORT = 10;

        //CustomerEntryBillingMonthsKubun
        public const short K_THISMONTH = 1;
        public const short K_NEXTMONTH = 2;
        public const short K_AFTER2MONTHS = 3;
        public const short K_AFTER3MONTHS = 4;
        public const short K_AFTER4MONTHS = 5;
        public const short K_AFTER5MONTHS = 6;
        public const short K_AFTER6MONTHS = 7;
        public const short K_AFTER7MONTHS = 8;
        public const short K_AFTER8MONTHS = 9;
        public const short K_AFTER9MONTHS = 10;

        //CustomerEntryAccTypeKubun
        public const short K_CURRENT = 1;
        public const short K_SAVING = 2;

        //CustomerEntryDutyFreeKubun
        public const short K_NONE = 1;

        //CustomerEntryDenpKubun
        public const short K_DOMESTIC = 1;
        public const short K_OVERSEAS = 2;

        //CustomerEntryPaymentOptionKubun
        public const short K_BULK = 1;
        public const short K_PART = 2;

        //SupplierEntryPaymentOptionKubun
        public const short K_SUPPLIERTHISMONTH = 1;
        public const short K_SUPPLIERNEXTMONTH = 2;
        public const short K_SUPPLIERAFTER2MONTHS = 3;
        public const short K_SUPPLIERAFTER3MONTHS = 4;
        public const short K_SUPPLIERAFTER4MONTHS = 5;
        public const short K_SUPPLIERAFTER5MONTHS = 6;
        public const short K_SUPPLIERAFTER6MONTHS = 7;
        public const short K_SUPPLIERAFTER7MONTHS = 8;
        public const short K_SUPPLIERAFTER8MONTHS = 9;
        public const short K_SUPPLIERAFTER9MONTHS = 10;

        //NyukaTypeKubun
        public const short K_SELF = 1;
        public const short K_KUROKAWA = 2;
        public const short K_SPOT = 3;

        // Produt spec kubun
        public const short K_PRODSPEC_CUSTOMERSPECIFIC = 1;
        public const short K_PRODSPEC_GENERAL = 2;

        // Strain Max Min Kubun
        public const short K_STRAIN_MAX = 1;
        public const short K_STRAIN_MIN = 2;

        // Standard Decarb Kubun
        public const short K_DMF = 1;
        public const short K_DMS = 2;
        public const short K_DMT = 3;

        // Standard Hardness Kubun
        public const short K_HRB = 10;
        public const short K_HRC = 20;
        public const short K_HV = 30;
        public const short K_HB = 40;
        public const short K_HS = 50;
        public const short K_HRA = 60;
        public const short K_HRD = 70;
        public const short K_ACTUAL = 99;

        // Hardness Min Max Kubun
        public const short K_HRD_MAX = 1;
        public const short K_HRD_MIN = 2;

        // Product specification Min Max Kubun (Elongation,Yeild,Press,Tensile,Hardness)
        public const short K_PROD_SPEC_MAX = 1;
        public const short K_PROD_SPEC_MIN = 2;

        //ReceivingPlaceKubun
        public const short K_SHIPYARDMGMTCOMPANY = 1;
        public const short K_MAKERNAME = 2;
        public const short K_SUPPLIER = 3;

        //Material Master Kubun
        public const short K_MATERIALCOIL = 1;
        public const short K_MATERIALBAR = 2;
        public const short K_MATERIALKUROKAWA = 3;

        // AariKubun
        public const short K_PRESENT = 1;

        // InStock Kubun
        public const short K_ONLYNOTINSTOCK = 1;
        public const short K_ONLYINSTOCK = 2;

        // Storage Kubun
        public const short K_NYUKA = 3;

        // Split Kubun
        public const short K_SPLIT = 1;

        // Replace Kubun
        public const short K_REPLACE = 1;

        //Process Plan Status kubun
        public const short K_INPROCESS = 0;
        public const short K_COMPLETED = 1;

        // Process Kubun
        public const int K_HIKINUKI = 1;
        public const int K_STRAIGHTENING = 2;
        public const int K_FINALINSPECTION = 3;

        // Payment month Kubun
        public const int K_PAYMENTMONTH_PRESENT = 1;
        public const int K_PAYMENTMONTH_NEXT = 2;
        public const int K_PAYMENTMONTH_AFTER = 3;

        // Department group Kubun
        public const int K_DEPTGROUP_SALES = 1;
        public const int K_DEPTGROUP_SALES_MNGMNT = 2;
        public const int K_DEPTGROUP_MATERIAL = 3;
        public const int K_DEPTGROUP_ACCOUNTING = 4;
        public const int K_DEPTGROUP_SYSTEM = 5;
        public const int K_DEPTGROUP_SOHO = 6;
        public const int K_DEPTGROUP_FACTORY = 7;
        public const int K_DEPTGROUP_FACTORY_OFC = 8;
        public const int K_DEPTGROUP_PLANT_TECH = 9;
        public const int K_DEPTGROUP_QUALITY = 10;
        public const int K_DEPTGROUP_PRODUCTION = 11;
        public const int K_DEPTGROUP_WAREHOUSE = 12;
        public const int K_DEPTGROUP_OTHER = 13;

        // Machine Type  Kubun
        public const int K_MACHINEGROUP_ACIDWASH = 10;
        public const int K_MACHINEGROUP_HEAT_TREATMENT = 20;
        public const int K_MACHINEGROUP_HARDNING_TEMPERING = 30;
        public const int K_MACHINEGROUP_ATTACHING = 40;
        public const int K_MACHINEGROUP_SHOT_BLAST = 50;
        public const int K_MACHINEGROUP_DRAW_BENCH = 60;
        public const int K_MACHINEGROUP_DRAWING = 70;
        public const int K_MACHINEGROUP_STRAIGHT_CUT = 80;
        public const int K_MACHINEGROUP_WIRE_DRAWING = 90;
        public const int K_MACHINEGROUP_STRAIGHTENING = 100;
        public const int K_MACHINEGROUP_CHAMFERING = 110;
        public const int K_MACHINEGROUP_EC = 120;
        public const int K_MACHINEGROUP_UT = 130;
        public const int K_MACHINEGROUP_STAMPPING = 140;
        public const int K_MACHINEGROUP_CUTTING = 150;
        public const int K_MACHINEGROUP_OTHER = 999;


        // Receiving place group Kubun
        public const int K_RECEIVINGPLACE_SHIPYARDCOMAPNY = 1;
        public const int K_RECEIVINGPLACE_MAKER = 2;
        public const int K_RECEIVINGPLACE_TRADINGCOMPANY = 3;

        // Nyuka shukka Kubun
        public const int K_NYUKASHUKKA_SCRAP = 1;
        public const int K_NYUKASHUKKA_SHIPMENT = 2;
        public const int K_NYUKASHUKKA_NYUKOINTAKE = 3;
        public const int K_NYUKASHUKKA_NYUKAARRIVALSTOCK = 4;
        public const int K_NYUKASHUKKA_SCRAPSUPPLIER = 5;
        public const int K_NYUKASHUKKA_SCRAPCUTTING = 6;
        public const int K_NYUKASHUKKA_SCRAPTANAOROSHII = 7;
        public const int K_NYUKASHUKKA_SCRAPOTHER = 8;

        //SearchType Kubun
        public const int K_BYGENERAL = 1;
        public const int K_BYSALES_INCHRAGE = 2;
        public const int K_BYCUSTOMER = 3;

        // Dispatch Master Defect Kubun
        public const int K_DEFECT_CLIENT = 10;
        public const int K_DEFECT_SALESBUSINESSSALES = 20;
        public const int K_DEFECT_SALESBUSINESSCLERK = 30;
        public const int K_DEFECT_WAREHOUSE = 40;
        public const int K_DEFECT_FACTORY = 50;
        public const int K_DEFECT_MATERIALMAKER = 60;
        public const int K_DEFECT_SUPPLIERVENDER = 70;
        public const int K_DEFECT_UNKNOWN = 80;
        public const int K_DEFECT_OTEHR = 99;

        //Shukka Instructio nKubun
        public const int K_UNAVAILABILITY = 20;


        //Shukka Instructio CUTTING_RATE_FORMULA_KUBUN
        public const int K_ORIGINAL_WEIGHT = 10;                //親重量×単価＋個数×工賃　（端材客先納品）
        public const int K_PARENT_WEIGHT = 20;                  //親重量×単価＋個数×工賃　（端材社内廃棄）
        public const int K_UNIT_RATE = 30;                      //個数単価
        public const int K_CUTTING_WEIGHT = 40;                 //切断重量×単価＋個数×工賃
        public const int K_KILOGRAM_RATE = 50;                  //キロ単価　（材料費＋工賃＋歩留り込み）

        // Process Kubun
        public const int K_PROCESS_ACID_WASH = 10;
        public const int K_PROCESS_COATING = 20;
        public const int K_PROCESS_HEAT_TREATMENT = 30;
        public const int K_PROCESS_REFINING = 40;
        public const int K_PROCESS_ATTACHING = 100;
        public const int K_PROCESS_SHOT_BLAST = 110;
        public const int K_PROCESS_DRAWING_B_TO_B = 120;
        public const int K_PROCESS_DRAWING_C_TO_B = 130;
        public const int K_PROCESS_UnderDrawing = 140;
        public const int K_PROCESS_DRAWING_C_TO_C = 145;
        public const int K_PROCESS_MULTI_ROLL_STRIGHENING = 150;
        public const int K_PROCESS_PRESS_STRAIGHTENING = 155;
        public const int K_PROCESS_CHAMFER_S = 160;
        public const int K_PROCESS_EC = 170;
        //public const int K_PROCESS_PROCESS_UT = 180;
        public const int K_PROCESS_STAMPING = 190;
        public const int K_PROCESS_CUTTING = 200;
        public const int K_PROCESS_PROCESS_OIL_SPRAY = 400;
        public const int K_PROCESS_PACKING = 410;
        public const int K_PROCESS_PROCESS_UNIT = 420;
        public const int K_PROCESS_FUREINSPECTION = 800;
        public const int K_PROCESS_HARD_TEMPERING_INSPECTION = 810;
        public const int K_PROCESS_INTERNAL_QUALITY_INSPECTION = 820;
        public const int K_PROCESS_MECHANICAL_INSPECTION_HARDNESS = 830;
        public const int K_PROCESS_HARDNESS_TENSILE_STRENGTH = 835;
        public const int K_PROCESS_COMPLETED_INSPECTION = 840;
        public const int K_PROCESS_UNDER_INSPECTION = 841;
        public const int K_PROCESS_VISUAL_INSPECTION = 842;
        public const int K_PROCESS_SPARK_TEST = 843;
        public const int K_PROCESS_QUALITY_ASSURANCE = 870;
        public const int K_PROCESS_WIP_TRANSFER = 880;
        public const int K_PROCESS_WIP_TRAN_RECIEVER = 890;
        public const int K_PROCESS_HTWipDivision = 900;
        public const int K_PROCESS_HTWipCombine = 901;
        public const int K_PROCESS_OTHER = 998;
        public const int K_PROCESS_NO_NEXT_PROCESS = 999;
        public const int K_PROCESS_CUTTING_OPENING = 201;
        public const int K_PROCESS_CUTTING_TRIMMING = 202;
        public const int K_PROCESS_HEAT_TREATMENT_GENERAL = 35;
        public const int K_PROCESS_EC_ONLY = 171;
        public const int K_PROCESS_NEXTPROCESS_CHANGE = 997;

        // Process condition  Kubun

        public const int K_PROCCONDITIONKUBUN = 0;
        public const int K_PROCCONDITIONKUBUN_1 = 10;
        public const int K_PROCCONDITIONKUBUN_6 = 11;
        public const int K_PROCCONDITIONKUBUN_2 = 20;
        public const int K_PROCCONDITIONKUBUN_3 = 30;
        public const int K_PROCCONDITIONKUBUN_7 = 40;
        public const int K_PROCCONDITIONKUBUN_8 = 50;
        public const int K_PROCCONDITIONKUBUN_4 = 51;
        public const int K_PROCCONDITIONKUBUN_5 = 52;
        public const int K_PROCCONDITIONKUBUN_9 = 60;

        public const int K_HTCONDITIONKUBUN_1 = 10;
        public const int K_HTCONDITIONKUBUN_2 = 20;
        public const int K_HTCONDITIONKUBUN_3 = 30;
        public const int K_HTCONDITIONKUBUN_4 = 40;

        // Stamp   Kubun
        public const int K_STAMP_1 = 1;
        public const int K_STAMP_2 = 2;
        public const int K_STAMP_3 = 3;
        public const int K_STAMP_4 = 4;
        public const int K_STAMP_5 = 5;
        public const int K_STAMP_6 = 6;
        public const int K_STAMP_7 = 7;
        public const int K_STAMP_8 = 8;

        // Cold Drawing   Winding Shape
        public const int C_SHAPE_1 = 1;
        public const int C_SHAPE_2 = 2;
        public const int C_SHAPE_3 = 3;
        public const int C_SHAPE_4 = 4;

        // Cold Drawing   Die Shape
        public const int C_DIESHAPE_1 = 1;
        public const int C_DIESHAPE_2 = 2;
        public const int C_DIESHAPE_3 = 3;
        public const int C_DIESHAPE_4 = 4;
        // Cold Drawing   Magnetic Dust

        public const int C_MDust_1 = 10;
        public const int C_MDust_2 = 15;
        public const int C_MDust_3 = 20;
        public const int C_MDust_4 = 99;

        // Cold Drawing   Dice Insp Start

        public const int C_DiceStart_1 = 1;
        public const int C_DiceStart_2 = 2;
        public const int C_DiceStart_3 = 3;

        // Cold Drawing   Dice Insp End

        public const int C_DiceEnd_1 = 1;
        public const int C_DiceEnd_2 = 2;
        public const int C_DiceEnd_3 = 3;

        public const int HRD_RESULT_OK = 1;
        public const int HRD_RESULT_NG = 2;
        public const int HRD_TEMP_AGAIN = 3;

        // Cold Drawing   Dice Insp End

        public const int K_UP_1 = 1;
        public const int K_Btwn_2 = 2;
        public const int K_Down_3 = 3;

        //Matl Coil/Bar/KW
        public const int M_COIL = 1;
        public const int M_BAR = 2;
        public const int M_KW = 3;

        public const int IO_OUT = 1;
        public const int IO_BTWN = 2;
        public const int IO_IN = 3;

        public const int O_E = 1;
        public const int O_W = 2;
        public const int O_N = 3;
        public const int O_S = 4;

        //Unit wt Kubun
        public const int U_FIXED_WT = 1;
        public const int U_WT_RANGE = 2;
        public const int U_BUNCH_PIECES = 3;

        // Corner Radius kubun
        public const short K_CornerR_OK = 1;
        public const short K_CornerR_NG = 2;

        //Material Reply Forcast
        public const short M_REPLY_OK = 1;
        public const short K_REPLY_NG = 2;
        public const short K_REPLY_HOLD = 3;

        //CSV File Generate 
        public const short CHAR = 0;
        public const short NUM = 1;
        public const short DATE = 2;
        public const short TIME = 3;
        public const short DATETIME = 4;
        public const short DATE_RANGE = 5;
        public const short TIME_RANGE = 6;
        public const short DATETIME_RANGE = 7;
        public const short ON_OFF = 8;

        //maker millsheet config screen
        public const short STEELTYPE = 1;
        public const short CHARGE = 2;
        public const short DIMENSION = 3;
        public const short SHAPE = 4;
        public const short PIECES = 5;
        public const short WEIGHT = 6;
        public const short CCNAME1 = 7;
        public const short CCRANGE1 = 8;
        public const short CCMIN1 = 9;
        public const short CCMAX1 = 10;
        public const short CCACTUAL1 = 11;
        public const short CCNAME2 = 12;
        public const short CCRANGE2 = 13;
        public const short CCMIN2 = 14;
        public const short CCMAX2 = 15;
        public const short CCACTUAL2 = 16;
        public const short CCNAME3 = 17;
        public const short CCRANGE3 = 18;
        public const short CCMIN3 = 19;
        public const short CCMAX3 = 20;
        public const short CCACTUAL3 = 21;
        public const short CCNAME4 = 22;
        public const short CCRANGE4 = 23;
        public const short CCMIN4 = 24;
        public const short CCMAX4 = 25;
        public const short CCACTUAL4 = 26;
        public const short CCNAME5 = 27;
        public const short CCRANGE5 = 28;
        public const short CCMIN5 = 29;
        public const short CCMAX5 = 30;
        public const short CCACTUAL5 = 31;
        public const short CCNAME6 = 32;
        public const short CCRANGE6 = 33;
        public const short CCMIN6 = 34;
        public const short CCMAX6 = 35;
        public const short CCACTUAL6 = 36;
        public const short CCNAME7 = 37;
        public const short CCRANGE7 = 38;
        public const short CCMIN7 = 39;
        public const short CCMAX7 = 40;
        public const short CCACTUAL7 = 41;
        public const short CCNAME8 = 42;
        public const short CCRANGE8 = 43;
        public const short CCMIN8 = 44;
        public const short CCMAX8 = 45;
        public const short CCACTUAL8 = 46;
        public const short CCNAME9 = 47;
        public const short CCRANGE9 = 48;
        public const short CCMIN9 = 49;
        public const short CCMAX9 = 50;
        public const short CCACTUAL9 = 51;
        public const short CCNAME10 = 52;
        public const short CCRANGE10 = 53;
        public const short CCMIN10 = 54;
        public const short CCMAX10 = 55;
        public const short CCACTUAL10 = 56;
        public const short CCNAME11 = 57;
        public const short CCRANGE11 = 58;
        public const short CCMIN11 = 59;
        public const short CCMAX11 = 60;
        public const short CCACTUAL11 = 61;
        public const short CCNAME12 = 62;
        public const short CCRANGE12 = 63;
        public const short CCMIN12 = 64;
        public const short CCMAX12 = 65;
        public const short CCACTUAL12 = 66;
        public const short CCNAME13 = 67;
        public const short CCRANGE13 = 68;
        public const short CCMIN13 = 69;
        public const short CCMAX13 = 70;
        public const short CCACTUAL13 = 71;
        public const short CCNAME14 = 72;
        public const short CCRANGE14 = 73;
        public const short CCMIN14 = 74;
        public const short CCMAX14 = 75;
        public const short CCACTUAL14 = 76;
        public const short CCNAME15 = 77;
        public const short CCRANGE15 = 78;
        public const short CCMIN15 = 79;
        public const short CCMAX15 = 80;
        public const short CCACTUAL15 = 81;
        public const short CCNAME16 = 82;
        public const short CCRANGE16 = 83;
        public const short CCMIN16 = 84;
        public const short CCMAX16 = 85;
        public const short CCACTUAL16 = 86;
        public const short CCNAME17 = 87;
        public const short CCRANGE17 = 88;
        public const short CCMIN17 = 89;
        public const short CCMAX17 = 90;
        public const short CCACTUAL17 = 91;
        public const short CCNAME18 = 92;
        public const short CCRANGE18 = 93;
        public const short CCMIN18 = 94;
        public const short CCMAX18 = 95;
        public const short CCACTUAL18 = 96;
        public const short CCNAME19 = 97;
        public const short CCRANGE19 = 98;
        public const short CCMIN19 = 99;
        public const short CCMAX19 = 100;
        public const short CCACTUAL19 = 101;
        public const short CCNAME20 = 102;
        public const short CCRANGE20 = 103;
        public const short CCMIN20 = 104;
        public const short CCMAX20 = 105;
        public const short CCACTUAL20 = 106;
        public const short CCNAME21 = 107;
        public const short CCRANGE21 = 108;
        public const short CCMIN21 = 109;
        public const short CCMAX21 = 110;
        public const short CCACTUAL21 = 111;
        public const short CCNAME22 = 112;
        public const short CCRANGE22 = 113;
        public const short CCMIN22 = 114;
        public const short CCMAX22 = 115;
        public const short CCACTUAL22 = 116;
        public const short CCNAME23 = 117;
        public const short CCRANGE23 = 118;
        public const short CCMIN23 = 119;
        public const short CCMAX23 = 120;
        public const short CCACTUAL23 = 121;
        public const short CCNAME24 = 122;
        public const short CCRANGE24 = 123;
        public const short CCMIN24 = 124;
        public const short CCMAX24 = 125;
        public const short CCACTUAL24 = 126;
        public const short CCNAME25 = 127;
        public const short CCRANGE25 = 128;
        public const short CCMIN25 = 129;
        public const short CCMAX25 = 130;
        public const short CCACTUAL25 = 131;
        public const short HARD_STD1 = 132;
        public const short HARD_MIN1 = 133;
        public const short HARD_MAX1 = 134;
        public const short HARD_ACTUAL1 = 135;
        public const short HARD_STD2 = 136;
        public const short HARD_MIN2 = 137;
        public const short HARD_MAX2 = 138;
        public const short HARD_ACTUAL2 = 139;
        public const short HARD_STD3 = 140;
        public const short HARD_MIN3 = 141;
        public const short HARD_MAX3 = 142;
        public const short HARD_ACTUAL3 = 143;
        public const short HARD_STD4 = 144;
        public const short HARD_MIN4 = 145;
        public const short HARD_MAX4 = 146;
        public const short HARD_ACTUAL4 = 147;
        public const short HARD_STD5 = 148;
        public const short HARD_MIN5 = 149;
        public const short HARD_MAX5 = 150;
        public const short HARD_ACTUAL5 = 151;
        public const short HARD_STD6 = 152;
        public const short HARD_MIN6 = 153;
        public const short HARD_MAX6 = 154;
        public const short HARD_ACTUAL6 = 155;
        public const short HARD_STD7 = 156;
        public const short HARD_MIN7 = 157;
        public const short HARD_MAX7 = 158;
        public const short HARD_ACTUAL7 = 159;
        public const short HARD_STD8 = 160;
        public const short HARD_MIN8 = 161;
        public const short HARD_MAX8 = 162;
        public const short HARD_ACTUAL8 = 163;
        public const short HARD_STD9 = 164;
        public const short HARD_MIN9 = 165;
        public const short HARD_MAX9 = 166;
        public const short HARD_ACTUAL9 = 167;
        public const short HARD_STD10 = 168;
        public const short HARD_MIN10 = 169;
        public const short HARD_MAX10 = 170;
        public const short HARD_ACTUAL10 = 171;
        public const short HARD_STD11 = 172;
        public const short HARD_MIN11 = 173;
        public const short HARD_MAX11 = 174;
        public const short HARD_ACTUAL11 = 175;
        public const short HARD_STD12 = 176;
        public const short HARD_MIN12 = 177;
        public const short HARD_MAX12 = 178;
        public const short HARD_ACTUAL12 = 179;
        public const short HARD_STD13 = 180;
        public const short HARD_MIN13 = 181;
        public const short HARD_MAX13 = 182;
        public const short HARD_ACTUAL13 = 183;
        public const short HARD_STD14 = 184;
        public const short HARD_MIN14 = 185;
        public const short HARD_MAX14 = 186;
        public const short HARD_ACTUAL14 = 187;
        public const short HARD_STD15 = 188;
        public const short HARD_MIN15 = 189;
        public const short HARD_MAX15 = 190;
        public const short HARD_ACTUAL15 = 191;
        public const short HARD_STD16 = 192;
        public const short HARD_MIN16 = 193;
        public const short HARD_MAX16 = 194;
        public const short HARD_ACTUAL16 = 195;
        public const short HARD_STD17 = 196;
        public const short HARD_MIN17 = 197;
        public const short HARD_MAX17 = 198;
        public const short HARD_ACTUAL17 = 199;
        public const short HARD_STD18 = 200;
        public const short HARD_MIN18 = 201;
        public const short HARD_MAX18 = 202;
        public const short HARD_ACTUAL18 = 203;
        public const short HARD_STD19 = 204;
        public const short HARD_MIN19 = 205;
        public const short HARD_MAX19 = 206;
        public const short HARD_ACTUAL19 = 207;
        public const short HARD_STD20 = 208;
        public const short HARD_MIN20 = 209;
        public const short HARD_MAX20 = 210;
        public const short HARD_ACTUAL20 = 211;
        public const short HARD_STD21 = 212;
        public const short HARD_MIN21 = 213;
        public const short HARD_MAX21 = 214;
        public const short HARD_ACTUAL21 = 215;
        public const short HARD_STD22 = 216;
        public const short HARD_MIN22 = 217;
        public const short HARD_MAX22 = 218;
        public const short HARD_ACTUAL22 = 219;
        public const short HARD_STD23 = 220;
        public const short HARD_MIN23 = 221;
        public const short HARD_MAX23 = 222;
        public const short HARD_ACTUAL23 = 223;
        public const short HARD_STD24 = 224;
        public const short HARD_MIN24 = 225;
        public const short HARD_MAX24 = 226;
        public const short HARD_ACTUAL24 = 227;
        public const short HARD_STD25 = 228;
        public const short HARD_MIN25 = 229;
        public const short HARD_MAX25 = 230;
        public const short HARD_ACTUAL25 = 231;

        //EC for hardness and tensile decision check
        public const int EC_FI_RESULT_OK = 10;
        public const int EC_FI_RESULT_NG = 20;
        public const int EC_FI_RESULT_HOLD = 30;
        public const int EC_FI_RESULT_NA = 0;
        public static Dictionary<int, string> ProductUnitKubunGet()
        {
            Dictionary<int, string> ProductUnitKubun = new Dictionary<int, string>()
        {
             {K_UNIT_BAR,Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_Hone")) },
             {K_UNIT_BUNDLE,Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_Thaba_L")) },
             {K_UNIT_PIECES,Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_Pieces_L")) },
             {K_UNIT_COIL,Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_UnitType4Of4_L")) },
        };
            return ProductUnitKubun;
        }
        public static Dictionary<int, string> ProductTypeKubunGet()
        {
            Dictionary<int, string> ProductTypeKubun = new Dictionary<int, string>()
        {
             {K_COILBAR, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_ProdKubun1of5_L")) },
             {K_BARBAR, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_ProdKubun2of5_L")) },
             {K_COILCOIL, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_ProdKubun3of5_L")) },
             {K_STRGT_CUT, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_ProdKubun4of5_L")) },
             {K_OTHER, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_Other")) },
        };
            return ProductTypeKubun;
        }
        public static Dictionary<int, string> MetlTypeKubunGet()
        {
            Dictionary<int, string> matlTypeKubun = new Dictionary<int, string>()
        {
             {K_UNIT_BAR,Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_Hone1")) },
             {K_UNIT_COIL,Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_UnitType4Of4_L")) },
        };
            return matlTypeKubun;
        }

        public static Dictionary<int, string> MetlTypeKubunFullNameGet()
        {
            Dictionary<int, string> matlTypeKubun = new Dictionary<int, string>()
        {
             {M_COIL,Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_Coil")) },
             {M_BAR,Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_Bar")) },
        };
            return matlTypeKubun;
        }

        public static Dictionary<int, string> MagariKubunGet()
        {
            Dictionary<int, string> MagariKubun = new Dictionary<int, string>()
            {
                {K_BEND_HEXAGON, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_Bend")) },
                {K_BEND_ROUND, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_Bend_Round")) },
                {K_BEND_HEXAGON_HOST, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_EdgeDistortion")) }

            };
            return MagariKubun;

        }

        public static Dictionary<int, string> ColorKubunGet()
        {
            Dictionary<int, string> ColorKubun = new Dictionary<int, string>()
            {
                {K_COLOR_BLUE, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_Color_Blue")) },
                {K_COLOR_RED, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_Color_Red")) },
                {K_COLOR_ORANGE, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_Color_Orange")) },
                {K_COLOR_YELLOW, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_Color_Yellow")) },
                {K_COLOR_YELLOWGREEN, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_Color_YellowGreen")) },
                {K_COLOR_CREAM, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_Color_Cream")) },
                {K_COLOR_GREEN, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_Color_Green")) },
                {K_COLOR_GRAY, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_Color_Gray")) },
                {K_COLOR_BLACK, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_Color_Black")) },
                {K_COLOR_GOLD, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_Color_Gold")) },
                {K_COLOR_CON, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_Color_Con")) },
                {K_COLOR_SILVER, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_Color_Silver")) },
                {K_COLOR_WHITE, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_Color_White")) },
                {K_COLOR_SKYBLUE, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_Color_SkyBlue")) },
                {K_COLOR_BROWN, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_Color_Brown")) },
                {K_COLOR_DARKGREY, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_Color_DarkGrey")) },
                {K_COLOR_LIGHTGRAY, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_Color_LightGrey")) },
                {K_COLOR_SKINCOLOR, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_Color_SkinColor")) },
                {K_COLOR_PINK, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_Color_Pink")) }, // wrong
                {K_COLOR_LIGHTBLUE, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_Color_LightBlue")) },
                {K_COLOR_LIGHTGREEN, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_Color_LightGreen")) },
                {K_COLOR_METALLIC, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_Color_Metallic")) }
            };
            return ColorKubun;
        }

        public static Dictionary<int, string> CoilDiaWindingKubunGet()
        {
            Dictionary<int, string> CoilDiaWindingKubun = new Dictionary<int, string>()
            {
                {K_COIL_DIA_WINDING_LEFT, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_CoilWindDirections1of3")) },
                {K_COIL_DIA_WINDING_RIGHT, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_CoilWindDirections2of3")) },
                {K_COIL_DIA_WINDING_OTHER, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_Other")) },
            };
            return CoilDiaWindingKubun;
        }

        public static Dictionary<int, string> ECDoneCheckKubunGet()
        {
            Dictionary<int, string> CoilDiaWindingKubun = new Dictionary<int, string>()
            {
                {K_PRESENT, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_Present")) },
            };

            return CoilDiaWindingKubun;
        }


        public static Dictionary<int, string> NextProcessKubunGet()
        {
            Dictionary<int, string> NextProcessKubun = new Dictionary<int, string>()
        {

             {K_PROCESS_ACID_WASH, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_ProcessAcidWash")) },
             {K_PROCESS_COATING, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_Coating")) },
             {K_PROCESS_HEAT_TREATMENT, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_ProcessHeatTreatment")) },
             {K_PROCESS_REFINING, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_Refining")) },
             {K_PROCESS_ATTACHING, Convert.ToString(_localizationServices.GetLocalizedHtmlString("MachineTypeGroup5")) },
             {K_PROCESS_SHOT_BLAST, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_ProcessShotBlast")) },
             {K_PROCESS_DRAWING_B_TO_B, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_ProcessDrawingBtoB")) },
             {K_PROCESS_DRAWING_C_TO_B, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_ProcessDrawingCtoB")) },
             {K_PROCESS_UnderDrawing, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_UnderDrawing")) },
             {K_PROCESS_DRAWING_C_TO_C, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_ProcessDrawingCtoC")) },
             {K_PROCESS_MULTI_ROLL_STRIGHENING, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_ProcessMultiROLLStrighening")) },
             {K_PROCESS_PRESS_STRAIGHTENING, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_ProcessPressStraightening")) },
             {K_PROCESS_CHAMFER_S, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_ProcessChamferS")) },
             {K_PROCESS_EC, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_ProcessEC")) },
             //{K_PROCESS_PROCESS_UT, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_ProcessUT")) },//UT machine added in EC by client 30 Sep 2025
             {K_PROCESS_STAMPING, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_ProcessStamping")) },
             {K_PROCESS_CUTTING, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_ProcessCutting")) },
             {K_PROCESS_PROCESS_OIL_SPRAY, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_ProcessOil_Spray")) },
             {K_PROCESS_PACKING, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_Packing")) },
             {K_PROCESS_PROCESS_UNIT, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_ProcessUnit")) },
             {K_PROCESS_FUREINSPECTION, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_ProcessDistortion_Inspection")) },
             {K_PROCESS_HARD_TEMPERING_INSPECTION, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_ProcessTemperature_Inspection")) },
             {K_PROCESS_INTERNAL_QUALITY_INSPECTION, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_ProcessInternal_Quality_Inspection")) },
             {K_PROCESS_MECHANICAL_INSPECTION_HARDNESS, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_ProcessMechanical_Inspection_Hardness")) },
             {K_PROCESS_HARDNESS_TENSILE_STRENGTH, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_ProcessMechanical_Insp_Hardness_Tensile")) },
             {K_PROCESS_COMPLETED_INSPECTION, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_ProcessCompletedInspection")) },
             {K_PROCESS_UNDER_INSPECTION, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_ProcessUnderInspection")) },
             {K_PROCESS_VISUAL_INSPECTION, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_ProcessVisualInspection")) },
             {K_PROCESS_SPARK_TEST, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_ProcessSpark_Inspection")) },
             {K_PROCESS_QUALITY_ASSURANCE, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_ProcessQA")) },
             {K_PROCESS_WIP_TRANSFER, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_ProcessWIP_Forwarding")) },
             {K_PROCESS_WIP_TRAN_RECIEVER, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_ProcessWIP_Accepting_Transfer")) },
             {K_PROCESS_HTWipDivision, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_ProcessHTWipDivision")) },
             {K_PROCESS_HTWipCombine, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_ProcessHTWipCombine")) },
             {K_PROCESS_OTHER, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_ProcessOther_Process")) },
             {K_PROCESS_NO_NEXT_PROCESS, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_ProcessNoNextProcess")) },
             {K_PROCESS_HEAT_TREATMENT_GENERAL, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_ProcessHeatTreatmentGeneral")) },
             {K_PROCESS_CUTTING_OPENING, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_CuttingOpening")) },
             {K_PROCESS_CUTTING_TRIMMING, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_CuttingTrimming")) },
             {K_PROCESS_EC_ONLY, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_EC")) },
             {K_PROCESS_NEXTPROCESS_CHANGE, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_NextProcessChange")) }


        };
            return NextProcessKubun;
        }
        public static Dictionary<int, string> GetDieShape()
        {
            Dictionary<int, string> GetgWindingShape = new Dictionary<int, string>()
        {
             {C_DIESHAPE_1, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_R")) },
             {C_DIESHAPE_2, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_DieDimKubun2Of4")) },
             {C_DIESHAPE_3, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_DieDimKubun3Of4")) },
             {C_DIESHAPE_4, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_DieDimKubun4Of4")) }
        };
            return GetgWindingShape;
        }
        public static Dictionary<int, string> GetColdDrawingWindingShape()
        {
            Dictionary<int, string> GetgWindingShape = new Dictionary<int, string>()
        {
             {C_SHAPE_1, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_RightCoil")) },
             {C_SHAPE_2, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_LeftCoil")) },
             {C_SHAPE_3, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_RightCarrier")) },
             {C_SHAPE_4, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_LeftCarrier")) }
        };
            return GetgWindingShape;
        }

        public static Dictionary<int, string> GetColdDrawingMDust()
        {
            Dictionary<int, string> GetWindingShape = new Dictionary<int, string>()
        {
             {C_MDust_1, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_OK1")) },
             {C_MDust_2, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_OK2")) },
             {C_MDust_3, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_NotGood")) },
             {C_MDust_4, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_Other")) }
        };
            return GetWindingShape;
        }
        public static Dictionary<int, string> GetDiceInspStartData()
        {
            Dictionary<int, string> GetDiceInspStart = new Dictionary<int, string>()
        {
             {C_DiceStart_1, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_NewMeasure")) },
             {C_DiceStart_2, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_Good")) },
             {C_DiceStart_3, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_Wrap")) }

        };
            return GetDiceInspStart;
        }
        public static Dictionary<int, string> GetDiceInspEndData()
        {
            Dictionary<int, string> GetDiceInspEnd = new Dictionary<int, string>()
        {

             {C_DiceEnd_1, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_Good")) },
             {C_DiceEnd_2, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_Wrap")) },
             {C_DiceEnd_3, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_ProdShukkoKubun7_Scrap")) }

        };
            return GetDiceInspEnd;
        }
        public static Dictionary<int, string> StampKubunGet()
        {
            Dictionary<int, string> StampKubun = new Dictionary<int, string>()
        {
             {K_STAMP_1, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_StampValue_MBE")) },
             {K_STAMP_2, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_StampValue_MBD")) },
             {K_STAMP_3, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_StampValue_BK6")) },
             {K_STAMP_4, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_StampValue_NBC")) },
             {K_STAMP_5, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_StampValue_BW6U")) },
             {K_STAMP_6, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_StampValue_BB6")) },
             {K_STAMP_7, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_StampValue_NMY")) },
             {K_STAMP_8, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_StampValue_NDQ")) },
        };
            return StampKubun;
        }

        public static Dictionary<int, string> StandardHardnessKubunGet()
        {
            Dictionary<int, string> StandardHardnessKubun = new Dictionary<int, string>()
        {
             {K_HRB, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_StandardHardnessKubun1of7")) },
             {K_HRC, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_StandardHardnessKubun2of7")) },
             {K_HB, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_StandardHardnessKubun3of7")) },
             {K_HV, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_StandardHardnessKubun4of7")) },
             {K_HS, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_StandardHardnessKubun5of7")) },
             {K_HRA, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_StandardHardnessKubun6of7")) },
             {K_HRD, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_StandardHardnessKubun7of7")) },
             {K_ACTUAL, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_ActualPattern")) }
        };
            return StandardHardnessKubun;
        }

        public static Dictionary<int, string> HardnessMaxMinKubunGet()
        {
            Dictionary<int, string> StandardHardnessKubun = new Dictionary<int, string>()
        {
             {K_HRD_MAX, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2")) },
             {K_HRD_MIN, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2")) },

        };
            return StandardHardnessKubun;
        }

        public static Dictionary<int, string> HardnessMaxMinKubun()
        {
            Dictionary<int, string> StandardHardnessKubun = new Dictionary<int, string>()
        {
             {K_HRD_MAX, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1")) },
             {K_HRD_MIN, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2")) },

        };
            return StandardHardnessKubun;
        }

        public static Dictionary<int, string> ResultKubunGet()
        {
            Dictionary<int, string> ResultKubun = new Dictionary<int, string>()
        {
             {K_RESULT_OK, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_OK")) },
             {K_RESULT_NG, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_NG")) },
             {K_RESULT_HOLD, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_Hold")) },
             {K_RESULT_PROC_ADDITION, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_DecisionKubun4of4")) },
             //{K_RESULT_REHEATTREATMENT, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_ReHeatTreatment")) },
             //{K_RESULT_TEMPAGAIN, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_Temp_Again")) },
             //{K_RESULT_REDRAWING, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_ReDrawing")) },
             //{K_RESULT_RESTRAIGHTENING, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_Restraightening")) },
             //{K_RESULT_RECHAMFERING, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_Re_Chamfering")) },
             //{K_RESULT_RE_EC, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_ReEc")) },
             {K_RESULT_SPARKUNTESTED, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lb_SparkUntested")) },
             {K_RESULT_PRODRECIEPT, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_DieDimKubun5Of5")) },
             {K_RESULT_UNTESTED, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_UnTested"))}

        };
            return ResultKubun;
        }

        public static Dictionary<int, string> TrialResultKubunGet()
        {
            Dictionary<int, string> TrialResultKubun = new Dictionary<int, string>()
            {
             {K_RESULT_OK, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_OK")) },
             {K_RESULT_NG, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_NG")) },
            };
            return TrialResultKubun;
        }
        public static Dictionary<int, string> HrdResultKubunGet()
        {
            Dictionary<int, string> HrdResultKubun = new Dictionary<int, string>()
            {
             {HRD_RESULT_OK, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_OK")) },
             {HRD_RESULT_NG, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_NG")) },
             {HRD_TEMP_AGAIN, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_Temp_Again")) },
            };
            return HrdResultKubun;
        }
        public static Dictionary<int, string> UnitTypeKubunGet()
        {
            Dictionary<int, string> TrialResultKubun = new Dictionary<int, string>()
            {
             {K_UNIT_BAR,Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_Hone")) },
             {K_UNIT_BUNDLE,Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_Thaba_L")) },
            };
            return TrialResultKubun;
        }
        public static Dictionary<int, string> UnitChemicalSpeciKubunGet()
        {
            Dictionary<int, string> ChemicalSpeciResultKubun = new Dictionary<int, string>()
            {
             {K_Chem_C,Convert.ToString(_localizationServices.GetLocalizedHtmlString("lblChemC")) },
             {K_Chem_Sl,Convert.ToString(_localizationServices.GetLocalizedHtmlString("lblChemSl")) },
              {K_Chem_Mn,Convert.ToString(_localizationServices.GetLocalizedHtmlString("lblChemMn")) },
             {K_Chem_P,Convert.ToString(_localizationServices.GetLocalizedHtmlString("lblChemP")) },
              {K_Chem_S,Convert.ToString(_localizationServices.GetLocalizedHtmlString("lblChemS")) },
             {K_Chem_Cu,Convert.ToString(_localizationServices.GetLocalizedHtmlString("lblChemCu")) },
              {K_Chem_Nl,Convert.ToString(_localizationServices.GetLocalizedHtmlString("lblChemNl")) },
             {K_Chem_Cr,Convert.ToString(_localizationServices.GetLocalizedHtmlString("lblChemCr")) },
              {K_Chem_Mo,Convert.ToString(_localizationServices.GetLocalizedHtmlString("lblChemMo")) },
             {K_Chem_NlCr,Convert.ToString(_localizationServices.GetLocalizedHtmlString("lblChemNlCr")) },



            };
            return ChemicalSpeciResultKubun;
        }
        public static Dictionary<int, string> GeneralSpecialKubunGet()
        {
            Dictionary<int, string> TrialResultKubun = new Dictionary<int, string>()
            {
             {K_GENSPEC_GENERAL,Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_GeneralSpecialKubun1of2")) },
             {K_GENSPEC_SPECIAL,Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_GeneralSpecialKubun2of2")) },
            };
            return TrialResultKubun;
        }
        public static Dictionary<int, string> MaterialTypeListGet()
        {
            Dictionary<int, string> ResultMaterialType = new Dictionary<int, string>()
            {
             {K_MATERIAL_TYPE_COIL,Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_Coil")) },
             {K_MATERIAL_TYPE_BAR,Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_Bar")) },
             {K_MATERIAL_TYPE_KUROKAWA,Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_Kurokawa")) },
            };
            return ResultMaterialType;

        }
        public static Dictionary<int, string> UnitTypeKubunGetForGeneralProcess()
        {
            Dictionary<int, string> TrialResultKubun = new Dictionary<int, string>()
            {
             {K_UNIT_COIL,Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_Coil_L")) },
             {K_UNIT_BAR,Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_Hone")) },
             {K_UNIT_BUNDLE,Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_Thaba_L")) },
            };
            return TrialResultKubun;
        }
        public static Dictionary<int, string> UnitTypeKubunMillsheet()
        {
            Dictionary<int, string> TrialResultKubun = new Dictionary<int, string>()
            {
             {K_UNIT_BAR,Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_Hone")) },
             {K_UNIT_BUNDLE,Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_Thaba_L")) },
             {K_UNIT_COIL,Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_Coil_L")) }
            };
            return TrialResultKubun;
        }
        public static Dictionary<int, string> NGReasonKubunGet()
        {
            Dictionary<int, string> NGReasonKubun = new Dictionary<int, string>()
        {
             { K_NG_CUTTER_SCRATCH,Convert.ToString(GetGlobalResourceString("lbl_ExternalSurfaceScratch")) },
             { K_NG_DICE_SCRATCHES , Convert.ToString(GetGlobalResourceString("lbl_DiesNGReason"))},
             { K_NG_ROLLER_SCRATCH , Convert.ToString(GetGlobalResourceString("lbl_RollerNGReason"))},
             { K_NG_MACHINE_TROUBLE, Convert.ToString(GetGlobalResourceString("lbl_NGReasonKubun4of10"))},
             { K_NG_BEND, Convert.ToString(GetGlobalResourceString("lbl_NGReasonKubun5of10"))},
             { K_NG_SCRATCH, Convert.ToString(GetGlobalResourceString("lbl_NGReasonScratch"))},
             { K_NG_DIM_HIGH ,Convert.ToString(GetGlobalResourceString("lbl_NGReasonKubun7of10"))},
             { K_NG_DIM_LOW, Convert.ToString(GetGlobalResourceString("lbl_NGReasonKubun8of10"))},
             { K_NG_LENGTH_LONG, Convert.ToString(GetGlobalResourceString("lbl_NGReasonKubun9of10"))},
             { K_NG_LENGTH_SHORT, Convert.ToString(GetGlobalResourceString("lbl_NGReasonKubun10of10"))},

        };
            return NGReasonKubun;
        }

        public static Dictionary<int, string> BendStateKubunGet()
        {
            Dictionary<int, string> BendStateKubun = new Dictionary<int, string>()
        {
             {K_BEND_STATUS_OK, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_OKStatus")) },
             {K_BEND_STATUS_OK2, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_BendStatusKubun2of7")) },
             {K_BEND_STATUS_OK3, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_BendStatusKubun3of7")) },
             {K_BEND_STATUS_HOLD, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_Hold")) },
             {K_BEND_STATUS_STRAIGHTENING, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_Straightening")) },
             {K_BEND_STATUS_MULTIROLL, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_BendStatusKubun6of7")) },
             {K_BEND_STATUS_OTHER, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_Other")) },
        };
            return BendStateKubun;
        }

        public static Dictionary<int, string> ExteriorConditionKubunGet()
        {
            Dictionary<int, string> ExteriorConditionKubun = new Dictionary<int, string>()
        {
             {K_EXT_COND_OK, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_OKStatus")) },
             {K_EXT_COND_HOLD, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_Hold")) },
             {K_EXT_COND_MATERIAL_SCRATCH, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun2of11"))},
             {K_EXT_COND_HELIX_CORRECTION, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun3of11")) },
             {K_EXT_COND_OUT_OF_DIM, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun11of11")) },
             {K_EXT_COND_SURFACE_CORRECTION, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun4of11")) },
             {K_EXT_COND_RUSTED, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun6of11")) },
             {K_EXT_COND_DICE_SCRATCH, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun7of11")) },
             {K_EXT_COND_JAW_SCRATCH, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun1of11")) },
             {K_EXT_COND_CUTTER_SCRATCH, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun9of11")) },
             {K_EXT_COND_ROLLER_SCRATCH, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun10of11")) },
             {K_EXT_COND_OTHER, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_Other")) },
             //{K_EXT_COND_DIMENSSION_CORRECTION, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun5of10")) },
             
             
             
             
             
             
        };
            return ExteriorConditionKubun;
        }

        public static Dictionary<int, string> MaterialUnitKubunGet()
        {
            Dictionary<int, string> MaterialUnitKubun = new Dictionary<int, string>()
        {
             {K_UNIT_BAR,Convert.ToString(GetGlobalResourceString("lbl_Hone")) },
             {K_UNIT_BUNDLE,Convert.ToString(GetGlobalResourceString("lbl_Thaba_L")) },
             {K_UNIT_PIECES,Convert.ToString(GetGlobalResourceString("lbl_Pieces_L")) },
             {K_UNIT_COIL,Convert.ToString(GetGlobalResourceString("lbl_Coil_L")) },
        };
            return MaterialUnitKubun;
        }

        public static Dictionary<int, string> TPTestKubunGet()
        {
            Dictionary<int, string> TPTestKubun = new Dictionary<int, string>()
        {
             {K_TPTEST_NONE, Convert.ToString(GetGlobalResourceString("lbl_TPTestKubun1of3")) },
             {K_TPTEST_150, Convert.ToString(GetGlobalResourceString("lbl_TPTestKubun2of3")) },
             {K_TPTEST_450, Convert.ToString(GetGlobalResourceString("lbl_TPTestKubun3of3")) },
        };
            return TPTestKubun;
        }

        public static Dictionary<int, string> GetTPTestKubun()
        {
            Dictionary<int, string> TPTestKubun = new Dictionary<int, string>()
        {
             {K_TPTEST_OK, Convert.ToString(GetGlobalResourceString("lbl_OK")) },
             {K_TPTEST_NA, Convert.ToString(GetGlobalResourceString("lbl_NA")) },
             
        };
            return TPTestKubun;
        }
        public static Dictionary<int, string> HardnessOilTypeKubunGet()
        {
            Dictionary<int, string> TPTestKubun = new Dictionary<int, string>()
        {
             {H_TEMP_WATER, Convert.ToString(GetGlobalResourceString("lbl_Temp_Kubun_Water")) },
             {H_TEMP_LOW, Convert.ToString(GetGlobalResourceString("lbl_Temp_Kubun_LowUQE")) },
             {H_TEMP_MIDDLE, Convert.ToString(GetGlobalResourceString("lbl_Temp_Kubun_MiddleUQE")) },
             {H_TEMP_HIGHT, Convert.ToString(GetGlobalResourceString("lbl_Temp_Kubun_HighUQE")) },

        };
            return TPTestKubun;
        }

        public static Dictionary<int, string> GetHardnessTextureTypeKubun()
        {
            Dictionary<int, string> TPTestKubun = new Dictionary<int, string>()
        {

             { H_TEXTURE_M, Convert.ToString(GetGlobalResourceString("lbl_Texture_M")) },
             { H_TEXTURE_M_B, Convert.ToString(GetGlobalResourceString("lbl_Texture_M_b")) },
             { H_TEXTURE_M_FE, Convert.ToString(GetGlobalResourceString("lbl_Texture_M_fe")) },
             { H_TEXTURE_M_B_FE, Convert.ToString(GetGlobalResourceString("lbl_Texture_M_b_fe")) },
             { H_TEXTURE_M_B_FE1, Convert.ToString(GetGlobalResourceString("lbl_Texture_m_B_fe1")) },
             { H_TEXTURE_UNCONDITIONAL, Convert.ToString(GetGlobalResourceString("lbl_Texture_Unconditional")) },

        };
            return TPTestKubun;
        }

        public static Dictionary<int, string> OkNgDimDecisionKubunGet()
        {
            Dictionary<int, string> OkNgDimDecisionKubun = new Dictionary<int, string>()
            {
             {K_DIM_OK , Convert.ToString(GetGlobalResourceString("lbl_OK")) },
             {K_DIM_NG, Convert.ToString(GetGlobalResourceString("lbl_NG")) },
             {K_DIM_UNTESTED, Convert.ToString(GetGlobalResourceString("lbl_UnTested")) },
        };
            return OkNgDimDecisionKubun;
        }

        public static Dictionary<int, string> OkNgLengthDecisionKubunGet()
        {
            Dictionary<int, string> OkNgLengthDecisionKubun = new Dictionary<int, string>()
            {
             {K_DIM_OK , Convert.ToString(GetGlobalResourceString("lbl_OK")) },
             {K_DIM_NG, Convert.ToString(GetGlobalResourceString("lbl_NG")) },
             {K_LENGTH_UNTESTED, Convert.ToString(GetGlobalResourceString("lbl_UnTested")) },
        };
            return OkNgLengthDecisionKubun;
        }

        public static Dictionary<int, string> OkNgChamferingDecisionKubunGet()
        {
            Dictionary<int, string> OkNgChamferingDecisionKubun = new Dictionary<int, string>()
            {
             {K_CHAMFERING_OK , Convert.ToString(GetGlobalResourceString("lbl_OK")) },
             {K_CHAMFERING_NG, Convert.ToString(GetGlobalResourceString("lbl_NG")) },
             {K_CHAMFERING_UNTESTED, Convert.ToString(GetGlobalResourceString("lbl_UnTested")) },
        };
            return OkNgChamferingDecisionKubun;
        }

        public static Dictionary<int, string> OkNgColorDecisionKubunGet()
        {
            Dictionary<int, string> OkNgColorDecisionKubun = new Dictionary<int, string>()
            {
             {K_COLOR_OK , Convert.ToString(GetGlobalResourceString("lbl_OK")) },
             {K_COLOR_NG, Convert.ToString(GetGlobalResourceString("lbl_NG")) },
             {K_COLOR_UNTESTED, Convert.ToString(GetGlobalResourceString("lbl_UnTested")) },
        };
            return OkNgColorDecisionKubun;
        }

        public static Dictionary<int, string> OkNgExtTextureDecisionKubunGet()
        {
            Dictionary<int, string> OkNgExtTextureDecisionKubun = new Dictionary<int, string>()
            {
             {K_EXT_TEXTURE_OK , Convert.ToString(GetGlobalResourceString("lbl_OK")) },
             {K_EXT_TEXTURE_NG, Convert.ToString(GetGlobalResourceString("lbl_NG")) },
             {K_EXT_TEXTURE_UNTESTED, Convert.ToString(GetGlobalResourceString("lbl_UnTested")) },
        };
            return OkNgExtTextureDecisionKubun;
        }

        public static Dictionary<int, string> OkNgCombinedDecisionKubunGet()
        {
            Dictionary<int, string> OkNgCombinedDecisionKubun = new Dictionary<int, string>()
        {
             {K_COMBINED_OK  , Convert.ToString(GetGlobalResourceString("lbl_OK")) },
             {K_COMBINED_UNTESTED, Convert.ToString(GetGlobalResourceString("lbl_UnTested")) },
             {K_COMBINED_HOLD, Convert.ToString(GetGlobalResourceString("lbl_Hold")) },
             {K_COMBINED_ADDEDPROCESS, Convert.ToString(GetGlobalResourceString("lblProcessKubunAddedProcess")) },
        };
            return OkNgCombinedDecisionKubun;
        }
        public class MasterName
        {
            public static string MST_CUSTOMER = _localizationServices.GetLocalizedHtmlString("lbl_Customer");
            public static string MST_SUPPLIER = _localizationServices.GetLocalizedHtmlString("lbl_Supplier_L");
            public static string MST_DEPARTMENT_GROUP = _localizationServices.GetLocalizedHtmlString("lbl_DepartmentGroupMaster");
            public static string MST_MOCHIKOMISAKI = _localizationServices.GetLocalizedHtmlString("lbl_MochikomisakiMaster");
            public static string MST_MUKESAKI = _localizationServices.GetLocalizedHtmlString("lbl_MukesakiShort");
            public static string MST_PACKING = _localizationServices.GetLocalizedHtmlString("lbl_Packing");

            public static string MST_EMPLOYEE = _localizationServices.GetLocalizedHtmlString("lbl_EmployeeMaster");
            public static string MST_LOCATION = _localizationServices.GetLocalizedHtmlString("lbl_LocationMaster");
            public static string MST_DEPARTMENT = _localizationServices.GetLocalizedHtmlString("lbl_DepartmentMaster");

            public static string MST_RECEIVER = _localizationServices.GetLocalizedHtmlString("lbl_ReceiverMaster");
            public static string MST_RECEIVING_PLACE = _localizationServices.GetLocalizedHtmlString("lbl_ReceivingPlaceMaster");
            public static string MST_CUSTOMER_TYPE = _localizationServices.GetLocalizedHtmlString("lbl_CustomerTypeMaster");
            public static string MST_PROCESS = _localizationServices.GetLocalizedHtmlString("lbl_ProcessMaster");
            public static string MST_SHIPYARD_WAREHOUSE = _localizationServices.GetLocalizedHtmlString("lbl_ShipyardAndWarehouseMaster");
            public static string MST_INTERNAL_NAME = _localizationServices.GetLocalizedHtmlString("lbl_InternalNameMaster");
            public static string MST_BANK = _localizationServices.GetLocalizedHtmlString("lbl_BankMaster");
            public static string MST_CONDITION = _localizationServices.GetLocalizedHtmlString("lbl_ConditionMaster");
            public static string MST_STEEL_TYPE = _localizationServices.GetLocalizedHtmlString("lbl_SteelTypeMaster");
            public static string MST_MACHINE = _localizationServices.GetLocalizedHtmlString("lbl_MachineMaster");
            public static string MST_MACHINE_GROUP = _localizationServices.GetLocalizedHtmlString("lbl_MachineGroupMaster");
            public static string MST_MAKER = _localizationServices.GetLocalizedHtmlString("lbl_MakerMaster");
            public static string MST_MATL_NAME_MAKER = _localizationServices.GetLocalizedHtmlString("lbl_MatlNameMakerMaster");
            public static string MST_HIKITORISAKI = _localizationServices.GetLocalizedHtmlString("lbl_HikitorisakiMaster");
            public static string MST_PRODUCT_TYPE = _localizationServices.GetLocalizedHtmlString("lbl_HikitorisakiMaster");
            public static string MST_PROCESS_DEFECT = _localizationServices.GetLocalizedHtmlString("lbl_ProcessDefectMaster");
            public static string MST_CHEMICAL_COMPOSITION = _localizationServices.GetLocalizedHtmlString("lbl_ChemicalCompositionMaster");
            public static string MST_JIS_TEST = _localizationServices.GetLocalizedHtmlString("lbl_JISTestMaster");
        }
        public class ActiveInactiveLabel
        {
            //Define active/inactive label screen wise
            public static string MST_CUSTOMER_ACTIVE = _localizationServices.GetLocalizedHtmlString("lbl_Reactivate");
            public static string MST_CUSTOMER_INACTIVE = _localizationServices.GetLocalizedHtmlString("lblInActive");
            public static string MST_SUPPLIER_ACTIVE = _localizationServices.GetLocalizedHtmlString("lbl_Reactivate");
            public static string MST_SUPPLIER_INACTIVE = _localizationServices.GetLocalizedHtmlString("lblInActive");
            public static string MST_DEPTGROUP_ACTIVE = _localizationServices.GetLocalizedHtmlString("lbl_Activate");
            public static string MST_DEPTGROUP_INACTIVE = _localizationServices.GetLocalizedHtmlString("lbl_DeActivate");
            public static string MST_MOCHIKOMISAKI_ACTIVE = _localizationServices.GetLocalizedHtmlString("lbl_Reactivate");
            public static string MST_MOCHIKOMISAKI_INACTIVE = _localizationServices.GetLocalizedHtmlString("lblInActive");
            public static string MST_MUKESAKI_ACTIVE = _localizationServices.GetLocalizedHtmlString("lbl_Reactivate");
            public static string MST_MUKESAKI_INACTIVE = _localizationServices.GetLocalizedHtmlString("lblInActive");
            public static string MST_PACKING_ACTIVE = _localizationServices.GetLocalizedHtmlString("lbl_Reuse");
            public static string MST_PACKING_INACTIVE = _localizationServices.GetLocalizedHtmlString("lbl_Discard");
            public static string MST_EMPLOYEE_ACTIVE = _localizationServices.GetLocalizedHtmlString("lbl_ActiveEmployee");
            public static string MST_EMPLOYEE_INACTIVE = _localizationServices.GetLocalizedHtmlString("lbl_InActiveEmployee");
            public static string MST_LOCATION_ACTIVE = _localizationServices.GetLocalizedHtmlString("lbl_Resume1");
            public static string MST_LOCATION_INACTIVE = _localizationServices.GetLocalizedHtmlString("lbl_Eliminate");
            public static string MST_DEPARTMENT_ACTIVE = _localizationServices.GetLocalizedHtmlString("lbl_Resume1");
            public static string MST_DEPARTMENT_INACTIVE = _localizationServices.GetLocalizedHtmlString("lbl_Eliminate");
            public static string MST_RECEIVER_ACTIVE = _localizationServices.GetLocalizedHtmlString("lbl_Reactivate");
            public static string MST_RECEIVER_INACTIVE = _localizationServices.GetLocalizedHtmlString("lblInActive");
            public static string MST_RECEIVING_PLACE_ACTIVE = _localizationServices.GetLocalizedHtmlString("lbl_Reactivate");
            public static string MST_RECEIVING_PLACE_INACTIVE = _localizationServices.GetLocalizedHtmlString("lblInActive");
            public static string MST_CUSTOMER_TYPE_ACTIVE = _localizationServices.GetLocalizedHtmlString("lbl_Reactivate2");
            public static string MST_CUSTOMER_TYPE_INACTIVE = _localizationServices.GetLocalizedHtmlString("lblInActive2");
            public static string MST_PROCESS_ACTIVE = _localizationServices.GetLocalizedHtmlString("lbl_ProcessActive");
            public static string MST_PROCESS_INACTIVE = _localizationServices.GetLocalizedHtmlString("lbl_ProcessInactive");
            public static string MST_SHIPYARD_WAREHOUSE_ACTIVE = _localizationServices.GetLocalizedHtmlString("lbl_Reactivate");
            public static string MST_SHIPYARD_WAREHOUSE_INACTIVE = _localizationServices.GetLocalizedHtmlString("lblInActive");
            public static string MST_INTERNAL_NAME_ACTIVE = _localizationServices.GetLocalizedHtmlString("lbl_Reactivate2");
            public static string MST_INTERNAL_NAME_INACTIVE = _localizationServices.GetLocalizedHtmlString("lbl_Deactivate2");
            public static string MST_BANK_ACTIVE = _localizationServices.GetLocalizedHtmlString("lbl_Reactivate2");
            public static string MST_BANK_INACTIVE = _localizationServices.GetLocalizedHtmlString("lblInActive2");
            public static string MST_MATERIL_ACTIVE = _localizationServices.GetLocalizedHtmlString("lbl_Reactivate2");
            public static string MST_MATERIL_INACTIVE = _localizationServices.GetLocalizedHtmlString("lbl_Deactivate2");
            public static string MST_CONDITION_ACTIVE = _localizationServices.GetLocalizedHtmlString("lbl_Reactivate2");
            public static string MST_CONDITION_INACTIVE = _localizationServices.GetLocalizedHtmlString("lbl_Deactivate2");
            public static string MST_STEEL_TYPE_ACTIVE = _localizationServices.GetLocalizedHtmlString("lbl_Reactivate2");
            public static string MST_STEEL_TYPE_INACTIVE = _localizationServices.GetLocalizedHtmlString("lbl_Deactivate2");
            public static string MST_MACHINE_ACTIVE = _localizationServices.GetLocalizedHtmlString("lbl_ReInstall");
            public static string MST_MACHINE_INACTIVE = _localizationServices.GetLocalizedHtmlString("lbl_Remove");
            public static string MST_MACHINE_GROUP_ACTIVE = _localizationServices.GetLocalizedHtmlString("lbl_Resume1");
            public static string MST_MACHINE_GROUP_INACTIVE = _localizationServices.GetLocalizedHtmlString("lbl_Eliminate");
            public static string MST_PRODUCT_TYPE_ACTIVE = _localizationServices.GetLocalizedHtmlString("lbl_Reactivate2");
            public static string MST_PRODUCT_TYPE_INACTIVE = _localizationServices.GetLocalizedHtmlString("lbl_Deactivate2");
            public static string MST_PRODUCT_ACTIVE = _localizationServices.GetLocalizedHtmlString("lbl_Reactivate2");
            public static string MST_PRODUCT_INACTIVE = _localizationServices.GetLocalizedHtmlString("lbl_Deactivate2");
            public static string MST_MAKER_ACTIVE = _localizationServices.GetLocalizedHtmlString("lbl_Reactivate");
            public static string MST_MAKER_INACTIVE = _localizationServices.GetLocalizedHtmlString("lblInActive");
            public static string MST_MATL_NAME_MAKER_ACTIVE = _localizationServices.GetLocalizedHtmlString("lbl_Reactivate2");
            public static string MST_MATL_NAME_MAKER_INACTIVE = _localizationServices.GetLocalizedHtmlString("lbl_Deactivate2");

            public static string MST_HIKITORISAKI_ACTIVE = _localizationServices.GetLocalizedHtmlString("lbl_Reactivate");
            public static string MST_HIKITORISAKI_INACTIVE = _localizationServices.GetLocalizedHtmlString("lblInActive");
            public static string MST_PROCESS_DEFECT_ACTIVE = _localizationServices.GetLocalizedHtmlString("lbl_Resume1");
            public static string MST_PROCESS_DEFECT_INACTIVE = _localizationServices.GetLocalizedHtmlString("lbl_Eliminate");
            public static string MST_JIS_TEST_ACTIVE = _localizationServices.GetLocalizedHtmlString("lbl_InUse");
            public static string MST_JIS_TEST_INACTIVE = _localizationServices.GetLocalizedHtmlString("lbl_Eliminate");
            public static string MST_CHEMICAL_COMPOSITION_ACTIVE = _localizationServices.GetLocalizedHtmlString("lbl_Resume1");
            public static string MST_CHEMICAL_COMPOSITION_INACTIVE = _localizationServices.GetLocalizedHtmlString("lbl_Eliminate");
        }

        public static Dictionary<int, string> ProcConditionKubunGet()
        {
            Dictionary<int, string> ProcConditionKubun = new Dictionary<int, string>()
        {
             {K_PROCCONDITIONKUBUN, Convert.ToString(GetGlobalResourceString("lbl_NO1")) },
             {K_PROCCONDITIONKUBUN_1, Convert.ToString(GetGlobalResourceString("lbl_ProcConditionKubun_1")) },
             {K_PROCCONDITIONKUBUN_6, Convert.ToString(GetGlobalResourceString("lbl_ProcConditionKubun_5")) },
             {K_PROCCONDITIONKUBUN_2, Convert.ToString(GetGlobalResourceString("lbl_ProcConditionKubun_6")) },
             {K_PROCCONDITIONKUBUN_3, Convert.ToString(GetGlobalResourceString("lbl_ProcConditionKubun_2")) },
             {K_PROCCONDITIONKUBUN_7, Convert.ToString(GetGlobalResourceString("lbl_ProcConditionKubun_7")) },
             {K_PROCCONDITIONKUBUN_8, Convert.ToString(GetGlobalResourceString("lbl_ProcConditionKubun_8")) },
             {K_PROCCONDITIONKUBUN_4, Convert.ToString(GetGlobalResourceString("lbl_ProcConditionKubun_3")) },
             {K_PROCCONDITIONKUBUN_5, Convert.ToString(GetGlobalResourceString("lbl_ProcConditionKubun_4")) },
             {K_PROCCONDITIONKUBUN_9, Convert.ToString(GetGlobalResourceString("lbl_ProcConditionKubun_9")) },

        };
            return ProcConditionKubun;
        }

        public static Dictionary<int, string> MaterialKubunGet()
        {
            Dictionary<int, string> MaterialKubun = new Dictionary<int, string>()
        {
             {K_SELF_RECEIVED, Convert.ToString(GetGlobalResourceString("lbl_ProdMatlKubun1of5")) },
             {K_RECEIVED_WITH_PAYMENT, Convert.ToString(GetGlobalResourceString("lbl_ProdMatlKubun2of5")) },
             {K_RECEIVED_FREE, Convert.ToString(GetGlobalResourceString("lbl_ProdMatlKubun3of5")) },
        };
            return MaterialKubun;
        }
        public static string GetGlobalResourceString(string key)
        {
            return Convert.ToString(_localizationServices.GetLocalizedHtmlString(key));
        }
        public static Dictionary<int, string> OkNgSparkDecisionKubunGet()
        {
            Dictionary<int, string> OkNgSparkTextureDecisionKubun = new Dictionary<int, string>()
            {
             {K_SPARK_OK , Convert.ToString(GetGlobalResourceString("lbl_OK")) },
             {K_SPARK_NG, Convert.ToString(GetGlobalResourceString("lbl_NG")) },
             {K_SPARK_UNTESTED, Convert.ToString(GetGlobalResourceString("lbl_UnTested")) },
        };
            return OkNgSparkTextureDecisionKubun;
        }
        public static Dictionary<int, string> DecarbKubunGet()
        {
            Dictionary<int, string> DecarbKubun = new Dictionary<int, string>()
            {
             { K_DMF, Convert.ToString(GetGlobalResourceString("lbl_DecarbDMF")) },
             { K_DMS, Convert.ToString(GetGlobalResourceString("lbl_DecarbDMS")) },
             { K_DMT, Convert.ToString(GetGlobalResourceString("lbl_DecarbDMT")) },
        };
            return DecarbKubun;
        }
        public static Dictionary<int, string> StorageManagementKubun()
        {
            Dictionary<int, string> ProductUnitKubun = new Dictionary<int, string>()
        {
             {K_FACTORY_STORAGE,Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_StorageManagementKubun1of5")) },
             {K_BRANCH_STORAGE,Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_StorageManagementKubun2of5")) },
             {K_QUICK_DELIVERY,Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_QuickDispatch")) },
             {K_STRG_MGMT_TYPE_ORDERED,Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_PlannedStock")) },
             {K_NO_INSTRUCTION,Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_StorageManagementKubun5of5")) },
        };
            return ProductUnitKubun;
        }
        public static Dictionary<int, string> SpecialManagementKubun()
        {
            Dictionary<int, string> ProductUnitKubun = new Dictionary<int, string>()
        {
             {K_NEW_GOODS,Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_SpecialManagementKubun1of3")) },
             {K_PROTOTYPE_GOODS,Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_SpecialManagementKubun2of3")) },
             {K_SPECIALMGMT_GOODS,Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_SpecialManagementKubun3of3")) },

        };
            return ProductUnitKubun;
        }
        public static Dictionary<int, string> ProductManagementKubun()
        {
            Dictionary<int, string> ProductUnitKubun = new Dictionary<int, string>()
        {
             {K_FIRST_GOOD,Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_ProductionManagementKubun1of2")) },
             {K_REPLACE_GOOD,Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_ProductionManagementKubun2of2")) },


        };
            return ProductUnitKubun;
        }
        public static Dictionary<int, string> StackPositionKubun()
        {
            Dictionary<int, string> ProductUnitKubun = new Dictionary<int, string>()
        {
             { K_UP_1, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_StackPositionKubun1of3")) },
             {K_Btwn_2,Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_StackPositionKubun2of3")) },
             {K_Down_3,Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_StackPositionKubun3of3")) },
        };
            return ProductUnitKubun;
        }

        public static Dictionary<int, string> MatlTypeCBK()
        {
            Dictionary<int, string> MaterialUnitKubun = new Dictionary<int, string>()
        {
             {M_COIL,Convert.ToString(GetGlobalResourceString("lbl_Coil")) },
             {M_BAR,Convert.ToString(GetGlobalResourceString("lbl_Bar")) },
             {M_KW,Convert.ToString(GetGlobalResourceString("lbl_Kurokawa")) },

        };
            return MaterialUnitKubun;
        }

        public static Dictionary<int, string> InOutKubun()
        {
            Dictionary<int, string> InOutKubun = new Dictionary<int, string>()
        {
             {IO_OUT,Convert.ToString(GetGlobalResourceString("lbl_Out")) },
             {IO_BTWN,Convert.ToString(GetGlobalResourceString("lbl_Btwn")) },
             {IO_IN,Convert.ToString(GetGlobalResourceString("lbl_In")) },
        };
            return InOutKubun;
        }

        public static Dictionary<int, string> OrientationKubun()
        {
            Dictionary<int, string> OrientationKubun = new Dictionary<int, string>()
        {
             {O_E,Convert.ToString(GetGlobalResourceString("lbl_E")) },
             {O_W,Convert.ToString(GetGlobalResourceString("lbl_W")) },
             {O_N,Convert.ToString(GetGlobalResourceString("lbl_N")) },
             {O_S,Convert.ToString(GetGlobalResourceString("lbl_S")) },
        };
            return OrientationKubun;
        }
        public static Dictionary<int, string> ProcessPlanKubun()
        {
            Dictionary<int, string> ProductUnitKubun = new Dictionary<int, string>()
        {
            {K_GENERAL,Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_General")) },
            {K_REPROCESS,Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_Reprocess")) },
            {K_ADDEDPROCESS,Convert.ToString(_localizationServices.GetLocalizedHtmlString("lblProcessKubunAddedProcess")) },
            {K_CUTTING,Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_Cutting")) },


        };
            return ProductUnitKubun;
        }

        public static Dictionary<int, string> HardnessDecisionGet()
        {
            Dictionary<int, string> HrdResultKubun = new Dictionary<int, string>()
            {
             {HRD_RESULT_OK, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_OK")) },
             {HRD_RESULT_NG, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_NG")) },
             {HRD_TEMP_AGAIN, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_Temp_Again")) },
            };
            return HrdResultKubun;
        }

        public static Dictionary<int, string> UnitWtKubunGet()
        {
            Dictionary<int, string> HrdResultKubun = new Dictionary<int, string>()
            {
             {U_FIXED_WT, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_UnitWt")) },
             {U_WT_RANGE, Convert.ToString(_localizationServices.GetLocalizedHtmlString("rdb_UnitWtRange_L")) },
             {U_BUNCH_PIECES, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_Bunch")) },
            };
            return HrdResultKubun;
        }

        public static Dictionary<int, string> CornerRadiuscisionKubunGet()
        {
            Dictionary<int, string> CornerRDecisionKubun = new Dictionary<int, string>()
            {
             {K_CornerR_OK , Convert.ToString(GetGlobalResourceString("lbl_OK")) },
             {K_CornerR_NG, Convert.ToString(GetGlobalResourceString("lbl_NG")) },

        };
            return CornerRDecisionKubun;
        }
        public static Dictionary<int, string> FIKubunGet()
        {
            Dictionary<int, string> CornerRDecisionKubun = new Dictionary<int, string>()
            {
             {K_CornerR_OK , Convert.ToString(GetGlobalResourceString("lbl_OK")) },
             {K_CornerR_NG, Convert.ToString(GetGlobalResourceString("lbl_NG")) },

        };
            return CornerRDecisionKubun;
        }

        public static Dictionary<int, string> GetMaterialReplyForcast()
        {
            Dictionary<int, string> MaterialReply = new Dictionary<int, string>()
            {
             {M_REPLY_OK , Convert.ToString(GetGlobalResourceString("lbl_OK")) },
             {K_REPLY_NG, Convert.ToString(GetGlobalResourceString("lbl_NG")) },
             {K_REPLY_HOLD, Convert.ToString(GetGlobalResourceString("lbl_Hold")) },

        };
            return MaterialReply;
        }
        public static Dictionary<int, List<string>> GetCSVSearchConditions()
        {

            Dictionary<int, List<string>> _dictionary = new Dictionary<int, List<string>>();

            AddToDictionary(_dictionary, CHAR, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_CharEqualTo")));
            AddToDictionary(_dictionary, CHAR, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_CharContains")));
            AddToDictionary(_dictionary, CHAR, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_CharStartWith")));

            AddToDictionary(_dictionary, NUM, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_SignEqualTo")));
            AddToDictionary(_dictionary, NUM, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_SignNotEqualTo")));
            AddToDictionary(_dictionary, NUM, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_SignLessThan")));
            AddToDictionary(_dictionary, NUM, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_SignLessThanEqualTo")));
            AddToDictionary(_dictionary, NUM, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_SignGreaterThan")));
            AddToDictionary(_dictionary, NUM, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_SignGreaterThanEqualTo")));

            AddToDictionary(_dictionary, DATE, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_SignEqualTo")));
            AddToDictionary(_dictionary, DATE, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_SignNotEqualTo")));
            AddToDictionary(_dictionary, DATE, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_SignLessThan")));
            AddToDictionary(_dictionary, DATE, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_SignLessThanEqualTo")));
            AddToDictionary(_dictionary, DATE, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_SignGreaterThan")));
            AddToDictionary(_dictionary, DATE, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_SignGreaterThanEqualTo")));

            AddToDictionary(_dictionary, TIME, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_SignEqualTo")));
            AddToDictionary(_dictionary, TIME, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_SignNotEqualTo")));
            AddToDictionary(_dictionary, TIME, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_SignLessThan")));
            AddToDictionary(_dictionary, TIME, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_SignLessThanEqualTo")));
            AddToDictionary(_dictionary, TIME, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_SignGreaterThan")));
            AddToDictionary(_dictionary, TIME, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_SignGreaterThanEqualTo")));

            AddToDictionary(_dictionary, DATETIME, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_SignEqualTo")));
            AddToDictionary(_dictionary, DATETIME, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_SignNotEqualTo")));
            AddToDictionary(_dictionary, DATETIME, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_SignLessThan")));
            AddToDictionary(_dictionary, DATETIME, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_SignLessThanEqualTo")));
            AddToDictionary(_dictionary, DATETIME, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_SignGreaterThan")));
            AddToDictionary(_dictionary, DATETIME, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_SignGreaterThanEqualTo")));

            return _dictionary;
        }
        static void AddToDictionary(Dictionary<int, List<string>> dict, int key, string value)
        {
            if (!dict.TryGetValue(key, out var list))
            {
                list = new List<string>();
                dict[key] = list;
            }
            list.Add(value);
        }
        public static Dictionary<int, string> GetMillsheetPageLables()
        {
            Dictionary<int, string> _dictionary = new Dictionary<int, string>()
            {
             {STEELTYPE,Convert.ToString(GetGlobalResourceString("lbl_TypeOfSteel")) },
             {CHARGE,Convert.ToString(GetGlobalResourceString("lbl_Charge")) },
             {DIMENSION,Convert.ToString(GetGlobalResourceString("lbl_Dimensions")) },
             {SHAPE,Convert.ToString(GetGlobalResourceString("lbl_Shape")) },
             {PIECES,Convert.ToString(GetGlobalResourceString("lbl_NoOfUnits")) },
             {WEIGHT,Convert.ToString(GetGlobalResourceString("lbl_ThabaWeight_L")) },
             {CCNAME1,Convert.ToString(GetGlobalResourceString("lbl_CCNAME1")) },
             {CCRANGE1,Convert.ToString(GetGlobalResourceString("lbl_CCRANGE1")) },
             {CCMIN1,Convert.ToString(GetGlobalResourceString("lbl_CCMIN1")) },
             {CCMAX1,Convert.ToString(GetGlobalResourceString("lbl_CCMAX1")) },
             {CCACTUAL1,Convert.ToString(GetGlobalResourceString("lbl_CCACTUAL1")) },
             {CCNAME2,Convert.ToString(GetGlobalResourceString("lbl_CCNAME2")) },
             {CCRANGE2,Convert.ToString(GetGlobalResourceString("lbl_CCRANGE2")) },
             {CCMIN2,Convert.ToString(GetGlobalResourceString("lbl_CCMIN2")) },
             {CCMAX2,Convert.ToString(GetGlobalResourceString("lbl_CCMAX2")) },
             {CCACTUAL2,Convert.ToString(GetGlobalResourceString("lbl_CCACTUAL2")) },
             {CCNAME3,Convert.ToString(GetGlobalResourceString("lbl_CCNAME3")) },
             {CCRANGE3,Convert.ToString(GetGlobalResourceString("lbl_CCRANGE3")) },
             {CCMIN3,Convert.ToString(GetGlobalResourceString("lbl_CCMIN3")) },
             {CCMAX3,Convert.ToString(GetGlobalResourceString("lbl_CCMAX3")) },
             {CCACTUAL3,Convert.ToString(GetGlobalResourceString("lbl_CCACTUAL3")) },
             {CCNAME4,Convert.ToString(GetGlobalResourceString("lbl_CCNAME4")) },
             {CCRANGE4,Convert.ToString(GetGlobalResourceString("lbl_CCRANGE4")) },
             {CCMIN4,Convert.ToString(GetGlobalResourceString("lbl_CCMIN4")) },
             {CCMAX4,Convert.ToString(GetGlobalResourceString("lbl_CCMAX4")) },
             {CCACTUAL4,Convert.ToString(GetGlobalResourceString("lbl_CCACTUAL4")) },
             {CCNAME5,Convert.ToString(GetGlobalResourceString("lbl_CCNAME5")) },
             {CCRANGE5,Convert.ToString(GetGlobalResourceString("lbl_CCRANGE5")) },
             {CCMIN5,Convert.ToString(GetGlobalResourceString("lbl_CCMIN5")) },
             {CCMAX5,Convert.ToString(GetGlobalResourceString("lbl_CCMAX5")) },
             {CCACTUAL5,Convert.ToString(GetGlobalResourceString("lbl_CCACTUAL5")) },
             {CCNAME6,Convert.ToString(GetGlobalResourceString("lbl_CCNAME6")) },
             {CCRANGE6,Convert.ToString(GetGlobalResourceString("lbl_CCRANGE6")) },
             {CCMIN6,Convert.ToString(GetGlobalResourceString("lbl_CCMIN6")) },
             {CCMAX6,Convert.ToString(GetGlobalResourceString("lbl_CCMAX6")) },
             {CCACTUAL6,Convert.ToString(GetGlobalResourceString("lbl_CCACTUAL6")) },
             {CCNAME7,Convert.ToString(GetGlobalResourceString("lbl_CCNAME7")) },
             {CCRANGE7,Convert.ToString(GetGlobalResourceString("lbl_CCRANGE7")) },
             {CCMIN7,Convert.ToString(GetGlobalResourceString("lbl_CCMIN7")) },
             {CCMAX7,Convert.ToString(GetGlobalResourceString("lbl_CCMAX7")) },
             {CCACTUAL7,Convert.ToString(GetGlobalResourceString("lbl_CCACTUAL7")) },
             {CCNAME8,Convert.ToString(GetGlobalResourceString("lbl_CCNAME8")) },
             {CCRANGE8,Convert.ToString(GetGlobalResourceString("lbl_CCRANGE8")) },
             {CCMIN8,Convert.ToString(GetGlobalResourceString("lbl_CCMIN8")) },
             {CCMAX8,Convert.ToString(GetGlobalResourceString("lbl_CCMAX8")) },
             {CCACTUAL8,Convert.ToString(GetGlobalResourceString("lbl_CCACTUAL8")) },
             {CCNAME9,Convert.ToString(GetGlobalResourceString("lbl_CCNAME9")) },
             {CCRANGE9,Convert.ToString(GetGlobalResourceString("lbl_CCRANGE9")) },
             {CCMIN9,Convert.ToString(GetGlobalResourceString("lbl_CCMIN9")) },
             {CCMAX9,Convert.ToString(GetGlobalResourceString("lbl_CCMAX9")) },
             {CCACTUAL9,Convert.ToString(GetGlobalResourceString("lbl_CCACTUAL9")) },
             {CCNAME10,Convert.ToString(GetGlobalResourceString("lbl_CCNAME10")) },
             {CCRANGE10,Convert.ToString(GetGlobalResourceString("lbl_CCRANGE10")) },
             {CCMIN10,Convert.ToString(GetGlobalResourceString("lbl_CCMIN10")) },
             {CCMAX10,Convert.ToString(GetGlobalResourceString("lbl_CCMAX10")) },
             {CCACTUAL10,Convert.ToString(GetGlobalResourceString("lbl_CCACTUAL10")) },
             {CCNAME11,Convert.ToString(GetGlobalResourceString("lbl_CCNAME11")) },
             {CCRANGE11,Convert.ToString(GetGlobalResourceString("lbl_CCRANGE11")) },
             {CCMIN11,Convert.ToString(GetGlobalResourceString("lbl_CCMIN11")) },
             {CCMAX11,Convert.ToString(GetGlobalResourceString("lbl_CCMAX11")) },
             {CCACTUAL11,Convert.ToString(GetGlobalResourceString("lbl_CCACTUAL11")) },
             {CCNAME12,Convert.ToString(GetGlobalResourceString("lbl_CCNAME12")) },
             {CCRANGE12,Convert.ToString(GetGlobalResourceString("lbl_CCRANGE12")) },
             {CCMIN12,Convert.ToString(GetGlobalResourceString("lbl_CCMIN12")) },
             {CCMAX12,Convert.ToString(GetGlobalResourceString("lbl_CCMAX12")) },
             {CCACTUAL12,Convert.ToString(GetGlobalResourceString("lbl_CCACTUAL12")) },
             {CCNAME13,Convert.ToString(GetGlobalResourceString("lbl_CCNAME13")) },
             {CCRANGE13,Convert.ToString(GetGlobalResourceString("lbl_CCRANGE13")) },
             {CCMIN13,Convert.ToString(GetGlobalResourceString("lbl_CCMIN13")) },
             {CCMAX13,Convert.ToString(GetGlobalResourceString("lbl_CCMAX13")) },
             {CCACTUAL13,Convert.ToString(GetGlobalResourceString("lbl_CCACTUAL13")) },
             {CCNAME14,Convert.ToString(GetGlobalResourceString("lbl_CCNAME14")) },
             {CCRANGE14,Convert.ToString(GetGlobalResourceString("lbl_CCRANGE14")) },
             {CCMIN14,Convert.ToString(GetGlobalResourceString("lbl_CCMIN14")) },
             {CCMAX14,Convert.ToString(GetGlobalResourceString("lbl_CCMAX14")) },
             {CCACTUAL14,Convert.ToString(GetGlobalResourceString("lbl_CCACTUAL14")) },
             {CCNAME15,Convert.ToString(GetGlobalResourceString("lbl_CCNAME15")) },
             {CCRANGE15,Convert.ToString(GetGlobalResourceString("lbl_CCRANGE15")) },
             {CCMIN15,Convert.ToString(GetGlobalResourceString("lbl_CCMIN15")) },
             {CCMAX15,Convert.ToString(GetGlobalResourceString("lbl_CCMAX15")) },
             {CCACTUAL15,Convert.ToString(GetGlobalResourceString("lbl_CCACTUAL15")) },
             {CCNAME16,Convert.ToString(GetGlobalResourceString("lbl_CCNAME16")) },
             {CCRANGE16,Convert.ToString(GetGlobalResourceString("lbl_CCRANGE16")) },
             {CCMIN16,Convert.ToString(GetGlobalResourceString("lbl_CCMIN16")) },
             {CCMAX16,Convert.ToString(GetGlobalResourceString("lbl_CCMAX16")) },
             {CCACTUAL16,Convert.ToString(GetGlobalResourceString("lbl_CCACTUAL16")) },
             {CCNAME17,Convert.ToString(GetGlobalResourceString("lbl_CCNAME17")) },
             {CCRANGE17,Convert.ToString(GetGlobalResourceString("lbl_CCRANGE17")) },
             {CCMIN17,Convert.ToString(GetGlobalResourceString("lbl_CCMIN17")) },
             {CCMAX17,Convert.ToString(GetGlobalResourceString("lbl_CCMAX17")) },
             {CCACTUAL17,Convert.ToString(GetGlobalResourceString("lbl_CCACTUAL17")) },
             {CCNAME18,Convert.ToString(GetGlobalResourceString("lbl_CCNAME18")) },
             {CCRANGE18,Convert.ToString(GetGlobalResourceString("lbl_CCRANGE18")) },
             {CCMIN18,Convert.ToString(GetGlobalResourceString("lbl_CCMIN18")) },
             {CCMAX18,Convert.ToString(GetGlobalResourceString("lbl_CCMAX18")) },
             {CCACTUAL18,Convert.ToString(GetGlobalResourceString("lbl_CCACTUAL18")) },
             {CCNAME19,Convert.ToString(GetGlobalResourceString("lbl_CCNAME19")) },
             {CCRANGE19,Convert.ToString(GetGlobalResourceString("lbl_CCRANGE19")) },
             {CCMIN19,Convert.ToString(GetGlobalResourceString("lbl_CCMIN19")) },
             {CCMAX19,Convert.ToString(GetGlobalResourceString("lbl_CCMAX19")) },
             {CCACTUAL19,Convert.ToString(GetGlobalResourceString("lbl_CCACTUAL19")) },
             {CCNAME20,Convert.ToString(GetGlobalResourceString("lbl_CCNAME20")) },
             {CCRANGE20,Convert.ToString(GetGlobalResourceString("lbl_CCRANGE20")) },
             {CCMIN20,Convert.ToString(GetGlobalResourceString("lbl_CCMIN20")) },
             {CCMAX20,Convert.ToString(GetGlobalResourceString("lbl_CCMAX20")) },
             {CCACTUAL20,Convert.ToString(GetGlobalResourceString("lbl_CCACTUAL20")) },
             {CCNAME21,Convert.ToString(GetGlobalResourceString("lbl_CCNAME21")) },
             {CCRANGE21,Convert.ToString(GetGlobalResourceString("lbl_CCRANGE21")) },
             {CCMIN21,Convert.ToString(GetGlobalResourceString("lbl_CCMIN21")) },
             {CCMAX21,Convert.ToString(GetGlobalResourceString("lbl_CCMAX21")) },
             {CCACTUAL21,Convert.ToString(GetGlobalResourceString("lbl_CCACTUAL21")) },
             {CCNAME22,Convert.ToString(GetGlobalResourceString("lbl_CCNAME22")) },
             {CCRANGE22,Convert.ToString(GetGlobalResourceString("lbl_CCRANGE22")) },
             {CCMIN22,Convert.ToString(GetGlobalResourceString("lbl_CCMIN22")) },
             {CCMAX22,Convert.ToString(GetGlobalResourceString("lbl_CCMAX22")) },
             {CCACTUAL22,Convert.ToString(GetGlobalResourceString("lbl_CCACTUAL22")) },
             {CCNAME23,Convert.ToString(GetGlobalResourceString("lbl_CCNAME23")) },
             {CCRANGE23,Convert.ToString(GetGlobalResourceString("lbl_CCRANGE23")) },
             {CCMIN23,Convert.ToString(GetGlobalResourceString("lbl_CCMIN23")) },
             {CCMAX23,Convert.ToString(GetGlobalResourceString("lbl_CCMAX23")) },
             {CCACTUAL23,Convert.ToString(GetGlobalResourceString("lbl_CCACTUAL23")) },
             {CCNAME24,Convert.ToString(GetGlobalResourceString("lbl_CCNAME24")) },
             {CCRANGE24,Convert.ToString(GetGlobalResourceString("lbl_CCRANGE24")) },
             {CCMIN24,Convert.ToString(GetGlobalResourceString("lbl_CCMIN24")) },
             {CCMAX24,Convert.ToString(GetGlobalResourceString("lbl_CCMAX24")) },
             {CCACTUAL24,Convert.ToString(GetGlobalResourceString("lbl_CCACTUAL24")) },
             {CCNAME25,Convert.ToString(GetGlobalResourceString("lbl_CCNAME25")) },
             {CCRANGE25,Convert.ToString(GetGlobalResourceString("lbl_CCRANGE25")) },
             {CCMIN25,Convert.ToString(GetGlobalResourceString("lbl_CCMIN25")) },
             {CCMAX25,Convert.ToString(GetGlobalResourceString("lbl_CCMAX25")) },
             {CCACTUAL25,Convert.ToString(GetGlobalResourceString("lbl_CCACTUAL25")) },
             {HARD_STD1,Convert.ToString(GetGlobalResourceString("lbl_HARD_STD1")) },
             {HARD_MIN1,Convert.ToString(GetGlobalResourceString("lbl_HARD_MIN1")) },
             {HARD_MAX1,Convert.ToString(GetGlobalResourceString("lbl_HARD_MAX1")) },
             {HARD_ACTUAL1,Convert.ToString(GetGlobalResourceString("lbl_HARD_ACTUAL1")) },
             {HARD_STD2,Convert.ToString(GetGlobalResourceString("lbl_HARD_STD2")) },
             {HARD_MIN2,Convert.ToString(GetGlobalResourceString("lbl_HARD_MIN2")) },
             {HARD_MAX2,Convert.ToString(GetGlobalResourceString("lbl_HARD_MAX2")) },
             {HARD_ACTUAL2,Convert.ToString(GetGlobalResourceString("lbl_HARD_ACTUAL2")) },
             {HARD_STD3,Convert.ToString(GetGlobalResourceString("lbl_HARD_STD3")) },
             {HARD_MIN3,Convert.ToString(GetGlobalResourceString("lbl_HARD_MIN3")) },
             {HARD_MAX3,Convert.ToString(GetGlobalResourceString("lbl_HARD_MAX3")) },
             {HARD_ACTUAL3,Convert.ToString(GetGlobalResourceString("lbl_HARD_ACTUAL3")) },
             {HARD_STD4,Convert.ToString(GetGlobalResourceString("lbl_HARD_STD4")) },
             {HARD_MIN4,Convert.ToString(GetGlobalResourceString("lbl_HARD_MIN4")) },
             {HARD_MAX4,Convert.ToString(GetGlobalResourceString("lbl_HARD_MAX4")) },
             {HARD_ACTUAL4,Convert.ToString(GetGlobalResourceString("lbl_HARD_ACTUAL4")) },
             {HARD_STD5,Convert.ToString(GetGlobalResourceString("lbl_HARD_STD5")) },
             {HARD_MIN5,Convert.ToString(GetGlobalResourceString("lbl_HARD_MIN5")) },
             {HARD_MAX5,Convert.ToString(GetGlobalResourceString("lbl_HARD_MAX5")) },
             {HARD_ACTUAL5,Convert.ToString(GetGlobalResourceString("lbl_HARD_ACTUAL5")) },
             {HARD_STD6,Convert.ToString(GetGlobalResourceString("lbl_HARD_STD6")) },
             {HARD_MIN6,Convert.ToString(GetGlobalResourceString("lbl_HARD_MIN6")) },
             {HARD_MAX6,Convert.ToString(GetGlobalResourceString("lbl_HARD_MAX6")) },
             {HARD_ACTUAL6,Convert.ToString(GetGlobalResourceString("lbl_HARD_ACTUAL6")) },
             {HARD_STD7,Convert.ToString(GetGlobalResourceString("lbl_HARD_STD7")) },
             {HARD_MIN7,Convert.ToString(GetGlobalResourceString("lbl_HARD_MIN7")) },
             {HARD_MAX7,Convert.ToString(GetGlobalResourceString("lbl_HARD_MAX7")) },
             {HARD_ACTUAL7,Convert.ToString(GetGlobalResourceString("lbl_HARD_ACTUAL7")) },
             {HARD_STD8,Convert.ToString(GetGlobalResourceString("lbl_HARD_STD8")) },
             {HARD_MIN8,Convert.ToString(GetGlobalResourceString("lbl_HARD_MIN8")) },
             {HARD_MAX8,Convert.ToString(GetGlobalResourceString("lbl_HARD_MAX8")) },
             {HARD_ACTUAL8,Convert.ToString(GetGlobalResourceString("lbl_HARD_ACTUAL8")) },
             {HARD_STD9,Convert.ToString(GetGlobalResourceString("lbl_HARD_STD9")) },
             {HARD_MIN9,Convert.ToString(GetGlobalResourceString("lbl_HARD_MIN9")) },
             {HARD_MAX9,Convert.ToString(GetGlobalResourceString("lbl_HARD_MAX9")) },
             {HARD_ACTUAL9,Convert.ToString(GetGlobalResourceString("lbl_HARD_ACTUAL9")) },
             {HARD_STD10,Convert.ToString(GetGlobalResourceString("lbl_HARD_STD10")) },
             {HARD_MIN10,Convert.ToString(GetGlobalResourceString("lbl_HARD_MIN10")) },
             {HARD_MAX10,Convert.ToString(GetGlobalResourceString("lbl_HARD_MAX10")) },
             {HARD_ACTUAL10,Convert.ToString(GetGlobalResourceString("lbl_HARD_ACTUAL10")) },
             {HARD_STD11,Convert.ToString(GetGlobalResourceString("lbl_HARD_STD11")) },
             {HARD_MIN11,Convert.ToString(GetGlobalResourceString("lbl_HARD_MIN11")) },
             {HARD_MAX11,Convert.ToString(GetGlobalResourceString("lbl_HARD_MAX11")) },
             {HARD_ACTUAL11,Convert.ToString(GetGlobalResourceString("lbl_HARD_ACTUAL11")) },
             {HARD_STD12,Convert.ToString(GetGlobalResourceString("lbl_HARD_STD12")) },
             {HARD_MIN12,Convert.ToString(GetGlobalResourceString("lbl_HARD_MIN12")) },
             {HARD_MAX12,Convert.ToString(GetGlobalResourceString("lbl_HARD_MAX12")) },
             {HARD_ACTUAL12,Convert.ToString(GetGlobalResourceString("lbl_HARD_ACTUAL12")) },
             {HARD_STD13,Convert.ToString(GetGlobalResourceString("lbl_HARD_STD13")) },
             {HARD_MIN13,Convert.ToString(GetGlobalResourceString("lbl_HARD_MIN13")) },
             {HARD_MAX13,Convert.ToString(GetGlobalResourceString("lbl_HARD_MAX13")) },
             {HARD_ACTUAL13,Convert.ToString(GetGlobalResourceString("lbl_HARD_ACTUAL13")) },
             {HARD_STD14,Convert.ToString(GetGlobalResourceString("lbl_HARD_STD14")) },
             {HARD_MIN14,Convert.ToString(GetGlobalResourceString("lbl_HARD_MIN14")) },
             {HARD_MAX14,Convert.ToString(GetGlobalResourceString("lbl_HARD_MAX14")) },
             {HARD_ACTUAL14,Convert.ToString(GetGlobalResourceString("lbl_HARD_ACTUAL14")) },
             {HARD_STD15,Convert.ToString(GetGlobalResourceString("lbl_HARD_STD15")) },
             {HARD_MIN15,Convert.ToString(GetGlobalResourceString("lbl_HARD_MIN15")) },
             {HARD_MAX15,Convert.ToString(GetGlobalResourceString("lbl_HARD_MAX15")) },
             {HARD_ACTUAL15,Convert.ToString(GetGlobalResourceString("lbl_HARD_ACTUAL15")) },
             {HARD_STD16,Convert.ToString(GetGlobalResourceString("lbl_HARD_STD16")) },
             {HARD_MIN16,Convert.ToString(GetGlobalResourceString("lbl_HARD_MIN16")) },
             {HARD_MAX16,Convert.ToString(GetGlobalResourceString("lbl_HARD_MAX16")) },
             {HARD_ACTUAL16,Convert.ToString(GetGlobalResourceString("lbl_HARD_ACTUAL16")) },
             {HARD_STD17,Convert.ToString(GetGlobalResourceString("lbl_HARD_STD17")) },
             {HARD_MIN17,Convert.ToString(GetGlobalResourceString("lbl_HARD_MIN17")) },
             {HARD_MAX17,Convert.ToString(GetGlobalResourceString("lbl_HARD_MAX17")) },
             {HARD_ACTUAL17,Convert.ToString(GetGlobalResourceString("lbl_HARD_ACTUAL17")) },
             {HARD_STD18,Convert.ToString(GetGlobalResourceString("lbl_HARD_STD18")) },
             {HARD_MIN18,Convert.ToString(GetGlobalResourceString("lbl_HARD_MIN18")) },
             {HARD_MAX18,Convert.ToString(GetGlobalResourceString("lbl_HARD_MAX18")) },
             {HARD_ACTUAL18,Convert.ToString(GetGlobalResourceString("lbl_HARD_ACTUAL18")) },
             {HARD_STD19,Convert.ToString(GetGlobalResourceString("lbl_HARD_STD19")) },
             {HARD_MIN19,Convert.ToString(GetGlobalResourceString("lbl_HARD_MIN19")) },
             {HARD_MAX19,Convert.ToString(GetGlobalResourceString("lbl_HARD_MAX19")) },
             {HARD_ACTUAL19,Convert.ToString(GetGlobalResourceString("lbl_HARD_ACTUAL19")) },
             {HARD_STD20,Convert.ToString(GetGlobalResourceString("lbl_HARD_STD20")) },
             {HARD_MIN20,Convert.ToString(GetGlobalResourceString("lbl_HARD_MIN20")) },
             {HARD_MAX20,Convert.ToString(GetGlobalResourceString("lbl_HARD_MAX20")) },
             {HARD_ACTUAL20,Convert.ToString(GetGlobalResourceString("lbl_HARD_ACTUAL20")) },
             {HARD_STD21,Convert.ToString(GetGlobalResourceString("lbl_HARD_STD21")) },
             {HARD_MIN21,Convert.ToString(GetGlobalResourceString("lbl_HARD_MIN21")) },
             {HARD_MAX21,Convert.ToString(GetGlobalResourceString("lbl_HARD_MAX21")) },
             {HARD_ACTUAL21,Convert.ToString(GetGlobalResourceString("lbl_HARD_ACTUAL21")) },
             {HARD_STD22,Convert.ToString(GetGlobalResourceString("lbl_HARD_STD22")) },
             {HARD_MIN22,Convert.ToString(GetGlobalResourceString("lbl_HARD_MIN22")) },
             {HARD_MAX22,Convert.ToString(GetGlobalResourceString("lbl_HARD_MAX22")) },
             {HARD_ACTUAL22,Convert.ToString(GetGlobalResourceString("lbl_HARD_ACTUAL22")) },
             {HARD_STD23,Convert.ToString(GetGlobalResourceString("lbl_HARD_STD23")) },
             {HARD_MIN23,Convert.ToString(GetGlobalResourceString("lbl_HARD_MIN23")) },
             {HARD_MAX23,Convert.ToString(GetGlobalResourceString("lbl_HARD_MAX23")) },
             {HARD_ACTUAL23,Convert.ToString(GetGlobalResourceString("lbl_HARD_ACTUAL23")) },
             {HARD_STD24,Convert.ToString(GetGlobalResourceString("lbl_HARD_STD24")) },
             {HARD_MIN24,Convert.ToString(GetGlobalResourceString("lbl_HARD_MIN24")) },
             {HARD_MAX24,Convert.ToString(GetGlobalResourceString("lbl_HARD_MAX24")) },
             {HARD_ACTUAL24,Convert.ToString(GetGlobalResourceString("lbl_HARD_ACTUAL24")) },
             {HARD_STD25,Convert.ToString(GetGlobalResourceString("lbl_HARD_STD25")) },
             {HARD_MIN25,Convert.ToString(GetGlobalResourceString("lbl_HARD_MIN25")) },
             {HARD_MAX25,Convert.ToString(GetGlobalResourceString("lbl_HARD_MAX25")) },
             {HARD_ACTUAL25,Convert.ToString(GetGlobalResourceString("lbl_HARD_ACTUAL25")) },
             };
            return _dictionary;
        }
        public static Dictionary<int, string> MatlListGet()
        {
            Dictionary<int, string> ResultMaterialType = new Dictionary<int, string>()
            {
             {K_MATERIAL_COIL,Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_Coil")) },
             {K_MATERIAL_BAR_KUROKAWA,Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_Bar_Kurokawa")) },
            };
            return ResultMaterialType;

        }
        public static Dictionary<int, string> ResultDecisionFIGet()
        {
            Dictionary<int, string> ResultKubun = new Dictionary<int, string>()
        {
             {EC_FI_RESULT_OK, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_OK")) },
             {EC_FI_RESULT_NG, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_NG")) },
             {EC_FI_RESULT_HOLD, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_Hold")) },
             {EC_FI_RESULT_NA, Convert.ToString(_localizationServices.GetLocalizedHtmlString("lbl_NA")) },
             

        };
            return ResultKubun;
        }
    }
}
