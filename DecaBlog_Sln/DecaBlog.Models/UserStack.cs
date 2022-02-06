﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecaBlog.Models
{
    public class UserStack
    {
        public string Id { get; set; }
        public string StackId { get; set; }
        public Stack Stack { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }

        public UserStack()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
