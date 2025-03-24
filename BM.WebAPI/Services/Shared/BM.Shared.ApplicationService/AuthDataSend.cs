using BM.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Shared.ApplicationService
{
    public interface AuthDataSend
    {
        public Task<ResponeDto> GetUserToOtherDomain(int userID);
    }
    public interface IGetCustomerByOrUserID
    {
        public Task<ResponeDto> GetCustomerByOrUserID(int userID);
    }
    public interface IEmailCancelBooking
    {
        public Task SendCancelEmail(string to, int orderID, DateTime createAt);
    }
}
