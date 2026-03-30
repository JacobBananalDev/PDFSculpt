namespace PDFSculpt.Core.Models
{
    public class PdfPage
    {
        public int PageNumber { get; set; }

        public int DisplayPageNumber => PageNumber + 1;

        public double Width { get; set; }

        public double Height { get; set; }

        public byte[]? ImageData { get; set; }
    }
}
