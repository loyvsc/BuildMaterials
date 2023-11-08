using BuildMaterials.BD;
using BuildMaterials.Models;
using System.Windows;
using System.Windows.Input;

namespace BuildMaterials.ViewModels
{
    public static class Extensions
    {
        public static MySqlDataReader ExecuteMySqlReaderAsync(this MySqlCommand command)
        {
            return (MySqlDataReader) command.ExecuteReaderAsync().Result;
        }
    }

    public class AddTTNViewModel
    {
        public Models.TTN TTN { get; set; }

        public ICommand CancelCommand => new RelayCommand((sender) => _window.Close());
        public ICommand AddCommand => new RelayCommand((sender) => AddMaterial());

        private readonly Window _window = null!;
        public List<Material> Materials => App.DBContext.Materials.ToList();

        public readonly Settings Settings = new Settings();
        public List<Seller>? CustomersList { get; set; }

        public AddTTNViewModel()
        {
            TTN = new TTN();
        }

        public AddTTNViewModel(Window window) : this()
        {
            _window = window;
            CustomersList = App.DBContext.Sellers.ToList();
        }

        ~AddTTNViewModel()
        {
            CustomersList = null;
        }

        private void AddMaterial()
        {
            if (TTN.IsValid)
            {
                App.DBContext.TTNs.Add(TTN);
                _window.DialogResult = true;
                return;
            }
            System.Windows.MessageBox.Show("Не вся информация была введена!", "Новый ТТН", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}