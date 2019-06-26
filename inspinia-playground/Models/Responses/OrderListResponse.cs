using System.Collections.Generic;

namespace inspinia.Models
{
    public class OrderListResponse
    {
        public IEnumerable<OrderItemResponse> Orders { get; set; }

        public int Total { get; set; }

        public int Page { get; set; }

        public int PageSize { get; set; }
    }
}
