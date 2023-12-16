using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using TestNinja.Mocking;

namespace TestNinja.UnitTests.Mocking
{
    [TestFixture]
    public class BookingHelperOverlappingBookingsExistTests
    {
        IBookingRepository _bookRepository;

        [SetUp]
        public void Setup()
        {
            _bookRepository = Substitute.For<IBookingRepository>();
        }

        [Test]
        public void IfStatusOfTheBookingCancelledButBookingsOverlap_ReturnEmptyString()
        {
            var booking = new Booking {
                Id = 1,
                ArrivalDate = new DateTime(2003, 5, 4),
                DepartureDate = new DateTime(2077, 7, 7),
                Status = "Cancelled",
                Reference = "Refr77"
            };

            var result = BookingHelper.OverlappingBookingsExist(booking, _bookRepository);

            Assert.That(result, Is.EqualTo(string.Empty));
        }

        [Test]
        [TestCaseSource(nameof(notOverlappingBookingsSource))]
        public void NewBookingDoesNotOverlapTheExistingBooking_ReturnsEmptyString(Booking booking)
        {
            _bookRepository.GetOtherBookings(booking.Id).Returns(SampleBooking.sampleBookingsList);

            var result = BookingHelper.OverlappingBookingsExist(booking, _bookRepository);

            Assert.That(result, Is.EqualTo(string.Empty));
        }

        [Test]
        [TestCaseSource(nameof(overlappingBookingsSource))]
        public void NewBookingOverlapsTheExistingBooking_ReturnsTheReferenceOfTheOverlappingBooking(Booking booking)
        {
            _bookRepository.GetOtherBookings(booking.Id).Returns(SampleBooking.sampleBookingsList);

            var result = BookingHelper.OverlappingBookingsExist(booking, _bookRepository);

            Assert.That(result, Is.EqualTo("Shady"));
        }


        public static Booking[] overlappingBookingsSource =
        {
            new Booking
            {
                Id = 1,
                ArrivalDate = new DateTime(2015, 1, 1),
                DepartureDate = new DateTime(2017, 1, 2),
                Status = "Completed",
                Reference = "Gang"
            },
            new Booking
            {
                Id = 1,
                ArrivalDate = new DateTime(2005, 1, 1),
                DepartureDate = new DateTime(2024, 1, 2),
                Status = "Completed",
                Reference = "Gang"
            },
            new Booking
            {
                Id = 1,
                ArrivalDate = new DateTime(2007, 1, 1),
                DepartureDate = new DateTime(2024, 1, 2),
                Status = "Completed",
                Reference = "Gang"
            },
            new Booking
            {
                Id = 1,
                ArrivalDate = new DateTime(2005, 1, 1),
                DepartureDate = new DateTime(2020, 1, 2),
                Status = "Completed",
                Reference = "Gang"
            },
            new Booking
            {
                Id = 1,
                ArrivalDate = new DateTime(2006, 9, 8),
                DepartureDate = new DateTime(2023, 6, 22),
                Status = "Completed",
                Reference = "Gang"
            }
        };

        public static Booking[] notOverlappingBookingsSource =
        {
            new Booking
            {
                Id = 1,
                ArrivalDate = new DateTime(2005, 1, 1),
                DepartureDate = new DateTime(2006, 1, 1),
                Status = "Completed",
                Reference = "Gang"
            },
            new Booking
            {
                Id = 1,
                ArrivalDate = new DateTime(2024, 1, 1),
                DepartureDate = new DateTime(2024, 1, 2),
                Status = "Completed",
                Reference = "Gang"
            }
        };
    }


    public static class SampleBooking
    {
        public static IQueryable<Booking> sampleBookingsList = new List<Booking>
        {
            new Booking
            {
                Id = 50,
                ArrivalDate = new DateTime(2006, 9, 8),
                DepartureDate = new DateTime(2023, 6, 22),
                Status = "Active",
                Reference = "Shady"
            }
        }.AsQueryable();
    }
}
