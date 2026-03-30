using PDFSculpt.App.ViewModels;
using PDFSculpt.Core.Annotations;
using PDFSculpt.Core.Models;
using PDFSculpt.Infrastructure.Services;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PDFSculpt.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var pdfService = new PdfService();
            DataContext = new MainViewModel(pdfService);
        }

        private bool _isDrawing;
        private Polyline? _currentLine;
        private PdfPage? _currentPage;

        private void Canvas_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (DataContext is not MainViewModel vm)
                return;

            if (vm.SelectedTool != AnnotationTool.Draw)
                return;

            var canvas = sender as Canvas;

            _isDrawing = true;

            _currentPage = canvas?.Tag as PdfPage;

            _currentLine = new Polyline
            {
                Stroke = Brushes.Red,
                StrokeThickness = 2
            };

            var point = e.GetPosition(canvas);
            _currentLine.Points.Add(point);

            canvas?.Children.Add(_currentLine);
        }

        private void Canvas_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (!_isDrawing || _currentLine == null)
                return;

            var canvas = sender as Canvas;
            var point = e.GetPosition(canvas);

            _currentLine.Points.Add(point);
        }

        private void Canvas_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _isDrawing = false;
            _currentLine = null;
            _currentPage = null;
        }
    }
}