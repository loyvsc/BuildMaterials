using System.Windows;
using System.Windows.Input;

namespace BuildMaterials.ViewModels
{
    public class AddEmployeeViewModel
    {
        public Models.Employee Employee { get; set; }

        public ICommand CancelCommand { get => new RelayCommand((sender) => _window.Close()); }
        public ICommand AddCommand { get => new RelayCommand((sender) => AddMaterial()); }        

        private readonly Window _window = null!;
        public string[] EmployeeAccessLevel => App.DBContext.AccessLevel;
        public int SelectedAccessLevel { get; set; }

        public AddEmployeeViewModel()
        {
            Employee = new Models.Employee();
        }

        public AddEmployeeViewModel(Window window) : this()
        {
            _window = window;
        }

        private void AddMaterial()
        {
            Employee.AccessLevel = SelectedAccessLevel;
            if (Employee.IsValid)
            {
                App.DBContext.Employees.Add(Employee);
                _window.DialogResult = true;
                return;
            }
            MessageBox.Show("Не вся информация была введена!", "Новый сотрудник", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}