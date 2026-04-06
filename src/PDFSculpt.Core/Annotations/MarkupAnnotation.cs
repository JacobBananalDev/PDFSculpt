namespace PDFSculpt.Core.Annotations
{
    public class MarkupAnnotation
    {
        // Normalized coordinates
        public double X { get; set; }
        public double Y { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }

        public string Color { get; set; } = "Red";

        public bool IsReadOnly { get; set; } = true;

        // Replies attached to this annotation
        public List<ReplyAnnotation> Replies { get; set; } = new();
    }
}
