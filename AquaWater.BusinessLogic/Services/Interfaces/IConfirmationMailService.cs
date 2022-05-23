using AquaWater.Dto.Response;
using System.Threading.Tasks;

namespace AquaWater.Data.Services.Implementations
{
    public interface IConfirmationMailService
    {
        Task SendAConfirmationEmail(UserResponseDto user);
        Task SendAConfirmationEmailForResetPassword(UserResponseDto user);
    }
}