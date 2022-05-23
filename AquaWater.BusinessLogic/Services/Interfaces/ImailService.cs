using AquaWater.Domain.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaWater.Data.Services.Interfaces
{
    public interface IMailService
    {
        string GetEmailTemplate(string templateName);
        Task SendEmailAsync(MailRequest mailRequest);
    }
}
