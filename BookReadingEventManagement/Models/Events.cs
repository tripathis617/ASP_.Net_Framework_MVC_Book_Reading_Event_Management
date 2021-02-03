using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookReadingEventManagement.Models
{
    public class Events
    {
        [Key]
        public string Id { get; set; }
        public string UserEmail { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public DateTime DateTime { get; set; }
        [Required]
        public string Location { get; set; }
        public string StartTime { get; set; }
        public string Type { get; set; }
        public int DurationinHours { get; set; }
        public string Description { get; set; }
        public string OtherDetails { get; set; }
        public string TotalInviteByEmails { get; set; }

        public IList<UsersEvents> UsersEventses { get; set; }
    }
}
