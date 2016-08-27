using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GPRSClient
{
    public partial class ucCommunication : UserControl
    {
        string message = string.Empty;

        public ucCommunication()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            message = txtSend.Text;
            rtxtRead.AppendText(message);

            BaseClass.SRPortComm.SendGPRSMessage(message);
        }

        

      
    }
}
