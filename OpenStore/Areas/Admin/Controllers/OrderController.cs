using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OpenStore.Areas.Admin.Models;
using OpenStore.Data;
using OpenStore.Models;
using PagedList.Core;

namespace OpenStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "مدير,موظف")]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public OrderController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index(int? select, string? search, int? page)
        {
            var Order = _context.Orders.Select(o => new OrderVm
            {
                ID = o.ID,
                SallerName = o.User.FullName,
                Cost = o.Cost,
                State = o.State,
                Date = o.Date,
                BankNum = o.BankNum,
                City = o.Province.City.Name,
                Province = o.Province.Name,
            });

            if (select.HasValue && search != null)
            {
                switch (select)
                {
                    case 1:
                        Order = Order.Where(i => i.SallerName.Contains(search));
                        break;
                    case 2:
                        Order = Order.Where(i => i.Cost.ToString().Contains(search));
                        break;
                    case 3:
                        Order = Order.Where(i => i.State.ToString().Contains(search));
                        break;
                    case 4:
                        Order = Order.Where(i => i.BankNum.ToString().Contains(search));
                        break;
                    case 5:
                        Order = Order.Where(i => i.City.Contains(search));
                        break;
                    case 6:
                        Order = Order.Where(i => i.Province.Contains(search));
                        break;
                }
            }

            return View(Order.OrderByDescending(i => i.Date).ToPagedList(page ?? 1, 20));
        }

        public IActionResult Completed(int? select, string? search, int? page)
        {
            var Order = _context.Orders.Select(o => new OrderVm
            {
                ID = o.ID,
                SallerName = o.User.FullName,
                Cost = o.Cost,
                State = o.State,
                Date = o.Date,
                BankNum = o.BankNum,
                City = o.Province.City.Name,
                Province = o.Province.Name,
            }).Where(i=>i.State == State.Delivered);

            if (select.HasValue && search != null)
            {
                switch (select)
                {
                    case 1:
                        Order = Order.Where(i => i.SallerName.Contains(search));
                        break;
                    case 2:
                        Order = Order.Where(i => i.Cost.ToString().Contains(search));
                        break;
                    case 3:
                        Order = Order.Where(i => i.State.ToString().Contains(search));
                        break;
                    case 4:
                        Order = Order.Where(i => i.BankNum.ToString().Contains(search));
                        break;
                    case 5:
                        Order = Order.Where(i => i.City.Contains(search));
                        break;
                    case 6:
                        Order = Order.Where(i => i.Province.Contains(search));
                        break;
                }
            }

            return View(Order.OrderByDescending(i => i.Date).ToPagedList(page ?? 1, 20));
        }

        public IActionResult UnCompleted(int? select, string? search, int? page)
        {
            var Order = _context.Orders.Select(o => new OrderVm
            {
                ID = o.ID,
                SallerName = o.User.FullName,
                Cost = o.Cost,
                State = o.State,
                Date = o.Date,
                BankNum = o.BankNum,
                City = o.Province.City.Name,
                Province = o.Province.Name,
            }).Where(i => i.State != State.Delivered);

            if (select.HasValue && search != null)
            {
                switch (select)
                {
                    case 1:
                        Order = Order.Where(i => i.SallerName.Contains(search));
                        break;
                    case 2:
                        Order = Order.Where(i => i.Cost.ToString().Contains(search));
                        break;
                    case 3:
                        Order = Order.Where(i => i.State.ToString().Contains(search));
                        break;
                    case 4:
                        Order = Order.Where(i => i.BankNum.ToString().Contains(search));
                        break;
                    case 5:
                        Order = Order.Where(i => i.City.Contains(search));
                        break;
                    case 6:
                        Order = Order.Where(i => i.Province.Contains(search));
                        break;
                }
            }

            return View(Order.OrderByDescending(i => i.Date).ToPagedList(page ?? 1, 20)); ;
        }

        public async Task<IActionResult> Details(int id)
        {
            var Order = await _context.Orders.Select(o => new OrderVm
            {
                ID = o.ID,
                SallerName = o.User.FullName,
                Cost = o.Cost,
                State = o.State,
                Date = o.Date,
                BankNum = o.BankNum,
                City = o.Province.City.Name,
                Province = o.Province.Name,
                Street = o.Street,
                Phone = o.User.Phones.ToList(),
                BankImg = o.BankImg,
                OrderProducts =  _context.OrderProducts.Where(i => i.OrderID == o.ID).Include(p => p.Product).ThenInclude(i=>i.GetCategoryLev2).ThenInclude(i=>i.CategoryLev1),
            }).FirstOrDefaultAsync(i => i.ID == id);

            return View(Order);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var Order = await _context.Orders.Select(o => new OrderVm
            {
                ID = o.ID,
                SallerName = o.User.FullName,
                BankNum = o.BankNum,
                CityID = o.Province.CityID,
                ProvinceID = o.Province.ID,
                Street = o.Street,
                State = o.State,
                Cost = o.Cost,
                OrderProducts = _context.OrderProducts.Where(i => i.OrderID == o.ID).Include(p => p.Product).ThenInclude(i => i.GetCategoryLev2).ThenInclude(i => i.CategoryLev1),
            }).FirstOrDefaultAsync(i => i.ID == id);

            ViewData["ProvinceID"] = new SelectList(_context.Provinces, "ID", "Name");
            ViewData["CityID"] = new SelectList(_context.Cities, "ID", "Name");

            return View(Order);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, OrderVm orderVm, List<int> IdO, List<int> Qun)
        {
            var Order = await _context.Orders.Select(o => new OrderVm
            {
                ID = o.ID,
                SallerName = o.User.FullName,
                BankNum = o.BankNum,
                Street = o.Street,
                OrderProducts = _context.OrderProducts.Where(i => i.OrderID == o.ID).Include(p => p.Product).ThenInclude(i => i.GetCategoryLev2).ThenInclude(i => i.CategoryLev1),
            }).FirstOrDefaultAsync(i => i.ID == id);


            ViewData["ProvinceID"] = new SelectList(_context.Provinces, "ID", "Name");
            ViewData["CityID"] = new SelectList(_context.Cities, "ID", "Name");

            if (ModelState.IsValid)
            {
                var order = await _context.Orders.FirstOrDefaultAsync(i => i.ID == Order.ID);
                order.BankNum = orderVm.BankNum;
                order.ProvinceID = orderVm.ProvinceID;
                order.State = orderVm.State;

                _context.Orders.Update(order);
                await _context.SaveChangesAsync();

                for (int i = 0; i < IdO.Count; i++)
                {
                    var OrderProduct = await _context.OrderProducts.FindAsync(IdO[i]);
                    OrderProduct.Quantity = Qun[i];
                    _context.OrderProducts.Update(OrderProduct);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));

            }


            return View(Order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeleteOrderProduct(int id)
        {

            var orderProduct = await _context.OrderProducts.FindAsync(id);
            _context.OrderProducts.Remove(orderProduct);
            await _context.SaveChangesAsync();

            return Ok();

        }

    }
}