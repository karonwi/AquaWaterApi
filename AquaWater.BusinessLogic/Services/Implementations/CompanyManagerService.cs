using AquaWater.Data.Repository.Interfaces;
using AquaWater.Data.Services.Implementations;
using AquaWater.Data.Services.Interfaces;
using AquaWater.Domain.Entities;
using AquaWater.Dto.Common;
using AquaWater.Dto.Response;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using System.Transactions;

namespace AquaWater.Data.Repository.Implementations
{
    public class CompanyManagerService : ICompanyManagerService
    {
        private readonly IGenericRepo<Company> _CompanyRepo;
        private readonly IConfirmationMailService _confirmationMailService;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IMailService _mailService;

        public CompanyManagerService(IGenericRepo<Company> genericRepo, IConfirmationMailService confirmationMailService,
            IMapper mapper, IUserService userService, IMailService mailService)
        {
            _CompanyRepo = genericRepo;
            _confirmationMailService = confirmationMailService;
            _mapper = mapper;
            _userService = userService;
            _mailService = mailService;
        }


        public async Task<Response<string>> CreateCompanyManagerAsyn(CompanyManagerRequestDTO companyManagerRequest)
        {
            var company = await _CompanyRepo.GetByIdAysnc(companyManagerRequest.CompanyId);
            if (company == null)
            {
                throw new ArgumentException($"Company with Id {companyManagerRequest.CompanyId} does not exist");
            }

            var response = new Response<string>();
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var userDetails = await _userService.RegisterAsync(companyManagerRequest.User);

                var template = _mailService.GetEmailTemplate("EmailTemplate.html");
                TextInfo textInfo = new CultureInfo("en-GB", false).TextInfo;
                var userName = textInfo.ToTitleCase(userDetails.FullName);

                await _confirmationMailService.SendAConfirmationEmail(userDetails);

                var companyManager = _mapper.Map<Company>(companyManagerRequest);

                await _CompanyRepo.InsertAsync(companyManager);

                transaction.Complete();
            }

            response.Success = true;
            response.Message = "Manager Registered Successful, Please comfirm email";
            return response;
        }
    }
}
