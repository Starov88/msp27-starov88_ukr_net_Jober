using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Jober.Models;
using Jober.Models.AdminViewModels;
using Microsoft.AspNetCore.Authorization;
using Jober.Data;

namespace Jober.Areas.Controllers
{
    [Authorize(Roles = "admin")]
    [Area("Admin")]
    public class RoleController : Controller
    {
        RoleManager<IdentityRole> _roleManager;
        UserManager<ApplicationUser> _userManager;
        ApplicationDbContext _context;

        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;
        }

        public IActionResult Index() => View(_roleManager.Roles.ToList());

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                IdentityResult result = await _roleManager.CreateAsync(new IdentityRole(name));
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(name);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            IdentityRole role = await _roleManager.FindByIdAsync(id);
            if (role != null)
            {
                IdentityResult result = await _roleManager.DeleteAsync(role);
            }
            return RedirectToAction("Index");
        }

        public IActionResult UserList() => View(_userManager.Users.ToList());

        public async Task<IActionResult> Edit(string userId)
        {
            // получаем пользователя
            ApplicationUser user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                // получем список ролей пользователя
                var userRoles = await _userManager.GetRolesAsync(user);
                var allRoles = _roleManager.Roles.ToList();
                ChangeRoleViewModel model = new ChangeRoleViewModel
                {
                    UserId = user.Id,
                    UserEmail = user.Email,
                    UserRoles = userRoles,
                    AllRoles = allRoles
                };
                return View(model);
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string userId, List<string> roles)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var allRoles = _roleManager.Roles.ToList();
                
                var addedRoles = roles.Except(userRoles);
                if (addedRoles.Contains("jober"))
                {
                    user.Balance += 100;
                    if (user.WorkerId == null)
                    {
                        user.Worker = new Worker
                        {
                            UserId = user.Id
                        };
                    }
                    else
                    {
                        Worker worker = _context.Workers.Where(w => w.UserId == user.Id).FirstOrDefault();
                        if (worker != null)
                        {
                            worker.IsActive = true;
                        }
                    }
                    _context.Update(user);
                    _context.SaveChanges();
                }
               
                var removedRoles = userRoles.Except(roles);
                if (removedRoles.Contains("jober"))
                {
                    Worker worker = _context.Workers.Where(w => w.UserId == user.Id).FirstOrDefault();

                    if(worker != null)
                    {
                        worker.IsActive = false;
                        _context.Update(worker);
                        _context.SaveChanges();
                    }
                }

                await _userManager.AddToRolesAsync(user, addedRoles);

                await _userManager.RemoveFromRolesAsync(user, removedRoles);

                return RedirectToAction("UserList");
            }

            return NotFound();
        }
    }
}