namespace EduBotControllerWindows
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.btnSend = new System.Windows.Forms.Button();
            this.rtbxInput = new System.Windows.Forms.RichTextBox();
            this.gbxBtns = new System.Windows.Forms.GroupBox();
            this.btnLedFrwd = new System.Windows.Forms.Button();
            this.btnLedRight = new System.Windows.Forms.Button();
            this.btnLedBack = new System.Windows.Forms.Button();
            this.btnLedLeft = new System.Windows.Forms.Button();
            this.btnFrwd = new System.Windows.Forms.Button();
            this.btnRight = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.btnLeft = new System.Windows.Forms.Button();
            this.rtbxLog = new System.Windows.Forms.RichTextBox();
            this.btnSettings = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.gbxBtns.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSend
            // 
            this.btnSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSend.Location = new System.Drawing.Point(501, 324);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 23);
            this.btnSend.TabIndex = 1;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // rtbxInput
            // 
            this.rtbxInput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbxInput.Location = new System.Drawing.Point(163, 12);
            this.rtbxInput.Name = "rtbxInput";
            this.rtbxInput.Size = new System.Drawing.Size(413, 306);
            this.rtbxInput.TabIndex = 0;
            this.rtbxInput.Text = "";
            this.rtbxInput.TextChanged += new System.EventHandler(this.rtbxInput_TextChanged);
            // 
            // gbxBtns
            // 
            this.gbxBtns.Controls.Add(this.btnLedFrwd);
            this.gbxBtns.Controls.Add(this.btnLedRight);
            this.gbxBtns.Controls.Add(this.btnLedBack);
            this.gbxBtns.Controls.Add(this.btnLedLeft);
            this.gbxBtns.Controls.Add(this.btnFrwd);
            this.gbxBtns.Controls.Add(this.btnRight);
            this.gbxBtns.Controls.Add(this.btnBack);
            this.gbxBtns.Controls.Add(this.btnLeft);
            this.gbxBtns.Location = new System.Drawing.Point(12, 12);
            this.gbxBtns.Name = "gbxBtns";
            this.gbxBtns.Size = new System.Drawing.Size(145, 306);
            this.gbxBtns.TabIndex = 2;
            this.gbxBtns.TabStop = false;
            this.gbxBtns.Text = "Buttons";
            // 
            // btnLedFrwd
            // 
            this.btnLedFrwd.Image = ((System.Drawing.Image)(resources.GetObject("btnLedFrwd.Image")));
            this.btnLedFrwd.Location = new System.Drawing.Point(52, 168);
            this.btnLedFrwd.Name = "btnLedFrwd";
            this.btnLedFrwd.Size = new System.Drawing.Size(40, 40);
            this.btnLedFrwd.TabIndex = 7;
            this.btnLedFrwd.UseVisualStyleBackColor = true;
            this.btnLedFrwd.Click += new System.EventHandler(this.btnLedFrwd_Click);
            // 
            // btnLedRight
            // 
            this.btnLedRight.Image = ((System.Drawing.Image)(resources.GetObject("btnLedRight.Image")));
            this.btnLedRight.Location = new System.Drawing.Point(98, 214);
            this.btnLedRight.Name = "btnLedRight";
            this.btnLedRight.Size = new System.Drawing.Size(40, 40);
            this.btnLedRight.TabIndex = 10;
            this.btnLedRight.UseVisualStyleBackColor = true;
            this.btnLedRight.Click += new System.EventHandler(this.btnLedRight_Click);
            // 
            // btnLedBack
            // 
            this.btnLedBack.Image = ((System.Drawing.Image)(resources.GetObject("btnLedBack.Image")));
            this.btnLedBack.Location = new System.Drawing.Point(52, 260);
            this.btnLedBack.Name = "btnLedBack";
            this.btnLedBack.Size = new System.Drawing.Size(40, 40);
            this.btnLedBack.TabIndex = 8;
            this.btnLedBack.UseVisualStyleBackColor = true;
            this.btnLedBack.Click += new System.EventHandler(this.btnLedBack_Click);
            // 
            // btnLedLeft
            // 
            this.btnLedLeft.Image = ((System.Drawing.Image)(resources.GetObject("btnLedLeft.Image")));
            this.btnLedLeft.Location = new System.Drawing.Point(6, 214);
            this.btnLedLeft.Name = "btnLedLeft";
            this.btnLedLeft.Size = new System.Drawing.Size(40, 40);
            this.btnLedLeft.TabIndex = 9;
            this.btnLedLeft.UseVisualStyleBackColor = true;
            this.btnLedLeft.Click += new System.EventHandler(this.btnLedLeft_Click);
            // 
            // btnFrwd
            // 
            this.btnFrwd.Image = ((System.Drawing.Image)(resources.GetObject("btnFrwd.Image")));
            this.btnFrwd.Location = new System.Drawing.Point(52, 19);
            this.btnFrwd.Name = "btnFrwd";
            this.btnFrwd.Size = new System.Drawing.Size(40, 40);
            this.btnFrwd.TabIndex = 3;
            this.btnFrwd.UseVisualStyleBackColor = true;
            this.btnFrwd.Click += new System.EventHandler(this.btnFrwd_Click);
            // 
            // btnRight
            // 
            this.btnRight.Image = global::EduBotControllerWindows.Properties.Resources.arrow_right;
            this.btnRight.Location = new System.Drawing.Point(98, 65);
            this.btnRight.Name = "btnRight";
            this.btnRight.Size = new System.Drawing.Size(40, 40);
            this.btnRight.TabIndex = 6;
            this.btnRight.UseVisualStyleBackColor = true;
            this.btnRight.Click += new System.EventHandler(this.btnRight_Click);
            // 
            // btnBack
            // 
            this.btnBack.Image = ((System.Drawing.Image)(resources.GetObject("btnBack.Image")));
            this.btnBack.Location = new System.Drawing.Point(52, 111);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(40, 40);
            this.btnBack.TabIndex = 4;
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // btnLeft
            // 
            this.btnLeft.Image = global::EduBotControllerWindows.Properties.Resources.arrow_left;
            this.btnLeft.Location = new System.Drawing.Point(6, 65);
            this.btnLeft.Name = "btnLeft";
            this.btnLeft.Size = new System.Drawing.Size(40, 40);
            this.btnLeft.TabIndex = 5;
            this.btnLeft.UseVisualStyleBackColor = true;
            this.btnLeft.Click += new System.EventHandler(this.btnLeft_Click);
            // 
            // rtbxLog
            // 
            this.rtbxLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbxLog.Location = new System.Drawing.Point(85, 353);
            this.rtbxLog.Name = "rtbxLog";
            this.rtbxLog.ReadOnly = true;
            this.rtbxLog.Size = new System.Drawing.Size(491, 61);
            this.rtbxLog.TabIndex = 12;
            this.rtbxLog.Text = "";
            // 
            // btnSettings
            // 
            this.btnSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSettings.Location = new System.Drawing.Point(12, 353);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(67, 61);
            this.btnSettings.TabIndex = 11;
            this.btnSettings.Text = "Settings";
            this.btnSettings.UseVisualStyleBackColor = true;
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
            // 
            // btnLoad
            // 
            this.btnLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnLoad.Location = new System.Drawing.Point(244, 324);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(75, 23);
            this.btnLoad.TabIndex = 13;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.Location = new System.Drawing.Point(163, 324);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 14;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(588, 426);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.btnSettings);
            this.Controls.Add(this.rtbxLog);
            this.Controls.Add(this.gbxBtns);
            this.Controls.Add(this.rtbxInput);
            this.Controls.Add(this.btnSend);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(604, 465);
            this.Name = "Main";
            this.Text = "EduBot Controller Windows";
            this.gbxBtns.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.RichTextBox rtbxInput;
        private System.Windows.Forms.GroupBox gbxBtns;
        private System.Windows.Forms.RichTextBox rtbxLog;
        private System.Windows.Forms.Button btnSettings;
        private System.Windows.Forms.Button btnLedFrwd;
        private System.Windows.Forms.Button btnLedRight;
        private System.Windows.Forms.Button btnLedBack;
        private System.Windows.Forms.Button btnLedLeft;
        private System.Windows.Forms.Button btnFrwd;
        private System.Windows.Forms.Button btnRight;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Button btnLeft;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnSave;
    }
}

