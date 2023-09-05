using System;
using System.Windows;
using System.Windows.Input;

namespace BuildMaterials.ViewModels
{
    public class AddMaterialViewModel
    {
        public Models.Material Material { get; set; }

        public ICommand CancelCommand { get => new RelayCommand((sender) => _window.Close()); }
        public ICommand AddCommand { get => new RelayCommand((sender) => AddMaterial()); }

        private readonly Window _window = null!;

        public AddMaterialViewModel()
        {
            Material = new Models.Material();
        }

        public AddMaterialViewModel(Window window) : this()
        {
            _window = window;
        }

        private void AddMaterial()
        {
            if (Material.IsValid)
            {
                Material.EnterCount = Material.Count;
                Material.EnterDate = DateTime.Now.Date;
                App.DBContext.Materials.Add(Material);
                _window.DialogResult = true;
                return;
            }
            MessageBox.Show("Не вся информация была введена!", "Новый материал", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}