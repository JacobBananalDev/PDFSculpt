using PDFSculpt.App.Commands;
using PDFSculpt.Core.Models;
using PDFSculpt.Core.Services;
using System.Collections.ObjectModel;

namespace PDFSculpt.App.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly IPdfService _pdfService;

        public MainViewModel(IPdfService pdfService)
        {
            _pdfService = pdfService;
        }

        public ObservableCollection<PdfPage> Pages { get; } = new();

        private string _title = "PDFSculpt";
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private string _SelectedFile = "No file selected";
        public string SelectedFile
        {
            get => _SelectedFile;
            set => SetProperty(ref _SelectedFile, value);
        }

        private RelayCommand? _TestCommand;
        public RelayCommand TestCommand => _TestCommand ??= new RelayCommand(ExecuteTestCommand);

        private void ExecuteTestCommand()
        {
            Title = "Button Clicked!";
        }

        private RelayCommand? _OpenPdfCommand;
        public RelayCommand OpenPdfCommand => _OpenPdfCommand ??= new RelayCommand(ExecuteOpenPdfCommand);

        private async void ExecuteOpenPdfCommand()
        {
            // temporaryu - will be replace with a proper service later
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "PDF Files (*.pdf)|*.pdf"
            };

            if (dialog.ShowDialog() == true)
            {
                SelectedFile = dialog.FileName;

                var document = await _pdfService.LoadDocumentAsync(dialog.FileName);

                Title = $"Loaded {document.PageCount} pages";

                Pages.Clear();

                foreach (var page in document.Pages)
                {
                    Pages.Add(page);
                }
            }
        }
    }
}
