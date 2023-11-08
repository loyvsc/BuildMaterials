using System.Windows;

namespace BuildMaterials
{
    public partial class App : System.Windows.Application
    {
        public static BD.ApplicationContext DBContext = null!;

        protected override void OnStartup(StartupEventArgs e)
        {
            DBContext = new BD.ApplicationContext();
            base.OnStartup(e);
        }
    }
}