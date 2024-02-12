using Microsoft.AspNetCore.Identity;

namespace TGJBookStoreWithIdentity.Models
{
    //this class extends the main identityuser class to add some extra fields about the user - JS 9NOV

    public class AppUser : IdentityUser 
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }    

    }
}
