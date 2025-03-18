using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Article.Domain.HelpModels.ResubmitArticleRequestModel
{
    public class ResubmitArticleRequest
    {
        [Required]
        public Guid ArticleId { get; set; }

        [Required]
        public IFormFile File { get; set; }
    }
}
