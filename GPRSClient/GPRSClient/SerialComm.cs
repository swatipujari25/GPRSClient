// <copyright file="SerialComm.cs" company="Sirveen Control System">
// Copyright (c) 2016 All Rights Reserved
// <author>Swati P</author>
// <date>12/02/2016 13:15 </date>
// <summary>This class contains code related to Serial Port communication</summary>
// </copyright>

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace GPRSClient
{
    public class SerialComm
    {
        #region MEMBER VARIABLES
        public SerialPort _serialPort = null;
        Boolean success = false;
        string buffer = string.Empty;
        int count = 0;
        string recievedData = string.Empty;
        String command = string.Empty;

        string _PortName = string.Empty;
        int _baudRt = 0;
        Parity _parity = Parity.None;
        int _DataBits = 8;
        StopBits _stopBits = StopBits.One;
        Boolean _isGPRS = false;

        #endregion

        #region PROPERTIES DECLARATION

        public Boolean IsGPRS
        {
            get { return _isGPRS; }
            set { _isGPRS = value; }
        }

        public StopBits StopBits
        {
            get { return _stopBits; }
            set { _stopBits = value; }
        }

        public int DataBits
        {
            get { return _DataBits; }
            set { _DataBits = value; }
        }

        public string PortName
        {
            get { return _PortName; }
            set { _PortName = value; }
        }

        public int BaudRt
        {
            get { return _baudRt; }
            set { _baudRt = value; }
        }

        public Parity pParity
        {
            get { return _parity; }
            set { _parity = value; }
        }

        #endregion

        #region CONNECTIVITY

        /// <summary>
        /// This method initializing the object for serial port object with the all configuration settings,
        /// which are selected by user in front end application.
        /// </summary>
        /// <param name="portName"></param>
        /// <param name="BaudRate"></param>
        /// <param name="_sparity"></param>
        /// <param name="DataBitsPerSecond"></param>
        /// <param name="_stopBits"></param>
        public void SerialPortSettings(string portName, int BaudRate, string _sparity, int DataBitsPerSecond,
           double _stopBits)
        {
            try
            {
                Parity parityVal = Parity.None;
                if (_sparity == Parity.Even.ToString())
                {
                    parityVal = Parity.Even;
                }
                else if (_sparity == Parity.Mark.ToString())
                {
                    parityVal = Parity.Mark;
                }
                else if (_sparity == Parity.None.ToString())
                {
                    parityVal = Parity.None;
                }
                else if (_sparity == Parity.Odd.ToString())
                {
                    parityVal = Parity.Odd;
                }
                else if (_sparity == Parity.Space.ToString())
                {
                    parityVal = Parity.Space;
                }

                StopBits stopbitVal = StopBits.One;
                if (_stopBits == 1)
                {
                    stopbitVal = StopBits.One;
                }
                else if (_stopBits == 1.5)
                {
                    stopbitVal = StopBits.OnePointFive;
                }
                else if (_stopBits == 2)
                {
                    stopbitVal = StopBits.Two;
                }

                PortName = portName;
                BaudRt = BaudRate;
                pParity = parityVal;
                DataBits = DataBitsPerSecond;
                StopBits = stopbitVal;

                _serialPort = new SerialPort(portName, BaudRate, parityVal, DataBitsPerSecond, stopbitVal);
                _serialPort.Handshake = Handshake.None;
                _serialPort.DtrEnable = true;
                _serialPort.RtsEnable = true;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }

        /// <summary>
        /// This method will Connect with the serial port and generate handler event 
        /// to receive the data from Serial port.
        /// </summary>
        /// <returns></returns>
        public Boolean Connect()
        {
            try
            {
                if (_serialPort != null)
                {
                    // _serialPort.BreakState = true;
                    if (!_serialPort.IsOpen)
                    {
                        _serialPort.Open();
                        _serialPort.DataReceived += new SerialDataReceivedEventHandler(sp_DataReceived);
                    }
                }
                else
                {

                    _serialPort = new SerialPort(PortName, BaudRt, pParity, DataBits, StopBits);
                    _serialPort.DtrEnable = true;
                    _serialPort.RtsEnable = true;
                    _serialPort.Handshake = Handshake.None;
                    _serialPort.Open();
                    _serialPort.DataReceived += new SerialDataReceivedEventHandler(sp_DataReceived);
                }

            }
            catch (System.IO.IOException ex)
            {
                Constants.IsPortExist = false;
                Logger.WriteLog(ex);

            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }

            return _serialPort.IsOpen;
        }


        /// <summary>
        /// This method will Disconnect the serial port communication
        /// </summary>
        public void Close()
        {
            try
            {
                if (_serialPort != null)
                {
                    //_serialPort.Dispose();
                    GC.SuppressFinalize(_serialPort);
                    _serialPort.Close();
                    _serialPort.DataReceived -= new SerialDataReceivedEventHandler(sp_DataReceived);
                    _serialPort = null;

                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }
        #endregion

        #region GPRS CONNECTIVITY


        /// <summary>
        /// Sending command to connect with GPRS
        /// </summary>
        /// <returns></returns>
        public void DisConnectToGPRS()
        {
            try
            {
                Constants.CurrentRequest = Constants.Commands.Close_CIPSERVER.ToString();
                CallingGPRSConnectivityProcess();
                Constants.IsGPRSConnectivityStarted = false;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }


        }

        /// <summary>
        /// Sending command to connect with GPRS
        /// </summary>
        /// <returns></returns>
        public void ConnectToGPRS()
        {
            try
            {
                CallingGPRSConnectivityProcess();

                Constants.IsGPRSConnectivityStarted = false;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }

        public void CheckSIMRegistartion()
        {
            try
            {
                success = false;
                buffer = string.Empty;
                Constants.CurrentRequest = Constants.Commands.CREG.ToString();
                command = GetCommand(string.Empty);
                recievedData = ExecuteCommand(command);
                Constants.receivingInprocess = true;
                WaitingForResponse(10);

                if (success)
                {
                    Constants.CurrentRequest = Constants.Commands.CSQ.ToString();
                    buffer = string.Empty;
                    command = GetCommand(string.Empty);
                    recievedData = ExecuteCommand(command);
                    Constants.receivingInprocess = true;
                    WaitingForResponse(30);
                    success = false;
                }

            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }

        public void CheckGPRSService()
        {
            try
            {
                success = false;
                buffer = string.Empty;
                Constants.CurrentRequest = Constants.Commands.CGATT.ToString();
                command = GetCommand(string.Empty);
                recievedData = ExecuteCommand(command);
                Constants.receivingInprocess = true;
                WaitingForResponse(10);
                
                    success = false;
               

            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }


        private void CallingGPRSConnectivityProcess()
        {
            try{
            Constants.Commands commandVal = Constants.Commands.AT;
            success = false;
            buffer = string.Empty;
            foreach (Constants.Commands value in Enum.GetValues(typeof(Constants.Commands)))
            {
                if (Constants.CurrentRequest == Convert.ToString(value))
                {
                    commandVal = value;
                    break;
                }
            }

            switch (commandVal)
            {
                case Constants.Commands.CIPSHUT:
                    CommandSendProcess(Constants.Commands.CGATT.ToString());
                    Constants.PreviousRequest = string.Empty;
                    break; 

                case Constants.Commands.CGATT:
                    CommandSendProcess(Constants.Commands.Open_CIPSERVER.ToString());
                    break;

                case Constants.Commands.Open_CIPSERVER:
                    CommandSendProcess(Constants.Commands.CIFSR.ToString(), Constants.GPRSPortNo.ToString());
                    break;

                case Constants.Commands.CIFSR:
                    CommandSendProcess(Constants.Commands.CIPSTATUS.ToString());
                    break;

                case Constants.Commands.CIPSTATUS:
                    CommandSendProcess(string.Empty);
                    break;               

                case Constants.Commands.Close_CIPSERVER:
                    CommandSendProcess(string.Empty);

                    break;
                case Constants.Commands.CIPCLOSE:
                    CommandSendProcess(string.Empty);
                    break;

            }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }

        /// <summary>
        /// Transmit the commands towards serial port for GPRS connection
        /// </summary>
        /// <param name="nextCommand"></param>
        /// <param name="parameter"></param>
        private void CommandSendProcess(string nextCommand, string parameter)
        {
            try
            {
                if (Constants.PreviousRequest != nextCommand)
                {
                    Constants.PreviousRequest = Constants.CurrentRequest;
                    command = GetCommand(parameter);
                    recievedData = ExecuteCommand(command);
                    Constants.receivingInprocess = true;
                    WaitingForResponse(30);

                    if (success)
                    {
                        Constants.CurrentRequest = nextCommand;
                    }

                    CallingGPRSConnectivityProcess();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }


        private void CommandSendProcess(string nextCommand)
        {
            try
            {
                if (Constants.PreviousRequest != nextCommand)
                {
                    Constants.PreviousRequest = Constants.CurrentRequest;
                    command = GetCommand(string.Empty);
                    recievedData = ExecuteCommand(command);
                    Constants.receivingInprocess = true;
                    WaitingForResponse(30);

                    if (success)
                    {
                        Constants.CurrentRequest = nextCommand;
                    }

                    CallingGPRSConnectivityProcess();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }
        #endregion

        /// <summary>
        /// This function will call to transmit command to  serial port.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public string ExecuteCommand(string command)
        {
            string input = string.Empty;
            try
            {
                _serialPort.DiscardOutBuffer();
                _serialPort.DiscardInBuffer();
                command = command + "\r";
                _serialPort.Write(command);
                Logger.Messages("TX :" + command);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
            return input;
        }

        public void sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string rData = string.Empty;

            try
            {
                if (e.EventType == SerialData.Chars)
                {
                    try
                    {
                       // Thread.Sleep(300);

                        rData = _serialPort.ReadExisting();

                        if (rData.Contains('\r') || rData.Contains('\n'))
                        {
                            var charsToRemove = new string[] { "\r", "\n" };
                            foreach (var c in charsToRemove)
                            {
                                rData = rData.Replace(c, string.Empty);
                            }
                        }
                        // _serialPort.DiscardInBuffer();
                        buffer += rData;

                        string dd = buffer;
                        Logger.Messages("RX :" + rData);

                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLog(ex);
                    }
                    ProcessReceivedCommand(buffer);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }

        }

        /// <summary>
        /// Process the received data 
        /// </summary>
        /// <param name="processbuffer"></param>
        private void ProcessReceivedCommand(string processbuffer)
        {
            try
            {
                #region SMS Commands
                //Test command
                if (Constants.CurrentRequest == Constants.Commands.AT.ToString() &&
                    processbuffer.EndsWith("OK"))
                {
                    Constants.receivingInprocess = false;
                    success = true;
                }
                if (Constants.CurrentRequest == Constants.Commands.AT.ToString() &&
                processbuffer.EndsWith(">"))
                {
                    Constants.receivingInprocess = false;
                    success = false;
                    ExecuteCommand(char.ConvertFromUtf32(26));
                }
                //Eco off command
                else if (Constants.CurrentRequest == Constants.Commands.ATE0.ToString() &&
                    processbuffer.EndsWith("OK"))
                {
                    Constants.receivingInprocess = false;
                    success = true;
                }
                //valid response of +CMGS command
                else if (Constants.CurrentRequest == Constants.Commands.CMGS.ToString() &&
                     processbuffer.EndsWith("> "))
                {
                    Constants.receivingInprocess = false;
                    success = true;
                }
                //invalid response of +CMGS command
                else if (Constants.CurrentRequest == Constants.Commands.CMGS.ToString() &&
                     processbuffer.EndsWith("ERROR"))
                {
                    Constants.receivingInprocess = false;
                    success = false;
                }
                //valid response after message sent
                else if ((Constants.CurrentRequest == Constants.Commands.SelectRoute.ToString()
                    || Constants.CurrentRequest == Constants.Commands.DeselectRoute.ToString()
                    || Constants.CurrentRequest == Constants.Commands.RouteStatus.ToString())
                    && processbuffer.EndsWith("OK"))
                {
                    Constants.receivingInprocess = false;
                    Constants.IsMessageSent = true;
                    success = true;
                }
                //Invalid response after message sent
                else if ((Constants.CurrentRequest == Constants.Commands.SelectRoute.ToString()
                    || Constants.CurrentRequest == Constants.Commands.DeselectRoute.ToString()
                    || Constants.CurrentRequest == Constants.Commands.RouteStatus.ToString())
                    && processbuffer.EndsWith("ERROR"))
                {
                    Constants.receivingInprocess = false;
                    Constants.IsMessageSent = false;
                    success = false;
                }
                //Valid Response of Read all Unread messages
                else if (Constants.CurrentRequest == Constants.Commands.UnRead.ToString()
                    && processbuffer.EndsWith("OK"))
                {
                    Constants.receivingInprocess = false;
                    Constants.listMessages = ReadResponseMessages(processbuffer);
                    success = true;
                }
                //Invalid Response of Read all Unread messages
                else if (Constants.CurrentRequest == Constants.Commands.UnRead.ToString()
                    && processbuffer.EndsWith("ERROR"))
                {
                    Constants.receivingInprocess = false;
                    Constants.listMessages = ReadResponseMessages(processbuffer);
                    success = true;
                }
                //Valid Response of Delete message
                else if (Constants.CurrentRequest == Constants.Commands.Delete.ToString()
                    && processbuffer.EndsWith("OK"))
                {
                    Constants.receivingInprocess = false;
                    success = true;
                }
                //Valid Response of Delete message
                else if (Constants.CurrentRequest == Constants.Commands.Delete.ToString()
                    && processbuffer.EndsWith("ERROR"))
                {
                    Constants.receivingInprocess = false;
                    success = true;
                }
                #endregion

                #region GPRS COMMANDS
                else if (Constants.CurrentRequest == Constants.Commands.CPIN.ToString() &&
                    buffer.Contains("+CPIN: READY") && buffer.EndsWith("OK"))
                {
                    Constants.receivingInprocess = false;
                    success = true;
                }
                else if (Constants.CurrentRequest == Constants.Commands.CSQ.ToString() &&
                  buffer.Contains("+CSQ:") && buffer.EndsWith("OK"))
                {
                    Constants.receivingInprocess = false;
                    success = true;
                    Regex r = new Regex(@"\+CSQ: (\d+),");
                    Match m = r.Match(buffer);

                    if (!m.Success)
                    {
                        m = r.Match(buffer);
                    }
                    if (m.Success)
                    {
                        Constants.GPRSSignalStrength = m.Groups[1].Value.ToString();
                    }
                }
                else if (Constants.CurrentRequest == Constants.Commands.CREG.ToString() &&  buffer.EndsWith("OK"))
                {   
                    Constants.IsSIMRegistered = true;  
                    Constants.receivingInprocess = false;
                    success = true;
                }
                else if (Constants.CurrentRequest == Constants.Commands.CREG.ToString() && buffer.EndsWith("ERROR"))
                {
                    Constants.IsSIMRegistered = false;
                    Constants.receivingInprocess = false;
                    success = false;
                }
                else if (Constants.CurrentRequest == Constants.Commands.CGATT.ToString() && buffer.EndsWith("OK"))
                {
                //    if (buffer.Contains("+CGATT: 1"))
                //    {
                        Constants.IsModemAttachedToService = true;
                   
                    Constants.receivingInprocess = false;
                    success = true;
                }
                else if (Constants.CurrentRequest == Constants.Commands.CGATT.ToString() && buffer.EndsWith("ERROR"))
                {
                    Constants.IsModemAttachedToService = false;
                    Constants.receivingInprocess = false;
                    success = false;
                }                
                else if (Constants.CurrentRequest == Constants.Commands.Open_CIPSERVER.ToString() &&  buffer.EndsWith("SERVER OK"))
                {
                    Constants.receivingInprocess = false;
                    success = true;
                }
                else if (Constants.CurrentRequest == Constants.Commands.Open_CIPSERVER.ToString() && buffer.EndsWith("ERROR"))
                {
                    Constants.GPRSConnectionStatus = Constants.GPRSConnection.Fail.ToString();
                    Constants.receivingInprocess = false;
                    success = false;
                    Constants.CurrentRequest = string.Empty;
                }
                //Close CIPSERVER
                else if (Constants.CurrentRequest == Constants.Commands.Close_CIPSERVER.ToString() &&  buffer.EndsWith("SERVER CLOSE"))
                {
                    Constants.receivingInprocess = false;
                    success = true;
                }
                else if (Constants.CurrentRequest == Constants.Commands.Close_CIPSERVER.ToString() && buffer.EndsWith("ERROR"))
                {
                    Constants.receivingInprocess = false;
                    success = false;
                    Constants.CurrentRequest = Constants.Commands.CIPSHUT.ToString();
                }
                //CIPCLOSE
                else if (Constants.CurrentRequest == Constants.Commands.CIPCLOSE.ToString() &&
                    buffer.EndsWith("CLOSE OK"))
                {
                    Constants.receivingInprocess = false;
                    success = true;
                }
                else if (Constants.CurrentRequest == Constants.Commands.CIPCLOSE.ToString() &&
            buffer.EndsWith("ERROR"))
                {
                    Constants.receivingInprocess = false;
                    success = false;

                }
                else if (Constants.CurrentRequest == Constants.Commands.CIFSR.ToString() && !buffer.EndsWith("OK"))
                {

                    Constants.GPRSIPAddress = buffer;
                    Constants.receivingInprocess = false;
                    success = true;
                }
                else if (Constants.CurrentRequest == Constants.Commands.CIFSR.ToString() &&
                     buffer.EndsWith("ERROR"))
                {
                    Constants.GPRSConnectionStatus = Constants.GPRSConnection.Fail.ToString();
                    Constants.receivingInprocess = false;
                    success = false;
                }
                else if (Constants.CurrentRequest == Constants.Commands.CIPSTATUS.ToString() &&
                    (buffer.EndsWith("OK") || buffer.EndsWith("STATE: SERVER LISTENING")))
                {
                    Constants.GPRSConnectionStatus = Constants.GPRSConnection.Connected.ToString();
                    Constants.receivingInprocess = false;
                    success = true;
                }
                else if (Constants.CurrentRequest == Constants.Commands.CIPSTATUS.ToString() &&
                     buffer.EndsWith("ERROR"))
                {
                    Constants.GPRSConnectionStatus = Constants.GPRSConnection.Fail.ToString();
                    Constants.receivingInprocess = false;
                    success = false;
                }
                else if (Constants.CurrentRequest == Constants.Commands.CIPSEND.ToString() &&
                     buffer.EndsWith("> "))
                {
                    Constants.receivingInprocess = false;
                    success = true;
                }
                else if (Constants.CurrentRequest == Constants.Commands.CIPSHUT.ToString() &&
                     buffer.EndsWith("SHUT OK"))
                {
                    Constants.receivingInprocess = false;
                    success = true;
                }

                if (processbuffer.Contains("+CPIN:"))//(processbuffer.EndsWith("+CPIN: READY"))
                {
                    Constants.IsBrokenConnectionEstablished = true;
                }
                //else
                #endregion

                IsNewMessageExist(processbuffer);

            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }

        /// <summary>
        /// Check weather serial port receiving any New message/SMS
        /// </summary>
        /// <param name="strbuffer"></param>
        private static void IsNewMessageExist(string strbuffer)
        {
            try
            {
                if (strbuffer.Contains("+CMTI: \"SM\","))
                {
                    Constants.IsNewMsg = true;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }

        private static void IsGPRSDeActivated(string strbuffer)
        {
            try
            {
                if (strbuffer.Contains("+PDP: DEACT"))
                {
                    Constants.IsGPRSDeactivated = true;
                    Constants.GPRSConnectionStatus = Constants.GPRSConnection.Disconnected.ToString();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }

        /// <summary>
        /// Check weather Modem connection is ready, which was already breaked
        /// </summary>
        /// <param name="strbuffer"></param>
        private void IsBrokenConnectionEstablished(string strbuffer)
        {
            try
            {
                if (strbuffer.Contains("+CPIN"))
                // if (strbuffer.Contains("Call Ready"))
                {
                    Constants.IsBrokenConnectionEstablished = true;

                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }

        /// <summary>
        /// transmit message to modem
        /// </summary>
        /// <param name="phoneNo"></param>
        /// <param name="Message"></param>
        public void SendMessage(string phoneNo, string Message)
        {
            string request = Constants.CurrentRequest;
            try
            {
                success = false;
                buffer = string.Empty;
                Constants.CurrentRequest = Constants.Commands.CMGS.ToString();
                command = GetCommand(phoneNo);
                recievedData = ExecuteCommand(command);
                Constants.receivingInprocess = true;
                WaitingForResponse(10);

                if (success)
                {
                    Constants.CurrentRequest = request;
                    buffer = string.Empty;
                    command = Message + char.ConvertFromUtf32(26);// +"\r";
                    recievedData = ExecuteCommand(command);
                    Constants.receivingInprocess = true;
                    WaitingForResponse(30);
                    success = false;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }

        /// <summary>
        /// Test AT commands
        /// </summary>
        /// <returns></returns>
        public Boolean CommunicationTestCommand()
        {
            try
            {
                #region AT COMMAND
                success = false;
                Constants.IsConnectionBreak = false;
                buffer = string.Empty;
                Constants.CurrentRequest = Constants.Commands.AT.ToString();
                command = GetCommand(string.Empty);
                recievedData = ExecuteCommand(command);
                Constants.receivingInprocess = true;
                WaitingForResponse(5);
                #endregion
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
            return success;
        }

        /// <summary>
        /// Wait mode to get the response from Serial port
        /// </summary>
        /// <param name="breakCount"></param>
        private void WaitingForResponse(int breakCount)
        {
            try
            {
                count = 0;
                while (Constants.receivingInprocess && count <= breakCount)
                {
                    Thread.Sleep(1000);
                    count += 1;
                }

                if (count > breakCount)
                {
                    Constants.CurrentRequest = string.Empty;
                    Constants.IsConnectionBreak = true;
                    Logger.Messages("Connection broken " + count);
                    // _serialPort.DiscardInBuffer();
                    // _serialPort.DiscardOutBuffer();
                    // Close();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }

        /// <summary>
        /// Transmit command to serial port, about to set ECO Off
        /// </summary>
        public void EcoOFFCommand()
        {
            try
            {
                success = false;
                Constants.IsConnectionBreak = false;
                buffer = string.Empty;
                Constants.CurrentRequest = Constants.Commands.ATE0.ToString();
                command = GetCommand(string.Empty);
                recievedData = ExecuteCommand(command);
                Constants.receivingInprocess = true;
                WaitingForResponse(10);


            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }

        /// <summary>
        /// Read received messages
        /// </summary>
        /// <returns></returns>
        public void ReadMessage()
        {
            success = false;
            Constants.IsConnectionBreak = false;
            string recievedData = string.Empty;
            String command = string.Empty;
            try
            {
                buffer = string.Empty;
                command = GetCommand(string.Empty);

                recievedData = ExecuteCommand(command);
                Constants.receivingInprocess = true;

                WaitingForResponse(10);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }

        /// <summary>
        /// Delete received messages
        /// </summary>
        /// <returns></returns>
        public void DeleteMessage()
        {
            success = false;
            Constants.IsConnectionBreak = false;
            string recievedData = string.Empty;
            String command = string.Empty;
            try
            {
                buffer = string.Empty;
                command = GetCommand(string.Empty);

                recievedData = ExecuteCommand(command);
                Constants.receivingInprocess = true;
                WaitingForResponse(10);

            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }

        /// <summary>
        /// Process recived messages
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public List<SplitMessage> ReadResponseMessages(string data)
        {
            List<SplitMessage> listMessages = new List<SplitMessage>();

            try
            {
                // \d+ is used for decimal value
                // .* is used for any character
                //Regex r = new Regex(@"\+CMGL: (\d+),""(.+)"",""(.+)"",(.*),""(.+)""\r\n(.*)");
                Regex r = new Regex(@"\+CMGL: (\d+),""(.+)"",""(.+)"",(.*),""(.+)""(.*)");
                Match m = r.Match(data);

                if (!m.Success)
                {
                    m = r.Match(data);
                }
                while (m.Success)
                {
                    SplitMessage msg = new SplitMessage();
                    msg.Index = m.Groups[1].Value;
                    msg.Status = m.Groups[2].Value;
                    msg.Sender = m.Groups[3].Value;
                    msg.Alphabet = m.Groups[4].Value;
                    msg.Sent = m.Groups[5].Value;
                    msg.Message = m.Groups[6].Value.Replace('\r', ' ');
                    if (msg.Message.EndsWith("OK"))
                    {
                        msg.Message = msg.Message.Remove(msg.Message.Length - 2, 2);
                    }
                    listMessages.Add(msg);

                    m = m.NextMatch();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
            return listMessages;
        }

        /// <summary>
        /// This function will retrieve the “AT” commands, based on user requirment.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public string GetCommand(string parameter)
        {
            string reqCommand = string.Empty;
            try
            {
                #region SMS COMMANDS
                if (Constants.CurrentRequest == Constants.Commands.AT.ToString())
                {
                    reqCommand = "AT\r\n";
                }
                else if (Constants.CurrentRequest == Constants.Commands.ATE0.ToString())
                {
                    reqCommand = "ATE0\r\n";
                }
                else if (Constants.CurrentRequest == Constants.Commands.UnRead.ToString())
                {
                    reqCommand = "AT+CMGL=\"REC UNREAD\"\r\n";
                }
                else if (Constants.CurrentRequest == Constants.Commands.CMGS.ToString())
                {
                    reqCommand = "AT+CMGS=\"" + parameter + "\"";
                }
                else if (Constants.CurrentRequest == Constants.Commands.Delete.ToString())
                {
                    reqCommand = "AT+CMGDA=\"DEL ALL\"\r\n";
                }
                #endregion

                #region GPRS COMMANDS
                else if (Constants.CurrentRequest == Constants.Commands.CPIN.ToString())
                {
                    reqCommand = "AT+CPIN?\r\n";
                }
                else if (Constants.CurrentRequest == Constants.Commands.CREG.ToString())
                {
                    reqCommand = "AT+CREG=1\r\n";
                }
                else if (Constants.CurrentRequest == Constants.Commands.CGATT.ToString())
                {
                    reqCommand = "AT+CGATT=1\r\n";
                }
                else if (Constants.CurrentRequest == Constants.Commands.Open_CIPSERVER.ToString())
                {
                    reqCommand = "AT+CIPSERVER=1," + parameter + "\r\n";
                }
                else if (Constants.CurrentRequest == Constants.Commands.CIFSR.ToString())
                {
                    reqCommand = "AT+CIFSR\r\n";
                }
                else if (Constants.CurrentRequest == Constants.Commands.CIPSTATUS.ToString())
                {
                    reqCommand = "AT+CIPSTATUS\r\n";
                }
                else if (Constants.CurrentRequest == Constants.Commands.CIPSEND.ToString())
                {
                    reqCommand = "AT+CIPSEND\r\n";
                }
                else if (Constants.CurrentRequest == Constants.Commands.Close_CIPSERVER.ToString())
                {
                    reqCommand = "AT+CIPSERVER=0\r\n";
                }
                else if (Constants.CurrentRequest == Constants.Commands.CIPSHUT.ToString())
                {
                    reqCommand = "AT+CIPSHUT\r\n";
                }
                else if (Constants.CurrentRequest == Constants.Commands.CSQ.ToString())
                {
                    reqCommand = "AT+CSQ\r\n";
                }
                #endregion
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
            return reqCommand;
        }
    }
}
