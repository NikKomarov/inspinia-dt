using System.ComponentModel.DataAnnotations;

namespace inspinia.Models.Requests
{
    public class GetOrdersRequest
    {
        [Range(1, int.MaxValue)]
        public int Page { get; set; }

        [Range(1, int.MaxValue)]
        public int PageSize { get; set; }
    }
}
