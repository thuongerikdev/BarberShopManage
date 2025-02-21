using BM.Booking.Infrastructure;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Booking.ApplicationService.Common
{
    public class BookingServiceBase
    {
        protected readonly ILogger _logger;
        protected readonly BookingDbContext _dbContext;
        public BookingServiceBase(ILogger logger, BookingDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }
    }
}
