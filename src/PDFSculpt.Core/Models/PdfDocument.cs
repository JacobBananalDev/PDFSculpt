namespace PDFSculpt.Core.Models
{
    public class PdfDocument
    {
        public string FilePath { get; set; } = string.Empty;

        public int PageCount { get; set; }

        public List<PdfPage> Pages { get; set; } = new();
    }
}
