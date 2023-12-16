using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace TestNinja.Mocking
{
    public class HousekeeperHelper
    {
        private readonly IUnitOfWork _unitOfWork;
        IEmailStorage _emailStorage;
        IHousekeeperStorage _housekeeperStorage;
        IXtraMessageBox _xtraMessageBox;

        public HousekeeperHelper(
            IUnitOfWork unitOfWork, 
            IHousekeeperStorage housekeeperStorage, 
            IEmailStorage emailStorage, 
            IXtraMessageBox xtraMessageBox)
        {
            _unitOfWork = unitOfWork;
            _emailStorage = emailStorage;
            _housekeeperStorage = housekeeperStorage;
            _xtraMessageBox = xtraMessageBox;
        }

        public bool SendStatementEmails(DateTime statementDate)
        {
            IQueryable<Housekeeper> housekeepers = _unitOfWork.Query<Housekeeper>();

            foreach (var housekeeper in housekeepers)
            {
                if (housekeeper.Email == null)
                    continue;

                string statementFilename = _housekeeperStorage.SaveInfoToTheStatement(housekeeper, statementDate);

                if (string.IsNullOrWhiteSpace(statementFilename))
                    continue;

                var emailAddress = housekeeper.Email;
                var emailBody = housekeeper.StatementEmailBody;

                try
                {
                    _emailStorage.SendFileAsEmail(
                        new FileEmail
                        {
                            EmailAddress = emailAddress,
                            EmailBody = emailBody,
                            StatementFilename = statementFilename,
                            StatementDate = statementDate,
                            AddressorName = housekeeper.FullName
                        });
                }
                catch (Exception e)
                {
                    _xtraMessageBox.Show(e.Message, string.Format("Email failure: {0}", emailAddress),
                        MessageBoxButtons.OK);
                }
            }

            return true;
        }
    }

    public enum MessageBoxButtons
    {
        OK
    }

    public class XtraMessageBox : IXtraMessageBox
    {
        public void Show(string s, string housekeeperStatements, MessageBoxButtons ok)
        {
        }
    }

    public class MainForm
    {
        public bool HousekeeperStatementsSending { get; set; }
    }

    public class DateForm
    {
        public DateForm(string statementDate, object endOfLastMonth)
        {
        }

        public DateTime Date { get; set; }

        public DialogResult ShowDialog()
        {
            return DialogResult.Abort;
        }
    }

    public enum DialogResult
    {
        Abort,
        OK
    }

    public class SystemSettingsHelper
    {
        public static string EmailSmtpHost { get; set; }
        public static int EmailPort { get; set; }
        public static string EmailUsername { get; set; }
        public static string EmailPassword { get; set; }
        public static string EmailFromEmail { get; set; }
        public static string EmailFromName { get; set; }
    }

    public class Housekeeper
    {
        public string Email { get; set; }
        public int Oid { get; set; }
        public string FullName { get; set; }
        public string StatementEmailBody { get; set; }
    }

    public class HousekeeperStatementReport
    {
        public HousekeeperStatementReport(int housekeeperOid, DateTime statementDate)
        {
        }

        public bool HasData { get; set; }

        public void CreateDocument()
        {
        }

        public void ExportToPdf(string filename)
        {
        }
    }
}