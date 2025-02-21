using BM.Booking.Dtos;
using BM.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Booking.ApplicationService.PaymentModule.Abtracts
{
    public interface IBookingOrderService
    {
        public Task <ResponeDto> BookingCreateOrder(BookingCreateOrderDto bookingCreateOrderDto);
        public Task<ResponeDto> BookingUpdateOrder(BookingUpdateOrderDto bookingUpdateOrderDto);
        public Task<ResponeDto> BookingDeleteOrder(int orderID);
        public Task<ResponeDto> BookingGetOrder(int orderID);
        public Task<ResponeDto> BookingGetAllOrder();
    }
}
