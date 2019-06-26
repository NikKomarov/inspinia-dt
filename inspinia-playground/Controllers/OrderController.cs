using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using inspinia_playground.Models;
using Data;

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
    }
}
