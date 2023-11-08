using BuildMaterials.BD;
using BuildMaterials.Models;
using System.Windows;
using System.Windows.Input;

namespace BuildMaterials.ViewModels
{
    public class AddContractViewModel
    {
        public Contract Contract { get; set; }
        public ICommand CancelCommand => new RelayCommand((sender) => _window.Close());
        public ICommand AddCommand => new RelayCommand((sender) => AddMaterial());

        private readonly Window _window = null!;
        public readonly Settings Settings;

        public List<Material> Materials => App.DBContext.Materials.ToList();

        public List<Seller> CustomersList
        {
            get
            {
                List<Seller> customers = new List<Seller>(128);
                using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
                {
                    _connection.Open();
                    using (MySqlCommand _command = new MySqlCommand
                        ("SELECT CompanyName, Adress FROM customers union SELECT CompanyName, Adress FROM providers;", _connection))
                    {
                        using (MySqlDataReader reader = _command.ExecuteMySqlReaderAsync())
                            while (reader.Read())
                            {
                                customers.Add(new Seller() { CompanyName = reader.GetString(0), Adress = reader.GetString(1) });
                            }
                    }
                    _connection.Close();
                }
                customers.Add(new Seller() { CompanyName = Settings.CompanyName, Adress = Settings.CompanyAdress });
                return customers;
            }
        }

        public string[] EmployeeNames
        {
            get
            {
                List<string> fio = new List<string>(32);
                using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
                {
                    _connection.OpenAsync().Wait();
                    using (MySqlCommand _command = new MySqlCommand("SELECT Name, Surname, pathnetic FROM Employees;", _connection))
                    {
                        using (MySqlDataReader reader = _command.ExecuteMySqlReaderAsync())
                            while (reader.Read())
                            {
                                fio.Add($"{reader.GetString(0)} {reader.GetString(1)} {reader.GetString(2)}");
                            }
                    }
                    _connection.CloseAsync().Wait();
                }
                return fio.ToArray();
            }
        }

        public int SelectedShipperIndex;
        public int SelectedConsigneeIndex;

        public AddContractViewModel()
        {
            Contract = new Contract();
            Settings = new Settings();
        }

        public AddContractViewModel(Window window) : this()
        {
            _window = window;
        }

        private void AddMaterial()
        {
            if (Contract.IsValid)
            {
                App.DBContext.Contracts.Add(Contract);
                _window.DialogResult = true;
                return;
            }
            System.Windows.MessageBox.Show("Не вся информация была введена!", "Новый счет-фактура", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}