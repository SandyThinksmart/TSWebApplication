using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace TSWebApplication.Domain.Utility
{
    public class Constants
    {
        public class ExceptionNumber
        {
            public const int FK_CONSTRAIN = 2292;
            public const int UK_CONSTRAIN = 1;
        }

        public class MonthNumber
        {
            public const int JANUARY = 1;
            public const int FEBRUARY = 2;
            public const int MARCH = 3;
            public const int APRIL = 4;
            public const int MAY = 5;
            public const int JUNE = 6;
            public const int JULY = 7;
            public const int AUGUST = 8;
            public const int SEPTEMBER = 9;
            public const int OCTOBER = 10;
            public const int NOVEMBER = 11;
            public const int DECEMBER = 12;
        }

        public class ScreenDetails
        {
            public const string USER_NAME = "userName";
            public const string USER_ID = "userID";
            public const string ID = "id";
        }
        public class Numbers
        {
            public const string PO_START_SEQUENCE = "001";
            public const string ZERO = "0";
            public const string ONE = "1";
            public const string MINUS_ONE = "-1";
            public const string TWO = "2";
            public const string THREE = "3";
            public const string FOUR = "4";
            public const int LENGTH_NIFUDA = 9;
            public const int LENGTH_CHUUBAN = 9;
            public const int LENGTH_CHUUBAN_GYOUBAN = 11;
            public const int GENERAL_CUSTOMER_CD = 9999;//VEREFY THIS CUSTOMER_CD FROM MST_CUSTOMER MASTER
            public const short DEFAULT_CLOUSER_DAY = 99;
            public const int DELIVERY_NOTE_TYPE_SALES = 1;
            public const int DELIVERY_NOTE_TYPE_FACTORY = 2;
            public const int DELIVERY_NOTE_TYPE_SALESINVOICE = 3;
            public const int PRODUCT_KUBUN = 1;
            public const int MATERIAL_KUBUN = 2;
            

        }
        public class SessionName
        {
            public const string USER_ID = "USER_ID";
            public const string USER_NAME = "USER_NAME";
            public const string PASSWORD = "PASSWORD";
            public const string WORKSTATION_NAME = "WORKSTATION_NAME";
        }

        public class CDDisplayFormat
        {
            /* Display formats for CDs of all Masters */
            public const string MASTER_PACKING = "{0:000}";
            public const string MASTER_COUNTRY = "{0:000}";
            public const string MASTER_CUSTOMER_TYPE = "{0:000}";
            public const string MASTER_CURRENCY = "{0:000}";
            public const string MASTER_SHUKKA_DEFECT = "{0:000000}";
            public const string MASTER_CUSTOMER = "{0:000000}";
            public const string MASTER_CUSTOMER_FINANCE_DEPT = "{0:000000}";
            public const string MASTER_RECEIVER = "{0:000000}";
            public const string MASTER_SUPPLIER = "{0:000000}";
            public const string MASTER_BANK = "{0:000}";
            public const string MASTER_DESIGNATION = "{0:000}";
            public const string MASTER_EMPLOYEE = "{0:000000}";
            public const string MASTER_DEPARTMENT_GROUP = "{0:000000}";
            public const string MASTER_LOCATION = "{0:000}";
            public const string MASTER_DEPARTMENT = "{0:0000}";
            public const string MASTER_SALES_INCHARGE = "";        //
            public const string MASTER_MAKER = "{0:000}";
            public const string MASTER_MUKESAKI = "{0:0000}";
            public const string MASTER_FUTOU = "{0:000}";                // Shipyard and Warehouse Master
            public const string MASTER_HIKITORISAKI = "{0:0000}"; //ReceivingPlace Master
            public const string MASTER_SUBCONTRACTOR = "{0:000000}";
            public const string MASTER_PROCESS = "{0:0000}";
            public const string MASTER_PROCESS_TEMPLATE = "{0:00000000}";    //
            public const string MASTER_MACHINE = "{0:0000}";
            public const string MASTER_MACHINE_GROUP = "{0:000}";
            public const string MASTER_PRODUCT = "{0:00000000}";
            public const string MASTER_PROD_NAME_INTERNAL = "{0:000000}";
            public const string MASTER_STEEL_TYPE = "{0:000}";
            public const string MASTER_MATERIAL_NAME_INTERNAL = "{0:000000}";
            public const string MASTER_MATERIAL_NAME_MAKER = "{0:000000}";
            public const string MASTER_MATERIAL = "{0:000000}";
            public const string MASTER_CONDITION = "{0:000}";
            public const string MASTER_CONFIGURATION = "{0:000}";
            public const string MASTER_USER = "{0:000000}";
            public const string MASTER_PRODUCT_TYPE = "{0:000}";
            public const string MONTH_FORMAT = "00";
            public const string TRN_MATERIAL_NYUKO = "{0:00000000}";
            public const string MASTER_PROCESS_DEFECT = "{0:0000}";
            public const string MASTER_JIS_TEST = "{0:0000}";
            public const string MASTER_CHEMICAL_COMPOSITION = "{0:00000}";
            public const string TRANS_DEL_NOTE_FACTORY = "{0:000000}";
        }


        public class StringFormat
        {
            public const string ACT_DIM_D = "{0:##0.0##}";
        }
        public class CDMaxLength
        {
            public const int SHUKKA_INSTRUCTION_CD = 8;
            public const int NIFUDA_CD = 11;
            public const int NIFUDA_NO = 9;
            public const int CHUUBAN = 9;
            public const int GYOUBAN = 2;
            public const int CHUUBAN_GYOUBAN = 11;
            public const int PO_DTL_CD = 10;
            public const int PO_NO = 8;
            public const int ACT_ALLOC_NO = 14;
            public const int HIKITORI_IRAI_DTL_CD = 10;
        }

        public class PageURL
        {
            public const string LOGIN = "~/Login/LoginPage.aspx";
            public const string MENU = "~/Menu.aspx";
            public const string HTLOGIN = "~/HandyTerminal/LogInPage.aspx";
            public const string HTMENU = "/HandyTerminal/MenuScreen.aspx";
            public const string HTMENU_SALES = "/HandyTerminal/MenuScreenSales.aspx";
            public const string HTMENU_FACTORY = "/HandyTerminal/MenuScreenFactory.aspx";
            public const string MASTER_COUNTRY = "M_CountryEntry.aspx";
            public const string MASTER_CUSTOMER_TYPE = "M_CustomerTypeEntry.aspx";
            public const string MASTER_SHUKKA_DEFECT = "M_ShukkaDefectEntry.aspx";
            public const string MASTER_CURRENCY = "M_CurrencyEntry.aspx";
            public const string MASTER_CUSTOMER = "M_CustomerEntry.aspx";
            public const string MASTER_RECEIVER = "M_ReceiverEntry.aspx";
            public const string MASTER_SUPPLIER = "M_SupplierEntry.aspx";
            public const string MASTER_BANK = "M_BankEntry.aspx";
            public const string MASTER_DESIGNATION = "M_DesignationEntry.aspx";
            public const string MASTER_EMPLOYEE = "M_EmployeeEntry.aspx";
            public const string MASTER_DEPARTMENT_GROUP = "M_DepartmentGroupEntry.aspx";
            public const string MASTER_LOCATION = "M_LocationEntry.aspx";
            public const string MASTER_DEPARTMENT = "M_DepartmentEntry.aspx";
            public const string MASTER_SALES_INCHARGE = "";
            public const string MASTER_MAKER = "M_MakerEntry.aspx";
            public const string MASTER_MUKESAKI = "M_MukesakiEntry.aspx";
            public const string MASTER_FUTOU = "M_FotouEntry.aspx";
            public const string MASTER_HIKITORISAKI = "M_ReceivingPlaceEntry.aspx";
            public const string MASTER_SUBCONTRACTOR = "M_SubContractorEntry.aspx";
            public const string MASTER_PACKING = "M_PackingEntry.aspx";
            public const string MASTER_PROCESS = "M_ProcessEntry.aspx";
            public const string MASTER_PROCESS_TEMPLATE = "M_ProcessTemplateEntry.aspx";
            public const string MASTER_MACHINE = "M_MachineEntry.aspx";
            public const string MASTER_MACHINE_GROUP = "M_MachineGroupEntry.aspx";
            public const string MASTER_PRODUCT = "M_ProductMaster.aspx";
            public const string MASTER_PRODUCT_HISTORY = "../Masters/M_ProductMaster.aspx";
            public const string MASTER_PROD_NAME_INTERNAL = "M_ProductNameInternalEntry.aspx";
            public const string MASTER_STEEL_TYPE = "M_MaterialTypeEntry.aspx";
            public const string MASTER_MATERIAL_NAME_INTERNAL = "M_MaterialNameInternalEntry.aspx";
            public const string MASTER_MATERIAL_NAME_MAKER = "M_MaterialNameMakerEntry.aspx";
            public const string MASTER_MATERIAL = "M_MaterialEntry.aspx";
            public const string MASTER_CONDITION = "M_ConditionEntry.aspx";
            public const string MASTER_CONFIGURATION = "M_ConfigurationEntry.aspx";
            public const string MASTER_PRODUCT_TYPE = "M_ProductTypeEntry.aspx";
            public const string MASTER_PROCESS_DEFECT = "M_ProcessDefectEntry.aspx";
            public const string MASTER_JIS_TEST = "M_JISTest.aspx";
            public const string MASTER_CHEMICAL_COMPOSITION = "M_ChemicalCompositionEntry.aspx";
            public const string MASTER_MOCHIKOMISAKI = "M_MochikomisakiEntry.aspx";

            public const string LIST_MASTER_COUNTRY = "M_CountryList.aspx";
            public const string LIST_CURRENCY_LIST = "M_CurrencyList.aspx";
            public const string LIST_SUTESTMASTER_LIST = "SUTestMasterList.aspx";
            public const string LIST_SHUKKA_DEFECT_LIST = "M_ShukkaDefectList.aspx";
            public const string LIST_MASTER_BANK = "M_BankList.aspx";
            public const string LIST_MASTER_CONDITION = "M_ConditionList.aspx";
            public const string LIST_MASTER_CUSTOMER = "M_CustomerList.aspx";
            public const string LIST_MASTER_CUSTOMER_TYPE = "M_CustomerTypeList.aspx";
            public const string LIST_MASTER_DEPARTMENT_GROUP = "M_DepartmentGroupList.aspx";
            public const string LIST_MASTER_DEPARTMENT = "M_DepartmentList.aspx";
            public const string LIST_MASTER_DESIGNATION = "M_DesignationList.aspx";
            public const string LIST_MASTER_EMPLOYEE = "M_EmployeeList.aspx";
            public const string LIST_MASTER_FUTOU = "M_FotouList.aspx";
            public const string LIST_MASTER_HIKITORISAKI = "M_ReceivingPlaceList.aspx";
            public const string LIST_MASTER_LOCATION = "M_LocationList.aspx";
            public const string LIST_MASTER_MACHINE_GROUP = "M_MachineGroupList.aspx";
            public const string LIST_MASTER_MACHINE = "M_MachineList.aspx";
            public const string LIST_MASTER_MAKER = "M_MakerList.aspx";
            public const string LIST_MASTER_MATERIAL = "M_MaterialList.aspx";
            public const string LIST_MASTER_MATERIAL_NAME_INTERNAL = "M_MaterialNameInternalList.aspx";
            public const string LIST_MASTER_MATERIAL_NAME_MAKER = "M_MaterialNameMakerList.aspx";
            public const string LIST_MASTER_STEEL_TYPE = "M_MaterialTypeList.aspx";
            public const string LIST_MASTER_MUKESAKI = "M_MukesakiList.aspx";
            public const string LIST_MASTER_PACKING = "M_PackingList.aspx";
            public const string LIST_MASTER_PROCESS = "M_ProcessList.aspx";
            public const string LIST_MASTER_PROCESS_TEMPLATE = "M_ProcessTemplateList.aspx";
            public const string LIST_MASTER_PRODUCT = "M_ProductList.aspx";
            public const string LIST_MASTER_PROD_NAME_INTERNAL = "M_ProductNameInternalList.aspx";
            public const string LIST_MASTER_RECEIVER = "M_ReceiverList.aspx";
            public const string LIST_MASTER_SUBCONTRACTOR = "M_SubContractorList.aspx";
            public const string LIST_MASTER_SUPPLIER = "M_SuppliertList.aspx";
            public const string LIST_MASTER_CONFIGURATION = "M_ConfigurationList.aspx";
            public const string LIST_MASTER_PRODUCT_TYPE = "M_ProductTypeList.aspx";
            public const string LIST_MASTER_PROCESS_DEFECT = "M_ProcessDefectList.aspx";
            public const string LIST_MASTER_CHEMICAL_COMPOSITION = "M_ChemicalCompositionList.aspx";
            public const string LIST_MASTER_JIS_TEST = "M_JISTestList.aspx";
            public const string LIST_MASTER_MOCHIKOMISAKI = "M_MochikomisakiList.aspx";

            public const string TRN_EXCHANGE_RATE_ENTRY = "ExchangeRateEntryEditDelete.aspx";
            public const string TRN_PRODUCT_SEARCH_SCREEN = "ProductSearchScreen.aspx";
            public const string TRN_PROCESS_PLAN_SCREEN = "ProcessPlanScreen.aspx";
            public const string TRN_KAKOUIRAI_ENTRY = "KhakoiraiEntry.aspx";
            public const string TRN_PROCESS_PLAN_SEARCH_SCREEN = "ProcessPlanProductSearchScreen.aspx";

            public const string TRN_FI_CORNER_RADIUS_INFORMATION = "FICornerRadiusInformation.aspx";
            public const string TRN_FI_DATA_EXAMINER_NIFUDA_SCAN_SCREEN = "FIDateExaminerNifudaScanScreen.aspx";
            public const string TRN_FI_DECISION_CONFIRMATION = "FIDecisionConfirmationScreen.aspx";
            public const string TRN_FI_INSPECTION = "HTFI.aspx";
            public const string HT_FI_INSPECTION_SPRAK = "HTFISprak.aspx";
            public const string TRN_FI_PRODUCTSHUKKO = "HTShukko.aspx";
            public const string TRN_FI_NIFUDADA_QTY_WEIGHT_INFO = "FINifudadaQuantityWeightInformation.aspx";
            public const string TRN_FI_PRODUCT_INFO = "FIProductInformation.aspx";

            public const string TRN_PRODUCT_STOCK_VERIFICATION = "HTProductStockVerification.aspx";

            public const string TRN_FI_DEL_CORNER_RADIUS_INFORMATION = "FIDeleteCornerRadiusInformation.aspx";
            public const string TRN_FI_DEL_DATA_EXAMINER_NIFUDA_SCAN_SCREEN = "FIDeleteDateExaminerNifudaScanScreen.aspx";
            public const string TRN_FI_DEL_DECISION_CONFIRMATION = "FIDeleteDecisionConfirmationScreen.aspx";
            public const string TRN_FI_DEL_INSPECTION = "FIDeleteInspectionScreen.aspx";
            public const string TRN_FI_DEL_NIFUDADA_QTY_WEIGHT_INFO = "FIDeleteNifudadaQuantityWeightInformation.aspx";
            public const string TRN_FI_DEL_PRODUCT_INFO = "FIDeleteProductInformation.aspx";

            public const string TRN_FI_EDIT_CORNER_RADIUS_INFORMATION = "FIDeleteCornerRadiusInformation.aspx";
            public const string TRN_FI_EDIT_DATA_EXAMINER_NIFUDA_SCAN_SCREEN = "FIDeleteDateExaminerNifudaScanScreen.aspx";
            public const string TRN_FI_EDIT_DECISION_CONFIRMATION = "FIDeleteDecisionConfirmationScreen.aspx";
            public const string TRN_FI_EDIT_INSPECTION = "FIDeleteInspectionScreen.aspx";
            public const string TRN_FI_EDIT_NIFUDADA_QTY_WEIGHT_INFO = "FIDeleteNifudadaQuantityWeightInformation.aspx";
            public const string TRN_FI_EDIT_PRODUCT_INFO = "FIDeleteProductInformation.aspx";

            public const string TRN_FI_DIMENSION_LENGTH_INFO = "FIDimensionLengthInformation.aspx";
            public const string TRN_PRODUCT_DETAIL_SCREEN = "ProductDetailScreen.aspx";
            public const string TRN_ORDER_CONFIRMATION_MAIL_SCREEN = "OrderConfirmationMailScreen.aspx";

            public const string TRN_SHUKKA_INST_NIFUDA_GEN_LIST = "ShukkaInstructionNifudaGenerationList.aspx";


            public const string TRN_HT_PRODUCT_SHUKKA_SCREEN = "HTSohoShukka.aspx";
            public const string TRN_SHUKKO_LIST_SCREEN = "ShukkoListScreen.aspx";

            public const string TRN_HT_SPOT_PRODUCT_NYUKO = "HTSpotProductNyuko.aspx";

            public const string TRN_SPOT_PO_POPUP = "SpotPoPopup.aspx";

            public const string TRN_SPOT_NYUKA_ENTRY_EDIT_DELETE = "SpotNyukaEntryEditDelete.aspx";

            public const string TRN_KUROKUWA_NYUKA_HIKITORIIRAI_POPUP = "HikitoriIraiPopup.aspx";

            public const string TRN_SPOT_PRODUCT_DETAIL_SCREEN = "SpotProductHistoryDetails.aspx";

            public const string TRN_KUROKAWA_NYUKA_ENTRY_SCREEN = "KurokawaNyukaEntryScreen.aspx";

            //public const string TRN_NYUKO_EDIT_DELETE_SCREEN = "ProductNyuko.aspx";
            public const string TRN_NYUKO_LIST_SCREEN = "ProductNyukoListScreen.aspx";
            public const string TRN_HT_NIFUDA_DIVISION = "HTNifudaDivision.aspx";
            public const string HT_PRODUCT_SHUKKA_FACTORY_SCREEN = "HTProductShukkaFactory.aspx";
            public const string TRN_TESOOU_LIST_SCREEN = "TensouListPage.aspx";
            public const string TRN_HEATTREATMENT_LIST_SCREEN = "HeatTreatmentList.aspx";
        }
        public class DefaultValues
        {
            public const string MAX_ROW_LIMIT = "100";
            public const string SEARCH_COMBO_MAX_LIMIT = "50";
            public const int USE_FREQ_MAX_VALUE = 999;
            public const short ZERO_DEFAULT = 0;
            public const string KOUBAN_REM_AND_PROD_STOCK_QTY_SEARCH_CONDITION = "100";  // Kouban Completion screen
            public const short DISPLAY_PRIORITY = 99;
            public const int ISPAN_CUSTOMER = 999;
            public const int ADDRESS_LENGTH = 100;
            public const long DEFAULT_PRODUCT = 99999;
        }
        public class PrcoessPlanKubun
        {
            public const int GENERAL = 10;
            public const int REPROCESS = 20;
            public const int ADDEDPROCESS = 30;
            public const int CUTTING = 40;
        }


        public class Operators
        {
            public const string EQUAL = "==";
            public const string GREATER_THAN = ">";
            public const string GREATER_THAN_OR_EQUAL = ">=";
            public const string LESS_THAN = "<";
            public const string LESS_THAN_OR_EQUAL = "<=";
            public const string EQUAL_OR_GREATER_THAN = "=>";
            public const string MINUS = "-";
            public const string PLUS = "+";
            public const string BLANK_SPACE = " ";
            public const string COMMA = ",";
            public const string SLASH = "/";

        }
        public class Country
        {
            public const string JAPAN = "JAPAN";
            public const string PHILIPPINES = "PHILIPPINES";
        }
        public class Japan
        {
            public const string CITY1 = "本　　　　　　社";//16
            public const string CITY2 = "戸塚第一工場";
            public const string CITY3 = "栃木野木工場";
            public const string CITY4 = "北 関 東 支 店";
            public const string CITY5 = "高 崎 営 業 所";
            public const string CITY6 = "東 北 営 業 所";
            public const string CITY7 = "山 形 営 業 所";
            public const string CITY8 = "諏 訪 営 業 所";
            public const string CITY9 = "新 潟 営 業 所";
            public const string CITY10 = "";

            public const string ADDRESS1 = "東   京    都    大   田     区   東   六   郷";
            public const string ADDRESS2 = "神奈川県横 浜 市 戸 塚 区 戸 塚 町 407";
            public const string ADDRESS3 = "栃  木  県   下  都   賀   郡    野   木   町";
            public const string ADDRESS4 = "群 馬 県 邑 楽 郡 板 倉 町 岩 田 2415-1";
            public const string ADDRESS5 = "群馬県佐波郡玉村町大字上之手2065-2";
            public const string ADDRESS6 = "福  島  県  須   賀  川  市  横  山  町 99";
            public const string ADDRESS7 = "山形県村山市本飯田 字 柳 堤 2486-192";
            public const string ADDRESS8 = "長    野    県    茅   野    市   金  沢 691";
            public const string ADDRESS9 = "新  潟   県   三    条     市   帯  織 8278";
            public const string ADDRESS10 = "";

            public const string CONTACT1 = "03(3732) 1221〒144-0046";
            public const string CONTACT2 = "045（871）2511〒244-0003";
            public const string CONTACT3 = "0280（54）0525〒329-0105";
            public const string CONTACT4 = "0276（82）0118〒374-0133";
            public const string CONTACT5 = "0270（64）0080〒370-1127";
            public const string CONTACT6 = "0248（76）4662〒962-0041";
            public const string CONTACT7 = "0237（55）4830〒995-0003";
            public const string CONTACT8 = "0266（73）2999〒391-0012";
            public const string CONTACT9 = "0256(45)7121 〒959-1117";
            public const string CONTACT10 = "";
        }

        public class Philippines
        {
            public const string CITY1 = "Philippines 001";
            public const string CITY2 = "Philippines 002";
            public const string CITY3 = "Philippines 003";
            public const string CITY4 = "Philippines 004";
            public const string CITY5 = "Philippines 005";
            public const string CITY6 = "Philippines 006";
            public const string CITY7 = "Philippines 007";
            public const string CITY8 = "Philippines 008";
            public const string CITY9 = "Philippines 009";
            public const string CITY10 = "";

            public const string ADDRESS1 = "Philippines Address001";
            public const string ADDRESS2 = "Philippines Address002";
            public const string ADDRESS3 = "Philippines Address004";
            public const string ADDRESS4 = "Philippines Address004";
            public const string ADDRESS5 = "Philippines Address005";
            public const string ADDRESS6 = "Philippines Address006";
            public const string ADDRESS7 = "Philippines Address007";
            public const string ADDRESS8 = "Philippines Address008";
            public const string ADDRESS9 = "Philippines Address009";
            public const string ADDRESS10 = "";

            public const string CONTACT1 = "03(3732) 1221〒144-0046";
            public const string CONTACT2 = "045（871）2511〒244-0003";
            public const string CONTACT3 = "0280（54）0525〒329-0105";
            public const string CONTACT4 = "0276（82）0118〒374-0133";
            public const string CONTACT5 = "0270（64）0080〒370-1127";
            public const string CONTACT6 = "0248（76）4662〒962-0041";
            public const string CONTACT7 = "0237（55）4830〒995-0003";
            public const string CONTACT8 = "0266（73）2999〒391-0012";
            public const string CONTACT9 = "0256(45)7121 〒959-1117";
            public const string CONTACT10 = "";
        }

        public class AlertType
        {
            public const string WARNNING = "Warnning";
            public const string ERROR = "Error";
            public const string SUCCESS = "Success";

        }
        public class Control
        {
            public const string TEXTBOX = "TextBox";
            public const string DROPDOWN = "DropDown";
            public const string SEARCH_COMBO = "SearchCombo";
            public const string CHECKBOX = "CheckBox";
            public const string HIDDEN_FIELD = "HiddenField";
        }
        public class CssClass
        {
            public const string KOUBAN = "GridKouban";
            public const string KOUBANKUROKAWA = "GridKoubanKurokawa";
            public const string KOUBAN_MEDUIM = "GridKoubanMeduim";
            public const string MATL_NAME_MAKER = "GridMatlNameMakerTooLarge";
            public const string MATL_NAME_MAKER_L = "GridMatlNameMakerMeduim";
            public const string MATL_NAME_INTERNAL = "GridMatlNameInternal";
            public const string INTERNAL_NAME = "GridInternalName";
            public const string INTERNAL_NAME_LARGE = "GridInternalNameLarge";
            public const string MAKER_NAME = "GridMakerNameLargeXX";
            public const string MAKER_NAME_XL = "GridMakerNameXL";
            public const string WAREHOUSE_STOCK = "GridStockQtyShort";
            public const string GEN_SPECIAL = "GridGeneralSpecial";
            public const string PROCESS = "GridProcessNameLarge";
            public const string MACHINE = "GridMachineNameMedium";
            public const string MACHINE_SHORT = "GridMachineName";
            public const string LOCATION = "GridLocationMedium";
            public const string LOCATION_SHORT = "GridLocation";
            public const string MAKER_NAME_X = "GridMakerNameLargeX";
            public const string MAKER_NAME_FOR_PO = "GridMakerNameForPO";
            public const string PO_NUMBER = "GridPONumber";
            public const string PO_NUMBER_MEDUIM = "GridPONumberMeduim";
            public const string DIM = "GridDimD";
            public const string DIM_SHORT = "GridDimDShort";
            public const string DIM_D_W_OTH = "GridDimOtherLarge";
            public const string DIMD_SHORT = "GridDimDShort";
            public const string DIMD_SHAPE = "GridDimDShape";
            public const string DIM_D_W_OTHER = "GridDimDDimWOtherLargeXX";
            public const string DIM_OTHER = "GridDimOther";
            public const string PROD_DIM_OTHER = "GridDimOtherL";
            public const string SHAPE = "GridShapeTooShort";
            public const string SHAPE1 = "GridShapeTooShort1";  // Add this increse shape size by Sandeep Hajare  15/02/2022 
            public const string SHAPE_SHORT = "GridShapeShort";
            public const string LENGTH = "GridLength";
            public const string LENGTH_SHORT = "GridLengthMedium";
            public const string FUTOU_NAME = "GridFutouNameLarge";
            public const string FUTOU_NAME_MEDUIM = "GridFutouNameMeduim";
            public const string MOCHIKOMISAKI_NAME = "GridMochikomisakiName";
            public const string MUKESAKI_NAME = "GridMukesakiName";
            public const string ROLL = "GridRoll";
            public const string CHANCE = "GridChance";
            public const string CHANCE_S = "GridChanceSmall";
            public const string YEAR = "GridYear";
            public const string GridDate = "GridDate";
            public const string DATE_SHORT = "GridDateShort";
            public const string CONDITION = "GridCondition";
            public const string COIL_BAR = "GridCoilBar";
            public const string COIL_BAR_MED = "GridCoilBarVeryMedium";
            public const string COIL_BAR_MEDIUM = "GridCoilBarMeduim";
            public const string GENS_PEC = "GridGenSpec";
            public const string GENS_PEC_SMALL = "GridGenSpecSmall";
            public const string GENS_PEC_LONG = "GridGenSpecLong";
            public const string NYUKO_GENS_PEC = "GridGenSpecExtraLong";
            public const string GENS_PES_LONG = "GridGenSpecLong";
            public const string DATE = "GridDate txtDate";
            public const string NYUKA_DATE = "GridDateL txtDate";
            public const string QUANTITY_KG = "GridKGQuantity";
            public const string QUANTITY_TN = "GridTNQuantity";
            public const string NUYKO_QUANTITY_KG = "GridKGQuantityLarge";
            public const string SUPPLIER_NAME = "GridSupplierName";
            public const string SUPPLIER_NAME_FOR_PO = "GridSupplierNameForPO";
            public const string CONDITION_DESCRIPTION = "GridCondition";
            public const string CONDITION_DESCRIPTION_XL = "GridConditionXL";
            public const string CONDITION_DESCRIPTION_LARGE = "GridConditionDesc";
            public const string CHARGE = "GridCharge";
            public const string CHARGEXL = "GridCharge";
            //css clss for fields alignment in  gridview
            public const string CONDITION_ALIGNMENT = "GridAlignmentCondition";
            public const string DIMD_SHAPE_ALIGNMENT = "GridAlignmentDimDShape";
            public const string SHAPE_ALIGNMENT = "GridAlignmentShape";

            public const string PRODUCT_NAME_CUST = "GridProdNameCust";
            public const string CUSTOMER_NAME = "GridCustomerShort";
            public const string CUSTOMER_NAME_LARGE = "GridCustomerLarge";
            public const string PROD_NAME_CUST_LARGE = "GridProdNameCustLarge";
            public const string REPORT_PRINT_REPRINT = "GridReportPrintReprint";
        }
        public class ControlWidth
        {
            public const int LENGTH_WIDTH = 65;
            public const int DIM_D_WIDTH = 50;
            public const int DIM_D_WIDTH_SHORT = 30;
            public const int SHAPE_WIDTH = 30;
            public const int CONDITION_WIDTH = 60;
        }
        public class StringValues
        {
            public const string WHITESPACE = " ";
            public const string DEVELOPMENT = "development";
            public const string PRODUCTION = "production";
            public const string SPACE_EN = " ";
            public const string SPACE = " ";
            public const string NEW_LINE = "<br>";
            public const string LINE_BREAK = "</br>";
            public const string SPACE_JP = "　";
            public const string ZERO_STRING = "0";
            public const string COMMA_STRING = ",";
            public const string STAR_STRING = "*";
            public const string COLON = ":";
            public const string OK = "OK";
            public const int CHUUBAN_GYOUBAN_LENGTH = 11;
            public const string DOT_JP = "・";
            public const string EQUAL_OR_GREATER_THAN = "=>";
            public const string RIGHT = "⇒";

            public static string HIKINUKI = "HK";

            public static string STRAIGHTNING = "ST";
            public static string EC = "EC";
            public static string UT = "UT";
            public static string HT_Condaction = "HT";


            public static string SALES_INVOICE = "SALES";
            public static string CUSTOMER_INVOICE = "CUSTOMER";

            public const string COMPANY_NAME = "HSK";
            public const string UNDERSCORE = "_";
            public const string SPECIAL_CUSTOMER = "一般";      // CUSTOMER can not be modified or deletion
            public const string TEMP_PDF_FILE = "tempReport";

            public const string PERCENTAGE = "%";
            public static short NIFUDA_TYPE = 2;// "SPOT";
            public static short KUROKAWA_PRODUCT_NIFUDA = 2; //"KUROKAWA";
            public const string ONE_STRING = "1";
            public const string DIM_D_SEPARATED = "×";   //     DIM_D X DIM_W
            public const string TILDE_SIGN = "～";
            public const string DASH = "-";
            public const short GENERAL_NIFUDA = 4;
            public const short PROD_SHUKKO_SPLIT_KUBUN = 15;
            public const short PROD_NYUKO_SPLIT_KUBUN = 15;
            public const short PROD_SHUKKO_REPLACEMENT_KUBUN = 16;
            public const short PROD_NYUKO_REPLACEMENT_KUBUN = 16;
            public const string DEPLOYMENT_TARGET = "DeploymentTarget";
            public const string PREFIX_OF_CHAMFER = "C-";
            public const string PREFIX_OF_CORNER_RADIUS = "R-";
            public const string OPEN_PARAMETER = "(";
            public const string CLOSE_PARAMETER = ")";

            public const int STROGE_KUBUN_STOCK = 1;
            public const int STROGE_KUBUN_WIP = 2;
            public const int STROGE_KUBUN_NYUKA = 3;
            public const int MULTIPLE_ENTRY_POPUP_ROW_COUNT = 100;

            public const int NYUKO = 1;
            public const int SHUKOO = 2;
            public const int SHUKKA = 3;
        }

        public class MasterTable
        {
            public const string MST_MOCHIKOMISAKI = "MST_MOCHIKOMISAKI";
            public const string MST_HIKITORISAKI = "MST_HIKITORISAKI";
            public const string MST_MUKESAKI = "MST_MUKESAKI";
            public const string MST_EMPLOYEE = "MST_EMPLOYEE";
            public const string MST_FUTOU = "MST_FUTOU";
            public const string MST_LOCATION = "MST_LOCATION";
            public const string MST_RECEIVER = "MST_RECEIVER";
            public const string MST_BANK = "MST_BANK";
            public const string MST_MAKER = "MST_MAKER";
            public const string MST_MACHINE = "MST_MACHINE";
            public const string MST_CUSTOMER = "MST_CUSTOMER";
            public const string MST_MACHINE_GROUP = "MST_MACHINE_GROUP";
            public const string MST_SUPPLIER = "MST_SUPPLIER";
            public const string MST_PROCESS_DEFECT = "MST_PROCESS_DEFECT";
            public const string MST_JIS_TEST_SPEC = "MST_JIS_TEST_SPEC";
        }
        public class MasterTableCD
        {
            public const string MOCHIKOMISAKI_CD = "MOCHIKOMISAKI_CD";
            public const string HIKITORISAKI_CD = "HTSAKI_CD";
            public const string MUKESAKI_CD = "MUKESAKI_CD";
            public const string EMPLOYEE_CD = "EMPLOYEE_CD";
            public const string FUTOU_CD = "FUTOU_CD";
            public const string LOCATION_CD = "LOCATION_CD";
            public const string RECEIVER_CD = "RECEIVER_CD";
            public const string BANK_CD = "BANK_CD";
            public const string MAKER_CD = "MAKER_CD";
            public const string MACHINE_CD = "MACHINE_CD";
            public const string CUSTOMER_CD = "CUSTOMER_CD";
            public const string MACHINE_GROUP_CD = "MACHINE_GROUP_CD";
            public const string SUPPLIER_CD = "SUPPLIER_CD";
            public const string PROCESS_DEFECT_CD = "PROCESS_DEFECT_CD";
            public const string JIS_TEST_SPEC_CD = "JIS_TEST_SPEC_CD";
        }

        public class KubunStringValues
        {
            public const short PROD_NYUKO_FI_KUBUN = 10;
            public const short PROD_NYUKO_NOTINSPECTED_KUBUN = 11;
            public const short PROD_NYUKO_SPLIT_KUBUN = 15;
            public const short PROD_NYUKO_REPLACEMENT_KUBUN = 16;
            public const short PROD_NYUKO_USEASPRODUCT_KUBUN = 20;
            public const short PROD_NYUKO_SUBCONTRACTOR_KUBUN = 30;
            public const short PROD_NYUKO_STOCKTAKING_KUBUN = 80;
            public const short PROD_NYUKO_OTHER_KUBUN = 99;

            public const short PROD_SHUKKO_GENERAL_KUBUN = 10;
            public const short PROD_SHUKKO_SPLIT_KUBUN = 15;
            public const short PROD_SHUKKO_REPLACEMENT_KUBUN = 16;
            public const short PROD_SHUKKO_USEASMATERIAL_KUBUN = 20;
            public const short PROD_SHUKKO_SUBCONTRACTOR_KUBUN = 30;
            public const short PROD_SHUKKO_STOCKTAKING_KUBUN = 80;
            public const short PROD_SHUKKO_SCRAP_KUBUN = 90;
            public const short PROD_SHUKKO_OTHER_KUBUN = 99;

            public const short SHUKKA_GENERAL = 10;      // General Shukka
            public const short SHUKKA_SUBSTITUTE = 20;  //DaiyouShukka

        }

        public class Alignment
        {
            public const string CENTER = "Center";
            public const string LEFT = "Left";
            public const string RIGHT = "Right";
        }
        public class SortParameters
        {
            public const string DESC_ORDER = "Desc";

            public const string SORT_EXPRESSION = "Grid_SortExpression";
            public const string SORT_DIRECTION = "Grid_SortDirection";
        }

        public class ResourceStrings
        {
            public const string SAVE_SUCCESS = "SaveSuccess";
            public const string SAVE_ERROR = "SaveError";
            public const string BAR = "BAR";
            public const string COIL = "COIL";
            public const string KURUKUA = "コイル";
            public const string WARNNING = "W";
            public const string ERROR = "E";
            public const string SUCCESS = "S";
            public const string ACTUAL = "ｼﾞｯｾｷﾁ";
        }

        public class ResourceKey
        {
            public const string MASTER_COUNTRY_L = "lblMenu_CountryMaster";

            public const string MASTER_CUSTOMER_TYPE_L = "title_MasterCustomerType_L";
            public const string MASTER_CUSTOMER_L = "title_MasterCustomer_L";
            public const string CURRENCY_LIST = "title_CurrencyMaster_L";
            public const string SUTestMaster_LIST = "title_CurrencyMaster_L";
            public const string MASTER_SHUKKA_DEFECT_L = "title_ShukkaDefect_L";
            public const string MASTER_RECEIVER_L = "title_MasterReceiver_L";
            public const string MASTER_CUSTOMERFINANCE_L = "title_MasterCustomerFinance_L";

            public const string MASTER_SUPPLIER_L = "title_SupplierMaster";
            public const string MASTER_PAYEE_L = "title_MasterPayee_L";
            public const string MASTER_BANK_L = "title_MasterBank_L";

            public const string MASTER_DESIGNATION_L = "title_MasterDesignation_L";
            public const string MASTER_EMPLOYEE_L = "title_MasterEmployee_L";
            public const string MASTER_DEPARTMENT_GROUP_L = "title_MasterDepartmentGroup_L";
            public const string MASTER_LOCATION_L = "title_MasterLocation_L";
            public const string MASTER_DEPARTMENT_L = "title_MasterDepartment_L";
            public const string MASTER_SALES_INCHARGE_L = "title_MasterSalesIncharge_L";

            public const string MASTER_MAKER_L = "title_MasterMaker_L";
            public const string MASTER_MUKESAKI_L = "title_MasterMukesaki_L";
            public const string MASTER_FUTOU_L = "title_MasterFutou_L";
            public const string MASTER_HIKITORISAKI_L = "title_MasterReceivingPlace_L";
            public const string MASTER_SUBCONTRACTOR_L = "title_MasterSubcontractor_L";

            public const string MASTER_PROCESS_L = "title_MasterProcess_L";
            public const string MASTER_PROCESS_TEMPLATE_L = "title_MasterProcessTemplate_L";
            public const string MASTER_MACHINE_L = "lbl_MachineMaster";
            public const string MASTER_MACHINE_GROUP_L = "title_MasterMachineGroup_L";
            public const string MASTER_PRODUCT_L = "title_MasterProduct_L";
            public const string MASTER_PROD_NAME_INTERNAL_L = "lbl_MatlNameMakerInternalMaster";

            public const string MASTER_STEEL_TYPE_L = "title_MasterSteelType_L";
            public const string MASTER_MATERIAL_NAME_INTERNAL_L = "lbl_MatlNameMakerInternalMaster";
            public const string MASTER_MATERIAL_NAME_MAKER_L = "title_MaterialNameMaker_L";
            public const string MASTER_MATERIAL_L = "title_MasterMaterial_L";

            public const string MASTER_PRODUCT_TYPE_L = "lbl_ProductType_Master";

            public const string MASTER_CONDITION_L = "title_MasterCondition_L";
            public const string MASTER_CONFIGURATION_L = "title_MasterConfiguration_L";
            public const string MASTER_PACKING_L = "title_MasterPacking_L";
            public const string MASTER_PROCESS_DEFECT_L = "title_MasterProcessDefect_L";
            public const string MASTER_CHEMICAL_COMPOSITION_L = "title_MasterChemicalComposition_L";
            public const string MASTER_JIS_TEST = "title_MasterJISTest";

            public const string LIST_MASTER_COUNTRY_L = "title_ListMasterCountry_L";
            public const string LIST_MASTER_BANK_L = "title_ListMasterBank_L";
            public const string LIST_MASTER_CONDITION_L = "title_ListMasterCondition_L";
            public const string LIST_MASTER_CUSTOMER_L = "title_ListMasterCustomer_L";
            public const string LIST_CURRENCY_MASTER_L = "title_ListCurrencyMaster_L";
            public const string LIST_SHUKKA_DEFECT_MASTER_L = "title_ListShukkaDefectMaster_L";
            public const string LIST_MASTER_CUSTOMER_TYPE_L = "title_ListMasterCustomerType_L";

            public const string LIST_MASTER_DEPARTMENT_GROUP_L = "title_ListMasterDepartmentGroup_L";
            public const string LIST_MASTER_DEPARTMENT_L = "title_ListMasterDepartment_L";
            public const string LIST_MASTER_DESIGNATION_L = "title_ListMasterDesignation_L";
            public const string LIST_MASTER_EMPLOYEE_L = "title_ListMasterEmployee_L";
            public const string LIST_MASTER_FUTOU_L = "title_ListMasterFutou_L";
            public const string LIST_MASTER_HIKITORISAKI_L = "title_ListMasterReceivingPlace_L";
            public const string LIST_MASTER_LOCATION_L = "title_ListMasterLocation_L";
            public const string LIST_MASTER_MACHINE_GROUP_L = "title_ListMasterMachineGroup_L";
            public const string LIST_MASTER_MACHINE_L = "title_ListMasterMachine_L";
            public const string LIST_MASTER_MAKER_L = "title_ListMasterMaker_L";
            public const string LIST_MASTER_MATERIAL_L = "title_ListMasterMaterial_L";
            public const string LIST_MASTER_MATERIAL_NAME_INTERNAL_L = "lbl_InternalNameList";
            public const string LIST_MASTER_MATERIAL_NAME_MAKER_L = "title_ListMaterialNameMaker_L";
            public const string LIST_MASTER_STEEL_TYPE_L = "title_ListMasterSteelType_L";
            public const string LIST_MASTER_MUKESAKI_L = "title_ListMasterMukesaki_L";
            public const string LIST_MASTER_PAYEE_L = "title_ListMasterPayee_L";
            public const string LIST_MASTER_PROCESS_L = "lbl_ProcessList";
            public const string LIST_MASTER_PROCESS_TEMPLATE_L = "title_ListMasterProcessTemplate_L";
            public const string LIST_MASTER_PRODUCT_L = "title_ListMasterProduct_L";
            public const string LIST_MASTER_PROD_NAME_INTERNAL_L = "lbl_InternalNameList";
            public const string LIST_MASTER_RECEIVER_L = "title_ListMasterReceiver_L";
            public const string LIST_MASTER_SUBCONTRACTOR_L = "title_ListMasterSubcontractor_L";
            public const string LIST_MASTER_SUPPLIER_L = "title_ListMasterSupplier_L";
            public const string LIST_MASTER_CONFIGURATION_L = "title_ListMasterConfiguration_L";
            public const string LIST_MASTER_PACKING_L = "title_ListMasterPacking_L";
            public const string LIST_MASTER_PRODUCT_TYPE_L = "lbl_ProductTpe_List";
            public const string LIST_JIS_TEST = "title_ListMasterJISTest";

        }

        public class SortOrder
        {
            public const string ASC = "asc";
            public const string DESC = "desc";
            public const string ACTIVE = "active";
            public const string IN_ACTIVE = "disabled";
            public const string IME_MODE = "ime-mode";
        }

        public class IMEMode
        {
            public const string ACTIVE = "active";
            public const string IN_ACTIVE = "disabled";
            public const string IME_MODE = "ime-mode";
        }

        public class ViewStateKeys
        {
            public const string SORT_CRITERIA = "SortCriteria";
            public const string SEARCH_CRITERIA = "SearchCriteria";
        }

        public class GridRowBackColors
        {
            public const string DEFAULT_BACK_COLOR = "#E2E2E2";
            public const string ALTERNET_ROW_BACK_COLOR = "#F7F7F7";
            public const string SELECTED_ROW_BACK_COLOR = "#FFCC33";

        }

        public class InActiveFlag
        {
            public const short ACTIVE = 0;
            public const short IN_ACTIVE = 1;
        }

        public class FileContentType
        {
            public const string PDF = "Application/pdf";
            public const string TEXT = "Application/txt";
        }
        public class KoubanComplete
        {
            public const short Kouban_InComplete = 0;
            public const short Kouban_Complete = 1;
        }

        public class NIFUDA_TYPE
        {
            public const short PRODUCT = 1;
            public const short SPOT = 2;
            public const short KUROKAWA = 2;
            public const short SHUKKA = 5;
            public const short GENERAL = 4;
        }

        public class Entities
        {
            public const string CUSTOMER = "CUSTOMER";
            public const string DRAWING = "DRAWING";
            public const string EMPLOYEE = "EMPLOYEE";
            public const string LOCATION = "LOCATION";
            public const string DEPARTMENT = "DEPARTMENT";
            public const string DESIGNATION = "DESIGNATION";
            public const string MACHINE = "MACHINE";
            public const string MATERIAL_TYPE = "MATERIAL_TYPE";
            public const string MOCHIKOMISAKI = "MOCHIKOMISAKI";
            public const string HIKITORISAKI = "HIKITORISAKI";
            public const string FUTOU = "FUTOU";
            public const string MAKER = "MAKER";
            public const string CONDITION = "CONDITION";
            public const string MATL_NAME_MAKER = "MATL_NAME_MAKER";
            public const string MATL_NAME_MAKER_INTERNAL_STEEL_TYPE = "MATL_NAME_MAKER.MATL_NAME_INTERNAL.MATERIAL_TYPE";
            public const string MATL_NAME_INTERNAL_STEEL_TYPE = "MATL_NAME_INTERNAL.MATERIAL_TYPE";
            public const string CUSTOMER_FINANCE_DEPT = "CUSTOMER_FINANCE_DEPT";
            public const string CUSTOMER_DEPARTMENT = "CUSTOMER_DEPARTMENT";
            public const string DEPARTMENT_CUSTOMER = "CUSTOMER_DEPARTMENT.CUSTOMER";
            public const string KOUBAN_MATERIAL_CONDITION = "KOUBAN.MATERIAL.CONDITION";
            public const string KOUBAN_NYUKO = "KOUBAN.NYUKO";
            public const string KOUBAN_MATERIAL = "KOUBAN.MATERIAL";
            public const string KOUBAN_MATERIAL_MATL_NAME_MAKER_MAKER = "KOUBAN.MATERIAL.MATL_NAME_MAKER.MAKER";
            public const string KOUBAN_MATERIAL_FUTOU = "KOUBAN.MATERIAL.FUTOU";
            public const string SUPPLIER = "SUPPLIER";
            public const string MACHINE_LOCATION = "MACHINE.LOCATION";
            public const string PRODUCT_NAME_INTERNAL = "PRODUCT_NAME_INTERNAL";
            public const string PRODUCT_NAME_CUSTOMER = "PRODUCT_NAME_CUSTOMER";
            public const string TEMP_ALLOCATION = "TEMP_ALLOCATION";
            public const string KOUBAN = "KOUBAN";
            public const string KAKOUIRAI = "KAKOUIRAI";
            public const string PRODUCT = "PRODUCT";
            public const string V_PRODUCT_DETAILS = "V_PRODUCT_DETAILS";
            public const string MATL_NAME_MAKER_MATL_NAME_INTERNAL = "MATL_NAME_MAKER.MATL_NAME_INTERNAL";
            public const string MUKESAKI = "MUKESAKI";
            public const string TENSOU = "TENSOU";
            public const string ACCEPTANCE = "ACCEPTANCE";
            public const string BUNKATSU = "BUNKATSU";
        }

        public class Masters
        {
            public const string COUNTRY_MASTER = "COUNTRY MASTER";
            public const string PRO_NAME_INTERNAL = "PRODUCT NAME INTERNAL MASTER";
            public const string MATERIAL = "MATERIAL MASTER";
            public const string MATERIAL_NAME_INTERNAL = "MATERIAL NAME INTERNAL MASTER";
            public const string MATERIAL_NAME_MAKER = "MATERIAL NAME MAKER MASTER";
            public const string CONDITION_MASTER = "CONDITION MASTER";
            public const string BANK = "BANK MASTER";
            public const string CUSTOMER = "CUSTOMER MASTER";
            public const string CUSTOMER_BILLING = "CUSTOMER BILLING MASTER";
            public const string CUSTOMER_TYPE = "CUSTOMER TYPE MASTER";
            public const string DEPARTMENT = "DEPARTMENT MASTER";
            public const string DEPARTMENT_GROUP = "DEPARTMENT GROUP MASTER";
            public const string DESIGNATION = "DESIGNATION MASTER";
            public const string EMPLOYEE = "EMPLOYEE MASTER";
            public const string FUTOU = "FUTOU  MASTER";
            public const string HIKITORISAKI = "HIKITORISAKI MASTER";
            public const string LOCATION = "LOCATION MASTER";
            public const string MODEL = "MODEL MASTER";
            public const string MACHINE_GROUP = "MACHINE GROUP MASTER";
            public const string MAKER = "MAKER MASTER";
            public const string MATERIAL_CLASSIFICATION = "MATERIAL CLASSIFICATION MASTER";
            public const string MUKESAKI = "MUKESAKI MASTER";
            public const string PACKING_MASTER = "PACKING MASTER";
            public const string PROCESS = "PROCESS MASTER";
            public const string PROCESS_TEMPLATES = "PROCESS TEMPLATES MASTER";
            public const string RECEIVER = "RECEIVER MASTER";
            public const string SALES_INCHARGE = "SALES INCHARGE MASTER";
            public const string SPO_PAID_MATERIAL = "SPOT AND PAID MATERIAL MASTER";
            public const string SUBCONTRACTOR = "SUBCONTRACTOR MASTER";
            public const string SUPPLIER = "SUPPLIER MASTER";
            public const string MOCHIKOMISAKI = "MOCHIKOMISAKI MASTER";
            public const string PRODUCT = "PRODUCT MASTER";
            public const string CONFIGURATION = "CONFIGURATION MASTER";
            public const string MATERIAL_TYPE = "MATERIAL TYPE MASTER";
            public const string MACHINE = "MACHINE MASTER";
            public const string PRODUCT_TYPE = "PRODUCT TYPE MASTER";
            public const string JIS_TEST = "JIS TEST SPEC MASTER";

            public const string BANK_LIST = "BANK LIST";
            public const string CONDITION_LIST_PAGE = "CONDITION LIST";
            public const string COUNTRY_LIST_PAGE = "COUNTRY LIST";
            public const string CURRENCY_LIST_PAGE = "CURRENCY_LIST";
            public const string SHUKKA_DEFECT_LIST_PAGE = "SHUKKA_DEFECT_LIST_PAGE";
            public const string CUSTOMER_BILLING_LIST = "CUSTOMER BILLING LIST";
            public const string CUSTOMER_LIST = "CUSTOMER LIST";
            public const string CUSTOMER_TYPE_LIST = "CUSTOMER TYPE LIST";
            public const string DEPARTMENT_LIST = "DEPARTMENT LIST";
            public const string DEPARTMENT_GROUP_LIST = "DEPARTMENT GROUP LIST";
            public const string DESIGNATION_LIST = "DESIGNATION  LIST";
            public const string EMPLOYEE_LIST = "EMPLOYEE LIST";
            public const string FUTOU_LIST = "FUTOU LIST";
            public const string HIKITORISAKI_LIST = "HIKITORISAKI LIST";
            public const string LOCATION_LIST = "LOCATION LIST";
            public const string MODEL_LIST = "MODEL LIST";
            public const string MACHINE_GROUP_LIST = "MACHINE GROUP LIST";
            public const string MAKER_LIST = "MAKER LIST";
            public const string MATERIAL_LIST = "MATERIAL LIST";
            public const string MATERIAL_NAME_INTERNAL_LIST = "MATERIAL NAME MAKER LIST";
            public const string MATERIAL_NAME_MAKER_LIST = "MATERIAL NAME MAKER LIST";
            public const string MATERIAL_CLASSIFICATION_LIST = "MATERIAL CLASSIFICATION LIST";
            public const string MOCHIKOMISAKI_LIST = "MOCHIKOMISAKI LIST";
            public const string MUKESAKI_LIST = "MUKESAKI LIST";
            public const string PROCESS_LIST = "PROCESS LIST";
            public const string PRODUCT_LIST = "PRODUCT LIST";
            public const string PRODUCT_VERSION_LIST = "PRODUCT VERSION LIST";
            public const string PRO_NAME_INTERNAL_LIST = "PRODUCT NAME INTERNAL LIST";
            public const string RECEIVER_LIST = "RECEIVER LIST";
            public const string SPO_PAID_MATERIAL_LIST = "SPOT AND PAID MATERIAL LIST";
            public const string SUBCONTRACTOR_LIST = "SUBCONTRACTOR LIST";
            public const string SUPPLIER_LIST = "SUPPLIER LIST";
            public const string CONFIGURATION_LIST = "CONFIGURATION LIST";
            public const string MATERIAL_TYPE_LIST = "MATERIAL TYPE LIST MASTER";
            public const string MACHINE_LIST = "MACHINE LIST MASTER";
            public const string PRODUCT_TYPE_LIST = "PRODUCT TYPE LIST MASTER";
            public const string JIS_TEST_LIST = "JIS TEST SPEC LIST";

        }

       
        public class CountryMaster
        {
            public const string COUNTRY_CD = "COUNTRY_CD";
            public const string COUNTRY_NAME = "COUNTRY_NAME";
            public const string CREATED_DATE = "CREATED_DATE";
            public const string UPDATED_DATE = "UPDATED_DATE";

        }
        public class ShukkaDefectMaster
        {
            public const string SHUKKA_DEFECT_CD = "SHUKKA_DEFECT_CD";
            public const string DEFECT_LOCATION = "DEFECT_LOCATION";
            public const string DEFECT_DESCRIPTION = "DEFECT_DESCRIPTION";
            public const string CREATED_DATE = "CREATED_DATE";
            public const string UPDATED_DATE = "UPDATED_DATE";

        }
        /// <summary>
        ///     For name of report save in Temp folder.
        /// </summary>
        /// <createdBy>
        ///     Created By SatishP[20160712].
        /// </createdBy>
        public class Reports
        {
            public const string WIP_TESOU_REQUEST_LIST_REPORT = "WIPTensouRequestList";
            public const string WIP_TESOU_ACCEPTANCE_LIST_REPORT = "WIPTensouAcceptanceList";
            public const string PRODUCT_STOCK_NIFUDA = "ProductStockNifuda";
            public const string SALES_LEDGER_REPORT = "SalesLedgerReport";
            public const string SPOT_PRODUCT_STOCK_NIFUDA = "SpotProductStockNifuda";
            public const string SHUKKA_TENSOUE_NIFUDA = "ShukkaTensouNifuda";
            public const string PROCESS_PLAN = "ProcessPlan";
            public const string SHUKKA_DELIVERY = "ShukkaDelivery";
            public const string SALES_SHUKKA_DELIVERY = "SalesShukkaDelivery";
            public const string FINAL_INSPECTION = "FinalInspection";
            public const string SHUKKA_TENSOU_INST_CARD = "ShukkaTensouInstrCard";
            public const string SPOT_SHUKKA_TENSOU_INST_CARD = "SpotShukkaTensouInstrCard";
            public const string PRODUCT_STOCK_MONTHLY = "ProductStockMonthly";
            public const string UNVERIFIED_PRODUCT_STOCK = "UnverifiedProductStock";
            public const string PRODUCT_STOCK_LIST = "ProductStockList";
            public const string SPOT_PRODUCT_STOCK_LIST = "SpotProductStockList";
            public const string SPOT_MONTHLY_PRODUCT_STOCK_LIST = "SpotMonthlyProductStockList";
            public const string MONTHLY_PRODUCT_STOCK_LIST = "MonthlyProductStockList";
            public const string MONTHLY_SHUKKA_STOCK_LIST = "MonthlyShukkaStockList";
            public const string UNVERIFIED_PRODUCT_STOCK_REPORT = "UnverifiedProductStock";
            public const string MONTHLY_PRODUCT_STOCK_REPORT = "MonthlyProductStock";
            public const string PURCHASE_ORDER = "PurchaseOrder";
            public const string SPOT_PURCHASE_ORDER = "SpotPurchaseOrder";
            public const string HIKINUKI = "Hikinuki";
            public const string STRAIGHTENING = "Straightening";
            public const string KAKOUIRAI_REPORT = "Kakouirai";
            public const string KAKOUIRAI_DELIVERY_CONFIRM_REPORT = "KakouiraiDeliveryConfirm";
            public const string HOLD_KAKOUIRAI_REPORT = "HoldKakouirai";
            public const string FACTORY_PRODUCTION_PLAN = "FactoryProductionPlan";
            public const string KUROKUWA_NYUKA_PLAN_REPORT = "KurokuwaNyukaPlan";
            public const string SPOT_NYUKA_PLAN_REPORT = "SpotNyukaPlan";
            public const string SPOT_PRODUCT_STOCK_TAKING_BATCH_LIST_REPORT = "SpotProductStockTakingBatchList";
            public const string UNVERIFIED_SPOT_PRODUCT_STOCK = "UnverifiedSpotProductStock";
            public const string VERIFIED_PRODUCT_STOCK = "VerifiedProductStock";
            public const string SALES_RECEIVABLES_REPORT = "SalesReceivablesReport";
            public const string PREINVOICE_DATA_REPORT = "PreInvoiceDataReport";
            public const string UNCONFIRM_RATE_REPORT = "UnconfirmRateReport";
            public const string FINAL_INVOICE_REPORT = "FinalInvoiceReport";
            public const string POMAIN_ANALYSIS_REPORT = "RPOMainAnalysisReport";
        }
        public class ReportsNames
        {
            public const string ALL_PRODUCT_STOCK_LIST_REPORT = "AllProductStockList";
            public const string ALL_SPOT_PRODUCT_STOCK_LIST_REPORT = "AllSpotProductStockList";
            public const string UNVERIFIED_PRODUCT_STOCK_LIST_REPORT = "UnverifiedProductStockList";
            public const string UNVERIFIED_SPOT_PRODUCT_STOCK_LIST_REPORT = "UnverifiedSpotProductStockList";
            public const string VERIFIED_PRODUCT_STOCK_LIST_REPORT = "VerifiedProductStockList";
            public const string SPOT_NYUKA_RECEIVING_LIST = "SpotNyukaReceivingList";
            public const string PENDING_PRODUCTION_ORDER_LIST = "PendingProductionOrderList";
            public const string WIP_PRODUCT_PROCESS_REPORT = "WipProductProcessReport";
        }

        public class Transaction
        {
            public const string KAKOUIRAI = "KAKOUIRAI";
            public const string KAKOUIRAI_LIST = "KAKOUIRAI LIST";
            public const string DELIVERY_CONFIRMATION = "DELIVERY CONFIRMATION";

            public const string SHUKKA_TENSOU_INSTRUCTION_CARD = "SHUKKA TENSOU INSTRUCTION CARD";
            public const string SHUKKA_TENSOU_INSTRUCTION_CARD_LIST = "SHUKKA TENSOU INSTRUCTION CARD LIST";
            public const string SPOT_SHUKKA_TENSOU_INSTRUCTION_CARD = "SPOT SHUKKA TENSOU INSTRUCTION CARD";
            public const string SPOT_SHUKKA_TENSOU_INSTRUCTION_CARD_LIST = "SPOT SHUKKA TENSOU INSTRUCTION CARD LIST";
            public const string SHUKKA_INSTRUCTION_NIFUDA_GENERATION_SCREEN = "SHUKKA INSTRUCTION NIFUDA GENERATION SCREEN";
            public const string SHUKKA_INSTRUCTION_NIFUDA_GENERATION_LIST_SCREEN = "SHUKKA INSTRUCTION NIFUDA GENERATION LIST";
            public const string SHUKKA_NIFUDA_GENERATION_SCREEN = "SHUKKA NIFUDA GENERATION ";
            public const string SHUKKA_NIFUDA_REPRINT_SCREEN = "SHUKKA NIFUDA REPRINT ";
            public const string SHUKK0_SCREEN = "HT SHUKK0 ";

            public const string THABA_COMPLETION = "THABA COMPLETION";  //
            public const string INVOICE_GENERATION = "INVOICE GENERATION";
            public const string DELIVERY_DATE_PRINT = "DELIVERY DATE PRINT";
            public const string PURCHASE_ORDER = "PURCHASE ORDER";
            public const string HIKITORIIRAI_ENTRY = " HIKITORIIRAI ENTRY";
            public const string HIKITORIIRAI_REPORT = " HIKITORIIRAI REPORT";

            public const string SPOT_PURCHASE_ORDER = "SPOT PURCHASE ORDER";
            public const string KUBAN = "KOBAN";
            public const string NYUKO = "NYUKO";
            public const string ACT_ALLOCATION = "ACT ALLOCATION";
            public const string NYUKO_EDIT_DELETE = "NYUKO EDIT DELETE";
            public const string TEMP_ALLOCATION = "TEMP ALLOCATION";
            public const string CHANGE_PASSWORD = "CHANGE PASSWORD";
            public const string LOGIN = "LOGIN PAGE";
            public const string HIKINUKI_ENTRY = "HIKINUKI ENTRY";
            public const string HIKINUKI_EDIT_DELETE = "HIKINUKI EDIT DELETE";
            public const string EC_INSPECTION = "EC INSPECTION";
            public const string FINAL_INSPECTION_II = "FINAL INSPECTION II";
            public const string FINAL_INSPECTION_REPORT = "FINAL INSPECTION REPORT";
            public const string PROCESS_PLAN_COMPLETION = "PROCESS PLAN COMPLETION";
            public const string KOUBAN_COMPLETION = "KOUBAN COMPLETION";
            public const string PROCESS_PLAN_REPORT_DELETE = "PROCESS PLAN REPORT DELETE";
            public const string REQUESTED_MATERIAL_INTAKE = "REQUESTED MATERIAL INTAKE";
            public const string REQUESTED_MATERIAL_INTAKE_REPORT = "REQUESTED MATERIAL INTAKE REPORT";
            public const string JIZAII_NYUKA = "JIZAII NYUKA";
            public const string JIZAII_NYUKA_EDIT_DELETE = "JIZAII NYUKA EDIT DELETE";
            public const string FACTORY_PRODUCTION_PLAN = "FACTORY PRODUCTION PLAN";
            public const string NIFUDA_PRINT = "NIFUDA PRINT";
            public const string NIFUDA_DIVISION = "NIFUDA DIVISION";
            public const string PROCESS_PLAN = "PROCESS PLAN";
            public const string PROCESS_PLAN_LIST = "PROCESS PLAN LIST";
            public const string SHUKKA_NIFUDA_GENERATION_LIST = "SHUKKA NIFUDA GENERATION LIST ";
            public const string MATERIAL_SHUKKO = "MATERIAL SHUKKO";
            public const string MATERIAL_SHUKKO_EDIT_DELETE = "MATERIAL SHUKKO EDIT DELETE";
            public const string FUTOU_SHUKKO = "FUTOU SHUKKO";
            public const string FUTOU_SHUKKO_EDIT_DELETE = "FUTOU SHUKKO EDIT DELETE";
            public const string MATERIAL_NYUKO_KOUBAN_POPUP = "MATERIAL NYUKO KOUBAN POPUP";
            public const string EXCHANGE_RATE_ENTRY_EDIT_DELETE = "EXCHANGE RATE ENTRY EDIT DELETE";
            public const string EXCHANGE_RATE_ENTRY_LIST = "EXCHANGE RATE ENTRY LIST";
            public const string SPECIAL_PRODUCT_TAG_GENERATION = "SPECIAL PRODUCT TAG GENERATION";
            public const string SHUKKA_NIFUDA_GENERATION_BACH = "SHUKKA NIFUDA GENERATION BACH";
            public const string STAMPING_ENTRY = "STAMPING ENTRY";
            public const string STAMPING_EDIT_DELETE = "STAMPING EDIT DELETE";
            public const string WIP_TENSOU = "WIP TENSO ENTRY EDIT DELETE";
            public const string WIP_TENSOU_LIST = "WIP TENSO LIST";
            public const string Shukka = "出荷";
            public const string Nyukka = "入荷";


            public const string FI_CORNER_RADIUS_INFORMATION = "FI CORNER RADIUS INFORMATION";
            public const string FI_DATA_EXAMINER_NIFUDA_SCAN_SCREEN = "FI DATA EXAMINER NIFUDA SCAN SCREEN";
            public const string FI_DECISION_CONFIRMATION = "FI  DECISION CONFIRMATION";
            public const string FI_INSPECTION = "FI INSPECTION";
            public const string FI_INSPECTION_SPRAK = "FI INSPECTION SPRAK";
            public const string FI_NIFUDADA_QTY_WEIGHT_INFO = "FI NIFUDADA QUANTITY WEIGHT INFORMATION";
            public const string FI_PRODUCT_INFO = " FI PRODUCT INFORMATION";
            public const string FI_DIMENSION_LENGTH_INFO = "FI DIMENSION LENGTH INFORMATION";

            public const string FI_DEL_CORNER_RADIUS_INFORMATION = "FI DELETE CORNER RADIUS INFORMATION";
            public const string FI_DEL_DATA_EXAMINER_NIFUDA_SCAN_SCREEN = "FI DELETE DATA EXAMINER NIFUDA SCAN SCREEN";
            public const string FI_DEL_DECISION_CONFIRMATION = "FI DELETE DECISION CONFIRMATION";
            public const string FI_DEL_INSPECTION = "FI DELETE INSPECTION";
            public const string FI_DEL_NIFUDADA_QTY_WEIGHT_INFO = "FI DELETE NIFUDADA QUANTITY WEIGHT INFORMATION";
            public const string FI_DEL_PRODUCT_INFO = "FI DELETE PRODUCT INFORMATION";

            public const string FI_EDIT_CORNER_RADIUS_INFORMATION = "FI EDIT CORNER RADIUS INFORMATION";
            public const string FI_EDIT_DATA_EXAMINER_NIFUDA_SCAN_SCREEN = "FI EDIT DATA EXAMINER NIFUDA SCAN SCREEN";
            public const string FI_EDIT_DECISION_CONFIRMATION = "FI EDIT DECISION CONFIRMATION";
            public const string FI_EDIT_INSPECTION = "FI EDIT INSPECTION";
            public const string FI_EDIT_NIFUDADA_QTY_WEIGHT_INFO = "FI EDIT NIFUDADA QUANTITY WEIGHT INFORMATION";
            public const string FI_EDIT_PRODUCT_INFO = "FI EDIT PRODUCT INFORMATION";
            public const string PROCESS_PLAN_REPROCESSING1 = "PROCESS PLAN REPROCESSING1";

            public const string SHUKKO_SCREEN = "SHUUKO SCREEN";
            public const string SPOT_MATERIAL_NYUKO_SCREEN = "SPOT_MATERIAL_NYUKO_SCREEN";
            public const string SPOT_MATERIAL_NYUKO_LIST = "SPOT_MATERIAL_NYUKO_LIST";
            public const string POST_VERIFIACTION_PRODUCT_STOCK_REPORT_REPRINT = "POST VERIFIACTION PRODUCT STOCK REPORT REPRINT SCREEN";
            public const string POST_VERIFIACTION_PRODUCT_STOCK_REPORT_GENERATION_SCREEN = "POST VERIFIACTION PRODUCT STOCK REPORT GENERATION SCREEN";
            public const string LOCATION_BASED_REPORT_GENERATION_SCREEN = "LOCATION BASED REPORT GENERATION SCREEN";
            public const string HT_NIFUDA_DETAILS = "HT NIFUDA DETAILS";
            public const string HT_SPOT_PRODUCT_NYUKO_SCREEN = "HT SPOT PRODUCT NYUKO SCREEN";
            public const string HT_NYUKO_SCREEN = "HT_NYUKO_SCREEN";
            public const string SALES_LEDGER_REPORT_SCREEN = "SALES LEDGER REPORT SCRREN";
            public const string SALES_LEDGER_REPORT = "SALES LEDGER REPORT";

            public const string PRODUCT_STOCK_SEARCH_SCREEN = "PRODUCT STOCK SEARCH SCREEN";
            public const string PRODUCT_INQUIRY_POPUP = "PRODUCT INQUIRY POPUP";
            public const string STRAIGHTENING_ENTRY = "STRAIGHTENING ENTRY";
            public const string STRAIGHTENING_EDIT_DELETE = "STRAIGHTENING EDIT DELETE";
            public const string EMAIL_SCREEN = "EMAIL SCREEN";

            public const string PMS_REPORT_PRINT_SCREEN = "PMS REPORT PRINT SCREEN";
            public const string HIKINUKI_REPORT = "HIKINUKI REPORT";
            public const string STRAIGHTENING_REPORT = "STRAIGHTENING REPORT";

            public const string PROCESS_STATUS_SEARCH_FOR_SALES1 = "PROCESS STATUS SEARCH FOR SALES 1";
            public const string HOLD_KAKOUIRAI_REPORT = " HOLD KAKOUIRAI REPORT";
            public const string DELIVERY_CONFIRMATION_REPORT = "DELIVERY CONFIRMATION REPORT";
            public const string MATERIAL_DEPARTMENT_KAKOUIRAI_REPORT = "MATERIAL DEPARTMENT KAKOUIRAI REPORT";
            public const string KAKOUIRAI_REPORT = " KAKOUIRAI REPORT";
            public const string PROCESS_STATUS_SEARCH_FOR_FACTORY = "PROCESS STATUS SEARCH FOR FACTORY";
            public const string KOUBAN_ENTRY = "KOUBAN ENTRY";
            public const string KHAKORAI_COMPLETION = "KHAKORAI COMPLETION";
            public const string MONTH_END_SHUKKA_BATCH_SCREEN = "MONTH END SHUKKA BATCH SCREEN";
            public const string MONTHLY_SHUKKA_STOCK_REPORT = "MONTHLY SHUKKA STOCK LIST REPORT";
            public const string KUROKUWA_NYUKA_PLAN_REPORT = "KUROKUWA NYUKA PLAN REPORT";
            public const string SPOT_PO_LIST = "SPOT PO LIST";
            public const string NIFUDA_REPRINT_CANCELATION = "NIFUDA REPRINT CANCELATION";
            public const string SPOT_PO_POPUP = "SPOT PO POPUP";
            public const string SPOT_NYUKA_ENTRY_EDIT_DELETE = "SPOT NYUKA ENTRY EDIT DELETE";
            public const string SPOT_PRODUCT_NIFUDA_GENERATION_SCREEN = "SPOT PRODUCT NIFUDA GENERATION SCREEN";
            public const string KUROKAWA_PRODUCT_NIFUDA_GENERATION_SCREEN = "KUROKAWA PRODUCT NIFUDA GENERATION SCREEN";
            public const string SPOT_PRODUCT_STOCK_TAKING_BATCH_ENTRY_SCREEN = "SPOT PRODUCT STOCK TAKING BATCH ENTRY SCREEN";
            public const string SPOT_PRODUCT_NIFUDA_GENERATION_REPORT = "SPOT PRODUCT NIFUDA GENERATION REPORT";
            public const string SPOT_PRODUCT_STOCK_SCREEN = "SPOT PRODUCT STOCK SCREEN";
            public const string SPOT_NYUKA_LIST = "SPOT NYUKA LIST PAGE";
            public const string SPOT_PRODUCT_STOCK_TAKING_BATCH_REPRINT_SCREEN = "SPOT PRODUCT STOCK TAKING BATCH REPRINT SCREEN";
            public const string NIFUDA_VERIFIED_PRODUCT_STOCK_LIST_REPORT = "NIFUDA VERIFIED PRODUCT STOCK LIST REPORT";
            public const string KUROKAWA_NYUKA_ENTRY_SCREEN = "KUROKAWA NYUKA ENTRY SCREEN";
            public const string KUROKAWA_NYUKA_LIST = "KUROKAWA NYUKA LIST PAGE";
            public const string PRODUCT_NYUKO_SCREEN = "PRODUCT SHUUKO SCREEN";
            public const string PROCESS_PLAN_REPROCESSING2 = "PROCESS PLAN REPROCESSING2";
            public const string SALE_RECEIVABLES_PRINT_SCREEN = "SALES RECEIVABLES PRINT SCREEN";
            public const string HT_NIFUDA_DIVISION = "HT NIFUDA DIVISION";
            public const string PAYMENTS_RECEIVED_ENTRY_SCREEN = "PAYMENTS RECEIVED ENTRY SCREEN";
            public const string PAYMENTS_RECEIVED_LIST = "PAYMENTS RECEIVED LIST";
            public const string SPECIAL_PRODUCT_TAG_GENRATION = "SPECIAL PRODUCT TAG GENRATION";
            public const string UN_CONFINED_RATE_REPORT_PRINT_SCREEN = "UN_CONFINED_RATE_REPORT_PRINT_SCREEN";
            public const string SALES_INVOICE_ENTRY_SCREEN = "SALES INVOICE ENTRY SCREEN";
            public const string SALES_INVOICE_SEARCH_SCREEN = "SALES INVOICE SEARCH SCREEN";
            public const string SALES_INVOICE_ENTRY_LIST = "SALES INVOICE ENTRY LIST";
            public const string SALES_INVOICE_AMOUNT_RATE_CONFIRMATION_SCREEN = "SALES INVOICE RATE AMOUNT CONFIRMATION SCREEN";
            public const string CURRENT_PREVIOUS_CONSUMPTION_TAX_GENERATION = "CURRENT PREVIOUSCONSUMPTION TAX GENERATION";
            public const string FACTORY_SHUKKAA = "FACTORY SHUKKAA";
            public const string DELIVERY_ACKNOWLEDGEMENT_INVOICE = "DELIVERY ACKNOWLEDGEMENT INVOICE";
            public const string FINAL_INVOICE_REPORT_PRINT_SCREEN = "FINAL_INVOICE_REPORT_PRINT_SCREEN";
            public const string REQUEST_MATERIAL_INATKE_LIST = "REQUEST_MATERIAL_INATKE_LIST_SCREEN";
            public const string PRE_INVOICE_DATA_PRINT_SCREEN = "PRE INVOICE DATA PRINT SCREEN";
            public const string INVOICE_BILL_AMOUNT_CORRECTION = "INVOICE BILL AMOUNT CORRECTION";
            public const string SALES_RECEIPT_REPORT = "SALES RECEIPT REPORT";
            public const string FACTORY_INVOICE_ENTRY_SCREEN = "FACTORY INVOICE ENTRY SCREEN";
            public const string SPOT_PRODUCT_STOCK_SEARCH_SCREEN = "SPOT PRODUCT STOCK SEARCH SCREEN";
            public const string CHAMFER_ENTRY = "CHAMFER ENTRY";
            public const string CHAMFER_EDIT_DELETE = "CHAMFER EDIT DELETE";
            public const string TENSILE_ENTRY = "TENSILE ENTRY";
            public const string TENSILE_EDIT_DELETE = "TENSILE EDIT DELETE";
            public const string HARDNESS_ENTRY = "HARDNESS ENTRY";
            public const string HARDNESS_EDIT_DELETE = "HARDNESS EDIT DELETE";
            public const string TENSOU_ENTRY_EDIT_DELETE = "TENSOU ENTRY EDIT DELETE";
            public const string TENSOU_LIST = "TENSOU LIST";
            public const string HEAT_TREATMENT_ENTRY = " HEAT TREATMENT ENTRY";
            public const string HEAT_TREATMENT_EDIT_DELETE = " HEAT TREATMENT EDIT DELETE";
            public const string HEAT_TREATMENT_LIST = "HEAT TREATMENT LIST";
            public const string EC_ENTRY = "EC ENTRY";
            public const string EC_EDIT_DELETE = "EC EDIT DELETE";
            public const string POST_HT_INSP_ENTRY_EDIT_DELETE = "POST HT INSPECTION ENTRY EDIT DELETE";
            public const string POST_HT_INSP_LIST = "POST HT INSPECTION LIST";
            public const string PROCESS_STATUS_FOR_FACTORY = "PROCESS STATUS FOR FACTORY";
        }

        public class DateFormats
        {
            public const string YYYYMMDD = "yyyy/MM/dd";
            public const string ZERO_YYYYMMDD = "{0:yyyy/MM/dd}";
            public const string ZERO_MMDDYYYY = "{0:MM/dd/yyyy}";
            public const string YYYYMMDD_HHMM = "yyyy/MM/dd HH:mm";
            public const string YYYYMMDD_HHMMSS = "yyyy/MM/dd hh:mm:ss";
            public const string YYYYMMDD_HH24MMSS = "yyyy/MM/dd HH:mm:ss";
            public const string YYYYMMDD_HH24MISS = "yyyy/MM/dd HH24:MI:SS";
            public const string MMDDYYYY = "MM/dd/yyyy";
            public const string MMDDYYYY_HHMM = "MM/dd/yyyy HH:mm";
            public const string DDMMYYYY = "dd/MM/yyyy";
            public const string DDMMYYYY_HHMM = "dd/MM/yyyy HH:mm";
            public const string DD_MMM_YY_HH_MM_SS = "dd-MMM-yy_hh_mm_ss";
            public const string YYYY_MM_DD_HH_MM_SS = "yyyyMMdd_HHmmss";
            public const string YYYY_MM_DD_HH_MM_SS_FFF = "yyyy_MM_dd_HH_mm_ss_fff";
            public const string YYYYMMDD_HHMMSS_FFF = "yyyyMMdd_HHmmss_fff";
            public const string DD_MMM_YY = "dd-MMM-yy";
            public const string YYMMDD = "yy/MM/dd";

        }
        public class FileType
        {
            public const string TEXT = ".txt";
        }
        public class NullValue
        {
            public static int? INT = null;
            public static long? LONG = null;
            public static short? SHORT = null;
            public static decimal? DECIMAL = null;
            public static DateTime? DATE = null;
            public static double? DOUBLE = null;


        }

        public class Symbols
        {
            public const string CHAMFER_MINUS = "C-";
            public const string DAY_OF_DATE = "日";
        }

        public class DropdownField
        {
            public const string DataValueField = "CD";
            public const string DataTextField = "Name";
            public const string DefaultValue = "0";
            public const string Select = "";
        }

        public class DropDownSearchOptions
        {
            public const int ALL = 0;
            public const int NULL = -1;
        }

        public class ConfigurationKeys
        {
            public const string DEFAULT_EMAIL_ID = "DefaultEmailId";
            public const string DATE_FORMAT = "DateFormat";
            public const string EMAIL_SUBJECT = "EmailSubject";
            public const string ENVIRONMENT = "Environment";
            public const string LOGIN_PAGE = "LoginPage";
            public const string MENU_PAGE = "MenuPage";
            public const string EMAIL_FROM = "LogFileSender";
            public const string JAPAN_ADDRESS = "JAPAN_ADDRESS";                            // Invoice and sales Invoice report Address    ::   [2018-08-22 Pradeep M.]
            public const string JAPAN_ADDRESS_CONTACT_NO = "JAPAN_ADDRESS_CONTACT_NO";
            public const string JAPAN_ADDRESS_DEPARTMENT = "JAPAN_ADDRESS_DEPARTMENT";
            public const string PHILIPPINES_ADDRESS = "PHILIPPINES_ADDRESS";
            public const string PO_REPORT_JAPAN_ADDRESS = "JAPAN_ADDRESS_PO";               // HikitoriIrai,PO and Spot PO Report japan address    : [2018-04-10 Pradeep]
            public const string PO_REPORT_PHILIPPINES_ADDRESS = "PHILIPPINES_ADDRESS_PO";   // HikitoriIrai,PO and Spot PO Report philippines address : [2018-04-10 Pradeep]
            public const string IS_COMMA_IN_INVOICE_AMOUNT = "IsCommaInInvoiceAmount";
            public const string IS_NIFUDA_FULL_NAME = "NIFUDA_FULL_NAME";
            public const string MAX_NYUKA_NEGATIVE_VALUE = "MAX_NYUKA_NEGATIVE_VALUE";
            public const string MAX_PRODUCT_NYUKO_NEGATIVE_VALUE = "MAX_PRODUCT_NYUKA_NEGATIVE_VALUE";
            // public const string MAX_PRODUCT_NYUKO_NEGATIVE_VALUE = "MAX_PRODUCT_NYUKO_NEGATIVE_VALUE";
            public const string MAX_PRODUCT_SHUKKO_NEGATIVE_VALUE = "MAX_PRODUCT_SHUKKO_NEGATIVE_VALUE";

            public const string SHUKKO_MAX_NEGATIVE_WEIGHT = "SHUKKO_MAX_NEGATIVE_WEIGHT";
            public const string MAX_ACT_ALLOC_NEGATIVE_VALUE = "MAX_ACT_ALLOC_NEGATIVE_VALUE";
            public const string MAX_NYUKO_NEGATIVE_VALUE = "MAX_NYUKO_NEGATIVE_VALUE";
            public const string NIFUDA_COMPLETE_QUANTITY_DIFFERENCE = "NIFUDA_COMPLETE_QUANTITY_DIFFERENCE";
            public const string PRODUCT_STOCK_SCREEN_WEIGHT = "PRODUCT_STOCK_SCREEN_WEIGHT";
            public const string STOCK_LIST_SCREEN_MIN_WEIGHT = "REPROCESSING_MATL_LIST_SHOW_MIN_WEIGHT";
            public const string SHUKKA_MAX_NEGATIVE_WEIGHT = "SHUKKA_MAX_NEGATIVE_WEIGHT";
            public const string KOUBAN_REM_STOCK_QTY = "KOUBAN_REM_STOCK_QTY";
            public const string PHILIPPINES_ADDRESS_DELIVERY_NOTE = "PHILIPPINES_ADDRESS_DELIVERY_NOTE";
            public const string JAPAN_ADDRESS_DELIVERY_NOTE = "JAPAN_ADDRESS_DELIVERY_NOTE";
            public const string DEFAULT_TABA_COMPLETION_WEIGHT = "DEFAULT_TABA_COMPLETION_WEIGHT";
            public const string KAKOUIRAI_COMPLETION_WEIGHT = "DEFAULT_KAKOUIRAI_COMPLETION_WEIGHT";
        }


        public class CommonGridColumnKeys
        {
            public const string SELECT = "SELECT";
            public const string CODE = "CODE";
            public const string YOBIGANA = "YOBIGANA";
            public const string YOBINA = "YOBINA";
            public const string CREATED_DATE = "CREATEDDATE";
            public const string UPDATED_DATE = "UPDATEDDATE";
        }

        public class PopUpKubun
        {
            public const string MATERIAL = "MATERIAL";
            public const string KOUBAN = "KOUBAN";
            public const string PO = "PO";
            public const string NYUKO = "NYUKO";
            public const string NYUKA = "NYUKA";
            public const string PO_LIST = "PO_LIST";
            public const string MAKER = "MAKER_LIST";
            public const string MOCHIKOMISAKI = "MKSAKI_NAME";
            public const string MOCHI_LOCATION = "1";
            public const string MOCHI_CUSTOMER = "2";
            public const string MOCHI_SUB_CONTRACTOR = "3";
            public const string SUPPLIER = "SUPPLIER_LIST";
            public const string MACHINE = "MACHINE_LIST";
            public const string PROCESS = "PROCESS_LIST";
            public const string CUSTOMER_NAME = "CUSTOMER_NAME";
            public const string LOCATION = "LOCATION_LIST";
            public const string PROCESS_WITH_PROCESS_KUBUN = "PROCESS_LIST_WITH_PROCESS_KUBUN";
            //where column list
            public const string KOUBAN_CD = "lblkoubanCd";
            public const string MATERIAL_CD = "lblMATERIAL_CD";
            public const string KOUBAN_LIST = "KOUBAN_LIST";
            public const string FUTOU_NAME = "FUTOU_NAME";

            public const string General = "10";     //Shukka Ins Kuban //ShukkaTensouInstructionCardScreen.aspx screen use for bind dropdown  ******** public enum SHUKKA_KUBUN_EN ****** Value bind Same as this in same Constant Class Of General 
            public const string Substitute = "20"; //Shukka Ins Kuban //ShukkaTensouInstructionCardScreen.aspx screen use for bind dropdown  ******** public enum SHUKKA_KUBUN_EN ****** Value bind Same as this in same Constant Class Substitute 





        }

        public class HTFlow
        {
            public const string FACTORYSHUKKA = "FACTORYSHUKKA";
            public const string BUNKATSU = "BUNKATSU";
            public const string FACTORYTENSO = "FACTORYTENSO";
            public const string ACCEPTANCE = "ACCEPTANCE";
            public const string SALESSHUKKA = "SALESSHUKKA";
            public const string SALESTENSO = "SALESTENSO";
            public const string STOCKTAKING = "STOCKTAKING";
            public const string STOCKUPDATE = "STOCKUPDATE";
            public const string SHIREHIN = "SHIREHIN";
            public const string FINALINSPECTION = "FINALINSPECTION";
            public const string PRODUCT_SHUKKO = "PRODUCT_SHUKKO";
            public const string PRODUCT_SHUKKO_EDIT = "PRODUCT_SHUKKO_EDIT";
            public const string PRODUCT_SHUKKO_DELETE = "PRODUCT_SHUKKO_DELETE";
        }
        public class ComponentType
        {
            public const string CHUUBAN_GYOUBAN = "CHUUBAN_GYOUBAN";
            public const string CHUUBAN = "CHUUBAN";
            public const string NIFUDA = "NIFUDA";
        }

        public class NifudaType
        {
            public const short PRODUCT = 1;//"PRODUCT";
            public const short SPOT = 2;//"SPOT";
            public const short KUROKAWA = 2;//"KUROKAWA";
            public const short GENERAL = 4;//"GENERAL";//Blank Nifuda
            public const short SHUKKA = 5;//"SHUKKA";
            public const string TENSOU = "TENSOU";

            public const string DAIYOU = "DAIYOU";
            public const string SHIREHIN = "SHIREHIN";//Spot Product



            public const short SPLIT = 4;
            public const short REPLACEMENT = 4;

        }

        public class NoOfRecordsDisplayOnReport
        {
            public const int FINALINSPECTION_RECORD_NO = 12;
            public const int SALES_INVOCE_RECORD_NO = 6;
            public const int HIKINUKI_REPORT_RECORD_NO = 10;
            public const int STRAIGHTNING_REPORT_RECORD_NO = 10;
            public const int MATERIAL_DEPARTMENT_KAKOUIRAI_REPORT_RECORD_NO = 7;
            public const int HOLD_KAKOUIRAI_REPORT_RECORD_NO = 6;
            public const int FACTORY_PRODUCTION_PLAN_RECORD_NO = 31;     //30  Report UI Changes so extra row is added 2019-10-03 Pradeep M.
            public const int SPOT_NYUKA_PLAN_RECORD_NO = 15;            //13  Report UI Changes so extra row is added 2019-11-08 Pradeep M.
            public const int HIKITORI_IRAI_RECORD_NO = 10;
            public const int UNCONFIRM_RATE_RECORD_NO = 15;
            public const int FINAL_INVOICE_RECORD_NO = 5;
            public const int SALES_RECEIVABLES_RECORD_NO = 15;
            public const int PRE_INVOICE_DATA_RECORD_NO = 10;
            public const int WIP_TENSOU_REQUEST_RECORD_NO = 24;
            public const int WIP_TENSOU_ACCEPTANCE_RECORD_NO = 24;
        }

        public class NoOfRecordsPPProdPoint
        {
            public const int PRODPOINT_RECORD_COUNT = 13;
        }
        public class Culture
        {
            public const string JP = "Japanese";
            public const string JPN = "日本語";
            public const string EN = "English";
            public const string jaJP = "ja-JP";
            public const string enUS = "en-US";
        }
        public class HTMasterPageButtonID
        {
            public const string BTN_SUBMIT = "btnSubmit";

            public const string TXT_CUSTOMER = "txtCustomer";
        }

        public class LocationKubun
        {
            public const int HeadQuarter = 1;
            public const int BranchOffice = 2;
            public const int SalesOffice = 3;
            public const int Factory = 4;
        }

        public class SearchBy
        {
            public const string STANDARD = "1";
            public const string SALAESINCHARGE = "2";
            public const string CUSTOMER = "3";
        }

        public class RelationType
        {
            public const int ProdTagToShippingTag = 0;
            public const int ProdTagToProdTag = 1;
            public const int ShippingTagToShippingTag = 2;
        }

        public static Dictionary<string, int> NG_REASON = new Dictionary<string, int>()
       {
           {" ",0},
           {"カッター刃傷", 1},
           {"ダイス傷", 2},
           {"ローラー傷", 3},
           {"機械トラブル", 4},
           {"曲がり", 5},
           {"錠傷", 6},
           {"寸法（高）", 7},
           {"寸法（低）", 8},
           {"長さ（長）", 9},
           {"長さ（短）", 10},
       };
        public static Dictionary<string, int> BEND_STATUS = new Dictionary<string, int>()
       {
          {"良",1},
          {"良２",2},
          {"良3",3},
          {"保留",4},
          {":矯正",5},
          {"多ロール",6},
          {"その他",7},
       };

        public static Dictionary<string, int> EXTERNALSURFACECONDITION = new Dictionary<string, int>()
       {
           {"良",1},
           {"保留",2},
           {"らせん矯正",3},
           {"肌矯正",4},
           {"寸法矯正",5},
           {"ダイス傷",6},
           {":錠傷",7},
           {"カッター刃傷",8},
           {"ローラー傷",9},
           {"その他",10},
       };
        public static Dictionary<string, int> TPTEST_JP = new Dictionary<string, int>()
        {
           {"無し",0},
           {"150",1},
           {"450",2},
        };

        public static Dictionary<string, int> TPTEST_EN = new Dictionary<string, int>()
        {
           {"NONE",0},
           {"150",1},
           {"450",2},
        };

        public static Dictionary<string, int> COMPONENT_TYPE_VALID_LENGTH = new Dictionary<string, int>()
       {
           {"CHUUBAN",9},
           {"NIFUDA",9},
           {"CHUUBAN_GYOUBAN",11},
       };

        public static Dictionary<String, int> CURRENCY_FOR_DECODING = new Dictionary<string, int>
        {
             {"Lek",1},
          // {"؋",2}, // Commented because of encoding problem
            {"$",3},
            {"ƒ",4},
          //  {"₼",5},   // Commented because of encoding problem
            {"p.",6},
            {"BZ$",7},
            {"$b", 8},
            {"KM",9},
            {"P",10},
            {"лв" ,11},
            {"R$",12},
           // {"៛",13},     // Commented because of encoding problem
           // {"¥",14},
            { "¥",14},   //Unicode Number For Japan yen  &#165
            {"₡",15},
            {"kn",16},
            { "₱",17},     //Unicode Number For Philippines peso  &#8369
           //{"₱",17},
            {"Kč",18},
            {"kr" ,19},
            {"RD$",20},
            {"£",21},
            {"€",22},
            //{"₾",23},
            {"¢",24},
            {"Q" , 25},
            {"L",26},
            {"Ft",27},
            {"₹",28},
            {"Rp",29},
          //  {"﷼",30},  // Commented because of encoding problem
            {"₪" ,31},
            {"J$",32},
            {"₩",33},
            {"₭",34},
            {"Ls",35},
            {"Lt",36},
            {"ден",37},
            {"RM",38},
            {"₨",39},
            {"₮",40},
            {"MT",41},
            {"C$",42},
            {"₦",43},
            {"B/.",44},
            {"Gs",45},
            {"S/.",46},
            {"zł",47},
            {"lei",48},
            //{"₽",49},
            {"Дин.",50},
            {"S" ,51},
            {"CHF",52},
            {"NT$",53},
            {"฿",54},
            {"TT$" ,55},
            //{"₺",56},   // Commented because of encoding problem
            {"₴",57},
            {"$U" ,58},
            {"Bs" ,59},
            {"₫",60},
            {"Z$" ,61},
        };

        public static Dictionary<string, int> CURRENCY = new Dictionary<string, int>()     //For DropDown
        {

            {"Lek",1},
          //  {"؋",2}, // Commented because of encoding problem
            {"$",3},
            {"ƒ",4},
          //  {"₼",5},   // Commented because of encoding problem
            {"p.",6},
            {"BZ$",7},
            {"$b", 8},
            {"KM",9},
            {"P",10},
            {"лв" ,11},
            {"R$",12},
           // {"៛",13},    // Commented because of encoding problem
            {"¥",14},
            {"₡",15},
            {"kn",16},
            {"₱",17},
            {"Kč",18},
            {"kr" ,19},
            {"RD$",20},
            {"£",21},
            {"€",22},
            //{"₾",23},
            {"¢",24},
            {"Q" , 25},
            {"L",26},
            {"Ft",27},
            {"₹",28},
            {"Rp",29},
          //  {"﷼",30},  // Commented because of encoding problem
            {"₪" ,31},
            {"J$",32},
            {"₩",33},
            {"₭",34},
            {"Ls",35},
            {"Lt",36},
            {"ден",37},
            {"RM",38},
            {"₨",39},
            {"₮",40},
            {"MT",41},
            {"C$",42},
            {"₦",43},
            {"B/.",44},
            {"Gs",45},
            {"S/.",46},
            {"zł",47},
            {"lei",48},
            //{"₽",49},
            {"Дин.",50},
            {"S" ,51},
            {"CHF",52},
            {"NT$",53},
            {"฿",54},
            {"TT$" ,55},
            //{"₺",56},   // Commented because of encoding problem
            {"₴",57},
            {"$U" ,58},
            {"Bs" ,59},
            {"₫",60},
            {"Z$" ,61},
        };
        public static Dictionary<string, int> PRODUCT_KUBUN_JP = new Dictionary<string, int>()
        {
            {"COIL→BAR",1 },
            {"BAR→BAR",2 },
            {"COIL→COIL",3 },
            {"直線カット",4 },
            {"その他",5 }
        };
        public static Dictionary<string, int> PRODUCT_KUBUN_EN = new Dictionary<string, int>()
        {
            {"COIL→BAR",1 },
            {"BAR→BAR",2 },
            {"COIL→COIL",3 },
            {"STRGT CUT",4 },
            {"OTHER",5 }
        };
        public static Dictionary<string, int> REPROCESS_KUBUN_JP = new Dictionary<string, int>()
        {
            //{"再加工（在庫）",1 },
             {"通常",10 },// ADDED BY Kuldeep on 2019/08/13, REfered Kubun Document 
              {"再加工",20 },
            {"追加工程",30 },
            //{"再加工（返す）",2 }, //comment bcz refer review comment sheet by sunhara (visit dec 2017).
            //Remove on suggestion of Sunahara San on Agust 2016 Visit
            //  追加工程_仕掛 = 2,//追加工程（仕掛） 
            //{"追加工程（在庫）",3 },
            
           // {"追加工程（（返す）",4 },  //comment bcz refer review comment sheet by sunhara (visit dec 2017).
            {"切断",40 },
            //{"スポット",6 },
        };
        public static Dictionary<string, int> REPROCESS_KUBUN_EN = new Dictionary<string, int>()
        {
          //  {"Reprocess (Stock)",1 },
          {"General",10 },// ADDED BY Kuldeep on 2019/08/13, REfered Kubun Document 
           {"Reprocess (Return)",20 },
           {"AddedProcess ",30},
           
            //Remove on suggestion of Sunahara San on Agust 2016 Visit
            //AddedProcess_Live  = 2,//(Live)
            //{"AddedProcess (Stock)",3 },
         //  {"AddedProcess (Return)",4 },
         
            {"Cutting",40 },
          //  {"Spot",6 },
        };
    }



    public static class Enums
    {

        public enum ShukkoKubunJP
        {
            出庫 = 1,
            棚卸 = 2,
            その他 = 3
        }

        public enum ShukkoKubunEN
        {
            GEN = 1,
            PROCESS = 2,
            OTHER = 3

        }

        public enum InvoiceKubunJP
        {
            通常 = 1,
            来勘 = 2
        }

        public enum InvoiceKubunEN
        {
            NORMAL = 1,
            DNT_KNW = 2

        }
        public enum LocationKubunJP
        {
            本社 = 1,
            支店 = 2,
            営業所 = 3,
            工場 = 4,
        }

        public enum LocationKubunEN
        {
            HeadQuarter = 1,
            BranchOffice = 2,
            SalesOffice = 3,
            Factory = 4,
        }

        // elements at 2,6 and 11 positions are chnged

        public enum KOUBAN_ENTRY_KUBUN_JP
        {
            [Description("工番回答（通常）")]
            工番回答_通常 = 1,
            [Description("工番返答（分割）")]
            工番返答_分割 = 2
        }
        public enum KOUBAN_ENTRY_KUBUN_EN
        {
            [Description("NORMAL")]
            ReplyNormal = 1,
            [Description("SPLIT")]
            ReplySplit = 2
        }
        public enum KOUBAN_UPDATE_KUBUN_JP
        {
            [Description("重量・納期変更")]
            重量_納期変更 = 3,
            [Description("持込先変更")]
            持込先変更 = 4,
            [Description("その他変更")]
            その他変更 = 5,
            [Description("工番取消")]
            工番取消 = 6,
            [Description("修正(入力ミス)")]
            修正_入力ミス = 7,
            [Description("削除(入力ミス)")]
            削除_入力ミス = 8
        }

        public enum KOUBAN_UPDATE_KUBUN_EN
        {
            [Description("WT DELV CHANGE")]
            WtDlyTimeChange = 3,
            [Description("DELV.PLACE CHANGE")]
            MochiChange = 4,
            [Description("OTHER CHANGES")]
            OtherChanges = 5,
            [Description("CANCLE")]
            KoubanCancel = 6,
            [Description("INPUT MISTAKE")]
            EditIPMiss = 7,
            [Description("DELETE")]
            DeleteIPMiss = 8
        }
        public enum KOUBAN_RPLY_KUBUN_JP
        {
            [Description("工番回答(通常）")]
            工番回答_通常 = 1,
            [Description("工番返答（分割）")]
            工番返答_分割 = 2,
            [Description("重量・納期変更")]
            重量_納期変更 = 3,
            [Description("持込先変更")]
            持込先変更 = 4,
            [Description("その他変更")]
            その他変更 = 5,
            [Description("工番取消")]
            工番取消 = 6,
            [Description("修正(入力ミス)")]
            修正_入力ミス = 7,
            [Description("削除(入力ミス)")]
            削除_入力ミス = 8
        }

        public enum KOUBAN_RPLY_KUBUN_EN
        {
            [Description("NORMAL")]
            ReplyNormal = 1,
            [Description("SPLIT")]
            ReplySplit = 2,
            [Description("WT DELV CHANGE")]
            WtDlyTimeChange = 3,
            [Description("DELV.PLACE CHANGE")]
            MochiChange = 4,
            [Description("OTHER CHANGES")]
            OtherChanges = 5,
            [Description("CANCLE")]
            KoubanCancel = 6,
            [Description("INPUT MISTAKE")]
            EditIPMiss = 7,
            [Description("DELETE")]
            DeleteIPMiss = 8
        }
        public enum DepartmentGroupKubunJP
        {
            販売営業 = 1,
            販売管理事務 = 2,
            資材 = 3,
            経理 = 4,
            システム = 5,
            [Description("倉庫(本社・営業所)")]
            倉庫本社_営業所 = 6,
            工場管理 = 7,
            工場事務 = 8,
            工場技術 = 9,
            工場品質保証 = 10,
            工場生産作業者 = 11,
            工場倉庫 = 12,
            その他 = 13
        }

        public enum DepartmentGroupKubunEN
        {
            Sales = 1,
            Sales_Mngmnt = 2,
            Material = 3,
            Accounting = 4,
            System = 5,
            SOHO = 6,
            Factory = 7,
            Factory_ofc = 8,
            Plant_Tech = 9,
            Quality = 10,
            Production = 11,
            Warehouse = 12,
            Other = 13
        }

        public enum ProductKubun
        {
            [Description("COIL→BAR")]
            CoilBar = 1,
            [Description("BAR→BAR")]
            BarBar = 2,
            [Description("COIL→COIL")]
            CoilCoil = 3,
            [Description("直線カット")]
            直線カット = 4,
            [Description("その他")]
            その他 = 5,
        }

        public enum ProductKubunEn
        {
            [Description("COIL→BAR")]
            CoilBar = 1,
            [Description("BAR→BAR")]
            BarBar = 2,
            [Description("COIL→COIL")]
            CoilCoil = 3,
            [Description("STRGT_CUT")]
            STRGT_CUT = 4,
            [Description("OTHER")]
            OTHER = 5,
        }
        public enum ProdSpecKubun
        {
            顧客スペック = 1,
            一般材 = 2,
        }
        public enum ProdSpecKubunEn
        {
            CustomerSpecific = 1,
            General = 2,
        }
        public enum MATERIAL_SPEC_KUBUN_JP
        {
            汎用 = 1,
            特別 = 2,
        }
        public enum MATERIAL_SPEC_KUBUN_EN
        {
            General = 1,
            Special = 2,
        }
        public enum MATERIAL_TYPE_JP
        {
            自社材 = 1,//引取素材 replace with 自社材 Redmine #2159
            有償支給 = 2,
            無償支給 = 3,
            // 購入品 = 4,                        // M_ProductMaster material kubun remove last 2 kubun.   Change by  Sunahara San =>    Visit_Dec2019 
            // 購入素材 = 5,
        }
        public enum MATERIAL_TYPE_EN
        {

            [Description("Self Received")]
            SelfReceived = 1,
            [Description("Received with Payment")]
            ReceivedWithPayment = 2,
            [Description("Received Free")]
            ReceivedFree = 3,
            /*  [Description("Purchased Product")]
              PurchasedProduct = 4,
              [Description("Raw Material")]
              RawMaterial = 5,*/
        }


        public enum Division_Type_JP
        {
            通常 = 10,
            素材転用 = 20,
            棚卸 = 80,
            その他 = 99,
        }
        public enum Division_Type_EN
        {
            General = 10,
            UseAsRawMaterial = 20,
            StockTaking = 80,
            Other = 99,
        }
        public enum MATERIAL_TYPE_KUBUN_JP
        {
            COIL = 1,
            BAR = 2,
            黒皮 = 3,
        }
        public enum MATERIAL_TYPE_KUBUN_EN
        {
            COIL = 1,
            BAR = 2,
            KUROKAWA = 3,
        }

        public enum PO_KUBUN_JP
        {
            [Description("追加発注")]
            PoAddition = 1,

            [Description("発注変更")]      //（工番無し）  PO Report Show 4 kanji [2019-11-1 PradeeP M.]
            PoChangeKoubanAbsent = 2,

            [Description("発注取消")]       //（工番無し）
            PoDeleteKoubanAbsent = 3,

            [Description("発注変更")]       //（工番有り）
            PoChangeKoubanPresent = 4,

            [Description("発注取消")]       //（工番有り）
            PoDeleteKoubanPresent = 5,
        }
        public enum PO_KUBUN_EN
        {
            [Description("PO New")]
            PoAddition = 1,

            [Description("PO Change")]      // (Kouban Not Registered)
            PoChangeKoubanAbsent = 2,

            [Description("PO Delete")]      // (Kouban Not Registered)
            PoDeleteKoubanAbsent = 3,

            [Description("PO Change")]      // (Kouban Registered)
            PoChangeKoubanPresent = 4,

            [Description("PO Delete")]      // (Kouban Registered)
            PoDeleteKoubanPresent = 5,


        }
        /*  public enum  MatlTypeShort_EN// Same kubun for English and Japanese
          {
              C = 1,
              B = 2,
              P = 3,
              BN = 4,
          }
          public enum MatlTypeShort_JP// Same kubun for English and Japanese
          {
              C = 1,
              B = 2,
              P = 3,
              束 = 4,
          }

      */
        public enum KeyTypes
        {
            String = 1,
            Integer = 2,
            Decimal = 3,
        }

        public enum PRODUCT_UNIT_KUBUN_JP
        {
            本 = 1,
            束 = 2,
            個 = 3,
            Ｃ = 4//コイル = 4
        }
        public enum PRODUCT_UNIT_KUBUN_EN
        {
            B = 1,
            BN = 2,
            P = 3,
            C = 4
        }

        public enum HON_THABA_KUBUN_JP
        {
            本 = 1,
            束 = 2,
        }
        public enum HON_THABA_KUBUN_EN
        {
            B = 1,
            BN = 2,
        }

        public enum NYUKA_SHUKKA_TYPE_KUBUN_JP
        {
            [Description("出庫")]
            出庫 = 1,

            [Description("出荷")]
            出荷 = 2,

            [Description("入庫")]
            入庫 = 3,

            [Description("入荷")]
            入荷 = 4,

            [Description("出庫（外注）")]
            出庫_外注 = 5,

            [Description("出庫（切断）")]
            出庫_切断 = 6,

            [Description("出庫（棚卸）")]
            出庫_棚卸 = 7,

            [Description("出庫（その他）")]
            出庫_その他 = 8,
        }
        public enum NYUKA_SHUKKA_TYPE_KUBUN_EN
        {
            [Description("Scrap")]
            Scrap = 1,

            [Description("Shipment")]
            Shipment = 2,

            [Description("Nyuko Intake")]
            NyukoIntake = 3,

            [Description("Nyuka Arrival Stock")]
            NyukaArrivalStock = 4,

            [Description("Scrap (Supplier)")]
            ScrapSupplier = 5,

            [Description("Scrap (Cutting)")]
            ScrapCutting = 6,

            [Description("Scrap (Tanaoroshii)")]
            ScrapTanaoroshii = 7,

            [Description("Scrap (Other)")]
            ScrapOther = 8,

        }

        public enum BEND_STATUS_KUBUN_JP
        {
            良 = 1,
            良2 = 2,
            良３ = 3,
            保留 = 4,
            [Description(":矯正")]
            矯正 = 5,
            多ロール = 6,
            その他 = 7,
        }
        public enum BEND_STATUS_KUBUN_EN
        {
            OK = 1,
            OK2 = 2,
            OK3 = 3,
            HOLD = 4,
            STRAIGHTENING = 5,
            MULTIROLL = 6,
            OTHER = 7,
        }

        public enum EXT_TEXTURE_STATUS_EN
        {
            OK = 1,
            HOLD = 2,
            [Description("HELIX CORRECTION")]
            HELIXCORRECTION = 3,
            [Description("SURFACE CORRECTION")]
            SKINCORRECTION = 4,
            [Description("DIMENSSION CORRECTION")]
            DIMENSIONALCORRECTION = 5,
            [Description("DICE SCRATCH")]
            DICESCRATCH = 6,
            [Description("TABLET WOUND")]
            TABLETSCRATCH = 7,
            [Description("CUTTER SCRATCH")]
            CUTTERKNIFE = 8,
            [Description("ROLLER SCRATCH")]
            ROLLERSCRATCH = 9,
            OTHER = 10,
        }

        public enum EXT_TEXTURE_STATUS_JP
        {
            良 = 1,
            保留 = 2,
            らせん矯正 = 3,
            肌矯正 = 4,
            寸法矯正 = 5,
            ダイス傷 = 6,

            [Description(":錠傷")]
            錠傷 = 7,

            カッター刃傷 = 8,
            ローラー傷 = 9,
            その他 = 10,
        }



        public enum CoilWindDirectionsJP
        {
            左 = 1,
            右 = 2,
            その他 = 3,
        }
        public enum CoilWindDirectionsEN
        {
            Left = 1,
            Right = 2,
            Other = 3,
        }
        public enum MAGARI_KUBUN_JP
        {
            曲り = 1,
            フレ = 2,
            //端フレ=3,
        }

        public enum MAGARI_KUBUN_EN
        {
            Bend_Hexagon = 1,
            Bend_Round = 2,
            // Bend_Ends = 3,
        }

        public enum PROCESS_DECISION_KUBUN_JP
        {

            OK = 1,
            NG = 2,
            保留 = 3,
            工程追加 = 4,
        }

        public enum PROCESS_DECISION_KUBUN_EN
        {
            OK = 1,
            NG = 2,
            HOLD = 3,
            PROCESS_ADD = 4,
        }

        public enum TP_TEST_EN
        {
            [Description("NONE")]
            NONE = 1,
            [Description("150")]
            TP_TEST_150 = 2,
            [Description("450")]
            TP_TEST_450 = 3,

        }

        public enum TP_TEST_JP
        {
            [Description("無し")]
            無し = 1,
            [Description("150")]
            TP_TEST_150 = 2,
            [Description("450")]
            TP_TEST_450 = 3,

        }





        public enum REPROCESS_KUBUN_JP
        {
            [Description("再加工（在庫）")]
            再加工_在庫 = 1,
            [Description("再加工（返す）")]
            再加工_返す = 2,

            //Remove on suggestion of Sunahara San on Agust 2016 Visit
            //  追加工程_仕掛 = 2,//追加工程（仕掛） 

            [Description("追加工程（在庫）")]
            追加工程_在庫 = 3,
            [Description("追加工程（（返す）")]
            追加工程_返す = 4,
            [Description("切断")]
            切断 = 5,
            [Description("スポット")]
            スポット = 6,

        }

        public enum REPROCESS_KUBUN_EN
        {
            [Description("Reprocess (Stock)")]
            Reprocess_Stock = 1,
            [Description("Reprocess (Return)")]
            Reprocess_Return = 2,
            //Remove on suggestion of Sunahara San on Agust 2016 Visit
            //AddedProcess_Live  = 2,//(Live)
            [Description("AddedProcess (Stock)")]
            AddedProcess_Stock = 3,
            [Description("AddedProcess (Return)")]
            AddedProcess_Return = 4,
            [Description("Cutting")]
            Cutting = 5,
            [Description("Spot")]
            Spot = 6,
        }

        public enum ProductColorId
        {
            ｱｵ = 1,
            ｱｶ = 2,
            ｵﾚﾝｼﾞ = 3,
            ｷｲﾛ = 4,
            ｷﾐﾄﾞﾘ = 5,
            ｸﾘｰﾑ = 6,
            ｸﾞﾘｰﾝ = 7,
            ｸﾞﾚｰ = 8,
            ｸﾛ = 9,
            ｺﾞｰﾙﾄﾞ = 10,
            ｺﾝ = 11,
            ｼﾙﾊﾞｰ = 12,
            ｼﾛ = 13,
            ｽｶｲﾌﾞﾙｰ = 14,
            ﾁｬｲﾛ = 15,
            ﾈｽﾞﾐ = 16,
            ﾊｲｲﾛ = 17,
            ﾊﾀﾞｲﾛ = 18,
            ﾋﾟﾝｸ = 19,
            ﾐｽﾞｲﾛ = 20,
            ﾗｲﾄｸﾞﾘｰﾝ = 21,
            ﾜｶｻｲﾛ = 22,
        }
        public enum SalesProductionColorIdEng
        {
            Blue = 1,
            Red = 2,
            Orange = 3,
            Darkred = 4,
            Green = 7,
           
        }
        public enum ProductColorIdEng
        {
            Blue = 1,
            Red = 2,
            Orange = 3,
            Yellow = 4,
            YellowGreen = 5,
            Cream = 6,
            Green = 7,
            Gray = 8,
            Black = 9,
            Gold = 10,
            Con = 11,
            Silver = 12,
            White = 13,
            SkyBlue = 14,
            Brown = 15,
            DarkGrey = 16,
            LightGrey = 17,
            SkinColor = 18,
            Pink = 19,
            LightBlue = 20,
            LightGreen = 21,
            Metallic = 22,
        }

        public enum PackingOptions
        {
            PPｸﾗﾌﾄ = 1,
            ﾃﾂﾊﾞｺ = 2,
            ｷﾊﾞｺ = 3,
            ﾊﾞﾝｾﾝ = 4,
            ﾇﾉｱﾃﾊﾞﾝｾﾝ = 5,
        }

        public enum PackingOptionsEng
        {
            PPCraft = 1,
            SteelBox = 2,
            WoodenBox = 3,
            SteelWireBinding = 4,
            PolymerWireBinding = 5,
        }

        public enum StrainMaxMinOptions
        {
            [Description("以下(MAX)")]
            以下MAX = 1,
            [Description("以上(MIN)")]
            以上MIN = 2,
        }

        public enum StandardDecarb
        {
            DMF = 1,
            DMS = 2,
            DMT = 3,
        }

        public enum StandardHardness
        {
            HRB = 1,
            HRC = 2,
            HB = 3,
            HV = 4,
            HS = 5,
            HRA = 6,
            HRD = 7,
        }
        public enum STORAGE_KUBUN_ENG
        {
            Ordered = 1,
            ShipAfterProduction = 2,
            OneTimeOrder = 3,
        }
        public enum STORAGE_KUBUN_JP
        {
            計画在庫 = 1,
            製造後即出荷 = 2,
            スポット受注 = 3,
        }

        public enum SHAPE
        {
            R = 1,
            H = 2,
            S = 3,
            K = 4,
        }

        public enum DECISION
        {
            OK = 0,
            NG = 1,
            保留 = 2,
            工程追加 = 3,
        }



        public enum UNITTYPE
        {
            本 = 0,
            束 = 1,
        }

        public enum OKNG
        {
            OK = 0,
            NG = 1,
        }

        public enum NYUKA_KUBUN_ENG
        {
            Self = 1,
            Kurokawa = 2,
            Spot = 3,
        }

        public enum NYUKA_KUBUN_JP
        {
            自社 = 1,
            黒皮 = 2,
            スポット = 3,
        }

        public enum RECEIVING_PLACE_KUBUN_JP
        {
            [Description("埠頭管理会社（通常取入先）")]
            埠頭管理会社_通常取入先 = 1,
            [Description("メーカー")]
            メーカー = 2,
            [Description("商社")]
            商社 = 3,
        }

        public enum RECEIVING_PLACE_KUBUN_EN
        {
            [Description("Shipyard Mgmt Company")]
            ShipyardMgmtCompany = 1,
            [Description("Maker")]
            Maker = 2,
            [Description("Trading Company")]
            TradingCompany = 3,
        }

        public enum PROCESS_TEMPLATE_KUBUN_JP
        {
            [Description("再加工")]
            再加工 = 1,
            [Description("追加工程（仕掛）")]
            追加工程_仕掛 = 2,
            [Description("追加工程（在庫）")]
            追加工程_在庫 = 3,
            [Description("切断")]
            切断 = 4,
            [Description("スポット")]
            スポット = 5,
        }

        public enum PROCESS_TEMPLATE_KUBUN_EN
        {
            [Description("Reprocess")]
            Reprocess = 1,
            [Description("Added Process (Live)")]
            AddedProcess_Live = 2,
            [Description("Added Process (Stock)")]
            AddedProcess_Stock = 3,
            [Description("Cutting")]
            Cutting = 4,
            [Description("Spot")]
            Spot = 5,
        }

        //Added BY Sagar B on 26/7/2017
        public enum NG_REASON_JP
        {



            [Description("カッター刃傷")]
            カッター刃傷 = 1,
            [Description("ダイス傷")]
            ダイス傷 = 2,
            [Description("ローラー傷")]
            ローラー傷 = 3,
            [Description("機械トラブル")]
            機械トラブル = 4,
            [Description("曲がり")]
            曲がり = 5,
            [Description("錠傷")]
            錠傷 = 6,
            [Description("寸法（高）")]
            寸法_高 = 7,
            [Description("寸法（低）")]
            寸法_低 = 8,
            [Description("長さ（長）")]
            長さ_長 = 9,
            [Description("長さ（短）")]
            長さ_短 = 10
        }

        //Added BY Sagar B on 26/7/2017
        public enum NG_REASON_ENG
        {
            [Description("CUTTER KNIFE")]
            OK = 1,
            [Description("DICE SCRATCHES")]
            HOLD = 2,
            [Description("ROLLER SCRATCHES")]
            SPIRAL_CORRECTION = 3,
            [Description("MACHINE TROUBLE")]
            SKIN_CORRECTION = 4,
            [Description("BEND")]
            DIM_CORRECTION = 5,
            [Description("TABLET WOUND")]
            DICE_SCRATCHES = 6,
            [Description("DIM (HIGH)")]
            TABLET_WOUND = 7,
            [Description("DIM (LOW)")]
            CUTTER_KNIFE = 8,
            [Description("LENGTH (LONG) ")]
            ROLLER_SCRATCHES = 9,
            [Description("LENGTH (SHORT)")]
            OTHER = 10
        }


        public enum SHUKKA_KUBUN_JP
        {
            通常 = 10,        //as per new values _by Amol Kengar[2019/10/25] *****plz change value of  public class PopUpKubun General value same as 通常  In same class**********
            代用 = 20         //as per new values _by Amol Kengar[2019/10/25] *****plz change value of  public class PopUpKubun Substitute value same as 代用  In same class**********
            /*
            代替 = 3,
            [Description("代替＋代用")]
            代替_代用 = 4,
            転送指示のみ = 5
            */
        }
        public enum SHUKKA_KUBUN_EN
        {
            General = 10,             //as per new values _by Amol Kengar[2019/10/25] *****plz change value of  public class PopUpKubun General value same as General In same class **********
            Substitute = 20      //Unavailability_Substitute        //as per new values _by Amol Kengar[2019/10/25]   *****plz change value of  public class PopUpKubun Substitute value same as Substitute  In same class**********
            /*
          Returned_Prod_Substitute = 3,
          Returned_Prod_Unavailability_Substitute = 4,
          Transfer_Only = 5
          */
        }
        public enum PROCESS_KUBUN_JP
        {
            酸洗い = 1,
            口付け = 2,
            ショット = 3,
            引抜 = 4,
            矯正 = 5,
            面取 = 6,
            刻印 = 7,
            切断 = 8,
            熱処理 = 9,
            調質 = 10,
            ＥＣ = 11,
            ＵＴ = 12,
            内質検査 = 13,
            機械試験 = 14,
            最終検査 = 15,
            梱包 = 16,
            出荷 = 17,
            その他 = 18,
            メモ = 19,
            製品出庫 = 20,
            フレ = 21,
            油噴霧 = 22,
            結束 = 23,
            棚卸 = 24,
            調質検査 = 999,

        }
        public enum PROCESS_KUBUN_EN
        {
            Pickling = 1,
            口付け = 2,
            ShotBlasting = 3,
            Drawing = 4,
            Straightning = 5,
            Chamfering = 6,
            Stamping = 7,
            Cutting = 8,
            Heat_Treatment = 9,
            Refining = 10,
            EC = 11,
            UT = 12,
            Internal_Quality_Inspection = 13,
            Mechanical_Testing = 14,
            Final_Inspection = 15,
            Packing = 16,
            Shipping = 17,
            Other = 18,
            Memo = 19,
            Product_Scrap = 20,
            Bend = 21,
            Oil_Spray = 22,
            Binding = 23,
            Stock_Verification = 24,
            Hardndess_Tempering = 999,
            Tensile=31,
        }

        /// <summary>
        ///     Created By SatishP[20160707]- For FIReportPrintScreen.
        /// </summary>
        public enum FI_REPORT_SORT_KUBUN_JP
        {
            日報に登録した順番で印刷 = 0,
            [Description("注番＋束番の順番で印刷")]
            注番_束番の順番で印刷 = 1

        }
        public enum FI_REPORT_SORT_KUBUN_EN
        {
            REGISTERED_DAILY = 0,
            CHUUBAN_AND_BUNDLE_NO = 1
        }

        /// <summary>
        ///     Created By GirishB[20160712]- For Shukka Instruction Screen and Report.
        /// </summary>
        public enum SHUKKA_INSTRUCTION_QUANTITY_KUBUN_JP
        {
            前後 = 1,
            以上 = 2,
            以下 = 3
        }
        public enum SHUKKA_INSTRUCTION_QUANTITY_KUBUN_EN
        {
            [Description("More or Less")]
            MoreOrLess = 1,
            [Description("or More")]
            OrMore = 2,
            [Description("or Less")]
            OrLess = 3
        }

        /// <summary>
        ///     Created By GirishB[20160712]- For Shukka Instruction Screen and Report.
        /// </summary>
        public enum STOCK_STATE_KUBUN_JP
        {
            在庫 = 1,
            仕掛 = 2
        }
        public enum STOCK_STATE_KUBUN_EN
        {
            Stock = 1,
            WIP = 2
        }

        /// <summary>
        ///     Created By GirishB[20160712]- For Shukka Instruction Screen, Report, Invoice Screen, Report.
        /// </summary>
        public enum RATE_FORMULA_KUBUN_JP
        {

            [Description("重量✕単価")]
            重量x単価 = 1,
            [Description("員数✕単価")]
            員数x単価 = 2,
            [Description("金額")]
            金額 = 3
            //[Description("員数✖単価")]
            //員数x単価 = 2,
            //[Description("金額")]
            //金額 = 3

        }
        public enum RATE_FORMULA_KUBUN_EN
        {
            [Description("Qty✕Rate")]
            QtyｘRate = 1,
            [Description("Pieces✕Rate")]
            PiecesｘRate = 2,
            [Description("Specified")]
            Specified = 3
        }

        /// <summary>
        ///     Created By GirishB[20160713]- For Shukka Instruction Screen, Report.
        /// </summary>
        public enum PAYABLE_DAYS_KUBUN_JP
        {
            日前まで = 1,
            日前後 = 2,
            日後まで = 3
        }
        public enum PAYABLE_DAYS_KUBUN_EN
        {
            DaysEarly = 1,
            DaysEarlyOrLater = 2,
            DaysLater = 3
        }



        /// <summary>
        ///     Created By GirishB[20160713]- For Shukka Instruction Screen, Report.
        /// </summary>
        public enum SHUKKA_INSTRUCTION_SPECIAL_REQUEST_KUBUN_JP
        {
            有り = 1,
            無し = 2
        }
        public enum SHUKKA_INSTRUCTION_SPECIAL_REQUEST_KUBUN_EN
        {
            Specified = 1,
            NotSpecified = 2
        }

        public enum DeploymentTarget
        {
            Philippines = 1,
            China = 2,
            Japan = 3,
            India = 4
        }

        public enum NYUKO_SHUKKO_SHUKKA_KUBUN_JP
        {
            入庫 = 1,
            出庫 = 2,
            出荷 = 3
        }

        public enum NYUKO_SHUKKO_SHUKKA_KUBUN_EN
        {
            Stocking = 1,
            Scrap = 2,
            Shipment = 3
        }

        public enum SHUKKO_KUBUN_JP
        {
            通常 = 10,
            分割 = 15,
            張替 = 16,
            [Description("製品⇒素材（転用）")]
            素材転用 = 20,
            外注 = 30,
            棚卸 = 80,
            その他 = 99
        }
        public enum SHUKKO_KUBUNS_JP
        {
            通常 = 10,
            分割 = 15,
            張替 = 16,
            [Description("素材転用")]
            製品_素材_転用 = 20,
            外注 = 30,
            棚卸 = 80,
            その他 = 99
        }

        public enum SHUKKO_KUBUN_EN
        {
            Scrap = 10,
            Split = 15,
            Change = 16,
            UseAsMaterial = 20,
            SubContractor = 30,
            StockTaking = 80,
            Other = 99
        }

        public enum NYUKO_KUBUN_JP
        {
            [Description("自在入庫")]
            自在入庫 = 1,
            [Description("素材転用")]
            素材転用 = 2,
        }
        public enum NYUKO_KUBUN_ENG
        {
            [Description("StdNyuka")]
            StdNyuka = 1,
            [Description("MaterialDiversion")]
            MaterialDiversion = 2,
        }

        /// <summary>
        ///     Created By GirishB[20161116]- For Product Master, Process Template Master and Process Plan
        /// </summary>
        public enum PROCESS_COLOR_KUBUN_JP
        {
            [Description("000000")]
            ｸﾛ = 1,
            [Description("808080")]
            ｸﾞﾚｰ = 2,
            [Description("c0c0c0")]
            ｼﾙﾊﾞｰ = 3,
            [Description("800000")]
            ｸﾘ = 4,
            [Description("ff0000")]
            ｱｶ = 5,
            [Description("808000")]
            ｵﾘｰﾌﾞ = 6,
            [Description("ffff00")]
            ｷｲﾛ = 7,
            [Description("008000")]
            ｸﾞﾘｰﾝ = 8,
            [Description("00ff00")]
            ｾｯｶｲ = 9,
            [Description("008080")]
            ﾁｲﾙ = 10,
            [Description("00ffff")]
            ｱｸｱ = 11,
            [Description("000080")]
            ﾈｲﾋﾞｰ = 12,
            [Description("0000ff")]
            ｱｵ = 13,
            [Description("800080")]
            ﾑﾗｻｷ = 14,
            [Description("ff00ff")]
            ﾌｸｼｱ = 15
        }

        public enum PROCESS_COLOR_KUBUN_ENG
        {
            [Description("000000")]
            black = 1,
            [Description("808080")]
            gray = 2,
            [Description("c0c0c0")]
            silver = 3,
            [Description("800000")]
            maroon = 4,
            [Description("ff0000")]
            red = 5,
            [Description("808000")]
            olive = 6,
            [Description("ffff00")]
            yellow = 7,
            [Description("008000")]
            green = 8,
            [Description("00ff00")]
            lime = 9,
            [Description("008080")]
            teal = 10,
            [Description("00ffff")]
            aqua = 11,
            [Description("000080")]
            navy = 12,
            [Description("0000ff")]
            blue = 13,
            [Description("800080")]
            purple = 14,
            [Description("ff00ff")]
            fuchsia = 15
        }

        /// <summary>
        ///     Created By VijayT[20170901]- For Fotou Shukko.
        /// </summary>
        /// 

        public enum FUTOU_SHUKKO_KUBUN_JP
        {
            [Description("メーカー返品")]
            メーカー返品 = 10,
            [Description("処分")]
            処分 = 20,
            [Description("他社渡し")]
            他社渡し = 80,
            [Description("その他")]
            その他 = 99,
        }
        public enum FUTOU_SHUKKO_KUBUN_ENG
        {
            [Description("Returned To Maker")]
            ReturnedToMaker = 10,
            [Description("Disposed")]
            Disposed = 20,
            [Description("Delivered to Other Companies")]
            DeliveredToOtherCompanies = 80,
            [Description("Other")]
            Other = 99,
        }

        /// <summary>
        ///     Created By GirishB[20170420]- For PMS Entry Edit Delete Screens
        /// </summary>
        public enum NEXT_PROCESS_KUBUN_JP
        {
            矯正 = 1,
            完検査 = 2,
            次工程無し = 99
        }
        public enum NEXT_PROCESS_KUBUN_EN
        {
            [Description("Straightening")]
            Straightening = 1,
            [Description("Final Inspection")]
            FinalInspection = 2,
            [Description("None")]
            None = 99
        }



        public enum PROCESS_JP
        {
            引抜 = 1,
            矯正 = 2,
            完検査 = 3
        }
        public enum PROCESS_EN
        {
            Hikinuki = 1,
            [Description("Straightening")]
            Straightening = 2,
            [Description("Final Inspection")]
            FinalInspection = 3,

        }




        /// <summary>
        ///     Created By VijayT[20170509]- For Diaou Product Screen.
        /// </summary>
        public enum AARI_KUBUN_JP
        {
            有 = 1
        }
        public enum AARI_KUBUN_EN
        {
            Present = 1
        }



        //Added BY Sagar B on 9/8/2017
        public enum BEND_STATUS_KUBAN_JP
        {
            [Description("良")]
            良 = 1,
            [Description("良２")]
            良２ = 2,
            [Description("良３")]
            良３ = 3,
            [Description("保留")]
            保留 = 4,
            [Description(":矯正")]
            矯正 = 5,
            [Description("多ロール")]
            多ロール = 6,
            [Description("その他")]
            その他 = 7
        }

        //Added BY Sagar B on 9/8/2017
        public enum BEND_STATUS_KUBAN_ENG
        {
            [Description("OK")]
            OK = 1,
            [Description("OK2")]
            OK_2 = 2,
            [Description("OK3")]
            OK_3 = 3,
            [Description("HOLD")]
            HOLD = 4,
            [Description("CORRECTION")]
            CORRECTION = 5,
            [Description("MULTIROLL")]
            MULTIROLL = 6,
            [Description("OTHER")]
            OTHER = 7
        }

        public enum PAYMENT_MODE_JP  // DENPYOU_KUBUN 1
        {
            売掛取引 = 1,
            現金取引 = 2
        }
        public enum PAYMENT_MODE_EN  // DENPYOU_KUBUN 1
        {
            ACCOUNT_TRN = 1,
            CASH_TRN = 2
        }

        public enum CUST_LOCALITY_JP  // DENPYOU_KUBUN 2
        {
            国内 = 1,
            海外 = 2
        }
        public enum CUST_LOCALITY_EN    // DENPYOU_KUBUN 2
        {
            DOMESTIC = 1,
            OVERSEAS = 2
        }


        public enum SPOT_UNIT_EN //  Unit For Spot 
        {
            B = 1,
            BN = 2,
            P = 3,
            C = 4,
        }
        public enum SPOT_UNIT_JP
        {
            本 = 1,
            束 = 2,
            個 = 3,
            コイル = 4,
        }


        public enum MFG_LOCATION_KUBUN_JP
        {
            第一工場 = 1,
            第二工場 = 2,
            野木工場 = 3,
            結城工場 = 4,
            山形ＮＣ工場 = 5,
            中国蘇州工場 = 6,
            フィリピン工場 = 7
        }
        public enum MFG_LOCATION_KUBUN_EN
        {
            FIRST_FACTORY = 1,
            SECOND_FACTORY = 2,
            NOGY_FACTORY = 3,
            YUKI_FACTORY = 4,
            YAMAGATA_NC = 5,
            CHINA_SUZHOU = 6,
            PHILIPPINES = 7
        }

        public enum PRODUCT_MFG_KUBUN_JP
        {
            BAR_TO_BAR = 1,
            COIL_TO_BAR = 2,
            COIL_TO_COIL = 3,
            切削加工 = 4,
            その他加工 = 5,
            切断 = 6

        }
        public enum PRODUCT_MFG_KUBUN_EN
        {
            BAR_TO_BAR = 1,
            COIL_TO_BAR = 2,
            COIL_TO_COIL = 3,
            NEED_TO_CONFIRM = 4,
            OTHER = 5,
            CUTTING = 6
        }

        public enum MATERIAL_MFG_KUBUN_JP
        {
            自社材 = 1,
            有償支給材 = 2,
            無償支給材 = 3

        }
        public enum MATERIAL_MFG_KUBUN_EN
        {
            OWN_MATL = 1,
            PAID_MATL = 2,
            FREE_SUPPLIES = 3
        }

        public enum BILLING_KUBUN_JP
        {
            [Description("売上")]
            売上 = 1,
            [Description("売上（社内処理）")]
            売上_社内処理 = 2,
            [Description("返品")]
            返品 = 3,
            [Description("返品（社内処理）")]
            返品_社内処理 = 4,
            単価訂正 = 5,
            [Description("単価訂正（社内処理）")]
            単価訂正_社内処理 = 6,
            [Description("金額訂正")]
            金額訂正 = 7,
            [Description("金額訂正（社内処理）")]
            金額訂正_社内処理 = 8,
            [Description("重量訂正")]
            重量訂正 = 9,
            [Description("重量訂正（社内処理）")]
            重量訂正_社内処理 = 10,
            [Description("値引訂正")]
            値引訂正 = 11,
            [Description("値引訂正（社内処理）")]
            値引訂正_社内処理 = 12,
            [Description("検収差額")]
            検収差額 = 13,
            [Description("検収差額（社内処理）")]
            検収差額_社内処理 = 14,
            [Description("運賃")]
            運賃 = 15,
            [Description("運賃（社内処理）")]
            運賃_社内処理 = 16,
            [Description("切断賃")]
            切断賃 = 17,
            [Description("切断賃（社内処理）")]
            切断賃_社内処理 = 18,
            [Description("計算違い")]
            計算違い = 19,
            [Description("計算違い（社内処理）")]
            計算違い_社内処理 = 20,
            [Description("口銭")]
            口銭 = 21,
            [Description("口銭（社内処理）")]
            口銭_社内処理 = 22,
            [Description("金利")]
            金利 = 23,
            [Description("金利（社内処理）")]
            金利_社内処理 = 24,
            [Description("クレーム")]
            クレーム = 25,
            [Description("クレーム（社内処理）")]
            クレーム_社内処理 = 26,
            [Description("その他")]
            その他 = 27,
            [Description("その他（社内処理 ")]
            その他_社内処理 = 28

        }
        public enum BILLING_KUBUN_EN
        {
            [Description("Sales")]
            Sales = 1,
            [Description("Sales (Internal Processing)")]
            Sales_Internal_Processing = 2,
            [Description("Returns Henpin")]
            Returns_Henpin = 3,
            [Description("Returns Henpin (Internal Processing)")]
            Returns_Henpin_Internal_Processing = 4,
            [Description("Unit Price Correction")]
            Unit_Price_Correction = 5,
            [Description("Unit Price Correction (Internal Processing)")]
            Unit_Price_Correction_Internal_Processing = 6,
            [Description("Amount Correction")]
            Amount_Correction = 7,
            [Description("Amount Correction (Internal Processing)")]
            Amount_correction_Internal_Processing = 8,
            [Description("Weight Correction")]
            Weight_Correction = 9,
            [Description("Weight Correction (Internal Processing)")]
            Weight_Correction_Internal_Processing = 10,
            [Description("Discount Correction")]
            Discount_Correction = 11,
            [Description("Discount Correction (Internal Processing)")]
            Discount_Correction_Internal_Processing = 12,
            [Description("Acceptance Inspection Difference")]
            Acceptance_Inspection_Difference = 13,
            [Description("Acceptance Inspection difference (Internal Processing)")]
            Acceptance_Inspection_difference_Internal_Processing = 14,
            [Description("Fare")]
            Fare = 15,
            [Description("Fare (Internal Processing)")]
            Fare_Internal_Processing = 16,
            [Description("Cutting Fare")]
            Cutting_Fare = 17,
            [Description("Cutting Fare (Internal Processing)")]
            Cutting_Fare_Internal_Processing = 18,
            [Description("Difference In calculation")]
            Difference_In_Calculation = 19,
            [Description("Difference In calculation (Internal Processing)")]
            Difference_In_Calculation_Internal_Processing = 20,
            [Description("Commission")]
            Commission = 21,
            [Description("Commission (Internal Processing)")]
            Commission_Internal_Processing = 22,
            [Description("Interest Rate")]
            Interest_Rate = 23,
            [Description("Interest Rate (Internal Processing)")]
            Interest_Rate_Internal_Processing = 24,
            [Description("Claim")]
            Claim = 25,
            [Description("Claim (Internal Processing)")]
            Claim_Internal_Processing = 26,
            [Description("Other")]
            Other = 27,
            [Description("Other (Internal Processing) ")]
            Other_Internal_Processing = 28
        }

        public enum CUTTING_KUBUN_JP
        {
            [Description("一定寸法")]
            ONE_一定寸法 = 1,
            [Description("２ツ切り")]
            TWO_ツ切り = 2,
            [Description("３ツ切り")]
            THREE_ツ切り = 3,
        }

        public enum CUTTING_KUBUN_EN
        {
            [Description("Constant")]
            ONE_CONSTANT = 1,
            [Description("2 CUT")]
            TWO_CUT = 2,
            [Description("3 CUT")]
            THREE_CUT = 3,
        }

        public static Dictionary<string, int> DEFECTREASON = new Dictionary<string, int>()     //For DropDown
        {
            {"客先",1},
            {"販売業務(営業)",2 },
            {"販売業務 (事務員)",3},
            {"倉庫内",4},
            {"工場内",5},
            {"素材メーカー",6 },
            {"仕入先業者",7},
            {"不明",8},
            {"その他",9},
        };
    }
}
