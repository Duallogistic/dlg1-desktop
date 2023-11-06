// using CamioaneAmazon.CEF;
using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Web.UI.WebControls;
using System.Net;
using CefSharp.DevTools.Network;
using RestSharp;
using Newtonsoft.Json;
using System.Net.Http;  
using AmazonDeliveryPlanner.API;
using CefSharp.DevTools.WebAudio;
using RestSharp.Extensions;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;
using System.Runtime.InteropServices.ComTypes;

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
            //. InitPanel2Browser();

            // new Task(() => { Thread.Sleep(800); InitBrowser(); }).Start();

            browser.PreviewKeyDown += Browser_PreviewKeyDown;
            browser.KeyUp += Browser_KeyUp;
            browser.KeyboardHandler = new BrowserKeyboardHandler();

            // urlTextBox.Text = url;
            // 
            // StartExportDownloadThread();
        }

        public void GoToURL(string url)
        {
            browser.Load(url);
            InitAutoDownloadTimer(url);
        }

        //void StartExportDownloadThread()
        //{
        //    new Task(() => {
        //        int minRandomInterval = minRandomIntervalMinutes * 60 * 1000;
        //        int maxRandomInterval = maxRandomIntervalMinutes * 60 * 1000;

        //        int delayBase = minRandomInterval;
        //        int addedRandom = maxRandomInterval - minRandomInterval;

        //        int waitPeriodSec = (int)(delayBase + (new Random(DateTime.Now.Millisecond)).NextDouble() * addedRandom);

        //        TimeSpan waitPeriod = TimeSpan.FromMilliseconds(waitPeriodSec);

        //        GlobalContext.Log("Auto download with random interval - waiting {0} s", waitPeriod.TotalSeconds);
        //        Thread.Sleep(waitPeriod);


        //        ClickExportTripsFile();
        //    }).Start();
        //}

        private void Browser_KeyUp(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = false;
        }

        private void Browser_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if ((e.Control || e.KeyCode == Keys.ControlKey) && (e.KeyCode == Keys.F))
                GlobalContext.MainWindow.OpenSearchDriverForm();
        }

        //public delegate void InitBrowserHandler();
        //            if (this.InvokeRequired)
        //    {
        //    InitBrowserHandler ofc = new InitBrowserHandler(InitBrowser);

        //    if (!this.IsDisposed)
        //    {
        //        this.Invoke(ofc, new object[] { });
        //        return;
        //    }

        //    return;
        //}
        //    else
        //    {

        void InitBrowser()
        {
            // !
            // System.AccessViolationException: 'Attempted to read or write protected memory. This is often an indication that other memory is corrupt.'
            //GlobalContext.GlobalCefSettings.CachePath = @"C:\temp\cache_1";
            // string cachePath = GlobalContext.GlobalCefSettings.CachePath;
            // requestContextSettings.CachePath

            // string upworkStartUrl = "www.google.com"; // "https://www.upwork.com";
            // string upworkStartUrl = "https://www.upwork.com";

            browser = new ChromiumWebBrowser();
            // browser = new ChromiumWebBrowser(url, requestContextSettings.);

            if (requestContextSettings != null)
                browser.RequestContext = new RequestContext(requestContextSettings);
            // projectSearchTabPage.SuspendLayout();

            browser.DownloadHandler = new DownloadHandler();

            ((DownloadHandler)browser.DownloadHandler).OnBeforeDownloadFired += BrowserTimerExportUserControl_OnBeforeDownloadFired;
            ((DownloadHandler)browser.DownloadHandler).OnDownloadUpdatedFired += BrowserUserControl_OnDownloadUpdatedFired;

            // this.Controls.Add(browser);
            panel1.Controls.Add(browser);

            browser.Dock = DockStyle.Fill;

            // projectSearchTabPage.ResumeLayout();

            // projectSearchTabPage.Refresh();

            //browser.LoadingStateChanged += Browser_LoadingStateChanged;
            browser.FrameLoadEnd += Browser_FrameLoadEnd;

            //browser.IsBrowserInitializedChanged += Browser_IsBrowserInitializedChanged;

            // browser.RequestHandler = new CustomRequestHandler();

            //browser.Show();
            //browser.PerformLayout();
            this.PerformLayout();
            this.Invalidate();
            this.Refresh();
            //browser.Invalidate();
            //browser.Refresh();

            // LoadMFIFCPage();

            browser.Load(url);

            browser.Dock = DockStyle.Fill;

            browser.TitleChanged += Browser_TitleChanged;

            InitAutoDownloadTimer(url);
        }

        private void BrowserTimerExportUserControl_OnBeforeDownloadFired(object sender, DownloadItem e)
        {
            string fileSuffix = DateTime.Now.ToString("_yyyyMMdd_hhmmss_fff") + ".csv";
            
            e.FullPath = e.FullPath.Replace(".csv", fileSuffix);            
            e.SuggestedFileName = e.SuggestedFileName.Replace(".csv", fileSuffix);
        }

        void InitAutoDownloadTimer(string loadedUrl)
        {
            // https://relay.amazon.co.uk/tours/in-transit
            
            //--?
            if (((loadedUrl.IndexOf("in-transit", StringComparison.OrdinalIgnoreCase) >= 0) ||
                (loadedUrl.IndexOf("history", StringComparison.OrdinalIgnoreCase) >= 0) ||
                (loadedUrl.IndexOf("upcoming", StringComparison.OrdinalIgnoreCase) >= 0)
                 ) &&
                loadedUrl.IndexOf("relay.amazon", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                if (minRandomIntervalMinutes > 0 &&
                    maxRandomIntervalMinutes > 0 &&
                    minRandomIntervalMinutes < maxRandomIntervalMinutes)
                {
                    if (autoDownloadIntervalTask != null)
                    {
                        ts.Cancel();
                        // aut
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
                else
                {
                    GlobalContext.Log("Auto download with random interval not started because of the configured values - interval between {0} and {1} minutes", minRandomIntervalMinutes, maxRandomIntervalMinutes);
                }
            }
        }

        Task autoDownloadIntervalTask = null;
        CancellationTokenSource ts;

        void StartAutoDownloadInterval(bool first)
        {
            if (first)
            {
                int minRandomInterval = minRandomIntervalMinutes * 60 * 1000;
                int maxRandomInterval = maxRandomIntervalMinutes * 60 * 1000;

                int delayBase = minRandomInterval;
                int addedRandom = maxRandomInterval - minRandomInterval;

                int waitPeriodSec = (int)(delayBase + (new Random(DateTime.Now.Millisecond)).NextDouble() * addedRandom);

                TimeSpan waitPeriod = TimeSpan.FromMilliseconds(waitPeriodSec);

                if (UpdateAutoDownloadStatus != null)
                    UpdateAutoDownloadStatus(this, new UpdateAutoDownloadIntervalStatusEventArgs(String.Format("Waiting {0:00} seconds before downloading export file.", waitPeriod.TotalSeconds)));

                GlobalContext.Log("Auto download with random interval - waiting {0} s", waitPeriod.TotalSeconds);
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

                if (ts.IsCancellationRequested)
                    return;

                if (UpdateAutoDownloadStatus != null)
                    UpdateAutoDownloadStatus(this, new UpdateAutoDownloadIntervalStatusEventArgs(String.Format("Last export made at {0}. Expoting again at {1},  in {2:00} s", DateTime.Now.ToString("HH:mm:ss"), DateTime.Now.Add(waitPeriod).ToString("HH:mm:ss"), waitPeriod.TotalSeconds)));

                GlobalContext.Log("Auto download with random interval - waiting {0} s", waitPeriod.TotalSeconds);
                Thread.Sleep(waitPeriod);

                StartAutoDownloadInterval(false);
            }
        }

        private void BrowserUserControl_OnDownloadUpdatedFired(object sender, DownloadItem e)
        {
            if (e.IsComplete)
            {
                string uploadURL = "";

                try
                {
                    uploadURL = GlobalContext.SerializedConfiguration.ApiBaseURL + GlobalContext.SerializedConfiguration.FileUploadURL;
                    // string uploadURL = "http://167.86.94.125:52031/api/auth2/external/upload";

                   
                    GlobalContext.Log("Upload URL=\"{0}\"", uploadURL);

                    /*
                    string responseText = "";

                    using (WebClient client = new WebClient())
                    {
                        // client.UploadFile(uploadURL, e.FullPath);

                        byte[] rawResponse = client.UploadFile(uploadURL, "POST", e.FullPath);
                        
                        responseText = System.Text.Encoding.ASCII.GetString(rawResponse);
                    }
                    */
                    
                    // e.FullPath = e.FullPath.Replace(".csv", DateTime.Now.ToString("_yyyyMMdd_hhmmss_fff") + ".csv");

                    string fileName = e.SuggestedFileName;

                    // if (string.IsNullOrEmpty(fileName))
                        fileName = Path.GetFileName(e.FullPath);

                    
                    
                    // Task<Stream> responseStream = Upload(uploadURL, File.ReadAllBytes(e.FullPath), fileName);

                    string responseText = null;

                    string csvFileContents = File.ReadAllText(e.FullPath);                    
                    csvFileContents = csvFileContents.Replace(",Operator ID,Spot Work", ",Operator ID,Spot Work,ColBC,COlBD");

                    // niggere, hai sa puscam o bere, lasa prostiile ca oricum nu stii ce faci acolo

                    byte[] bytes = Encoding.UTF8.GetBytes(csvFileContents); // byte[] bytes = Encoding.ASCII.GetBytes();

                    HttpContent bytesContent = new ByteArrayContent(bytes);
                    // HttpContent bytesContent = new ByteArrayContent(File.ReadAllBytes(e.FullPath));                    

                    using (var client = new HttpClient())
                    using (var formData = new MultipartFormDataContent())
                    {
                        //formData.Add(stringContent, "param1", "param1");
                        //formData.Add(fileStreamContent, "file1", "file1");
                        formData.Add(bytesContent, "files", fileName);

                        var response = client.PostAsync(uploadURL, formData).Result;

                        // var response = await client.PostAsync(uploadURL, formData);

                        if (!response.IsSuccessStatusCode)
                        {
                            
                        }

                        Task<Stream> tsk = response.Content.ReadAsStreamAsync();
                        // return await response.Content.ReadAsStreamAsync();

                        StreamReader sr = new StreamReader(tsk.Result);

                        responseText = sr.ReadToEnd();

                        LogResponse(responseText);

                        response.EnsureSuccessStatusCode();

                        FileUploadFinished?.Invoke(this, new FileUploadFinishedEventArgs(fileName));

                        // new Thread(() => MessageBox.Show("File '" + fileName + "' was downloaded.", GlobalContext.ApplicationTitle)).Start();
                        GlobalContext.Log("File '" + fileName + "' was downloaded.");
                    }

                    // responseStream.RunSynchronously();                  

                    // responseStream.ToString();                    


                    /*
                    var client = new RestClient(uploadURL);

                    client.Timeout = -1;

                    var request = new RestRequest(uploadURL, Method.POST);
                    request.Method = Method.POST; // request.Method = Method.Post;

                    request.AlwaysMultipartFormData = true;
                    request.AddHeader("Connection", "keep-alive");
                    request.AddHeader("host", "dlg1prod.web.app");
                    request.AddHeader("Accept", @"text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,* / *;q=0.8");
                    request.AddHeader("Accept-Language", "en-US,en;q=0.5");
                    request.AddHeader("Accept-Encoding", "gzip, deflate, br");
                    request.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:107.0) Gecko/20100101 Firefox/107.0");

                    request.AddHeader("Content-Type", "multipart/form-data");

                    // request.AddFile("files", @"C:\Users\X\source\repos\AmazonDeliveryPlanner\AmazonDeliveryPlanner\doc\Trips 51.csv");
                    request.AddFile("files", e.FullPath);

                    
                    // request.AddHeader("", "");

                    IRestResponse response = client.Execute(request);
                    

                    if (GlobalContext.SerializedConfiguration.Debug)
                    {
                        try
                        {
                            var responseText = response.Content;

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

                        }
                    }

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        string fileName = e.SuggestedFileName;

                        if (string.IsNullOrEmpty(fileName))
                            fileName = Path.GetFileName(e.FullPath);

                        FileUploadFinished?.Invoke(this, new FileUploadFinishedEventArgs(fileName));
                    }
                    else
                    {
                        if (response.ErrorException != null)
                            throw new Exception("exception during upload", response.ErrorException);

                        if (!string.IsNullOrEmpty(response.ErrorMessage))
                            throw new Exception("exception during upload '" + response.ErrorMessage + "'");

                        throw new Exception("exception during upload: '" + response.StatusCode + " " + response.StatusDescription + "'");
                    }
                    // response.ThrowIfError();
                    */
                }
                catch (Exception ex)
                {
                    // webclient
                    //using (Stream stream = (ex as System.Net.WebException).Response.GetResponseStream())
                    //{
                    //    StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                    //    String responseString = reader.ReadToEnd();

                    //    MessageBox.Show("Exception download file: " + ex.Message + " " + responseString);
                    //}

                    GlobalContext.Log($"Exception uploading the file to ${uploadURL}: ${ex.Message}");
                    // MessageBox.Show($"Exception uploading the file to ${uploadURL}: ${ex.Message}");
                    new Thread(() => MessageBox.Show($"Exception uploading the file to ${uploadURL}: ${ex.Message}")).Start();
                }
            }
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

        private void Browser_TitleChanged(object sender, TitleChangedEventArgs e)
        {
            // set parent container tab tabpage title
            /*
            System.Action sa = (System.Action)(() =>
            {
                ((TabPage)this.Parent).Text = e.Title.Length >= 28 ? e.Title.Substring(0, 28) + "…" : e.Title;
            });

            if (this.InvokeRequired)
                this.Invoke(sa);
            else
                sa();
            */
        }

        private void Browser_LoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            // throw new NotImplementedException();
        }

        private void Browser_IsBrowserInitializedChanged(object sender, EventArgs e)
        {
            //if (GlobalContext.ShowDevTools)            //    browser.ShowDevTools();
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
                        string email = GlobalContext.SerializedConfiguration.RelayCredentialsEmail;
                        string pass = GlobalContext.SerializedConfiguration.RelayCredentialsPass;

                        string jsSource1 = string.Format(
                            "(function () {{ document.getElementById('ap_email').value = '{0}'; document.getElementById('ap_password').value = '{1}'; }} )(); ",
                            email,
                            pass
                        );

                        // ExecuteJavaScript(browser, jscode);

                        JavascriptResponse response = await browser.GetMainFrame().EvaluateScriptAsync(jsSource1);

                        // bool result = (bool)response.Result;
                    }

                    //System.Action sa = (System.Action)(() =>                    //{
                    //    urlTextBox.Text = e.Url;
                    //    // urlTextBox.Text = 
                    //    if (((TabPage)this.Parent).Text == "_____________")
                    //        ((TabPage)this.Parent).Text = MainForm.GetUrlTabPageName(e.Url);
                    //});

                    //if (this.InvokeRequired)
                    //    this.Invoke(sa);
                    //else
                    //    sa();

                    /*
                    this.Invoke((MethodInvoker)delegate {
                        // urlTextBox.Text = e.Url;
                        // urlTextBox.Text = 
                        //if (((TabPage)this.Parent).Text == "_____________")
                        //    ((TabPage)this.Parent).Text = MainForm.GetUrlTabPageName(e.Url);
                    });
                    */
                }
            }
            catch (Exception ex)
            {
                GlobalContext.Log("Exception = " + ex.Message); //--
            }
        }

        private void Browser_ConsoleMessage(object sender, ConsoleMessageEventArgs e)
        {
            // throw new NotImplementedException();
        }

        private void Browser_BrowserInitialized(object sender, EventArgs e)
        {
            
            // (sender as CefSharp.OffScreen.ChromiumWebBrowser).Load("");
        }

        private void Browser_MouseMove(object sender, MouseEventArgs e)
        {
            // DoMouseClick((uint)e.X, (uint)e.Y);
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

        public void DoMouseClick(uint X, uint Y)
        {
            //Call the imported function with the cursor's current position
            // uint X = (uint)Cursor.Position.X;
            // uint Y = (uint)Cursor.Position.Y;

            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, X, Y, 0, 0);
        }

        private void UserControl1_Load(object sender, EventArgs e)
        {
            //. InitBrowser();
        }

        protected override void OnLoad(EventArgs e)
        {
            // InitBrowser();

            // Call the base class OnLoad to ensure any delegate event handlers are still callled
            base.OnLoad(e);
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

        //private void loadUrlButton_Click(object sender, EventArgs e)
        //{
        //    browser.Load(urlTextBox.Text);
        //    InitAutoDownloadTimer(urlTextBox.Text);
        //}

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

        //void InitPanel2Browser()
        //{
        //    // !
        //    // System.AccessViolationException: 'Attempted to read or write protected memory. This is often an indication that other memory is corrupt.'
        //    //GlobalContext.GlobalCefSettings.CachePath = @"C:\temp\cache_1";
        //    // string cachePath = GlobalContext.GlobalCefSettings.CachePath;
        //    // requestContextSettings.CachePath

        //    // string upworkStartUrl = "www.google.com"; // "https://www.upwork.com";
        //    // string upworkStartUrl = "https://www.upwork.com";

        //    ChromiumWebBrowser browser2 = new ChromiumWebBrowser();
        //    // browser = new ChromiumWebBrowser(url, requestContextSettings.);

        //    if (requestContextSettings != null)
        //        browser2.RequestContext = new RequestContext(requestContextSettings);
        //    // projectSearchTabPage.SuspendLayout();

        //    // browser2.DownloadHandler = new DownloadHandler();

        //    // ((DownloadHandler)browser.DownloadHandler).OnDownloadUpdatedFired += BrowserUserControl_OnDownloadUpdatedFired;

        //    // this.Controls.Add(browser);
        //    // splitContainer1.Panel2.Controls.Add(browser2);

        //    browser2.Dock = DockStyle.Fill;

        //    // projectSearchTabPage.ResumeLayout();

        //    // projectSearchTabPage.Refresh();

        //    // browser.LoadingStateChanged += Browser_LoadingStateChanged;
        //    // browser2.FrameLoadEnd += Browser_FrameLoadEnd;

        //    //browser.IsBrowserInitializedChanged += Browser_IsBrowserInitializedChanged;

        //    // browser.RequestHandler = new CustomRequestHandler();

        //    //browser.Show();
        //    //browser.PerformLayout();
        //    this.PerformLayout();
        //    this.Invalidate();
        //    this.Refresh();
        //    //browser.Invalidate();
        //    //browser.Refresh();

        //    // LoadMFIFCPage();

        //    // string panel2URL = string.Format("https://dlg1.app/planning-overview/{0}/info", driverId);

        //    if (string.IsNullOrWhiteSpace(GlobalContext.SerializedConfiguration.PlanningOverviewURL))
        //    {
        //        GlobalContext.Log("Error: planning_overview_url value not set in configuration file.");
        //        MessageBox.Show("planning_overview_url value not set in configuration file.", GlobalContext.ApplicationTitle);
        //        return;
        //    }
            
        //    // ex.: http://dlg1.app/planning-overview/{user_id}/info
        //    string panel2URL = GlobalContext.SerializedConfiguration.AdminURL 
        //        + GlobalContext.SerializedConfiguration.PlanningOverviewURL.Replace("{user_id}", driverId.ToString()) 
        //        + "/" + GlobalContext.LoggedInPlanner.token;

        //    browser2.Load(panel2URL);

        //    GlobalContext.Log("Planning Overview Url is set to:  '{0}'", panel2URL);

        //    browser2.Dock = DockStyle.Fill;            
        //}

        async void ClickExportTripsFile()
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
            }
        }

        private void downloadTripsButton_Click(object sender, EventArgs e)
        {
            ClickExportTripsFile();
        }
    }
}
