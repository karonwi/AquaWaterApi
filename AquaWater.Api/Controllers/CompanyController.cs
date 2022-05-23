using AquaWater.BusinessLogic.Services.Interfaces;
using AquaWater.Domain.Commons;
using AquaWater.Dto.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Threading.Tasks;

namespace AquaWater.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpGet("GetAllCompaniesWithFeaturedProduct")]
        public IActionResult GetAllCompaniesWithFeaturedProduct([FromQuery]SearchRequest<CompanySearchDTO> search)
        {
            try
            {
                if (!TryValidateModel(search))
                {
                    return BadRequest();
                }
                var result = _companyService.GetAllCompaniesWithFeaturedProduct(search);
                return Ok(result);
            }
            catch (ArgumentException argex)
            {
                return BadRequest(argex.Message);
            }
            catch(Exception ex)
            {

                Log.Logger.Error(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured we are working on it");
            }
        }
    }
}
