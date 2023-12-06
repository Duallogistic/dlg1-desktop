// using CamioaneAmazon.CEF;
using CefSharp;
using CefSharp.WinForms;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace AmazonDeliveryPlanner
{
    public partial class BrowserTimerExportUserControl : UserControl, IDisposable
    {
        ChromiumWebBrowser browser;

        string url;

        RequestContextSettings requestContextSettings;

        public event EventHandler Close;
        public event EventHandler<FileUploadFinishedEventArgs> FileUploadFinished;

        public event EventHandler<UpdateAutoDownloadIntervalStatusEventArgs> UpdateAutoDownloadStatus;



        //long driverId;

        int minRandomIntervalMinutes;
        int maxRandomIntervalMinutes;

        bool exportFileAutoDownloadEnabled;
        string pageType = "unknown";


        public EventHandler<string> DloadDone;

        public BrowserTimerExportUserControl() : this("", null)
        {
        }

        public BrowserTimerExportUserControl(string url, RequestContextSettings requestContextSettings)
        {
            // this.driverId = driverId;
            this.url = url;
            this.requestContextSettings = requestContextSettings;

            InitializeComponent();

            InitBrowser();

            browser.PreviewKeyDown += Browser_PreviewKeyDown;
            browser.KeyUp += Browser_KeyUp;
            browser.KeyboardHandler = new BrowserKeyboardHandler();

        }

        public void GoToURL(string url)
        {
            browser.Load(url);
            InitAutoDownloadTimer(url);

            if (!string.IsNullOrWhiteSpace(url))
                SetPageType(url);
        }

        public void RestartTimers()
        {
            GlobalContext.Log("Restart timers for '{0}'", browser.Address);

            browser.Load(browser.Address); // reload current address
            InitAutoDownloadTimer(browser.Address);

            if (!string.IsNullOrWhiteSpace(browser.Address))
                SetPageType(browser.Address);
        }

       

        private void Browser_KeyUp(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = false;
        }

        private void Browser_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if ((e.Control || e.KeyCode == Keys.ControlKey) && (e.KeyCode == Keys.F))
                GlobalContext.MainWindow.OpenSearchDriverForm();
        }

       

        void InitBrowser()
        {
          

            browser = new ChromiumWebBrowser();

            if (requestContextSettings != null)
                browser.RequestContext = new RequestContext(requestContextSettings);
           

            browser.DownloadHandler = new DownloadHandler();

            ((DownloadHandler)browser.DownloadHandler).OnBeforeDownloadFired += BrowserTimerExportUserControl_OnBeforeDownloadFired;
            ((DownloadHandler)browser.DownloadHandler).OnDownloadUpdatedFired += BrowserUserControl_OnDownloadUpdatedFired;

            
            panel1.Controls.Add(browser);

            browser.Dock = DockStyle.Fill;

            browser.FrameLoadEnd += Browser_FrameLoadEnd;

        
            this.PerformLayout();
            this.Invalidate();
            this.Refresh();

            browser.Load(url);

            browser.Dock = DockStyle.Fill;

            InitAutoDownloadTimer(url);

            if (!string.IsNullOrWhiteSpace(url))
                SetPageType(url);
        }

        void SetPageType(string loadedUrl)
        {
            if (loadedUrl.IndexOf("relay.amazon", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                pageType = (loadedUrl.IndexOf("in-transit", StringComparison.OrdinalIgnoreCase) >= 0) ? "transit" : pageType;
                pageType = (loadedUrl.IndexOf("history", StringComparison.OrdinalIgnoreCase) >= 0) ? "history" : pageType;
                pageType = (loadedUrl.IndexOf("upcoming", StringComparison.OrdinalIgnoreCase) >= 0) ? "upcoming" : pageType;
            }
            else
            {
                pageType = "unknown";
                GlobalContext.Log("");
                GlobalContext.Log("    Warning - unknown page type");
            }
        }

        private void BrowserTimerExportUserControl_OnBeforeDownloadFired(object sender, DownloadItem e)
        {
            string fileSuffix = pageType + ".csv";
            e.FullPath = e.FullPath.Replace(".csv", fileSuffix);
            e.SuggestedFileName = fileSuffix; 
        }

        void InitAutoDownloadTimer(string loadedUrl)
        {
          
            string[] accepted = {
                "https://relay.amazon.co.uk/tours/history",
                "https://relay.amazon.co.uk/tours/in-transit",
                "https://relay.amazon.co.uk/tours/upcoming"
                };
            if (!accepted.Contains(loadedUrl)) return;
            if (minRandomIntervalMinutes == 0 || maxRandomIntervalMinutes == 0) {
                GlobalContext.Log("Auto download with random interval not started because of the configured values 2 - interval between {0} and {1} minutes", minRandomIntervalMinutes, maxRandomIntervalMinutes);
                return;
            }

            GlobalContext.Log("InitAutoDownloadTimer for: " + loadedUrl);
                
            if (autoDownloadIntervalTask != null)
            {
                ts.Cancel();
            }

            ts = new CancellationTokenSource();

            autoDownloadIntervalTask = Task.Run(() =>
                {
                    GlobalContext.Log("Started auto download with random interval between {0} and {1} minutes", minRandomIntervalMinutes, maxRandomIntervalMinutes);
                    StartAutoDownloadInterval(true);
                },
                ts.Token
            );
            
        }

        Task autoDownloadIntervalTask = null;
        CancellationTokenSource ts;

        void StartAutoDownloadInterval(bool first)
        {
            System.Windows.Forms.Timer refreshTimer = new System.Windows.Forms.Timer();

            if (first)
            {
                int minRandomInterval = minRandomIntervalMinutes * 60 * 1000;
                int maxRandomInterval = maxRandomIntervalMinutes * 60 * 1000;

                int delayBase = minRandomInterval;
                int addedRandom = maxRandomInterval - minRandomInterval;

                int waitPeriodMillisec = (int)(delayBase + (new Random(DateTime.Now.Millisecond)).NextDouble() * addedRandom);

                TimeSpan waitPeriod = TimeSpan.FromMilliseconds(waitPeriodMillisec);

                StartRefreshTimer(refreshTimer, (int)Math.Round(waitPeriod.TotalMilliseconds));

                if (UpdateAutoDownloadStatus != null)
                    UpdateAutoDownloadStatus(this, new UpdateAutoDownloadIntervalStatusEventArgs(String.Format("Waiting {0} before downloading export file.", waitPeriod.ToHumanReadableString())));

                GlobalContext.Log("Auto download with random interval - waiting {0}", waitPeriod.ToHumanReadableString());
                Thread.Sleep(waitPeriod);
            }

            {
                if (ts.IsCancellationRequested)
                    return;

                this.Invoke((MethodInvoker)delegate
                {
                    ClickExportTripsFile();
                });

                int minRandomInterval = minRandomIntervalMinutes * 60 * 1000;
                int maxRandomInterval = maxRandomIntervalMinutes * 60 * 1000;

                int delayBase = minRandomInterval;
                int addedRandom = maxRandomInterval - minRandomInterval;

                int waitPeriodMilliSec = (int)(delayBase + (new Random(DateTime.Now.Millisecond)).NextDouble() * addedRandom);

                TimeSpan waitPeriod = TimeSpan.FromMilliseconds(waitPeriodMilliSec);

                StartRefreshTimer(refreshTimer, waitPeriodMilliSec);

                if (ts.IsCancellationRequested)
                {
                    refreshTimer.Stop();
                    return;
                }

                if (UpdateAutoDownloadStatus != null)
                    UpdateAutoDownloadStatus(this, new UpdateAutoDownloadIntervalStatusEventArgs(String.Format("Last export made at {0}. Expoting again at {1},  in {2}", DateTime.Now.ToString("HH:mm:ss"), DateTime.Now.Add(waitPeriod).ToString("HH:mm:ss"), waitPeriod.ToHumanReadableString()))); // in {2:00} s

                GlobalContext.Log("Auto download with random interval - waiting {0}", waitPeriod.ToHumanReadableString());
                Thread.Sleep(waitPeriod);

                StartAutoDownloadInterval(false);

                if (ts.IsCancellationRequested)
                {
                    // refreshTimer.Stop();
                    return;
                }
            }

            refreshTimer.Stop();
        }

        void StartRefreshTimer(System.Windows.Forms.Timer refreshTimer, int waitPeriodMilliSec)
        {
                int randomROT = (int)(new Random(DateTime.Now.Millisecond)).Next(0, 15);
                int refreshOverTimeMs = (int)Math.Round(waitPeriodMilliSec * (0.80 + (randomROT / 10)));

                if (refreshOverTimeMs < 25 * 1000)
                    refreshOverTimeMs = (int)Math.Round(waitPeriodMilliSec * (0.50 + (randomROT / 10)));


                refreshTimer.Tick += RefreshTimer_Tick;
                refreshTimer.Interval = refreshOverTimeMs;
                refreshTimer.Start();
                GlobalContext.Log("Refreshing page in {0}", TimeSpan.FromMilliseconds(refreshTimer.Interval).ToHumanReadableString());
        }

        private void RefreshTimer_Tick(object sender, EventArgs e)
        {
            browser.Reload(); // browser.Reload(true); // ignores the cache
            GlobalContext.Log("Reloaded page '{0}'", browser.Address);
        }

        private async void BrowserUserControl_OnDownloadUpdatedFired(object sender, DownloadItem e)
        {
            if (!e.IsComplete) return;

            string uploadURL = "";

            try
            {
                uploadURL = GlobalContext.SerializedConfiguration.AdminURL + GlobalContext.SerializedConfiguration.ApiBaseURL + GlobalContext.SerializedConfiguration.FileUploadURL;

                GlobalContext.Log("Upload URL=\"{0}\"", uploadURL);

                string fileName = e.SuggestedFileName;
                if (string.IsNullOrEmpty(fileName))
                    fileName = Path.GetFileName(e.FullPath);

                string csvFileContents = File.ReadAllText(e.FullPath);
                csvFileContents = csvFileContents.Replace(",Operator ID,Spot Work", ",Operator ID,Spot Work,ColBC,COlBD");

                byte[] bytes = Encoding.UTF8.GetBytes(csvFileContents);
                HttpContent bytesContent = new ByteArrayContent(bytes);

                using (var client = new HttpClient())
                using (var formData = new MultipartFormDataContent())
                {
                    formData.Add(bytesContent, "files", fileName);

                    var response = await client.PostAsync(uploadURL, formData);

                    if (!response.IsSuccessStatusCode)
                    {
                        // Handle non-successful response
                    }

                    using (var stream = await response.Content.ReadAsStreamAsync())
                    using (var sr = new StreamReader(stream))
                    {
                        string responseText = sr.ReadToEnd();
                        LogResponse(responseText);
                    }

                    response.EnsureSuccessStatusCode();
                    FileUploadFinished?.Invoke(this, new FileUploadFinishedEventArgs(fileName));
                 
                    //MessageBox.Show("Fisierul " + fileName + " a fost descarcat", GlobalContext.ApplicationTitle);
                }
            }
            catch (Exception ex)
            {
                GlobalContext.Log($"Exception uploading the file to {uploadURL}: {ex.Message}");
                MessageBox.Show($"Exception uploading the file to {uploadURL}: {ex.Message}", GlobalContext.ApplicationTitle);
            }
            onUploadDone(browser);
        }

        private void onUploadDone(ChromiumWebBrowser browser)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    DloadDone?.Invoke(this, browser.Address);
                }));
            }
            else
            {
                DloadDone?.Invoke(this, browser.Address);
            }
            //DloadDone(this, browser.Address);
        }


        private static void LogResponse(string responseText)
        {
            if (GlobalContext.SerializedConfiguration.Debug)
            {
                try
                {
                    // var responseText = response.Content;

                    var r = Newtonsoft.Json.JsonConvert.DeserializeObject(responseText);

                    string identedResponse = Newtonsoft.Json.JsonConvert.SerializeObject(r, Formatting.Indented);

                    GlobalContext.Log("Server response: {0} {1}", System.Environment.NewLine, identedResponse);


                    string debugDirectory = Path.Combine(MainForm.GetFileStoragePath(), "debug");

                    if (!Directory.Exists(debugDirectory))
                        Directory.CreateDirectory(debugDirectory);

                    {
                        string debugFilePathTime = Path.Combine(debugDirectory, $"response_{DateTime.Now.ToString("yyyyMMdd_hhmmss")}.json");

                        File.WriteAllText(debugFilePathTime, identedResponse);
                    }

                    {
                        string debugFilePath = Path.Combine(debugDirectory, $"last_response.json");

                        File.WriteAllText(debugFilePath, identedResponse);
                    }
                }
                catch (Exception ex)
                {
                    GlobalContext.Log("Exception = " + ex.Message);
                }
            }
        }

        private async void Browser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            try
            {
                GlobalContext.Log("Browser_FrameLoadEnd url={0} frame={1}", e.Url, e.Frame.Name);

                if (e.Frame.IsMain)
                {
                    // var watch = System.Diagnostics.Stopwatch.StartNew();
                    // string html = await browser.GetSourceAsync();

                    // https://www.amazon.com/ap/signin?openid.return_to=https://relay.amazon.com/&openid.identity=http://specs.openid.net/auth/2.0/identifier_select&openid.assoc_handle=amzn_relay_desktop_us&openid.mode=checkid_setup&openid.claimed_id=http://specs.openid.net/auth/2.0/identifier_select&openid.ns=http://specs.openid.net/auth/2.0&pageId=amzn_relay_desktop_us
                    if ((e.Url.IndexOf("amazon.com/ap/signin") >= 0) ||
                        (e.Url.IndexOf("amazon.co.uk/ap/signin") >= 0))
                    {
                        string email = GlobalContext.ApiConfig.relayAuth.username;
                        string pass = GlobalContext.ApiConfig.relayAuth.password;

                        string jsSource1 = string.Format(
                            "(function () {{ document.getElementById('ap_email').value = '{0}'; document.getElementById('ap_password').value = '{1}'; }} )(); ",
                            email,
                            pass
                        );

                        JavascriptResponse response = await browser.GetMainFrame().EvaluateScriptAsync(jsSource1);
                    }

                 
                }
            }
            catch (Exception ex)
            {
                GlobalContext.Log("Exception = " + ex.Message); //--
            }
        }


        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);
        // Mouse actions
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;

        public string Url { get => url; /*set => url = value;*/ }
        public int MinRandomIntervalMinutes { get => minRandomIntervalMinutes; set => minRandomIntervalMinutes = value; }
        public int MaxRandomIntervalMinutes { get => maxRandomIntervalMinutes; set => maxRandomIntervalMinutes = value; }
        public bool ExportFileAutoDownloadEnabled
        {
            get => exportFileAutoDownloadEnabled;
            set => exportFileAutoDownloadEnabled = value;
        }

     
        private void showDevToolsButton_Click(object sender, EventArgs e)
        {
            browser.ShowDevTools();
        }

        private void refrehPageButton_Click(object sender, EventArgs e)
        {
            browser.Reload(true);
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            if (Close != null)
                Close(this, EventArgs.Empty);
        }

        private void increaseTextSizeButton_Click(object sender, EventArgs e)
        {
            browser.SetZoomLevel(browser.GetZoomLevelAsync().Result + 0.1);
        }

        private void decreaseTextSizeButton_Click(object sender, EventArgs e)
        {
            browser.SetZoomLevel(browser.GetZoomLevelAsync().Result - 0.1);
        }

     

        private void goBackButton_Click(object sender, EventArgs e)
        {
            browser.Back();
        }

        public class FileUploadFinishedEventArgs : EventArgs
        {
            public FileUploadFinishedEventArgs(string fileName)
            {
                FileName = fileName;
            }

            public string FileName { get; set; }
        }

        public class UpdateAutoDownloadIntervalStatusEventArgs : EventArgs
        {
            public UpdateAutoDownloadIntervalStatusEventArgs(string text)
            {
                Text = text;
            }

            public string Text { get; set; }
        }

        private void goForwardButton_Click(object sender, EventArgs e)
        {
            browser.Forward();
        }

        

        public async void ClickExportTripsFile()
        {
            try
            {
                GlobalContext.Log("Clicking on the export button...");

                JavascriptResponse response = await browser.GetMainFrame().EvaluateScriptAsync(GlobalContext.Scripts["clickExportTripsButton"]);

                if (response == null)
                    throw new NullReferenceException("response == null");

                if (response.Result == null)
                    throw new NullReferenceException("response.Result == null");
                // return;

                bool jsScriptResult = (bool)response.Result;

                if (!jsScriptResult)
                    GlobalContext.Log($"Failed to click on the export button!");
            }
            catch (Exception ex)
            {
                GlobalContext.Log("Failed to click on the export button: {0}", ex.Message);
                onUploadDone(browser);
            }
        }

        private void downloadTripsButton_Click(object sender, EventArgs e)
        {
            ClickExportTripsFile();
        }
    }
}
