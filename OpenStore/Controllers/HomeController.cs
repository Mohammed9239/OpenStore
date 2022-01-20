using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OpenStore.Data;
using OpenStore.Models;
using OpenStore.VM;
using PagedList.Core;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Type = OpenStore.Models.Type;

namespace OpenStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<User> _signInManager;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, SignInManager<User> SignInManager)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = SignInManager;
        }


        public async Task<IActionResult> Index()
        {
            // For reference Values In Layout
            await All();

            // For ADS in View
            var ADS1 = _context.ADS.Where(i => i.Page == Page.الرئيسية && i.Section == Section.الاول).FirstOrDefault();
            var ADS2 = _context.ADS.Where(i => i.Page == Page.الرئيسية && i.Section == Section.الثاني).FirstOrDefault();

            if (ADS1 != null)
            {
                ViewBag.ADS1 = ADS1.Photo ?? "";
            }

            if (ADS2 != null)
            {
                ViewBag.ADS2 = ADS2.Photo ?? "";
            }

            // Object for Date In View
            var model = new HomeVm
            {
                CategoryLev1s = await _context.CategoryLev1s.ToListAsync(),
                Products = await _context.Products.Take(10).Include(i=>i.GetCategoryLev2).Include(i=>i.Evaluations).Include(i=>i.Imgs).ToListAsync(),
                ProductsDiscount = await _context.Products.Take(10).Where(i => i.Discount != 0).Include(i => i.GetCategoryLev2).Include(i => i.Evaluations).Include(i => i.Imgs).ToListAsync(),
                ADs = await _context.ADS.Where(i => i.Page == Page.الرئيسية).ToListAsync(),
            };

            return View(model);
        }


        public async Task<IActionResult> Category(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            // For reference Values In Layout
            await All();

            // Object CategoryLev2s
            var model = await _context.CategoryLev2s.Include(i=>i.CategoryLev1).Where(i => i.CategoryLev1ID == id).ToListAsync();
            if (model == null)
            {
                return NotFound();
            }

            // Name Of CategoryLev1
            ViewBag.CategoryLev1Name =  _context.CategoryLev1s.FirstOrDefaultAsync(i => i.ID == id).Result.Name;

            // For ADS in View
            var ADS1 = _context.ADS.Where(i => i.Page == Page.الاصناف && i.Section == Section.الاول).FirstOrDefault();
            var ADS2 = _context.ADS.Where(i => i.Page == Page.الاصناف && i.Section == Section.الثاني).FirstOrDefault();

            if (ADS1 != null)
            {
                ViewBag.ADS1 = ADS1.Photo??"";
            }

            if (ADS2 != null)
            {
                ViewBag.ADS2 = ADS2.Photo ?? "";
            }

            return View(model);
        }

        public async Task<IActionResult> SubCategory(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // For reference Values In Layout
            await All();

            // Object CategoryLev2s
            var model = await _context.CategoryLev3s.Include(i => i.CategoryLev2).Where(i => i.CategoryLev2ID == id).ToListAsync();
            if (model == null)
            {
                return NotFound();
            }

            if (model.Count==0)
            {
                return RedirectToAction(nameof(Shop), new { @CategoryLev2 = id });
            }

            // Name Of CategoryLev1
            ViewBag.CategoryLev1Name =  model.FirstOrDefault().CategoryLev2.Name;

            // For ADS in View
            var ADS1 = _context.ADS.Where(i => i.Page == Page.الاصناف && i.Section == Section.الاول).FirstOrDefault();
            var ADS2 = _context.ADS.Where(i => i.Page == Page.الاصناف && i.Section == Section.الثاني).FirstOrDefault();

            if (ADS1 != null)
            {
                ViewBag.ADS1 = ADS1.Photo ?? "";
            }

            if (ADS2 != null)
            {
                ViewBag.ADS2 = ADS2.Photo ?? "";
            }

            return View(model);
        }

        public async Task<IActionResult> Shop(int? CategoryLev1, int? CategoryLev2, int? CategoryLev3, int? select, string? word, int? page)
        {
            // For reference Values In Layout
            await All();

            // For ADS in View
            var ADS1 = _context.ADS.Where(i => i.Page == Page.التسوق && i.Section == Section.الاول).FirstOrDefault();
            var ADS2 = _context.ADS.Where(i => i.Page == Page.التسوق && i.Section == Section.الثاني).FirstOrDefault();

            if (ADS1 != null)
            {
                ViewBag.ADS1 = ADS1.Photo ?? "";
            }

            if (ADS2 != null)
            {
                ViewBag.ADS2 = ADS2.Photo ?? "";
            }


            // Object List Of Product
            var model =  _context.Products.OrderByDescending(i=>i.Date)
                .Include(i => i.Evaluations)
                .Include(i => i.Imgs)
                .Include(i => i.GetCategoryLev2);


            // Select List Of CategoryLev1
            ViewData["CategoryLev1ID"] = new SelectList(_context.CategoryLev1s, "ID", "Name", CategoryLev1);

            // Get Product By Category Lev1
            if (CategoryLev1.HasValue)
            {
                ViewBag.CatName = _context.CategoryLev1s.FirstOrDefaultAsync(i=>i.ID== CategoryLev1).Result.Name;
                model = _context.Products.Where(i => i.GetCategoryLev2.CategoryLev1ID == CategoryLev1)
                    .Include(i => i.Evaluations)
                    .Include(i => i.Imgs)
                    .Include(i => i.GetCategoryLev2);
            }

            // Get Product By Category Lev2
            if (CategoryLev2.HasValue)
            {
                ViewBag.CatName = _context.CategoryLev1s.FirstOrDefaultAsync(i => i.CategoryLev2s.FirstOrDefault(i => i.ID == CategoryLev2).ID == CategoryLev2).Result.Name + " => " + _context.CategoryLev2s.FirstOrDefaultAsync(i => i.ID == CategoryLev2).Result.Name;

                model = _context.Products.Where(i => i.GetCategoryLev2ID == CategoryLev2)
                    .Include(i => i.Evaluations)
                    .Include(i => i.Imgs)
                    .Include(i => i.GetCategoryLev2);
            }


            // Get Product By Category Lev2
            if (CategoryLev3.HasValue)
            {
                ViewBag.CatName = _context.CategoryLev2s.FirstOrDefaultAsync(i => i.CategoryLev3s.FirstOrDefault(i => i.ID == CategoryLev3).ID == CategoryLev3).Result.CategoryLev1.Name + " => " + _context.CategoryLev2s.FirstOrDefaultAsync(i => i.CategoryLev3s.FirstOrDefault(i=>i.ID==CategoryLev3).ID == CategoryLev3).Result.Name + " => " + _context.CategoryLev3s.FirstOrDefaultAsync(i => i.ID == CategoryLev3).Result.Name;

                model = _context.Products.Where(i => i.GetCategoryLev3ID == CategoryLev3)
                    .Include(i => i.Evaluations)
                    .Include(i => i.Imgs)
                    .Include(i => i.GetCategoryLev2);
            }

            // To Get Product By Serceh
            if (!String.IsNullOrEmpty(word))
            {
                ViewBag.word = word;
                model = _context.Products.Where(i => i.Name.Contains(word) || i.GetCategoryLev2.Name.Contains(word))
                    .Include(i => i.Imgs)
                    .Include(i => i.GetCategoryLev2);

            }

            // To Sort Product By Select Option Like Price Low 

            if (select.HasValue)
            {
                switch (select)
                {
                    case 1:
                        ViewBag.sort = "الاعلى تقيماً";
                        model = _context.Products.OrderByDescending(i=>i.Evaluations.Max(m=>m.Value))
                            .Include(i => i.Evaluations)
                            .Include(i => i.Imgs)
                            .Include(i => i.GetCategoryLev2);
                        break;

                    case 2:
                        ViewBag.sort = "الاعلى سعر";
                        model = _context.Products.OrderByDescending(i => i.Price)
                            .Include(i => i.Evaluations)
                            .Include(i => i.Imgs)
                            .Include(i => i.GetCategoryLev2);
                        break;

                    case 3:
                        ViewBag.sort = "الاقل سعر";
                        model = _context.Products.OrderBy(i=>i.Price)
                            .Include(i => i.Evaluations)
                            .Include(i => i.Imgs)
                            .Include(i => i.GetCategoryLev2);
                        break;
                    default:
                        break;
                }
            }


            return View(model.ToPagedList(page ?? 1, 16));
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // For reference Values In Layout
            await All();

            // Object CategoryLev2s
            var model = await _context.Products.Select(p => new ProductDetails
            {
                ID = p.ID,
                Name = p.Name,
                CategoryName = String.Format("{0} => {1}",p.GetCategoryLev2.CategoryLev1.Name,p.GetCategoryLev2.Name),
                Evaluations = p.Evaluations.Sum(i=>i.Value) / p.Evaluations.Count ,
                EvaluationCount = p.Evaluations.Count,
                Imgs = p.Imgs.Select(i=>i.ImgUrl).ToList(),
                Price = p.Price,
                Discount = p.Discount,
                Qunatity = p.Qunatity,
                Description = p.Description,
                Property = p.ProdectProperties.Select(i=>i.PropertyLev2.PropertyLev1.Name).ToList(),
                PropertyValue = p.ProdectProperties.Select(i=>i.PropertyLev2.Name).ToList(),
            }).FirstOrDefaultAsync(i=>i.ID==id);

            if (model == null)
            {
                return NotFound();
            }

            // Get User 
            User user = await _userManager.GetUserAsync(User);
            string userid = user?.Id;

            // To Check User Is Evaluations
            ViewBag.Evaluation = await _context.Evaluations.FirstOrDefaultAsync(i => i.ProductID == id && i.UserID == userid);

            // To Check User Is Has This Product

            var CheckProductIsInCart = await _context.OrderProducts.Where(i => i.Order.UserID == userid && i.ProductID == id && i.Order.State == State.incart).ToListAsync();

            if (CheckProductIsInCart.Count > 0)
            {
                ViewBag.ProductIsInCart = true;

            }

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> AddToCart(OrderProduct orderProduct)
        {
            if (ModelState.IsValid)
            {
                // to get user
                User user = await _userManager.GetUserAsync(User);
                string userid = user.Id;
                var CheckUserIsHasOreder = await _context.Orders.FirstOrDefaultAsync(i => i.UserID == userid && i.State == State.incart);
                if (CheckUserIsHasOreder != null)
                {
                    var OrderProduct = new OrderProduct
                    {
                        OrderID = CheckUserIsHasOreder.ID,
                        ProductID = orderProduct.ProductID,
                        Quantity = orderProduct.Quantity,
                        Price = orderProduct.Price,
                    };

                    _context.Add(OrderProduct);
                    await _context.SaveChangesAsync();
                }

                else
                {
                    var Order = new Order
                    {
                        UserID = userid,
                        ProvinceID = user.ProvinceID,
                        State = State.incart,
                        Date = DateTime.Now,
                        Street = user.Street,
                    };

                    _context.Add(Order);
                    await _context.SaveChangesAsync();

                    var OrderProduct = new OrderProduct
                    {
                        OrderID = Order.ID,
                        ProductID = orderProduct.ProductID,
                        Quantity = orderProduct.Quantity,
                        Price = orderProduct.Price,
                    };

                    _context.Add(OrderProduct);
                    await _context.SaveChangesAsync();

                }

            }

            return RedirectToAction("Details", new { @id = orderProduct.ProductID });
        }


        [HttpPost]
        public async Task<IActionResult> Evaluation(Evaluation evaluation)
        {
            if (ModelState.IsValid)
            {
                // to get user
                User user = await _userManager.GetUserAsync(User);
                string userid = user.Id;
                var CheckUserIsEvaluation = await _context.Evaluations.FirstOrDefaultAsync(i => i.ProductID == evaluation.ProductID && i.UserID == userid);

                if (CheckUserIsEvaluation != null)
                {
                    CheckUserIsEvaluation.UserID = userid;
                    CheckUserIsEvaluation.ProductID = evaluation.ProductID;
                    CheckUserIsEvaluation.Value = evaluation.Value;
                    _context.Update(CheckUserIsEvaluation);
                    await _context.SaveChangesAsync();
                }

                else
                {
                    var Evaluation = new Evaluation
                    {
                        UserID = user.Id,
                        ProductID = evaluation.ProductID,
                        Value = evaluation.Value,
                    };
                    _context.Add(Evaluation);
                    await _context.SaveChangesAsync();
                }
            }


            return RedirectToAction("Details",new { @id=evaluation.ProductID });
        }

        [Authorize]
        public async Task<IActionResult> Cart()
        {
            // For reference Values In Layout
            await All();

            // For ADS in View
            var ADS1 = _context.ADS.Where(i => i.Page == Page.السلة && i.Section == Section.الاول).FirstOrDefault();
            var ADS2 = _context.ADS.Where(i => i.Page == Page.السلة && i.Section == Section.الثاني).FirstOrDefault();

            if (ADS1 != null)
            {
                ViewBag.ADS1 = ADS1.Photo ?? "";
            }

            if (ADS2 != null)
            {
                ViewBag.ADS2 = ADS2.Photo ?? "";
            }

            // Get User 
            User user = await _userManager.GetUserAsync(User);
            string userid = user.Id;

            // Get Product In Cart
            var OrderProduct = await _context.OrderProducts.Where(i => i.Order.UserID == userid && i.Order.State == State.incart)
                .Include(i => i.Order).Include(i => i.Product).ThenInclude(i => i.Imgs).ToListAsync();

            return View(OrderProduct);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderProduct = await _context.OrderProducts.FindAsync(id);

            if (orderProduct == null)
            {
                return NotFound();
            }

            _context.OrderProducts.Remove(orderProduct);
            await _context.SaveChangesAsync();


            return RedirectToAction("Cart");
        }

        [Authorize]
        public async Task<IActionResult> CompleteOrder()
        {
            // For reference Values In Layout
            await All();

            // For ADS in View
            var ADS1 = _context.ADS.Where(i => i.Page == Page.اكمال_الطلب && i.Section == Section.الاول).FirstOrDefault();
            var ADS2 = _context.ADS.Where(i => i.Page == Page.اكمال_الطلب && i.Section == Section.الثاني).FirstOrDefault();

            if (ADS1 != null)
            {
                ViewBag.ADS1 = ADS1.Photo ?? "";
            }

            if (ADS2 != null)
            {
                ViewBag.ADS2 = ADS2.Photo ?? "";
            }

            // to get user
            User user = await _userManager.GetUserAsync(User);
            string userid = user.Id;

            // To Get City 
            ViewData["City"] = new SelectList(_context.Cities, "ID", "Name");

            // Get Product In Cart
            var Oredr = await _context.Orders.Where(i => i.UserID == userid && i.State == State.incart).FirstOrDefaultAsync();

            Oredr.OrderProducts = await _context.OrderProducts.Include(i=>i.Product).Where(i => i.OrderID == Oredr.ID).ToListAsync();

            return View(Oredr);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CompleteOrder(int? id , Order order , int NewProvinceID, IFormFile BankImg)
        {
            // For reference Values In Layout
            await All();

            // For ADS in View
            var ADS1 = _context.ADS.Where(i => i.Page == Page.اكمال_الطلب && i.Section == Section.الاول).FirstOrDefault();
            var ADS2 = _context.ADS.Where(i => i.Page == Page.اكمال_الطلب && i.Section == Section.الثاني).FirstOrDefault();

            if (ADS1 != null)
            {
                ViewBag.ADS1 = ADS1.Photo ?? "";
            }

            if (ADS2 != null)
            {
                ViewBag.ADS2 = ADS2.Photo ?? "";
            }

            if (id == null)
            {
                return NotFound();
            }

            // To Get City 
            ViewData["City"] = new SelectList(_context.Cities, "ID", "Name");

            // To Get Order
            var Order = await _context.Orders.FindAsync(id);
            Order.OrderProducts = await _context.OrderProducts.Include(i => i.Product).Where(i => i.OrderID == Order.ID).ToListAsync();
           
            if (order == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                Order.BankNum = order.BankNum;
                Order.Cost = order.Cost;
                Order.BankImg = Img(BankImg);
                Order.State = State.Demand;
                if (NewProvinceID != 0)
                {
                    Order.ProvinceID = NewProvinceID;
                }

                _context.Orders.Update(Order);
                await _context.SaveChangesAsync();

                // To change Qun from product

                foreach (var item in Order.OrderProducts)
                {
                    var Product = await _context.Products.FindAsync(item.ProductID);
                    Product.Qunatity-=item.Quantity;
                    _context.Products.Update(Product);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction("DoneOrder");
            }


            return View(order);
        }

        [Authorize]
        public async Task<IActionResult> DoneOrder()
        {
            // For reference Values In Layout
            await All();

            // For ADS in View
            var ADS1 = _context.ADS.Where(i => i.Page == Page.تواصل && i.Section == Section.الاول).FirstOrDefault();
            var ADS2 = _context.ADS.Where(i => i.Page == Page.تواصل && i.Section == Section.الثاني).FirstOrDefault();

            if (ADS1 != null)
            {
                ViewBag.ADS1 = ADS1.Photo ?? "";
            }

            if (ADS2 != null)
            {
                ViewBag.ADS2 = ADS2.Photo ?? "";
            }

            return View();
        }


        public async Task<IActionResult> Contact()
        {
            // For reference Values In Layout
            await All();

            // For ADS in View
            var ADS1 = _context.ADS.Where(i => i.Page == Page.تواصل && i.Section == Section.الاول).FirstOrDefault();
            var ADS2 = _context.ADS.Where(i => i.Page == Page.تواصل && i.Section == Section.الثاني).FirstOrDefault();

            if (ADS1 != null)
            {
                ViewBag.ADS1 = ADS1.Photo ?? "";
            }

            if (ADS2 != null)
            {
                ViewBag.ADS2 = ADS2.Photo ?? "";
            }

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Contact(Notification notification)
        {
            // For reference Values In Layout
            await All();

            // For ADS in View
            var ADS1 = _context.ADS.Where(i => i.Page == Page.تواصل && i.Section == Section.الاول).FirstOrDefault();
            var ADS2 = _context.ADS.Where(i => i.Page == Page.تواصل && i.Section == Section.الثاني).FirstOrDefault();

            if (ADS1 != null)
            {
                ViewBag.ADS1 = ADS1.Photo ?? "";
            }

            if (ADS2 != null)
            {
                ViewBag.ADS2 = ADS2.Photo ?? "";
            }

            if (ModelState.IsValid)
            {
                // to get user
                User user = await _userManager.GetUserAsync(User);
                var Notification = new Notification
                {
                    ReceiverID = user.Id,
                    Title = notification.Title,
                    Message = notification.Message,
                    State = false,
                    Type = OpenStore.Models.Type.استفسار,
                    Date = DateTime.Now
                };

                _context.Add(Notification);
                await _context.SaveChangesAsync();
            }


            return View();
        }

        [Authorize]
        public async Task<IActionResult> NotificationList()
        {
            // For reference Values In Layout
            await All();

            // For ADS in View
            var ADS1 = _context.ADS.Where(i => i.Page == Page.الاشعارات && i.Section == Section.الاول).FirstOrDefault();
            var ADS2 = _context.ADS.Where(i => i.Page == Page.الاشعارات && i.Section == Section.الثاني).FirstOrDefault();

            if (ADS1 != null)
            {
                ViewBag.ADS1 = ADS1.Photo ?? "";
            }

            if (ADS2 != null)
            {
                ViewBag.ADS2 = ADS2.Photo ?? "";
            }

            // Get All Notification

            var Notification = await _context.Notifications.Where(i => i.Type == Type.اشعار).ToListAsync();

            return View(Notification);
        }

        [Authorize]
        public async Task<IActionResult> Notification(int? id)
        {
            // For reference Values In Layout
            await All();

            // For ADS in View
            var ADS1 = _context.ADS.Where(i => i.Page == Page.تفاصيل_الاشعارات && i.Section == Section.الاول).FirstOrDefault();
            var ADS2 = _context.ADS.Where(i => i.Page == Page.تفاصيل_الاشعارات && i.Section == Section.الثاني).FirstOrDefault();

            if (ADS1 != null)
            {
                ViewBag.ADS1 = ADS1.Photo ?? "";
            }

            if (ADS2 != null)
            {
                ViewBag.ADS2 = ADS2.Photo ?? "";
            }

            // Get All Notification

            var Notification = await _context.Notifications.FirstOrDefaultAsync(i => i.ID==id);

            return View(Notification);
        }


        [Authorize]
        public async Task<IActionResult> Profile()
        {
            // For reference Values In Layout
            await All();

            // To Check Is Password Changed
            ViewBag.IsChange =  TempData["IsChange"];

            // To Get Information Abot User
            User user = await _userManager.GetUserAsync(User);
            string userid = user.Id;
            user.Orders = await _context.Orders.Include(i=>i.OrderProducts).ThenInclude(i=>i.Product).Where(i => i.UserID == userid && i.State!=State.incart).ToListAsync();
            //user.Phones = await _context.Phones.Where(i => i.UserID == userid).ToListAsync();
            user.Province = await _context.Provinces.Include(i=>i.City).Where(i => i.ID == user.ProvinceID).FirstOrDefaultAsync();

            // To Get City 
            ViewData["City"] = new SelectList(_context.Cities, "ID", "Name");
            ViewData["ProvinceID"] = new SelectList(_context.Provinces.Where(i=>i.CityID==user.Province.CityID), "ID", "Name",user.ProvinceID);
            return View(user);
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditProfile(User u, /*List<int> PhonesID, List<string> Phones*/ IFormFile ImgUser)
        {
            // To Get Information Abot User
            User user = await _userManager.GetUserAsync(User);

            user.FullName = u.FullName;
            user.Email = u.Email;
            user.ProvinceID = u.ProvinceID;
            user.Street = u.Street;
            user.PhoneNumber = u.PhoneNumber;

            ////To Update Phone
            //for (int i = 0; i < PhonesID.Count; i++)
            //{
            //    var Phone = await _context.Phones.FindAsync(PhonesID[i]);
            //    Phone.Number = Phones[i];
            //    _context.Update(Phone);
            //    await _context.SaveChangesAsync();
            //}

            //// To Update UserImg
            if (ImgUser != null)
            {
                // Remove Old Img
                if (user.PhotoUrl != null)
                {
                    var oldimg = user.PhotoUrl;
                    string oldpath = string.Format("{0}/wwwroot/Uploads/UserImg/{1}",
                     Directory.GetCurrentDirectory(), oldimg);
                    System.IO.File.Delete(oldpath);
                }

                // New Img
                user.PhotoUrl = UImg(ImgUser);
            }

            await _userManager.UpdateAsync(user);
            return RedirectToAction("Profile");
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(string OldPass, string NewPass)
        {
            // To Get Information Abot User
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, OldPass, NewPass);
            if (!changePasswordResult.Succeeded)
            {
                TempData["IsChange"] = false;
                return RedirectToAction("Profile");
            }

            else
            {
                TempData["IsChange"] = true;
                return RedirectToAction("SuccessChangePassword");

            }

        }

        [Authorize]
        public async Task<IActionResult> SuccessChangePassword()
        {
            // For reference Values In Layout
            await All();


            return View();
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }



        // To Show CategoryLev1 And Count Cart And Notification
        public async Task All()
        {
            ViewData["CategoryLev1"] = await _context.CategoryLev1s.ToListAsync();
            ViewData["CartCount"] = "";
            ViewData["NotificationCount"] = "";
            if (_signInManager.IsSignedIn(User))
            {
                User user = await _userManager.GetUserAsync(User);
                string userid = user.Id;
                ViewData["CartCount"] = _context.OrderProducts.Where(i => i.Order.UserID == userid && i.Order.State == State.incart).ToListAsync().Result.Count;
                ViewData["NotificationCount"] = _context.Notifications.Where(i => i.Type == Type.اشعار).ToListAsync().Result.Count;
            }
        }

        // Change Product Qunatity From Cart When Change 
        public async Task<IActionResult> ChangeQunatity(int? id, int Qun)
        {
            var orderProduct = await _context.OrderProducts.FindAsync(id);
            if (orderProduct == null)
            {
                return Error();
            }

            orderProduct.Quantity = Qun;
            _context.OrderProducts.Update(orderProduct);
            await _context.SaveChangesAsync();
            return Ok();
        }

        // To Get Provinces
        public async Task<IActionResult> Provinces(int? id)
        {
            var ctor = await _context.Provinces.Where(i => i.CityID == id).ToListAsync();
            return Json(ctor);
        }

        // Add Img To Order (BankNum)
        public string Img(IFormFile Img)
        {
            string filename = "";
            if (Request.Form.Files.Count > 0 && Request.Form.Files[0].Length > 0)
            {
                if (Request.Form.Files.Count > 0 && Request.Form.Files[0].Length > 0)
                {
                    var file = Img;
                    string[] str = file.FileName.Split('.');
                    string ext = str[str.Length - 1];
                    filename = string.Format("{0}.{1}", Guid.NewGuid().ToString(), ext);
                    string path = string.Format("{0}/wwwroot/Uploads/BangImg/{1}",
                        Directory.GetCurrentDirectory(), filename);
                    using (var streem = new FileStream(path, FileMode.Create))
                    {
                        file.CopyTo(streem);
                    }
                }
            }
            return filename;
        }

        // Add Img To User
        public string UImg(IFormFile Img)
        {
            string filename = "";
            if (Request.Form.Files.Count > 0 && Request.Form.Files[0].Length > 0)
            {
                if (Request.Form.Files.Count > 0 && Request.Form.Files[0].Length > 0)
                {
                    var file = Img;
                    string[] str = file.FileName.Split('.');
                    string ext = str[str.Length - 1];
                    filename = string.Format("{0}.{1}", Guid.NewGuid().ToString(), ext);
                    string path = string.Format("{0}/wwwroot/Uploads/UserImg/{1}",
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
