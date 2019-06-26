using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace inspinia.Models
{
    public class OrderItemResponse
    {
        public long Id { get; set; }

        public string Client { get; set; }

        public string Status { get; set; }

        public string Email { get; set; }

        public decimal Sum { get; set; }
    }
}
