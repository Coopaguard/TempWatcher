using LibreHardwareMonitor.Hardware;

namespace tempWatcher.Statics
{
    public static class HwMonitor
    {
        private static UpdateVisitor? _visitor = null;
        private static Computer? _computer = null;

        public static void Init()
        {
            new Task(() =>
            {
                _visitor = new UpdateVisitor();
                _computer = _visitor.GetComputer();
            }).Start();
        }

        public static void Dispose()
        {
            _computer?.Close();
            _computer = null;
            _visitor = null;
        }

        public static Computer? Computer => _computer;

        public static List<IHardware> HwList => _computer == null ? [] : [.. _computer.Hardware];

        public static List<Tuple<string, string>> GetValues(IEnumerable<ElementView> elements)
        {
            var res = new List<Tuple<string, string>>();

            if (_computer != null)
            {
                _computer.Traverse(_visitor);
                var sensors = _computer.Hardware.SelectMany(h => h.Sensors);

                foreach (var element in elements)
                {
                    try
                    {
                        string sensor = Convert.ToString(sensors.First(s => s.Identifier.ToString() == element.CpuidPath).Value) ?? "";
                        if (sensor.Contains(','))
                            sensor = sensor[..(sensor.IndexOf(',') + 2)];

                        if (sensor != null)
                            res.Add(new Tuple<string, string>(element.CpuidPath, sensor));
                    }
                    finally
                    { }
                }
            }

            return res;
        }
    }

    internal class UpdateVisitor : IVisitor
    {
        public void VisitComputer(IComputer computer)
        {
            computer.Traverse(this);
        }
        public void VisitHardware(IHardware hardware)
        {
            hardware.Update();
            foreach (IHardware subHardware in hardware.SubHardware) subHardware.Accept(this);
        }
        public void VisitSensor(ISensor sensor) { }
        public void VisitParameter(IParameter parameter) { }

# pragma warning disable CA1822
        public Computer GetComputer()
        {
            var computer = new Computer
            {
                IsCpuEnabled = true,
                IsGpuEnabled = true,
                IsMemoryEnabled = true,
                IsMotherboardEnabled = true,
                IsControllerEnabled = true,
                IsNetworkEnabled = true,
                IsStorageEnabled = true
            };

            computer.Open();
            computer.Accept(new UpdateVisitor());

            return computer;
        }
# pragma warning restore CA1822
    }
}