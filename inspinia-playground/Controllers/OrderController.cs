using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using inspinia_playground.Models;
using Data;
using System.ComponentModel.DataAnnotations;
using inspinia.Models.Requests;
using inspinia.Models;

namespace inspinia_playground.Controllers
{
    public class OrderController : Controller
    {
        SalesDbContext _data;

        public OrderController(SalesDbContext data)
        {
            _data = data;
        }

        public IActionResult Index()
        {
            var orders = _data.Orders.ToList();

            return View();
        }


        public IActionResult Get(GetOrdersRequest request)
        {
            var orders = _data.Orders
                .Skip(request.Page - 1)
                .Take(request.PageSize);

            var total = _data.Orders.Count();

            var response = new OrderListResponse
            {
                //TODO: поставить автомаппер
                Orders = orders.Select(m => new OrderItemResponse
                {
                    Id = m.Id,
                    Client = m.Client,
                    Email = m.Email,
                    Status = m.Status,
                    Sum = m.Sum
                }),
                Page = request.Page,
                PageSize = request.PageSize,
                Total = total
            };

            return Json(response);
        }
    }
}
