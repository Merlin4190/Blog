using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecaBlog.Models.DTO
{
  public  class PhotoToUploadDto
    {
        [Required]
        public IFormFile Photo { get; set; }
    }
}
