﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Threading;
using System.Net;
using System.Windows.Forms;
using Newtonsoft.Json;

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
        string adminCredentialsEmail;
        string adminCredentialsPass;
        string downloadDirectoryPath;
        string fileUploadURL;
        string planningOverviewURL;
        bool debug;

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
    }
}
