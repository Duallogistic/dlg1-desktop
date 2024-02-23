//using CefSharp;
//using System;
//using System.IO;
//using System.Security.Cryptography.X509Certificates;
//using System.Text;

//namespace AmazonDeliveryPlanner.CEF
//{
//    public class xRequestHandler : IRequestHandler
//    {
//        //private static readonly Uri ResourceUrl = new Uri("http://test/resource/load");

//        //public static readonly string VersionNumberString = String.Format("Chromium: {0}, CEF: {1}, CefSharp: {2}",
//        //    Cef.ChromiumVersion, Cef.CefVersion, Cef.CefSharpVersion);

//        //bool IRequestHandler.OnBeforeBrowse(IWebBrowser browser, IRequest request, bool isRedirect)
//        //{
//        //    return false;
//        //}

//        //void IRequestHandler.OnPluginCrashed(IWebBrowser browser, string pluginPath)
//        //{
//        //    // TODO: Add your own code here for handling scenarios where a plugin crashed, for one reason or another.
//        //}

//        //bool IRequestHandler.OnBeforeResourceLoad(IWebBrowser browser, IRequestResponse requestResponse)
//        //{
//        //    IRequest request = requestResponse.Request;
//        //    if (request.Url.StartsWith(ResourceUrl.ToString()))
//        //    {
//        //        Stream resourceStream = new MemoryStream(Encoding.UTF8.GetBytes(
//        //            "<html><body><h1>Success</h1><p>This document is loaded from a System.IO.Stream</p></body></html>"));
//        //        requestResponse.RespondWith(resourceStream, "text/html");
//        //    }

//        //    return false;
//        //}

//        //bool IRequestHandler.GetAuthCredentials(IWebBrowser browser, bool isProxy, string host, int port, string realm, string scheme, ref string username, ref string password)
//        //{
//        //    return false;
//        //}

//        //bool IRequestHandler.OnBeforePluginLoad(IWebBrowser browser, string url, string policy_url, IWebPluginInfo info)
//        //{
//        //    bool blockPluginLoad = false;

//        //    // Enable next line to demo: Block any plugin with "flash" in its name
//        //    // try it out with e.g. http://www.youtube.com/watch?v=0uBOtQOO70Y
//        //    //blockPluginLoad = info.Name.ToLower().Contains("flash");
//        //    return blockPluginLoad;
//        //}

//        //void IRequestHandler.OnRenderProcessTerminated(IWebBrowser browser, CefTerminationStatus status)
//        //{
//        //    // TODO: Add your own code here for handling scenarios where the Render Process terminated for one reason or another.
//        //}
//        bool IRequestHandler.GetAuthCredentials(IWebBrowser chromiumWebBrowser, IBrowser browser, string originUrl, bool isProxy, string host, int port, string realm, string scheme, IAuthCallback callback)
//        {
//            throw new NotImplementedException();
//        }

//        IResourceRequestHandler IRequestHandler.GetResourceRequestHandler(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, bool isNavigation, bool isDownload, string requestInitiator, ref bool disableDefaultHandling)
//        {
//            throw new NotImplementedException();
//        }

//        bool IRequestHandler.OnBeforeBrowse(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, bool userGesture, bool isRedirect)
//        {
//            throw new NotImplementedException();
//        }

//        bool IRequestHandler.OnCertificateError(IWebBrowser chromiumWebBrowser, IBrowser browser, CefErrorCode errorCode, string requestUrl, ISslInfo sslInfo, IRequestCallback callback)
//        {
//            throw new NotImplementedException();
//        }

//        void IRequestHandler.OnDocumentAvailableInMainFrame(IWebBrowser chromiumWebBrowser, IBrowser browser)
//        {
//            throw new NotImplementedException();
//        }

//        bool IRequestHandler.OnOpenUrlFromTab(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, string targetUrl, WindowOpenDisposition targetDisposition, bool userGesture)
//        {
//            throw new NotImplementedException();
//        }

//        bool IRequestHandler.OnQuotaRequest(IWebBrowser chromiumWebBrowser, IBrowser browser, string originUrl, long newSize, IRequestCallback callback)
//        {
//            throw new NotImplementedException();
//        }

//        void IRequestHandler.OnRenderProcessTerminated(IWebBrowser chromiumWebBrowser, IBrowser browser, CefTerminationStatus status)
//        {
//            throw new NotImplementedException();
//        }

//        void IRequestHandler.OnRenderViewReady(IWebBrowser chromiumWebBrowser, IBrowser browser)
//        {
//            throw new NotImplementedException();
//        }

//        bool IRequestHandler.OnSelectClientCertificate(IWebBrowser chromiumWebBrowser, IBrowser browser, bool isProxy, string host, int port, X509Certificate2Collection certificates, ISelectClientCertificateCallback callback)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
