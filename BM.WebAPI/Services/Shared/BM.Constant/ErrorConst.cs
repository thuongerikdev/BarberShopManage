using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Constant
{
    public static class ErrorConst
    {
        public static ResponeDto Success(string errorMessage , object data = null)
        {
            return new ResponeDto
            {
                ErrorCode = 0,
                ErrorMessager = errorMessage,
                Data = data ?? new List<object>() // Nếu không có dữ liệu, trả về danh sách rỗng
            };
        }

        // Phương thức trả về phản hồi lỗi
        public static ResponeDto Error(int errorCode, string errorMessage)
        {
            return new ResponeDto
            {
                ErrorCode = errorCode,
                ErrorMessager = errorMessage,
                Data = null // Không có dữ liệu trong trường hợp lỗi
            };
        }
    }
}
