using AquaWater.BusinessLogic.Utilities;
using AquaWater.Data.Services.Interfaces;
using AquaWater.Domain.Settings;
using AquaWater.Dto.Response;
using Microsoft.Extensions.Configuration;
using System.Globalization;
using System.Threading.Tasks;

namespace AquaWater.Data.Services.Implementations
{
    public class ConfirmationMailService : IConfirmationMailService
    {
        private readonly IMailService _mailService;
        private readonly IConfiguration _configuration;

        public ConfirmationMailService(IMailService mailService, IConfiguration configuration)
        {
            _mailService = mailService;
            _configuration = configuration;
        }
        public async Task SendAConfirmationEmail(UserResponseDto user)
        {
            var template = _mailService.GetEmailTemplate("EmailTemplate.html");
            TextInfo textInfo = new CultureInfo("en-GB", false).TextInfo;
            var userName = textInfo.ToTitleCase(user.FullName);

            var encodedToken = TokenConverter.EncodeToken(user.Token);
            var link = $"{_configuration["Application:AppDomain"]}/Authentication/ConfirmEmail?email={user.Email}&token={encodedToken}";

            template = template.Replace("{User}", $"{userName}");
            template = template.Replace("{Body}", "Welcome to AquaWater Plc, Registration was successful, click the link below");
            template = template.Replace("{Linkl}", link);
            template = template.Replace("{Details}", $"If you have trouble clicking on the link above you can paste this link on your browser {link}");
            template = template.Replace("{Action}", "Confirm Email");

            var mailRequest = new MailRequest
            {
                ToEmail = user.Email,
                Body = template,
                Subject = "Confirm Email"
            };

            await _mailService.SendEmailAsync(mailRequest);
        }

        public async Task SendAConfirmationEmailForResetPassword(UserResponseDto user)
        {
            var template = _mailService.GetEmailTemplate("EmailTemplate.html");
            TextInfo textInfo = new CultureInfo("en-GB", false).TextInfo;

            var userName = textInfo.ToTitleCase(user.FullName);
            var encodedToken = TokenConverter.EncodeToken(user.Token);
            var link = $"{_configuration["Application:AppDomain"]}/Authentication/ResetPassword?email={user.Email}&token={encodedToken}";

            string message = "Reset Password";

            template = template.Replace("{User}", $"{userName}");
            template = template.Replace("{Body}", "Welcome to AquaWater Plc,To reset password, click the link below");
            template = template.Replace("{Link}", link);
            template = template.Replace("{Details}", $"If you have trouble clicking on the link above you can paste this link on your browser {link}");
            template = template.Replace("{Action}", $"{message}");

            var mailRequest = new MailRequest
            {
                ToEmail = user.Email,
                Body = template,
                Subject = "Reset Password"
            };

            await _mailService.SendEmailAsync(mailRequest);
        }
    }
}
