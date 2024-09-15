using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using System.IO;
using System.Runtime.CompilerServices;

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

        public static Config Cfg = new Config();

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
                    System.Text.Json.JsonSerializer.Deserialize<Config>(File.ReadAllText(_cfgFile), serialiserOption);
                }
                catch
                {
                    Cfg = new Config();
                }
            }

            Cfg = new Config();
        }
    }

    public sealed class Config
    {
        public string? BackgroundImagePath = null;

        public WindowState WindowState = WindowState.Normal;

        public int WindowsScreen = 1;

        public List<ElementView> ElementViews { get; set; } = [];
    }


    public class ElementView
    {
        public string CpuidPath { get; set; } = "";
        public string FontFamily { get; set; } = new FontFamily().FamilyNames.Values.First();
        public int FontSize { get; set; } = 12;
        public FontStyle FontStyle { get; set; } = FontStyles.Normal;
        public FontStretch FontStretch { get; set; } = FontStretches.Normal;
        public FontWeight FontWeight { get; set; } = FontWeights.Normal;
        public Color Foreground { get; set; } = Colors.BlanchedAlmond;

        public Point RatioPoint { get; set; } = new Point(0, 0);

        public ElementView()
        {

        }

        public ElementView(string cpuId)
        {
            this.CpuidPath = cpuId;
        }

    }
}
