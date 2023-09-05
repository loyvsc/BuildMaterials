using BuildMaterials.BD;
using BuildMaterials.Models;
using MySqlConnector;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Windows;
using System.Windows.Input;

namespace BuildMaterials.ViewModels
{
    public class AddAccountViewModel
    {
        public Models.Account Account { get; set; }
        public ICommand CancelCommand { get => new RelayCommand((sender) => _window.Close()); }
        public ICommand AddCommand { get => new RelayCommand((sender) => AddMaterial()); }

        private readonly Window _window = null!;
        public readonly Settings Settings;

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
                            customers.Add(new Customer() { CompanyName=reader.GetString(0),Adress=reader.GetString(1)});
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

        public AddAccountViewModel()
        {            
            Account = new Account();
            Settings = new Settings();
        }

        public AddAccountViewModel(Window window) : this()
        {
            _window = window;
        }

        private void AddMaterial()
        {
            Account.ShipperAdress = CustomersList[SelectedShipperIndex].Adress;
            Account.ConsigneeAdress = CustomersList[SelectedConsigneeIndex].Adress;
            if (Account.IsValid)
            {
                App.DBContext.Accounts.Add(Account);
                App.DBContext.SaveChanges();
                _window.DialogResult = true;
                return;
            }
            MessageBox.Show("Не вся информация была введена!", "Новый счет-фактура", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}