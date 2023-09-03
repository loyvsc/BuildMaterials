using BuildMaterials.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace BuildMaterials.ViewModels
{
    public static class ListExtensions
    {
        static public void AddProvider(this List<Customer> customers, List<Provider> providers)
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

        public ICommand CancelCommand { get => new RelayCommand((sender) => _window.Close()); }
        public ICommand AddCommand { get => new RelayCommand((sender) => AddMaterial()); }

        private readonly Window _window = null!;
        public List<string?> MaterialNames => App.DBContext.Materials.Select(x => x.Name).ToList();

        public readonly Settings Settings;
        public List<Customer>? CustomersList { get; set; }

        public AddTTNViewModel()
        {
            TTN = new TTN();
            Settings = new Settings();
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
                App.DBContext.SaveChanges();
                _window.DialogResult = true;
               return;
            }
            MessageBox.Show("Не вся информация была введена!", "Новый ТТН", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}