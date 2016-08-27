using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GPRSClient
{
    public static class Constants
    {
        public static string UserName;
        //  public static string UniqueUserName = "ucs";
        //this variable stores application unique password
        public static string UniquePassword = "8485";

        public static Boolean IsConnectingToModem = false;

        public static Boolean IsPortExist = true;
        //application login PersonName
        public static string LoginPersonName = string.Empty;
        public static string LoginContactNo = string.Empty;
        //to check wether login person is Admin or not
        public static Boolean IsAdmin = false;
        public static string CommunicationMode = string.Empty;
        public static string ConnectionStatus = string.Empty;
        public static Boolean IsModemConnected = false;

        //to check wether GPRS connectivity process started or not
        public static Boolean IsGPRSConnectivityStarted = false;
        public static string GPRSConnectionStatus = string.Empty;
        public static List<string> SMSMessages = new List<string>();
        public static string CurrentPort = string.Empty;
        // public static string IsMessageSent = string.Empty;
        public static Boolean IsNewMsg = false;
        public static Boolean IsGPRSDeactivated = false;
        public static List<SplitMessage> listMessages = new List<SplitMessage>();
        public static string recievedData = string.Empty;

        public static Boolean IsResponseSuccess = false;

        //to store current command request to GSM modem
        public static string CurrentRequest = string.Empty;

        //Current command request from application
        public static string CurrentCommandRequest = string.Empty;

        //to store previous command request to GSM modem
        public static string PreviousRequest = string.Empty;

        //to store previous parameter to GSM modem
        public static string PreviousParameter = string.Empty;

        //this variable indicates where serial port data receiving process completed or not
        public static Boolean IsDataReceiveCompleted = false;

        //this variable indicates GPRS signal strength
        public static string GPRSSignalStrength = string.Empty;

        //this variable will store IP address
        public static string GPRSServerIPAddress = string.Empty;

        //this variable will store IP address
        public static string GPRSIPAddress = string.Empty;

        //this variable will store Port No
        public static string GPRSPortNo = string.Empty;

        //to check wether modem connection is exist or not
        public static Boolean IsConnectionBreak = false;

        //to check modem break connection established
        public static Boolean IsBrokenConnectionEstablished = false;

        //Modem connectivity status
        public static string GSMConnectionStatus = string.Empty;

        //to know about continuation of receiving process.
        public static Boolean receivingInprocess = false;

        //If message sending process cancelled then this flag will become true
        public static Boolean IsProcessCancelled = false;

        //Check wether modem registred or not
        //public static Boolean IsSIMRegistered = false;

        //Is modem attached to the service
        //  public static Boolean IsModemAttachedToService = true;

        //Current GPRS connected server staus
        public static string ServerStatus = string.Empty;

        //Check wether client connected to server or not
        public static Boolean IsConnectedToClient = false;

       // public static string GPRSReceivedData = string.Empty;

        //new messages list
        public static List<string> ListNewMessages = new List<string>();

        public static string SMSMessage = string.Empty;

        public static string currentTrainNo = string.Empty;

        public static string MessageSendStatus = string.Empty;

        public static string GPRSReceivedMsg = string.Empty;

        public static string SIMAPN = "airtelGPRS.com";
        public static string DownloadPath = Application.StartupPath + @"\Data\FileData\";
        public static byte[] fileBytes = null;
        public static string FileName = string.Empty;
        public static Int64 FileSize = 0;
        public static Boolean IsFileDownloaded = false;

        public enum Commands
        {
            AT = 1,
            ATE0 = 2,
            UnRead = 3,
            Delete = 4,
            SelectRoute = 5,
            DeselectRoute = 6,
            RouteStatus = 7,
            MessageSend = 8,
            CMGS = 9,//to send SMS message
            CPIN = 10, //Ready command
            CSQ = 11, //Signle strength
            CGATT = 12, //Check device attached to the GPRS service/network or not           
            Open_CIPSERVER = 14,//start TCP server
            CIFSR = 15, //Get local IP address
            CIPSTATUS = 16,//Get CIP Status
            CIPSEND = 17,//Send GPRS message
            CIPSHUT = 18,//Close the broken connection
            Close_CIPSERVER = 19,//Close server
            CIPCLOSE = 20, //Close server
            CREG = 21,//SIM Registered or not
            GPRSDataReceiving = 22,//In Listening mode, receive data to client 
            uploadFile = 23,
            downloadFile = 24,
            
            CIPSTART = 26,
            CSTT = 27,
            CIICR = 28,
            //ClientGPRSReady = 29,
             ATO=30,//switch from Command mode to GPRS mode
            CIPMODE=31, //Transfer to data mode to command mode
            CIPCCFG=32,//To configure transfer mode  
            NonTransparentMode=33//Switch from data/GPRS mode to command/SMS mode
            // ServerListening=23//server in listening mode

        }

        public enum Connectivity
        {
            Connected = 1,
            Disconnected = 2,
            Not_Connected = 3
        }

        public enum SignalStrengthRanges
        {
            Marginal = 1, //2-9
            OK = 2,//10-14
            Good = 3,//15-19
            Excellent = 4//20-30
        }

        public enum GPRSConnection
        {
            Connected = 1,
            Disconnected = 2,
            Fail = 3,
            ClientGPRSReady = 4,
            PDP_DEACT = 5//GPRS disconnected from client
              
        }

        public enum MessageSendProcess
        {
            Msg_Sent = 1,
            Fail = 2
        }
    }
}
