namespace GPRSClient
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.configToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.connectToPortToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.connectToServerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.readMsgsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sendMsgsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.lblServerIPAddress = new System.Windows.Forms.Label();
            this.lblPortNo = new System.Windows.Forms.Label();
            this.lblIPAddress = new System.Windows.Forms.Label();
            this.lblConnectionStatus = new System.Windows.Forms.Label();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.gprsConnectControl = new GPRSClient.ucGPRSConnect();
            this.connectToPortControl = new GPRSClient.ucConnectToPort();
            this.communicationControl = new GPRSClient.ucCommunication();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.configToolStripMenuItem,
            this.readMsgsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(956, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // configToolStripMenuItem
            // 
            this.configToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connectToPortToolStripMenuItem,
            this.connectToServerToolStripMenuItem});
            this.configToolStripMenuItem.Name = "configToolStripMenuItem";
            this.configToolStripMenuItem.Size = new System.Drawing.Size(55, 20);
            this.configToolStripMenuItem.Text = "Config";
            this.configToolStripMenuItem.Click += new System.EventHandler(this.configToolStripMenuItem_Click);
            // 
            // connectToPortToolStripMenuItem
            // 
            this.connectToPortToolStripMenuItem.Name = "connectToPortToolStripMenuItem";
            this.connectToPortToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.connectToPortToolStripMenuItem.Text = "Connect To Port";
            this.connectToPortToolStripMenuItem.Click += new System.EventHandler(this.connectToPortToolStripMenuItem_Click);
            // 
            // connectToServerToolStripMenuItem
            // 
            this.connectToServerToolStripMenuItem.Name = "connectToServerToolStripMenuItem";
            this.connectToServerToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.connectToServerToolStripMenuItem.Text = "Connect To Server";
            this.connectToServerToolStripMenuItem.Click += new System.EventHandler(this.connectToServerToolStripMenuItem_Click);
            // 
            // readMsgsToolStripMenuItem
            // 
            this.readMsgsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sendMsgsToolStripMenuItem});
            this.readMsgsToolStripMenuItem.Name = "readMsgsToolStripMenuItem";
            this.readMsgsToolStripMenuItem.Size = new System.Drawing.Size(81, 20);
            this.readMsgsToolStripMenuItem.Text = "Commands";
            // 
            // sendMsgsToolStripMenuItem
            // 
            this.sendMsgsToolStripMenuItem.Name = "sendMsgsToolStripMenuItem";
            this.sendMsgsToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
            this.sendMsgsToolStripMenuItem.Text = "Send Msgs";
            this.sendMsgsToolStripMenuItem.Click += new System.EventHandler(this.sendMsgsToolStripMenuItem_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // lblServerIPAddress
            // 
            this.lblServerIPAddress.AutoSize = true;
            this.lblServerIPAddress.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblServerIPAddress.ForeColor = System.Drawing.Color.Black;
            this.lblServerIPAddress.Location = new System.Drawing.Point(15, 61);
            this.lblServerIPAddress.Name = "lblServerIPAddress";
            this.lblServerIPAddress.Size = new System.Drawing.Size(0, 16);
            this.lblServerIPAddress.TabIndex = 127;
            // 
            // lblPortNo
            // 
            this.lblPortNo.AutoSize = true;
            this.lblPortNo.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPortNo.ForeColor = System.Drawing.Color.Black;
            this.lblPortNo.Location = new System.Drawing.Point(15, 93);
            this.lblPortNo.Name = "lblPortNo";
            this.lblPortNo.Size = new System.Drawing.Size(0, 16);
            this.lblPortNo.TabIndex = 128;
            // 
            // lblIPAddress
            // 
            this.lblIPAddress.AutoSize = true;
            this.lblIPAddress.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIPAddress.ForeColor = System.Drawing.Color.Black;
            this.lblIPAddress.Location = new System.Drawing.Point(15, 29);
            this.lblIPAddress.Name = "lblIPAddress";
            this.lblIPAddress.Size = new System.Drawing.Size(0, 16);
            this.lblIPAddress.TabIndex = 129;
            // 
            // lblConnectionStatus
            // 
            this.lblConnectionStatus.AutoSize = true;
            this.lblConnectionStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblConnectionStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblConnectionStatus.Location = new System.Drawing.Point(60, 118);
            this.lblConnectionStatus.Name = "lblConnectionStatus";
            this.lblConnectionStatus.Size = new System.Drawing.Size(0, 24);
            this.lblConnectionStatus.TabIndex = 130;
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDisconnect.Location = new System.Drawing.Point(18, 304);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(82, 30);
            this.btnDisconnect.TabIndex = 131;
            this.btnDisconnect.Text = "DisConnect";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // gprsConnectControl
            // 
            this.gprsConnectControl.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.gprsConnectControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.gprsConnectControl.Location = new System.Drawing.Point(354, 161);
            this.gprsConnectControl.Name = "gprsConnectControl";
            this.gprsConnectControl.Size = new System.Drawing.Size(355, 340);
            this.gprsConnectControl.TabIndex = 0;
            this.gprsConnectControl.Visible = false;
            // 
            // connectToPortControl
            // 
            this.connectToPortControl.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.connectToPortControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.connectToPortControl.Location = new System.Drawing.Point(364, 118);
            this.connectToPortControl.Name = "connectToPortControl";
            this.connectToPortControl.Size = new System.Drawing.Size(327, 326);
            this.connectToPortControl.TabIndex = 0;
            this.connectToPortControl.Visible = false;
            // 
            // communicationControl
            // 
            this.communicationControl.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.communicationControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.communicationControl.Location = new System.Drawing.Point(354, 30);
            this.communicationControl.Name = "communicationControl";
            this.communicationControl.Size = new System.Drawing.Size(583, 480);
            this.communicationControl.TabIndex = 0;
            this.communicationControl.Visible = false;
            // 
            // button1
            // 
            this.button1.AutoSize = true;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(18, 194);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(127, 30);
            this.button1.TabIndex = 132;
            this.button1.Text = "Switch to Command";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.AutoSize = true;
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(18, 230);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(95, 30);
            this.button2.TabIndex = 133;
            this.button2.Text = "Switch to Data";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(956, 632);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnDisconnect);
            this.Controls.Add(this.lblConnectionStatus);
            this.Controls.Add(this.lblIPAddress);
            this.Controls.Add(this.lblPortNo);
            this.Controls.Add(this.lblServerIPAddress);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.gprsConnectControl);
            this.Controls.Add(this.connectToPortControl);
            this.Controls.Add(this.communicationControl);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Main";
            this.Text = "Main";
            this.Load += new System.EventHandler(this.Main_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem configToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem connectToPortToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem connectToServerToolStripMenuItem;
        private ucGPRSConnect gprsConnectControl;
        private ucConnectToPort connectToPortControl;
        private ucCommunication communicationControl;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ToolStripMenuItem readMsgsToolStripMenuItem;
        private System.Windows.Forms.Label lblServerIPAddress;
        private System.Windows.Forms.Label lblPortNo;
        private System.Windows.Forms.Label lblIPAddress;
        private System.Windows.Forms.ToolStripMenuItem sendMsgsToolStripMenuItem;
        private System.Windows.Forms.Label lblConnectionStatus;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}