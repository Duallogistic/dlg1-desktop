using CefSharp;
using CefSharp.WinForms;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using AmazonDeliveryPlanner.Properties;
using AmazonDeliveryPlanner.API;
using System.Net;
using System.Security.Policy;
using static System.Net.WebRequestMethods;
using Newtonsoft.Json;
using File = System.IO.File;
using System.Threading;
using CefSharp.Handler;
// using System.Runtime.InteropServices;

namespace AmazonDeliveryPlanner
{
    public partial class MainForm : Form
    {
        ChromiumWebBrowser adminBrowser;
        ChromiumWebBrowser driversPanelBrowser;
        static string configurationFilePath;

        public MainForm()
        {
            InitializeComponent();

            GlobalContext.ApplicationTitle = "Amazon Relay Delivery Planner";
            GlobalContext.MainWindow = this;
            this.Text = GlobalContext.ApplicationTitle;

            try
            {
                Init();
                InitAdminBrowser();
                InitDriversPanelBrowser();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception on init: " + System.Environment.NewLine +
                    ex.Message + System.Environment.NewLine +
                    ex.StackTrace + System.Environment.NewLine +
                    ex.TargetSite + System.Environment.NewLine +
                    ex.Source + System.Environment.NewLine
                    );
                
                Application.Exit();
            }

            driversPanel.Visible = false;
        }

        public static void InitializeCEF()
        {
            GlobalContext.Log("Director aplicatie: {0}", Utilities.GetApplicationPath());

            if (Cef.IsInitialized)
            {
                GlobalContext.Log("CEF a fost deja initializat");
                return;
            }

            if (GlobalContext.GlobalCefSettings != null)
            {
                GlobalContext.Log("exista deja o instanta CEFSettings");
                return;
            }

            string cachePath = Path.Combine(Utilities.GetApplicationPath(), "cachedirs\\c1");

            cachePath = "";

            //if (!Directory.Exists(cachePath))
            //    Directory.CreateDirectory(cachePath);

            CefSettings cfsettings = new CefSettings();

            // cfsettings.UserAgent = GlobalContext.UserAgent;
            // cfsettings.CachePath = cachePath;

            // set this to LogSeverity.Disable to avoid logging to 'debug.log' file and generating a big file
            cfsettings.LogSeverity = LogSeverity.Disable;
            // cfsettings.PersistSessionCookies = ;

            // CefSharp.Cef.Initialize(cfsettings);
            Cef.Initialize(cfsettings, performDependencyCheck: true, browserProcessHandler: null);

            // GlobalContext.GlobalCefSettings = cfsettings;

            GlobalContext.Log("Proces CEF initializat", cachePath);
            // GlobalContext.Log("Proces CEF initializat; cachePath={0}", cachePath);
            //browser.BrowserSettings.ApplicationCache = CefSharp.CefState.Disabled;
            //Cef.Initialize(cfsettings);

            // GlobalContext.ShowDevTools = false;
        }

        void Init()
        {
            try
            {
                if (!Directory.Exists(GetFileStoragePath()))
                    Directory.CreateDirectory(GetFileStoragePath());
            }
            catch (Exception)
            {
            }

            //settingsFilePath = Utilities.GetApplicationPath() + Path.DirectorySeparatorChar + GlobalContext.OptionsFile;
            //settingsFilePath = Utilities.GetUserApplicationPath() + Path.DirectorySeparatorChar + GlobalContext.OptionsFile;
            configurationFilePath = GetFileStoragePath() + Path.DirectorySeparatorChar + GlobalContext.ConfigurationFileName;

            if (File.Exists(configurationFilePath))
                LoadConfiguration();
                // GlobalContext.SerializedConfiguration = (SerializedConfiguration)Utilities.LoadXML(configurationFilePath, typeof(SerializedConfiguration));
            else
            {
                // SerializedConfiguration conf = SerializedConfiguration.GetDefaultOptions();
                SerializedConfiguration conf = new SerializedConfiguration();

                GlobalContext.SerializedConfiguration = conf;

                SaveConfiguration();
                // Utilities.SaveXML(configurationFilePath, GlobalContext.SerializedConfiguration);
            }

            try
            {
                if (!Directory.Exists(GlobalContext.SerializedConfiguration.DownloadDirectoryPath))
                    Directory.CreateDirectory(GlobalContext.SerializedConfiguration.DownloadDirectoryPath);
            }
            catch (Exception)
            {
            }

            // using System.Net;
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            // Use SecurityProtocolType.Ssl3 if needed for compatibility reasons
            // otherwise we get {"The request was aborted: Could not create SSL/TLS secure channel."}

            UpdateDriverList();

            // LoadScripts();

            InitializeCEF();

            // InitBrowser();

            //GlobalContext.Urls = new List<string>();

            //// test
            //GlobalContext.Urls = new List<string>()
            //{
            //    "https://relay.amazon.com/",
            //    // "http://localhost",
            //    // "http://localhost/test_cookie/test_set_cookie.php",
            //    // "http://localhost/test_cookie/test_show_cookie.php"
            //};

            if (!GlobalContext.SerializedConfiguration.Debug)
                mainTabControl.TabPages.Remove(loggingTabPage);
        }

