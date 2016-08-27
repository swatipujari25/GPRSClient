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
        public SendMessages()
        {
            InitializeComponent();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {

        }

        public string SendSMS(string phoneno,  string message)
        {
            string statusmsg = string.Empty;
            try
            {
                smsSentTime = DateTime.Now;
                SplitMessage outmsg = new SplitMessage();
                string responseMsg = string.Empty;


                BaseClass.SRPortComm.SendGPRSMessage(message);
                

                string currentCommand = string.Empty;

                if (Constants.IsMessageSent)
                {
                    statusmsg = "Msg Sent";
                    responseMsg = "Waiting ..";
                }
                else
                {
                    if (!Constants.IsConnectionBreak)
                    {
                        statusmsg = "Fail";
                    }
                }


                //if (BaseClass.SelectedMenuCommand == Convert.ToInt32(BaseClass.FormNames.Select_Route))
                //{
                //    currentCommand = BaseClass.FormNames.Select_Route.ToString().Replace('_', ' ');
                //}
                //else if (BaseClass.SelectedMenuCommand == Convert.ToInt32(BaseClass.FormNames.Select_Route_Status))
                //{
                //    currentCommand = BaseClass.FormNames.Select_Route_Status.ToString().Replace('_', ' ');
                //}
                //else if (BaseClass.SelectedMenuCommand == Convert.ToInt32(BaseClass.FormNames.Deselect_Route))
                //{
                //    currentCommand = BaseClass.FormNames.Deselect_Route.ToString().Replace('_', ' ');
                //}
                //else if (BaseClass.SelectedMenuCommand == Convert.ToInt32(BaseClass.FormNames.Deselect_Route_Status))
                //{
                //    currentCommand = BaseClass.FormNames.Deselect_Route_Status.ToString().Replace('_', ' ');
                //}
                //else if (BaseClass.SelectedMenuCommand == Convert.ToInt32(BaseClass.FormNames.Route_Status))
                //{
                //    currentCommand = BaseClass.FormNames.Route_Status.ToString().Replace('_', ' ');
                //}

                //BaseClass._BAL.SaveSMSDetails((int)BaseClass.CRUDOperations.Insert, 0, trainNo, coachId, BaseClass.MessageType.Request.ToString(),
                // currentCommand, smsSentTime, smsSentTime, coachNo,
                //  phoneno, message, statusmsg, null, responseMsg);
                //Constants.IsMessageSent = false;
            }
            catch (Exception ex)
            {
                Logger.Logger.WriteLog(ex);
            }
            return statusmsg;
        }
    }
}
