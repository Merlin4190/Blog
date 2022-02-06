using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecaBlog.Models.DTO
{
    public class LoginResponseDto
    {
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string Token { get; set; }
        public IList<string> Role { get; set; } = new List<string>();
    }
}
