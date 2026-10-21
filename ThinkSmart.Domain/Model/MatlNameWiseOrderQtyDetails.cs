using System;
using System.Collections.Generic;
using System.Text;

namespace TSWebApplication.Domain.Model
{
    public class MatlNameWiseOrderQtyDetails
    {
        public float? TOTALQTY { get; set; }
        public string INTERNAL_NAME_STRING { get; set; }
        public string MATL_KUBUN_NAME { get; set; }
        public Single? PLAN_QTY_THIS_MONTH { get; set; }
        public Single? PLAN_QTY_1MONTH_AHEAD { get; set; }
        public Single? PLAN_QTY_2MONTH_AHEAD { get; set; }
        public Single? PLAN_QTY_3MONTH_AHEAD { get; set; }
        public Single? PLAN_QTY_4MONTH_AHEAD { get; set; }
    }
}
