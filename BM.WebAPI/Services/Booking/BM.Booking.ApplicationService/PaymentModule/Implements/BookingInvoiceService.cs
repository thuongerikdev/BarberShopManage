using BM.Booking.ApplicationService.Common;
using BM.Booking.ApplicationService.PaymentModule.Abtracts;
using BM.Booking.Domain;
using BM.Booking.Dtos.CRUDDtos;
using BM.Booking.Infrastructure;
using BM.Constant;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Booking.ApplicationService.PaymentModule.Implements
{
    public class BookingInvoiceService : BookingServiceBase, IBookingInvoiceService
    {
        public BookingInvoiceService(ILogger<BookingInvoiceService> logger, BookingDbContext dbContext) : base(logger, dbContext)
        {
        }
        public async Task<ResponeDto> BookingCreateInvoice(BookingCreateInvoiceDto bookingCreateInvoiceDto)
        {
            _logger.LogInformation("BookingCreateInvoice");
            try
            {
                var invoice = new BookingInvoice
                {
                    invoiceDate = bookingCreateInvoiceDto.invoiceDate,
                    status = bookingCreateInvoiceDto.status,
                    totalAmount = bookingCreateInvoiceDto.totalAmount,
                    orderID = bookingCreateInvoiceDto.orderID,
                    paymentTerms = bookingCreateInvoiceDto.paymentTerms,
                    paymentMethod = bookingCreateInvoiceDto.paymentMethod

                };
                _dbContext.BookingInvoices.Add(invoice);
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Tạo invoice thành công", invoice);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> BookingUpdateInvoice(BookingUpdateInvoiceDto bookingUpdateInvoiceDto)
        {
            _logger.LogInformation("BookingUpdateInvoice");
            try
            {
                var invoice = await _dbContext.BookingInvoices.FindAsync(bookingUpdateInvoiceDto.invoiceID);
                if (invoice == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy invoice");
                }
                invoice.invoiceDate = bookingUpdateInvoiceDto.invoiceDate;
                invoice.status = bookingUpdateInvoiceDto.status;
                invoice.totalAmount = bookingUpdateInvoiceDto.totalAmount;
                invoice.orderID = bookingUpdateInvoiceDto.orderID;
                invoice.paymentTerms = bookingUpdateInvoiceDto.paymentTerms;
                invoice.paymentMethod = bookingUpdateInvoiceDto.paymentMethod;
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Cập nhật invoice thành công", invoice);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> BookingDeleteInvoice(int invoiceID)
        {
            _logger.LogInformation("BookingDeleteInvoice");
            try
            {
                var invoice = await _dbContext.BookingInvoices.FindAsync(invoiceID);
                if (invoice == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy invoice");
                }
                _dbContext.BookingInvoices.Remove(invoice);
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Xóa invoice thành công", invoice);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> BookingGetInvoice(int invoiceID)
        {
            _logger.LogInformation("BookingGetInvoice");
            try
            {
                var invoice = await _dbContext.BookingInvoices.FindAsync(invoiceID);
                if (invoice == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy invoice");
                }
                return ErrorConst.Success("Lấy invoice thành công", invoice);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> BookingGetAllInvoice()
        {
            _logger.LogInformation("BookingGetAllInvoice");
            try
            {
                var invoices = await _dbContext.BookingInvoices.ToListAsync();
                return ErrorConst.Success("Lấy danh sách invoice thành công", invoices);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> BookingGetInvoiceByOrderID(int orderID)
        {
            _logger.LogInformation("BookingGetInvoiceByOrderID for orderID: {OrderID}", orderID);

            // Validate input
            if (orderID <= 0)
            {
                return ErrorConst.Error(400, "OrderID phải lớn hơn 0");
            }

            try
            {
                var invoice = await _dbContext.BookingInvoices

                    .Where(x => x.orderID == orderID)
                    .Select(x => new 
                    {
                        InvoiceID = x.invoiceID,
                        InvoiceDate = x.invoiceDate,
                        Status = x.status,
                        TotalAmount = x.totalAmount,
                        OrderID = x.orderID,
                        PaymentTerms = x.paymentTerms,
                        PaymentMethod = x.paymentMethod,
                        Order = new 
                        {
                            OrderID = x.BookingOrder.orderID,
                            CustID = x.BookingOrder.custID,
                            OrderStatus = x.BookingOrder.orderStatus,
                            Appointments = x.BookingOrder.BookingAppointments
                                .Select(y => new 
                                {
                                    AppointmentID = y.appID,
                                    EmpID = y.empID,
                                    AppointmentStatus = y.appStatus,
                                    Service = y.serviceDetailID,



                                })
                                .ToList()
                        }
                    })
                    .FirstOrDefaultAsync();

                if (invoice == null)
                {
                    return ErrorConst.Error(404, "Không tìm thấy invoice cho orderID này");
                }

                return ErrorConst.Success("Lấy invoice thành công", invoice);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy invoice cho orderID: {OrderID}", orderID);
                return ErrorConst.Error(500, "Đã xảy ra lỗi khi xử lý yêu cầu");
            }
        }

        }
}
