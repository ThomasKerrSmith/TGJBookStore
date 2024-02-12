using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TGJBookStoreWithIdentity.Enums;
using TGJBookStoreWithIdentity.Models;
using TGJBookStoreWithIdentity.ViewModels;

//ALL COMMENTS BY J.S 9NOV

namespace TGJBookStoreWithIdentity.Controllers
{

    [Authorize(Roles = "Admin,Auditor,Developer")]
    public class UserRoleController : Controller
    {
        private readonly UserManager<AppUser> _userManager; //object to call UserManager Class methods which will help working with Users
        private readonly RoleManager<IdentityRole> _roleManager; //object to call RoleManager Class methods which will help working with Roles

        //adding them to constructor
        public UserRoleController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        //get all users and roles per user
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync(); //get users using the userManager Class method

            var userRolesViewModel = new List<UserRoleViewModel>(); //initialise a list of type viewmodel to show results

            foreach (AppUser user in users)
            {
                var thisViewModel = new UserRoleViewModel();
                thisViewModel.UserId = user.Id;
                thisViewModel.Email = user.Email;
                thisViewModel.FirstName = user.FirstName;
                thisViewModel.LastName = user.LastName;
                thisViewModel.Roles = await GetUserRoles(user);

                userRolesViewModel.Add(thisViewModel); //pass the list to view
            }
            return View(userRolesViewModel);
        }


        //method to get the list of roles for the given user in the Index action link
        private async Task<List<string>> GetUserRoles(AppUser user)
        {
            return new List<string>(await _userManager.GetRolesAsync(user));
        }


        //Here are TWO "ChangeRole" action methods. One is GET and one is POST
        //GET Changerole is to get the roles for the selected user
        //POST Changerole will assign the changed roles to the user

        [HttpGet]
        public async Task<IActionResult> ChangeRoles(string userId)
        {
            ViewBag.userId = userId;
            var user = await _userManager.FindByIdAsync(userId); //check if the user exists first 

            if (user == null) //handle error if user is null
            {
                ViewBag.ErrorMessage = $"UserID = {userId} not found";
                return View("NotFound");
            }

            ViewBag.UserName = user.UserName; //assign matching username to Id

            var model = new List<UserRoleManagerViewModel>(); //initialise a viewmodel to load user details and list of their roles

            foreach (var role in _roleManager.Roles) //search all our roles
            {

                //pass the fetched viewmodel data here
                var userRolesViewModel = new UserRoleManagerViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };

                if (await _userManager.IsInRoleAsync(user, role.Name)) //if the user is in a role tick the box
                {
                    userRolesViewModel.isSelected = true;
                }
                else
                {
                    userRolesViewModel.isSelected = false; 
                }

                model.Add(userRolesViewModel); //add the viewmodel obj to the List<viewmodel> and pass into view(form)
            }
            return View(model);
        }

        //POST method is for assigning the changed roles to the user on button click
        //Onclick it will pass the model obj from the GET method above and the userID
        [HttpPost]
        public async Task<IActionResult> ChangeRoles(List<UserRoleManagerViewModel> model, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId); //make obj of the user we want to apply changes to

            if (user == null)
            {
                return View();
            }

            var currentRoles = await _userManager.GetRolesAsync(user); //get the users current roles as a list

            //remove user from unchecked roles
            var changes = await _userManager.RemoveFromRolesAsync(user, currentRoles);

            //add user to checked roles
            changes = await _userManager.AddToRolesAsync(user, model.Where(s => s.isSelected).Select(r => r.RoleName));

            return RedirectToAction("Index"); //go back to list of all users and roles
        }
    }
}
