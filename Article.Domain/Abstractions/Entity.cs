using System.ComponentModel.DataAnnotations;

namespace Article.Domain.Abstractions
{
    public class Entity
    {
        [Key]
        public Guid Id { get; set; }
    }
}
