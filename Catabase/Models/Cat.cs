using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Catabase.Models
{
    public enum Sex
    {
        M,F,I
    }
    public class Cat
    {
        public int CatId { get; set; }
        [Required]
        [MaxLength(40)]
        [RegularExpression(@"[ \w\[ \]`!@#$%\^&*()={}:;<>+.'-]*", ErrorMessage = "Name contains invalid characters")]
        public string Name { get; set; }
        [MaxLength(20)]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Please only use characters A-Z (spaces are allowed)")]
        public string? Breed { get; set; }
        [Required]
        public Sex Sex { get; set; }
        [Required]
        [MaxLength(20)]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Please only use characters A-Z (spaces are allowed)")]
        public string Colour { get; set; }
        [MaxLength(200)]
        [RegularExpression(@"[ \w\[ \]`!@#$%\^&*()={}:;<>+.'-]*", ErrorMessage = "Text contains invalid characters")]
        public string? Bio { get; set; }

        public DateTime? DateOfBirth { get; set; }
        [Required]
        public string OwnerID { get; set; }
        public CatabaseUser Owner { get; set; }
    }
}
