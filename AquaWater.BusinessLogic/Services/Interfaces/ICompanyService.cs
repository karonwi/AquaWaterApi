using AquaWater.Domain.Commons;
using AquaWater.Dto.Request;
using AquaWater.Dto.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaWater.BusinessLogic.Services.Interfaces
{
    public interface ICompanyService
    {
        SearchResponse<IEnumerable<CompanyResponseDTO>> GetAllCompaniesWithFeaturedProduct(SearchRequest<CompanySearchDTO> search);
    }
}
