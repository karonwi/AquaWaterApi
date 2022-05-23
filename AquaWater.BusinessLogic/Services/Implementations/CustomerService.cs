using AquaWater.Data.Repository;
using AquaWater.Data.Repository.Interfaces;
using AquaWater.Data.Services.Interfaces;
using AquaWater.Domain.Entities;
using AquaWater.Dto.Request;
using AquaWater.Dto.Response;
using AutoMapper;
using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Transactions;

namespace AquaWater.Data.Services.Implementations
{

    public class CustomerService : ICustomerService
    {
        private readonly IGenericRepo<Customer> _CustomerRepo;
        private readonly IConfirmationMailService _confirmationMailService;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IMailService _mailService;

        public CustomerService(IGenericRepo<Customer> genericRepo, IConfirmationMailService confirmationMailService,
            IMapper mapper, IUserService userService, IMailService mailService)
        {
            _CustomerRepo = genericRepo;
            _confirmationMailService = confirmationMailService;
            _mapper = mapper;
            _userService = userService;
            _mailService = mailService;
        }

        public async Task<Response<string>> CreateCustomer(CustomerRequestDTO customerRequestDTO)
        {

            var response = new Response<string>();
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
               var userDetails = await _userService.RegisterAsync(customerRequestDTO.User);

                TextInfo textInfo = new CultureInfo("en-GB", false).TextInfo;
                var userName = textInfo.ToTitleCase(userDetails.FullName);

                await _confirmationMailService.SendAConfirmationEmail(userDetails);

                var Customer = _mapper.Map<Customer>(customerRequestDTO);
                Customer.UserId = userDetails.Id;

                await _CustomerRepo.InsertAsync(Customer);

                transaction.Complete();
            }


            response.Success = true;
            response.Message = "Customer Registered Successful, Please comfirm email";
            return response;
        }
    }
}
