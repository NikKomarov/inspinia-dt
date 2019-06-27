using Data;
using inspinia.Models;
using inspinia.Models.Datatables;
using inspinia.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var predicate = GetPredicate(request);

            var entities = Sort(_data.Orders.Where(predicate), request)
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

        [NonAction]
        private Expression<Func<Data.Order, bool>> GetPredicate(DataTableAjaxPostModel request)
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

            return predicate;
        }

        [NonAction]
        private IEnumerable<Data.Order> Sort(IEnumerable<Data.Order> collection, DataTableAjaxPostModel request)
        {
            if (request.order.Any())
            {
                var order = request.order.First();
                var func = GetSortingFunc(order.column);
                switch (order.dir)
                {
                    case "asc": return collection.OrderBy(func);
                    case "desc": return collection.OrderByDescending(func);
                    default:
                        break; 
                }
               
            }

            return collection;
        }

        [NonAction]
        private Func<Data.Order, object> GetSortingFunc(int column)
        {
            switch (column)
            {
                case 0: return order => order.Id;
                case 1: return order => order.Status;
                case 2: return order => order.Client;
                case 3: return order => order.Email;
                case 4: return order => order.Sum;
                default: throw new ArgumentException(nameof(column));
            }
        }
    }
}
