using AquaWater.Dto.Response;
using System.Threading.Tasks;

namespace AquaWater.BusinessLogic.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<Response<string>> DeleteAllTransactionsAsync(string userId);
        Task<Response<string>> DeleteTransactionByIdAsync(string transactionId);
    }
}