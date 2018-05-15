using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Jober.Data;
using Jober.Models;
using Jober.Models.UserViewModels;
using Jober.Services.Informer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Jober.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public UserController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            UserInformer informer = new UserInformer(User, _context, _userManager);

            var model = await informer.GetUserPageDataAsync();
            
            return View(model);
        }

        public async Task<IActionResult> IndexWrite()
        {
            UserInformer informer = new UserInformer(User, _context, _userManager);

            var model = await informer.GetUserPageDataAsync();
            
            return PartialView("_UserWritePartial", model);
        }

        public async Task<IActionResult> IndexRead()
        {
            UserInformer informer = new UserInformer(User, _context, _userManager);

            var model = await informer.GetUserPageDataAsync();

            return PartialView("_UserReadPartial", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveUser(UserPageViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Cities = await _context.Cities.ToListAsync();
                model.City = model.Cities.Where(c => c.Id == model.CityId).Select(c => c.Name).SingleOrDefault();
                
                return View(model);
            }

            UserInformer informer = new UserInformer(User, _context, _userManager);
            var user = await informer.GetUserWorkerAsync();
            if (user != null)
            {
                user.UserName = model.Name;
                user.PhoneNumber = model.PhoneNumber;
                user.CityId = model.CityId;

                WorkerSetting workerSettings = user.Worker.WorkerSettingJson.ParseToWorkerSetting();
                if(workerSettings.CityId != model.CityId)
                {
                    workerSettings.CityId = model.CityId;
                    workerSettings.DistrictIds = new List<int>();
                    user.Worker.WorkerSettingJson = workerSettings.ToJSON();
                }
                
                _context.Entry(user).State = EntityState.Modified;
                _context.Entry(user.Worker).State = EntityState.Modified;
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    
                }
            }
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "jober")]
        public async Task<IActionResult> Office()
        {
            UserInformer informer = new UserInformer(User, _context, _userManager);
            WorkerPageViewModel workerPageViewModel = await informer.GetWorkerPageDataAsync(UserInformerOption.New);
            if (workerPageViewModel == null)
                return NotFound();
            return View(workerPageViewModel);
        }

        [Authorize(Roles = "jober")]
        public async Task<IActionResult> WorkerNewOrders()
        {
            UserInformer informer = new UserInformer(User, _context, _userManager);
            WorkerPageViewModel workerPageViewModel = await informer.GetWorkerPageDataAsync(UserInformerOption.New);
            if (workerPageViewModel == null)
                return NotFound();
            return PartialView("_WorkerNewOrdersListPartial", workerPageViewModel);
        }

        [Authorize(Roles = "jober")]
        public async Task<IActionResult> WorkerOrders()
        {
            UserInformer informer = new UserInformer(User, _context, _userManager);
            WorkerPageViewModel workerPageViewModel = await informer.GetWorkerPageDataAsync(UserInformerOption.Work);
            if (workerPageViewModel == null)
                return NotFound();
            return PartialView("_WorkerOrdersListPartial", workerPageViewModel);
        }

        [Authorize(Roles = "jober")]
        public async Task<IActionResult> WorkerSettings()
        {
            UserInformer informer = new UserInformer(User, _context, _userManager);
            WorkerPageViewModel workerPageViewModel = await informer.GetWorkerPageDataAsync(UserInformerOption.Settings);
            if (workerPageViewModel == null)
                return NotFound();
            return PartialView("_WorkerSettingsPartial", workerPageViewModel);
        }
    }
}