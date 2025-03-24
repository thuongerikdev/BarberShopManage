using BM.Booking.Dtos.CRUDdtos;
using BM.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Booking.ApplicationService.PaymentModule.Abtracts
{
    public interface IBookingPromoService
    {
        public Task <ResponeDto> BookingCreatePromo(BookingCreatePromotDto bookingCreatePromoDto);
        public Task<ResponeDto> BookingUpdatePromo(BookingUpdatePromotDto bookingUpdatePromoDto);
        public Task<ResponeDto> BookingDeletePromo(int promoID);
        public Task<ResponeDto> BookingGetPromo(int promoID);
        public Task<ResponeDto> BookingGetAllPromo();
    }
}
