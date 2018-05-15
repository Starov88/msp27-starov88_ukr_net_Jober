using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Jober.Data;
using Jober.Models;
using Jober.Models.HomeViewModels;
using Microsoft.Extensions.Configuration;
using Jober.Services.Informer;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using Jober.Hubs;

namespace Jober.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        
        //private readonly IConfiguration _configuration;

        public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            //_configuration = configuration;
        }

        public async Task<IActionResult> Index(int category = 0)
        {
            List<Category> allCategories = await _context.Categories.ToListAsync();
            if (allCategories != null)
            {
                if (category > 0)
                {
                    ViewData["categoryId"] = category;
                    return View(allCategories.Where(c => c.ParentId == category));
                }
                return View(allCategories.Where(c => c.ParentId == null));
            }
            return RedirectToAction(nameof(Error));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Order(int category = 0)
        {
            if (category > 0)
            {
                Category selectedСategory = await _context.Categories.Where(c => c.Id == category).Include(s => s.Services).SingleOrDefaultAsync();

                if (selectedСategory != null)
                {
                    if (selectedСategory.Services != null && selectedСategory.Services.Count > 0)
                    {
                        UserInformer informer = new UserInformer(User, _context, _userManager);
                        var user = informer.GetUser();
                        OrderViewModel model = new OrderViewModel();
                        model.SelectedCategoryId = category;
                        model.SelectedCategoryName = await _context.Categories.Where(c => c.Id == category).Select(c => c.Name).FirstOrDefaultAsync();
                        model.Services = selectedСategory.Services.OrderBy(s => s.Name).ToList();
                        model.City = await _context.Cities.Where(c => c.Id == user.CityId).Include(c => c.Districts).FirstOrDefaultAsync();
                        if (model.City == null)
                            RedirectToAction(nameof(Error));

                        return View(model);
                    }
                }
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Order(OrderViewModel model)
        {
            Category selectedСategory = await _context.Categories.Where(c => c.Id == model.SelectedCategoryId).Include(s => s.Services).SingleOrDefaultAsync();
            if (selectedСategory == null || selectedСategory.Services == null || selectedСategory.Services.Count == 0)
                return View();

            if (model.QuantityString?.Count > 0)
            {
                model.Quantity = new Dictionary<int, byte>();
                foreach (var item in model.QuantityString)
                {
                    string[] splitData = item.Split(new char[] { '-' });
                    if (splitData.Length == 2)
                    {
                        int key;
                        byte value;
                        if (int.TryParse(splitData[0], out key) && byte.TryParse(splitData[1], out value))
                        {
                            model.Quantity.Add(key, value);
                        }
                    }
                }
            }

            if (!ModelState.IsValid)
            {
                if (model.SelectedCategoryId == 0)
                    return RedirectToAction(nameof(Index));
                Response.HttpContext.Items.Add("category", model.SelectedCategoryId);
                
                model.Services = selectedСategory.Services.OrderBy(s => s.Name).ToList();
                model.City = await _context.Cities.Where(c => c.Id == model.CityId).Include(c => c.Districts).FirstOrDefaultAsync();
                model.UserId = _userManager.GetUserId(User);

                return View(model);
            }

            float totalCost = 0;
            foreach (var item in model.ServiceIds)
            {
                totalCost += selectedСategory.Services.Where(s => s.Id == item).Select(s => s.Price).FirstOrDefault() * model.Quantity[item];
            }

            model.Date = model.Date.AddHours(model.Time);

            Order newOrder = new Order
            {
                TotalCost = totalCost,
                CategoryId = model.SelectedCategoryId,
                Location = new Location
                {
                    CityId = model.CityId,
                    DistrictId = model.DistrictId,
                    Address = model.Address,
                    ZipCode = null,
                    UserId = _userManager.GetUserId(User)
                },
                Date = model.Date,
                Number = "NoNumber",
                UserId = _userManager.GetUserId(User),
                StatusId = 1,
                Description = model.Description,
                OrderDetails = new List<OrderDetail>()
            };

            OrderDetail orderDetail;
            foreach (var servId in model.ServiceIds)
            {
                orderDetail = new OrderDetail
                {
                    Quantity = model.Quantity[servId],
                    ServiceId = servId,
                    OrderId = newOrder.Id
                };

                newOrder.OrderDetails.Add(orderDetail);
            }

            newOrder = _context.Add(newOrder).Entity;
            _context.SaveChanges();

            //OrderHub hub = new OrderHub();
            //await hub.Send(newOrder.Description);


            //Order newOrder = new Order
            //{
            //    C
            //}

            //var user = await _userManager.GetUserAsync(User);
            //if (user == null)
            //{
            //    throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            //}

            //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            //var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
            //var email = user.Email;
            //await _emailSender.SendEmailConfirmationAsync(email, callbackUrl);

            //StatusMessage = "Verification email sent. Please check your email.";

            return RedirectToAction(nameof(Index));
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Error(ErrorViewModel error)
        {
            return View(error);
        }
    }
}
