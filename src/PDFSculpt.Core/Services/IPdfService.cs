namespace PDFSculpt.Core.Services
{
    public interface IPdfService
    {
        Task OpenDocumentAsync(string filePath);

        int GetPageCount();

        Task<byte[]> RenderPageAsync(int pageNumber, double scale = 1.0);
    }
}
