using System.ComponentModel.DataAnnotations;

namespace Article.Domain.Abstractions
{
    public abstract class Entity
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
    }
}
