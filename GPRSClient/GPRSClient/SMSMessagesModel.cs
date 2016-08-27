using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GPRSClient
{
  public  class SMSMessagesModel
  {
      #region MEMBER VARIABLES
      string _MsgType = string.Empty;
        string _SMSType = string.Empty;
        string _SMSDate = string.Empty;
        string _SMSTime = string.Empty;
        string _SimNo = string.Empty;
        string _Message = string.Empty;
      #endregion

        #region PROPERTIES
        public string Message
        {
            get { return _Message; }
            set { _Message = value; }
        } 

        public string SimNo
        {
            get { return _SimNo; }
            set { _SimNo = value; }
        }

        public string SMSTime
        {
            get { return _SMSTime; }
            set { _SMSTime = value; }
        }

        public string SMSDate
        {
            get { return _SMSDate; }
            set { _SMSDate = value; }
        }       

        public string MsgType
        {
            get { return _MsgType; }
            set { _MsgType = value; }
        }

        #endregion
  }


  public class SplitMessage
  {

      #region Private Variables
      private string index;
      private string status;
      private string sender;
      private string alphabet;
      private string sent;
      private string message;
      #endregion

      #region Public Properties
      public string Index
      {
          get { return index; }
          set { index = value; }
      }
      public string Status
      {
          get { return status; }
          set { status = value; }
      }
      public string Sender
      {
          get { return sender; }
          set { sender = value; }
      }
      public string Alphabet
      {
          get { return alphabet; }
          set { alphabet = value; }
      }
      public string Sent
      {
          get { return sent; }
          set { sent = value; }
      }
      public string Message
      {
          get { return message; }
          set { message = value; }
      }
      #endregion

  }
}
