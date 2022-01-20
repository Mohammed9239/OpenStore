using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OpenStore.Data;
using OpenStore.Models;

namespace OpenStore.Areas.Admin.Controllers
{
    [Area("ADMIN")]
    [Authorize(Roles = "مدير,موظف")]
    public class LocationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LocationController(ApplicationDbContext context )
        {
            _context = context;
        }
        public IActionResult Index()
        {
            ViewData["CityID"] = new SelectList(_context.Cities, "ID", "Name");
            return View();
        }

        public IActionResult AddCity()
        {
            ViewData["CityID"] = new SelectList(_context.Cities, "ID", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCity(City city)
        {
            if (ModelState.IsValid)
            {
                _context.Add(city);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(city);
        }

        public async Task<IActionResult> EditCity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var city = await _context.Cities.FindAsync(id);
            if (city == null)
            {
                return NotFound();
            }
            return View(city);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCity(int id, City city)
        {
            if (id != city.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(city);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(city);
        }


        public IActionResult AddProvince(int? id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProvince(int id, Province province)
        {
            if (ModelState.IsValid)
            {
                var p = new Province
                {
                    CityID = id,
                    Name = province.Name,
                };
                _context.Add(p);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(province);
        }

        public async Task<IActionResult> EditProvince(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var province = await _context.Provinces.FindAsync(id);
            if (province == null)
            {
                return NotFound();
            }
            ViewData["CityID"] = new SelectList(_context.Cities, "ID", "ID", province.CityID);
            return View(province);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProvince(int id, Province province)
        {
            if (id != province.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(province);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CityID"] = new SelectList(_context.Cities, "ID", "ID", province.CityID);
            return View(province);
        }


        public async Task<IActionResult> GetP(int? id)
        {
            var p = await _context.Provinces.Where(i => i.CityID == id).ToListAsync();
            return Json(p);
        }
    }
}