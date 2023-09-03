using System.Windows;
using System.Windows.Input;

namespace BuildMaterials.ViewModels
{
    public class AddCustomerViewModel
    {
        public Models.Customer Customer { get; set; }

        public ICommand CancelCommand { get => new RelayCommand((sender) => _window.Close()); }
        public ICommand AddCommand { get => new RelayCommand((sender) => AddMaterial()); }

        private readonly Window _window = null!;

        public AddCustomerViewModel()
        {
            Customer = new Models.Customer();
        }

        public AddCustomerViewModel(Window window) : this()
        {
            _window = window;
        }

        private void AddMaterial()
        {
            if (Customer.IsValid)
            {
                App.DBContext.Customers.Add(Customer);
                App.DBContext.SaveChanges();
                _window.DialogResult = true;
                return;
            }
            MessageBox.Show("Не вся информация была введена!", "Новый заказчик", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}