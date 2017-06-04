using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;
namespace DeleteLog
{
    public partial class DeleteLogs : ServiceBase
    {
        System.Timers.Timer objTimer = new System.Timers.Timer();
        string[] path = ConfigurationSettings.AppSettings["LogFilePath"].ToString().Split(',');
        int Duration = Convert.ToInt32(ConfigurationSettings.AppSettings["LogDuration"]);
        int Interval = Convert.ToInt32(ConfigurationSettings.AppSettings["interval"]);

        public DeleteLogs()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {

            objTimer.Enabled = true;
            objTimer.Interval = Interval;
            objTimer.Elapsed += objTimer_Elapsed;
            objTimer.Enabled = true;
            objTimer.Start();
        }

        void objTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            lock (this)
            {
                Task objTask = new Task(() => LogDeleter());
                objTask.Start();
            }
        }

        private void LogDeleter()
        {
            try
            {
                foreach (string strPath in path)
                {
                    string[] directoryPath = null;
                    string[] subDirectoryPath = null;
                    if (Directory.Exists(strPath))
                    {
                        directoryPath = Directory.GetDirectories(strPath);
                        foreach (var strDirectoryPath in directoryPath)
                        {
                            subDirectoryPath = Directory.GetDirectories(strPath);
                            string[] filePath = Directory.GetFiles(strDirectoryPath, "*.txt");
                            foreach (string strFilePath in filePath)
                            {
                                FileInfo objFileInfo = new FileInfo(strFilePath);
                                if (DateTime.Now.Day - objFileInfo.LastWriteTime.Day == Duration)
                                {
                                    File.Delete(strFilePath);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        protected override void OnStop()
        {
        }
    }
}