        #region logging

        int logCounter = 0;

        private bool logToScreen = true;

        // Constants for extern calls to various scrollbar functions
        private const int SB_HORZ = 0x0;
        private const int SB_VERT = 0x1;
        private const int WM_HSCROLL = 0x114;
        private const int WM_VSCROLL = 0x115;
        private const int SB_THUMBPOSITION = 4;
        private const int SB_BOTTOM = 7;
        private const int SB_OFFSET = 13;

        [DllImport("user32.dll")]
        static extern int SetScrollPos(IntPtr hWnd, int nBar, int nPos, bool bRedraw);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int GetScrollPos(IntPtr hWnd, int nBar);
        [DllImport("user32.dll")]
        private static extern bool PostMessageA(IntPtr hWnd, int nBar, int wParam, int lParam);
        [DllImport("user32.dll")]
        static extern bool GetScrollRange(IntPtr hWnd, int nBar, out int lpMinPos, out int lpMaxPos);


        private delegate void SetTextCallback(string text);

        public void Output(string text, params object[] args)
        {
            string msg = string.Format(text, args);
            Output(msg);
        }

        public void Output(string text)
        {
            if (this.logTextBox.InvokeRequired)
            {
                SetTextCallback stc = new SetTextCallback(Output);
                try
                {
                    if (!this.IsDisposed)
                        this.Invoke(stc, new object[] { text });
                }
                catch (ObjectDisposedException)
                {
                }
            }
            else
            {
                if (GlobalContext.SerializedConfiguration.Debug)
                {
                    try
                    {
                        //if (text == "x")
                        //{
                        //    UpdateProgress();
                        //    return;
                        //}

                        if (autoScrollCheckBox.Checked)
                        {
                            bool bottomFlag = false;
                            int VSmin;
                            int VSmax;
                            int sbOffset;
                            int savedVpos;

                            // Win32 magic to keep the textbox scrolling to the newest append to the textbox unless
                            // the user has moved the scrollbox up
                            sbOffset = (int)((this.logTextBox.ClientSize.Height - SystemInformation.HorizontalScrollBarHeight) / (this.logTextBox.Font.Height));
                            savedVpos = GetScrollPos(this.logTextBox.Handle, SB_VERT);
                            GetScrollRange(this.logTextBox.Handle, SB_VERT, out VSmin, out VSmax);
                            if (savedVpos >= (VSmax - sbOffset - 1))
                                bottomFlag = true;

                            this.logTextBox.AppendText("[" + DateTime.Now.ToString("dd HH:mm:ss.fff") + "] " + text + "\r\n");

                            if (bottomFlag)
                            {
                                GetScrollRange(this.logTextBox.Handle, SB_VERT, out VSmin, out VSmax);
                                savedVpos = VSmax - sbOffset;
                                bottomFlag = false;
                            }

                            SetScrollPos(this.logTextBox.Handle, SB_VERT, savedVpos, true);
                            PostMessageA(this.logTextBox.Handle, WM_VSCROLL, SB_THUMBPOSITION + 0x10000 * savedVpos, 0);
                        }
                        else
                            logTextBox.AppendText("[" + DateTime.Now.ToString("dd HH:mm:ss.fff") + "] " + text + "\r\n");


                        //if (autoScrollCheckBox.Checked)
                        //{
                        //    logTextBox.Select()
                        //    logTextBox.SelectionStart = logTextBox.Text.Length;
                        //    logTextBox.ScrollToCaret();
                        //}

                        if (logCounter++ > 9000)
                        {
                            logTextBox.Clear();
                            logCounter = 0;
                        }
                    }
                    catch (ObjectDisposedException //ex
                )
                    {

                    }
                }
        }
            
        }

