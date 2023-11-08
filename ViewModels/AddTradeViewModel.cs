using BuildMaterials.BD;
using BuildMaterials.Models;
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
        public EmployeeFIO(string? surname, string? name = "", string? pathnetic = "")
        {
            Surname = surname;
            Name = name;
            Pathnetic = pathnetic;
        }

        public override string ToString()
        {
            return Surname + " " + Name + " " + Pathnetic;
        }
    }

    public class AddTradeViewModel : NotifyPropertyChangedBase
    {
        public Models.Trade Trade { get; set; }
        public ICommand CancelCommand => new RelayCommand((sender) => _window.Close());
        public ICommand AddCommand => new RelayCommand((sender) => AddMaterial());
        public List<Employee> SellersFIO => App.DBContext.Employees.Select("SELECT * FROM Employees");
        public List<Material> Materials => App.DBContext.Materials.Select("SELECT * FROM Materials");

        public Models.Material SelectedMaterial
        {
            get => _selMat;
            set
            {
                _selMat = value;
                MaxCountValue = ", но не больше " + value.Count;
                OnPropertyChanged("SelectedMaterial");
            }
        }
        private Models.Material _selMat = null!;

        private readonly Window _window = null!;
        public Employee? SellectedEmployee { get; set; }

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
        private string _maxCountValue = string.Empty;

        private void AddMaterial()
        {
            if (Trade.Count > _selMat.Count)
            {
                System.Windows.MessageBox.Show("Продано больше, чем в наличии!", "Товарооборот", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (SellectedEmployee != null && SelectedMaterial!=null)
            {
                Trade.SellerID = SellectedEmployee!.ID;
                Trade.MaterialID = SelectedMaterial.ID;
            }
            else
            {
                System.Windows.MessageBox.Show("Введите всю требуемую информацию!", "Товарооборот", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (Trade.IsValid)
            {
                App.DBContext.Trades.Add(Trade);
                App.DBContext.Query($"UPDATE Materials SET COUNT = COUNT-{Trade.Count} WHERE id = {SelectedMaterial.ID};");
                _window.DialogResult = true;
                return;
            }
            System.Windows.MessageBox.Show("Не вся информация была введена!", "Товарооборот", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}