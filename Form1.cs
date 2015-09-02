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
using Newtonsoft.Json;
using System.Web;

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

        public void setMax(int max)
        {
            ProgressSpinner.Invoke((MethodInvoker)(() =>
            {
                ProgressSpinner.Maximum = max;
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

        struct Response
        {
            public int code;
            public string msg;
        }

        struct Request
        {
            public string json;
            public string key;

            public Request(string json,string key)
            {
                this.json = json;
                this.key = key;
            }
        }

        private void SendRequest(string jsonData)
        {
            Request dataPacket = new Request(jsonData, StrBoxAPIKey.Text);
            WebRequest request = WebRequest.Create(Uri.EscapeUriString("http://www.raidops.net:9292/api/import.json"));
            request.ContentType = "application/json";
            request.Method = "POST";
            byte[] bytedata = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(dataPacket));
            request.ContentLength = bytedata.Length;

            Stream requestStream = request.GetRequestStream();
            requestStream.Write(bytedata, 0, bytedata.Length);
            requestStream.Flush();
            requestStream.Close();

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

            Response objResponse = JsonConvert.DeserializeObject<Response>(responseString);
            
            setStatus(objResponse.msg);
            GetImportProgress();
        }

        private void GetImportProgress()
        {
            bool done = false;
            int counter = 0;
            while (!done)
            {
                WebRequest request = WebRequest.Create(Uri.EscapeUriString("http://www.raidops.net:9292/api/get_status.json?key=" + StrBoxAPIKey.Text));
                request.ContentType = "application/x-www-form-urlencoded";
                request.Method = "POST";

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

                Response objResponse = JsonConvert.DeserializeObject<Response>(responseString);

                setStatus(objResponse.msg);

                if (counter == 0)
                {
                    List<String> parts = new List<String>();
                    if (objResponse.msg.Contains("/")) { parts = objResponse.msg.Split('/').ToList(); }
                    setMax(Convert.ToInt32(parts.Last()));
                    setProgress(0);
                }
                else
                {
                    List<String> parts = new List<String>();
                    if (objResponse.msg.Contains("/")) { parts = objResponse.msg.Split('/').ToList(); }
                    setProgress(Convert.ToInt32(parts.First()));
                }

                Thread.Sleep(500);
                counter++;
                if (counter > 40 || objResponse.msg == "Import successful") done = true;
            }
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
