using System.Windows;
using tempWatcher.Statics;

namespace tempWatcher
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            HwMonitor.Init();
        }
    }
}
