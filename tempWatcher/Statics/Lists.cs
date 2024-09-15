using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;

namespace tempWatcher.Statics
{
    public static class Lists
    {
        public static List<FontFamily> FontsList => [.. Fonts.SystemFontFamilies];

        public static readonly List<Tuple<string, WindowState>> WindowStates = [
            new ("Normal", WindowState.Normal),
        new ("Maximized",WindowState.Maximized),
        new ("Minimized",WindowState.Minimized)
        ];
        private static List<string> FontFamilies => ["Aharoni", "Andalus", "AngsanaUPC", "Angsana New", "Arabic Transparent", "Arial", "Arial Black", "Batang", "BrowalliaUPC", "Browallia New", "Comic Sans MS", "CordiaUPC", "Cordia New", "Courier New", "David", "DFKai-SB", "DilleniaUPC", "Estrangelo Edessa", "EucrosiaUPC", "Fixed Miriam Transparent", "Franklin Gothic", "FrankRuehl", "FreesiaUPC", "Gautami", "Georgia", "Gulim", "Impact", "IrisUPC", "JasmineUPC", "KaiTi", "Kartika", "KodchiangUPC", "Latha", "Levenim MT", "LilyUPC", "Lucida Console", "Lucida Sans", "Lucida Sans Unicode", "Mangal", "Marlett", "Microsoft Sans Serif", "PMingLiU", "Miriam", "Miriam Fixed", "MS Gothic", "MS Mincho", "MV Boli", "Narkisim", "Palatino Linotype", "PMingLiU-ExtB", "Raavi", "Rod", "Shruti", "SimHei", "Simplified Arabic Fixed", "Simplified Arabic Fixed", "SimSun-ExtB", "Sylfaen", "Symbol", "Tahoma", "Times New Roman", "Traditional Arabic", "Trebuchet MS", "Tunga", "Verdana", "Vrinda", "Webdings", "Wingdings"];

        public static List<Tuple<string, FontStyle>> FontsStyle => [
            new("Normal", FontStyles.Normal),
            new("Italic", FontStyles.Italic),
            new("Oblique", FontStyles.Oblique)
        ];

        public static List<Tuple<string, FontStretch>> FontsStrech => [
            new("Normal", FontStretches.Normal),
            new("Medium", FontStretches.Medium),
            new("SemiExpanded", FontStretches.SemiExpanded),
            new("ExtraExpanded", FontStretches.ExtraExpanded),
            new("UltraExpanded", FontStretches.UltraExpanded),
            new("SemiCondensed", FontStretches.SemiCondensed),
            new("ExtraCondensed", FontStretches.ExtraCondensed),
            new("UltraCondensed", FontStretches.UltraCondensed)
        ];

        public static List<Tuple<string, FontWeight>> FontsWeight => [
            new("Normal", FontWeights.Normal),
            new("Regular", FontWeights.Regular),
            new("DemiBold", FontWeights.DemiBold),
            new("Bold", FontWeights.Bold),
            new("ExtraBold", FontWeights.ExtraBold),
            new("UltraBold", FontWeights.UltraBold),
            new("Heavy", FontWeights.Heavy),
            new("Thin", FontWeights.Thin),
            new("Light", FontWeights.Light),
            new("UltraLight", FontWeights.UltraLight),
        ];
    }
}
