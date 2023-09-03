using BuildMaterials.Models;
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
                List<Customer> customers = App.DBContext.Customers.Select(x => new Customer() { CompanyName = x.CompanyName, Adress = x.Adress }).ToList();
                customers.AddRange(App.DBContext.Providers.Select(x => new Customer() { Adress = x.Adress, CompanyName = x.CompanyName }).ToList());
                customers.Add(new Customer() { CompanyName = Settings.CompanyName, Adress = Settings.CompanyAdress });
                return customers;
            }
        }

        public string[] EmployeeNames => App.DBContext.Employees.Select(x => x.SurName + " " + x.Name + " " + x.Pathnetic).ToArray();
        
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