using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace CityInfo.API.Services
{
    public class CloudMailService : IMailService
    {
        private string _mailTo = Startup.Configuration["mailSettings:mailTo"];
        private string _mailFrom = Startup.Configuration["mailSettings:mailFrom"];
        private string _smtpServer = Startup.Configuration["mailSettings:smtpServer"];

        public void Send(string subject, string message)
        {
            SmtpClient client = new SmtpClient(_smtpServer);
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(Startup.Configuration["mailSettings:smtpUser"], Startup.Configuration["mailSettings:smtpPasswd"]);

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(_mailFrom);
            mailMessage.To.Add(_mailTo);
            mailMessage.Body = $"{message}";
            mailMessage.Subject = $"RELEASE: {subject}";

            client.Send(mailMessage);
        }
    }
}
