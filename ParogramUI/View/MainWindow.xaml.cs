using BigFilesSort.Models;
using Microsoft.Win32;
using ProgramUI.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProgramUI.View
{
    public partial class MainWindow : Window
    {
        private MainWindowViewModel model;

        public MainWindow()
        {
            InitializeComponent();

            model = new MainWindowViewModel();
            DataContext = model;
        }

        private void CommandBinding_SelectPath(object sender, ExecutedRoutedEventArgs e)
        {
            var OF = new OpenFileDialog();
            if (OF.ShowDialog() == true)
            {
                model.FilePath = OF.FileName;
            }
        }
        private void CommandBinding_Sort(object sender, ExecutedRoutedEventArgs e)
        {
            if (model != null && !model.IsInProcess)
            {
                model.IsInProcess = true;
                model.SortTask = Task.Factory.StartNew(() =>
                {
                    var path = "";
                    var time = DateTime.Now;
                    Dispatcher.Invoke((Action)(() => {
                        path = model.FilePath;
                    }));
                    try
                    {
                        FileSorter.Sort(path, 100_000_000, 100_000_000);

                        Dispatcher.Invoke((Action)(() => {
                            MessageBox.Show($"Завершено за {(DateTime.Now - time).TotalSeconds} сек.!", "Завершено");
                            model.SortTask = null;
                            model.IsInProcess = false;
                        }));
                    }
                    catch (Exception exc)
                    {
                        Dispatcher.Invoke((Action)(() => {
                            MessageBox.Show($"Ошибка: {exc.Message}", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                            model.SortTask = null;
                            model.IsInProcess = false;
                        }));
                    }
                });
            }
        }
        private void CommandBinding_SelectPath_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !(model?.IsInProcess ?? true);
        }
        private void CommandBinding_Sort_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !(model?.IsInProcess ?? true) && File.Exists(model.FilePath);
        }
    }
}
