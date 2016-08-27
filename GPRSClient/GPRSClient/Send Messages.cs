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
    public partial class SendMessages : Form
    {
        string message = string.Empty;

        public SendMessages()
        {
            InitializeComponent();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            message = txtSendMsg.Text;
          //  rtxtMessages.AppendText( "RtNo:12724, is selected");
//            BaseClass.SRPortComm.SendGPRSMessage("RtNo:12723, is selected");
            rtxtMessages.AppendText(message);
            BaseClass.SRPortComm.SendMessage("+917331153863", message);
        }

        private void btnDeselect_Click(object sender, EventArgs e)
        {
            message = txtSendMsg.Text;
            //rtxtMessages.AppendText("RtNo:12723, is deselected");
//            BaseClass.SRPortComm.SendGPRSMessage("RtNo:12723, is deselected");
            rtxtMessages.AppendText(message);
            BaseClass.SRPortComm.SendMessage("+917331153863", message);
        }

        private void btnStatus_Click(object sender, EventArgs e)
        {
            message = txtSendMsg.Text;
            //rtxtMessages.AppendText("status, RtNo:12723, CurStn:SECUNDERABAD JN, NxtStn:KAZIPET JN, Dist:79");
          //  BaseClass.SRPortComm.SendGPRSMessage("status, RtNo:12723, CurStn:SECUNDERABAD JN, NxtStn:KAZIPET JN, Dist:79");
         //   BaseClass.SRPortComm.SendMessage("+917331153863", "status, RtNo:12723, CurStn:SECUNDERABAD JN, NxtStn:KAZIPET JN, Dist:79");
            rtxtMessages.AppendText(message);
            BaseClass.SRPortComm.SendMessage("+917331153863", message);
        }

        private void SendMessages_Load(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (Constants.CurrentRequest == Constants.Commands.ClientGPRSReady.ToString())
            {
                if (!string.IsNullOrEmpty(Constants.GPRSReceivedMsg))
                {
                    rtxtMessages.AppendText(Constants.GPRSReceivedMsg);
                    Constants.GPRSReceivedMsg = string.Empty;
                }
            }
        }
    }
}
