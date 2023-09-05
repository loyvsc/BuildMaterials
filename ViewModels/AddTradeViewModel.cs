using BuildMaterials.BD;
using BuildMaterials.Models;
using MySqlConnector;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace BuildMaterials.ViewModels
{
    public class EmployeeFIO
    {
        public string? Surname;
        public string? Name;
        public string? Pathnetic;

        public EmployeeFIO() { }

        public override string ToString()
        {
            return Surname + " " + Name + " " + Pathnetic;
        }
    }

    public class AddTradeViewModel : INotifyPropertyChanged
    {
        public Models.Trade Trade { get; set; }
        public ICommand CancelCommand => new RelayCommand((sender) => _window.Close());
        public ICommand AddCommand => new RelayCommand((sender) => AddMaterial());
        public string[] SellersFIO
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
        public Models.Material[] Materials => App.DBContext.Materials.Select("SELECT ID,Name,Count FROM Materials").ToArray();

        public Models.Material SelectedMaterial
        {
            get => _selMat;
            set
            {
                _selMat = value;
                MaxCountValue = "но не больше " + value.Count;
                OnPropertyChanged("SelectedMaterial");
            }
        }
        private Models.Material _selMat;

        private readonly Window _window = null!;
        public string SelectedFIO { get; set; }

        public AddTradeViewModel()
        {
            Trade = new Models.Trade();
        }

        public AddTradeViewModel(Window window) : this()
        {
            _window = window;
        }

        public string MaxCountValue
        {
            get => _maxCountValue;
            set
            {
                _maxCountValue = value;
                OnPropertyChanged("MaxCountValue");
            }
        }
        private string _maxCountValue;

        public event PropertyChangedEventHandler? PropertyChanged;

        private void AddMaterial()
        {
            if (Trade.Count > _selMat.Count)
            {
                MessageBox.Show("Продано больше, чем в наличии!", "Товарооборот", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Trade.SellerFio = SelectedFIO;
            Trade.MaterialName = SelectedMaterial.Name;
            if (Trade.IsValid)
            {
                App.DBContext.Query($"UPDATE Materials SET COUNT = COUNT-{Trade.Count} WHERE id = {SelectedMaterial.ID};");
                App.DBContext.Trades.Add(Trade);
                _window.DialogResult = true;
                return;
            }
            MessageBox.Show("Не вся информация была введена!", "Товарооборот", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}