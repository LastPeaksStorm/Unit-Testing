using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestNinja.Mocking
{
    public interface IBookingRepository
    {
        IQueryable<Booking> GetOtherBookings(int? bookingId = null);
    }
    public class BookingRepository : IBookingRepository
    {
        public IQueryable<Booking> GetOtherBookings(int? bookingId = null)
        {
            var unitOfWork = new UnitOfWork();
            var _bookings =
                unitOfWork.Query<Booking>()
                    .Where(
                        b => b.Id != bookingId && b.Status != "Cancelled");
            return _bookings;
        }
    }
}
