using System.Net;
using System.Net.Mail;

namespace Identity.Service.Helpers.EmailHelper
{
   public class EmailConfirm
   {
      public static void SendEmailForConfirmation(string link, string email)
      {
         MailMessage mail = new MailMessage();
         SmtpClient smtpClient = new SmtpClient(); // host
         mail.From = new MailAddress("srcnsvr16@gmail.com", "Sercan Sever");
         mail.To.Add(email);
         mail.Subject = "Hesabı Doğrular";
         mail.Body = "<h3 class='d-flex justify-content-center m-md-3'>Hesabınızı doğrulamak için aşağıdaki linke tıklayınız</h3><hr/>";
         mail.Body += $"<a href='{link}'>Hesabı Doğrula</a>";
         mail.IsBodyHtml = true;
         smtpClient.Host = "smtp.gmail.com";
         smtpClient.Port = 587;
         smtpClient.Credentials = new NetworkCredential("srcnsvr16@gmail.com", "SS2468024");
         smtpClient.EnableSsl = true;
         smtpClient.Send(mail);
      }
   }
}