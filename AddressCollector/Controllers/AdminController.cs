using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AddressCollector.Data.Auth;
using AddressCollector.Helper;
using AddressCollector.Shared;
using AddressCollector.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AddressCollector.Controllers
{
    
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: /<controller>/
        [Authorize(Roles="Administrator")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles="Administrator, Ondernemer")]
        public IActionResult UserManagement()
        {
            var users = new List<ApplicationUser>();
            
            if (User.IsInRole("Administrator"))
            {
                users = _userManager.Users.ToList();
            }
            else
            {
                users = _userManager.Users.Where(x => x.OndernemerId == User.FindFirstValue(ClaimTypes.NameIdentifier)).ToList();
            }

            foreach (var user in users)
            {

                user.Rol = _userManager.GetRolesAsync(user).Result.FirstOrDefault();
            }

            return View(users.OrderBy(x => x.OndernemerId));
        }

        [Authorize(Roles="Administrator, Ondernemer")]
        public IActionResult AddUser()
        {
            return View();
        }


        [HttpPost]
        [Authorize(Roles="Administrator, Ondernemer")]
        public async Task<IActionResult> AddUser(AddUserViewModel addUserViewModel)
        {
            if (!ModelState.IsValid) return View(addUserViewModel);
            
            var user = new ApplicationUser()
            {
                UserName = addUserViewModel.Email,
                Email = addUserViewModel.Email,
                Naam = addUserViewModel.Naam
            };

            if (User.IsInRole(Constants.OndernemerUserRole))
            {
                user.OndernemerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            }

            var result = await _userManager.CreateAsync(user, addUserViewModel.Password);

            
            if (result.Succeeded)
            {
                //if user is in role 'Ondernemer' then also automatically add role 'Klant' to new user
                if (User.IsInRole(Constants.OndernemerUserRole))
                {
                    var rol = _roleManager.FindByNameAsync(Constants.KlantUserRole);
                    if (rol != null)
                    {
                        var userRoleViewModel = new UserRoleViewModel {UserId = user.Id, RoleId = rol.Result.Id};
                        //RedirectToAction("AddUserToRole", userRoleViewModel);
                        await AddUserToRol(userRoleViewModel);
                    }

                }

                return RedirectToAction("UserManagement", _userManager.Users);
            }

            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(addUserViewModel);
        }

        [Authorize(Roles="Administrator, Ondernemer")]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return RedirectToAction("UserManagement", _userManager.Users);

            return View(user);
        }

        [HttpPost]
        [Authorize(Roles="Administrator, Ondernemer")]
        public async Task<IActionResult> EditUser(string id, string naam, string email)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {
                user.Email = email;
                user.UserName = email;
                user.Naam = naam;
                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                    return RedirectToAction("UserManagement", _userManager.Users);

                ModelState.AddModelError("", "User not updated, something went wrong.");

                return View(user);
            }

            return RedirectToAction("UserManagement", _userManager.Users);
        }

        [HttpPost]
        [Authorize(Roles="Administrator, Ondernemer")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                    return RedirectToAction("UserManagement");
                else
                    ModelState.AddModelError("", "Something went wrong while deleting this user.");
            }
            else
            {
                ModelState.AddModelError("", "This user can't be found");
            }
            return View("UserManagement", _userManager.Users);
        }

        //Role management
        [Authorize(Roles="Administrator")]
        public IActionResult RoleManagement()
        {
            var roles = _roleManager.Roles;
            return View(roles);
        }

        [Authorize(Roles="Administrator")]
        public IActionResult AddNewRole() => View();

        [HttpPost]
        [Authorize(Roles="Administrator")]
        public async Task<IActionResult> AddNewRole(AddRoleViewModel addRoleViewModel)
        {

            if (!ModelState.IsValid) return View(addRoleViewModel);

            var role = new IdentityRole
            {
                Name = addRoleViewModel.RoleName
            };

            IdentityResult result = await _roleManager.CreateAsync(role);

            if (result.Succeeded)
            {
                return RedirectToAction("RoleManagement", _roleManager.Roles);
            }

            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(addRoleViewModel);
        }
        
        [Authorize(Roles="Administrator")]
        public async Task<IActionResult> EditRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
                return RedirectToAction("RoleManagement", _roleManager.Roles);

            var editRoleViewModel = new EditRoleViewModel
            {
                Id = role.Id,
                RoleName = role.Name,
                Users = new List<string>()
            };


            foreach (var user in _userManager.Users)
            {
                if (await _userManager.IsInRoleAsync(user, role.Name))
                    editRoleViewModel.Users.Add(user.Naam);
            }

            return View(editRoleViewModel);
        }

        [HttpPost]
        [Authorize(Roles="Administrator")]
        public async Task<IActionResult> EditRole(EditRoleViewModel editRoleViewModel)
        {
            var role = await _roleManager.FindByIdAsync(editRoleViewModel.Id);

            if (role != null)
            {
                role.Name = editRoleViewModel.RoleName;

                var result = await _roleManager.UpdateAsync(role);

                if (result.Succeeded)
                    return RedirectToAction("RoleManagement", _roleManager.Roles);

                ModelState.AddModelError("", "Role not updated, something went wrong.");

                return View(editRoleViewModel);
            }

            return RedirectToAction("RoleManagement", _roleManager.Roles);
        }

        [HttpPost]
        [Authorize(Roles="Administrator")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            IdentityRole role = await _roleManager.FindByIdAsync(id);
            if (role != null)
            {
                var result = await _roleManager.DeleteAsync(role);
                if (result.Succeeded)
                    return RedirectToAction("RoleManagement", _roleManager.Roles);
                ModelState.AddModelError("", "Something went wrong while deleting this role.");
            }
            else
            {
                ModelState.AddModelError("", "This role can't be found.");
            }
            return View("RoleManagement", _roleManager.Roles);
        }

        //Users in roles
        [Authorize(Roles="Administrator")]
        public async Task<IActionResult> AddUserToRole(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null)
                return RedirectToAction("RoleManagement", _roleManager.Roles);

            var addUserToRoleViewModel = new UserRoleViewModel { RoleId = role.Id, Naam = role.Name};

            foreach (var user in _userManager.Users)
            {
                if (!await _userManager.IsInRoleAsync(user, role.Name))
                {
                    addUserToRoleViewModel.Users.Add(user);
                }
            }

            return View(addUserToRoleViewModel);
        }

        [HttpPost]
        [Authorize(Roles="Administrator")]
        public async Task<IActionResult> AddUserToRole(UserRoleViewModel userRoleViewModel)
        {
            var user = await _userManager.FindByIdAsync(userRoleViewModel.UserId);
            var role = await _roleManager.FindByIdAsync(userRoleViewModel.RoleId);

            var result = await _userManager.AddToRoleAsync(user, role.Name);

            if (result.Succeeded)
            {
                return RedirectToAction("EditRole",  "Admin", new { id = role.Id});
            }
            
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(userRoleViewModel);
        }

        [Authorize(Roles="Administrator")]
        public async Task<IActionResult> DeleteUserFromRole(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null)
                return RedirectToAction("RoleManagement", _roleManager.Roles);

            var addUserToRoleViewModel = new UserRoleViewModel { RoleId = role.Id, Naam = role.Name };

            foreach (var user in _userManager.Users)
            {
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    addUserToRoleViewModel.Users.Add(user);
                }
            }

            return View(addUserToRoleViewModel);
        }

        [HttpPost]
        [Authorize(Roles="Administrator")]
        public async Task<IActionResult> DeleteUserFromRole(UserRoleViewModel userRoleViewModel)
        {
            var user = await _userManager.FindByIdAsync(userRoleViewModel.UserId);
            var role = await _roleManager.FindByIdAsync(userRoleViewModel.RoleId);

            var result = await _userManager.RemoveFromRoleAsync(user, role.Name);

            if (result.Succeeded)
            {
                return RedirectToAction("EditRole",  "Admin", new { id = role.Id});
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(userRoleViewModel);
        }

        [Authorize(Roles="Administrator, Ondernemer")]
        private async Task<IActionResult> AddUserToRol(UserRoleViewModel userRoleViewModel)
        {
            var user = await _userManager.FindByIdAsync(userRoleViewModel.UserId);
            var role = await _roleManager.FindByIdAsync(userRoleViewModel.RoleId);

            var result = await _userManager.AddToRoleAsync(user, role.Name);

            if (result.Succeeded)
            {
                return RedirectToAction("UserManagement", _userManager.Users);
            }
            
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(userRoleViewModel);
        }
    }
}