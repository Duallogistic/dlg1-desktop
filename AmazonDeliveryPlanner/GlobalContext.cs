using AmazonDeliveryPlanner.API;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AmazonDeliveryPlanner
{
    public static class GlobalContext
    {
        static MainForm mainForm;

        public static MainForm MainWindow
        {
            get { return GlobalContext.mainForm; }
            set { GlobalContext.mainForm = value; }
        }

        /*static string templatesFileTitle = "templates.xml";

        public static string TemplatesFileTitle
        {
            get { return GlobalContext.templatesFileTitle; }
            set { GlobalContext.templatesFileTitle = value; }
        }
        */

        //static Settings appSettings;

        //public static Settings AppSettings
        //{
        //    get { return GlobalContext.appSettings; }
        //    set { GlobalContext.appSettings = value; }
        //}

        static string applicationTitle;

        public static string ApplicationTitle
        {
            get { return GlobalContext.applicationTitle; }
            set { GlobalContext.applicationTitle = value; }
        }

        public static CefSettings GlobalCefSettings { get => globalCefSettings; set => globalCefSettings = value; }
        public static string UserAgent { get => userAgent; set => userAgent = value; }
        public static List<string> Urls { get => urls; set => urls = value; }
        public static DriverList LastDriverList { get => lastDriverList; set => lastDriverList = value; }

        public static void Log(string text, params object[] args)
        {
            // string msg = string.Format(text, args);

            gLogger.Log(0, string.Format(text, args));            
        }

        public static void LogWithLevel(int level, string text, params object[] args)
        {
            // string msg = string.Format(text, args);

            gLogger.Log(level, string.Format(text, args));
        }

        // public static GLogger GLogger { get => gLogger; set => gLogger = value; }

        //static string optionsFile = "settings.xml";

        //public static string OptionsFile
        //{
        //    get { return GlobalContext.optionsFile; }
        //    set { GlobalContext.optionsFile = value; }
        //}

        static GLogger gLogger;

        static GlobalContext()
        {
            gLogger = new GLogger();

            gLogger.InitLogger();
        }

        //static DownloadManager downloadManager;        

        static bool useDebugTestResult = false;

        static CefSettings globalCefSettings;

        static string userAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.163 Safari/537.36";

        static string lastG_Recaptcha_Response;
        static string lastG_Recaptcha_PageURL;

        //static string _getDropdownPosScript = "";
        //static string _clickDropdownCIFOptionScript = "";
        //static string _clickResultDetailsScript = "";
        //static string _detectResultsScript = "";
        static Dictionary<string, string> scripts;

        static bool showDevTools;

        static List<string> urls;

        static DriverList lastDriverList;
    }
}
