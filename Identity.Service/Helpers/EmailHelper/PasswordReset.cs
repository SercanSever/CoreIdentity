using System.Net;
using System.Net.Mail;

namespace Identity.Service.Helpers.EmailHelper
{
   public class PasswordReset
   {
      public static PasswordResetSendEmail(string link, string email)
      {
         MailMessage mail = new MailMessage();
         SmtpClient smtpClient = new SmtpClient();
         mail.From = new MailAddress("sercan.sever16@gmail.com");
         mail.To.Add(email);
         mail.Subject = "Şifre Yenileme";
         mail.Body = "<h3>Şifrenizi yenilemek için aşağıdaki linke tıklayınız.</h3><hr/>";
         mail.Body += $"<a href='{link}'>Şifre Yenile</a>";
         mail.IsBodyHtml = true;
         smtpClient.Port=587;
         smtpClient.Credentials=new NetworkCredential("sercan.sever16@gmail.com","Deneme");
         smtpClient.Send(mail);
      }
   }
}