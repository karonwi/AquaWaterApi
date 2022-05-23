﻿using AquaWater.BusinessLogic.Services.Interfaces;
using AquaWater.Dto.Request;
using Microsoft.AspNetCore.Authorization;
using AquaWater.Dto.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Threading.Tasks;
using System.Security.Claims;

namespace AquaWater.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "RequireCustomerOnly")]
    [Authorize(Policy = "RequireCustomerOnly")]

    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        [Route("VerifyOTP")]
        [Authorize(Policy = "RequireCompanyManagerOnly")]
        public async Task<IActionResult> VerifyOTP([FromBody] VerifyOtpDTO verifyOtp)
        {
            try
            {
                if (!TryValidateModel(verifyOtp))
                {
                    return BadRequest();
                }

                var result = await _orderService.VerifyOTP(verifyOtp);
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
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured we are working on it");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrderAsync(OrderRequest createOrderRequest)
        {
            var userId = HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;

            var result = await _orderService.CreateOrderAsync(createOrderRequest, userId);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetOrderByIdAsync(string id)
        {
            var result = await _orderService.GetOrderByIdAsync(id);
            return Ok(result);
        }
    }
}
