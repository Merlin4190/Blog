using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecaBlog.Models
{
    public class Notification : BaseEntity
    {
        [Required]
        public string ActivityId { get; set; }
        [Required]
        public string NoticeText { get; set; }
    }
}
