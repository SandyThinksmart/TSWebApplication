using System;
using System.Collections.Generic;
using System.Text;

namespace TSWebApplication.Domain.Model
{
    public class VehicleDetails
    {
        public long VEHICLE_CD { get; set; }
       
        public string VEHICLE_NAME { get; set; }
       
        public string SHORT_NAME { get; set; }
       
        public string YOBIGANA { get; set; }
        public string YOBINA { get; set; }
        public string DRIVER_NAME { get; set; }
        public string VEHICLE_NUMBER { get; set; }
        public string TELEPHONE_NO { get; set; }
        public long? DISPLACEMENT { get; set; }
        public long MAX_LOAD { get; set; }
        public string REMARKS { get; set; }
        
        public int UN_USE_FLAG { get; set; }
       
        public DateTime CREATED_DATE { get; set; }
        public DateTime UPDATED_DATE { get; set; }
        public string CREATED_DATE_STRING { get; set; }
        public string UPDATED_DATE_STRING { get; set; }

        public string TRANSPORT_COMPANY_NAME { get; set; }
        public long USER_ID { get; set; }
    }
}
