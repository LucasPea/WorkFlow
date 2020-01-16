using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace NextCowry.integrate
{
    public class Logger
    {
        private int LogLevel = 0;
        //private static Semaphore _sem;
        private string LogPath = null;

        public Logger(string Folder)
        {
            if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["LogLevel"]))
            {
                LogLevel = int.Parse(ConfigurationManager.AppSettings["LogLevel"]);

            }
            LogPath = Path.Combine("C:\\" + Folder);
            DirectoryInfo LogDir = new DirectoryInfo(LogPath);
            if (!LogDir.Exists)
            {
                LogDir.Create();
            }
        }

        public Logger()
        {
            if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["LogLevel"]))
            {
                LogLevel = int.Parse(ConfigurationManager.AppSettings["LogLevel"]);

            }

            //  _sem = new Semaphore(1, 1, "DSALogSemaphore");
            LogPath = Path.Combine("C:\\AppStoreLogs");
            DirectoryInfo LogDir = new DirectoryInfo(LogPath);
            if (!LogDir.Exists)
            {
                LogDir.Create();
            }
        }

        #region Log
        public void Log(string message, int level)
        {
            if (level <= LogLevel)
            {
                //    _sem.WaitOne();
                try
                {
                    DateTime today = DateTime.Now;
                    string rootPathLog = LogPath + "\\";

                    FileInfo mf = new FileInfo(rootPathLog + "-" + today.Year + "-" + today.Month + "-" + today.Day + ".log");
                    StreamWriter sw = mf.AppendText();
                    sw.WriteLine(DateTime.Now.ToString() + ": " + message);
                    sw.Close();
                }
                catch (Exception)
                {
                    // do nithing.
                }
                finally
                {
                    //     _sem.Release();
                }

            }

        }

        public void Log(string message, params object[] parameters)
        {
            this.Log(string.Format(message, parameters), 0);
        }
        #endregion
    }
}