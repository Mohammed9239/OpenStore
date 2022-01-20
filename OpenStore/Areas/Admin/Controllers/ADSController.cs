using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenStore.Data;
using OpenStore.Models;
using PagedList.Core;

namespace OpenStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="مدير,موظف")]
    public class ADSController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ADSController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int? select, string? search, int? page)
        {
            
            var Ads = _context.ADS.Select(b => new ADS
            {
                ID = b.ID,
                Name = b.Name,
                Page = b.Page,
                Section = b.Section,
                Price = b.Price,
                Photo = b.Photo
            });

            if (select.HasValue && search != null)
            {
                switch (select)
                {
                    case 1:
                        Ads = Ads.Where(i => i.Name.Contains(search));
                        break;
                    case 2:
                        Ads = Ads.Where(i => i.Price.ToString().Contains(search));
                        break;

                }
            }

            return View(Ads.ToPagedList(page ?? 1, 20));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aDS = await _context.ADS
                .FirstOrDefaultAsync(m => m.ID == id);
            if (aDS == null)
            {
                return NotFound();
            }

            return View(aDS);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ADS aDS, IFormFile Img)
        {
            if (ModelState.IsValid)
            {
                aDS.Photo = ImgAds(Img);
                _context.Add(aDS);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(aDS);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aDS = await _context.ADS.FindAsync(id);
            if (aDS == null)
            {
                return NotFound();
            }
            return View(aDS);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ADS aDS, IFormFile Img)
        {
            if (id != aDS.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // To Remove Old Img
                if (Img != null)
                {
                    string oldpath = string.Format("{0}/wwwroot/Uploads/ADS/{1}",
                    Directory.GetCurrentDirectory(), aDS.Photo);
                    if (aDS.Photo != null)
                    {
                        System.IO.File.Delete(oldpath);
                    }
                    aDS.Photo = ImgAds(Img);
                }

                _context.Update(aDS);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(aDS);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var aDS = await _context.ADS.FindAsync(id);
            // Remove Img
            string oldpath = string.Format("{0}/wwwroot/Uploads/ADS/{1}",
            Directory.GetCurrentDirectory(), aDS.Photo);
            if (aDS.Photo != null)
            {
                System.IO.File.Delete(oldpath);
            }

            _context.ADS.Remove(aDS);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        // Add Img
        public string ImgAds(IFormFile fav)
        {
            string filename = "";
            if (Request.Form.Files.Count > 0 && Request.Form.Files[0].Length > 0)
            {
                if (Request.Form.Files.Count > 0 && Request.Form.Files[0].Length > 0)
                {
                    var file = fav;
                    string[] str = file.FileName.Split('.');
                    string ext = str[str.Length - 1];
                    filename = string.Format("{0}.{1}", Guid.NewGuid().ToString(), ext);
                    string path = string.Format("{0}/wwwroot/Uploads/ADS/{1}",
                        Directory.GetCurrentDirectory(), filename);
                    using (var streem = new FileStream(path, FileMode.Create))
                    {
                        file.CopyTo(streem);
                    }
                }
            }
            return filename;
        }

    }
}