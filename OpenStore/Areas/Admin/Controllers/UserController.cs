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
    [Authorize(Roles = "مدير,موظف")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IPasswordHasher<User> _passwordHash;
        public UserController(ApplicationDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager , IPasswordHasher<User> passwordHash)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _passwordHash = passwordHash;
        }

        // GET: User
        public ActionResult Index(int? select, string? search, int? page)
        {

            var Users = _userManager.Users.Select(b => new UserVm
            {
                ID = b.Id,
                FullName = b.FullName,
                Email = b.Email,
                UserName = b.UserName,
                roles = _userManager.GetRolesAsync(b).Result,
                Date = b.Date,
                City = b.Province.City.Name,
                Province = b.Province.Name,
            });

            if (select.HasValue && search != null)
            {
                switch (select)
                {
                    case 1:
                        Users = Users.Where(i => i.FullName.Contains(search));
                        break;
                    case 2:
                        Users = Users.Where(i => i.Email.Contains(search));
                        break;
                    case 3:
                        Users = Users.Where(c => c.roles.Any(i => i.Contains(search)));
                        break;
                    case 4:
                        Users = Users.Where(i => i.Date.ToString().Contains(search));
                        break;
                    case 5:
                        Users = Users.Where(i => i.City.Contains(search));
                        break;
                }
            }

            return View(Users.OrderByDescending(i => i.Date).ToPagedList(page ?? 1, 20));
        }

        // GET: User/Details/5
        public async Task<ActionResult> Details(string id)
        {            
            var User = await _userManager.Users.Select(b => new UserVm
            {
                ID = b.Id,
                FullName = b.FullName,
                Email = b.Email,
                UserName = b.UserName,
                roles = _userManager.GetRolesAsync(b).Result,
                Date = b.Date,
                City = b.Province.City.Name,
                Province = b.Province.Name,
                Password = b.PasswordHash.ToString(),
                Address = b.State,
                PhotoUrl = b.PhotoUrl,
                CommercialNumber = b.CommercialNumber,
            }).FirstOrDefaultAsync(i=>i.UserName== id);


            return View(User);
        }

        // GET: User/Create
        public ActionResult Create()
        {
            ViewData["roles"] = new SelectList(_roleManager.Roles,"Id","Name");
            ViewData["CityID"] = new SelectList(_context.Cities, "ID", "Name");
            return View();
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(UserVm model , IFormFile UserImg)
        {
            ViewData["roles"] = new SelectList(_roleManager.Roles, "Id", "Name", model.RoleID);
            ViewData["CityID"] = new SelectList(_context.Cities, "ID", "Name");
            var role = await _roleManager.FindByIdAsync(model.RoleID);

            if (ModelState.IsValid)
            {
                //check is email and username with other
                var Users = await _userManager.Users.ToListAsync();
                if (Users.Any(i => i.Email == model.Email))
                {
                    ModelState.AddModelError("", "الايميل مسجل مسبقا");
                    return View();
                }

                if (Users.Any(i => i.UserName == model.UserName))
                {
                    ModelState.AddModelError("", "اسم المستخدم مسجل مسبقا");
                    return View();
                }

                var _User = new User
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    FullName = model.FullName,
                    ProvinceID = model.ProvinceID,
                    State = model.Address,
                    Date = DateTime.Now,
                    CommercialNumber = model.CommercialNumber ?? ""                    
                };

                Img(UserImg,_User);
                

                var result = await _userManager.CreateAsync(_User, model.Password);

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("Users", "خطاء غير متوقع");
                }

                if (role == null)
                {
                    ModelState.AddModelError("Users", "حصل خطاء اثناء البحث عن الصلاحية المختاره");
                    return View();
                }

                else
                {
                    var AddToRole_result = await _userManager.AddToRoleAsync(_User, role.Name);
                    if (!AddToRole_result.Succeeded)
                    {
                        ModelState.AddModelError("", "حصل خطاء غير متوقع اثناء اضافة الصلاحية للمستخدم");
                        return View();
                    }
                }

                return RedirectToAction(nameof(Index));

            }

            return View();
        }

        // GET: User/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            //ViewData["roles"] = new SelectList(_roleManager.Roles, "Id", "Name");
            ViewData["CityID"] = new SelectList(_context.Cities, "ID", "Name");
            ViewData["ProvinceID"] = new SelectList(_context.Provinces, "ID", "Name");

            var user = await _userManager.FindByIdAsync(id);
            var roles = await _roleManager.Roles.ToListAsync();
            var Province = await _context.Provinces.ToListAsync();
            var City = await _context.Cities.ToListAsync();


            var ViewModel = new EditUser
            {
                UserId = user.Id,
                UserName = user.UserName,
                Roles = roles.Select(role => new RoleVm {
                    ID = role.Id,
                    Name = role.Name,
                    IsSelected = _userManager.IsInRoleAsync(user, role.Name).Result,
                }).ToList(),
                FullName = user.FullName,
                Email = user.Email,
                CommercialNumber = user.CommercialNumber,
                Cities = City,
                CityID = Province.Where(i=>i.ID==user.ProvinceID).Select(i=>i.CityID).FirstOrDefault(),
                Province = Province,
                ProvinceID = user.ProvinceID,
                Address = user.State,
                PhotoUrl = user.PhotoUrl

            };



            if (user == null)
                return NotFound();


            return View(ViewModel);
        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id , EditUser model, string? RoleID , IFormFile UserImg)
        {
            ViewData["roles"] = new SelectList(_roleManager.Roles, "Id", "Name");
            ViewData["CityID"] = new SelectList(_context.Cities, "ID", "Name" );
            ViewData["ProvinceID"] = new SelectList(_context.Provinces, "ID", "Name", model.ProvinceID);



            var user = await _userManager.FindByIdAsync(id);


            if (user == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                //check is email and username with other
                var Users = await _userManager.Users.ToListAsync();
                if (Users.Any(i => i.Email == model.Email && i.Id != user.Id))
                {
                    ModelState.AddModelError("", "الايميل مسجل مسبقا");
                    return View();
                }

                if (Users.Any(i => i.UserName == model.UserName && i.Id != user.Id))
                {
                    ModelState.AddModelError("", "اسم المستخدم مسجل مسبقا");
                    return View();
                }

                //update
                user.UserName = model.UserName;
                user.Email = model.Email;
                user.FullName = model.FullName;
                user.ProvinceID = model.ProvinceID;
                user.State = model.Address;
                user.CommercialNumber = model.CommercialNumber ?? "";



                //img
                if (UserImg != null)
                {
                    var oldimg = user.PhotoUrl;
                    string oldpath = string.Format("{0}/wwwroot/Uploads/UserImg/{1}",
                     Directory.GetCurrentDirectory(), oldimg);
                    if (oldimg != null)
                    {
                        System.IO.File.Delete(oldpath);
                    }
                }
                Img(UserImg, user);


                //to update role
                var old_role =  _userManager.GetRolesAsync(user).Result.FirstOrDefault();
                if (RoleID != null)
                {
                    var new_role = await _roleManager.FindByIdAsync(RoleID);

                    await _userManager.RemoveFromRoleAsync(user, old_role);


                    var AddToRole_result = await _userManager.AddToRoleAsync(user, new_role.Name);
                    if (!AddToRole_result.Succeeded)
                    {
                        ModelState.AddModelError("", "حصل خطاء غير متوقع اثناء اضافة الصلاحية للمستخدم");
                        return View();
                    }

                    //update user
                    IdentityResult result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                        return RedirectToAction("Index");
                    else
                        ModelState.AddModelError("", "خطاء غير متوقع");
                }
                else
                    ModelState.AddModelError("", " اضف صلاحية ");




            }


            return View(user);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var oldimg = user.PhotoUrl;
                string oldpath = string.Format("{0}/wwwroot/Uploads/UserImg/{1}",
                 Directory.GetCurrentDirectory(), oldimg);
                if (oldimg != null)
                {
                    System.IO.File.Delete(oldpath);
                }

                IdentityResult result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                    return RedirectToAction("Index");
                else
                    ModelState.AddModelError("", "خطاء غير متوقع");
            }
            else
                ModelState.AddModelError("", "المستخدم غير موجود");

            return View(user);
        }


        [HttpGet]
        public async Task<IActionResult> getsub(int? id)
        {
            var ctor = await _context.Provinces.Where(i => i.CityID == id).ToListAsync();
            return Json(ctor);
        }

        //Add Img
        public void Img(IFormFile fav, User u)
        {
            if (Request.Form.Files.Count > 0 && Request.Form.Files[0].Length > 0)
            {
                var file = fav;
                string[] str = file.FileName.Split('.');
                string ext = str[str.Length - 1];
                string filename = string.Format("{0}.{1}", Guid.NewGuid().ToString(), ext);
                string path = string.Format("{0}/wwwroot/Uploads/UserImg/{1}",
                    Directory.GetCurrentDirectory(), filename);
                using (var streem = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(streem);
                }

                u.PhotoUrl = filename;
            }
        }

        //Edit Img

        public void EImg(IFormFile fav, UserVm u)
        {
            if (Request.Form.Files.Count > 0 && Request.Form.Files[0].Length > 0)
            {
                var file = fav;
                string[] str = file.FileName.Split('.');
                string ext = str[str.Length - 1];
                string filename = string.Format("{0}.{1}", Guid.NewGuid().ToString(), ext);
                string path = string.Format("{0}/wwwroot/Uploads/UserImg/{1}",
                    Directory.GetCurrentDirectory(), filename);
                using (var streem = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(streem);
                }

                u.PhotoUrl = filename;
            }
        }
    }
}