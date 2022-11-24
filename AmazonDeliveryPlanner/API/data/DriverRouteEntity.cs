using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonDeliveryPlanner.API.data
{
    public class DriverRouteEntity
    {
        public long? id;
        public long? driver_id;
        public string plan_note;
        public string op_note;
        public string vrid;
        public string loc1;
        public string loc2;
        public string loc3;
        public char shift;
        public DateTime pick_up_date;
        public DateTime created_at;
        public DateTime updated_at;        
    }
}
