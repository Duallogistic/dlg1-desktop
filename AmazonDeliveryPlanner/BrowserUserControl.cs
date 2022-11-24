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

namespace AmazonDeliveryPlanner
{
    public partial class BrowserUserControl : UserControl, IDisposable
    {
        ChromiumWebBrowser browser;

        string url;

        RequestContextSettings requestContextSettings;

        public BrowserUserControl(string url, RequestContextSettings requestContextSettings)
        {
            this.url = url;
            this.requestContextSettings = requestContextSettings;

            InitializeComponent();            

            InitBrowser();

            // new Task(() => { Thread.Sleep(800); InitBrowser(); }).Start();
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

            this.Controls.Add(browser);
            browser.Dock = DockStyle.Fill;

            // projectSearchTabPage.ResumeLayout();

            // projectSearchTabPage.Refresh();

            //browser.LoadingStateChanged += Browser_LoadingStateChanged;
            //browser.FrameLoadEnd += Browser_FrameLoadEnd;

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
            GlobalContext.Log("Browser_FrameLoadEnd url={0} frame={1}", e.Url, e.Frame.Name);

            if (e.Frame.IsMain)
            {
                // var watch = System.Diagnostics.Stopwatch.StartNew();
                // string html = await browser.GetSourceAsync();
            }
        }

        private void BilantPageBrowser_ConsoleMessage(object sender, ConsoleMessageEventArgs e)
        {
            // throw new NotImplementedException();
        }

        private void BilantPageBrowser_BrowserInitialized(object sender, EventArgs e)
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

        public string Url { get => url; set => url = value; }

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
    }
}
