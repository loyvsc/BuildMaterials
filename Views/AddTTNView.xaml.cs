using BuildMaterials.ViewModels;
using System.Windows;
using System.Windows.Documents;
using BuildMaterials.Models;
using System.Collections.Generic;

namespace BuildMaterials.Views
{
    public partial class AddTTNView : Window
    {        
        public AddTTNView()
        {
            InitializeComponent();
            DataContext = new ViewModels.AddTTNViewModel(this);
        }

        public AddTTNView(List<Provider> providers)
        {
            InitializeComponent();
            DataContext = new ViewModels.AddTTNViewModel(this, providers);
        }
    }
}