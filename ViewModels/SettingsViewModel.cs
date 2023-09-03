using BuildMaterials.Models;
using System.Windows;
using System.Windows.Input;

namespace BuildMaterials.ViewModels
{
    public class SettingsViewModel
    {
        public ICommand CancelCommand => new RelayCommand((sender) => Close());
        public ICommand SaveCommand => new RelayCommand((sender) => SaveSettings());

        private readonly Window _window = null!;

        public Settings Settings { get; private set; }

        public SettingsViewModel()
        {
            Settings = new Settings();
        }

        public SettingsViewModel(Window window) : this()
        {
            _window = window;
        }

        private void SaveSettings()
        {
            if (Settings.IsValid)
            {
                Settings.Save();
                Close();                
            }
            else
            {
                MessageBox.Show("Наименование организации не введено!", "Настройки", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Close()
        {
            _window.Close();
        }
    }
}