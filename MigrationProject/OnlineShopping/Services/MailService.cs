using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using Online.Models;
using Online.Interfaces;

public class MailService : IMailService
{
    private readonly IConfiguration _config;

    public MailService(IConfiguration config)
    {
        _config = config;
    }

    public async Task<bool> SendContactAutoReplyAsync(ContactUs contact)
    {
        var smtpClient = new SmtpClient(_config["Mail:SmtpHost"])
        {
            Port = int.Parse(_config["Mail:Port"]),
            Credentials = new NetworkCredential(_config["Mail:Username"], _config["Mail:Password"]),
            EnableSsl = true
        };

        string body = $@"
            <p>Hi <strong>{contact.Name}</strong>,</p>
            <p>Thank you for contacting us!</p>
            <p>We’ve received your message and will get back to you soon.</p>
            <hr/>
            <p><strong>Your message:</strong></p>
            <blockquote>{contact.Message}</blockquote>
            <p>Regards,<br/>Support Team</p>
        ";

        var message = new MailMessage
        {
            From = new MailAddress(_config["Mail:From"]),
            Subject = $"Thanks for contacting us, {contact.Name}",
            Body = body,
            IsBodyHtml = true
        };

        message.To.Add(contact.Email);

        await smtpClient.SendMailAsync(message);
        return true;
    }
}
