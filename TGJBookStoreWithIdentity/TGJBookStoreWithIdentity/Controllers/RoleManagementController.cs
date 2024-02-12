using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


//ALL COMMENTS BY JONATHAN SIVANANTHAN 9NOV


namespace TGJBookStoreWithIdentity.Controllers
{

    //This controller is to: allow the Admin to Add and View 'Roles'

    [Authorize(Roles = "Admin,Auditor,Developer")]
    public class RoleManagementController : Controller
    {
        //create rolemanager instance
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleManagementController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }


        //get a list of roles
        public IActionResult Index()
        {
            var roles = _roleManager.Roles.ToList();
            return View(roles);
        }


        //new async method to add a role 
        [HttpPost]
        public async Task<IActionResult> AddNewRole(string role)
        {
            if (role != null)
            {
                await _roleManager.CreateAsync(new IdentityRole(role.Trim())); //get the new role as type IdentityRole and trim whitespace
            }
            return RedirectToAction("Index"); //refresh to index page after role added
        }

    }
}
