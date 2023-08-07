using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Catabase.Models
{
    public class Post
    {
        public int PostId { get; set; }
        [MaxLength(100)]
        [RegularExpression(@"[\w\[ \]`!@#$%\^&*()={}:;<>+'-]*", ErrorMessage = "Text contains invalid characters")]
        public string? Caption { get; set; }
        public string ImageUrl { get; set; }
        public int LikeCount { get; set; }
        public DateTime PostTime { get; set; }
        public string CatabaseUserId { get; set; }
        public CatabaseUser CatabaseUser { get; set; }
        public ICollection<PostAttribution> PostAttributions { get; set; }
        public ICollection<Like>? Likes { get; set; }
        public ICollection<Comment> Comments { get; set; }


    }
}
