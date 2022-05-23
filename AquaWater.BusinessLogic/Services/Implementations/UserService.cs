using AquaWater.Data.Repository.Interfaces;
using AquaWater.Data.Services.Interfaces;
using AquaWater.BusinessLogic.Utilities;
using AquaWater.Domain.Entities;
using AquaWater.Dto.Request;
using AquaWater.Dto.Response;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AquaWater.Data.Repository.Implementations
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public UserService(IMapper mapper, UserManager<User> userManager)
        {
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<UserResponseDto> RegisterAsync(UserRegistrationRequestDTO registrationRequest)
        {
            User user = _mapper.Map<User>(registrationRequest);
            user.UserName = registrationRequest.Email;
            user.ProfilePictureUrl = "None";
            IdentityResult result = await _userManager.CreateAsync(user, registrationRequest.Password);
            if (result.Succeeded)
            {
                var emailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var response = _mapper.Map<UserRegistrationRequestDTO>(user);
                var answer = new UserResponseDto
                {
                    Id = user.Id,
                    Token = emailToken,
                    FullName = $"{user.FirstName + " " + user.LastName}",
                    Email = user.Email,
                };
                return answer;

            }
            string errors = result.Errors.Aggregate(string.Empty, (current, error) => current + (error.Description + Environment.NewLine));
            throw new ArgumentException(errors);
        }
    }
}
