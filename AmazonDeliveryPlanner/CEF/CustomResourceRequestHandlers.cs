using CefSharp;
using CefSharp.Handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AmazonDeliveryPlanner.CEF
{
    // from https://stackoverflow.com/questions/31250797/chromium-send-custom-header-info-on-initial-page-load-c-sharp
    // https://github.com/cefsharp/CefSharp/wiki/General-Usage#response-filtering

    public delegate void ProjectListLoadedHandler(string xhrSource);

    public class CustomResourceRequestHandler : ResourceRequestHandler
    {
        public /*static */event ProjectListLoadedHandler ProjectListLoaded;

        Control parentControl;

        protected override CefReturnValue OnBeforeResourceLoad(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IRequestCallback callback)
        {
            base.OnBeforeResourceLoad(chromiumWebBrowser, browser, frame, request, callback); // ?

            //z GlobalContext.Log("OnBeforeResourceLoad():           {0}", request.Url);

            // var headers = request.Headers;
            // headers["User-Agent"] = "My User Agent";
            // request.Headers = headers;            

            // request.PostData

            // return CefReturnValue.Continue;

            // GlobalContext.Log("OnBeforeResourceLoad {0}", request.Url);

            // if (request.Url.IndexOf("formCautare") >= 0)
            //     GlobalContext.LastG_Recaptcha_PageURL = request.Url;

            
            if (request.Url.IndexOf("https://www.x.com/...") >= 0)
            {
                // string decodedPostData = UTF8Encoding.UTF8.GetString(request.PostData.Elements[0].Bytes);                
            }

            //if (request.Url.IndexOf("") >= 0)
            //{
            //    GlobalContext.Log("raspuns cautare OnBeforeResourceLoad()");
            //    // ((ROSFBrowserUserControl)((CefSharp.WinForms.ChromiumWebBrowser)chromiumWebBrowser).Tag)
            //}
            // return CefReturnValue.Continue;
            /*

            if ((request.Url.IndexOf("formCautare") >= 0) &&
                (request.Url.IndexOf("wmcCautare%3AformCautare%3A%3AIFormSubmitListener%3A%3A") < 0)
                )
            {
                string decodedPostData = UTF8Encoding.UTF8.GetString(request.PostData.Elements[0].Bytes);
                
                decodedPostData += GlobalContext.LastG_Recaptcha_Response;

                if (!decodedPostData.Contains("FONRCPortalWeb"))
                {

                    request.PostData.RemoveElement(request.PostData.Elements[0]);
                    request.InitializePostData();

                    // request.PostData = new 

                    var element = request.PostData.CreatePostDataElement();
                    element.Bytes = UTF8Encoding.UTF8.GetBytes(decodedPostData);

                    request.PostData.AddElement(element);


                    // request.PostData.AddData(GlobalContext.LastG_Recaptcha_Response);

                    decodedPostData = UTF8Encoding.UTF8.GetString(request.PostData.Elements[0].Bytes);

                    GlobalContext.Log("Added G_Recaptcha_Response to post data {0}", request.Url);
                }
            }
            */

            return CefReturnValue.Continue;
            
        }

        protected override void OnResourceLoadComplete(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IResponse response, UrlRequestStatus status, long receivedContentLength)
        {
            // GlobalContext.Log("OnResourceLoadComplete():           {0}", request.Url);

            // https://relay.amazon.co.uk/api/loadboard/search
            //if (request.ResourceType == ResourceType.Xhr)
            if (request.Url.IndexOf("api/loadboard/search") >= 0)
            {
                //You can now get the data from the stream
                var bytes = memoryStream.ToArray();

                if (response.Charset == "utf-8")
                {
                    var str = System.Text.Encoding.UTF8.GetString(bytes);
                    OnSearchResultListLoaded(str);
                }
                //else
                //{
                //        //Deal with different encoding here
                //        var str = System.Text.Encoding.UTF8.GetString(bytes);
                //        OnSearchResultListLoaded(str);
                //}
                
            }
            
            base.OnResourceLoadComplete(chromiumWebBrowser, browser, frame, request, response, status, receivedContentLength);
        }


        void OnSearchResultListLoaded(string xhrSource)
        {
            if (parentControl.InvokeRequired)
            {
                ProjectListLoadedHandler stc = new ProjectListLoadedHandler(OnSearchResultListLoaded);

                try
                {
                    if (!parentControl.IsDisposed)
                        parentControl.Invoke(stc, new object[] { xhrSource });
                }
                catch (ObjectDisposedException)
                {
                }
            }
            else
            {
                if (ProjectListLoaded != null)
                    ProjectListLoaded(xhrSource);
            }
        }

        // https://github.com/cefsharp/CefSharp/wiki/General-Usage#response-filtering

        private readonly System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();

        public Control ParentControl { get => parentControl; set => parentControl = value; }

        protected override IResponseFilter GetResourceResponseFilter(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IResponse response)
        {
            return new CefSharp.ResponseFilter.StreamResponseFilter(memoryStream);
        }

        //protected override void OnResourceLoadComplete(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IResponse response, UrlRequestStatus status, long receivedContentLength)
        //{
        //    //You can now get the data from the stream
        //    var bytes = memoryStream.ToArray();

        //    if (response.Charset == "utf-8")
        //    {
        //        var str = System.Text.Encoding.UTF8.GetString(bytes);
        //    }
        //    else
        //    {
        //        //Deal with different encoding here
        //    }
        //}
    }

    public class CustomRequestHandler : RequestHandler
    {
        //CustomResourceRequestHandler customResourceRequestHandler;

        //public CustomResourceRequestHandler CustomResourceRequestHandler { get => customResourceRequestHandler; set => customResourceRequestHandler = value; }

        public /*static */event ProjectListLoadedHandler ProjectListLoaded;

        Control parentControl;

        public Control ParentControl { get => parentControl; set => parentControl = value; }

        protected override IResourceRequestHandler GetResourceRequestHandler(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, bool isNavigation, bool isDownload, string requestInitiator, ref bool disableDefaultHandling)
        {
            // when using the cached object, the behaviour is erronated and an old resource is read
            // return customResourceRequestHandler;

            CustomResourceRequestHandler crrh = new CustomResourceRequestHandler();
            crrh.ParentControl = parentControl;
            crrh.ProjectListLoaded += ProjectListLoaded;

            return crrh;
            // return new CustomResourceRequestHandler();
        }
    }

    // more at https://stackoverflow.com/questions/31250797/chromium-send-custom-header-info-on-initial-page-load-c-sharp
    // https://stackoverflow.com/a/41174808/
    // https://stackoverflow.com/a/57291720

    //https://stackoverflow.com/a/45818741
    //https://stackoverflow.com/a/45624621
    //https://blog.dotnetframework.org/2018/10/26/intercepting-ajax-requests-in-cefsharp-chrome-for-c/
    //https://stackoverflow.com/questions/42497464/how-can-cefsharp-intercept-xhr-request-to-obtain-response-body-value
    //https://github.com/cefsharp/CefSharp/blob/master/CefSharp.Example/CefSharpSchemeHandler.cs

    //https://stackoverflow.com/a/45624621
}
