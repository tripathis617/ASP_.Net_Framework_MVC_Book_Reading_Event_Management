using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BookReadingEventManagement.Models
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }
        public string CommentText { get; set; }
        [ForeignKey("EventIdFoerignid")]
        public string EventId { get; set; }
        public Events EventIdFoerignid { get; set; }
    }
}
