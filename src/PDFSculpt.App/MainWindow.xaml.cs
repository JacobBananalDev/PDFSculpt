using PDFSculpt.App.ViewModels;
using PDFSculpt.Core.Annotations;
using PDFSculpt.Core.Models;
using PDFSculpt.Infrastructure.Services;
using System.Runtime.CompilerServices;
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
        private StrokeAnnotation? _currentStroke;

        private void Canvas_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (DataContext is not MainViewModel vm)
                return;

            if (vm.SelectedTool != AnnotationTool.Draw)
                return;

            var canvas = sender as Canvas;
            var page = canvas?.Tag as PdfPage;

            if (canvas == null || page == null)
                return;

            _isDrawing = true;
            _currentPage = page;

            _currentLine = new Polyline
            {
                Stroke = Brushes.Red,
                StrokeThickness = 2
            };

            _currentStroke = new StrokeAnnotation
            {
                PageNumber = page.PageNumber
            };

            var point = e.GetPosition(canvas);
            _currentLine.Points.Add(point);

            _currentStroke?.Points.Add(new AnnotationPoint
            {
                X = point.X,
                Y = point.Y
            });

            canvas?.Children.Add(_currentLine);
        }

        private void Canvas_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (!_isDrawing || _currentLine == null || _currentStroke == null)
                return;

            var canvas = sender as Canvas;
            var point = e.GetPosition(canvas);

            _currentLine.Points.Add(point);

           

            _currentStroke.Points.Add(new AnnotationPoint
            {
                X = point.X,
                Y = point.Y
            });
        }

        private void Canvas_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (_currentPage != null && _currentStroke != null)
            {
                _currentPage.Strokes.Add(_currentStroke);
            }

            _isDrawing = false;
            _currentLine = null;
            _currentStroke = null;
            _currentPage = null;
        }

        private void Canvas_Loaded(object sender, RoutedEventArgs e)
        {
            var canvas = sender as Canvas;
            var page = canvas?.Tag as PdfPage;

            if (canvas == null || page == null)
                return;

            canvas.Children.Clear();

            foreach (var stroke in page.Strokes)
            {
                var polyline = new Polyline
                {
                    Stroke = Brushes.Red,
                    StrokeThickness = stroke.Thickness
                };

                foreach (var p in stroke.Points)
                {
                    polyline.Points.Add(new System.Windows.Point(p.X, p.Y));
                }

                canvas.Children.Add(polyline);
            }
        }
    }
}