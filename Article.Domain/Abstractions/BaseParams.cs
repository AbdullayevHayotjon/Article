using System.ComponentModel.DataAnnotations;

namespace Article.Domain.Abstractions
{
    public abstract class BaseParams : Entity
    {
        [Required]
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime UpdateDate { get; set; } = DateTime.UtcNow;

        public DateTime? DeleteDate { get; set; }

        [Required]
        public bool IsDeleted { get; set; } = false;
    }
}
