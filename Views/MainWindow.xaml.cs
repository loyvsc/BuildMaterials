using BuildMaterials.Models;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BuildMaterials.Views
{
    public partial class MainWindow : Window
    {
        private readonly ViewModels.MainWindowViewModel mainViewModel;

        public MainWindow()
        {
            InitializeComponent();
            mainViewModel = new ViewModels.MainWindowViewModel();
            DataContext = mainViewModel;
        }

        public MainWindow(Employee employee)
        {
            InitializeComponent();
            mainViewModel = new ViewModels.MainWindowViewModel();
            mainViewModel.CurrentEmployee = employee;
            DataContext = mainViewModel;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            mainViewModel.SaveChanges(e);
            base.OnClosing(e);
        }

        private void TabControl_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            mainViewModel.OnTabChanged(e);
        }

        private void DataGrid_CellEditEnding(object sender, System.Windows.Controls.DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction.Equals(DataGridEditAction.Commit))
            {
                mainViewModel.IsSaved = false;
            }
        }

        private void DataGrid_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DataGridRow? dgr = null;
            var visParent = VisualTreeHelper.GetParent(e.OriginalSource as FrameworkElement);
            while (dgr == null && visParent != null)
            {
                dgr = visParent as DataGridRow;
                visParent = VisualTreeHelper.GetParent(visParent);
            }
            if (dgr == null) { return; }

            mainViewModel.SelectedRowIndex = dgr.GetIndex();
        }
    }
}