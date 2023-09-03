using System.Windows;

namespace BuildMaterials.Views
{
    public partial class AddCustomerView : Window
    {        
        public AddCustomerView()
        {
            InitializeComponent();
            DataContext = new ViewModels.AddCustomerViewModel(this);
        }
    }
}