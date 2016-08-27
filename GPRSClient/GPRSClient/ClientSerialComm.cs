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
    public class ClientSerialComm
    {
        #region MEMBER VARIABLES
        public SerialPort _serialPort = null;
        //  Boolean success = false;
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
        byte[] readBuffer = null;
        //   Boolean isNewFile = false;
        int currentPKTLen = 0;
        int downloadedcurrentPktLen = 0;
        int downloadedLength = 0;

        int remainLength = 0;
        int lastPktLength = 0;
        Boolean isNewPkt = true;
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

                if (_serialPort != null)
                {
                    _serialPort = null;
                    Close();
                }

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
                Constants.CurrentRequest = Constants.Commands.CIPCLOSE.ToString();
                CallingGPRSCommand();
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }

        public void SwitchToCommand()
        {
            try
            {
                Constants.CurrentRequest = Constants.Commands.NonTransparentMode.ToString();
                CallingGPRSCommand();
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }

        public void SwitchToData()
        {
            try
            {
                Constants.CurrentRequest = Constants.Commands.ATO.ToString();
                CallingGPRSCommand();
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
                CallingGPRSCommand();
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
                Constants.IsResponseSuccess = false;
                buffer = string.Empty;
                Constants.CurrentRequest = Constants.Commands.CREG.ToString();
                command = GetCommand(string.Empty);
                recievedData = ExecuteCommand(command);
                Constants.receivingInprocess = true;
                // WaitingForResponse(10000);

                //if (Constants.IsResponseSuccess)
                //{
                //    Constants.CurrentRequest = Constants.Commands.CSQ.ToString();
                //    buffer = string.Empty;
                //    command = GetCommand(string.Empty);
                //    recievedData = ExecuteCommand(command);
                //    Constants.receivingInprocess = true;
                //    WaitingForResponse(30000);
                //    Constants.IsResponseSuccess = false;
                //}

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
                Constants.IsResponseSuccess = false;
                buffer = string.Empty;
                Constants.CurrentRequest = Constants.Commands.CGATT.ToString();
                command = GetCommand(string.Empty);
                recievedData = ExecuteCommand(command);
                Constants.receivingInprocess = true;
                // WaitingForResponse(10);

                //Constants.IsResponseSuccess = false;


            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }


        private void CallingGPRSCommand()
        {
            try
            {
                Thread.Sleep(500);
                Constants.IsResponseSuccess = false;
                buffer = string.Empty;
                recievedData = ExecuteCommand(GetCommand(string.Empty));
                Constants.receivingInprocess = true;
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
        //private void CommandSendProcess(string nextCommand, string parameter)
        //{
        //    try
        //    {
        //        if (Constants.PreviousRequest != nextCommand)
        //        {
        //            Constants.PreviousRequest = Constants.CurrentRequest;
        //            command = GetCommand(parameter);
        //            recievedData = ExecuteCommand(command);
        //            Constants.receivingInprocess = true;
        //            if (Constants.CurrentRequest == Constants.Commands.Open_CIPSERVER.ToString())
        //            {
        //                WaitingForResponse(60000);
        //            }
        //            else
        //            {
        //                WaitingForResponse(30000);
        //            }

        //            if (Constants.IsResponseSuccess)
        //            {
        //                Constants.CurrentRequest = nextCommand;
        //            }

        //            CallingGPRSConnectivityProcess();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Logger.WriteLog(ex);
        //    }
        //}


        //private void CommandSendProcess(string nextCommand)
        //{
        //    try
        //    {
        //        if (Constants.PreviousRequest != nextCommand)
        //        {
        //            Constants.PreviousRequest = Constants.CurrentRequest;
        //            command = GetCommand(string.Empty);
        //            recievedData = ExecuteCommand(command);
        //            Constants.receivingInprocess = true;
        //            WaitingForResponse(30000);

        //            if (Constants.IsResponseSuccess)
        //            {
        //                Constants.CurrentRequest = nextCommand;
        //            }
        //            else
        //            {
        //                Constants.CurrentRequest = Constants.Commands.CIPSHUT.ToString();
        //            }
        //            CallingGPRSConnectivityProcess();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Logger.WriteLog(ex);
        //    }
        //}

        //private void CommandSendProcess(string nextCommand)
        //{
        //    try
        //    {
        //        if (Constants.PreviousRequest != nextCommand)
        //        {
        //            Constants.PreviousRequest = Constants.CurrentRequest;
        //            command = GetCommand(string.Empty);
        //            recievedData = ExecuteCommand(command);
        //            Constants.receivingInprocess = true;
        //            WaitingForResponse(30000);

        //            if (Constants.IsResponseSuccess)
        //            {
        //                Constants.CurrentRequest = nextCommand;
        //            }
        //            else
        //            {
        //                Constants.CurrentRequest = Constants.Commands.CIPSHUT.ToString();
        //            }
        //            CallingGPRSConnectivityProcess();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Logger.WriteLog(ex);
        //    }
        //}
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
                // command = command +"\r";
                _serialPort.Write(command);
                Logger.Messages(DateTime.Now.ToLongTimeString() + ":TX :" + command);
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
                        readBuffer = new byte[_serialPort.BytesToRead];
                        _serialPort.Read(readBuffer, 0, readBuffer.Length);

                        // rData = _serialPort.ReadExisting();

                        rData = Encoding.ASCII.GetString(readBuffer);
                        Logger.Messages(DateTime.Now.ToLongTimeString() + ":RX :" + rData);
                        if (rData.Contains('\r'))//|| rData.Contains('\n'))
                        {
                            //var charsToRemove = new string[] { "\r", "\n" };
                            var charsToRemove = new string[] { "\r" };
                            foreach (var c in charsToRemove)
                            {
                                rData = rData.Replace(c, string.Empty);
                            }
                        }

                        buffer += rData;

                        string dd = buffer;
                        //Logger.Messages("RX :" + rData);

                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLog(ex);
                    }
                    ProcessReceivedCommand(ref buffer, readBuffer);
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
        private void ProcessReceivedCommand(ref string processbuffer, byte[] readBufferData)
        {
            try
            {
                #region SMS Commands

                #region AT
                if (Constants.CurrentRequest == Constants.Commands.AT.ToString() &&
                    processbuffer.EndsWith("OK\n"))
                {
                    Constants.receivingInprocess = false;
                    Constants.IsResponseSuccess = true;
                }
                else if (Constants.CurrentRequest == Constants.Commands.AT.ToString() &&
                   processbuffer.EndsWith("ERROR\n"))
                {
                    Constants.receivingInprocess = false;
                    Constants.IsResponseSuccess = false;
                }
                if (Constants.CurrentRequest == Constants.Commands.AT.ToString() &&
                processbuffer.EndsWith(">"))
                {
                    Constants.receivingInprocess = false;
                    Constants.IsResponseSuccess = false;
                    ExecuteCommand(char.ConvertFromUtf32(26));
                }
                #endregion

                #region ATE0
                else if (Constants.CurrentRequest == Constants.Commands.ATE0.ToString() &&
                    processbuffer.EndsWith("OK\n"))
                {
                    Constants.receivingInprocess = false;
                    Constants.IsResponseSuccess = true;
                }
                #endregion

                #region CPIN
                else if (Constants.CurrentRequest == Constants.Commands.CPIN.ToString() &&
                    processbuffer.Contains("+CPIN: READY") && processbuffer.EndsWith("OK\n"))
                {
                    Constants.receivingInprocess = false;
                    Constants.IsResponseSuccess = true;
                }
                #endregion

                #region CSQ
                else if (Constants.CurrentRequest == Constants.Commands.CSQ.ToString() &&
                  processbuffer.Contains("+CSQ:") && processbuffer.EndsWith("OK\n"))
                {
                    Constants.receivingInprocess = false;
                    Constants.IsResponseSuccess = true;
                    Regex r = new Regex(@"\+CSQ: (\d+),");
                    Match m = r.Match(processbuffer);

                    if (!m.Success)
                    {
                        m = r.Match(processbuffer);
                    }
                    if (m.Success)
                    {
                        Constants.GPRSSignalStrength = m.Groups[1].Value.ToString();
                    }
                }
                #endregion

                #region CREG
                else if (Constants.CurrentRequest == Constants.Commands.CREG.ToString() && processbuffer.EndsWith("OK\n"))
                {
                    Constants.receivingInprocess = false;
                    Constants.IsResponseSuccess = true;
                }
                else if (Constants.CurrentRequest == Constants.Commands.CREG.ToString() && processbuffer.EndsWith("ERROR\n"))
                {
                    Constants.receivingInprocess = false;
                    Constants.IsResponseSuccess = false;
                }
                #endregion

                #region CMGS
                //valid response of +CMGS command
                else if (Constants.CurrentRequest == Constants.Commands.CMGS.ToString() &&
                     processbuffer.EndsWith("> "))
                {

                    Constants.CurrentRequest = Constants.CurrentCommandRequest;
                    buffer = string.Empty;
                    command = Constants.SMSMessage + char.ConvertFromUtf32(26);// +"\r";
                    recievedData = ExecuteCommand(command);
                }
                //invalid response of +CMGS command
                else if (Constants.CurrentRequest == Constants.Commands.CMGS.ToString() &&
                     processbuffer.EndsWith("ERROR\n"))
                {
                    Constants.receivingInprocess = false;
                    Constants.IsResponseSuccess = false;
                    Constants.MessageSendStatus = Constants.MessageSendProcess.Fail.ToString();
                }
                #endregion

                #region Select, Deselect, Status Route SMS
                //valid response after message sent
                else if ((Constants.CurrentRequest == Constants.Commands.SelectRoute.ToString()
                    || Constants.CurrentRequest == Constants.Commands.DeselectRoute.ToString()
                    || Constants.CurrentRequest == Constants.Commands.RouteStatus.ToString())
                    && processbuffer.EndsWith("OK\n"))
                {
                    Constants.receivingInprocess = false;
                    Constants.IsResponseSuccess = true;
                    Constants.MessageSendStatus = Constants.MessageSendProcess.Msg_Sent.ToString().Replace('_', ' ');
                }
                //Invalid response after message sent
                else if ((Constants.CurrentRequest == Constants.Commands.SelectRoute.ToString()
                    || Constants.CurrentRequest == Constants.Commands.DeselectRoute.ToString()
                    || Constants.CurrentRequest == Constants.Commands.RouteStatus.ToString())
                    && processbuffer.EndsWith("ERROR\n"))
                {
                    Constants.receivingInprocess = false;
                    Constants.IsResponseSuccess = false;
                    Constants.MessageSendStatus = Constants.MessageSendProcess.Fail.ToString();
                }
                #endregion

                #region Read Unread msgs
                //Valid Response of Read all Unread messages
                else if (Constants.CurrentRequest == Constants.Commands.UnRead.ToString()
                    && processbuffer.EndsWith("OK\n"))
                {
                    Constants.receivingInprocess = false;
                    Constants.IsResponseSuccess = true;
                    Constants.listMessages = ReadResponseMessages(processbuffer);

                }
                //Invalid Response of Read all Unread messages
                else if (Constants.CurrentRequest == Constants.Commands.UnRead.ToString()
                    && processbuffer.EndsWith("ERROR\n"))
                {
                    Constants.receivingInprocess = false;
                    Constants.listMessages = ReadResponseMessages(processbuffer);
                    Constants.IsResponseSuccess = true;
                }
                #endregion

                #region Delete all SMS
                //Valid Response of Delete message
                else if (Constants.CurrentRequest == Constants.Commands.Delete.ToString()
                    && processbuffer.EndsWith("OK\n"))
                {
                    Constants.receivingInprocess = false;
                    Constants.IsResponseSuccess = true;
                }
                //Valid Response of Delete message
                else if (Constants.CurrentRequest == Constants.Commands.Delete.ToString()
                    && processbuffer.EndsWith("ERROR\n"))
                {
                    Constants.receivingInprocess = false;
                    Constants.IsResponseSuccess = true;
                }
                #endregion
                #endregion

                #region GPRS COMMANDS

                #region Client Disconnected
                else if (processbuffer.Contains("+PDP: DEACT"))//(processbuffer.EndsWith("+CPIN: READY"))
                {
                    Constants.GPRSConnectionStatus = Constants.GPRSConnection.Fail.ToString();
                    Constants.ServerStatus = Constants.GPRSConnection.Fail.ToString();
                }
                #endregion

                #region CIPSHUT
                else if (Constants.CurrentRequest == Constants.Commands.CIPSHUT.ToString() &&
                     processbuffer.EndsWith("SHUT OK\n"))
                {
                    buffer = string.Empty;
                    Constants.CurrentRequest = Constants.Commands.CGATT.ToString();
                    recievedData = ExecuteCommand(GetCommand(string.Empty));
                }
                else if (Constants.CurrentRequest == Constants.Commands.CIPSHUT.ToString() &&
                     processbuffer.EndsWith("ERROR\n"))
                {
                    Constants.receivingInprocess = false;
                    Constants.IsResponseSuccess = false;
                }
                #endregion

                #region CGATT
                else if (Constants.CurrentRequest == Constants.Commands.CGATT.ToString() && processbuffer.EndsWith("OK\n"))
                {
                    buffer = string.Empty;
                    if (Constants.IsGPRSConnectivityStarted)
                    {
                        Constants.CurrentRequest = Constants.Commands.CIPMODE.ToString();
                        recievedData = ExecuteCommand(GetCommand(Constants.GPRSPortNo));
                    }
                    else
                    {
                        Constants.receivingInprocess = false;
                        Constants.IsResponseSuccess = true;
                    }
                }
                else if (Constants.CurrentRequest == Constants.Commands.CGATT.ToString() && processbuffer.EndsWith("ERROR\n"))
                {
                    Constants.GPRSConnectionStatus = Constants.GPRSConnection.Fail.ToString();
                    Constants.receivingInprocess = false;
                    Constants.IsResponseSuccess = false;
                }
                #endregion

                #region CIPMODE
                else if (Constants.CurrentRequest == Constants.Commands.CIPMODE.ToString() && processbuffer.EndsWith("OK\n"))
                {
                    buffer = string.Empty;
                    if (Constants.IsGPRSConnectivityStarted)
                    {
                        Constants.CurrentRequest = Constants.Commands.CSTT.ToString();
                        recievedData = ExecuteCommand(GetCommand(Constants.GPRSPortNo));
                    }
                    else
                    {
                        Constants.receivingInprocess = false;
                        Constants.IsResponseSuccess = true;
                    }
                }
                else if (Constants.CurrentRequest == Constants.Commands.CIPMODE.ToString() && processbuffer.EndsWith("ERROR\n"))
                {
                    Constants.receivingInprocess = false;
                    Constants.IsResponseSuccess = false;
                }
                #endregion

                #region CSTT
                else if (Constants.CurrentRequest == Constants.Commands.CSTT.ToString() && processbuffer.EndsWith("OK\n"))
                {
                    buffer = string.Empty;
                    Constants.CurrentRequest = Constants.Commands.CIICR.ToString();
                    recievedData = ExecuteCommand(GetCommand(string.Empty));
                }
                else if (Constants.CurrentRequest == Constants.Commands.CSTT.ToString() && processbuffer.EndsWith("Error\n"))
                {
                    Constants.GPRSConnectionStatus = Constants.GPRSConnection.Fail.ToString();
                    Constants.receivingInprocess = false;
                    Constants.IsResponseSuccess = false;
                }
                #endregion

                #region CIICR
                else if (Constants.CurrentRequest == Constants.Commands.CIICR.ToString() && processbuffer.EndsWith("OK\n"))
                {
                    buffer = string.Empty;
                    Constants.CurrentRequest = Constants.Commands.CIFSR.ToString();
                    recievedData = ExecuteCommand(GetCommand(string.Empty));
                    Thread.Sleep(1000);
                }
                else if (Constants.CurrentRequest == Constants.Commands.CIICR.ToString() && processbuffer.EndsWith("Error\n"))
                {
                    Constants.GPRSConnectionStatus = Constants.GPRSConnection.Fail.ToString();
                    Constants.receivingInprocess = false;
                    Constants.IsResponseSuccess = false;
                }
                #endregion

                #region CIFSR
                else if (Constants.CurrentRequest == Constants.Commands.CIFSR.ToString() && !processbuffer.EndsWith("OK\n"))
                {
                    Constants.GPRSIPAddress = processbuffer;
                    Constants.CurrentRequest = Constants.Commands.CIPSTART.ToString();
                    recievedData = ExecuteCommand(GetCommand(string.Empty));
                    buffer = string.Empty;
                }
                else if (Constants.CurrentRequest == Constants.Commands.CIFSR.ToString() && processbuffer.EndsWith("ERROR"))
                {
                    Constants.GPRSConnectionStatus = Constants.GPRSConnection.Fail.ToString();
                    Constants.receivingInprocess = false;
                    Constants.IsResponseSuccess = false;
                }
                #endregion

                #region CIPSTART
                else if (Constants.CurrentRequest == Constants.Commands.CIPSTART.ToString() && processbuffer.EndsWith("CONNECT\n"))
                {
                    Constants.GPRSConnectionStatus = Constants.GPRSConnection.ClientGPRSReady.ToString();
                    Constants.receivingInprocess = false;
                    Constants.IsResponseSuccess = true;
                    buffer = string.Empty;

                }
                else if (Constants.CurrentRequest == Constants.Commands.CIPSTART.ToString() && processbuffer.EndsWith("FAIL\n"))
                {
                    Constants.GPRSConnectionStatus = Constants.GPRSConnection.Fail.ToString();
                    Constants.receivingInprocess = false;
                    Constants.IsResponseSuccess = false;
                    Constants.CurrentRequest = string.Empty;
                    buffer = string.Empty;
                }
                #endregion

                #region CLIENT READY
                else if (Constants.GPRSConnectionStatus == Constants.GPRSConnection.ClientGPRSReady.ToString())
                {
                    Constants.GPRSReceivedMsg = processbuffer;
                    if (processbuffer.EndsWith("CLOSED\n"))
                    {
                        Constants.GPRSConnectionStatus = Constants.GPRSConnection.Fail.ToString();
                        Constants.ServerStatus = Constants.GPRSConnection.Fail.ToString();
                    }

                    #region  Download FILE

                    if (processbuffer.StartsWith("FSend") && processbuffer.EndsWith("\n"))
                    {
                        downloadedLength = 0;
                        Constants.CurrentRequest = Constants.Commands.downloadFile.ToString();

                        String[] split = processbuffer.Split(' ');
                        if (split.Length == 3)
                        {
                            Constants.FileName = split[1].ToString();
                            Constants.FileSize = Convert.ToInt64(split[2].ToString());

                            using (var stream = new FileStream(Constants.DownloadPath + Constants.FileName, FileMode.Create, FileAccess.Write, FileShare.None))
                            {
                                stream.Close();
                            }
                        }
                        ExecuteCommand("FSOK\n");                       
                        processbuffer = string.Empty;
                    }

                    
                    else if (Constants.CurrentRequest == Constants.Commands.downloadFile.ToString())
                    {

                        if (processbuffer.EndsWith("FEOK\n"))
                        {
                            Constants.CurrentRequest = string.Empty;
                            processbuffer = string.Empty;
                            Constants.IsFileDownloaded = true;
                        }
                        else
                        {
                            if (!Directory.Exists(Constants.DownloadPath))
                            {
                                Directory.CreateDirectory(Constants.DownloadPath);
                            }
                            else
                            {
                                string[] listFiles = Directory.GetFiles(DownloadPath);
                                if (listFiles.Length > 0)
                                {
                                    for (int i = 0; i < listFiles.Length; i++)
                                    {
                                        if (File.Exists(listFiles[i]))
                                        {
                                            try
                                            {
                                                File.Delete(listFiles[i]);
                                            }
                                            catch (Exception)
                                            {

                                            }
                                        }
                                    }
                                }                               
                            }

                            using (var stream = new FileStream(Constants.DownloadPath + Constants.FileName, FileMode.Append,
                                FileAccess.Write, FileShare.None))

                            using (var bw = new BinaryWriter(stream))
                            {
                                processbuffer = string.Empty;
                                downloadedLength = downloadedLength + readBufferData.Length;

                                bw.Write(readBufferData);
                                stream.Close();
                                if (downloadedLength == Constants.FileSize)
                                {
                                    ExecuteCommand("OK\n");
                                }
                            }
                        }
                    }
                    #endregion

                    #region Switch from TransparentMode to NonTransparentMode
                    else if (Constants.CurrentRequest == Constants.Commands.NonTransparentMode.ToString() && processbuffer.EndsWith("OK\n"))
                    {
                        buffer = string.Empty;
                        Constants.receivingInprocess = false;
                        Constants.IsResponseSuccess = true;
                    }
                    else if (Constants.CurrentRequest == Constants.Commands.NonTransparentMode.ToString() && processbuffer.EndsWith("Error\n"))
                    {
                        Constants.receivingInprocess = false;
                        Constants.IsResponseSuccess = false;
                    }
                    #endregion

                    #region Switch from TransparentMode to NonTransparentMode
                    else if (Constants.CurrentRequest == Constants.Commands.ATO.ToString() && processbuffer.EndsWith("OK\n"))
                    {
                        buffer = string.Empty;
                        Constants.receivingInprocess = false;
                        Constants.IsResponseSuccess = true;
                    }
                    else if (Constants.CurrentRequest == Constants.Commands.ATO.ToString() && processbuffer.EndsWith("Error\n"))
                    {
                        Constants.receivingInprocess = false;
                        Constants.IsResponseSuccess = false;
                    }
                    #endregion
                }
                #endregion

               

                #endregion

                //if(processbuffer.EndsWith("TCP CLOSED\n"))
                //{
                //    Constants.GPRSConnectionStatus = Constants.GPRSConnection.Fail.ToString();
                //    Constants.ServerStatus = Constants.GPRSConnection.Fail.ToString();
                //}

                IsNewMessageExist(processbuffer);

                IsBrokenConnectionEstablished(processbuffer);
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
        public void SendMessage(string phoneNo)
        {
            string request = Constants.CurrentRequest;
            try
            {
                Constants.IsResponseSuccess = false;
                buffer = string.Empty;
                Constants.CurrentRequest = Constants.Commands.CMGS.ToString();
                command = GetCommand(phoneNo);
                recievedData = ExecuteCommand(command);
                Constants.receivingInprocess = true;
                //WaitingForResponse(10000);

                //if (Constants.IsResponseSuccess)
                //{
                //    Constants.CurrentRequest = request;
                //    buffer = string.Empty;
                //    command = Message + char.ConvertFromUtf32(26);// +"\r";
                //    recievedData = ExecuteCommand(command);
                //    Constants.receivingInprocess = true;
                //    WaitingForResponse(30);
                //    Constants.IsResponseSuccess = false;
                //    buffer = string.Empty;
                //}
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }

        public void SendGPRSMessage(string Message)
        {
            string request = Constants.CurrentRequest;
            try
            {
                Constants.IsResponseSuccess = false;
                buffer = string.Empty;
                Constants.CurrentRequest = Constants.Commands.CIPSEND.ToString();
                command = GetCommand(string.Empty);
                recievedData = ExecuteCommand(command);
                Constants.receivingInprocess = true;
                //WaitingForResponse(10);

                //if (Constants.IsResponseSuccess)
                //{
                //    Constants.CurrentRequest = request;
                //    buffer = string.Empty;
                //    command = Message + char.ConvertFromUtf32(26);// +"\r";
                //    recievedData = ExecuteCommand(command);
                //    Constants.receivingInprocess = true;
                //    WaitingForResponse(60);
                //    Constants.IsResponseSuccess = false;
                //}
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
                Constants.IsResponseSuccess = false;
                Constants.IsConnectionBreak = false;
                buffer = string.Empty;
                Constants.CurrentRequest = Constants.Commands.AT.ToString();
                command = GetCommand(string.Empty);
                recievedData = ExecuteCommand(command);
                Constants.receivingInprocess = true;
                // WaitingForResponse(5000);
                #endregion
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
            return Constants.IsResponseSuccess;
        }

        /// <summary>
        /// Wait mode to get the response from Serial port
        /// </summary>
        /// <param name="breakCount"></param>
        //private void WaitingForResponse(int breakCount)
        //{
        //    try
        //    {
        //        count = 100;
        //        while (Constants.receivingInprocess && count <= breakCount)
        //        {
        //            Thread.Sleep(100);
        //            count += 100;
        //        }

        //        if (count > breakCount)
        //        {
        //            Constants.CurrentRequest = string.Empty;
        //            Constants.IsConnectionBreak = true;
        //            Logger.Logger.Messages("Connection broken " + count);
        //            // _serialPort.DiscardInBuffer();
        //            // _serialPort.DiscardOutBuffer();
        //            // Close();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Logger.WriteLog(ex);
        //    }
        //}

        /// <summary>
        /// Transmit command to serial port, about to set ECO Off
        /// </summary>
        public void EcoOFFCommand()
        {
            try
            {
                Constants.IsResponseSuccess = false;
                Constants.IsConnectionBreak = false;
                buffer = string.Empty;
                Constants.CurrentRequest = Constants.Commands.ATE0.ToString();
                command = GetCommand(string.Empty);
                recievedData = ExecuteCommand(command);
                Constants.receivingInprocess = true;
                //WaitingForResponse(10);


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
            Constants.IsResponseSuccess = false;
            Constants.IsConnectionBreak = false;
            string recievedData = string.Empty;
            String command = string.Empty;
            try
            {
                buffer = string.Empty;
                command = GetCommand(string.Empty);

                recievedData = ExecuteCommand(command);
                Constants.receivingInprocess = true;

                //WaitingForResponse(10);
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
            Constants.IsResponseSuccess = false;
            Constants.IsConnectionBreak = false;
            string recievedData = string.Empty;
            String command = string.Empty;
            try
            {
                buffer = string.Empty;
                command = GetCommand(string.Empty);

                recievedData = ExecuteCommand(command);
                Constants.receivingInprocess = true;
                //WaitingForResponse(10);

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
                Regex r = new Regex(@"\+CMGL: (\d+),""(.+)"",""(.+)"",(.*),""(.+)""\n(.*)");
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
                    msg.Message = m.Groups[6].Value;
                    if (msg.Message.EndsWith("OK\n"))
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
                else if (Constants.CurrentRequest == Constants.Commands.CIPCLOSE.ToString())
                {
                    reqCommand = "AT+CIPCLOSE=1\r\n";
                }
                else if (Constants.CurrentRequest == Constants.Commands.CIPSHUT.ToString())
                {
                    reqCommand = "AT+CIPSHUT\r\n";
                }
                else if (Constants.CurrentRequest == Constants.Commands.CSQ.ToString())
                {
                    reqCommand = "AT+CSQ\r\n";
                }
                else if (Constants.CurrentRequest == Constants.Commands.CIPMODE.ToString())
                {
                    reqCommand = "AT+CIPMODE=1\r\n";
                }
                else if (Constants.CurrentRequest == Constants.Commands.CSTT.ToString())
                {
                    reqCommand = "AT+CSTT=\"" + Constants.SIMAPN.ToString() + "\"\r\n";
                }
                else if (Constants.CurrentRequest == Constants.Commands.CIICR.ToString())
                {
                    reqCommand = "AT+CIICR\r";
                }
                else if (Constants.CurrentRequest == Constants.Commands.CIPSTART.ToString())
                {
                    reqCommand = "AT+CIPSTART=\"TCP\",\"" + Constants.GPRSServerIPAddress + "\",\"" + Constants.GPRSPortNo + "\"\r";
                }
                else if (Constants.CurrentRequest == Constants.Commands.NonTransparentMode.ToString())
                {
                    reqCommand = "+++";
                }
                else if (Constants.CurrentRequest == Constants.Commands.ATO.ToString())
                {
                    reqCommand = "ATO\r";
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
