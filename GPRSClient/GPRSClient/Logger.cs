// <copyright file="ErrorLog.cs" company="Sirveen Control System">
// Copyright (c) 2014 All Rights Reserved
// <author>Swati P</author>
// <date>12/02/2016 13:15 </date>
// <summary>This class contains code related to application errors</summary>
// </copyright>


using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Diagnostics;

namespace GPRSClient
{
   public class Logger
    {
       /// <summary>
       /// To Save exception into text/ errorlog document,
       /// which will help the developer to fix the runtime/ production issues
       /// </summary>
       /// <param name="ex"></param>
        public static void WriteLog(Exception ex)
        {
            try
            {
                string rootPath = Directory.GetCurrentDirectory();

                string Path =  rootPath+@"\Errorlog\Errorlog.txt";
                //This text is added to create the file
                if (!File.Exists(Path))
                {
                    StreamWriter sw = File.CreateText(Path);
                }

                //after file creation we are adding below details, 
                //which are contains brief description about the exception
                using (StreamWriter sw = File.AppendText(Path))
                {
                    sw.WriteLine("Source        : " +
                   ex.Source.ToString().Trim());
                    sw.WriteLine("Method        : " +
                            ex.TargetSite.Name.ToString());
                    sw.WriteLine("Date          : " +
                            DateTime.Now.ToLongTimeString());
                    sw.WriteLine("Time          : " +
                            DateTime.Now.ToShortDateString());
                    sw.WriteLine("Computer      : " +
                            Dns.GetHostName().ToString());
                    sw.WriteLine("Error         : " +
                            ex.Message.ToString().Trim());
                    sw.WriteLine("Stack Trace   : " +
                            ex.StackTrace.ToString().Trim());
                    sw.WriteLine("======================================");
                    sw.Flush();
                    sw.Close();
                }
            }
            catch (Exception x)
            { 
            }
        }

       /// <summary>
       /// Message updates in log file, for tracking the details
       /// </summary>
       /// <param name="msg"></param>
        public static void Messages(string msg)
        {
            try
            {
                string rootPath = Directory.GetCurrentDirectory();
                string Path = rootPath + @"\Errorlog\Errorlog.txt";
                //This text is added to create the file
                if (!File.Exists(Path))
                {
                    StreamWriter sw = File.CreateText(Path);
                }

                //after file creation we are adding below details, 
                //which are contains brief description about the exception
                using (StreamWriter sw = File.AppendText(Path))
                {                  
                    //sw.WriteLine("Message         : " +
                    sw.WriteLine(
                            msg.ToString().Trim());
                   
                    //sw.WriteLine("======================================");
                    sw.Flush();
                    sw.Close();
                }
            }
            catch (Exception x)
            {
            }
        }
    }
}
