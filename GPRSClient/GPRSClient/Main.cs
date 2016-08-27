using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace GPRSClient
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        DialogResult result = new DialogResult();
        string receivedMsg = string.Empty;

        #region Delegates      
        public delegate void SMSResponse();
        public SMSResponse smsResponseDelegate;

        public delegate void CheckConnection();
        public CheckConnection chkConnectionDelegate;
        public CheckConnection chkBrokenConDelegate;  

        /// <summary>
        /// When modem is fail/disconnected, this method will call
        /// </summary>
        private void InvokeCheckConnection()
        {
            try
            {
                lblConnectionStatus.Invoke(chkConnectionDelegate);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }

        /// <summary>
        /// Update Broken connection connected status
        /// </summary>
        private void InvokeConnectBrokenConnection()
        {
            try
            {
                lblConnectionStatus.Invoke(chkBrokenConDelegate);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }

        /// <summary>
        /// Update sent messages responses
        /// </summary>
        private void InvokeResponseSMS()
        {
            try
            {
                communicationControl.rtxtRead.Invoke(smsResponseDelegate);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }

        private void BindResponse()
        {
            try
            {
                communicationControl.rtxtRead.AppendText("Server ==> " + receivedMsg);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }

        /// <summary>
        /// Try to connect broken connection 
        /// </summary>
        private void ConnectBrokenConnection()
        {
            try
            {
                BaseClass.timeCounter = 5000;
                BaseClass.SRPortComm.CommunicationTestCommand();    
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }
        #endregion


        private void MessagesReadingProcess()
        {
            try
            {
                Thread.Sleep(1000);

                Constants.CurrentRequest = Constants.Commands.UnRead.ToString();
                BaseClass.SRPortComm.ReadMessage();
                BaseClass.timeCounter = 10000;
                BaseClass.counterWatch.Start();

                // UpdateGridWithNewMessages();
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }

        private void connectToPortToolStripMenuItem_Click(object sender, EventArgs e)
        {
            connectToPortControl.Visible = true;
            gprsConnectControl.Visible = false;
        }

        private void connectToServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            connectToPortControl.Visible = false;
            gprsConnectControl.Visible = true;

            if(!string.IsNullOrEmpty(Constants.GPRSServerIPAddress) && !string.IsNullOrEmpty(Constants.GPRSPortNo))
            {
                gprsConnectControl.txtIPAddress.Text = Constants.GPRSServerIPAddress;
                gprsConnectControl.txtPortNo.Text = Constants.GPRSPortNo;
            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            #region update current Modem Status ... 

            if (Constants.ConnectionStatus == Constants.Connectivity.Connected.ToString())
            {
                lblConnectionStatus.Text = BaseClass.ModemStatus.Modem_Connected.ToString().Replace('_', ' ');
                Constants.IsModemConnected = true;
                lblConnectionStatus.Visible = true;

            }
            else if (Constants.ConnectionStatus == Constants.Connectivity.Disconnected.ToString())
            {
                lblConnectionStatus.Text = BaseClass.ModemStatus.Modem_Disconnected.ToString().Replace('_', ' ');
                Constants.IsModemConnected = false;
                lblConnectionStatus.Visible = true;
            }
            else if (!string.IsNullOrEmpty(Constants.ConnectionStatus))
            {
                // this.connectToPortControl.Visible = true;
                lblConnectionStatus.Text = BaseClass.ModemStatus.MODEM_FAIL.ToString().Replace('_', ' ');
                Constants.IsModemConnected = false;
            }
            #endregion

            #region Modem Fail lable should blink ... 
            if (!Constants.IsConnectingToModem && lblConnectionStatus.Text == BaseClass.ModemStatus.MODEM_FAIL.ToString().Replace('_', ' '))
            {
                if (lblConnectionStatus.Visible)
                {
                    lblConnectionStatus.Visible = false;
                }
                else
                {
                    lblConnectionStatus.Visible = true;
                }
            }
            #endregion

            #region Connecting to Modem ... 
            if (Constants.IsConnectingToModem)
            {
                //At command succeed below code will execute
                if (Constants.CurrentRequest == Constants.Commands.AT.ToString())
                {
                    if (Constants.receivingInprocess)
                    {
                        BaseClass.CalculateCounterTime();

                        if (BaseClass.timeCounter <= BaseClass.milliTime)
                        {
                            Constants.IsResponseSuccess = false;
                            Constants.receivingInprocess = false;
                            Stop_Reset_D_Counter();
                        }
                    }
                    else if (!Constants.receivingInprocess && Constants.IsResponseSuccess)
                    {
                        Stop_Reset_D_Counter();
                        BaseClass.SRPortComm.EcoOFFCommand();
                        BaseClass.timeCounter = 5000;
                        BaseClass.counterWatch.Start();
                    }
                    else if (!Constants.receivingInprocess && !Constants.IsResponseSuccess)
                    {
                        Stop_Reset_D_Counter();
                        connectToPortControl.statusMessage = "Please select valid COM port" + Environment.NewLine + " And Check Modem Power supply";
                        connectToPortControl.displayForeColor = Color.Red;
                        connectToPortControl.InvokeCheckConnection();

                        Constants.ConnectionStatus = Constants.Connectivity.Not_Connected.ToString().Replace('_', ' ');

                        connectToPortControl.btnConnectStatus = true;
                        connectToPortControl.InvokeConnectDisplay();
                        connectToPortControl.btnDisConnectStatus = false;
                        connectToPortControl.InvokeDisconnectDisplay();

                        BaseClass.SelectRoute = false;
                        BaseClass.SelectRouteStatus = false;
                        BaseClass.DeselectRoute = false;
                        BaseClass.DeselectRouteStatus = false;
                        BaseClass.RouteStatus = false;
                        BaseClass.GPRSMode = false;
                        Constants.IsModemConnected = false;
                        Constants.IsConnectingToModem = false;

                    }
                }
                //ATE0 Eco command Succeed below code will excute
                else if (Constants.CurrentRequest == Constants.Commands.ATE0.ToString())
                {
                    if (Constants.receivingInprocess)
                    {
                        BaseClass.CalculateCounterTime();

                        if (BaseClass.timeCounter <= BaseClass.milliTime)
                        {
                            Constants.IsResponseSuccess = false;
                            Constants.receivingInprocess = false;
                            Stop_Reset_D_Counter();
                        }
                    }
                    else if (!Constants.receivingInprocess && Constants.IsResponseSuccess)
                    {
                        Stop_Reset_D_Counter();
                        BaseClass.SRPortComm.CheckSIMRegistartion();
                        BaseClass.timeCounter = 5000;
                        BaseClass.counterWatch.Start();
                    }
                    //ATE0 Eco command fail below code will excute
                    else if (!Constants.receivingInprocess && !Constants.IsResponseSuccess)
                    {
                        Stop_Reset_D_Counter();
                        Constants.IsConnectingToModem = false;
                    }
                }
                //CREG command Succeed below code will excute
                else if (Constants.CurrentRequest == Constants.Commands.CREG.ToString())
                {
                    if (Constants.receivingInprocess)
                    {
                        BaseClass.CalculateCounterTime();

                        if (BaseClass.timeCounter <= BaseClass.milliTime)
                        {
                            Constants.IsResponseSuccess = false;
                            Constants.receivingInprocess = false;
                            Stop_Reset_D_Counter();
                        }
                    }
                    else if (!Constants.receivingInprocess && Constants.IsResponseSuccess)
                    {
                        Stop_Reset_D_Counter();
                        BaseClass.SelectRoute = true;
                        BaseClass.SelectRouteStatus = true;
                        BaseClass.DeselectRoute = true;
                        BaseClass.DeselectRouteStatus = true;
                        BaseClass.RouteStatus = true;

                        Constants.CommunicationMode = BaseClass.CommunicationMode.SMS.ToString();
                        BaseClass.SRPortComm.CheckGPRSService();

                        BaseClass.timeCounter = 10000;
                        BaseClass.counterWatch.Start();

                        connectToPortControl.btnConnectStatus = false;
                        connectToPortControl.InvokeConnectDisplay();
                        connectToPortControl.btnDisConnectStatus = true;
                        connectToPortControl.InvokeDisconnectDisplay();

                        Constants.IsConnectingToModem = false;
                        Constants.ConnectionStatus = Constants.Connectivity.Connected.ToString();
                        MessageBox.Show("Connection Established Successfully");
                        
                        Constants.IsModemConnected = true;
                        connectToPortControl.InvokeCurrentUCDisplay();
                    }
                    //ATE0 Eco command fail below code will excute
                    else if (!Constants.receivingInprocess && !Constants.IsResponseSuccess)
                    {
                        Stop_Reset_D_Counter();
                        MessageBox.Show("SIM not registered");
                        BaseClass.SelectRoute = false;
                        BaseClass.SelectRouteStatus = false;
                        BaseClass.DeselectRoute = false;
                        BaseClass.DeselectRouteStatus = false;
                        BaseClass.RouteStatus = false;
                        BaseClass.GPRSMode = false;
                        Constants.IsConnectingToModem = false;
                        Constants.CurrentRequest = string.Empty;
                    }
                }
                //CGATT command succeed below code will execute
                else if (Constants.CurrentRequest == Constants.Commands.CGATT.ToString())
                {
                    if (Constants.receivingInprocess)
                    {
                        BaseClass.CalculateCounterTime();

                        if (BaseClass.timeCounter <= BaseClass.milliTime)
                        {
                            Constants.IsResponseSuccess = false;
                            Constants.receivingInprocess = false;
                            Stop_Reset_D_Counter();
                            BaseClass.GPRSMode = false;

                            Constants.IsConnectingToModem = false;
                        }
                    }
                    else if (!Constants.receivingInprocess && Constants.IsResponseSuccess)
                    {
                        BaseClass.GPRSMode = true;                     
                        this.gprsConnectControl.Visible = false;
                        Stop_Reset_D_Counter();

                        Constants.IsConnectingToModem = false;
                    }
                    else if (!Constants.receivingInprocess && !Constants.IsResponseSuccess)
                    {
                        BaseClass.GPRSMode = false; 
                        Stop_Reset_D_Counter();
                        Constants.IsConnectingToModem = false;
                    }
                }
            }
            #endregion

            #region  Send messages
            ////while SMS sending process this block will execute       

            //else if (BaseClass.CurrentProcess == Constants.Commands.MessageSend.ToString())
            //{
            //    if (BaseClass.IsSendOperationInprocess)
            //    {
            //        if (Constants.receivingInprocess)
            //        {
            //            BaseClass.CalculateCounterTime();

            //            if (BaseClass.timeCounter <= BaseClass.milliTime)
            //            {
            //                Constants.IsResponseSuccess = false;
            //                Constants.receivingInprocess = false;
            //                Constants.IsConnectionBreak = true;
            //                Stop_Reset_D_Counter();
            //                Constants.CurrentRequest = string.Empty;
            //                Constants.MessageSendStatus = Constants.MessageSendProcess.Fail.ToString();
            //                setRouteControl.InvokeGrid();

            //                if (BaseClass.SentMsgCount < 3)
            //                {
            //                    setRouteControl.SMSProcess();
            //                }
            //                else if (BaseClass.SentMsgCount == 3)
            //                {
            //                    setRouteControl.InvokeGrid();
            //                }
            //                else if (BaseClass.ListSelectedDgvRows.Count != 0)
            //                {
            //                    Constants.MessageSendStatus = string.Empty;
            //                    setRouteControl.SMSProcess();
            //                }
            //            }
            //        }
            //        else if (Constants.MessageSendStatus == Constants.MessageSendProcess.Msg_Sent.ToString().Replace('_', ' '))// (!Constants.receivingInprocess && Constants.IsResponseSuccess)
            //        {
            //            Stop_Reset_D_Counter();
            //            setRouteControl.InvokeGrid();

            //            string xx = Constants.CurrentRequest;
            //            if (BaseClass.ListSelectedDgvRows.Count != 0)
            //            {
            //                Constants.MessageSendStatus = string.Empty;
            //                setRouteControl.SMSProcess();
            //            }

            //        }
            //        else if (Constants.MessageSendStatus == Constants.MessageSendProcess.Fail.ToString())
            //        {
            //            Constants.IsResponseSuccess = false;
            //            Constants.receivingInprocess = false;
            //           Stop_Reset_D_Counter();
            //            Constants.CurrentRequest = string.Empty;

            //            if (BaseClass.SentMsgCount < 3)
            //            {
            //                setRouteControl.SMSProcess();
            //            }
            //            else if (BaseClass.SentMsgCount == 3)
            //            {
            //                setRouteControl.InvokeGrid();
            //            }
            //            else if (BaseClass.ListSelectedDgvRows.Count != 0)
            //            {
            //                Constants.MessageSendStatus = string.Empty;
            //                setRouteControl.SMSProcess();
            //            }
            //        }

            //        #region Stop the SMS sending process
            //        if (Constants.IsProcessCancelled || Constants.IsConnectionBreak)
            //        {
            //            BaseClass.ListSelectedDgvRows.Clear();
            //            setRouteControl.msg = string.Empty;
            //            setRouteControl.InvokeStatus();
            //            setRouteControl.SetControlStatus(true);
            //            BaseClass.CurrentProcess = string.Empty;
            //            BaseClass.IsSendOperationInprocess = false;
            //            Constants.IsProcessCancelled = false;
            //        }
            //        #endregion
            //    }
            //}

            #endregion

            #region Read Messages
            else if (Constants.CurrentRequest == Constants.Commands.UnRead.ToString())
            {
                if (Constants.receivingInprocess)
                {
                    BaseClass.CalculateCounterTime();

                    if (BaseClass.timeCounter <= BaseClass.milliTime)
                    {
                        Constants.IsResponseSuccess = false;
                        Constants.receivingInprocess = false;
                        Stop_Reset_D_Counter();
                        Constants.CurrentRequest = string.Empty;
                    }
                }
                 if (Constants.listMessages.Count > 0)// (!Constants.receivingInprocess && Constants.IsResponseSuccess)
                {
                    Constants.IsResponseSuccess = false;
                    Constants.receivingInprocess = false;
                    Stop_Reset_D_Counter();
                   
                        string messages = "";
                        for (int msgcount = 0; msgcount < Constants.listMessages.Count; msgcount++)
                        {
                            SplitMessage msgModel = Constants.listMessages[msgcount];
                            messages = messages + Environment.NewLine + msgModel.Sender + ", " + msgModel.Sent + ", " + msgModel.Message + Environment.NewLine;
                            receivedMsg = messages;

                            if (msgModel.Message.Contains("Connect GPRS"))
                            {
                                
                                string sMsg = msgModel.Message;
                                string[] splitm = sMsg.Split(':');
                                if (splitm.Length == 3)
                                {
                                    string[] splitcomma = splitm[1].ToString().Split(',');
                                    if (splitcomma.Length == 2)
                                    {
                                        Constants.GPRSServerIPAddress = splitcomma[0].ToString().Trim();
                                    }

                                    Constants.GPRSPortNo = splitm[2].ToString().Trim();
                                }

                                lblServerIPAddress.Text = "Server IP: " + Constants.GPRSServerIPAddress.ToString();
                                lblPortNo.Text = "Port: " + Constants.GPRSPortNo;                               
                            }
                        }

                        if (communicationControl.Visible)
                        {
                            InvokeResponseSMS();
                        }

                        Constants.CurrentRequest = Constants.Commands.Delete.ToString();
                        BaseClass.SRPortComm.DeleteMessage();
                    
                }
            }
            #endregion

            #region Delete Messages
            else if (Constants.CurrentRequest == Constants.Commands.Delete.ToString())
            {
                if (Constants.receivingInprocess)
                {
                    BaseClass.CalculateCounterTime();

                    if (BaseClass.timeCounter <= BaseClass.milliTime)
                    {
                        Constants.IsResponseSuccess = false;
                        Constants.receivingInprocess = false;
                        Stop_Reset_D_Counter();
                        Constants.CurrentRequest = string.Empty;
                    }
                }
                else if  (!Constants.receivingInprocess && Constants.IsResponseSuccess)
                {
                    Stop_Reset_D_Counter();
                    Constants.CurrentRequest = string.Empty;

                    #region start connecting to GPRS
                   gprsConnectControl.ConnectToGPRS();
                    #endregion
                }
            }
            #endregion

            #region New message  
            //If application found any new message, then it will start reading received messages
            else if (Constants.IsNewMsg && !BaseClass.IsSendOperationInprocess)
            {
                MessagesReadingProcess();
                Constants.IsNewMsg = false;
            }
            #endregion

            #region Check connection breaks
            else if (Constants.IsConnectionBreak)
            {
                InvokeCheckConnection();
            }
            #endregion

            #region Broken connectin established
            //else if (Constants.IsBrokenConnectionEstablished)
            //{
            //    // InvokeConnectBrokenConnection();
            //    BaseClass.timeCounter = 5000;
            //    BaseClass.SRPortComm.CommunicationTestCommand();
            //    BaseClass.counterWatch.Start();
            //}
            #endregion

            #region At Command response when broken connection Established
            else if (Constants.IsBrokenConnectionEstablished && Constants.CurrentRequest == Constants.Commands.AT.ToString())
            {
                if (Constants.receivingInprocess)
                {
                    BaseClass.CalculateCounterTime();

                    if (BaseClass.timeCounter <= BaseClass.milliTime)
                    {
                        Constants.IsResponseSuccess = false;
                        Constants.receivingInprocess = false;
                        Stop_Reset_D_Counter();
                        Constants.CurrentRequest = string.Empty;

                        Constants.IsConnectionBreak = true;

                    }
                }
                else if (!Constants.receivingInprocess && Constants.IsResponseSuccess)
                {
                    Stop_Reset_D_Counter();
                    Constants.CurrentRequest = string.Empty;

                    Constants.IsBrokenConnectionEstablished = false;
                    Constants.ConnectionStatus = Constants.Connectivity.Connected.ToString();
                }

            }
            #endregion            

            #region GPRS Connectivity
            //while GPRS connectivity process started, below block will execute
            else if (Constants.IsGPRSConnectivityStarted)
            {
                if (Constants.receivingInprocess)
                {
                    BaseClass.CalculateCounterTime();

                    if (BaseClass.timeCounter <= BaseClass.milliTime)
                    {
                        Constants.IsResponseSuccess = false;
                        Constants.receivingInprocess = false;
                        Constants.IsConnectionBreak = true;
                        Stop_Reset_D_Counter();
                        Constants.CurrentRequest = string.Empty;
                        Constants.GPRSConnectionStatus = Constants.GPRSConnection.Fail.ToString();
                    }
                }
                //After successfully GPRS connected, below block will execute
                else if (!Constants.receivingInprocess && Constants.GPRSConnectionStatus == Constants.GPRSConnection.ClientGPRSReady.ToString())
                {
                    Stop_Reset_D_Counter();
                    gprsConnectControl.ConnectivityProcessMsg = string.Empty;
                    Constants.IsGPRSConnectivityStarted = false;
                   // gprsConnectControl.InvokeGPRSConnectivity();
                    //gprsConnectControl.InvokeIPAddress();
                   // gprsConnectControl.InvokeSignalStrength();

                   
                    lblIPAddress.Text = "IP: " + Constants.GPRSIPAddress;
                 

                    result = MessageBox.Show("GPRS Connection Established", "", MessageBoxButtons.OK);
                    if (result == DialogResult.OK)
                    {
                        gprsConnectControl.Visible = false;
                        communicationControl.Visible = true;
                    }
                }
                else if (!Constants.receivingInprocess && Constants.GPRSConnectionStatus == Constants.GPRSConnection.Fail.ToString())
                {
                    Stop_Reset_D_Counter();
                    gprsConnectControl.ConnectivityProcessMsg = string.Empty;
                    Constants.IsGPRSConnectivityStarted = false;
                    gprsConnectControl.InvokeGPRSConnectivity();
                    result = MessageBox.Show("GPRS Connection Fail", "", MessageBoxButtons.OK);

                }
            }
            #endregion

            #region Server GPRS in Listening mode ...
            else if (Constants.GPRSConnectionStatus == Constants.GPRSConnection.ClientGPRSReady.ToString())
            {
                 if ( !string.IsNullOrEmpty(Constants.GPRSReceivedMsg))
                {   
                    communicationControl.rtxtRead.AppendText(Constants.GPRSReceivedMsg);
                    Constants.GPRSReceivedMsg = string.Empty;
                 }
                 else if (Constants.IsFileDownloaded)
                 {
                     Constants.IsFileDownloaded = false;
                     MessageBox.Show("File Downloaded Successfully");
                 }
                 else if (Constants.CurrentRequest == Constants.Commands.NonTransparentMode.ToString())
                 {
                     if (Constants.IsResponseSuccess && !Constants.receivingInprocess)
                     {Constants.CurrentRequest = string.Empty;
                         MessageBox.Show("Switch to Non Transparent mode");
                         
                     }
                 }
                 else if (Constants.CurrentRequest == Constants.Commands.ATO.ToString())
                 {
                     if (Constants.IsResponseSuccess && !Constants.receivingInprocess)
                     { Constants.CurrentRequest = string.Empty;
                         MessageBox.Show("Switch to Transparent mode");
                        
                     }
                 }
            }
            #endregion

            #region GPRS Disconnecting
            else if (Constants.IsGPRSDeactivated)
            {
                if (Constants.receivingInprocess)
                {
                    BaseClass.CalculateCounterTime();

                    if (BaseClass.timeCounter <= BaseClass.milliTime)
                    {
                        Constants.IsResponseSuccess = false;
                        Constants.receivingInprocess = false;
                        Constants.IsConnectionBreak = true;
                        Stop_Reset_D_Counter();
                        Constants.CurrentRequest = string.Empty;
                        Constants.GPRSConnectionStatus = Constants.GPRSConnection.Fail.ToString();
                    }
                }
                else if (Constants.GPRSConnectionStatus == Constants.GPRSConnection.Disconnected.ToString())
                {
                    Constants.IsGPRSDeactivated = false;
                    result = MessageBox.Show("GPRS connection Disconnected", "", MessageBoxButtons.OK);
                }               
            }
            #endregion
        }

       

        private static void Stop_Reset_D_Counter()
        {
            BaseClass.counterWatch.Stop();
            BaseClass.counterWatch.Reset();
        }

        private void readMsgsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
           
        }


        private void ConnectToServer()
        { 
        
        }

        private void sendMsgsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            communicationControl.Visible = true;
        }

        private void configToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void Main_Load(object sender, EventArgs e)
        {

            smsResponseDelegate = new SMSResponse(BindResponse);
            chkBrokenConDelegate = new CheckConnection(ConnectBrokenConnection);
        }

        private void gprsConnectControl_Load(object sender, EventArgs e)
        {

        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show("Are you sure to Disconnect the GPRS?", "Confirmation", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    BaseClass.SRPortComm.DisConnectToGPRS();
                    Constants.IsGPRSDeactivated = true;
                    // Constants.GPRSConnectionStatus = Constants.GPRSConnection.Disconnected.ToString();
                    // MessageBox.Show("GPRS connection Disconnected");
                }

            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
              
                    BaseClass.SRPortComm.SwitchToCommand();
                    Constants.IsGPRSDeactivated = true;
                   

            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {

                BaseClass.SRPortComm.SwitchToData();
                Constants.IsGPRSDeactivated = true;


            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }

        

       
    }
}
