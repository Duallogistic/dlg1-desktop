using Newtonsoft.Json;
using System.Collections.Generic;

namespace AmazonDeliveryPlanner
{
    public class SerializedConfiguration
    {
        string adminURL;
        string driverListURL;
        string[] defaultTabs;
        string apiBaseURL;
        string plannerListURL;
        string relayCredentialsEmail;
        string relayCredentialsPass;
        int autoDownloadExportFileRandomMinInterval;
        int autoDownloadExportFileRandomMaxInterval;
        string adminCredentialsEmail;
        string adminCredentialsPass;
        string downloadDirectoryPath;
        string fileUploadURL;
        string planningOverviewURL;
        bool debug;
        List<TripPageConfiguration> tripPages;

        [JsonProperty("admin_url")]
        public string AdminURL { get => adminURL; set => adminURL = value; }
        [JsonProperty("driver_list_url")]
        public string DriverListURL { get => driverListURL; set => driverListURL = value; }
        [JsonProperty("default_tabs")]
        public string[] DefaultTabs { get => defaultTabs; set => defaultTabs = value; }
        [JsonProperty("api_base_url")]
        public string ApiBaseURL { get => apiBaseURL; set => apiBaseURL = value; }
        [JsonProperty("planner_list_url")]
        public string PlannerListURL { get => plannerListURL; set => plannerListURL = value; }
        [JsonProperty("relay_credentials_email")]
        public string RelayCredentialsEmail { get => relayCredentialsEmail; set => relayCredentialsEmail = value; }
        [JsonProperty("relay_credentials_pass")]
        public string RelayCredentialsPass { get => relayCredentialsPass; set => relayCredentialsPass = value; }
        [JsonProperty("auto_download_export_file_random_min_interval")]
        public int AutoDownloadExportFileRandomMinInterval { get => autoDownloadExportFileRandomMinInterval; set => autoDownloadExportFileRandomMinInterval = value; }
        [JsonProperty("auto_download_export_file_random_max_interval")]
        public int AutoDownloadExportFileRandomMaxInterval { get => autoDownloadExportFileRandomMaxInterval; set => autoDownloadExportFileRandomMaxInterval = value; }
        [JsonProperty("admin_credentials_email")]
        public string AdminCredentialsEmail { get => adminCredentialsEmail; set => adminCredentialsEmail = value; }
        [JsonProperty("admin_credentials_pass")]
        public string AdminCredentialsPass { get => adminCredentialsPass; set => adminCredentialsPass = value; }
        [JsonProperty("download_directory_path")]
        public string DownloadDirectoryPath { get => downloadDirectoryPath; set => downloadDirectoryPath = value; }
        [JsonProperty("file_upload_url")] 
        public string FileUploadURL { get => fileUploadURL; set => fileUploadURL = value; }
        [JsonProperty("debug")]
        public bool Debug { get => debug; set => debug = value; }
        [JsonProperty("planning_overview_url")]
        public string PlanningOverviewURL { get => planningOverviewURL; set => planningOverviewURL = value; }
        [JsonProperty("trip_pages", IsReference = true)]
        public List<TripPageConfiguration> TripPageConfigurations { get => tripPages; set => tripPages = value; }
    }


    public class TripPageConfiguration
    {
        string url;
        int minRandomIntervalMinutes;
        int maxRandomIntervalMinutes;

        [JsonProperty("url")]        
        public string Url { get => this.url; set => this.url = value; }

        [JsonProperty("min_random_interval_minutes")]
        public int MinRandomIntervalMinutes { get => minRandomIntervalMinutes; set => minRandomIntervalMinutes = value; }

        [JsonProperty("max_random_interval_minutes")]                
        public int MaxRandomIntervalMinutes { get => maxRandomIntervalMinutes; set => maxRandomIntervalMinutes = value; }
    }

}
