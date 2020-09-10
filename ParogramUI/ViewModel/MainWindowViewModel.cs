using ProgramUI.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProgramUI.ViewModel
{
    public class MainWindowViewModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool isInProcess;
        private string filePath;

        public bool IsInProcess
        {
            get => isInProcess;
            set
            {
                isInProcess = value;
                OnPropertyChanged();
            }
        }
        public string FilePath
        {
            get => filePath;
            set
            {
                filePath = value;
                OnPropertyChanged();
            }
        }
        public Task SortTask { get; set; }

        public MainWindowViewModel()
        {
            IsInProcess = false;
            FilePath = "";
        }

        private void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }

    public class MainWindowCommands
    {
        public static RoutedCommand SelectPath { get; set; }
        public static RoutedCommand Sort { get; set; }

        static MainWindowCommands()
        {
            SelectPath = new RoutedCommand("SelectPath", typeof(MainWindow));
            Sort = new RoutedCommand("Sort", typeof(MainWindow));
        }
    }
}
