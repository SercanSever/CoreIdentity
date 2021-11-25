using System.Net;
using System.Net.Mail;

namespace Identity.Service.Helpers.EmailHelper
{
   public class PasswordReset
   {
      public static void PasswordResetSendEmail(string link, string email)
      {
         MailMessage mail = new MailMessage();
         SmtpClient smtpClient = new SmtpClient(); // host
         mail.From = new MailAddress("srcnsvr16@gmail.com", "Sercan Sever"); 
         mail.To.Add(email);
         mail.Subject = "Şifre Yenileme";
         mail.Body = "<h3>Şifrenizi yenilemek için aşağıdaki linke tıklayınız.</h3><hr/>";
         mail.Body += $"<a href='{link}'>Şifre Yenile</a>";
         mail.IsBodyHtml = true;
         smtpClient.Host="smtp.gmail.com";
         smtpClient.Port=587;
         smtpClient.Credentials=new NetworkCredential("srcnsvr16@gmail.com","SS2468024");
         smtpClient.EnableSsl = true;
         smtpClient.Send(mail);
      }
   }
}