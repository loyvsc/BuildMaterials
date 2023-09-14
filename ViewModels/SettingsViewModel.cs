using BuildMaterials.Models;
using System.Windows;
using System.Windows.Input;

namespace BuildMaterials.ViewModels
{
    public class SettingsViewModel
    {
        public ICommand CancelCommand => new RelayCommand((sender) => _window.Close());
        public ICommand SaveCommand => new RelayCommand((sender) => SaveSettings());
        public ICommand DBDropCommand => new RelayCommand((sender) => DropDB());

        private readonly Window _window = null!;

        public Settings Settings { get; private set; } = new Settings();

        public SettingsViewModel() { }

        public SettingsViewModel(Window window)
        {
            _window = window;
        }

        private void DropDB()
        {
            App.DBContext.Query("DROP DATABASE buildmaterials;");
            App.DBContext.InitializeDatabase();
            App.DBContext.Employees.Add(new Employee(-1, "Имя", "Фамилия", "Отчество", "Администратор", "+375259991234","BM123456789",DateTime.Now));
        }

        private void SaveSettings()
        {
            if (Settings.IsValid)
            {
                Settings.Save();
                _window.Close();
            }
            else
            {
                System.Windows.MessageBox.Show("Наименование организации не введено!", "Настройки", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}