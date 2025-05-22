using BM.Booking.Dtos.BussinessDtos;
using BM.Booking.Dtos.CRUDdtos;
using BM.Constant;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Booking.ApplicationService.BussinessModule.Abtracts
{
    public interface IBookingBussinessService
    {
         //Task<ResponeDto> CreateOrderAppoint(List<BookingCreateBussinessAppointDto> appoint , BookingCreateOrderBussinessDto order , int promoID);
        Task<ResponeDto> CreateOrderAppoint(List<BookingCreateBussinessAppointDto> appoint, BookingCreateOrderBussinessDto order, int? promoID);

        public  Task<string> CreatePaymentUrl(int TournamentID, HttpContext context);

        PaymentResponseModel PaymentExecute(IQueryCollection collections);
    }
}
