using System;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using System.Net.Mail;
using NLog;

namespace CheckService2
{
    class SmtpMail
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public static void SendEmail (string from, string to, string Subject, string Body, string Host, int Port, string Password)
        {
            try
            {
                logger.Debug("Инициирую отправку сообщения с параметрами: " + from + " "+ to + " " + Subject + " " + Body + " " + Host + " " + Port + " " + "Password");
                MailAddress fromMailAddress = new MailAddress(from, "CheckService");
                if (fromMailAddress != null)
                {
                    logger.Debug("Переменной fromMailAddress передано значение  = " + fromMailAddress);
                }
                else
                {
                    logger.Error("Переменной fromMailAddress передано значение  = " + fromMailAddress);
                }
                MailAddress toAddress = new MailAddress(to, "Recepient");
                if (toAddress != null)
                {
                    logger.Debug("Переменной toAddress передано не корректное значение = " + toAddress);
                }
                else
                {
                    logger.Error("Переменной toAddress передано не корректное значение = " + toAddress);
                }
                using (MailMessage mailMessage = new MailMessage(from, to))
                using (SmtpClient smtpClient = new SmtpClient())
                {
                    mailMessage.Subject = Subject;
                    mailMessage.Body = Body;
                    mailMessage.IsBodyHtml = true;
                    smtpClient.Host = Host;
                    smtpClient.Port = Port;
                    smtpClient.EnableSsl = true;
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential(fromMailAddress.Address, Password);

                    smtpClient.Send(mailMessage);

                    logger.Info("Сообщение от " + fromMailAddress + " отправлено на адрес " + toAddress);
                }
            }
            catch
            {
                Exception ex = new Exception();
                Console.ForegroundColor = ConsoleColor.Red;
                logger.Error("При отправке сообщения произвошла ошибка " + ex.Message + " " + ex.HResult + " " + ex.Data);
                Console.ResetColor();
            }
            finally
            {
                Console.Read();
            }
        }
    }
}
