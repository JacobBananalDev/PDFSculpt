using PDFiumSharp;
using PDFSculpt.Core.Services;

namespace PDFSculpt.Infrastructure.Services
{
    public class PdfService : IPdfService
    {
        private PdfDocument? _document;

        public Task<Core.Models.PdfDocument> LoadDocumentAsync(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("Invalid file path", nameof(filePath));

            if (!File.Exists(filePath))
                throw new FileNotFoundException("PDF file not found", filePath);

            // Load PDF into memory
            var bytes = File.ReadAllBytes(filePath);

            // PDFiumSharp V2 manages memory internally (no Dispose needed)
            _document = new PdfDocument(bytes);

            var result = new Core.Models.PdfDocument
            {
                FilePath = filePath,
                PageCount = _document.Pages.Count
            };

            for (int i = 0; i < _document.Pages.Count; i++)
            {
                var page = _document.Pages[i];
                var size = page.Size;

                result.Pages.Add(new Core.Models.PdfPage
                {
                    PageNumber = i, // 0-based index
                    Width = size.Width,
                    Height = size.Height
                });
            }

            return Task.FromResult(result);
        }

        public Task<byte[]> RenderPageAsync(Core.Models.PdfPage page, double scale = 1)
        {
            if (_document == null)
                throw new InvalidOperationException("Document not loaded");

            if (page == null)
                throw new ArgumentNullException(nameof(page));

            var pdfPage = _document.Pages[page.PageNumber];

            int width = (int)(pdfPage.Width * scale);
            int height = (int)(pdfPage.Height * scale);

            // Create bitmap (BGRA format for WPF compatibility)
            using var bitmap = new PDFiumBitmap(width, height, true);

            // Fill background white
            bitmap.Fill(0xFFFFFFFF);

            // Render page into bitmap
            pdfPage.Render(bitmap);

            // Convert to byte[]
            using var ms = new MemoryStream();
            bitmap.Save(ms);

            return Task.FromResult(ms.ToArray());
        }
    }
}
