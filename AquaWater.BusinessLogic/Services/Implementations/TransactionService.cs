using AquaWater.BusinessLogic.Services.Interfaces;
using AquaWater.Data.Context;
using AquaWater.Data.Repository;
using AquaWater.Dto.Response;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace AquaWater.BusinessLogic.Services.Implementations
{
    public class TransactionService : ITransactionService
    {
        private readonly IGenericRepo<Transaction> _transactionRepository;

        private readonly AppDbContext _dbContext;
        public TransactionService(IGenericRepo<Transaction> transanctionRepository, AppDbContext appDbContext)
        {
            _transactionRepository = transanctionRepository;
            _dbContext = appDbContext;
        }

        public async Task<Response<string>> DeleteAllTransactionsAsync(string userId)
        {
            var customer = _dbContext.Customers.FirstOrDefault(x => x.UserId == userId);
            if (customer != null)
            {
                throw new ArgumentException("User not exist");
            }
            var transactions = _dbContext.Orders.Where(x => x.CustomerId == customer.Id).Include(y => y.Transaction).Select(x => x.Transaction.Id);

            foreach (var item in transactions)
            {
                await _transactionRepository.DeleteAsync(item.ToString());
            }
            return new Response<string>()
            {
                Message = "Your transactions have been successfully deleted",
                Success = true
            };
        }

        public async Task<Response<string>> DeleteTransactionByIdAsync(string transactionId)
        {

            var transaction = _dbContext.Transactions.Where(d => d.Id == Guid.Parse(transactionId)).Include(t => t.Orders).FirstOrDefault();

            {
                if (transaction.Orders.CustomerId == Guid.Parse(transactionId))
                {
                    await _transactionRepository.DeleteAsync(transactionId);
                    return new Response<string>()
                    {
                        Message = $"Your transaction with {transactionId} has been successfully deleted",
                        Success = true
                    };
                }
            }
            throw new ArgumentException($"Transction with {transactionId} not found");
        }

    }
}
