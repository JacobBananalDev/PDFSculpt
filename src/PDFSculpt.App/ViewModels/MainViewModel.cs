using PDFSculpt.App.Commands;
using PDFSculpt.Core.Annotations;
using PDFSculpt.Core.Models;
using PDFSculpt.Core.Services;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

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

        private Core.Models.PdfDocument? _currentDocument;

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

        private CancellationTokenSource? _zoomCts;

        private double _zoom = 1.0;
        public double Zoom
        {
            get => _zoom;
            set
            {
                if (SetProperty(ref _zoom, value))
                {
                    // instant visual zoom
                    ZoomScale = value;

                    _zoomCts?.Cancel();
                    _zoomCts = new CancellationTokenSource();

                    _ = ReloadPagesWithDelayAsync(_zoomCts.Token);
                }
            }
        }

        private double _zoomScale = 1.0;
        public double ZoomScale
        {
            get => _zoomScale;
            set => SetProperty(ref _zoomScale, value);
        }

        private AnnotationTool _selectedTool = AnnotationTool.None;
        public AnnotationTool SelectedTool
        {
            get => _selectedTool;
            set => SetProperty(ref _selectedTool, value);
        }

        private async Task ReloadPagesWithDelayAsync(CancellationToken token)
        {
            try
            {
                await Task.Delay(400, token); // wait until user stops dragging

                await ReloadPagesAsync();
            }
            catch (TaskCanceledException)
            {
                // ignore
            }
        }

        private async Task ReloadPagesAsync()
        {
            if (_currentDocument == null)
                return;

            Pages.Clear();

            foreach (var page in _currentDocument.Pages)
            {
                var imageBytes = await _pdfService.RenderPageAsync(page, Zoom);

                page.ImageData = imageBytes;

                Pages.Add(page);
            }
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

                _currentDocument = document;

                await ReloadPagesAsync();
            }
        }

        private RelayCommand? _selectDrawToolCommand;
        public RelayCommand SelectDrawToolCommand => _selectDrawToolCommand ??= new RelayCommand(ExecuteSelectDrawToolCommand);

        private void ExecuteSelectDrawToolCommand()
        {
            SelectedTool = AnnotationTool.Draw;
        }
    }
}
