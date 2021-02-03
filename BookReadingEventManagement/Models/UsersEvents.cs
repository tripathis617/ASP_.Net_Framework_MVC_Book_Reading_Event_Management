using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookReadingEventManagement.Models
{
    public class UsersEvents
    {
        public string UserId { get; set; }
        public ApplicationUser Users { get; set; }
        public string EventId { get; set; }
        public Events Events { get; set; }
    }
}
