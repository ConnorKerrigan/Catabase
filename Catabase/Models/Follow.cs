using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Catabase.Models
{
    public class Follow
    {
        public int FollowId { get; set; }
        [Required]
        public CatabaseUser User { get; set; }
        [Required]
        public int ProfileId { get; set; }
        public Profile Profile { get; set; }
    }
}
