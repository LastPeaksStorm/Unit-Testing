using System;
using System.IO;

namespace TestNinja.Mocking
{
    public interface IHousekeeperStorage
    {
        string SaveInfoToTheStatement(Housekeeper housekeeper, DateTime statementDate);
    }
    public class HousekeeperStorage : IHousekeeperStorage
    {
        public string SaveInfoToTheStatement(Housekeeper housekeeper, DateTime statementDate)
        {
            return SaveStatement(housekeeper.Oid, housekeeper.FullName, statementDate);
        }
        
        private static string SaveStatement(int housekeeperOid, string housekeeperName, DateTime statementDate)
        {
            var report = new HousekeeperStatementReport(housekeeperOid, statementDate);

            if (!report.HasData)
                return string.Empty;

            report.CreateDocument();

            var filename = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                string.Format("Sandpiper Statement {0:yyyy-MM} {1}.pdf", statementDate, housekeeperName));

            report.ExportToPdf(filename);

            return filename;
        }
        
    }
}
