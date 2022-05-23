using AquaWater.Data.Repository.Interfaces;
using AquaWater.Dto.Common;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Threading.Tasks;

namespace AquaWater.Api.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class CompanyManagerController : ControllerBase
    {
        private readonly ICompanyManagerService _companyMgRepo;

        public CompanyManagerController(ICompanyManagerService companyMgRepo)
        {
            _companyMgRepo = companyMgRepo;
        }

        [HttpPost]
        public async Task<ActionResult> AddUserAsync(CompanyManagerRequestDTO registrationrequestDTO)
        {
            try
            {
                if (!TryValidateModel(registrationrequestDTO))
                {
                    return BadRequest();
                }

                var result = await _companyMgRepo.CreateCompanyManagerAsyn(registrationrequestDTO);

                return Ok(result);
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
