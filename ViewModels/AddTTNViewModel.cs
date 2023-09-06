using BuildMaterials.BD;
using BuildMaterials.Models;
using System.Windows;
using System.Windows.Input;

namespace BuildMaterials.ViewModels
{
    public static class ListExtensions
    {
        public static void AddProvider(this List<Customer> customers, List<Provider> providers)
        {
            for (int i = 0; i < providers.Count; i++)
            {
                customers.Add((Customer)providers[i]);
            }
        }
    }

    public class AddTTNViewModel
    {
        public Models.TTN TTN { get; set; }

        public ICommand CancelCommand => new RelayCommand((sender) => _window.Close());
        public ICommand AddCommand => new RelayCommand((sender) => AddMaterial());

        private readonly Window _window = null!;
        public List<string?> MaterialNames
        {
            get
            {
                List<string?> list = new List<string?>(64);
                using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
                {
                    _connection.Open();
                    using (MySqlCommand command = new MySqlCommand("SELECT Name FROM Materials", _connection))
                    {
                        MySqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            list.Add(reader.GetString(0));
                        }
                    }
                    _connection.Close();
                }
                return list;
            }

        }

        public readonly Settings Settings = new Settings();
        public List<Customer>? CustomersList { get; set; }

        public AddTTNViewModel()
        {
            TTN = new TTN();
        }

        public AddTTNViewModel(Window window) : this()
        {
            _window = window;
        }

        public AddTTNViewModel(Window window, List<Customer> customers, List<Provider> providers) : this(window)
        {
            CustomersList = customers;
            CustomersList.AddProvider(providers);
            CustomersList.Add(new Customer() { CompanyName = Settings.CompanyName });
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