using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenStore.Services;
using OpenStore.Helper;
using Microsoft.AspNetCore.Authorization;
using OpenStore.Models;
using OpenStore.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.IO;

namespace OpenStore.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class APIsController : ControllerBase
    {
        private readonly IAuthServices _authServices;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public APIsController(IAuthServices authServices , ApplicationDbContext context , UserManager<User> userManager)
        {
            _authServices = authServices;
            _context = context;
            _userManager = userManager;
        }

        [HttpPost("Register")]
        // send ProvinceID from Province Task
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authServices.Register(model);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            return Ok(result);

        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] TokenRequestModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authServices.GetToken(model);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            return Ok(result);

        }


        [HttpGet("Home")]
        public JsonResult Home()
        {
            var ADS1 = _context.ADS.Where(i => i.Page == Page.الرئيسية && i.Section == Section.الاول).FirstOrDefault();
            var ADS2 = _context.ADS.Where(i => i.Page == Page.الرئيسية && i.Section == Section.الثاني).FirstOrDefault();

            string ServerName = System.IO.Directory.GetCurrentDirectory();

            var data = new
            {
                ProductsLast = _context.Products.Take(10).OrderByDescending(i => i.Date).Select(p => new
                {
                    Name = p.Name,
                    Price = p.Price,
                    Discount = p.Discount,
                    Evaluation = p.Evaluations.Sum(i => i.Value) / p.Evaluations.Count,
                    Img = string.Format("{0}/wwwroot/Uploads/ProductImg/{1}",
                        ServerName, p.Imgs.FirstOrDefault(i => i.ImgUrl.StartsWith("fav")).ImgUrl)
                }),

                ProductsDiscount = _context.Products.Where(i=>i.Discount!=0).Take(10).OrderByDescending(i => i.Date).Select(p => new
                {
                    Name = p.Name,
                    Price = p.Price,
                    Discount = p.Discount,
                    Evaluation = p.Evaluations.Sum(i => i.Value) / p.Evaluations.Count,
                    Img = string.Format("{0}/wwwroot/Uploads/ProductImg/{1}",
                        ServerName, p.Imgs.FirstOrDefault(i => i.ImgUrl.StartsWith("fav")).ImgUrl)
                }),

                Ads1 = string.Format("{0}/wwwroot/Uploads/ADS/{1}",
                        System.IO.Directory.GetCurrentDirectory(), ADS1?.Photo),

                Ads2 = string.Format("{0}/wwwroot/Uploads/ADS/{1}",
                        System.IO.Directory.GetCurrentDirectory(), ADS2?.Photo),
            };

            return new JsonResult(data);

        }


        [HttpGet("Category")]
        public JsonResult Category()
        {
            var ADS1 = _context.ADS.Where(i => i.Page == Page.الاصناف && i.Section == Section.الاول).FirstOrDefault();
            var ADS2 = _context.ADS.Where(i => i.Page == Page.الاصناف && i.Section == Section.الثاني).FirstOrDefault();

            string ServerName = System.IO.Directory.GetCurrentDirectory();

            var data = new
            {
                ProductsLast = _context.CategoryLev1s.OrderBy(i => i.Index).Select(p => new
                {
                    Name = p.Name,
                    Img = string.Format("{0}/wwwroot/Uploads/ProductImg/{1}",
                        ServerName, p.ImgUrl)
                }),

                Ads1 = string.Format("{0}/wwwroot/Uploads/ADS/{1}",
                        System.IO.Directory.GetCurrentDirectory(), ADS1?.Photo),

                Ads2 = string.Format("{0}/wwwroot/Uploads/ADS/{1}",
                        System.IO.Directory.GetCurrentDirectory(), ADS2?.Photo),
            };

            return new JsonResult(data);

        }


        [HttpGet("Shop")]
        public JsonResult Shop()
        {
            var ADS1 = _context.ADS.Where(i => i.Page == Page.التسوق && i.Section == Section.الاول).FirstOrDefault();
            var ADS2 = _context.ADS.Where(i => i.Page == Page.التسوق && i.Section == Section.الثاني).FirstOrDefault();

            string ServerName = System.IO.Directory.GetCurrentDirectory();

            var data = new
            {
                Products = _context.Products.OrderByDescending(i => i.Date).Select(p => new
                {
                    id = p.ID,
                    Name = p.Name,
                    Price = p.Price,
                    CategoryName = String.Format("{0} => {1}", p.GetCategoryLev2.CategoryLev1.Name, p.GetCategoryLev2.Name),
                    Discount = p.Discount,
                    Evaluation = p.Evaluations.Sum(i => i.Value) / p.Evaluations.Count,
                    Img = string.Format("{0}/wwwroot/Uploads/ProductImg/{1}",
                        ServerName, p.Imgs.FirstOrDefault(i => i.ImgUrl.StartsWith("fav")).ImgUrl)
                }),

                Ads1 = string.Format("{0}/wwwroot/Uploads/ADS/{1}",
                        System.IO.Directory.GetCurrentDirectory(), ADS1?.Photo),

                Ads2 = string.Format("{0}/wwwroot/Uploads/ADS/{1}",
                        System.IO.Directory.GetCurrentDirectory(), ADS2?.Photo),
            };

            return new JsonResult(data);

        }


        [HttpGet("Details")]
        public JsonResult Details(int id)
        {

            string ServerName = System.IO.Directory.GetCurrentDirectory();

            var data = new
            {
                Products = _context.Products.Select(p => new
                {
                    id = p.ID,
                    Name = p.Name,
                    Description = p.Description,
                    CategoryName = String.Format("{0} => {1}", p.GetCategoryLev2.CategoryLev1.Name, p.GetCategoryLev2.Name),
                    Price = p.Price,
                    Discount = p.Discount,
                    Qunatity = p.Qunatity,
                    Evaluation = p.Evaluations.Sum(i => i.Value) / p.Evaluations.Count,
                    Img = p.Imgs.Select(i=> new { 
                        url = ServerName+i.ImgUrl
                    }),
                    Property = p.ProdectProperties.Select(i => i.PropertyLev2.PropertyLev1.Name).ToList(),
                    PropertyValue = p.ProdectProperties.Select(i => i.PropertyLev2.Name).ToList(),

                }).FirstOrDefault(i=>i.id==id),
            };

            return new JsonResult(data);

        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,Roles ="مدير")]
        [HttpPost("AddToCart")]
        public async Task<IActionResult> AddToCart(OrderProduct orderProduct)
        {
            var GetUserName = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var UserId = User.FindFirst("uid")?.Value;

            if (ModelState.IsValid)
            {
                // to get user
                User user = await _userManager.FindByIdAsync(UserId);
                var CheckUserIsHasOreder = await _context.Orders.FirstOrDefaultAsync(i => i.UserID == UserId && i.State == State.incart);

                if (CheckUserIsHasOreder != null)
                {
                    var OrderProduct = new OrderProduct
                    {
                        OrderID = CheckUserIsHasOreder.ID,
                        ProductID = orderProduct.ProductID,
                        Quantity = orderProduct.Quantity,
                        Price = Convert.ToDouble(orderProduct.Price),
                    };

                    _context.Add(OrderProduct);
                    await _context.SaveChangesAsync();

                    return Ok();
                }

                else
                {
                    var Order = new Order
                    {
                        UserID = UserId,
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
                        Price = Convert.ToDouble(orderProduct.Price),
                    };

                    _context.Add(OrderProduct);
                    await _context.SaveChangesAsync();

                    return Ok();
                }

            }

            else
            {
                return BadRequest();
            }

        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("Cart")]
        public JsonResult Cart()
        {
            var ADS1 = _context.ADS.Where(i => i.Page == Page.السلة && i.Section == Section.الاول).FirstOrDefault();
            var ADS2 = _context.ADS.Where(i => i.Page == Page.السلة && i.Section == Section.الثاني).FirstOrDefault();

            string ServerName = System.IO.Directory.GetCurrentDirectory();

            // Get User 
            var UserId = User.FindFirst("uid")?.Value;

            var data = new
            {
                Products = _context.OrderProducts.Where(i => i.Order.UserID == UserId && i.Order.State == State.incart).Select(p => new
                {
                    id = p.ID,
                    Name = p.Product.Name,
                    Price = p.Price,
                    Qunatity = p.Quantity,
                    Img = string.Format("{0}/wwwroot/Uploads/ProductImg/{1}",
                          ServerName, p.Product.Imgs.FirstOrDefault(i => i.ImgUrl.StartsWith("fav")).ImgUrl),
                    OrderId = p.OrderID,

                }),
            };

            return new JsonResult(data);

        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("RemoveFromCart")]
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


            return Ok();
        }



        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("CompleteOrderGet")]
        public IActionResult CompleteOrder()
        {

            // For ADS in View
            var ADS1 = _context.ADS.Where(i => i.Page == Page.اكمال_الطلب && i.Section == Section.الاول).FirstOrDefault();
            var ADS2 = _context.ADS.Where(i => i.Page == Page.اكمال_الطلب && i.Section == Section.الثاني).FirstOrDefault();

            string ServerName = System.IO.Directory.GetCurrentDirectory();

            // to get user
            var UserId = User.FindFirst("uid")?.Value;

            //// To Get City 
            //ViewData["City"] = new SelectList(_context.Cities, "ID", "Name");

            var data = new
            {
                Products = _context.OrderProducts.Where(i => i.Order.UserID == UserId && i.Order.State == State.incart).Select(p => new
                {
                    id = p.ID,
                    Name = p.Product.Name,
                    Price = p.Price,
                    Qunatity = p.Quantity,
                    Img = string.Format("{0}/wwwroot/Uploads/ProductImg/{1}",
                          ServerName, p.Product.Imgs.FirstOrDefault(i => i.ImgUrl.StartsWith("fav")).ImgUrl),
                    OrderId = p.OrderID,

                }),
            };

            return new JsonResult(data);
        }



        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("CompleteOrderPost")]
        //id = OrderId -- NewProvinceID get id from Province Task
        public async Task<IActionResult> CompleteOrder(int? id, Order order, int NewProvinceID, IFormFile BankImg)
        {

            // For ADS in View
            var ADS1 = _context.ADS.Where(i => i.Page == Page.اكمال_الطلب && i.Section == Section.الاول).FirstOrDefault();
            var ADS2 = _context.ADS.Where(i => i.Page == Page.اكمال_الطلب && i.Section == Section.الثاني).FirstOrDefault();


            if (id == null)
            {
                return NotFound();
            }


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
                    Product.Qunatity -= item.Quantity;
                    _context.Products.Update(Product);
                    await _context.SaveChangesAsync();
                }

                return Ok();
            }

            return BadRequest();

        }





        [HttpGet("Province")]
        public async Task<JsonResult> Province()
        {

            var data = new
            {
                Province = await _context.Provinces.ToListAsync()
            };

            return new JsonResult(data);

        }


        [HttpGet("City")]
        public async Task<JsonResult> City()
        {

            var data = new
            {
                Cities = await _context.Cities.ToListAsync()
            };

            return new JsonResult(data);

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
    }
}