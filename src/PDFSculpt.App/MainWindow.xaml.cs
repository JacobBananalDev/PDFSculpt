using PDFSculpt.App.ViewModels;
using PDFSculpt.Infrastructure.Services;
using System.Windows;

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
    }
}