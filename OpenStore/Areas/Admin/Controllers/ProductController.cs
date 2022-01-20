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
using OpenStore.Areas.Admin.Models;
using OpenStore.Data;
using OpenStore.Models;
using PagedList.Core;

namespace OpenStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "مدير,موظف,متجر")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        public ProductController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index(int? select, string? search, int? page)
        {
            var Products = _context.Products.Select(b => new ProductVm
            {
                ID = b.ID,
                StoreName = b.Store.FullName,
                ProductName = b.Name,
                Price = b.Price,
                Qunatity = b.Qunatity,
                CategoryLev1 = b.GetCategoryLev2.CategoryLev1.Name,
                CategoryLev2 = b.GetCategoryLev2.Name,
                Date = b.Date,
                Special = b.Special,
            });

            if (select.HasValue && search != null)
            {
                switch (select)
                {
                    case 1:
                        Products = Products.Where(i => i.StoreName.Contains(search));
                        break;
                    case 2:
                        Products = Products.Where(i => i.ProductName.Contains(search));
                        break;
                    case 3:
                        Products = Products.Where(i => i.CategoryLev1.Contains(search) || i.CategoryLev2.Contains(search));
                        break;
                }
            }

            return View(Products.OrderByDescending(i => i.Date).ToPagedList(page ?? 1, 20));
        }

        public IActionResult Special(int? select, string? search, int? page)
        {
            var Products = _context.Products.Select(b => new ProductVm
            {
                ID = b.ID,
                StoreName = b.Store.FullName,
                ProductName = b.Name,
                Price = b.Price,
                Qunatity = b.Qunatity,
                CategoryLev1 = b.GetCategoryLev2.CategoryLev1.Name,
                Date = b.Date,
                Special = b.Special,
            }).Where(i=>i.Special==1);

            if (select.HasValue && search != null)
            {
                switch (select)
                {
                    case 1:
                        Products = Products.Where(i => i.StoreName.Contains(search));
                        break;
                    case 2:
                        Products = Products.Where(i => i.ProductName.Contains(search));
                        break;
                    case 3:
                        Products = Products.Where(i => i.CategoryLev1.Contains(search) || i.CategoryLev2.Contains(search));
                        break;
                }
            }

            return View(Products.OrderByDescending(i => i.Date).ToPagedList(page ?? 1, 20));
        }
        public IActionResult Details(int id)
        {
            var Products = _context.Products.Select(b => new ProductVm
            {
                ID = b.ID,
                StoreName = b.Store.FullName,
                ProductName = b.Name,
                Price = b.Price,
                Qunatity = b.Qunatity,
                Discount = b.Discount,
                CategoryLev1 = b.GetCategoryLev2.CategoryLev1.Name,
                CategoryLev2 = b.GetCategoryLev2.Name ,
                PropertyLev1 = b.ProdectProperties.Select(i => i.PropertyLev2.PropertyLev1.Name).ToList(),
                PropertyLev2 = b.ProdectProperties.Select(i => i.PropertyLev2.Name).ToList(),
                Description = b.Description,
                Imgs = b.Imgs.Select(i => i.ImgUrl).ToList(),

            }).FirstOrDefault(i => i.ID == id);

            return View(Products);
        }

        public IActionResult Create()
        {
            ViewData["CatLev1"] = new SelectList(_context.CategoryLev1s, "ID", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProducCreatetVm product , IFormFile FavImg1 , List<IFormFile> Imgs)
        {
            ViewData["CatLev1"] = new SelectList(_context.CategoryLev1s, "ID", "Name");
            if (ModelState.IsValid)
            {
                // To Add Product
                User user = await _userManager.GetUserAsync(User);
                var pr = new Product
                {
                    StoreID = user.Id,
                    GetCategoryLev2ID = product.GetCategoryLev2ID,
                    GetCategoryLev3ID = product.GetCategoryLev3ID,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    Qunatity = product.Qunatity,
                    Special = 0,
                    Discount = product.Discount,
                    Date = DateTime.Now,
                };

                _context.Add(pr);
                await _context.SaveChangesAsync();

                // To Add Fav Img
                var FImg = new Img
                {
                    ProductID = pr.ID,
                    ImgUrl = FavImg(FavImg1),
                };
                _context.Add(FImg);
                await _context.SaveChangesAsync();

                // To Add Multi Img
                foreach (var item in Imgs)
                {
                    var img = new Img
                    {
                        ProductID = pr.ID,
                        ImgUrl = Img(item)
                    };
                    _context.Add(img);
                    await _context.SaveChangesAsync();
                }


                // To Add To ProdectProperty
                if (product.PropertyLev2ID != null)
                {
                    foreach (var item in product.PropertyLev2ID)
                    {
                        var PP = new ProdectProperty
                        {
                            ProductID = pr.ID,
                            PropertyLev2ID = item
                        };
                        _context.Add(PP);
                        await _context.SaveChangesAsync();
                    }
                }


                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.GetCategoryLev2)
                .FirstOrDefaultAsync(m => m.ID == id);
            product.Imgs = await _context.Imgs.Where(i => i.ProductID == id).ToListAsync();
            if (product == null)
            {
                return NotFound();
            }

            ViewData["GetCategoryLev1ID"] = new SelectList(_context.CategoryLev1s, "ID", "Name", product.GetCategoryLev2.CategoryLev1ID);
            ViewData["GetCategoryLev2ID"] = new SelectList(_context.CategoryLev2s.Where(i=>i.CategoryLev1ID==product.GetCategoryLev2.CategoryLev1ID), "ID", "Name", product.GetCategoryLev2ID);
            ViewData["GetCategoryLev3ID"] = new SelectList(_context.CategoryLev3s.Where(i => i.CategoryLev2ID == product.GetCategoryLev2ID), "ID", "Name", product.GetCategoryLev3ID);

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id , Product product, List<int> PropertyLev2ID, IFormFile FavImg1, List<IFormFile> Imgs)
        {

            var imgs = _context.Imgs.Where(i => i.ProductID == id).ToList();
            if (id != product.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // To Update Product
                _context.Update(product);
                await _context.SaveChangesAsync();

                // To Update New Fav Img
                if (FavImg1!=null)
                {
                    // To Remove Old Fav Img
                    var favimg = imgs.FirstOrDefault(i => i.ImgUrl.StartsWith("fav"));
                    string oldpath = string.Format("{0}/wwwroot/Uploads/ProductImg/{1}",
                    Directory.GetCurrentDirectory(), favimg.ImgUrl);
                    System.IO.File.Delete(oldpath);
                    // To Update New Fav Img
                    favimg.ImgUrl = FavImg(FavImg1);
                    _context.Update(favimg);
                    await _context.SaveChangesAsync();
                }

                // To Update Multi Img
                if (Imgs.Count>0)
                {
                    // To Remove Old Imgs
                    var img = imgs.Where(i => !i.ImgUrl.StartsWith("fav")).ToList();
                    foreach (var item in img)
                    {
                        string oldpath = string.Format("{0}/wwwroot/Uploads/ProductImg/{1}",
                         Directory.GetCurrentDirectory(), item.ImgUrl);
                        System.IO.File.Delete(oldpath);
                        _context.Imgs.Remove(item);
                        await _context.SaveChangesAsync();
                    }

                    // To Update New Imgs
                    foreach (var item in Imgs)
                    {
                        var NewImg = new Img
                        {
                            ProductID = product.ID,
                            ImgUrl = Img(item)
                        };
                        _context.Add(NewImg);
                        await _context.SaveChangesAsync();
                    }
                }

                // To Update ProdectProperty
                var pp = await _context.ProdectProperties.Where(i => i.ProductID == id).ToListAsync();
                for (int i = 0; i < pp.Count; i++)
                {
                    pp[i].PropertyLev2ID = PropertyLev2ID[i];
                    _context.Update(pp[i]);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));

            }

            return View(product);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var imgs = _context.Imgs.Where(i => i.ProductID == id).ToList();
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            foreach (var item in imgs)
            {
                string oldpaths = string.Format("{0}/wwwroot/Uploads/ProductImg/{1}",
                 Directory.GetCurrentDirectory(), item.ImgUrl);
                if (oldpaths != null)
                {
                    System.IO.File.Delete(oldpaths);
                }
            }
            return RedirectToAction(nameof(Index));
        }



        // Change Product Special
        [HttpPost]
        public async Task<IActionResult> ChangeSpecial(int? id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product.Special == 0)
            {
                product.Special = 1;
            }

            else
            {
                product.Special = 0;
            }
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return Json(product);
        }

        // GetCatLev2
        public async Task<IActionResult> GetCatLev2(int? id)
        {
            var Cat = await _context.CategoryLev2s.Where(i => i.CategoryLev1ID == id).ToListAsync();
            return Json(Cat);
        }

        // GetCatLev3
        public async Task<IActionResult> GetCatLev3(int? id)
        {
            var Cat = await _context.CategoryLev3s.Where(i => i.CategoryLev2ID == id).ToListAsync();
            return Json(Cat);
        }

        // GetPro
        public IActionResult GetPro(int? id)
        {
            var P = _context.PropertyLev1s.Where(i => i.CategoryLev2ID == id).Select(b => new GetProVm
            {
                Namep = b.Name,
                PropertyLev2 = b.PropertyLev2s.Select(m=>new pro2
                {
                    ID=m.ID,
                    Name=m.Name,
                    IsSelected = _context.ProdectProperties.Any(i=>i.PropertyLev2ID==m.ID),
                }).ToList(),
                
            }).ToList();

            return Json(P);
        }


        // Add FavImg
        public string FavImg(IFormFile fav)
        {
            string filename = "";
            if (Request.Form.Files.Count > 0 && Request.Form.Files[0].Length > 0)
            {
                if (Request.Form.Files.Count > 0 && Request.Form.Files[0].Length > 0)
                {
                    var file = fav;
                    string[] str = file.FileName.Split('.');
                    string ext = str[str.Length - 1];
                    filename = string.Format("{0}.{1}", "fav" + Guid.NewGuid().ToString(), ext);
                    string path = string.Format("{0}/wwwroot/Uploads/ProductImg/{1}",
                        Directory.GetCurrentDirectory(), filename);
                    using (var streem = new FileStream(path, FileMode.Create))
                    {
                        file.CopyTo(streem);
                    }
                }
            }
            return filename;
        }

        // Add MultiImg
        public string Img(IFormFile fav)
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
                    string path = string.Format("{0}/wwwroot/Uploads/ProductImg/{1}",
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