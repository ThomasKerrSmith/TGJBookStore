namespace TGJBookStoreWithIdentity.ViewModels
{
    //this viewmodel will be used get  user details and their allocated roles as a ienumerabe interface (list) - J.S 9NOV
    public class UserRoleViewModel
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
