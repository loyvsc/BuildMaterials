using System.Windows;

namespace BuildMaterials.Views
{
    public partial class AddProviderView : Window
    {        
        public AddProviderView()
        {
            InitializeComponent();
            DataContext = new ViewModels.AddProviderViewModel(this);
        }
    }
}