using AquaWater.BusinessLogic.Services.Implementations;
using AquaWater.Data.Repository.Interfaces;
using AquaWater.Dto.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Threading.Tasks;

namespace AquaWater.Api.Controllers
{
    [Route("api/v1/[Controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthenticationServices _authenticationServices;

        public AccountController(IAuthenticationServices authenticationServices)
        {
            _authenticationServices = authenticationServices;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginAsync([FromBody] UserRequestDTO userRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _authenticationServices.LoginAsync(userRequest);
                if (!result.Success)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }


            catch (AccessViolationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured we are working on it");
            }
        }

        [HttpPost]
        [Route("confirm-email")]
        public async Task<IActionResult> ConfirmEmailAsync([FromBody] ConfirmEmailRequestDTO confirmEmailRequestDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _authenticationServices.EmailConfirmationAsync(confirmEmailRequestDTO);
                if (!result.Success)
                {
                    return BadRequest(result);
                }
                return Ok(result);

            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured we are working on it");
            }
        }

        [HttpPatch("UpdatePassword")]
        public async Task<IActionResult> UpdatePassword(UpdatePasswordDTO updatePasswordDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _authenticationServices.UpdatePassword(updatePasswordDTO);

                if (!result.Success)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured we are working on it");
            }
        }

        [HttpPost("Reset-Password")]

        public async Task<IActionResult> ResetPasswordAsync(ResetPasswordDTO resetPassword)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _authenticationServices.ResetPasswordAsync(resetPassword);
                if (!result.Success)
                {
                    return BadRequest(result);

                }
                return Ok(result);
            }
            catch (ArgumentNullException argex)
            {
                return BadRequest(argex.Message);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred we are working on it");
            }
        }

        [HttpPost("Forgot-Password")]

        public async Task<IActionResult> ForgotPasswordAsync(string email)
        {
            try
            {
                if (string.IsNullOrEmpty(email))
                {
                    return NotFound();
                }
                var result = await _authenticationServices.ForgotPasswordAsync(email);
                if (result.Success)
                {
                    return Ok(result);
                }
            }
            catch (ArgumentNullException argex)
            {
                return BadRequest(argex.Message);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex.Message);
            }
            return BadRequest(400);
        }
    }
}