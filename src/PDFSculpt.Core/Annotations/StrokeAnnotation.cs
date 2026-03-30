namespace PDFSculpt.Core.Annotations
{
    public class StrokeAnnotation
    {
        public int PageNumber { get; set; }

        public List<AnnotationPoint> Points { get; set; } = new();

        public string ColorHex { get; set; } = "#FF0000";

        public double Thickness { get; set; } = 2.0;
    }
}
