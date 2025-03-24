using BM.Booking.Dtos.CRUDdtos;
using BM.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Booking.ApplicationService.PaymentModule.Abtracts
{
    public interface IBookingServProService
    {
        public Task<ResponeDto> BookingCreateServPro(BookingCreateServPromoDto bookingCreateServPromo);
        public Task<ResponeDto> BookingUpdateServPro(BookingUpdateServPromoDto bookingUpdateServPromoDto);
        public Task<ResponeDto> BookingDeleteServPro(int serviceID);
        public Task<ResponeDto> BookingGetServPro(int serviceID);
        public Task<ResponeDto> BookingGetAllServPro();
    }
}
