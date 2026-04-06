namespace PDFSculpt.Core.Annotations
{
    public class ReplyAnnotation
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string ParentId { get; set; } = string.Empty;

        public double X { get; set; }
        public double Y { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }

        public string Text { get; set; } = string.Empty;

        public string Color { get; set; } = "Blue";
    }
}
