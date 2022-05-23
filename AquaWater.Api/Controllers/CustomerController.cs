﻿using AquaWater.BusinessLogic.Services.Interfaces;
using AquaWater.Data.Services.Interfaces;
using AquaWater.Dto.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AquaWater.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[Controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly IProductService _productService;
        public CustomerController(ICustomerService customerService, IProductService productService)
        {
            _customerService = customerService;
            _productService = productService;
        }

        [HttpPost]
        public async Task<ActionResult> CreateUserAsync(CustomerRequestDTO customerRequest)
        {
            try
            {
                if (!TryValidateModel(customerRequest))
                {
                    return BadRequest();
                }
                await _customerService.CreateCustomer(customerRequest);
                return Ok();

            }
            catch (ArgumentException argex)
            {
                return BadRequest(argex.Message);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex.Message);
                return StatusCode(500, "An error occured we are working on it");
            }
        }

        [HttpDelete("RemoveFavouriteProduct")]
        public async Task<ActionResult> RemoveFavouriteProduct(Guid productId)
        {
            try
            {
                var userId = HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;
                await _productService.RemoveFavouriteProduct(Guid.Parse(userId), productId);
                return Ok();
            }
            catch (ArgumentException argex)
            {
                return BadRequest(argex.Message);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex.Message);
                return StatusCode(500, "An error occured we are working on it");
            }

        }
    }
}

