using Microsoft.SqlServer.Server;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace TestNinja.Mocking
{
    public interface IEmailStorage
    {
        void SendFileAsEmail(FileEmail fileEmail);
    }
    public class EmailStorage : IEmailStorage
    {
        public void SendFileAsEmail(FileEmail fileEmail)
        {
            EmailTheFile(
                fileEmail.EmailAddress, 
                fileEmail.EmailBody, 
                fileEmail.StatementFilename,
                string.Format("Sandpiper Crossing Statement {0:yyyy-MM} {1}", 
                              fileEmail.StatementDate, 
                              fileEmail.AddressorName));
        }

        private static void EmailTheFile(
            string emailAddress,
            string emailBody,
            string filename,
            string subject)
        {
            var client = new SmtpClient(SystemSettingsHelper.EmailSmtpHost)
            {
                Port = SystemSettingsHelper.EmailPort,
                Credentials =
                    new NetworkCredential(
                        SystemSettingsHelper.EmailUsername,
                        SystemSettingsHelper.EmailPassword)
            };

            var from = new MailAddress(SystemSettingsHelper.EmailFromEmail, SystemSettingsHelper.EmailFromName,
                Encoding.UTF8);
            var to = new MailAddress(emailAddress);

            var message = new MailMessage(from, to)
            {
                Subject = subject,
                SubjectEncoding = Encoding.UTF8,
                Body = emailBody,
                BodyEncoding = Encoding.UTF8
            };

            message.Attachments.Add(new Attachment(filename));
            client.Send(message);
            message.Dispose();

            File.Delete(filename);
        }
    }
}
