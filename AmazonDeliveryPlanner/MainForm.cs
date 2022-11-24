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

namespace AmazonDeliveryPlanner
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            GlobalContext.ApplicationTitle = "Amazon Relay Delivery Planner";
            GlobalContext.MainWindow = this;
            this.Text = GlobalContext.ApplicationTitle;

            try
            {
                Init();
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

            if (!Directory.Exists(cachePath))
                Directory.CreateDirectory(cachePath);

            CefSettings cfsettings = new CefSettings();

            cfsettings.UserAgent = GlobalContext.UserAgent;
            // cfsettings.CachePath = cachePath;

            // set this to LogSeverity.Disable to avoid logging to 'debug.log' file and generating a big file
            cfsettings.LogSeverity = LogSeverity.Disable;
            // cfsettings.PersistSessionCookies = ;

            // CefSharp.Cef.Initialize(cfsettings);
            Cef.Initialize(cfsettings, performDependencyCheck: true, browserProcessHandler: null);

            // GlobalContext.GlobalCefSettings = cfsettings;

            GlobalContext.Log("Proces CEF initializat; cachePath={0}", cachePath);
            //browser.BrowserSettings.ApplicationCache = CefSharp.CefState.Disabled;
            //Cef.Initialize(cfsettings);

            // GlobalContext.ShowDevTools = false;

        }

        void Init()
        {
            // using System.Net;
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            // Use SecurityProtocolType.Ssl3 if needed for compatibility reasons
            // otherwise we get {"The request was aborted: Could not create SSL/TLS secure channel."}

            UpdateDriverList();

            // LoadScripts();

            InitializeCEF();

            // InitBrowser();

            GlobalContext.Urls = new List<string>();

            // test
            GlobalContext.Urls = new List<string>()
            {
                "https://relay.amazon.com/",
                "http://localhost",
                // "http://localhost/test_cookie/test_set_cookie.php",
                // "http://localhost/test_cookie/test_show_cookie.php"
            };
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
        {/*
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

            */
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
                if ((long)page.Tag == selectedDriver.driver_id)
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
            stp.Text = "Sesiune " + selectedDriver.ToString();

            stp.Tag = selectedDriver.driver_id; // the object changes on resfreshing data from server as new objects are created for the same entity
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

            string cachePath = Path.Combine(Utilities.GetApplicationPath(), "cachedirs", "TCSesiune" + sessionCount);

            if (!Directory.Exists(cachePath))
                Directory.CreateDirectory(cachePath);

            requestContextSettings.CachePath = cachePath;

            // if (false)
            foreach (string url in GlobalContext.Urls)
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
                urlTabPage.Text = url;
                urlTabPage.UseVisualStyleBackColor = true;

                // urlTabPage.BackColor = Color.Green;

                BrowserUserControl bUC = new BrowserUserControl(url, requestContextSettings);

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

                    bUC.ResumeLayout(!false);

                    bUC.PerformLayout();
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
                            urlTabPage.Text = e.URL.Substring(0, 20);
                            urlTabPage.UseVisualStyleBackColor = true;

                            // urlTabPage.BackColor = Color.Green;

                            BrowserUserControl bUC = new BrowserUserControl(e.URL, null /*requestContextSettings*/);

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

                                bUC.ResumeLayout(!false);

                                bUC.PerformLayout();
                            }

                            urlTabPage.ResumeLayout();
                        }
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


                    Driver drv = GlobalContext.LastDriverList.drivers.Where(dr => dr.driver_id == (long)page.Tag).SingleOrDefault();

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

        void UpdateDriverList()
        {
            try
            {
                DriverList driverList = DriversAPI.GetDrivers();

                GlobalContext.LastDriverList = driverList;

                UpdateDriverListControl();
            }
            catch (Exception ex)
            {
                GlobalContext.Log("Exception getting drivers from web service: '{0}'", ex.Message);
            }
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
    }
}