using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenStore.Data;
using OpenStore.Models;
using PagedList.Core;

namespace OpenStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "مدير,موظف")]
    public class NotificationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public NotificationController(ApplicationDbContext context,UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int? select, string? search, int? page)
        {
            var Notifications =  _context.Notifications
                .Include(i=>i.Receiver)
                .Where(i=>i.Type== OpenStore.Models.Type.اشعار).Select(b => new Notification
                {
                    ID = b.ID,
                    Title = b.Title,
                    Type = b.Type,
                    Receiver = b.Receiver,
                    Date = b.Date,
                });

            if (select.HasValue && search != null)
            {
                switch (select)
                {
                    case 1:
                        Notifications = Notifications.Where(i => i.Title.Contains(search));
                        break;
                    case 2:
                        Notifications = Notifications.Where(i => i.Receiver.FullName.Contains(search));
                        break;
                    case 3:
                        Notifications = Notifications.Where(i => i.Date.ToString().Contains(search));
                        break;
                }
            }

            return View(Notifications.OrderByDescending(i => i.Date).ToPagedList(page ?? 1, 20));
        }

        public async Task<IActionResult> Details(int? id)
        {
            var Notification = await _context.Notifications.Include(i => i.Receiver)
                .FirstOrDefaultAsync(i => i.ID == id);

            return View(Notification);
        }

        public IActionResult Create()
        {
            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Notification notification)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.GetUserAsync(User);
                var Notification = new Notification
                {
                    ReceiverID = user.Id,
                    Title = notification.Title,
                    Message = notification.Message,
                    State = false,
                    Type = OpenStore.Models.Type.اشعار,
                    Date = DateTime.Now
                };

                _context.Add(Notification);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(notification);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notification = await _context.Notifications.FindAsync(id);
            if (notification == null)
            {
                return NotFound();
            }
            return View(notification);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Notification notification)
        {
            if (id != notification.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(notification);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(notification);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}