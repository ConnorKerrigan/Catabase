using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Catabase.Models
{
    public class Profile
    {
        public int ProfileId { get; set; }
        public string? ProfilePicPath { get; set; }

        [ForeignKey("UserId")]
        public string UserId { get; set; }
        public CatabaseUser User { get; set; }
        public ICollection<Follow>? Follows { get; set; }
    }
}
