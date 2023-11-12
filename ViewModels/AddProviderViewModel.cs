using System.Windows;
using System.Windows.Input;

namespace BuildMaterials.ViewModels
{
    public class AddProviderViewModel
    {
        public Models.Seller Provider { get; set; }

        public ICommand CancelCommand => new RelayCommand((sender) => _window.Close());
        public ICommand AddCommand => new RelayCommand((sender) => AddMaterial());

        private readonly Window _window = null!;

        public AddProviderViewModel()
        {
            Provider = new Models.Seller();
        }

        public AddProviderViewModel(Window window) : this()
        {
            _window = window;
        }

        private void AddMaterial()
        {
            if (Provider.IsValid)
            {
                App.DbContext.Sellers.Add(Provider);
                _window.DialogResult = true;
                return;
            }
            System.Windows.MessageBox.Show("Не вся информация была введена!", "Новый поставщик", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}