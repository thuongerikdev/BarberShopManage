using BM.Booking.Dtos.CRUDDtos;
using BM.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Booking.ApplicationService.PaymentModule.Abtracts
{
    public interface IBookingInvoiceService
    {
        public Task<ResponeDto> BookingCreateInvoice(BookingCreateInvoiceDto invoice);
        public Task<ResponeDto> BookingUpdateInvoice(BookingUpdateInvoiceDto invoice);
        public Task<ResponeDto> BookingDeleteInvoice(int invoiceID);
        public Task<ResponeDto> BookingGetInvoice(int invoiceID);
        public Task<ResponeDto> BookingGetAllInvoice();
        Task<ResponeDto> BookingGetInvoiceByOrderID(int orderID);
    }
}
