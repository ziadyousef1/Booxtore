using Booxtore.Domain.Models;

namespace Booxtore.Presentation.ViewModels
{
    public class BookReaderViewModel
    {
        public Book Book { get; set; } = null!;
        public string PdfPath { get; set; } = string.Empty;
    }
}