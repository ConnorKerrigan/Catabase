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
        public string Name { get; set; }
        [Required]
        [MaxLength(20)]
        public string Breed { get; set; }
        [Required]
        public Sex Sex { get; set; }
        [Required]
        [MaxLength(20)]
        public string Colour { get; set; }
        [MaxLength(200)]
        public string? Bio { get; set; }

        public DateTime? DateOfBirth { get; set; }
        [Required]
        public string OwnerID { get; set; }
        public CatabaseUser Owner { get; set; }
    }
}
