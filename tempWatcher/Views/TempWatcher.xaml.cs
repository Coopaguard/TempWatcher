using Microsoft.Win32;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using tempWatcher.Statics;

namespace tempWatcher.Views
{
    /// <summary>
    /// Logique d'interaction pour TempWatcher.xaml
    /// </summary>
    public partial class TempWatcher : Window, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler? PropertyChanged;

        public TempWatcher()
        {
            InitializeComponent();
            ConfState.Load();

            RefreshWindows();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ConfState.Save();
        }

        #region Bindable properties

        public bool IsMaximizeState
        {
            get => ConfState.Cfg.WindowState == WindowState.Maximized;
            set {
                if (value)
                {
                    ConfState.Cfg.WindowState = WindowState.Maximized;
                    RefreshWindows();
                }
            }
        }

        public bool IsNormalState { 
            get => ConfState.Cfg.WindowState == WindowState.Normal; 
            set {
                if (value)
                {
                    ConfState.Cfg.WindowState = WindowState.Normal;
                    RefreshWindows();
                }
            } 
        }

        private void BackGroundImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new()
            {
                Filter = "Images files |*.jpg;*.jpeg;*bmp;*.png;*.svg;*.gif",
                Multiselect = false
            };

            Mouse.OverrideCursor = Cursors.Wait;
            Task.Run(() =>
            {
            this.Dispatcher.Invoke(() => Mouse.OverrideCursor = null);
                var found = ofd.ShowDialog();
                if (found.HasValue && found.Value)
                {
                    ConfState.Cfg.BackgroundImagePath = ofd.FileName;
                    this.Dispatcher.Invoke(() => RefreshWindows());
                }
            });
        }

        private void AddElement_Click(object sender, RoutedEventArgs e)
        {
            var selector = new ValueSelectorView();
            selector.ShowDialog();

            if (!string.IsNullOrWhiteSpace(selector.SelectedValue))
            {

                RefreshElements();
            }
        }

        #endregion

        #region privates Windows

        public void RefreshWindows()
        {
            this.WindowState = ConfState.Cfg.WindowState;
            if(!string.IsNullOrWhiteSpace(ConfState.Cfg.BackgroundImagePath) && File.Exists(ConfState.Cfg.BackgroundImagePath))
                this.BackgroundImage.Source = new BitmapImage(new Uri(ConfState.Cfg.BackgroundImagePath));

            //refreshDataContext;
            this.DataContext = null;
            this.DataContext = this;
        }

        public void RefreshElements()
        {

        }

        #endregion

        #region privates Elements

        #endregion

    }
}
