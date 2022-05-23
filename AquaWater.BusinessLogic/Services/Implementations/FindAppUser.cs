using AquaWater.BusinessLogic.Services.Interfaces;
using AquaWater.Data.Repository;
using AquaWater.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaWater.BusinessLogic.Services.Implementations
{
    public class ApplicationUser : IApplicationUser
    {
        private readonly IGenericRepo<Customer> _customerGenericRepo;
        private readonly IGenericRepo<AdminUser> _adminGenericRepo;
        private readonly IGenericRepo<CompanyManager> _companyMangerGenericRepo;

        public ApplicationUser(IGenericRepo<Customer> customerGenericRepo, IGenericRepo<AdminUser> adminGenericRepo, 
            IGenericRepo<CompanyManager> companyMangerGenericRepo)
        {
            _customerGenericRepo = customerGenericRepo;
            _adminGenericRepo = adminGenericRepo;
            _companyMangerGenericRepo = companyMangerGenericRepo;
        }

        public async Task<Customer> GetCustomerIdByUserId(string id)
        {
          var customers= await  _customerGenericRepo.GetAllAsync();
           var customer = customers.FirstOrDefault(x => x.UserId.ToString() == id);
            return customer;
        }

        public async Task<AdminUser> GetAdminIdByUserId(string id)
        {
            var adminUsers= await _adminGenericRepo.GetAllAsync();
            var adminUser = adminUsers.FirstOrDefault(x => x.UserId.ToString() == id);
            return adminUser;
        }

        public async Task<CompanyManager> GetCompanyManagerIdByUserId(string id)
        {
            var companyManagers= await _companyMangerGenericRepo.GetAllAsync();
            var companyManager = companyManagers.FirstOrDefault(x => x.UserId.ToString() == id);
            return companyManager;
        }
    }
}
