
//all comments made by Jonathan 9Nov

namespace TGJBookStoreWithIdentity.ViewModels
{
    //the viewmodel for changing roles by User
    public class UserRoleManagerViewModel
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public bool isSelected { get; set; } //whether the role is ticked or not 

    }
}
