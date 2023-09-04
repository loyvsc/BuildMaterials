using BuildMaterials.BD;
using System.Windows;

namespace BuildMaterials
{
    public partial class App : Application
    {
        public static ApplicationContext DBContext;

        protected override void OnStartup(StartupEventArgs e)
        {
            DBContext = new ApplicationContext();
            base.OnStartup(e);
        }
    }
}