using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Forms;
using System.Net;
using System.IO;
using System.Xml.Linq;
using System.Threading;

namespace RaidOpsUploader
{
    public partial class Main : MetroForm
    {
        
        public Main()
        {
            InitializeComponent();

            this.StyleManager = msmMain;
            this.LblStatus.Text = "Waiting...";
            this.ProgressSpinner.Speed = 1;
            ProgressSpinner.Maximum = 4;
            setProgress(-1);
        }

        private void BtnUpload_Click(object sender, EventArgs e)
        {
            //ProgressSpinner.Speed = 2;
            setProgress(0);
            try
            {
                System.Threading.ThreadPool.QueueUserWorkItem(delegate {IsWebsiteUp();}, null);
            }
            catch (Exception exc)
            {
                reset(exc.Message);
            }
        }

        private void reset(String msg)
        {
            LblStatus.Text = msg;
        }

        public void setStatus(string msg)
        {
            LblStatus.Invoke((MethodInvoker)(() =>
            {
                LblStatus.Text = msg;
            }));
        }

        public void setProgress(int step)
        {
            ProgressSpinner.Invoke((MethodInvoker)(() =>
            {
                ProgressSpinner.Value = step;
            }));
        }



        // Upload Chain
        private void IsWebsiteUp()
        {
            setStatus("Checking if website is up and running...");

            WebRequest request = WebRequest.Create("http://www.raidops.net");
            request.Timeout = 2000;
            
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            if (response == null || response.StatusCode != HttpStatusCode.OK)
                throw new Exception("Website Down");
            request.Abort();
            setProgress(1);
            IsApiUp();
        }

        private void IsApiUp()
        {
            setStatus("Checking if API is up and running...");

            WebRequest request = WebRequest.Create("http://www.raidops.net/api/import.json");
            request.Timeout = 2000;
            request.Method = "POST";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            if (response == null || response.StatusCode != HttpStatusCode.OK)
                throw new Exception("API Down");
            request.Abort();
            setProgress(2);
            IsDataInClipboard();
        }

        private void IsDataInClipboard()
        {

            setStatus("Looking for addon's save data... (clipboard)");

            string jsonData = "";
            ClipboardAsync Clipboard2 = new ClipboardAsync();
            jsonData = Clipboard2.GetText();

            if (jsonData.StartsWith("{\"tRaids\":") || jsonData.StartsWith("{\"tMembers\":"))
            {
                setProgress(4);
                SendRequest(jsonData);
            }
            else
            {
                IsXMLFile();
            }
 

        }
       

        private void IsXMLFile()
        {
            setStatus("Looking for addon's save data... (xml)");
            
            String filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "NCSOFT\\Wildstar\\AddonSaveData\\RaidOps_0_Gen.xml");
            if (!File.Exists(filePath)) throw new Exception(filePath);

            StreamReader streamReader = new StreamReader(filePath);
            setProgress(3);

            IsXMLValid(streamReader);

        }

        private void IsXMLValid(StreamReader xmlData)
        {
            
            XElement xmlSave = XElement.Load(xmlData);
            bool bFound = false;
            string jsonData = "";
            foreach (var item in xmlSave.Elements())
            {
                string nodeName = Convert.ToString(item.Attribute("K"));
                if (nodeName == "K=\"dataForWebsiteExport\"")
                {
                    bFound = true;
                    jsonData = item.Value;
                    break; 
                }
            }
            if (!bFound)
            {
                setStatus("There's no website export data stored...");
                return;
            }
            else
            {
                SendRequest(jsonData);
            }

            setProgress(4);
            
        }

