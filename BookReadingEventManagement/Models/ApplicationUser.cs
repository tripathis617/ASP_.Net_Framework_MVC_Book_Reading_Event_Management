using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookReadingEventManagement.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string UserFullName { get; set; }
        public IList<UsersEvents> UsersEventses { get; set; }
    }
}
