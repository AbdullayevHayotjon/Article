using Article.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Article.Domain.HelpModels.TempUserModel
{
    [Table("TempUsers", Schema = "HelpSchema")]
    public class TempUser : Entity
    {
        public string Firstname { get; set; } = string.Empty;
        public string Lastname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string HashedPassword { get; set; } = string.Empty;
        public int VerificationCode { get; set; } = 0;
        public DateTime ExpirationTime { get; set; } = DateTime.UtcNow.AddMinutes(2);
    }
}
