using AquaWater.Domain.Commons;
using AquaWater.Dto.Request;
using AquaWater.Dto.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AquaWater.BusinessLogic.Services.Interfaces
{
    public interface IProductService
    {
        Task<SearchResponse<IEnumerable<ProductDTO>>> GetProductsByCompanyID(SearchRequest<ProductSearchDTO> search);
        Task RemoveFavouriteProduct(Guid productId, Guid custommerId);
        Task<Response<ProductResponseDTO>> GetProductByIdAsync(string productId);
        Task<Response<string>> AddFavoriteProductAsync(string productId, string userId);
    }
}