        #endregion

        int sessionCount = 0;

        Dictionary<long, bool> openTabDrivers = new Dictionary<long, bool>();

        void AddSessionTab()
        {
            if (selectedDriver == null)
            {
                MessageBox.Show("No driver selected", GlobalContext.ApplicationTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            foreach (TabPage page in tabControl.TabPages)
            {
                if (((DriverSessionObject)page.Tag).DriverId == selectedDriver.driver_id)
                { 
                    tabControl.SelectedTab = page;
                    return;
                }
            }

            sessionCount++;

            TabPage stp = new TabPage();

            tabControl.SuspendLayout();

            stp.SuspendLayout();

            tabControl.TabPages.Add(stp);


            stp.Name = "PageSesiune" + sessionCount;
            stp.Text = selectedDriver.ToString();

            DriverSessionObject driverSessionObject = new DriverSessionObject() { 
                DriverId = selectedDriver.driver_id                
            };

            stp.Tag = driverSessionObject; // the object changes on resfreshing data from server as new objects are created for the same entity
            // stp.Text = "Sesiune " + sessionCount;
            // tp.Tag = bUC;

            // bUC.OnFinishedQuery += MFbUC_OnFinishedQuery;

            DriverUserControl driverUC = new DriverUserControl(selectedDriver);

            driverUC.SuspendLayout();

            // driverUC.Dock = System.Windows.Forms.DockStyle.Fill;
            driverUC.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            driverUC.Location = new System.Drawing.Point(3, 0);
            driverUC.Name = "TCSesiune_DriverUC_" + sessionCount;

            // driverUC.Tag = selectedDriver;
            driverUC.SessionClosed += DriverUC_SessionClosed;
            driverUC.OpenURL += DriverUC_OpenURL;

            driverUC.ResumeLayout();

            stp.Controls.Add(driverUC);


            //stp.ResumeLayout();
            //stp.Refresh();
            //stp.Invalidate();
            //tabControl.ResumeLayout();
            //// tabControl.SelectTab(stp);
            //tabControl.Refresh();
            //tabControl.Invalidate();
            //stp.SuspendLayout();
            //tabControl.SuspendLayout();


            TabControl urlsTabControl = new System.Windows.Forms.TabControl();

            //urlsTabControl.SuspendLayout();
            //stp.Controls.Add(urlsTabControl);

            // urlsTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            // urlsTabControl.Dock = System.Windows.Forms.DockStyle.None;

            urlsTabControl.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom | AnchorStyles.Top;
            urlsTabControl.Location = new System.Drawing.Point(3, driverUC.Height + 0 + 5);
            urlsTabControl.Height = stp.Height - driverUC.Height + 0 - 5;
            urlsTabControl.Width = stp.Width;
            urlsTabControl.Name = "TCSesiune" + sessionCount;

            // urlsTabControl.Name = "tabControl";
            // urlsTabControl.Dock = System.Windows.Forms.DockStyle.Bottom;


            // bpipbUC.Name = "x";
            // this.tabControl1.Size = new System.Drawing.Size(852, 586);
            // urlsTabControl.TabIndex = 1;

            RequestContextSettings requestContextSettings = new RequestContextSettings();

            requestContextSettings.PersistSessionCookies = !false;
            requestContextSettings.PersistUserPreferences = !false;

            string cachePath = Path.Combine(Utilities.GetApplicationPath(), "cachedirs", "TCSesiune_" + selectedDriver.driver_id); // "TCSesiune" + sessionCount

            if (!Directory.Exists(cachePath))
                Directory.CreateDirectory(cachePath);

            requestContextSettings.CachePath = cachePath;

            // if (false)
            foreach (string url in GlobalContext.SerializedConfiguration.DefaultTabs /*GlobalContext.Urls*/)
            {
                TabPage urlTabPage = new System.Windows.Forms.TabPage();

                // 
                // tabControl
                // 
                urlsTabControl.Controls.Add(urlTabPage);
                // urlsTabControl.Location = new System.Drawing.Point(4, 41);
                // urlsTabControl.SelectedIndex = 0;
                // urlsTabControl.Size = new System.Drawing.Size(1079, 687);
                // urlsTabControl.TabIndex = 0;
                // 
                // tabPage1
                // 
                urlTabPage.Location = new System.Drawing.Point(4, 24 /*+ driverUC.Height*/);
                // urlTabPage.Name = "tabPage1";
                urlTabPage.Padding = new System.Windows.Forms.Padding(3);
                urlTabPage.Size = new System.Drawing.Size(1071, 659 /*- driverUC.Height*/);
                urlTabPage.TabIndex = 0;
                urlTabPage.Text = GetUrlTabPageName(url);
                urlTabPage.UseVisualStyleBackColor = true;

                // urlTabPage.BackColor = Color.Green;

                driverSessionObject.ReqContextSettings = requestContextSettings;

                BrowserUserControl bUC = new BrowserUserControl(url, requestContextSettings, selectedDriver.driver_id);

                {
                    // mfbUC.Cif = cif;

                    bUC.SuspendLayout();

                    urlTabPage.Controls.Add(bUC);

                    urlTabPage.Tag = bUC;

                    // bUC.OnFinishedQuery += MFbUC_OnFinishedQuery;

                    bUC.Dock = System.Windows.Forms.DockStyle.Fill;
                    bUC.Location = new System.Drawing.Point(3, 0);
                    // bpipbUC.Name = "x";
                    // this.tabControl1.Size = new System.Drawing.Size(852, 586);
                    bUC.TabIndex = 1;
                    bUC.Close += BUC_Close;

                    bUC.ResumeLayout(!false);

                    bUC.PerformLayout();

                    bUC.Tag = driverUC;
                    bUC.FileUploadFinished += BUC_FileUploadFinished;
                }

                urlTabPage.ResumeLayout();
            }

            urlsTabControl.SelectedIndex = 0;

            urlsTabControl.SuspendLayout();
            stp.Controls.Add(urlsTabControl);

            stp.ResumeLayout();

            urlsTabControl.ResumeLayout(false);
            tabControl.ResumeLayout();

            tabControl.SelectTab(stp);

            tabControl.Refresh();
            tabControl.Invalidate();

            // urlsTabControl.Location = new System.Drawing.Point(3, driverUC.Height + 0 + 5);

            // urlsTabControl.Location.Y = driverUC.Height + 5;
            //if (!this.Visible /*this.WindowState == FormWindowState.Minimized*/)
            //{
            //    this.Show();
            //    this.Hide();
            //}

            openTabDrivers[selectedDriver.driver_id] = true;
        }

        private void BUC_FileUploadFinished(object sender, BrowserUserControl.FileUploadFinishedEventArgs e)
        {
            System.Action sa = (System.Action)(() =>
            {
                ((sender as BrowserUserControl).Tag as DriverUserControl).UpdateUploadLabel("uploaded file " + e.FileName);
            });

            if (this.InvokeRequired)
                this.Invoke(sa);
            else
                sa();
            
        }

        private void DriverUC_OpenURL(object sender, OpenURLEventArgs e)
        {
            foreach (TabPage page in tabControl.TabPages)
            {
                if (page.Controls.Contains((Control)sender))
                {
                    foreach (Control c in page.Controls)
                        if (c is TabControl)
                        {
                            TabControl urlsTabControl = c as TabControl;


                            #region GMaps open test
                            bool isGMapsOpen = false;

                            foreach (TabPage tp in urlsTabControl.TabPages) // 
                                if ((tp.Tag as BrowserUserControl).Url.Contains("google.com/maps"))
                                    isGMapsOpen = true;

                            if (isGMapsOpen && e.URL.Contains("google.com/maps"))
                                break; // if there's one tap page already open with the google maps location, don't open a second one
                            #endregion

                            TabPage urlTabPage = new System.Windows.Forms.TabPage();

                            // 
                            // tabControl
                            // 
                            urlsTabControl.Controls.Add(urlTabPage);
                            // urlsTabControl.Location = new System.Drawing.Point(4, 41);
                            // urlsTabControl.SelectedIndex = 0;
                            // urlsTabControl.Size = new System.Drawing.Size(1079, 687);
                            // urlsTabControl.TabIndex = 0;
                            // 
                            // tabPage1
                            // 
                            urlTabPage.Location = new System.Drawing.Point(4, 24 /*+ driverUC.Height*/);
                            // urlTabPage.Name = "tabPage1";
                            urlTabPage.Padding = new System.Windows.Forms.Padding(3);
                            urlTabPage.Size = new System.Drawing.Size(1071, 659 /*- driverUC.Height*/);
                            urlTabPage.TabIndex = 0;
                            urlTabPage.Text = GetUrlTabPageName(e.URL);
                            urlTabPage.UseVisualStyleBackColor = true;

                            // urlTabPage.BackColor = Color.Green;
                            
                            BrowserUserControl bUC = new BrowserUserControl(e.URL, ((DriverSessionObject)page.Tag).ReqContextSettings, ((DriverSessionObject)page.Tag).DriverId);

                            {
                                // mfbUC.Cif = cif;

                                bUC.SuspendLayout();

                                urlTabPage.Controls.Add(bUC);

                                urlTabPage.Tag = bUC;

                                // bUC.OnFinishedQuery += MFbUC_OnFinishedQuery;

                                bUC.Dock = System.Windows.Forms.DockStyle.Fill;
                                bUC.Location = new System.Drawing.Point(3, 0);
                                // bpipbUC.Name = "x";
                                // this.tabControl1.Size = new System.Drawing.Size(852, 586);
                                bUC.TabIndex = 1;
                                bUC.Close += BUC_Close;

                                bUC.ResumeLayout(!false);

                                bUC.PerformLayout();
                            }

                            urlTabPage.ResumeLayout();

                            urlsTabControl.SelectedTab = urlTabPage;
                        }
                }
            }
        }

        private void BUC_Close(object sender, EventArgs e)
        {
            foreach (TabPage page in tabControl.TabPages)
            {
                foreach (Control c in page.Controls)
                    if (c is TabControl)
                    {
                        TabControl urlsTabControl = c as TabControl;

                        foreach (TabPage tp in urlsTabControl.TabPages) // 
                            if ((tp.Tag as BrowserUserControl) == sender)
                                urlsTabControl.TabPages.Remove(tp);
                    }
            }
        }
        private void DriverUC_SessionClosed(object sender, EventArgs e)
        {
            foreach (TabPage page in tabControl.TabPages)
            {
                if (page.Controls.Contains((Control)sender))
                {
                    tabControl.TabPages.Remove(page);


                    Driver drv = GlobalContext.LastDriverList.drivers.Where(dr => dr.driver_id == ((DriverSessionObject)page.Tag).DriverId).SingleOrDefault();

                    openTabDrivers[drv.driver_id] = false;

                    driverListBox.Refresh();
                }
            }
        }

        private void openSettingsButton_Click(object sender, EventArgs e)
        {
            SettingsForm sForm = new SettingsForm();

            if (sForm.ShowDialog() == DialogResult.OK)
            {
                SaveConfiguration();
                // ...
            }
        }

        private void addSessionButton_Click(object sender, EventArgs e)
        {
            AddSessionTab();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        DriverList _driverList;

        void UpdateDriverList()
        {
            int tryCount = 0;
            Exception lastException = null;

            do
            {
                try
                {
                    tryCount++;

                    DriverList driverList = DriversAPI.GetDrivers();

                    GlobalContext.LastDriverList = driverList;

                    _driverList = driverList;

                    UpdateDriverListControl();

                    return;
                }
                catch (Exception ex)
                {
                    GlobalContext.Log("Exception getting drivers from web service: '{0}'", ex.Message);
                    lastException = ex;
                }
            } while (tryCount < 4);

            if (lastException != null)
                MessageBox.Show("Exception loading drivers: " + lastException.Message);
        }

        void UpdateDriverListControl()
        {
            driverListBox.Items.Clear();

            driverListBox.Items.AddRange(
                GlobalContext.LastDriverList.drivers.Where(dr =>
                {
                    if (activeDriversRadioButton.Checked)
                        return dr.is_user_active;
                    else
                        return true;
                }).ToArray()
            );

            // openTabDrivers.Clear();

            foreach (Driver dr in GlobalContext.LastDriverList.drivers)
                if (!openTabDrivers.ContainsKey(dr.driver_id))
                    openTabDrivers.Add(dr.driver_id, false);
        }
        
        private void refreshDriversButton_Click(object sender, EventArgs e)
        {
            UpdateDriverList();
        }

        Driver selectedDriver;

        private void driverListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedDriver = GlobalContext.LastDriverList.drivers.Where(dr => dr.ToString() == driverListBox.SelectedItem.ToString()).SingleOrDefault();
        }

        private void driverListBox_DoubleClick(object sender, EventArgs e)
        {
            AddSessionTab();
        }

        private void driversRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            UpdateDriverList();
        }

        private void driverListBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0)
                return;

