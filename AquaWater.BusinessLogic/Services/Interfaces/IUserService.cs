using AquaWater.Dto.Request;
using AquaWater.Dto.Response;
using System.Threading.Tasks;

namespace AquaWater.Data.Repository.Interfaces
{
    public interface IUserService
    {
        public Task<UserResponseDto> RegisterAsync(UserRegistrationRequestDTO registrationRequest);
    }
}
