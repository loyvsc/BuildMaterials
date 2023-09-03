using BuildMaterials.BD;
using System.Windows;

namespace BuildMaterials
{
    public partial class App : Application
    {
        public static ApplicationContext DBContext = null!;
        public App()
        {
            DBContext = new ApplicationContext();
        }
    }
}