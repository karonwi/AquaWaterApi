using AquaWater.Dto.Request;
using AquaWater.Dto.Response;
using System.Threading.Tasks;

namespace AquaWater.Data.Services.Interfaces
{
    public interface ICustomerService
    {
        public Task<Response<string>> CreateCustomer(CustomerRequestDTO customerRequestDTO);
    }
}
