﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace DeleteLog
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            try
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[] 
            { 
                new DeleteLogs() 
            };
                ServiceBase.Run(ServicesToRun);
            }
            catch (Exception ex)
            {
                
               StreamWriter sm = new StreamWriter(@"C:\log.txt");
                sm.WriteLine( ex.ToString());
               
            }
            
        }
    }
}
