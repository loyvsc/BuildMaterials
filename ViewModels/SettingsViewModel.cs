using BuildMaterials.Models;
using System.Windows;
using System.Windows.Input;

namespace BuildMaterials.ViewModels
{
    public class SettingsViewModel
    {
        public ICommand CancelCommand => new RelayCommand((sender) => _window.Close());
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
            App.DbContext.Query("DROP DATABASE buildmaterials;");
            App.DbContext.CreateDatabase();
        }
    }
}