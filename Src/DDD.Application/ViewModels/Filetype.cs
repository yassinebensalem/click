using Microsoft.AspNetCore.Http;

namespace DDD.Application.ViewModels
{
    public class Filetype
    {
        public IFormFile PDFPath { get; set; }
        public IFormFile CoverPath { get; set; }
    }
}
