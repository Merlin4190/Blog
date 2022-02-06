using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecaBlog.Models.DTO
{
   public class PasswordResetDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Token { get; set; }
        [Required]
        [StringLength(40, MinimumLength = 5)]
        public string NewPassword { get; set; }

    }
}
