namespace RaidOpsUploader
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.msmMain = new MetroFramework.Components.MetroStyleManager(this.components);
            this.BtnUpload = new MetroFramework.Controls.MetroButton();
            this.StrBoxAPIKey = new MetroFramework.Controls.MetroTextBox();
            this.ProgressSpinner = new MetroFramework.Controls.MetroProgressSpinner();
            this.LblStatusTitle = new MetroFramework.Controls.MetroLabel();
            this.LblStatus = new MetroFramework.Controls.MetroLabel();
            this.LblApiKey = new MetroFramework.Controls.MetroLabel();
            this.TTLblApi = new MetroFramework.Components.MetroToolTip();
            this.BtnDown = new MetroFramework.Controls.MetroButton();
            this.UpdateNotice = new MetroFramework.Controls.MetroLink();
            this.VersionLabel = new MetroFramework.Controls.MetroLabel();
            ((System.ComponentModel.ISupportInitialize)(this.msmMain)).BeginInit();
            this.SuspendLayout();
            // 
            // msmMain
            // 
            this.msmMain.Owner = this;
            this.msmMain.Style = MetroFramework.MetroColorStyle.Orange;
            this.msmMain.Theme = MetroFramework.MetroThemeStyle.Dark;
            // 
            // BtnUpload
            // 
            this.BtnUpload.Location = new System.Drawing.Point(23, 190);
            this.BtnUpload.Name = "BtnUpload";
            this.BtnUpload.Size = new System.Drawing.Size(389, 23);
            this.BtnUpload.TabIndex = 0;
            this.BtnUpload.Text = "Upload";
            this.BtnUpload.UseSelectable = true;
            this.BtnUpload.Click += new System.EventHandler(this.BtnUpload_Click);
            // 
            // StrBoxAPIKey
            // 
            this.StrBoxAPIKey.Lines = new string[0];
            this.StrBoxAPIKey.Location = new System.Drawing.Point(23, 80);
            this.StrBoxAPIKey.MaxLength = 32767;
            this.StrBoxAPIKey.Name = "StrBoxAPIKey";
            this.StrBoxAPIKey.PasswordChar = '\0';
            this.StrBoxAPIKey.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.StrBoxAPIKey.SelectedText = "";
            this.StrBoxAPIKey.Size = new System.Drawing.Size(389, 23);
            this.StrBoxAPIKey.TabIndex = 1;
            this.StrBoxAPIKey.UseSelectable = true;
            this.StrBoxAPIKey.TextChanged += new System.EventHandler(this.SaveAPIKey);
            this.StrBoxAPIKey.Click += new System.EventHandler(this.StrBoxAPIKey_Click);
            // 
            // ProgressSpinner
            // 
            this.ProgressSpinner.Location = new System.Drawing.Point(189, 129);
            this.ProgressSpinner.Maximum = 100;
            this.ProgressSpinner.Name = "ProgressSpinner";
            this.ProgressSpinner.Size = new System.Drawing.Size(55, 55);
            this.ProgressSpinner.TabIndex = 2;
            this.ProgressSpinner.UseSelectable = true;
            // 
            // LblStatusTitle
            // 
            this.LblStatusTitle.AutoSize = true;
            this.LblStatusTitle.Location = new System.Drawing.Point(24, 109);
            this.LblStatusTitle.Name = "LblStatusTitle";
            this.LblStatusTitle.Size = new System.Drawing.Size(46, 19);
            this.LblStatusTitle.TabIndex = 3;
            this.LblStatusTitle.Text = "Status:";
            // 
            // LblStatus
            // 
            this.LblStatus.AutoSize = true;
            this.LblStatus.Location = new System.Drawing.Point(76, 109);
            this.LblStatus.Name = "LblStatus";
            this.LblStatus.Size = new System.Drawing.Size(21, 19);
            this.LblStatus.TabIndex = 4;
            this.LblStatus.Text = "--";
            // 
            // LblApiKey
            // 
            this.LblApiKey.AutoSize = true;
            this.LblApiKey.Location = new System.Drawing.Point(24, 55);
            this.LblApiKey.Name = "LblApiKey";
            this.LblApiKey.Size = new System.Drawing.Size(86, 19);
            this.LblApiKey.TabIndex = 5;
            this.LblApiKey.Text = "Your API key:";
            this.TTLblApi.SetToolTip(this.LblApiKey, "You can find it on your guild\'s page in API keys section.\r\nRemeber to log in.");
            // 
            // TTLblApi
            // 
            this.TTLblApi.Style = MetroFramework.MetroColorStyle.Blue;
            this.TTLblApi.StyleManager = null;
            this.TTLblApi.Theme = MetroFramework.MetroThemeStyle.Light;
            // 
            // BtnDown
            // 
            this.BtnDown.Location = new System.Drawing.Point(23, 219);
            this.BtnDown.Name = "BtnDown";
            this.BtnDown.Size = new System.Drawing.Size(389, 23);
            this.BtnDown.TabIndex = 6;
            this.BtnDown.Text = "Download";
            this.BtnDown.UseSelectable = true;
            this.BtnDown.Click += new System.EventHandler(this.BtnDown_Click);
            // 
            // UpdateNotice
            // 
            this.UpdateNotice.AutoSize = true;
            this.UpdateNotice.FontWeight = MetroFramework.MetroLinkWeight.Regular;
            this.UpdateNotice.Location = new System.Drawing.Point(257, 55);
            this.UpdateNotice.Name = "UpdateNotice";
            this.UpdateNotice.Size = new System.Drawing.Size(6, 6);
            this.UpdateNotice.TabIndex = 7;
            this.UpdateNotice.UseSelectable = true;
            this.UpdateNotice.Click += new System.EventHandler(this.UpdateNotice_Click);
            // 
            // VersionLabel
            // 
            this.VersionLabel.AutoSize = true;
            this.VersionLabel.FontSize = MetroFramework.MetroLabelSize.Small;
            this.VersionLabel.Location = new System.Drawing.Point(372, 245);
            this.VersionLabel.Name = "VersionLabel";
            this.VersionLabel.Size = new System.Drawing.Size(17, 15);
            this.VersionLabel.TabIndex = 8;
            this.VersionLabel.Text = "--";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(435, 259);
            this.Controls.Add(this.VersionLabel);
            this.Controls.Add(this.UpdateNotice);
            this.Controls.Add(this.BtnDown);
            this.Controls.Add(this.LblApiKey);
            this.Controls.Add(this.LblStatus);
            this.Controls.Add(this.LblStatusTitle);
            this.Controls.Add(this.ProgressSpinner);
            this.Controls.Add(this.StrBoxAPIKey);
            this.Controls.Add(this.BtnUpload);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Main";
            this.Resizable = false;
            this.Style = MetroFramework.MetroColorStyle.Default;
            this.Text = "RaidOps Uploader";
            this.Theme = MetroFramework.MetroThemeStyle.Default;
            ((System.ComponentModel.ISupportInitialize)(this.msmMain)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private MetroFramework.Components.MetroStyleManager msmMain;
        private MetroFramework.Controls.MetroButton BtnUpload;
        private MetroFramework.Controls.MetroTextBox StrBoxAPIKey;
        private MetroFramework.Controls.MetroLabel LblStatusTitle;
        private MetroFramework.Controls.MetroProgressSpinner ProgressSpinner;
        private MetroFramework.Controls.MetroLabel LblStatus;
        private MetroFramework.Controls.MetroLabel LblApiKey;
        private MetroFramework.Components.MetroToolTip TTLblApi;
        private MetroFramework.Controls.MetroButton BtnDown;
        private MetroFramework.Controls.MetroLink UpdateNotice;
        private MetroFramework.Controls.MetroLabel VersionLabel;
    }
}

