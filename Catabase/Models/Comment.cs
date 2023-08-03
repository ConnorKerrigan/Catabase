using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Catabase.Models
{
    public class Comment
    {
        public int CommentId { get; set; }
        [Required]
        public int PostId { get; set; }
        public Post Post { get; set; }
        [Required]
        public string UserId { get; set; }
        public CatabaseUser User { get; set; }
        [Required]
        public string CommentContent { get; set; }
    }
}
