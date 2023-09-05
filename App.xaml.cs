using BuildMaterials.BD;
using System.Windows;

namespace BuildMaterials
{
    public partial class App : System.Windows.Application
    {
        public static BuildMaterials.BD.ApplicationContext DBContext = null!;

        protected override void OnStartup(StartupEventArgs e)
        {
            DBContext = new BuildMaterials.BD.ApplicationContext();
            base.OnStartup(e);
        }
    }
}