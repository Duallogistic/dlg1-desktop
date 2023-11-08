using AmazonDeliveryPlanner.API.data;
using Newtonsoft.Json;
using System.Net;
using System.Windows.Forms;

namespace AmazonDeliveryPlanner.API
{
    public class InitAppApi
    {
        public PlannerEntity[] planners { get; set; }
        public ApiConfig configuration { get; set; }


        public static InitAppApi GetAppInit()
        {
            using (WebClient wc = new WebClient())
            {
                string getPlannersURL = GlobalContext.SerializedConfiguration.AdminURL + GlobalContext.SerializedConfiguration.ApiBaseURL + GlobalContext.SerializedConfiguration.PlannerListURL;
                // MessageBox.Show(getPlannersURL);
                GlobalContext.Log("Getting planners from  '{0}'", getPlannersURL);
                var jsonResponse = wc.DownloadString(getPlannersURL);
                InitAppApi initAppConfig = JsonConvert.DeserializeObject<InitAppApi>(jsonResponse);
                GlobalContext.ApiConfig = initAppConfig.configuration;
                return initAppConfig;
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
