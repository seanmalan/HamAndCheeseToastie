using System.Net.Mail;

namespace HamAndCheeseToastie.Helpers;

public class EmailHelper
{
    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        using var smtpClient = new SmtpClient("smtp.your-email-provider.com")
        {
            Port = 587,
            Credentials = new System.Net.NetworkCredential("your-email@example.com", "your-password"),
            EnableSsl = true,
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress("your-email@example.com"),
            Subject = subject,
            Body = body,
            IsBodyHtml = false,
        };
        mailMessage.To.Add(toEmail);

        await smtpClient.SendMailAsync(mailMessage);
    }
}