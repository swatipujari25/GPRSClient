using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace GPRSClient
{
    public partial class ucConnectToPort : UserControl
    {
        public ucConnectToPort()
        {
            InitializeComponent();
        }
        public string statusMessage = string.Empty;
        public Color displayForeColor = Color.Green;

        public Boolean btnConnectStatus = true;
        public Boolean btnDisConnectStatus = false;

        #region Delegates Declaration and implementation

        public delegate void StatusMsg();
        public StatusMsg ConnectionStatusDelegate;

        public delegate void EnableControls();
        public EnableControls ConnectButtonDelegate;
        public EnableControls DisconnectButtonDelegate;
        public EnableControls currentUCDelegate;

        public delegate void BindPortControl();
        public BindPortControl BindPortControlDelegate;

        public delegate void MsgBoxDisplay();
        public MsgBoxDisplay MsgBoxDelegate;

        /// <summary>
        /// it will invoke to update the connection status
        /// </summary>
        public void InvokeCheckConnection()
        {
            try
            {
                lblStatus.Invoke(ConnectionStatusDelegate);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }

        /// <summary>
        /// It will invoke to update the connection status
        /// </summary>
        private void StatusUpdate()
        {
            try
            {
                lblStatus.Text = statusMessage;
                lblStatus.ForeColor = displayForeColor;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }

        /// <summary>
        /// It will invoke the Connect button visibility
        /// </summary>
        public void InvokeConnectDisplay()
        {
            try
            {
                btnConnect.Invoke(ConnectButtonDelegate);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }

        /// <summary>
        /// It will invoke the Connect button visibility
        /// </summary>
        private void DisplayOfConnectButton()
        {
            try
            {
                btnConnect.Enabled = btnConnectStatus;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }

        /// <summary>
        /// It will invoke the Disconnect button visibility
        /// </summary>
        public void InvokeDisconnectDisplay()
        {
            try
            {
                btnDisConnect.Invoke(DisconnectButtonDelegate);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }

        /// <summary>
        /// it will invoke the Disconnect button visibility
        /// </summary>
        private void DisplayOfDisconnectButton()
        {
            try
            {
                btnDisConnect.Enabled = btnDisConnectStatus;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }

        /// <summary>
        /// it will invoke the current usercontrol visibility
        /// </summary>
        public void InvokeCurrentUCDisplay()
        {
            try
            {
                this.Invoke(currentUCDelegate);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }

        /// <summary>
        /// it will invoke the current usercontrol visibility
        /// </summary>
        private void DisplayOfCurrentUC()
        {
            try
            {
                this.Visible = false;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }

        /// <summary>
        /// it will invoke to update the connection status
        /// </summary>
        public void InvokeBindPortControl()
        {
            try
            {
                cboPortNo.Invoke(BindPortControlDelegate);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }


        public void InvokeMessageBox()
        {
            try
            {
                this.Invoke(MsgBoxDelegate);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }

        public void ShowMsgBox()
        {
            try
            {
                MessageBox.Show(statusMessage);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }
        #endregion

        /// <summary>
        /// it will load when control first time loading
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ucConnectToPort_Load(object sender, EventArgs e)
        {
            try
            {
                ConnectionStatusDelegate = new StatusMsg(StatusUpdate);
                ConnectButtonDelegate = new EnableControls(DisplayOfConnectButton);
                DisconnectButtonDelegate = new EnableControls(DisplayOfDisconnectButton);
                currentUCDelegate = new EnableControls(DisplayOfCurrentUC);
                BindPortControlDelegate = new BindPortControl(BindComPorts);
                MsgBoxDelegate = new MsgBoxDisplay(ShowMsgBox);
                BindBaudRate();


                DefaultConnectedStatus();
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }

        public void DefaultConnectedStatus()
        {
            BindComPorts();
            DefaultSettings();
            if (Constants.ConnectionStatus == Constants.Connectivity.Connected.ToString())
            {

                statusMessage = "Connected to " + cboPortNo.SelectedItem.ToString();

                //   lblStatus.Text = "Connected to " + cboPortNo.SelectedItem.ToString();
                displayForeColor = Color.Green;
                InvokeCheckConnection();
                btnDisConnect.Enabled = true;
                btnConnect.Enabled = false;
                Constants.IsModemConnected = true;
            }
            else if (Constants.ConnectionStatus == Constants.Connectivity.Disconnected.ToString())
            {
                statusMessage = "Not Connected";
                displayForeColor = Color.Red;
                InvokeCheckConnection();
                Constants.IsModemConnected = false;
                btnDisConnect.Enabled = false;
                btnConnect.Enabled = true;
            }
            else
            {
                statusMessage = "Not Connected";
                displayForeColor = Color.Red;
                InvokeCheckConnection();
                Constants.IsModemConnected = false;
                btnDisConnect.Enabled = false;
                btnConnect.Enabled = true;
            }
        }

        /// <summary>
        /// Get all the sytem ports
        /// </summary>
        private void BindComPorts()
        {
            try
            {
                if (cboPortNo.Items.Count > 0)
                {
                    cboPortNo.Items.Clear();
                }
                string[] ports = SerialPort.GetPortNames();
                cboPortNo.Items.Add("Select");
                foreach (string port in ports)
                {
                    cboPortNo.Items.Add(port);
                }
                cboPortNo.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }

        /// <summary>
        /// list of the BaudRates
        /// </summary>
        private void BindBaudRate()
        {
            try
            {
                List<int> list = new List<int>();
                list.Add(110);
                list.Add(300);
                list.Add(1200);
                list.Add(2400);
                list.Add(4800);
                list.Add(9600);
                list.Add(19200);
                list.Add(38400);
                list.Add(57600);
                list.Add(115200);
                list.Add(230400);
                list.Add(460800);
                list.Add(921600);
                cboBitsPerSec.DataSource = list;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }

        /// <summary>
        /// Default form settings
        /// </summary>
        private void DefaultSettings()
        {
            try
            {
                if (Constants.ConnectionStatus == Constants.Connectivity.Connected.ToString())
                {
                    cboPortNo.SelectedIndex = cboPortNo.FindStringExact(BaseClass.SRPortComm.PortName.ToString());
                    cboBitsPerSec.SelectedIndex = cboBitsPerSec.FindStringExact(BaseClass.SRPortComm.BaudRt.ToString());
                }
                else
                {
                    cboBitsPerSec.SelectedItem = 19200;
                    cboPortNo.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }

        /// <summary>
        /// Check the control validations
        /// </summary>
        /// <returns></returns>
        private Boolean IsValid()
        {
            Boolean valid = true;
            try
            {
                //  if (string.IsNullOrEmpty(cboPortNo.SelectedItem.ToString()))
                if (cboPortNo.SelectedIndex == 0)
                {
                    lblMsg.Text = "Select COM Port ";
                    lblStatus.Text = string.Empty;
                    valid = false;
                }
                else
                {
                    lblMsg.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
            return valid;
        }

        /// <summary>
        /// it triggers when user clicks on connect button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {


                if (IsValid())
                {
                    Constants.IsConnectingToModem = true;
                    statusMessage = "Connectivity InProcess....";
                    displayForeColor = Color.Green;
                    InvokeCheckConnection();
                    btnConnectStatus = false;
                    InvokeConnectDisplay();

                    BaseClass.SRPortComm.PortName = cboPortNo.SelectedItem.ToString();
                    BaseClass.SRPortComm.BaudRt = Convert.ToInt32(cboBitsPerSec.SelectedItem);
                    BaseClass.SRPortComm.DataBits = 8;

                    ConnectToPort();
                    //BaseClass.messageThread = new Thread(new ThreadStart(() => ConnectToPort()));
                    //BaseClass.messageThread.Start();                   
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }

        /// <summary>
        /// Connect to serial port
        /// </summary>
        /// <param name="portNo"> serial port</param>
        /// <param name="baudrt">BaudRate for serial port connectivity</param>
        public void ConnectToPort()
        {
            BaseClass.SRPortComm.Close();
            BaseClass.SRPortComm.SerialPortSettings(BaseClass.SRPortComm.PortName, BaseClass.SRPortComm.BaudRt,
                "None", BaseClass.SRPortComm.DataBits, 1.0);
            //   BaseClass.SRPortComm.SerialPortSettings(portNo,baudrt , "None", 8, 1);

            if (BaseClass.SRPortComm.Connect())
            {
                
                statusMessage = "Connected to " + BaseClass.SRPortComm.PortName;
                displayForeColor = Color.Green;
                InvokeCheckConnection();
                
                BaseClass.timeCounter = 5000;
                BaseClass.counterWatch.Start();

               BaseClass.SRPortComm.CommunicationTestCommand();              
            }
            else
            {
                if (!Constants.IsPortExist)
                {
                    statusMessage = "Port not exist";
                    //    InvokeBindPortControl();
                    Constants.IsPortExist = true;
                }
                else
                {
                    statusMessage = "Not Connected";
                }
                displayForeColor = Color.Red;
                InvokeCheckConnection();

                Constants.ConnectionStatus = Constants.Connectivity.Disconnected.ToString();
                btnConnectStatus = true;
                InvokeConnectDisplay();
                btnDisConnectStatus = false;
                InvokeDisconnectDisplay();

                //statusMessage = string.Empty;
                //displayForeColor = Color.Green;
                //InvokeCheckConnection();               

                BaseClass.SelectRoute = false;
                BaseClass.SelectRouteStatus = false;
                BaseClass.DeselectRoute = false;
                BaseClass.DeselectRouteStatus = false;
                BaseClass.RouteStatus = false;
                BaseClass.GPRSMode = false;
                Constants.IsModemConnected = false;
                MessageBox.Show("Connection Fail");
                Constants.IsConnectingToModem = false;
            }
           
            Constants.GSMConnectionStatus = lblStatus.Text;
        }

        /// <summary>
        /// Disconnect the serial port connectivity
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDisConnect_Click(object sender, EventArgs e)
        {
            try
            {

                BaseClass.SRPortComm.Close();
                statusMessage = "DisConnected";
                displayForeColor = Color.Red;
                InvokeCheckConnection();

                Constants.ConnectionStatus = Constants.Connectivity.Disconnected.ToString();
                btnDisConnect.Enabled = false;

                btnConnect.Enabled = true;
               
                BaseClass.SelectRoute = false;
                BaseClass.SelectRouteStatus = false;
                BaseClass.DeselectRoute = false;
                BaseClass.DeselectRouteStatus = false;
                BaseClass.RouteStatus = false;
                BaseClass.GPRSMode = false;
                Constants.IsModemConnected = false;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }

        /// <summary>
        /// Close the current user control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }
    }
}
