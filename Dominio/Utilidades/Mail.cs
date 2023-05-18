using System.Net;
using System.Net.Mail;

namespace Dominio.Utilidades
{
    public static class Mail
    {
        public static string? From { get; set; }
        public static string? Host { get; set; }
        public static string? PassWord { get; set; }
        public static int Port { get; set; }
        public static bool ActivoSsl { get; set; }

        public static async Task EnviarMail(string email, string subject, string message)
        {
            try
            {
                MailMessage mail = new()
                {
                    From = new MailAddress(From)
                };

                mail.To.Add(new MailAddress(email));

                mail.Subject = subject;
                mail.Body = message;
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.High;

                using (SmtpClient smtp = new SmtpClient(Host, Port))
                {
                    smtp.Credentials = new NetworkCredential(From, PassWord);
                    smtp.EnableSsl = ActivoSsl;
                    smtp.UseDefaultCredentials = false;
                    await smtp.SendMailAsync(mail);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
