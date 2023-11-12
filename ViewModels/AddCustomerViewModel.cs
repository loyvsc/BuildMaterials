using System.Windows;
using System.Windows.Input;

namespace BuildMaterials.ViewModels
{
    public class AddCustomerViewModel
    {
        public Models.Seller Customer { get; set; }

        public ICommand CancelCommand => new RelayCommand((sender) => _window.Close());
        public ICommand AddCommand => new RelayCommand((sender) => AddMaterial());

        private readonly Window _window = null!;

        public AddCustomerViewModel()
        {
            Customer = new Models.Seller(true);
        }

        public AddCustomerViewModel(Window window) : this()
        {
            _window = window;
        }

        private void AddMaterial()
        {
            if (Customer.IsValid)
            {
                App.DbContext.Sellers.Add(Customer);
                _window.DialogResult = true;
                return;
            }
            System.Windows.MessageBox.Show("Не вся информация была введена!", "Новый заказчик", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}