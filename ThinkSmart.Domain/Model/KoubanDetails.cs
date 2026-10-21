using System;
using System.Collections.Generic;
using System.Text;

namespace TSWebApplication.Domain.Model
{
    public class KoubanDetails
    {
        public short FunctionKubun { get; set; }

        public long? PO_NO { get; set; }
        public long? PO_DTL_CD { get; set; }
        public long? PO_DTL_NO { get; set; }

        public short? FUTOU_CD { get; set; }
        public string FUTOU_NAME { get; set; }

        public int? SHOUSHA_CD { get; set; }
        public string SHOUSHA_NAME { get; set; }

        public short? HTSAKI_CD { get; set; }
        public string HTSAKI_NAME { get; set; }

        public Int32? KOUBAN_CD { get; set; }
        public int? MATERIAL_CD { get; set; }

        public short? MATL_KUBUN { get; set; }
        public int? MAKER_CD { get; set; }
        public string MAKER_SHORT_NAME { get; set; }
        public string MATL_NAME_MAKER { get; set; }
        public string MATL_NAME_INTERNAL { get; set; }

        public int? MATL_NAME_INTERNAL_CD { get; set; }

        public int? MATL_NAME_MAKER_CD { get; set; }

        public int? MOCHIKOMISAKI_CD { get; set; }
        public string MKSAKI_NAME { get; set; }

        public short? MUKESAKI_CD { get; set; }
        public string MUKESAKI_NAME { get; set; }
        public string MATL_SPECIAL_KUBUN_VAL { get; set; }

        public short? MATL_SPECIAL_KUBUN { get; set; }
        public DateTime? DELIVERYDATE { get; set; }

        public string KOUBAN { get; set; }

        public string COIL_BAR_KURUKAWA { get; set; }
        public short? COIL_BAR_KURUKAWA_KUBUN { get; set; }

        public short? KOUBAN_COMPLETE { get; set; }
        public DateTime? KOUBAN_COMPLETE_DATE { get; set; }

        public int? PO_PENDING_QTY { get; set; }
        public string REMARKS { get; set; }
        public string ROLLING_DATE { get; set; }
        public short? ROLL_SHORT { get; set; }
        public string TOIAWASE_MEMO { get; set; }
        public short? TORIHIKI_COMPLETE { get; set; }

        public Single? DIM_D { get; set; }
        public string SHAPE { get; set; }
        public string DIM_SHAPE { get; set; }
        public decimal? PO_WEIGHT { get; set; }
        public DateTime PO_DATE { get; set; }
        public decimal? CONFIRMED_WEIGHT { get; set; }
        public Single? MATERIAL_LENGTH { get; set; }
        public string CONDITION_DESCRIPTION { get; set; }

        public short? PO_CHANCE { get; set; }
        public decimal? CONF_CHANCE { get; set; }

        public int? QTY { get; set; }
        public decimal? CONF_QTY { get; set; }

        public short? PO_ROLL_R { get; set; }
        public decimal? CONF_R { get; set; }

        public decimal? RATE { get; set; }
        public decimal? CONF_RATE { get; set; }

        public int? PO_YEAR { get; set; }
        public decimal? CONF_YYYY { get; set; }

        public string MATERIAL_KUBUN { get; set; }

        public short? DELY_DAYS { get; set; }
        public short? REPLY_DATE { get; set; }

        public int? USER_ID { get; set; }

        public DateTime? CREATED_DATE { get; set; }
        public DateTime? UPDATED_DATE { get; set; }

        public short? OPERATION_KUBUN { get; set; }


        /*Added new */
        public string ROLL_R_CHANCE { get; set; }

        public decimal? NYUKO_QUANTITY { get; set; }
        public decimal? ACT_ALLOC_QUANTITY { get; set; }
        public decimal? KOUBAN_REMAINING_QUANTITY { get; set; }
        public decimal? FUTOU_STOCK { get; set; }

        public decimal? FACTORY_STOCK { get; set; }
        public decimal? KOUBAN_STOCK { get; set; }
        public decimal? INTERNAL_STOCK { get; set; }
        public decimal? STOCK_QUANTITY { get; set; }
        public decimal? REMAINING_STOCK { get; set; }
        public string CHARGE { get; set; }
        public string PO_INT_MEMO { get; set; }
        public string PO_REMARK { get; set; }

        public int KOUBAN_HISTORY_NO { get; set; }
        public int KOUBAN_INT_NO { get; set; }
        public int CHG_SRLNO { get; set; }
        public DateTime CHG_DATE { get; set; }
        public int DEPARTMENT_GROUP_CD { get; set; }
        public int? REPL_KUBUN { get; set; }
        public string REMARK { get; set; }
        public DateTime? DELIVERY_DATE { get; set; }

        public string INTERNAL_NAME { get; set; }
        public Single? DIM_W { get; set; }
        public string DIM_OTHER { get; set; }
        public int? EMPLOYEE_CD { get; set; }

        public int? INTERNAL_NAME_CD { get; set; }
        public int? STEEL_TYPE_CD { get; set; }
        public string MATL_MAKER_NAME { get; set; }
        public string MAKER_NAME { get; set; }

        public int? GEN_SPC_DESC { get; set; }
        public int? GEN_SPC { get; set; }
        public int? CONDITION_CD { get; set; }
        public int? SUPPLIER_cd { get; set; }
        public int? MATL_KUBUN_DESC { get; set; }
        public decimal? LENGTH { get; set; }
        public int? IMP_RANK { get; set; }
        public int? MKSAKI_KUBUN { get; set; }

        public int? HTSAKI_KUBUN { get; set; }
        public string HTSAKI_CONT_PERSON { get; set; }

        public string FUTOU_SHORT_NAME { get; set; }

        public int? MATL_IN_ACTIVE { get; set; }

        public string DIM_D_SHAPE { get; set; }

        public string MATL_KUBUN_DECODED { get; set; }

        public string GEN_SPC_DECODE { get; set; }

        public decimal? REMAINING_TEMP_ALLOC_QTY { get; set; }
        public string YRC { get; set; }

        public int? MOCHIKOMIKSAKI_CD { get; set; }
        public decimal? TEMP_ALLOC_QTY { get; set; }
        public string DIM_D_STRING { get; set; }
        public int? INPUT_LOCATION_CD { get; set; }
        public string MAKER_YOBINA { get; set; }
        public string MAKER_YOBIGANA { get; set; }
        public DateTime? NYUKA_DATE { get; set; }
    }
}
