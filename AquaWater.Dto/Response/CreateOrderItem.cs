using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaWater.Dto.Response
{
    public class CreateOrderItem
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