        private void SendRequest(string jsonData)
        {
            WebRequest request = WebRequest.Create("http://www.raidops.net/api/import.json?key="+ StrBoxAPIKey.Text + "&json=" + jsonData);
            //request.Timeout = 2000;
            request.Method = "POST";
            byte[] byteArray = Encoding.UTF8.GetBytes("");
            // Set the ContentType property of the WebRequest.
            request.ContentType = "application/x-www-form-urlencoded";
            // Set the ContentLength property of the WebRequest.
           // request.ContentLength = byteArray.Length;
            // Get the request stream.
           // Stream dataStream = request.GetRequestStream();
            // Write the data to the request stream.
           // dataStream.Write(byteArray, 0, byteArray.Length);
            // Close the Stream object.
            //dataStream.Close();
            // Get the response.
            
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            if (response == null || response.StatusCode != HttpStatusCode.OK)
            {
                setStatus("No response from server...");
            }

            string responseString = "";
            using (Stream stream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                responseString = reader.ReadToEnd();
            }


            setStatus(responseString);
        }

    }
    class ClipboardAsync
    {

        private string _GetText;
        private void _thGetText(object format)
        {
            try
            {
                if (format == null)
                {
                    _GetText = Clipboard.GetText();
                }
                else
                {
                    _GetText = Clipboard.GetText((TextDataFormat)format);

                }
            }
            catch (Exception ex)
            {
                //Throw ex 
                _GetText = string.Empty;
            }
        }
        public string GetText()
        {
            ClipboardAsync instance = new ClipboardAsync();
            Thread staThread = new Thread(instance._thGetText);
            staThread.SetApartmentState(ApartmentState.STA);
            staThread.Start();
            staThread.Join();
            return instance._GetText;
        }
        public string GetText(TextDataFormat format)
        {
            ClipboardAsync instance = new ClipboardAsync();
            Thread staThread = new Thread(instance._thGetText);
            staThread.SetApartmentState(ApartmentState.STA);
            staThread.Start(format);
            staThread.Join();
            return instance._GetText;
        }

        private bool _ContainsText;
        private void _thContainsText(object format)
        {
            try
            {
                if (format == null)
                {
                    _ContainsText = Clipboard.ContainsText();
                }
                else
                {
                    _ContainsText = Clipboard.ContainsText((TextDataFormat)format);
                }
            }
            catch (Exception ex)
            {
                //Throw ex 
                _ContainsText = false;
            }
        }
        public bool ContainsText()
        {
            ClipboardAsync instance = new ClipboardAsync();
            Thread staThread = new Thread(instance._thContainsFileDropList);
            staThread.SetApartmentState(ApartmentState.STA);
            staThread.Start();
            staThread.Join();
            return instance._ContainsText;
        }
        public bool ContainsText(object format)
        {
            ClipboardAsync instance = new ClipboardAsync();
            Thread staThread = new Thread(instance._thContainsFileDropList);
            staThread.SetApartmentState(ApartmentState.STA);
            staThread.Start(format);
            staThread.Join();
            return instance._ContainsText;
        }

        private bool _ContainsFileDropList;
        private void _thContainsFileDropList(object format)
        {
            try
            {
                _ContainsFileDropList = Clipboard.ContainsFileDropList();
            }
            catch (Exception ex)
            {
                //Throw ex 
                _ContainsFileDropList = false;
            }
        }
        public bool ContainsFileDropList()
        {
            ClipboardAsync instance = new ClipboardAsync();
            Thread staThread = new Thread(instance._thContainsFileDropList);
            staThread.SetApartmentState(ApartmentState.STA);
            staThread.Start();
            staThread.Join();
            return instance._ContainsFileDropList;
        }

        private System.Collections.Specialized.StringCollection _GetFileDropList;
        private void _thGetFileDropList()
        {
            try
            {
                _GetFileDropList = Clipboard.GetFileDropList();
            }
            catch (Exception ex)
            {
                //Throw ex 
                _GetFileDropList = null;
            }
        }
        public System.Collections.Specialized.StringCollection GetFileDropList()
        {
            ClipboardAsync instance = new ClipboardAsync();
            Thread staThread = new Thread(instance._thGetFileDropList);
            staThread.SetApartmentState(ApartmentState.STA);
            staThread.Start();
            staThread.Join();
            return instance._GetFileDropList;
        }
    }
}
