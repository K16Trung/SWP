using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Entity
{
    public class Payment: BaseEntity
    {
        public int OrderId { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
        public Order Order { get; set; }
    }
}