            e.DrawBackground();

            Graphics g = e.Graphics;
            
            if (openTabDrivers.ElementAt(e.Index).Value)
                g.FillRectangle(new SolidBrush(Color.LightBlue), e.Bounds);
            else
                g.FillRectangle(new SolidBrush(Color.White), e.Bounds);

            ListBox lb = (ListBox)sender;
            g.DrawString(lb.Items[e.Index].ToString(), e.Font, new SolidBrush(Color.Black), new PointF(e.Bounds.X, e.Bounds.Y));

            e.DrawFocusRectangle();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        public static string GetUrlTabPageName(string url)
        {
            if (url.Contains("google.com/maps"))
                return "Google Maps";
            else
            if (url.Contains("relay.amazon.com"))
                return "Amazon Relay";
            else
                return string.IsNullOrEmpty(url) ? "_____________" : (url.Length >= 20) ? url.Substring(0, 20) : url;
        }

        void InitAdminBrowser()
        {
            // !
            // System.AccessViolationException: 'Attempted to read or write protected memory. This is often an indication that other memory is corrupt.'
            //GlobalContext.GlobalCefSettings.CachePath = @"C:\temp\cache_1";
            // string cachePath = GlobalContext.GlobalCefSettings.CachePath;
            // requestContextSettings.CachePath

            // string upworkStartUrl = "www.google.com"; // "https://www.upwork.com";
            // string upworkStartUrl = "https://www.upwork.com";

            adminBrowser = new ChromiumWebBrowser();
            // browser = new ChromiumWebBrowser(url, requestContextSettings.);

            
            RequestContextSettings requestContextSettings = new RequestContextSettings();

            requestContextSettings.PersistSessionCookies = !false;
            requestContextSettings.PersistUserPreferences = !false;            

            /*string cachePath = Path.Combine(Utilities.GetApplicationPath(), "cachedirs", "sesiune_admin");

            if (!Directory.Exists(cachePath))
                Directory.CreateDirectory(cachePath);

            requestContextSettings.CachePath = cachePath;
            */

            if (requestContextSettings != null)
                adminBrowser.RequestContext = new RequestContext(requestContextSettings);


            // projectSearchTabPage.SuspendLayout();

            // this.adminTabPage.Controls.Add(adminBrowser);
            adminBrowserPanel.Controls.Add(adminBrowser);
            adminBrowser.Dock = DockStyle.Fill;

            // projectSearchTabPage.ResumeLayout();

            // projectSearchTabPage.Refresh();

            //browser.LoadingStateChanged += Browser_LoadingStateChanged;
            adminBrowser.FrameLoadEnd += AdminBrowser_FrameLoadEnd;

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
            
            /* "https://admin.dlg1.app" */
            adminBrowser.Load(GlobalContext.SerializedConfiguration.AdminURL);

            adminBrowser.Dock = DockStyle.Fill;

            //adminBrowser.KeyUp += AdminBrowser_KeyUp;
            //adminBrowser.PreviewKeyDown += AdminBrowser_PreviewKeyDown;

            // adminBrowser.KeyboardHandler.OnKeyEvent(adminBrowser, adminBrowser)
            // adminBrowser.KeyboardHandler = new KeyboardHandler();
            // adminBrowser.KeyboardHandler.
        }

