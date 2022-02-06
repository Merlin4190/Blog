using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecaBlog.Models.DTO
{
    public class AddArticleTopicDTO
    {
        [Required]
        public string Topic { get; set; }
        [Required]
        public string TopicId { get; set; }
        [Required]
        [StringLength(150, ErrorMessage = "Maximum is 150 Character")]
        public string Abstract { get; set; }
        public string PhotoUrl { get; set; }
        public string PublicId { get; set; }
        [Required]
        [StringLength(150, ErrorMessage = "Maximum is 150 Character")]
        public string Category { get; set; }
        [Required]
        [StringLength(150, ErrorMessage = "Maximum is 150 Characters")]
        public string AuthorId { get; set; }
    }
}
