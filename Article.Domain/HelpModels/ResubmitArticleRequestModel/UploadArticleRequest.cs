using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Article.Domain.HelpModels.ResubmitArticleRequestModel
{
    public class UploadArticleRequest
    {
        public IFormFile File { get; set; } = null!;
        public string Title { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
    }
}
