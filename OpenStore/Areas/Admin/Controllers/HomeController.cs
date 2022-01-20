using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenStore.Areas.Admin.Models;
using OpenStore.Data;
using OpenStore.Models;

namespace OpenStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "مدير,موظف,متجر")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public HomeController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Check()
        {
            if (User.IsInRole("متجر"))
            {
                return RedirectToAction("Index", "Product", new { area = "Admin" });
            }

            else
            {
                return RedirectToAction("Index", "Home", new { area = "Admin" });

            }
        }
        public IActionResult Index(string Date)
        {

            var model = new HomeVm
            {
                Sales = _context.OrderProducts.Where(i => i.Order.State == State.Delivered).ToList().Count,
                UnCompletedOrder = _context.Orders.Where(i => i.State != State.Delivered).ToList().Count,
                CompletedOrder = _context.Orders.Where(i => i.State == State.Delivered).ToList().Count,
                Inquire = _context.Notifications.Where(i => i.Type == OpenStore.Models.Type.استفسار).ToList().Count,
                Saler = _userManager.GetUsersInRoleAsync("متجر").Result.Count,
                Customer = _userManager.GetUsersInRoleAsync("مشتري").Result.Count,
                Orders = _context.Orders.Select(o => new OrderVm
                {
                    ID = o.ID,
                    SallerName = o.User.FullName,
                    Cost = o.Cost,
                    Date = o.Date,
                }).Take(4).OrderByDescending(i => i.Date).ToList(),
                Notifications = _context.Notifications.Take(4).Where(i => i.Type == OpenStore.Models.Type.استفسار).OrderByDescending(i => i.Date).ToList(),
                Users = _userManager.Users.Select(u => new UserVm
                {
                    ID = u.Id,
                    FullName = u.FullName,
                    Date = u.Date,
                    roles = _userManager.GetRolesAsync(u).Result,
                    UserName = u.UserName,
                }).Take(4).ToList().OrderByDescending(i => i.Date).ToList(),
            };

            var day = DateTime.Now.Date.AddDays(-1);
            var week = DateTime.Now.Date.AddDays(-7);
            var month = DateTime.Now.Date.AddMonths(-1).AddDays(-DateTime.Now.Day);

            switch (Date)
            {
                case "day":
                    model.Orders = _context.Orders.Select(o => new OrderVm
                    {
                        ID = o.ID,
                        SallerName = o.User.FullName,
                        Cost = o.Cost,
                        Date = o.Date,
                    }).Take(4).Where(i => i.Date.Date <= DateTime.Now.Date && i.Date.Date >= day).OrderByDescending(i => i.Date).ToList();

                    model.Notifications = _context.Notifications.Take(4).Where(i => i.Type == OpenStore.Models.Type.استفسار).Where(i => i.Date.Date <= DateTime.Now.Date && i.Date.Date >= day).OrderByDescending(i => i.Date).ToList();

                    model.Users = _userManager.Users.Select(u => new UserVm
                    {
                        ID = u.Id,
                        FullName = u.FullName,
                        Date = u.Date,
                        roles = _userManager.GetRolesAsync(u).Result,
                    }).Take(4).ToList().OrderByDescending(i => i.Date).Where(i => i.Date.Value.Date <= DateTime.Now.Date && i.Date.Value.Date >= day).ToList();
                    break;

                case "week":
                    model.Orders = _context.Orders.Select(o => new OrderVm
                    {
                        ID = o.ID,
                        SallerName = o.User.FullName,
                        Cost = o.Cost,
                        Date = o.Date,
                    }).Take(4).Where(i => i.Date.Date <= DateTime.Now.Date && i.Date.Date >= week).OrderByDescending(i => i.Date).ToList();

                    model.Notifications = _context.Notifications.Take(4).Where(i => i.Type == OpenStore.Models.Type.استفسار).Where(i => i.Date.Date <= DateTime.Now.Date && i.Date.Date >= week).OrderByDescending(i => i.Date).ToList();

                    model.Users = _userManager.Users.Select(u => new UserVm
                    {
                        ID = u.Id,
                        FullName = u.FullName,
                        Date = u.Date,
                        roles = _userManager.GetRolesAsync(u).Result,
                    }).Take(4).ToList().OrderByDescending(i => i.Date).Where(i => i.Date.Value.Date <= DateTime.Now.Date && i.Date.Value.Date >= week).ToList();
                    break;

                case "month":
                    model.Orders = _context.Orders.Select(o => new OrderVm
                    {
                        ID = o.ID,
                        SallerName = o.User.FullName,
                        Cost = o.Cost,
                        Date = o.Date,
                    }).Take(4).Where(i => i.Date.Date <= DateTime.Now.Date && i.Date.Date >= month).OrderByDescending(i => i.Date).ToList();

                    model.Notifications = _context.Notifications.Take(4).Where(i => i.Type == OpenStore.Models.Type.استفسار).Where(i => i.Date.Date <= DateTime.Now.Date && i.Date.Date >= month).OrderByDescending(i => i.Date).ToList();

                    model.Users = _userManager.Users.Select(u => new UserVm
                    {
                        ID = u.Id,
                        FullName = u.FullName,
                        Date = u.Date,
                        roles = _userManager.GetRolesAsync(u).Result,
                    }).Take(4).ToList().OrderByDescending(i => i.Date).Where(i => i.Date.Value.Date <= DateTime.Now.Date && i.Date.Value.Date >= month).ToList();
                    break;
                default:
                    break;
            }

            return View(model);


        }
    }
}