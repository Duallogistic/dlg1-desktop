using AmazonDeliveryPlanner.API.data;
using AmazonDeliveryPlanner.Properties;
using CefSharp.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace AmazonDeliveryPlanner.API
{
    public class PlannerList
    {
        public Planner[] planners { get; set; }
    }

    //public class Root
    //{
    //    public List<Driver> drivers { get; set; }
    //}
    
    public class Planner
    {
        public static bool _ListModeToString = false;

        public long id { get; set; }

        public string email { get; set; }
        public string password { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string phone { get; set; }
        public string role_name { get; set; }
        public bool is_active { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public string[] roles { get; set; }
        // public string fire_id { get; set; } // string or?
        // public string app_version { get; set; } // string or?
        public string token { get; set; }        

        public override string ToString()
        {
            if (_ListModeToString)
                return (
                        (string.IsNullOrWhiteSpace(this.last_name)  ? "" : this.last_name.Trim()) + " " +
                        (string.IsNullOrWhiteSpace(this.first_name) ? "" : this.first_name.Trim()) + " " +
                        (string.IsNullOrWhiteSpace(this.role_name)  ? "" : "(" + this.role_name.Trim() + ")")
                       ).Trim();
            else
                return (
                        (string.IsNullOrWhiteSpace(this.last_name) ? "" : this.last_name.Trim()) + " " +
                        (string.IsNullOrWhiteSpace(this.first_name) ? "" : this.first_name.Trim()) + " " +
                        (string.IsNullOrWhiteSpace(this.role_name) ? "" : "(" + this.role_name.Trim() + ")")
                       ).Trim();
        }
    }

    public class PlannersAPI
    {
        public static PlannerList GetPlanners()
        {
            using (WebClient wc = new WebClient())
            {
                // string getDriversURL = GlobalContext.SerializedConfiguration.ApiBaseURL + "/auth2/external/get-all-planners";                
                // string getDriversURL = "http://167.86.94.125:52031/api/auth2/external/get-all-planners";
                string getPlannersURL = GlobalContext.SerializedConfiguration.PlannerListURL;

                GlobalContext.Log("Getting planners from  '{0}'", getPlannersURL);

                var jsonResponse = wc.DownloadString(getPlannersURL);


                PlannerList plannerList = JsonConvert.DeserializeObject<PlannerList>(jsonResponse);

                return plannerList;
            }
        }

        //public static DriverRouteEntity /*async Task<DriverRouteEntity>*/ PostRoute(API.data.DriverRouteEntity driverEntity)
        //{
        //    // ...

        //    string payload = JsonConvert.SerializeObject(new
        //    {
        //        driver_id = driverEntity.driver_id,
        //        plan_note = driverEntity.plan_note,
        //        op_note = driverEntity.op_note,
        //        vrid = driverEntity.vrid,
        //        loc1 = driverEntity.loc1,
        //        loc2 = driverEntity.loc2,
        //        loc3 = driverEntity.loc3,
        //        pick_up_date = driverEntity.pick_up_date.ToString("yyyy-MM-dd HH:mm:ss"),
        //        shift = driverEntity.shift
        //        //driver_id = driver_id,
        //        //plan_note = "text pentru planner",
        //        //op_note = "ceva text pr operator",
        //        //vrid = "AMZ12345",
        //        //loc1 = "BHX3",
        //        //loc2 = "BHX8",
        //        //loc3 = "",
        //        //pick_up_date = "2022-10-23 18:45:45",
        //        //shift = "D"
        //    });

        //    using (var client = new HttpClient())
        //    {
        //        var content = new StringContent(payload, Encoding.UTF8, "application/json");

        //        // HttpResponseMessage response = await client.PostAsync(Settings.Default.ApiPostEndPoint, content);

        //        throw new Exception("undefined api endpoint");

        //        using (HttpResponseMessage response = /*await */client.PostAsync("", content).Result)
        //        {
        //            response.EnsureSuccessStatusCode();
                    
        //            string responseBody = /*await */ response.Content.ReadAsStringAsync().Result;

        //            // DriverRouteEntity responseDriverEntity = JsonConvert.DeserializeObject<DriverRouteEntity>(responseBody);

        //            dynamic x = JObject.Parse(responseBody);
        //            // JObject v = JObject.Parse(responseBody);
        //            // JToken token = v["planning"];
        //            string planningJSON = JsonConvert.SerializeObject(x.planning);

        //            // JsonSerializerSettings settings = new JsonSerializerSettings();
        //            //settings.Error

        //            DriverRouteEntity responseDriverEntity = JsonConvert.DeserializeObject<DriverRouteEntity>(planningJSON /*, settings */);

        //            return responseDriverEntity;
        //            // JObject x = JObject.Parse(responseBody);

        //            // int z = ((int)x["zx"]);
        //        }                
        //    }          
        //}
    }
}
