using System.Windows;
using System.Windows.Input;

namespace BuildMaterials.ViewModels
{
    public class AddProviderViewModel
    {
        public Models.Provider Provider { get; set; }

        public ICommand CancelCommand { get => new RelayCommand((sender) => _window.Close()); }
        public ICommand AddCommand { get => new RelayCommand((sender) => AddMaterial()); }

        private readonly Window _window = null!;

        public AddProviderViewModel()
        {
            Provider = new Models.Provider();
        }

        public AddProviderViewModel(Window window) : this()
        {
            _window = window;
        }

        private void AddMaterial()
        {
            if (Provider.IsValid)
            {
                App.DBContext.Providers.Add(Provider);
                App.DBContext.SaveChanges();
                _window.DialogResult = true;
                return;
            }
            MessageBox.Show("Не вся информация была введена!", "Новый поставщик", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}