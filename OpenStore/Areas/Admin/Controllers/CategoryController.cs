using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OpenStore.Data;
using OpenStore.Models;

namespace OpenStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "مدير,موظف")]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// All Opertion Of CatgeoryLev 1 ///
        public async Task<IActionResult> Index()
        {
            return View(await _context.CategoryLev1s.OrderBy(i=>i.Index).ToListAsync());
        }

        public IActionResult AddCategoryLev1()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddCategoryLev1(CategoryLev1 categoryLev1 , IFormFile CatImg)
        {
            if (ModelState.IsValid)
            {
               
                categoryLev1.ImgUrl = Img(CatImg);
                _context.Add(categoryLev1);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(categoryLev1);
        }

        // IS NOT USE 
        public async Task<IActionResult> EditCategoryLev1(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoryLev1 = await _context.CategoryLev1s.FindAsync(id);
            if (categoryLev1 == null)
            {
                return NotFound();
            }
            return View(categoryLev1);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCategoryLev1(int id, CategoryLev1 categoryLev1, IFormFile CatImg)
        {
            if (id != categoryLev1.ID)
            {
                return NotFound();
            }

            //update img
            if (CatImg != null)
            {
                //remove old img
                if (categoryLev1.ImgUrl != null)
                {
                    var oldimg = categoryLev1.ImgUrl;
                    string oldpath = string.Format("{0}/wwwroot/Uploads/CatImg/{1}",
                     Directory.GetCurrentDirectory(), oldimg);
                    if (oldimg != null)
                    {
                        System.IO.File.Delete(oldpath);
                    }
                }
                //add new img
                categoryLev1.ImgUrl = Img(CatImg);
            }

            if (ModelState.IsValid)
            {
                _context.Update(categoryLev1);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
                   
            return View("Index", categoryLev1);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCategoryLev1(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoryLev1 = await _context.CategoryLev1s.FindAsync(id);

            //remove old img
            var oldimg = categoryLev1.ImgUrl;
            string oldpath = string.Format("{0}/wwwroot/Uploads/CatImg/{1}",
             Directory.GetCurrentDirectory(), oldimg);
            if (oldimg != null)
            {
                System.IO.File.Delete(oldpath);
            }

            _context.CategoryLev1s.Remove(categoryLev1);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        /// End Opertion Of CatgeoryLev 1 ///



        /// All Opertion Of CatgeoryLev 2 ///
        public async Task<IActionResult> IndexCategoryLev2(int id)
        {
            ViewData["CategoryLev1ID"] = new SelectList(_context.CategoryLev1s, "ID", "Name",id);

            return View(await _context.CategoryLev2s.OrderBy(i=>i.Index).Where(i=>i.CategoryLev1ID==id).ToListAsync());
        }

        public IActionResult AddCategoryLev2(int id)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddCategoryLev2(int id ,CategoryLev2 categoryLev2, IFormFile CatImg)
        {
            if (ModelState.IsValid)
            {
                var cat = new CategoryLev2
                {
                    CategoryLev1ID = id,
                    Name = categoryLev2.Name,
                    ImgUrl = Img(CatImg),
                };
                categoryLev2.CategoryLev1ID = id;
                _context.Add(cat);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(IndexCategoryLev2),new { @id=id });
            }
            return View(categoryLev2);
        }

        // IS NOT USE 
        public async Task<IActionResult> EditCategoryLev2(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoryLev2 = await _context.CategoryLev2s.FindAsync(id);
            if (categoryLev2 == null)
            {
                return NotFound();
            }
            return View(categoryLev2);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCategoryLev2(int id, CategoryLev2 categoryLev2, IFormFile CatImg)
        {
            if (id != categoryLev2.ID)
            {
                return NotFound();
            }

            //update img
            if (CatImg != null)
            {
                //remove old img
                var oldimg = categoryLev2.ImgUrl;
                string oldpath = string.Format("{0}/wwwroot/Uploads/CatImg/{1}",
                 Directory.GetCurrentDirectory(), oldimg);
                if (oldimg != null)
                {
                    System.IO.File.Delete(oldpath);
                }

                //add new img
                categoryLev2.ImgUrl = Img(CatImg);
            }

            if (ModelState.IsValid)
            {
                _context.Update(categoryLev2);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(IndexCategoryLev2), new { @id = categoryLev2.CategoryLev1ID });
            }

            return View(categoryLev2);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCategoryLev2(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoryLev2 = await _context.CategoryLev2s.FindAsync(id);

            //remove old img
            var oldimg = categoryLev2.ImgUrl;
            string oldpath = string.Format("{0}/wwwroot/Uploads/CatImg/{1}",
             Directory.GetCurrentDirectory(), oldimg);
            if (oldimg != null)
            {
                System.IO.File.Delete(oldpath);
            }


            _context.CategoryLev2s.Remove(categoryLev2);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        /// End Opertion Of CatgeoryLev 2  ///



        /// All Opertion Of CatgeoryLev 3 ///
        public async Task<IActionResult> IndexCategoryLev3(int id)
        {
            ViewData["CategoryLev2ID"] = new SelectList(_context.CategoryLev2s, "ID", "Name", id);

            return View(await _context.CategoryLev3s.OrderBy(i => i.Index).Where(i => i.CategoryLev2ID == id).ToListAsync());

        }

        public IActionResult AddCategoryLev3(int id)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddCategoryLev3(int id, CategoryLev3 categoryLev3, IFormFile CatImg)
        {
            if (ModelState.IsValid)
            {
                var cat = new CategoryLev3
                {
                    CategoryLev2ID = id,
                    Name = categoryLev3.Name,
                    ImgUrl = Img(CatImg),
                };
                categoryLev3.CategoryLev2ID = id;
                _context.Add(cat);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(IndexCategoryLev3), new { @id = id });
            }
            return View(categoryLev3);
        }

        // IS NOT USE 
        public async Task<IActionResult> EditCategoryLev3(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoryLev3 = await _context.CategoryLev3s.FindAsync(id);
            if (categoryLev3 == null)
            {
                return NotFound();
            }
            return View(categoryLev3);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCategoryLev3(int id, CategoryLev3 categoryLev3, IFormFile CatImg)
        {
            if (id != categoryLev3.ID)
            {
                return NotFound();
            }

            //update img
            if (CatImg != null)
            {
                //remove old img
                var oldimg = categoryLev3.ImgUrl;
                string oldpath = string.Format("{0}/wwwroot/Uploads/CatImg/{1}",
                 Directory.GetCurrentDirectory(), oldimg);
                if (oldimg != null)
                {
                    System.IO.File.Delete(oldpath);
                }

                //add new img
                categoryLev3.ImgUrl = Img(CatImg);
            }

            if (ModelState.IsValid)
            {
                _context.Update(categoryLev3);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(IndexCategoryLev3), new { @id = categoryLev3.CategoryLev2ID });
            }

            return View(categoryLev3);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCategoryLev3(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoryLev3 = await _context.CategoryLev3s.FindAsync(id);

            //remove old img
            var oldimg = categoryLev3.ImgUrl;
            string oldpath = string.Format("{0}/wwwroot/Uploads/CatImg/{1}",
             Directory.GetCurrentDirectory(), oldimg);
            if (oldimg != null)
            {
                System.IO.File.Delete(oldpath);
            }


            _context.CategoryLev3s.Remove(categoryLev3);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        /// End Opertion Of CatgeoryLev 2  ///




        /// All Opertion Of PropertyLev 1 ///
        public async Task<IActionResult> IndexPropertyLev1(int? cat1ID, int? cat2ID)
        {
            ViewData["CategoryLev1ID"] = new SelectList(_context.CategoryLev1s, "ID", "Name", cat1ID);
            ViewData["CategoryLev2ID"] = new SelectList(_context.CategoryLev2s.Where(i=>i.CategoryLev1ID==cat1ID), "ID", "Name",cat2ID);

            return View(await _context.PropertyLev1s.OrderBy(i=>i.Index).Where(i => i.CategoryLev2ID == cat2ID).ToListAsync());
        }

        public IActionResult AddPropertyLev1(int id, int Cat1ID)
        {
            var ToGetCatID = _context.CategoryLev2s.FirstOrDefault(i => i.ID == id);
            ViewBag.cat2id = ToGetCatID.ID;
            ViewBag.cat1id = ToGetCatID.CategoryLev1ID;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddPropertyLev1(int id, PropertyLev1 propertyLev1)
        {
            if (ModelState.IsValid)
            {
                var pro = new PropertyLev1
                {
                    CategoryLev2ID = id,
                    Name = propertyLev1.Name,
                    Index = propertyLev1.Index,
                };
                _context.Add(pro);
                await _context.SaveChangesAsync();

                var ToGetCat1ID = _context.CategoryLev2s.FirstOrDefault(i => i.ID == id);
                return RedirectToAction(nameof(IndexPropertyLev1), new { @cat1ID = ToGetCat1ID.CategoryLev1ID, @cat2ID = id });
            }
            return View(propertyLev1);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPropertyLev1(int id, PropertyLev1 propertyLev1,int Cat1ID)
        {
            if (id != propertyLev1.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(propertyLev1);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(IndexPropertyLev1), new { @cat1ID = Cat1ID, @cat2ID = propertyLev1.CategoryLev2ID });
            }

            return View(propertyLev1);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePropertyLev1(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var propertyLev1 = await _context.PropertyLev1s.FindAsync(id);

            _context.PropertyLev1s.Remove(propertyLev1);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(IndexPropertyLev1),new { @id=propertyLev1.CategoryLev2ID });
        }
        /// End Opertion Of PropertyLev 1 ///



        /// All Opertion Of PropertyLev 2  int? cat1ID, int? cat2ID , ///
        public async Task<IActionResult> IndexPropertyLev2(int? ProID , int? cat2ID)
        {
            var Cat1ID =  _context.CategoryLev2s.FirstOrDefaultAsync(i => i.ID == cat2ID).Result.CategoryLev1ID;
            ViewData["CategoryLev1ID"] = new SelectList(_context.CategoryLev1s, "ID", "Name", Cat1ID);
            ViewData["CategoryLev2ID"] = new SelectList(_context.CategoryLev2s.Where(i => i.CategoryLev1ID == Cat1ID), "ID", "Name", cat2ID);
            ViewData["PropertyLev1ID"] = new SelectList(_context.PropertyLev1s.Where(i => i.CategoryLev2ID == cat2ID), "ID", "Name", ProID);

            return View(await _context.PropertyLev2s.OrderBy(i=>i.Index).Where(i => i.PropertyLev1ID == ProID).ToListAsync());
        }

        public IActionResult AddPropertyLev2(int id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPropertyLev2(int id, PropertyLev2 propertyLev2 , IFormFile ProImg)
        {
            if (ModelState.IsValid)
            {
                var pro = new PropertyLev2
                {
                    PropertyLev1ID = id,
                    Name = propertyLev2.Name,
                    Index = propertyLev2.Index,
                    ImgUrl = Img(ProImg),
                };
                _context.Add(pro);
                await _context.SaveChangesAsync();

                var cat2id = _context.PropertyLev1s.FirstOrDefault(i => i.ID == id).CategoryLev2ID;
                return RedirectToAction(nameof(IndexPropertyLev2), new { @ProID = propertyLev2.PropertyLev1ID, @cat2ID = cat2id });
            }
            return View(propertyLev2);
        }

        public async Task<IActionResult> EditPropertyLev2(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var propertyLev2 = await _context.PropertyLev2s.FindAsync(id);
            if (propertyLev2 == null)
            {
                return NotFound();
            }
            return View(propertyLev2);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPropertyLev2(int id, PropertyLev2 propertyLev2, IFormFile ProImg, int Cat2ID)
        {
            if (id != propertyLev2.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                //update img
                if (ProImg != null)
                {
                    //remove old img
                    var oldimg = propertyLev2.ImgUrl;
                    string oldpath = string.Format("{0}/wwwroot/Uploads/CatImg/{1}",
                     Directory.GetCurrentDirectory(), oldimg);
                    if (oldimg != null)
                    {
                        System.IO.File.Delete(oldpath);
                    }

                    //add new img
                    propertyLev2.ImgUrl = Img(ProImg);
                }
                _context.Update(propertyLev2);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(IndexPropertyLev2), new { @ProID = propertyLev2.PropertyLev1ID, @cat2ID = Cat2ID });
            }

            return View(propertyLev2);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePropertyLev2(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var propertyLev2 = await _context.PropertyLev2s.FindAsync(id);

            //remove img
            var oldimg = propertyLev2.ImgUrl;
            string oldpath = string.Format("{0}/wwwroot/Uploads/CatImg/{1}",
             Directory.GetCurrentDirectory(), oldimg);
            if (oldimg != null)
            {
                System.IO.File.Delete(oldpath);
            }

            _context.PropertyLev2s.Remove(propertyLev2);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(IndexPropertyLev1), new { @id = propertyLev2.PropertyLev1ID });
        }
        /// End Opertion Of PropertyLev 2 ///




        // GetCatLev2
        public async Task<IActionResult> GetCatLev2(int? id)
        {
            var Cat = await _context.CategoryLev2s.Where(i => i.CategoryLev1ID == id).ToListAsync();
            return Json(Cat);
        }

        // GetPro1
        public async Task<IActionResult> GetPro1(int? id)
        {
            var Pro = await _context.PropertyLev1s.Where(i => i.CategoryLev2ID == id).ToListAsync();
            return Json(Pro);
        }

        /// Add Img ///
        public string Img(IFormFile fav)
        {
            string filename="";
            if (Request.Form.Files.Count > 0 && Request.Form.Files[0].Length > 0)
            {
                var file = fav;
                string[] str = file.FileName.Split('.');
                string ext = str[str.Length - 1];
                filename = string.Format("{0}.{1}", Guid.NewGuid().ToString(), ext);
                string path = string.Format("{0}/wwwroot/Uploads/CatImg/{1}",
                    Directory.GetCurrentDirectory(), filename);
                using (var streem = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(streem);
                }
            }
            return filename;

        }
    }
}