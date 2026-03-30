using PDFSculpt.Core.Models;

namespace PDFSculpt.Core.Services
{
    public interface IPdfService
    {
        Task<PdfDocument> LoadDocumentAsync(string filePath);

        Task<byte[]> RenderPageAsync(PdfPage page, double scale = 1.0);
    }
}
