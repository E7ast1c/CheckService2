using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceProcess;
using System.Threading;
using System.Net;
using System.IO;
using System.Net.Mail;
using NLog;

namespace CheckService2
{
    class Program
    {
        static void Main(string[] args)
        {
            
            for (int i=0; ;i++ )
            {
                GetService("Service1", "Server1");
                GetService("Service2", "Server2");
                GetService("Service3", "Server3");
                GetService("Service4", "Server4");
            }
        }

        static void GetService (string ServiceName,string ComputerName)
        {

            int CheckServiceTimer = 3000; // период срабатывания таймера на опрос сервиса
            logger.Trace("Период срабатывания таймера на опрос сервиса = " + CheckServiceTimer);
            try
            {
                ServiceController[] scServices;
                scServices = ServiceController.GetServices();

                
                foreach (ServiceController scTemp in scServices)
                {
                    ServiceController sc = new ServiceController(ServiceName, ComputerName);
                    Thread.Sleep(CheckServiceTimer); // сам таймер на задержку
                    if (sc.Status == ServiceControllerStatus.Running)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        logger.Info (ComputerName + " (" + sc.DisplayName + ")" + " Status = " + sc.Status);
                        Console.ResetColor();
                        break;
                    }

                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        logger.Info (ComputerName + " (" + sc.DisplayName + ")" + " Status = " + sc.Status);
                        Console.ResetColor();
                        SmtpMail.SendEmail("example@gmail.com",
                            "example@gmail.com",
                            sc.Status + " " + sc.DisplayName + " on server " + ComputerName,
                            "<h4>" + sc.Status + " " + sc.DisplayName + " on server " + ComputerName + "</h4>",
                            "smtp.gmail.com",
                            587,
                            "Password");
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                logger.Error(DateTime.Now.ToString() + " " + ex.Message);
               // Console.WriteLine (DateTime.Now.ToString() + " " + ex.Message);
                Console.ResetColor();
            }
        }

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
       
    }
}
