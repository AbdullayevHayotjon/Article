
using Article.Domain.MainModels.UserModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Article.Domain.HelpModels.Specialization_Model
{
    public class Specialization
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string WorkerCategory { get; set; }

        [ForeignKey("UserId")]
        public User user { get; set; }
    }

}
