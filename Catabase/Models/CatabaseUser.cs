using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Catabase.Models
{
    public class CatabaseUser : IdentityUser
    {

        [Required]
        public DateTime DateCreated { get; set; }
        
        public Profile? Profile { get; set; }
        public ICollection<Post>? Posts { get; set; }
        public ICollection<Cat>? Cats { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<Like>? Likes { get; set; }
        public ICollection<Follow>? Follows { get; set; }
    }
}
