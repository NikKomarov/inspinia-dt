using System;

namespace Data
{
    public class Order
    {
        public long Id { get; set; }

        public string Client { get; set; }

        public string Status { get; set; }

        public string Email { get; set; }

        public decimal Sum { get; set; }
    }
}
