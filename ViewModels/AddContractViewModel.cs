using BuildMaterials.BD;
using BuildMaterials.Models;
using MySqlConnector;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace BuildMaterials.ViewModels
{
    public class AddContractViewModel
    {
        public Models.Contract Contract { get; set; }
        public ICommand CancelCommand { get => new RelayCommand((sender) => _window.Close()); }
        public ICommand AddCommand { get => new RelayCommand((sender) => AddMaterial()); }

        private readonly Window _window = null!;
        public readonly Settings Settings;

        public string?[] MaterialNames
        {
            get
            {
                List<string> list = new List<string>(64);
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
                return list.ToArray();
            }
        }

        public List<Customer> CustomersList
        {
            get
            {
                List<Customer> customers = new List<Customer>(64);
                using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
                {
                    using (MySqlCommand _command = new MySqlCommand("SELECT CompanyName, Adress FROM customers;", _connection))
                    {
                        MySqlDataReader reader = _command.ExecuteReaderAsync().Result;
                        while (reader.Read())
                        {
                            customers.Add(new Customer() { CompanyName = reader.GetString(0), Adress = reader.GetString(1) });
                        }
                    }
                }
                List<Customer> providersAsCustomers = new List<Customer>(64);
                using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
                {
                    using (MySqlCommand _command = new MySqlCommand("SELECT CompanyName, Adress FROM providers;", _connection))
                    {
                        MySqlDataReader reader = _command.ExecuteReaderAsync().Result;
                        while (reader.Read())
                        {
                            customers.Add(new Customer() { CompanyName = reader.GetString(0), Adress = reader.GetString(1) });
                        }
                    }
                }
                customers.AddRange(providersAsCustomers);
                customers.Add(new Customer() { CompanyName = Settings.CompanyName, Adress = Settings.CompanyAdress });
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
                    using (MySqlCommand _command = new MySqlCommand("SELECT Name, Surname, pathnetic FROM Employees;", _connection))
                    {
                        MySqlDataReader reader = _command.ExecuteReaderAsync().Result;
                        while (reader.Read())
                        {
                            fio.Add($"{reader.GetString(0)} {reader.GetString(1)} {reader.GetString(2)}");
                        }
                    }
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
                App.DBContext.SaveChanges();
                _window.DialogResult = true;
                return;
            }
            MessageBox.Show("Не вся информация была введена!", "Новый счет-фактура", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}