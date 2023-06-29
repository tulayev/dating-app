using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class Photo : BaseEntity
    {
        public string Url { get; set; }

        public bool IsMain { get; set; }

        public string PublicId { get; set; }

        public int UserId { get; set; }

        public AppUser User { get; set; }
    }
}