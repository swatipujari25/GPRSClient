namespace GPRSClient
{
    partial class ucUploadFile
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label2 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblMsg = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.btnDisConnect = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.cboPortNo = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cboBitsPerSec = new System.Windows.Forms.ComboBox();
            this.lblTrainName = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(3, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(102, 16);
            this.label2.TabIndex = 132;
            this.label2.Text = "Port Settings";
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.IndianRed;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(301, 6);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(23, 23);
            this.btnClose.TabIndex = 131;
            this.btnClose.Text = "X";
            this.btnClose.UseVisualStyleBackColor = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panel1.Controls.Add(this.lblMsg);
            this.panel1.Controls.Add(this.lblStatus);
            this.panel1.Controls.Add(this.btnDisConnect);
            this.panel1.Controls.Add(this.btnConnect);
            this.panel1.Controls.Add(this.cboPortNo);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.cboBitsPerSec);
            this.panel1.Controls.Add(this.lblTrainName);
            this.panel1.Location = new System.Drawing.Point(4, 30);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(318, 296);
            this.panel1.TabIndex = 130;
            // 
            // lblMsg
            // 
            this.lblMsg.AutoSize = true;
            this.lblMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMsg.ForeColor = System.Drawing.Color.Red;
            this.lblMsg.Location = new System.Drawing.Point(67, 55);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(0, 16);
            this.lblMsg.TabIndex = 130;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.ForeColor = System.Drawing.Color.Red;
            this.lblStatus.Location = new System.Drawing.Point(44, 26);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 18);
            this.lblStatus.TabIndex = 129;
            // 
            // btnDisConnect
            // 
            this.btnDisConnect.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDisConnect.Location = new System.Drawing.Point(169, 184);
            this.btnDisConnect.Name = "btnDisConnect";
            this.btnDisConnect.Size = new System.Drawing.Size(81, 30);
            this.btnDisConnect.TabIndex = 4;
            this.btnDisConnect.Text = "Disconnect";
            this.btnDisConnect.UseVisualStyleBackColor = true;
            // 
            // btnConnect
            // 
            this.btnConnect.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConnect.Location = new System.Drawing.Point(65, 184);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(81, 30);
            this.btnConnect.TabIndex = 3;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            // 
            // cboPortNo
            // 
            this.cboPortNo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPortNo.FormattingEnabled = true;
            this.cboPortNo.Location = new System.Drawing.Point(156, 85);
            this.cboPortNo.Name = "cboPortNo";
            this.cboPortNo.Size = new System.Drawing.Size(121, 21);
            this.cboPortNo.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(41, 91);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(48, 15);
            this.label5.TabIndex = 125;
            this.label5.Text = "Port No";
            // 
            // cboBitsPerSec
            // 
            this.cboBitsPerSec.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboBitsPerSec.FormattingEnabled = true;
            this.cboBitsPerSec.Location = new System.Drawing.Point(156, 126);
            this.cboBitsPerSec.Name = "cboBitsPerSec";
            this.cboBitsPerSec.Size = new System.Drawing.Size(121, 21);
            this.cboBitsPerSec.TabIndex = 2;
            // 
            // lblTrainName
            // 
            this.lblTrainName.AutoSize = true;
            this.lblTrainName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTrainName.Location = new System.Drawing.Point(41, 132);
            this.lblTrainName.Name = "lblTrainName";
            this.lblTrainName.Size = new System.Drawing.Size(57, 15);
            this.lblTrainName.TabIndex = 123;
            this.lblTrainName.Text = "Baudrate";
            // 
            // ucUploadFile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.panel1);
            this.Name = "ucUploadFile";
            this.Size = new System.Drawing.Size(358, 364);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblMsg;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Button btnDisConnect;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.ComboBox cboPortNo;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cboBitsPerSec;
        private System.Windows.Forms.Label lblTrainName;
    }
}
