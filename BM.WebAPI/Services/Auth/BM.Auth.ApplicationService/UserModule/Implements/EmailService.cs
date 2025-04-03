using BM.Auth.ApplicationService.UserModule.Abtracts;
using BM.Auth.Domain;
using BM.Auth.Dtos;
using BM.Constant;
using BM.Shared.ApplicationService;
using MailKit.Search;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MimeKit;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmailSettings = BM.Auth.Domain.EmailSettings;

namespace BM.Auth.ApplicationService.UserModule.Implements
{
    public class EmailService : IEmailService , IEmailCancelBooking
    {
        private readonly EmailSettings _emailSettings;
        private readonly string _baseUrl;

        public EmailService(
            IOptions<EmailSettings> emailSettings,
            IConfiguration configuration)
        {
            _emailSettings = emailSettings.Value;
            // Lấy URL từ Kestrel hoặc ApplicationSettings trong appsettings.json
            _baseUrl = configuration["Kestrel:Endpoints:Http:Url"] ?? configuration["ApplicationSettings:BaseUrl"];
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_emailSettings.DisplayName, _emailSettings.Mail));
            email.To.Add(new MailboxAddress(to, to));
            email.Subject = subject;
            var password = _emailSettings.Password.Trim();


            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = body
            };
            email.Body = bodyBuilder.ToMessageBody();

            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            try
            {
                // Specify StartTls explicitly
                await smtp.ConnectAsync(_emailSettings.Host, _emailSettings.Port, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_emailSettings.Mail, password);
                await smtp.SendAsync(email);
            }
            finally
            {
                await smtp.DisconnectAsync(true);
            }
        }

        //public async Task<ResponeDto> SendInvoiceEmail(int userID)
        //{
        //    var subject = "Tournament Activation Confirmation";
        //    //var user = await _authUserService.AuthGetUser(userID);
        //    var userData = user.Data as AuthUser;
        //    var email = userData.email;
        //    var body = "Your Tournament Has been Activated";

        //    // Creating a professional HTML email body


        //    await SendEmailAsync(email, subject, body);
        //    return ErrorConst.Success("Email Sent Successfully");


        //}

        public async Task TestSend(string to)
        {
            var subJect = "Activate Email";
            var body = "Your Tournament Has been Activated ";
            await SendEmailAsync(to, subJect, body);
        }

        public async Task SendCancelEmail(string to , int orderID , DateTime createAt)
        {
            string subject = "Thông báo hủy đơn hàng";
            string body = $@"
                        <h2>Thông báo từ hệ thống</h2>
                        <p>Đơn hàng 
            {orderID} của bạn đã bị hủy tự động vì quá 30 phút kể từ khi đến hẹn  ({createAt}).</p>
                        <p>Vui lòng đặt lại lịch nếu cần.</p>";
            await SendEmailAsync(to, subject, body);
        }

        public async Task SendBookingCreate(string to, int orderID, DateTime createAt)
        {
            string subject = "Thông báo đặt lịch thành công ";
            string body = $@"
                        <h2>Thông báo từ hệ thống</h2>
                        <p>Đơn hàng 
            {orderID} của bạn đã đã được đặt thành công , vui lòng đến đúng hẹn   ({createAt}).</p>
                        <p>Vui lòng đặt lại lịch nếu cần.</p>";
            await SendEmailAsync(to, subject, body);
        }

        public async Task SendVerificationEmail(int userId, string email, string verificationToken)
        {
            var subject = "Verify Your Email Address";
            var verificationLink = $"{_baseUrl}/api/AuthUser/verify-email?token={verificationToken}&userId={userId}";

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = $@"
                <!DOCTYPE html>
                <html lang='vi'>
                <head>
                    <meta charset='UTF-8'>
                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                    <style>
                        body {{ font-family: Arial, sans-serif; margin: 0; padding: 0; background-color: #f4f4f4; color: #333; }}
                        .container {{ max-width: 600px; margin: 20px auto; background-color: #ffffff; border-radius: 8px; box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1); overflow: hidden; }}
                        .header {{ background-color: #4CAF50; padding: 20px; text-align: center; color: #ffffff; }}
                        .header h1 {{ margin: 0; font-size: 24px; }}
                        .content {{ padding: 20px; line-height: 1.6; }}
                        .content p {{ margin: 10px 0; }}
                        .button {{ display: inline-block; padding: 12px 25px; background-color: #4CAF50; color: #ffffff; text-decoration: none; border-radius: 5px; margin: 20px 0; }}
                        .button:hover {{ background-color: #45a049; }}
                        .footer {{ background-color: #f1f1f1; padding: 10px; text-align: center; font-size: 12px; color: #777; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h1>Cảm ơn bạn đã đăng ký!</h1>
                        </div>
                        <div class='content'>

                            <p>Chúng tôi rất vui mừng chào đón bạn đến với dịch vụ của <strong>Barber Management</strong>! Đăng ký của bạn đã được thực hiện thành công .</p>
                            <p>Để bắt đầu trải nghiệm dịch vụ, vui lòng nhấp vào nút dưới đây để xác nhận email của bạn:</p>
                            <a href='{verificationLink}' class='button'>Xác nhận email</a>
                            <p>Nếu bạn không thực hiện đăng ký này, vui lòng bỏ qua email này hoặc liên hệ với chúng tôi qua <a href='mailto:support@barbermanagement.com'>support@barbermanagement.com</a>.</p>
                            <p>Chúng tôi mong được phục vụ bạn!</p>
                            <p>Trân trọng,<br>Đội ngũ Barber Management</p>
                        </div>
                        <div class='footer'>
                            <p>&copy; 2023 Barber Management. Mọi quyền được bảo lưu.</p>
                            <p><a href='https://barbermanagement.com'>Trang web của chúng tôi</a> | <a href='mailto:support@barbermanagement.com'>Liên hệ hỗ trợ</a></p>
                        </div>
                    </div>
                </body>
                </html>"
            };

            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_emailSettings.DisplayName, _emailSettings.Mail));
            emailMessage.To.Add(new MailboxAddress(email, email));
            emailMessage.Subject = subject;
            emailMessage.Body = bodyBuilder.ToMessageBody();

            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            try
            {
                await smtp.ConnectAsync(_emailSettings.Host, _emailSettings.Port, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_emailSettings.Mail, _emailSettings.Password.Trim());
                await smtp.SendAsync(emailMessage);
            }
            finally
            {
                await smtp.DisconnectAsync(true);
            }
        }
    }
}
