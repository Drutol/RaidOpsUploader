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
        }

        private void BtnUpload_Click(object sender, EventArgs e)
        {
            ProgressSpinner.Speed = 2;
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

        public void set_msg(string msg)
        {

        }

        // Upload Chain
        private void IsWebsiteUp()
        {
            LblStatus.Invoke((MethodInvoker)(() =>
            {
                LblStatus.Text = "Checking if website is up and running...";
            }));
            
            WebRequest request = WebRequest.Create("http://www.raidops.net");
            request.Timeout = 2000;
            
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            if (response == null || response.StatusCode != HttpStatusCode.OK)
                throw new Exception("Website Down");
            request.Abort();
            ProgressSpinner.Invoke((MethodInvoker)(() =>
            {
                ProgressSpinner.Value = 1;
            }));
            IsApiUp();
        }

        private void IsApiUp()
        {
            LblStatus.Invoke((MethodInvoker)(() =>
            {
                LblStatus.Text = "Checking if API is up and running...";
            }));

            WebRequest request = WebRequest.Create("http://www.raidops.net");
            request.Timeout = 2000;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            if (response == null || response.StatusCode != HttpStatusCode.OK)
                throw new Exception("API Down");
            request.Abort();
            ProgressSpinner.Invoke((MethodInvoker)(() =>
            {
                ProgressSpinner.Value = 2;
            }));
            
            IsXMLFile();
        }

        private void IsXMLFile()
        {
            LblStatus.Invoke((MethodInvoker)(() =>
            {
                LblStatus.Text = "Looking for addon's save data...";
            }));
            
            String filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "NCSOFT\\Wildstar\\AddonSaveData\\RaidOps_0_Gen.xml");
            if (!File.Exists(filePath)) throw new Exception(filePath);

            StreamReader streamReader = new StreamReader(filePath);
            ProgressSpinner.Invoke((MethodInvoker)(() =>
            {
                ProgressSpinner.Value = 3;
            }));

            IsXMLValid(streamReader);

        }

        private void IsXMLValid(StreamReader xmlData)
        {
            
            XElement xmlSave = XElement.Load(xmlData);
            bool bFound = false;
            foreach (var item in xmlSave.Elements())
            {
                string nodeName = Convert.ToString(item.Attribute("K"));
                if (nodeName == "K=\"dataForWebsiteExport\"") { bFound = true; break; }
            }
            //if (!bFound) throw new Exception("There's no website export data stored...");
            ProgressSpinner.Invoke((MethodInvoker)(() =>
            {
                ProgressSpinner.Value = 4;
            }));
            
        }

    }
}