        //private void AdminBrowser_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        //{
        //    throw new NotImplementedException();
        //}

        //private void AdminBrowser_KeyUp(object sender, KeyEventArgs e)
        //{            
        //    if (e.Control && (e.KeyCode == Keys.NumPad1 || e.KeyCode == Keys.D1))
        //        e.Handled = false; // MainForm_KeyUp(sender, e);
        //    else
        //    if (e.Control && (e.KeyCode == Keys.NumPad2 || e.KeyCode == Keys.D2))
        //        e.Handled = false; // MainForm_KeyUp(sender, e);
        //}

        private void MainForm_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {

        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Control && (e.KeyCode == Keys.NumPad1 || e.KeyCode == Keys.D1))
                mainTabControl.SelectedTab = sessionsTabPage;
            else
            if (e.Control && (e.KeyCode == Keys.NumPad2 || e.KeyCode == Keys.D2))
                mainTabControl.SelectedTab = adminTabPage;
        }

        void SaveConfiguration()
        {
            string confJsonString = Newtonsoft.Json.JsonConvert.SerializeObject(GlobalContext.SerializedConfiguration, Formatting.Indented);

            File.WriteAllText(configurationFilePath, confJsonString);
        }

        void LoadConfiguration()
        {
            string confJsonString = File.ReadAllText(configurationFilePath);

            GlobalContext.SerializedConfiguration = Newtonsoft.Json.JsonConvert.DeserializeObject<SerializedConfiguration>(confJsonString);

            if (GlobalContext.SerializedConfiguration.DefaultTabs == null)
                GlobalContext.SerializedConfiguration.DefaultTabs = new string[0];
        }

        // This is the directory in which  the settings, output files and measurements directory are placed
        // Normally it would be the %APPDATA% - User Application Path directory but for portability reasons (to be easily moved between computers keeping settings), the application path can be used
        public static string GetFileStoragePath()
        {
            return Utilities.GetApplicationPath();
            //return Utilities.GetUserApplicationPath();
        }

        private async void AdminBrowser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            GlobalContext.Log("Admin Browser_FrameLoadEnd url={0} frame={1}", e.Url, e.Frame.Name);

            Thread.Sleep(800);

            if (e.Frame.IsMain)
            {
                // var watch = System.Diagnostics.Stopwatch.StartNew();
                // string html = await browser.GetSourceAsync();

                //System.Action sa = (System.Action)(() =>
                //{
                //    urlTextBox.Text = e.Url;
                //});

                //if (this.InvokeRequired)
                //    this.Invoke(sa);
                //else
                //    sa();
                
                this.Invoke((MethodInvoker)delegate { urlTextBox.Text = e.Url; });

                // https://www.amazon.com/ap/signin?openid.return_to=https://relay.amazon.com/&openid.identity=http://specs.openid.net/auth/2.0/identifier_select&openid.assoc_handle=amzn_relay_desktop_us&openid.mode=checkid_setup&openid.claimed_id=http://specs.openid.net/auth/2.0/identifier_select&openid.ns=http://specs.openid.net/auth/2.0&pageId=amzn_relay_desktop_us
                // if (e.Url.IndexOf("login") >= 0)
                {
                    string email = GlobalContext.SerializedConfiguration.AdminCredentialsEmail;
                    string pass = GlobalContext.SerializedConfiguration.AdminCredentialsPass;

                    string email_input_element_type = "email";
                    string pass_input_element_type = "<undefined>";

                    string jsSource1 = string.Format(
                        "(function () {{ " +
                        "var inputs = document.getElementsByTagName('input');\r\n\r\nfor(var i = 0; i < inputs.length; i++) {{\r\n    if(inputs[i].type.toLowerCase() == '{2}') {{\r\n  inputs[i].value = '{0}';       \r\n    }}\r\n if(inputs[i].type.toLowerCase() == '{3}') {{\r\n  inputs[i].value = '{1}';       \r\n    }}\r\n}}" +
                        " }} )(); ",
                        email,
                        pass,
                        email_input_element_type,
                        pass_input_element_type
                    );

                    // ExecuteJavaScript(browser, jscode);

                    JavascriptResponse response = await adminBrowser.GetMainFrame().EvaluateScriptAsync(jsSource1);

                    // bool result = (bool)response.Result;
                }
            }
        }

        private void toggleLeftPanelVisibilityButton_Click(object sender, EventArgs e)
        {
            ToggleLeftPanelVisibility();
        }

        void ToggleLeftPanelVisibility()
        {
            splitContainer1.Panel1Collapsed = !splitContainer1.Panel1Collapsed;
            toggleLeftPanelVisibilityButton.Text = splitContainer1.Panel1Collapsed ? "\u220E \u220E" : "| \u220E";
        }

        private void closeButton_Click(object sender, EventArgs e)
        {

        }

        private void showDevToolsButton_Click(object sender, EventArgs e)
        {
            adminBrowser.ShowDevTools();
        }

        private async void decreaseTextSizeButton_Click(object sender, EventArgs e)
        {
            // adminBrowser.SetZoomLevel(adminBrowser.GetZoomLevelAsync().Result - 0.1);

            var zoom = await adminBrowser.GetZoomLevelAsync();

            adminBrowser.SetZoomLevel(zoom - 0.1);
        }

        private async void increaseTextSizeButton_Click(object sender, EventArgs e)
        {
            // adminBrowser.SetZoomLevel(adminBrowser.GetZoomLevelAsync().Result + 0.1);

            var zoom = await adminBrowser.GetZoomLevelAsync();

            adminBrowser.SetZoomLevel(zoom + 0.1);
        }

        private void goBackButton_Click(object sender, EventArgs e)
        {
            adminBrowser.Back();
        }

        private void refrehPageButton_Click(object sender, EventArgs e)
        {
            adminBrowser.Reload(true);
        }

        private void loadUrlButton_Click(object sender, EventArgs e)
        {
            adminBrowser.Load(urlTextBox.Text);
        }

        private void urlTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                adminBrowser.Load(urlTextBox.Text);
        }

        private void goForwardButton_Click(object sender, EventArgs e)
        {
            adminBrowser.Forward();
        }

        void InitDriversPanelBrowser()
        {
            // !
            // System.AccessViolationException: 'Attempted to read or write protected memory. This is often an indication that other memory is corrupt.'
            //GlobalContext.GlobalCefSettings.CachePath = @"C:\temp\cache_1";
            // string cachePath = GlobalContext.GlobalCefSettings.CachePath;
            // requestContextSettings.CachePath

            // string upworkStartUrl = "www.google.com"; // "https://www.upwork.com";
            // string upworkStartUrl = "https://www.upwork.com";

            driversPanelBrowser = new ChromiumWebBrowser();
            // browser = new ChromiumWebBrowser(url, requestContextSettings.);


            RequestContextSettings requestContextSettings = new RequestContextSettings();

            requestContextSettings.PersistSessionCookies = !false;
            requestContextSettings.PersistUserPreferences = !false;

            /*string cachePath = Path.Combine(Utilities.GetApplicationPath(), "cachedirs", "sesiune_admin");

            if (!Directory.Exists(cachePath))
                Directory.CreateDirectory(cachePath);

            requestContextSettings.CachePath = cachePath;
            */

            if (requestContextSettings != null)
                driversPanelBrowser.RequestContext = new RequestContext(requestContextSettings);


            // projectSearchTabPage.SuspendLayout();

            // this.adminTabPage.Controls.Add(driversPanelBrowser);
            splitContainer1.Panel1.Controls.Add(driversPanelBrowser);
            driversPanelBrowser.Dock = DockStyle.Fill;

            // projectSearchTabPage.ResumeLayout();

            // projectSearchTabPage.Refresh();

            //browser.LoadingStateChanged += Browser_LoadingStateChanged;
            driversPanelBrowser.FrameLoadEnd += DriversPanelBrowser_FrameLoadEnd;

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

            /* "https://admin.dlg1.app" */
            driversPanelBrowser.Load(GlobalContext.SerializedConfiguration.DriverListURL);

            driversPanelBrowser.Dock = DockStyle.Fill;

            //driversPanelBrowser.KeyUp += driversPanelBrowser_KeyUp;
            //driversPanelBrowser.PreviewKeyDown += driversPanelBrowser_PreviewKeyDown;

            // driversPanelBrowser.KeyboardHandler.OnKeyEvent(driversPanelBrowser, driversPanelBrowser)
            // driversPanelBrowser.KeyboardHandler = new KeyboardHandler();
            // driversPanelBrowser.KeyboardHandler.

            refreshDriverListBrowserButton.BringToFront();
        }

        private void DriversPanelBrowser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            // throw new NotImplementedException();
        }

        private void refreshDriverListBrowserButton_Click(object sender, EventArgs e)
        {
            driversPanelBrowser.Reload(true);            
        }

        private void showOpenDriverFormButton_Click(object sender, EventArgs e)
        {
            OpenDriverForm openDriverForm = new OpenDriverForm(_driverList.drivers);

            openDriverForm.StartPosition = FormStartPosition.CenterParent;

            if (openDriverForm.ShowDialog() == DialogResult.OK)
            {
                selectedDriver = openDriverForm.SelectedDriver;
                AddSessionTab();
            }
        }
    }
}