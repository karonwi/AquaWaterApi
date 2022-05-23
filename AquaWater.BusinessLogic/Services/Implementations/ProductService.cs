using AquaWater.BusinessLogic.Services.Interfaces;
using AquaWater.BusinessLogic.Utilities;
using AquaWater.Data.Repository;
using AquaWater.Domain.Entities;
using AquaWater.Dto.Response;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using AquaWater.Domain.Commons;
using AquaWater.Dto.Request;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace AquaWater.BusinessLogic.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepo<CustomerFavourite> _customerFavouriteRepo;
        private readonly IGenericRepo<Product> _productRepo;
        private readonly IGenericRepo<Customer> _customerRepo;
        private readonly IGenericRepo<Company> _companyRepo;
        public ProductService(IMapper mapper, IGenericRepo<Product> productRepo, IGenericRepo<CustomerFavourite> customerFavouriteRepo,IGenericRepo<Customer> customerRepo, IGenericRepo<Company> companyRepo)
        {
            _mapper = mapper;
            _productRepo = productRepo;
            _customerFavouriteRepo = customerFavouriteRepo;
            _customerRepo = customerRepo;
            _companyRepo = companyRepo;
            _companyRepo = companyRepo;
        }

        public async Task RemoveFavouriteProduct(Guid productId, Guid custommerId)
        {
            var customerFavourite = _customerFavouriteRepo.Table.FirstOrDefault(x => x.ProductId == productId && x.CustomerId == custommerId);

            if (customerFavourite == null)
            {
                return;
            }
            await _customerFavouriteRepo.DeleteAsync(customerFavourite.Id.ToString());
        }

        public async Task<SearchResponse<IEnumerable<ProductDTO>>> GetProductsByCompanyID(SearchRequest<ProductSearchDTO> search)
        {
            var checkCompany = await _companyRepo.GetByIdAysnc(search.Data.CompanyId);

            if (checkCompany == null)
            {
                throw new ArgumentException($"Company with {search.Data.CompanyId} does not exist");
            }

            var companyProducts = _productRepo.TableNoTracking.Where(x => x.CompanyId == search.Data.CompanyId).Include(x => x.Rating).Include(x => x.ProductGallery).OrderBy(x => x.Id);
            return Paginator.Pagination<Product, ProductDTO>(companyProducts, _mapper, search.PageSize, search.Page);
        }

        public async Task<Response<ProductResponseDTO>> GetProductByIdAsync(string productId)
        {
            var product = await _productRepo.TableNoTracking.Where(x => x.Id == Guid.Parse(productId))
                                                    .Include(x => x.Review)
                                                    .Include(x => x.ProductGallery)
                                                    .AsNoTracking().FirstOrDefaultAsync();
            if (product != null)
            {
                var response = new Response<ProductResponseDTO>
                {
                    Data = _mapper.Map<ProductResponseDTO>(product),
                    Success = true
                };
                return response;
            }

            throw new ArgumentException($"Product with {productId} Not Found");
        }

        public async Task<Response<string>> AddFavoriteProductAsync(string productId, string userId)
        {
            var customer = _customerRepo.Table.Where(x => x.UserId == userId).FirstOrDefault();

            if (customer != null)
            {
               await _customerFavouriteRepo.InsertAsync(new CustomerFavourite
                {
                    CustomerId = customer.Id,
                    ProductId = Guid.Parse(productId)

                });
               
                return new Response<string>()
                {
                    Message = $"The product with {productId} has been succesfully added to your favourite",
                    Success = true
                };
            }
            throw new ArgumentException("User does not exit");
        }
    }
}