using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace GPRSClient
{
    public partial class ucGPRSConnect : UserControl
    {
        public ucGPRSConnect()
        {
            InitializeComponent();
        }

        public string ConnectivityProcessMsg = string.Empty;

        public delegate void GPRSConnectivity();
        public GPRSConnectivity gprsConnectivityDelegate;

        public GPRSConnectivity IPAddressDelegate;

        public GPRSConnectivity SignalStrengthDelegate;


        /// <summary>
        /// Display information while connecting to GPRS
        /// </summary>
        private void GPRSConnectivityProcess()
        {
            try
            {
                lblMsg.Text = ConnectivityProcessMsg;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }

        /// <summary>
        /// invoke GPRS connectivity
        /// </summary>
        public void InvokeGPRSConnectivity()
        {
            try
            {
                lblMsg.Invoke(gprsConnectivityDelegate);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }

        /// <summary>
        /// Display IPAddress
        /// </summary>
        private void GPRSIPAddress()
        {
            try
            {
                if (!string.IsNullOrEmpty(Constants.GPRSServerIPAddress))
                {
                    lblIPAddress.Text = "IP Address : " + Constants.GPRSServerIPAddress;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }

        /// <summary>
        /// invoke IPAddress
        /// </summary>
        public void InvokeIPAddress()
        {
            try
            {
                lblIPAddress.Invoke(IPAddressDelegate);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }


        /// <summary>
        /// Display Signal strength
        /// </summary>
        private void GPRSSignalStrength()
        {
            try
            {
                if (!string.IsNullOrEmpty(Constants.GPRSSignalStrength))
                {
                    lblSignalStrength.Text = "Signal Strength : " + Constants.GPRSSignalStrength;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }

        /// <summary>
        /// invoke Signal Strength
        /// </summary>
        public void InvokeSignalStrength()
        {
            try
            {
                lblSignalStrength.Invoke(SignalStrengthDelegate);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }



        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                ConnectToServer();
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }

        public void ConnectToServer()
        {
            if (ValidateCoach())
            {
                DialogResult result = MessageBox.Show("Are you sure to connect the GPRS?", "Confirmation", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    ConnectivityProcessMsg = "Please wait, GPRS Connecting...";
                    InvokeGPRSConnectivity();

                    Constants.GPRSServerIPAddress = txtIPAddress.Text;
                    Constants.GPRSPortNo = txtPortNo.Text;
                    ConnectToGPRS();
                }
                else
                {

                }
            }
        }


        public  void ConnectToGPRS()
        {
            if (!string.IsNullOrEmpty(Constants.GPRSServerIPAddress) && !string.IsNullOrEmpty(Constants.GPRSPortNo))
            {
                Constants.IsGPRSConnectivityStarted = true;
                Constants.GPRSConnectionStatus = string.Empty;
                Constants.CurrentRequest = Constants.Commands.CIPSHUT.ToString();
                BaseClass.SRPortComm.ConnectToGPRS();

                BaseClass.timeCounter = 180000;
                BaseClass.counterWatch.Start();
            }
        }

        public Boolean ValidateCoach()
        {
            Boolean isValid = true;

            try
            {
                // if (cboTrainNo.SelectedValue =="0")
                if (string.IsNullOrEmpty(txtPortNo.Text))
                {
                    isValid = false;
                    MessageBox.Show("Enter Port No");
                    txtPortNo.Focus();
                }

            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
            return isValid;
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

        private void btnClose_Click(object sender, EventArgs e)
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
        /// Restrict the user about to enter Invalid data in textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPortNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (char.IsLetterOrDigit(e.KeyChar) || e.KeyChar == '\b')
                {
                    e.Handled = false;
                }
                else
                {
                    MessageBox.Show("Please enter valid Port No");
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }

        private void ucGPRSConnect_Load(object sender, EventArgs e)
        {
            try
            {
                gprsConnectivityDelegate = new GPRSConnectivity(GPRSConnectivityProcess);
                IPAddressDelegate = new GPRSConnectivity(GPRSIPAddress);
                SignalStrengthDelegate = new GPRSConnectivity(GPRSSignalStrength);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }
    }
}
