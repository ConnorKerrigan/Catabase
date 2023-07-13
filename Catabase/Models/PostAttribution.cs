using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Catabase.Models
{
    public class PostAttribution
    {
        public int PostAttributionId { get; set; }
        [Required]
        public Post Post { get; set; }
        [Required]
        public Cat Cat { get; set; }
    }
}
