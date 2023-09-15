using BuildMaterials.Models;
using System.Windows;
using System.Windows.Input;

namespace BuildMaterials.ViewModels
{
    public class AddMaterialResponseViewModel : NotifyPropertyChangedBase
    {
        private readonly Window? _window;
        public MaterialResponse MaterialResponse { get; set; }
        public Material? SelectedMaterial { get; set; }
        public ICommand CancelCommand => new RelayCommand((sender) => _window?.Close());
        public ICommand AddCommand => new RelayCommand((sender) => AddMaterial());

        public List<Material> Materials => App.DBContext.Materials.ToList();
        public List<Employee> FinResponsibleEmployees => App.DBContext.Employees.Select("SELECT * FROM Employees WHERE FinResponsible = 1");

        public AddMaterialResponseViewModel()
        {
            MaterialResponse = new MaterialResponse();
        }

        public AddMaterialResponseViewModel(Window window) : this()
        {
            _window = window;
        }

        private void AddMaterial()
        {
            if (SelectedMaterial == null)
            {
                error();
                return;
            }
            else
            {
                MaterialResponse.Name = SelectedMaterial.Name!;
            }
            if (MaterialResponse.IsValid)
            {
                App.DBContext.MaterialResponse.Add(MaterialResponse);
                System.Windows.MessageBox.Show("Материально-ответственный отчет успешно добавлен!","Материально-ответственный отчет",MessageBoxButton.OK,MessageBoxImage.Information);
                _window?.Close();
            }
            else
            {
                error();
            }
            void error()=> System.Windows.MessageBox.Show("Добавление материально-ответственного отчета завершено с ошибкой!\nПопробуйте позже...", "Материально-ответственный отчет", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
