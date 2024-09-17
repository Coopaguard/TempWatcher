using System.IO;
using System.Windows;
using System.Windows.Media;

namespace tempWatcher.Statics
{
    public static class ConfState
    {

        #region privates

        private const string _cfgFile = "./Config.json";
        private static readonly System.Text.Json.JsonSerializerOptions serialiserOption = new()
        {
            WriteIndented = true,
            NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString,
            PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };

        #endregion

        public static Config Cfg = new();

        public static void Save()
        {
            File.WriteAllText(_cfgFile, System.Text.Json.JsonSerializer.Serialize(Cfg, serialiserOption));
        }

        public static void Load()
        {
            if (File.Exists(_cfgFile))
            {
                try
                {
                    Cfg = System.Text.Json.JsonSerializer.Deserialize<Config>(File.ReadAllText(_cfgFile), serialiserOption) ?? new();
                }
                catch
                {
                    Cfg = new Config();
                }
            }
            else
            {
                Cfg = new Config();
            }
        }
    }

    public sealed class Config
    {
        public string? BackgroundImagePath { get; set; } = null;

        public Stretch BackgroundStrech { get; set; } = Stretch.None;

        public WindowState WindowState { get; set; } = WindowState.Normal;

        public string WindowsScreen { get; set; } = string.Empty;

        public bool AllayOnTop {  get; set; } = false;

        public bool LockConfig { get; set; } = false;

        public Point? WindowSize {  get; set; }
        public Point? WindowLocation { get; set; }

        public List<ElementView> ElementViews { get; set; } = [];
    }


    public class ElementView
    {
        public string CpuidPath { get; set; } = "";
        public string FontFamily { get; set; } = "Lucida Console";
        public int FontSize { get; set; } = 20;
        public FontStyle FontStyle { get; set; } = FontStyles.Normal;
        public FontStretch FontStretch { get; set; } = FontStretches.Normal;
        public FontWeight FontWeight { get; set; } = FontWeights.Normal;
        public Color Foreground { get; set; } = Colors.BlanchedAlmond;

        public Thickness RatioPoint { get; set; } = new Thickness();

        public ElementView()
        {

        }

        public ElementView(string cpuId)
        {
            this.CpuidPath = cpuId;
        }

    }
}
