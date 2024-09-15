using LibreHardwareMonitor.Hardware;
using System.Windows;
using System.Windows.Controls;
using tempWatcher.Statics;

namespace tempWatcher.Views
{
    /// <summary>
    /// Logique d'interaction pour ValueSelectorView.xaml
    /// </summary>
    public partial class ValueSelectorView : Window
    {

        public string? SelectedValue = null;

        public ValueSelectorView()
        {
            InitializeComponent();
            ListSensor();
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {

            this.Close();
        }

        private void BtnAnnuler_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void ListSensor()
        {
            foreach (IHardware hardware in UpdateVisitor.Monitors())
            {
                var hardwareItem = new TreeViewItem{ 
                    Header = hardware.Name,
                    IsEnabled = false,
                    IsExpanded = true,
                };
                foreach (IHardware subhardware in hardware.SubHardware)
                {
                    var subHardwareItem = new TreeViewItem
                    {
                        Header = subhardware.Name,
                        IsEnabled = false,
                        IsExpanded = true,
                    };
                    foreach (ISensor sensor in subhardware.Sensors)
                    {
                        subHardwareItem.Items.Add(new TreeViewItem
                        {
                            Header = $"{sensor.Name}: {sensor.Value}",
                            IsEnabled = true
                        });
                    }
                    hardwareItem.Items.Add(subHardwareItem);
                }

                foreach (ISensor sensor in hardware.Sensors)
                {
                    hardwareItem.Items.Add(new TreeViewItem
                    {
                        Header = $"{sensor.Name}: {sensor.Value} {sensor.SensorType}",
                        IsEnabled = true
                    });
                }

                TVMonitor.Items.Add(hardwareItem);
            }
        }
    }
}
