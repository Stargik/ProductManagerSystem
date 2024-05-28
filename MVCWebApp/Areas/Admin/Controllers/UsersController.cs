using System.Data;
using BLL.Interfaces;
using BLL.Models;
using BLL.Services;
using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCWebApp.Areas.Admin.Models;
using MVCWebApp.Areas.Identity.Configuration;
using MVCWebApp.Areas.Identity.Models;

namespace MVCWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class UsersController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<User> userManager;

        public UsersController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        // GET: Users
        public async Task<ActionResult> Index()
        {
            var users = await userManager.Users.ToListAsync();
            return View(users);
        }

        // GET: Users/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var user = await userManager.FindByIdAsync(id);

            if (user is null)
            {
                return NotFound();

            }

            var userRoles = await userManager.GetRolesAsync(user);
            var allRoles = roleManager.Roles;

            UserViewModel userViewModel = new UserViewModel
            {
                User = user,
                UserRoles = userRoles,
                AllRoles = allRoles
            };

            return View(userViewModel);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            var allRoles = roleManager.Roles;

            ViewData["roles"] = allRoles;
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RegisterUserViewModel registerUserViewModel)
        {
            if ((await userManager.FindByEmailAsync(registerUserViewModel.Email)) is null)
            {
                User user = new User
                {
                    UserName = registerUserViewModel.Email,
                    Email = registerUserViewModel.Email,
                };

                if (registerUserViewModel.Name is not null)
                {
                    user.Name = registerUserViewModel.Name;
                }

                IdentityResult result = await userManager.CreateAsync(user, registerUserViewModel.Password);
                if (result.Succeeded)
                {
                    if (registerUserViewModel.UserRoles.Any())
                    {
                        await userManager.AddToRolesAsync(user, registerUserViewModel.UserRoles);
                    }
                    return RedirectToAction(nameof(Index));

                }
            }
            if ((await userManager.FindByEmailAsync(registerUserViewModel.Email)) is not null || (registerUserViewModel.Name is not null && (await userManager.FindByNameAsync(registerUserViewModel.Name)) is not null))
            {
                TempData["ErrorMessage"] = "Помилка. Користувач з цією поштою та/або логіном вже існує.";
            }
            else
            {
                TempData["ErrorMessage"] = "Помилка. Перевірте коректність даних.";
            }
                

            var allRoles = roleManager.Roles;
            ViewData["roles"] = allRoles;
            return View(registerUserViewModel);
        }

        // GET: Users/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var user = await userManager.FindByIdAsync(id);

            if (user is null)
            {
                return NotFound();

            }

            var userRoles = await userManager.GetRolesAsync(user);
            var allRoles = roleManager.Roles;

            UserViewModel userViewModel = new UserViewModel
            {
                User = user,
                UserRoles = userRoles,
                AllRoles = allRoles
            };

            return View(userViewModel);
        }

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, UserViewModel userViewModel)
        {
            if (id != userViewModel.User.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var user = await userManager.FindByIdAsync(id);

                if (user is null)
                {
                    return NotFound();

                }

                var userRoles = await userManager.GetRolesAsync(user);

                var allRoles = roleManager.Roles.ToList();

                var addedRoles = userViewModel.UserRoles.Except(userRoles);

                var removedRoles = userRoles.Except(userViewModel.UserRoles);

                if (removedRoles.Contains(RoleSettings.AdminRole) && (await userManager.GetUsersInRoleAsync(RoleSettings.AdminRole)).Count <= 1)
                {
                    TempData["ErrorMessage"] = "Помилка. Повинен існувати хоча б один адміністратор.";
                    return RedirectToAction(nameof(Index));
                }


                if (user.Email != userViewModel.User.Email)
                {
                    if ((await userManager.FindByEmailAsync(userViewModel.User.Email)) is not null)
                    {
                        TempData["ErrorMessage"] = "Помилка. Користувач з цією поштою вже існує.";
                        return View(userViewModel);
                    }
                    user.Email = userViewModel.User.Email;
                }

                if (userViewModel.User.Name != user.Name)
                {
                    user.Name = userViewModel.User.Name;
                }

                await userManager.UpdateAsync(user);

                await userManager.AddToRolesAsync(user, addedRoles);

                await userManager.RemoveFromRolesAsync(user, removedRoles);

                return RedirectToAction(nameof(Index));
            }
            return View(userViewModel);
        }

        // GET: Users/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var user = await userManager.FindByIdAsync(id);

            if (user is null)
            {
                return NotFound();

            }

            var userRoles = await userManager.GetRolesAsync(user);
            var allRoles = roleManager.Roles;

            UserViewModel userViewModel = new UserViewModel
            {
                User = user,
                UserRoles = userRoles,
                AllRoles = allRoles
            };

            return View(userViewModel);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var user = await userManager.FindByIdAsync(id);

            if (user is null)
            {
                return NotFound();

            }

            var userRoles = await userManager.GetRolesAsync(user);

            if (userRoles.Contains(RoleSettings.AdminRole) && (await userManager.GetUsersInRoleAsync(RoleSettings.AdminRole)).Count <= 1)
            {
                TempData["ErrorMessage"] = "Помилка. Повинен існувати хоча б один адміністратор.";
                return RedirectToAction(nameof(Index));
            }

            await userManager.DeleteAsync(user);

            return RedirectToAction(nameof(Index));

        }

    }
}
