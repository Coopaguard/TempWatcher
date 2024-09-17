using System.ComponentModel;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using tempWatcher.Statics;
using tempWatcher.Views;

namespace tempWatcher.Customs
{
    /// <summary>
    /// Logique d'interaction pour CustomLabel.xaml
    /// </summary>
    public partial class CustomLabel : UserControl
    {
        public ElementView ElementView;
        private readonly TempWatcher _mainView;

        public ElementView Element { get => ElementView; }

        public CustomLabel(ElementView srcElement, TempWatcher mainView)
        {
            InitializeComponent();
            ElementView = srcElement;
            _mainView = mainView;
            this.TextLbl.Content = srcElement.CpuidPath;
            Refresh();
        }

        public void Refresh(ElementView? v = null)
        {
            this.DataContext = null;
            if(v != null)
            {
                this.ElementView = v;
            }

            this.DataContext = this;
            this.TextLbl.Foreground = new SolidColorBrush(ElementView.Foreground);
        }

        public void SetValue(string value)
        {
            this.TextLbl.Content = value;
        }

        private void TextLbl_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            base.OnGiveFeedback(e);
            // These Effects values are set in the drop target's
            // DragOver event handler.
            if (e.Effects.HasFlag(DragDropEffects.Move))
            {
                Mouse.SetCursor(Cursors.Pen);
            }
            else
            {
                Mouse.SetCursor(Cursors.No);
            }
            e.Handled = true;
        }

        private void UserControl_MouseMove(object sender, MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if(e.LeftButton == MouseButtonState.Pressed && _mainView.MovingElement == null)
            {
                Mouse.OverrideCursor = Cursors.Hand;
                _mainView.MovingElement = this;
            }
        }

        private void DelSensor_Click(object sender, RoutedEventArgs e)
        {
            ConfState.Cfg.ElementViews.Remove(ElementView);
            Task.Run(() => {
                _mainView.Dispatcher.Invoke(() => _mainView.RefreshElements());
            });
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            var diag = new DesignLabel(ElementView, this);
            diag.ShowDialog();
        }
    }
}
