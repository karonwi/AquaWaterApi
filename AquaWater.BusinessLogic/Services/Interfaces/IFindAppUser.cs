using AquaWater.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaWater.BusinessLogic.Services.Interfaces
{
    public interface IApplicationUser
    {
        Task<Customer> GetCustomerIdByUserId(string id);
        Task<AdminUser> GetAdminIdByUserId(string id);
        Task<CompanyManager> GetCompanyManagerIdByUserId(string id);
    }
}
