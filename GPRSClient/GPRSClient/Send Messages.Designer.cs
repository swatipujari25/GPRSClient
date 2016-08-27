namespace GPRSClient
{
    partial class SendMessages
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
            this.btnSelect = new System.Windows.Forms.Button();
            this.btnDeselect = new System.Windows.Forms.Button();
            this.btnStatus = new System.Windows.Forms.Button();
            this.btnUpload = new System.Windows.Forms.Button();
            this.btnDownload = new System.Windows.Forms.Button();
            this.rtxtMessages = new System.Windows.Forms.RichTextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.txtSendMsg = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnSelect
            // 
            this.btnSelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSelect.Location = new System.Drawing.Point(24, 46);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(139, 41);
            this.btnSelect.TabIndex = 3;
            this.btnSelect.Text = "Select Response";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // btnDeselect
            // 
            this.btnDeselect.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeselect.Location = new System.Drawing.Point(24, 107);
            this.btnDeselect.Name = "btnDeselect";
            this.btnDeselect.Size = new System.Drawing.Size(139, 41);
            this.btnDeselect.TabIndex = 4;
            this.btnDeselect.Text = "DeSelect Response";
            this.btnDeselect.UseVisualStyleBackColor = true;
            this.btnDeselect.Click += new System.EventHandler(this.btnDeselect_Click);
            // 
            // btnStatus
            // 
            this.btnStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStatus.Location = new System.Drawing.Point(24, 166);
            this.btnStatus.Name = "btnStatus";
            this.btnStatus.Size = new System.Drawing.Size(139, 41);
            this.btnStatus.TabIndex = 5;
            this.btnStatus.Text = "Status Response";
            this.btnStatus.UseVisualStyleBackColor = true;
            this.btnStatus.Click += new System.EventHandler(this.btnStatus_Click);
            // 
            // btnUpload
            // 
            this.btnUpload.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpload.Location = new System.Drawing.Point(24, 223);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(139, 41);
            this.btnUpload.TabIndex = 6;
            this.btnUpload.Text = "Upload";
            this.btnUpload.UseVisualStyleBackColor = true;
            this.btnUpload.Visible = false;
            // 
            // btnDownload
            // 
            this.btnDownload.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDownload.Location = new System.Drawing.Point(24, 284);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(139, 41);
            this.btnDownload.TabIndex = 7;
            this.btnDownload.Text = "Download";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Visible = false;
            // 
            // rtxtMessages
            // 
            this.rtxtMessages.Location = new System.Drawing.Point(201, 81);
            this.rtxtMessages.Name = "rtxtMessages";
            this.rtxtMessages.Size = new System.Drawing.Size(301, 244);
            this.rtxtMessages.TabIndex = 8;
            this.rtxtMessages.Text = "";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 200;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // txtSendMsg
            // 
            this.txtSendMsg.Location = new System.Drawing.Point(201, 25);
            this.txtSendMsg.Multiline = true;
            this.txtSendMsg.Name = "txtSendMsg";
            this.txtSendMsg.Size = new System.Drawing.Size(301, 41);
            this.txtSendMsg.TabIndex = 9;
            // 
            // SendMessages
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(514, 390);
            this.Controls.Add(this.txtSendMsg);
            this.Controls.Add(this.rtxtMessages);
            this.Controls.Add(this.btnDownload);
            this.Controls.Add(this.btnUpload);
            this.Controls.Add(this.btnStatus);
            this.Controls.Add(this.btnDeselect);
            this.Controls.Add(this.btnSelect);
            this.Name = "SendMessages";
            this.Text = "Send Messages";
            this.Load += new System.EventHandler(this.SendMessages_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.Button btnDeselect;
        private System.Windows.Forms.Button btnStatus;
        private System.Windows.Forms.Button btnUpload;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.RichTextBox rtxtMessages;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TextBox txtSendMsg;
    }
}