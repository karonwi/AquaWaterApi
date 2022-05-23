using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaWater.Domain.Commons
{
    public class SearchRequest<T>
    {
        public T Data { get; set; }
        public int PageSize { get; set; }
        public int Page { get; set; }
    }
}
