using System;
using System.Collections.Generic;
using System.Linq;

namespace TestNinja.Mocking
{
    public static class BookingHelper
    {
        public static string OverlappingBookingsExist(Booking booking, IBookingRepository bookingRepository = null)
        {
            var _bookingRepository = bookingRepository ?? new BookingRepository();

            if (booking.Status == "Cancelled")
                return string.Empty;

            var _bookings = bookingRepository.GetOtherBookings(booking.Id);

            var overlappingBooking = _bookings.FirstOrDefault(
                    b =>
                        booking.ArrivalDate < b.DepartureDate
                        && b.ArrivalDate < booking.DepartureDate);

            return overlappingBooking == null
                ? string.Empty
                : overlappingBooking.Reference;
        }
    }

    public interface IUnitOfWork
    {
        IQueryable<T> Query<T>();
    }
    public class UnitOfWork : IUnitOfWork
    {
        public IQueryable<T> Query<T>()
        {
            return new List<T>().AsQueryable();
        }
    }

    public class Booking
    {
        public string Status { get; set; }
        public int Id { get; set; }
        public DateTime ArrivalDate { get; set; }
        public DateTime DepartureDate { get; set; }
        public string Reference { get; set; }
    }
}