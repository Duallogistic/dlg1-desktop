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

namespace AmazonDeliveryPlanner
{
    public partial class BrowserUserControl : UserControl, IDisposable
    {
        ChromiumWebBrowser browser;

        string url;

        RequestContextSettings requestContextSettings;

        public event EventHandler Close;
        public event EventHandler<FileUploadFinishedEventArgs> FileUploadFinished;

        long driverId;

        public BrowserUserControl(string url, RequestContextSettings requestContextSettings, long driverId)
        {
            this.driverId = driverId;
            this.url = url;
            this.requestContextSettings = requestContextSettings;

            InitializeComponent();            

            InitBrowser();
            InitPanel2Browser();

            // new Task(() => { Thread.Sleep(800); InitBrowser(); }).Start();

            urlTextBox.Text = url;            
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
        }

        private void BrowserUserControl_OnDownloadUpdatedFired(object sender, DownloadItem e)
        {
            if (e.IsComplete)
            {
                try
                {
                    string uploadURL = GlobalContext.SerializedConfiguration.ApiBaseURL + GlobalContext.SerializedConfiguration.FileUploadURL;
                    // string uploadURL = "http://167.86.94.125:52031/api/auth2/external/upload";

                    // uploadURL = "https://qa-srv.dlg1.app/api/auth2/external/upload";


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

                    string fileName = e.SuggestedFileName;

                    if (string.IsNullOrEmpty(fileName))
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

                        MessageBox.Show("Fisierul " + fileName + " a fost descarcat", GlobalContext.ApplicationTitle);
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
                    request.AddHeader("host", "srv.dlg1.app");
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

                    GlobalContext.Log("exception downloading file: " + ex.Message);

                    MessageBox.Show("Exception download file: " + ex.Message);                    
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

                }
            }
        }

        private void Browser_TitleChanged(object sender, TitleChangedEventArgs e)
        {
            System.Action sa = (System.Action)(() =>
            {
                ((TabPage)this.Parent).Text = e.Title.Length >= 28 ? e.Title.Substring(0, 28) + "…" : e.Title;
            });

            if (this.InvokeRequired)
                this.Invoke(sa);
            else
                sa();
        }

        private void Browser_LoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            // throw new NotImplementedException();
        }

        private void Browser_IsBrowserInitializedChanged(object sender, EventArgs e)
        {
            //if (GlobalContext.ShowDevTools)
            //    browser.ShowDevTools();

            //LoadMFIFCPage();
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

                    //System.Action sa = (System.Action)(() =>
                    //{
                    //    urlTextBox.Text = e.Url;
                    //    // urlTextBox.Text = 
                    //    if (((TabPage)this.Parent).Text == "_____________")
                    //        ((TabPage)this.Parent).Text = MainForm.GetUrlTabPageName(e.Url);
                    //});

                    //if (this.InvokeRequired)
                    //    this.Invoke(sa);
                    //else
                    //    sa();

                    this.Invoke((MethodInvoker)delegate {
                        urlTextBox.Text = e.Url;
                        // urlTextBox.Text = 
                        if (((TabPage)this.Parent).Text == "_____________")
                            ((TabPage)this.Parent).Text = MainForm.GetUrlTabPageName(e.Url);
                    });
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
            DoMouseClick((uint)e.X, (uint)e.Y);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);
        // Mouse actions
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;

        public string Url { get => url; /*set => url = value;*/ }

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

        private void loadUrlButton_Click(object sender, EventArgs e)
        {
            browser.Load(urlTextBox.Text);
        }

        private void goBackButton_Click(object sender, EventArgs e)
        {
            browser.Back();
        }

        private void urlTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                browser.Load(urlTextBox.Text);
        }

        public class FileUploadFinishedEventArgs : EventArgs
        {
            public FileUploadFinishedEventArgs(string fileName)
            {
                FileName = fileName;
            }
            
            public string FileName { get; set; }
        }

        private void goForwardButton_Click(object sender, EventArgs e)
        {
            browser.Forward();
        }


        /*
        
        private async Task<System.IO.Stream> Upload(string actionUrl, 
        //string paramString, Stream paramFileStream, 
        byte[] paramFileBytes, string fileName)
        {
            //HttpContent stringContent = new StringContent(paramString);
            //HttpContent fileStreamContent = new StreamContent(paramFileStream);
            HttpContent bytesContent = new ByteArrayContent(paramFileBytes);

            using (var client = new HttpClient())
            using (var formData = new MultipartFormDataContent())
            {
                //formData.Add(stringContent, "param1", "param1");
                //formData.Add(fileStreamContent, "file1", "file1");
                formData.Add(bytesContent, "files", fileName);

                var response = await client.PostAsync(actionUrl, formData);
                
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                return await response.Content.ReadAsStreamAsync();
            }
        }
        */

        void InitPanel2Browser()
        {
            // !
            // System.AccessViolationException: 'Attempted to read or write protected memory. This is often an indication that other memory is corrupt.'
            //GlobalContext.GlobalCefSettings.CachePath = @"C:\temp\cache_1";
            // string cachePath = GlobalContext.GlobalCefSettings.CachePath;
            // requestContextSettings.CachePath

            // string upworkStartUrl = "www.google.com"; // "https://www.upwork.com";
            // string upworkStartUrl = "https://www.upwork.com";

            ChromiumWebBrowser browser2 = new ChromiumWebBrowser();
            // browser = new ChromiumWebBrowser(url, requestContextSettings.);

            if (requestContextSettings != null)
                browser2.RequestContext = new RequestContext(requestContextSettings);
            // projectSearchTabPage.SuspendLayout();

            // browser2.DownloadHandler = new DownloadHandler();

            // ((DownloadHandler)browser.DownloadHandler).OnDownloadUpdatedFired += BrowserUserControl_OnDownloadUpdatedFired;

            // this.Controls.Add(browser);
            splitContainer1.Panel2.Controls.Add(browser2);

            browser2.Dock = DockStyle.Fill;

            // projectSearchTabPage.ResumeLayout();

            // projectSearchTabPage.Refresh();

            // browser.LoadingStateChanged += Browser_LoadingStateChanged;
            // browser2.FrameLoadEnd += Browser_FrameLoadEnd;

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

            // string panel2URL = string.Format("https://dlg1.app/planning-overview/{0}/info", driverId);

            if (string.IsNullOrWhiteSpace(GlobalContext.SerializedConfiguration.PlanningOverviewURL))
            {
                GlobalContext.Log("Error: planning_overview_url value not set in configuration file.");
                MessageBox.Show("planning_overview_url value not set in configuration file.", GlobalContext.ApplicationTitle);
                return;
            }
            
            // ex.: http://dlg1.app/planning-overview/{user_id}/info
            string panel2URL = GlobalContext.SerializedConfiguration.PlanningOverviewURL.Replace("{user_id}", driverId.ToString());
            browser2.Load(panel2URL);

            browser2.Dock = DockStyle.Fill;            
        }
    }
}
