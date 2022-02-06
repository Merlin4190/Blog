﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecaBlog.Models
{
    public class UserComment : BaseEntity
    {
        [Required]
        public string CommenterId { get; set; }
        [Required]
        public string TopicId { get; set; }
        [Required]
        public string CommentText { get; set; }
        public int Vote { get; set; }

        // navigation props
        [ForeignKey("TopicId")]
        public ArticleTopic ArticleTopic { get; set; }
        
        [ForeignKey("CommenterId")]
        public User User { get; set; }
    }
}
