using ColorPicker.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using tempWatcher.Customs;
using tempWatcher.Statics;

namespace tempWatcher.Views
{
    /// <summary>
    /// Logique d'interaction pour DesignLabel.xaml
    /// </summary>
    public partial class DesignLabel : Window
    {
        private readonly ElementView _elementView;
        private readonly CustomLabel _customLabel;

        public DesignLabel(ElementView element, CustomLabel sender)
        {
            InitializeComponent();

            _elementView = element;
            _customLabel = sender;

            //Binding source
            foreach (var f in Lists.FontFamilies)
                CbxFontFamily.Items.Add(new ComboBoxItem
                {
                    Content = f,
                    FontFamily = new FontFamily(f),
                    IsSelected = f == _elementView.FontFamily
                });
            CbxFontStrech.ItemsSource = Lists.FontsStrech.Select(w => w.Item1);
            CbxFontWeight.ItemsSource = Lists.FontsWeight.Select(w => w.Item1);
            AlphaColorSlider.Value = (double)_elementView.Foreground.A;

            this.DataContext = element;

            CbxFontFamily.SelectedItem = _elementView.FontFamily;
            CbxFontStrech.SelectedItem = Lists.FontsStrech.First(p => p.Item2 == _elementView.FontStretch).Item1;
            CbxFontWeight.SelectedItem = Lists.FontsWeight.First(p => p.Item2 == _elementView.FontWeight).Item1;
            var cStat = new ColorState();
            cStat.SetARGB(_elementView.Foreground.A, _elementView.Foreground.R, _elementView.Foreground.G, _elementView.Foreground.B);
            ForegroundPicker.ColorState = cStat;
            ForegroundPicker.ColorChanged += Foreground_ColorChanged;
        }

        #region Size

        private void AddSize_Click(object sender, RoutedEventArgs e)
        {
            var nval = Convert.ToInt32(TbSize.Text) + 1;
            if(nval <= 300)
            TbSize.Text = nval.ToString();
        }

        private void RmSize_Click(object sender, RoutedEventArgs e)
        {
            var nval = Convert.ToInt32(TbSize.Text) - 1;
            if (nval > 0)
                TbSize.Text = nval.ToString();
        }

        private void TbSize_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Int32.TryParse(TbSize.Text, out var nval))
            {
                if (nval < 0)
                    TbSize.Text = "0";

                if (nval < 0)
                    TbSize.Text = "300";

                _elementView.FontSize = nval;
                _customLabel.Refresh(_elementView);
            }
            e.Handled = true;
        }

        #endregion

        #region Combo

        private void CbxFontFamily_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.CbxFontFamily.SelectedValue is string selected && selected != _elementView.FontFamily)
            {
                _elementView.FontFamily = selected;
                _customLabel.Refresh(_elementView);
            }

            e.Handled = true;
        }

        private void CbxFontWeight_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string? selected = this.CbxFontWeight.SelectedValue as string;
            var pair = Lists.FontsWeight.First(w => w.Item1 == selected);

            if (selected != null && pair.Item2 != _elementView.FontWeight)
            {
                _elementView.FontWeight = pair.Item2;
                _customLabel.Refresh(_elementView);
            }

            e.Handled = true;
        }

        private void CbxFontStrech_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string? selected = this.CbxFontStrech.SelectedValue as string;
            var pair = Lists.FontsStrech.First(w => w.Item1 == selected);

            if (selected != null && pair.Item2 != _elementView.FontStretch)
            {
                _elementView.FontStretch = pair.Item2;
                _customLabel.Refresh(_elementView);
            }

            e.Handled = true;
        }

        #endregion

        private void Foreground_ColorChanged(object sender, RoutedEventArgs e)
        {
            var selected = this.ForegroundPicker.Color;

            if (_elementView.Foreground.R != selected.RGB_R
                || _elementView.Foreground.G != selected.RGB_G
                || _elementView.Foreground.B != selected.RGB_B)
            {
                _elementView.Foreground = new Color() { 
                    R = (byte)selected.RGB_R,
                    G = (byte)selected.RGB_G,
                    B = (byte)selected.RGB_B,
                    A = (byte)this.AlphaColorSlider.Value
                };
                _customLabel.Refresh(_elementView);
            }

            e.Handled = true;
        }

        private void AlphaColorSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var selected = this.AlphaColorSlider.Value;

            if((byte)selected != _elementView.Foreground.A)
            {
                _elementView.Foreground = new Color()
                {
                    R = _elementView.Foreground.R,
                    G = _elementView.Foreground.G,
                    B = _elementView.Foreground.B,
                    A = (byte)selected
                };
                _customLabel.Refresh(_elementView);
            }

            e.Handled = true;
        }
    }
}
