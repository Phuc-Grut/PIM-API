using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.Domain.PIM.Models
{
    public class SP_GET_PRODUCT_BY_STOREID
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal? Price { get; set; }
        public Guid? UnitId { get; set; }
        public string Currency { get; set; }
    }
}
