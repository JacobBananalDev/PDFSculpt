using PDFSculpt.Core.Models;
using PDFSculpt.Core.Services;

namespace PDFSculpt.Infrastructure.Services
{
    public class PdfService : IPdfService
    {
        public Task<PdfDocument> LoadDocumentAsync(string filePath)
        {
            var document = new PdfDocument
            {
                FilePath = filePath,
                PageCount = 3,
            };

            for (int i = 1; i <= document.PageCount; i++)
            {
                document.Pages.Add(new PdfPage
                {
                    PageNumber = i,
                    Width = 612, // 8.5 inches at 72 DPI
                    Height = 792 // 11 inches at 72 DPI
                });
            }

            return Task.FromResult(document);
        }

        public Task<byte[]> RenderPageAsync(PdfPage page, double scale = 1)
        {
            // Return empty byte array for now (stub)
            return Task.FromResult(new byte[0]);
        }
    }
}
