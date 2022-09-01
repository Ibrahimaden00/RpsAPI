
using System.Net;
using System.Net.Mail;
namespace RpsAPI.Models
{
    public class Mail
    {

        public static void Email(string token, string mail)
        {
            try
            {
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress("testkonto167@gmail.com");
                message.To.Add(new MailAddress(mail));
                message.Subject = "Reset Code";
                message.IsBodyHtml = true; //to make message body as html  
                message.Body = "This is your reset code " +token;
                smtp.Port = 587;
                smtp.Host = "smtp.gmail.com"; //for gmail host  
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("testkonto167@gmail.com", "yamtmoxgpkryyxte");
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
            }
            catch (Exception) { }
        }   
    }
}
