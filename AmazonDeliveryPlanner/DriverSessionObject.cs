using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Threading;
using System.Net;
using System.Windows.Forms;
using CefSharp;

namespace AmazonDeliveryPlanner
{
    public class DriverSessionObject
    {
        long driverId;
        RequestContextSettings reqContextSettings;

        public long DriverId { get => driverId; set => driverId = value; }
        public RequestContextSettings ReqContextSettings { get => reqContextSettings; set => reqContextSettings = value; }
    }
}
