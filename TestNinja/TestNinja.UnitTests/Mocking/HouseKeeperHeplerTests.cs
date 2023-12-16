using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using TestNinja.Mocking;

namespace TestNinja.UnitTests.Mocking
{
    [TestFixture]
    public class HouseKeeperHeplerSendStatementEmailsTests
    {
        HousekeeperHelper housekeeperHelper;
        IUnitOfWork unitOfWork;
        IHousekeeperStorage housekeeperStorage;
        IEmailStorage emailStorage;
        IXtraMessageBox xtraMessageBox;
        DateTime dateTime;

        [SetUp]
        public void SetUp()
        {
            unitOfWork = Substitute.For<IUnitOfWork>();
            housekeeperStorage = Substitute.For<IHousekeeperStorage>();
            emailStorage = Substitute.For<IEmailStorage>();
            xtraMessageBox = Substitute.For<IXtraMessageBox>();
            housekeeperHelper = new HousekeeperHelper(unitOfWork, housekeeperStorage, emailStorage, xtraMessageBox);
            dateTime = new DateTime(2006, 9, 8);
        }

        [Test]
        public void NoHouseKeepersFound_WillNotCallTheFileStatementMethod()
        {
            unitOfWork.Query<Housekeeper>().Returns(new List<Housekeeper> { }.AsQueryable());

            housekeeperHelper.SendStatementEmails(dateTime);

            housekeeperStorage.DidNotReceive().SaveInfoToTheStatement(Arg.Any<Housekeeper>(), dateTime);
        }

        [Test]
        public void NoHouseKeepersWithNullEmail_WillNotCallTheFileStatementMethod()
        {
            unitOfWork.Query<Housekeeper>().Returns(
                new List<Housekeeper>
                {
                    new Housekeeper
                    {}
                }.AsQueryable());

            housekeeperHelper.SendStatementEmails(dateTime);

            housekeeperStorage.DidNotReceive().SaveInfoToTheStatement(Arg.Any<Housekeeper>(), dateTime);
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void StatementFileIsEmpty_WillNotCallSendFileAsMessageMethod(string emptyReturns)
        {
            unitOfWork.Query<Housekeeper>().Returns(
                new List<Housekeeper>
                {
                    new Housekeeper
                    {Email="a@.c"},
                    new Housekeeper
                    {Email="a@.c"}
                }.AsQueryable());
            housekeeperStorage.SaveInfoToTheStatement(Arg.Any<Housekeeper>(), dateTime).Returns(emptyReturns);

            housekeeperHelper.SendStatementEmails(dateTime);

            emailStorage.DidNotReceive().SendFileAsEmail(Arg.Any<FileEmail>());
        }

        [Test]
        public void SendFileEmailThrowsAnException_WillNotCallXtraMessageBoxShow()
        {
            unitOfWork.Query<Housekeeper>().Returns(
                new List<Housekeeper>
                {
                    new Housekeeper
                    {
                        Email="a@.c"
                    },
                    new Housekeeper
                    {
                        Email="b@.c"
                    }
                }.AsQueryable());
            Exception exc = new Exception();
            housekeeperStorage.SaveInfoToTheStatement(Arg.Any<Housekeeper>(), dateTime).Returns("Not empty.");
            emailStorage.When(x => x.SendFileAsEmail(Arg.Any<FileEmail>())).Do(x => { throw exc; });

            housekeeperHelper.SendStatementEmails(dateTime);

            xtraMessageBox.Received(1).Show(exc.Message, "Email failure: a@.c", MessageBoxButtons.OK);
            xtraMessageBox.Received(1).Show(exc.Message, "Email failure: b@.c", MessageBoxButtons.OK);
        }

        [Test]
        public void ValidHousekeepersFound_CallTheSaveInfoStatementMethod()
        {
            Housekeeper[] housekeepers = new Housekeeper[]
                {
                    new Housekeeper
                    {
                        Email="email1",
                        Oid=1
                    },
                    new Housekeeper
                    {
                        Email="email2",
                        Oid=2
                    }
                };
            unitOfWork.Query<Housekeeper>().Returns(housekeepers.AsQueryable());

            housekeeperHelper.SendStatementEmails(new DateTime(2006, 9, 8));

            housekeeperStorage.Received(1).SaveInfoToTheStatement(housekeepers[0], dateTime);
            housekeeperStorage.Received(1).SaveInfoToTheStatement(housekeepers[1], dateTime);
        }

        [Test]
        public void ValidStatementFile_CallSendFileAsEmailMethod()
        {
            Housekeeper[] housekeepers = new Housekeeper[]
                {
                    new Housekeeper
                    {
                        FullName="Jack Smith",
                        Email="email1",
                        StatementEmailBody="email body1",
                        Oid=1
                    },
                    new Housekeeper
                    {
                        FullName="Jack Swag",
                        Email="email2",
                        StatementEmailBody="email body2",
                        Oid=2
                    }
                };
            unitOfWork.Query<Housekeeper>().Returns(housekeepers.AsQueryable());
            housekeeperStorage.SaveInfoToTheStatement(Arg.Any<Housekeeper>(), dateTime).Returns("string");

            housekeeperHelper.SendStatementEmails(dateTime);

            emailStorage.Received(1).SendFileAsEmail(Arg.Is<FileEmail>
                (fe =>
                    fe.EmailAddress == housekeepers[0].Email
                    && fe.EmailBody == housekeepers[0].StatementEmailBody
                    && fe.StatementFilename == "string"
                    && fe.StatementDate == dateTime
                    && fe.AddressorName == housekeepers[0].FullName
                ));
            emailStorage.Received(1).SendFileAsEmail(Arg.Is<FileEmail>
                (fe =>
                    fe.EmailAddress == housekeepers[1].Email
                    && fe.EmailBody == housekeepers[1].StatementEmailBody
                    && fe.StatementFilename == "string"
                    && fe.StatementDate == dateTime
                    && fe.AddressorName == housekeepers[1].FullName
                ));
        }
    }
}
