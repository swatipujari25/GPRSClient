using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GPRSClient
{
    public partial class ReadMessages : Form
    {
        public ReadMessages()
        {
            InitializeComponent();
        }

        private void btnRead_Click(object sender, EventArgs e)
        {
           txtMsgs.AppendText(BaseClass.MessagesReadingProcess());
        }

      
    }
}
