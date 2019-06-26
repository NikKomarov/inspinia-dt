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
using inspinia.Models.Datatables;
using System.Linq.Expressions;

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

        [HttpPost]
        public IActionResult GetDatatable(DataTableAjaxPostModel request)
        {
            Expression<Func<Data.Order, bool>> predicate = order => true;

            if (!string.IsNullOrWhiteSpace(request.search?.value))
            {
                var search = request.search.value.ToLower();

                predicate = order => order.Client.ToLower().Contains(search)
                    || order.Email.ToLower().Contains(search)
                    || order.Status.ToLower().Contains(search)
                    //TODO: придумать что-то умнее
                    || order.Sum.ToString() == search;
            }
            var entities = _data.Orders
                .Where(predicate)
                .Skip(request.start)
                .Take(request.length);

            var total = _data.Orders.Count();
            var totalFilter = _data.Orders.Where(predicate).Count();

            var orders = entities.Select(m => new OrderItemResponse
                {
                    Id = m.Id,
                    Client = m.Client,
                    Email = m.Email,
                    Status = m.Status,
                    Sum = m.Sum
                });

            return Json(new
            {
                draw = request.draw,
                recordsTotal = total,
                recordsFiltered = totalFilter,
                data = orders
            });
        }
    }
}
