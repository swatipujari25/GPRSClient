using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace GPRSClient
{
    public static class BaseClass
    {
        public static Boolean GPRSMode = false;
        public static Boolean GSMMode = false;
        //public static Boolean SetRouteCmd = false;
        public static Boolean SelectRoute = false;
        public static Boolean SelectRouteStatus = false;
        public static Boolean DeselectRoute = false;
        public static Boolean DeselectRouteStatus = false;
        public static Boolean RouteStatus = false;

        public static Boolean UpdateCmd = false;
        public static Boolean TrainDetails = false;
        public static Boolean CoachDetails = false;
        // public static Boolean ReadSMS = false;
        public static Boolean UserDetails = false;
        public static string ReadMSgStatus = string.Empty;
        public static Dictionary<int, string> DicCoaches = new Dictionary<int, string>();
        public static Boolean IsSendOperationInprocess = false;
        public static string CurrentProcess = string.Empty;
        public static int SelectedMenuCommand = 0;

        public static ClientSerialComm SRPortComm = new ClientSerialComm();
        public static string ContactusEmailId = "infoscs8485@gmail.com";
        // public static Boolean modemConnectionStatus = false;

        public static int SentMsgCount = 1;
        public static int timeCounter = 0;
        public static Stopwatch counterWatch = new Stopwatch();
        public static TimeSpan stopWatchTimeCheck = new TimeSpan();
        public static int milliTime = 0;
        public static int dgvRowIndex = 0;
        public static List<int> ListSelectedDgvRows = new List<int>();

        public enum CRUDOperations
        {
            Insert = 1,
            Update = 2,
            Delete = 3,
            Select = 4
        }

        public enum CommunicationMode
        {
            SMS = 1,
            GPRS = 2
        }

        public enum MessageType
        {
            Request = 1,
            Response = 2
        }

        public enum Roles
        {
            Admin = 1,
            User = 2
        }

        public enum FormNames
        {
            Select_Route = 1,
            Select_Route_Status = 2,
            Deselect_Route = 3,
            Deselect_Route_Status = 4,
            Route_Status = 5,
            Upload_File = 6,
            Download_File = 7
        }

        public enum ModemStatus
        {
            Modem_Connected = 1,
            Modem_Disconnected = 2,
            MODEM_FAIL = 3
        }

        public enum FontStyles
        {
            Arial = 1 //English
        }

        public enum SMSRequestFrom
        {
            Connect = 1,
            Send = 2
        }


        public static void CalculateCounterTime()
        {

            stopWatchTimeCheck = BaseClass.counterWatch.Elapsed;

            if (stopWatchTimeCheck.Seconds > 0)
            {
                milliTime = (stopWatchTimeCheck.Seconds * 1000) + stopWatchTimeCheck.Milliseconds;
            }
            else
            {
                milliTime = stopWatchTimeCheck.Milliseconds;
            }
        }
    }
}
