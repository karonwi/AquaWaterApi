using AquaWater.BusinessLogic.Services.Interfaces;
using AquaWater.BusinessLogic.Utilities;
using AquaWater.Data.Context;
using AquaWater.Data.Repository;
using AquaWater.Domain.Commons;
using AquaWater.Domain.Entities;
using AquaWater.Dto.Request;
using AquaWater.Dto.Response;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaWater.BusinessLogic.Services.Implementations
{
    public class CompanyService : ICompanyService
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepo<Company> _companyRepo;
        public CompanyService(IMapper mapper, IGenericRepo<Company> companyRepo)
        {
            _mapper = mapper;
            _companyRepo = companyRepo;
        }
        public SearchResponse<IEnumerable<CompanyResponseDTO>> GetAllCompaniesWithFeaturedProduct(SearchRequest<CompanySearchDTO> search)
        {
            if (search == null)
            {
                throw new ArgumentException($"Invalid search parameters");
            }
            var query = _companyRepo.TableNoTracking;

            if (search.Data != null)
            {
                if (string.IsNullOrWhiteSpace(search.Data.CompanyName))
                {
                    query = query.Where(x => x.CompanyName.Contains(search.Data.CompanyName));
                }
                if (string.IsNullOrWhiteSpace(search.Data.State))
                {
                    query = query.Where(x => x.Location.State.Contains(search.Data.State));
                }
                if (string.IsNullOrWhiteSpace(search.Data.Country))
                {
                    query.Where(x => x.Location.Country.Contains(search.Data.Country));
                }
            }

            var IsFeatured = query.Include(x => x.Product.Where(c => c.IsFeature)).ThenInclude(x => x.ProductGallery)
                .Include(x => x.Location)
                .OrderBy(x => x.Id);

            return Paginator.Pagination<Company, CompanyResponseDTO>(IsFeatured, _mapper, search.PageSize, search.Page);
        }
    }
}
