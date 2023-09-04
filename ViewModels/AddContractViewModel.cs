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
                using (MySqlConnection _connection = new MySqlConnection(App.DBContext.ConnectionString))
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
                List<Customer> customers = App.DBContext.Customers.Select(x => new Customer() { CompanyName = x.CompanyName, Adress = x.Adress }).ToList();
                customers.AddRange(App.DBContext.Providers.Select(x => new Customer() { Adress = x.Adress, CompanyName = x.CompanyName }).ToList());
                customers.Add(new Customer() { CompanyName = Settings.CompanyName, Adress = Settings.CompanyAdress });
                return customers;
            }
        }

        public string[] EmployeeNames => App.DBContext.Employees.Select(x => x.SurName + " " + x.Name + " " + x.Pathnetic).ToArray();

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