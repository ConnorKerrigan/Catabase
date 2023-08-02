using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Catabase.Models
{
    public class Like
    {
        public int LikeId { get; set; }
        [Required]
        public Post Post { get; set; }
        [Required]
        public string UserId { get; set; }
        public CatabaseUser User { get; set; }

    }
}
