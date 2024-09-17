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
            var selected = ((TreeViewItem)this.TVMonitor.SelectedItem).Tag as string;
            if (!string.IsNullOrEmpty(selected)) {
                SelectedValue = selected;
            }
            this.Close();
        }

        private void BtnAnnuler_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void ListSensor()
        {
            foreach (IHardware hardware in HwMonitor.HwList)
            {
                var hardwareItem = new TreeViewItem
                {
                    Header = hardware.Name,
                    IsExpanded = true,
                };
                foreach (IHardware subhardware in hardware.SubHardware)
                {
                    var subHardwareItem = new TreeViewItem
                    {
                        Header = subhardware.Name,
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
                        Tag = sensor.Identifier.ToString(),
                        IsEnabled = true
                    });
                }

                this.TVMonitor.Items.Add(hardwareItem);
            }
        }
    }
}
