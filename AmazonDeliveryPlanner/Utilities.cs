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

namespace AmazonDeliveryPlanner
{
    public class Utilities
    {
        string GetApplicationPathEx()
        {
            throw new NotImplementedException();            
        }

        public static string GetApplicationPath()
        {
            //return Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            return Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
            // return Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        }

        public static object LoadXML(string filePath, Type objectType)
        {
            StreamReader reader = null;

            try
            {
                reader = new StreamReader(filePath);

                XmlSerializer bf = new XmlSerializer(objectType, types);

                return bf.Deserialize(reader);
            }
            catch (Exception)
            {
                //Debug.Debug.AddException(ex);
                throw; // Console.WriteLine(ex.Message);                
            }
            finally
            {
                if (reader != null) reader.Close();
            }
        }

        static Type[] types = new Type[] { /*typeof()*/ };
            
        public static bool SaveXML(string filePath, object objectToSerialize)
        {
            FileStream writer = null;

            try
            {
                //File.Copy(filePath, filePath + ".bak", true);

                writer = File.Open(filePath, FileMode.CreateNew);

                XmlSerializer bf = new XmlSerializer(objectToSerialize.GetType(), types);

                bf.Serialize(writer, objectToSerialize);

                //File.Delete(filePath + ".bak");

                return true;
            }
            catch (Exception)
            {
                //Debug.Debug.AddException(ex);

                //try
                //{
                //    File.Copy(filePath + ".bak", filePath, true);
                //}
                //catch (Exception exception)
                //{

                //}

                throw;// Console.WriteLine(ex.Message);
            }
            finally
            {
                writer.Close();
            }
        }

        public static int IndexOf(string text, string[] searchTexts)
        {
            int pos = 0;

            for (int index = 0; index < searchTexts.Length; index++)
                if ((pos = text.IndexOf(searchTexts[index], StringComparison.OrdinalIgnoreCase)) >= 0)
                    return pos;

            return -1;
        }

        public static string GetUserApplicationPath()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + Path.DirectorySeparatorChar + Path.GetFileName(System.Environment.GetCommandLineArgs()[0]);
        }

        public static void LogMessage(string message, string header)
        {
            throw new NotSupportedException();

            //log4net.ILog logger = log4net.LogManager.GetLogger("File");
            //logger.Info(header + ": " + message);
        }
    }
}
