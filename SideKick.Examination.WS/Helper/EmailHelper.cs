using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace SideKick.Examination.WS.Helper
{
    public static class EmailHelper
    {
        public static async Task<bool> SendKeyCode(string code, string emailTo, string displayName)
        {
            try
            {
                var fromAddress = new MailAddress("sidekick.exam.marck@gmail.com", "Sidekick Support");
                var toAddress = new MailAddress(emailTo, displayName);
                const string fromPassword = "$ideKick123456";

                const string subject = "Verify your Sidekick account";
                string body = $"Hi {displayName}, <br><br> Welcome to the wonderful world of Sidekick! <br><br>" +
                    "To complete registration, enter the PIN code below into your Sidekick app. <br><br>" +
                    $"Your <b style='color:red;font-size:18px;'>PIN CODE: <u>{code}</u> </b> <br><br>" +
                    $"If the code doesn't work, please wait and try again after 1 minute. <br><br>" +
                    $"Please note these codes are only valid for 5 minutes.";

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
                };

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                    
                })
                {
                    await smtp.SendMailAsync(message);
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }


    }
}
