using PDFSculpt.App.Commands;
using System.Windows.Data;

namespace PDFSculpt.App.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private string _title = "PDFSculpt";
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private RelayCommand? _TestCommand;
        public RelayCommand TestCommand => _TestCommand ??= new RelayCommand(ExecuteTestCommand);

        private void ExecuteTestCommand()
        {
            Title = "Button Clicked!";
        }
    }
}
