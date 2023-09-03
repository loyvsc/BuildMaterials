using BuildMaterials.BD;
using System.Windows;

namespace BuildMaterials
{
    public partial class App : Application
    {
        public static readonly ApplicationContext DBContext = new();
    }
}