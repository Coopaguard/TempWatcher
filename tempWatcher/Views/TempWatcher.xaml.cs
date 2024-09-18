using Microsoft.Win32;
using System.IO;
using System.Reflection.PortableExecutable;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using tempWatcher.Customs;
using tempWatcher.Statics;
using WpfScreenHelper;
using WpfScreenHelper.Enum;

namespace tempWatcher.Views
{
    /// <summary>
    /// Logique d'interaction pour TempWatcher.xaml
    /// </summary>
    public partial class TempWatcher : Window
    {
        readonly Task SyncRunner;
        readonly CancellationTokenSource Canceler = new();
        readonly CancellationToken CancelToken;

        public TempWatcher()
        {
            CancelToken = Canceler.Token;
            InitializeComponent();
            ConfState.Load();

            RefreshWindows();
            RefreshElements();

            var rk = Registry.CurrentUser.OpenSubKey
                ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                this.StartWithWindows.IsChecked = rk?.GetValue(this.Name) != null;

            SyncRunner = new Task(() =>
            {
                while (true)
                {
                    Thread.Sleep(1000);

                    var vals = HwMonitor.GetValues(ConfState.Cfg.ElementViews);
                    MainContainer.Dispatcher.Invoke(() => SetValues(vals));

                }
            }, CancelToken);
            SyncRunner.Start();
        }

        public CustomLabel? MovingElement;

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            HwMonitor.Dispose();
            Canceler.Cancel();
            ConfState.Save();
        }

        private void SetValues(List<Tuple<string, string>> values)
        {
            foreach (var item in MainContainer.Dispatcher.Invoke(() => MainContainer.Children))
            {
                if (item is CustomLabel cust)
                {
                    cust.Dispatcher.Invoke(() =>
                        cust.SetValue(values.FirstOrDefault(v => v.Item1 == cust.Element.CpuidPath)?.Item2 ?? ""));
                }
            }
        }

        #region Bindable properties

        public bool IsMaximizeState
        {
            get => ConfState.Cfg.WindowState == WindowState.Maximized;
            set
            {
                if (value)
                {
                    ConfState.Cfg.WindowState = WindowState.Maximized;
                    RefreshWindows();
                }
            }
        }

        public bool IsNormalState
        {
            get => ConfState.Cfg.WindowState == WindowState.Normal;
            set
            {
                if (value)
                {
                    ConfState.Cfg.WindowState = WindowState.Normal;
                    RefreshWindows();
                }
            }
        }

        public bool IsStrechFill
        {
            get => ConfState.Cfg.BackgroundStrech == Stretch.Fill;
            set
            {
                if (value)
                {
                    ConfState.Cfg.BackgroundStrech = Stretch.Fill;
                    RefreshWindows();
                }
            }
        }

        public bool IsStrechUniform
        {
            get => ConfState.Cfg.BackgroundStrech == Stretch.Uniform;
            set
            {
                if (value)
                {
                    ConfState.Cfg.BackgroundStrech = Stretch.Uniform;
                    RefreshWindows();
                }
            }
        }

        public bool IsStrechUniformToFill
        {
            get => ConfState.Cfg.BackgroundStrech == Stretch.UniformToFill;
            set
            {
                if (value)
                {
                    ConfState.Cfg.BackgroundStrech = Stretch.UniformToFill;
                    RefreshWindows();
                }
            }
        }

        public bool IsStrechNone
        {
            get => ConfState.Cfg.BackgroundStrech == Stretch.None;
            set
            {
                if (value)
                {
                    ConfState.Cfg.BackgroundStrech = Stretch.None;
                    RefreshWindows();
                }
            }
        }

        public bool IsOverlay
        {
            get => ConfState.Cfg.AllayOnTop;
            set
            {
                ConfState.Cfg.AllayOnTop = value;
                RefreshWindows();
            }
        }

        public bool IsLock
        {
            get => ConfState.Cfg.LockConfig;
            set
            {
                ConfState.Cfg.LockConfig = value;
                this.MainContainer.IsEnabled = !value;
            }
        }

        #endregion

        #region Context Menu

        private void ContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            // Load screen List
            ScreenSelector.Items.Clear();
            foreach (var s in WpfScreenHelper.Screen.AllScreens)
            {
                var item = new MenuItem
                {
                    Header = $"Ecran {s.DeviceName}",
                    Tag = s.DeviceName,
                    IsCheckable = true,
                    IsChecked = ConfState.Cfg.WindowsScreen == s.DeviceName || (s.Primary && ConfState.Cfg.WindowsScreen == string.Empty)
                };
                item.Click += ScreenChange;

                ScreenSelector.Items.Add(item);
            }
        }

        private void BackGroundImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new()
            {
                Filter = "Images files |*.jpg;*.jpeg;*bmp;*.png;*.svg;*.gif",
                Multiselect = false
            };

            var found = ofd.ShowDialog();
            if (found.HasValue && found.Value)
            {
                ConfState.Cfg.BackgroundImagePath = ofd.FileName;
                this.Dispatcher.Invoke(() => RefreshWindows());
            }
        }

        private void AddElement_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            var selector = new ValueSelectorView();

            this.Dispatcher.Invoke(() => Mouse.OverrideCursor = null);
            selector.ShowDialog();

            if (!string.IsNullOrWhiteSpace(selector.SelectedValue))
            {
                ConfState.Cfg.ElementViews.Add(new ElementView(selector.SelectedValue));
                RefreshElements();
            }
        }

        private void ScreenChange(object sender, RoutedEventArgs e)
        {
            var selected = ((MenuItem)sender).Tag as string ?? "";
            if (selected != ConfState.Cfg.WindowsScreen)
            {
                ConfState.Cfg.WindowsScreen = selected;
                this.SetWindowPosition(WindowPositions.Center, Screen.AllScreens.First(s => s.DeviceName == selected));
            }
        }

        private void StartWithWindows_Click(object sender, RoutedEventArgs e)
        {
            var rk = Registry.CurrentUser.OpenSubKey
                ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            if (rk != null && this.StartWithWindows.IsChecked)
                rk.SetValue(this.Name, System.Reflection.Assembly.GetExecutingAssembly().Location);
            if (rk != null && !this.StartWithWindows.IsChecked)
                rk.DeleteValue(this.Name, false);
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            ConfState.Save();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            HwMonitor.Dispose();
            this.Close();
        }

        #endregion

        #region privates Windows

        public void RefreshWindows()
        {
            this.BackgroundImage.Stretch = ConfState.Cfg.BackgroundStrech;

            if (string.IsNullOrEmpty(ConfState.Cfg.WindowsScreen))
                ConfState.Cfg.WindowsScreen = Screen.PrimaryScreen.DeviceName;

            this.SetWindowPosition(
                WindowPositions.Center,
                Screen.AllScreens.First(s => s.DeviceName == ConfState.Cfg.WindowsScreen));

            this.Topmost = ConfState.Cfg.AllayOnTop;

            //Set Size & location
            if (ConfState.Cfg.WindowSize != null)
            {
                this.Height = ConfState.Cfg.WindowSize.Value.Y;
                this.Width = ConfState.Cfg.WindowSize.Value.X;
            }

            if (ConfState.Cfg.WindowLocation != null)
            {
                this.Left = ConfState.Cfg.WindowLocation.Value.X;
                this.Top = ConfState.Cfg.WindowLocation.Value.Y;
            }

            //Set State
            this.WindowState = ConfState.Cfg.WindowState;
            if (!string.IsNullOrWhiteSpace(ConfState.Cfg.BackgroundImagePath) && File.Exists(ConfState.Cfg.BackgroundImagePath))
                this.BackgroundImage.Source = new BitmapImage(new Uri(ConfState.Cfg.BackgroundImagePath));


            //refreshDataContext;
            this.DataContext = null;
            this.DataContext = this;

            //Lock Grid
            this.MainContainer.IsEnabled = !ConfState.Cfg.LockConfig;
        }

        public void RefreshElements()
        {
            //Clear container
            ((Grid)this.MainContainer).Children.RemoveRange(1, ((Grid)this.MainContainer).Children.Count -1);

            foreach (var el in ConfState.Cfg.ElementViews)
            {
                var cust = new CustomLabel(el, this);
                ((Grid)this.MainContainer).Children.Add(cust);
            }
        }

        private void MainContainer_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && MovingElement != null)
            {
                Point position = e.GetPosition(this);
                MovingElement.Margin = new Thickness(position.X, position.Y, 0, 0);
            }

            if (e.LeftButton == MouseButtonState.Released && MovingElement != null)
            {
                Mouse.OverrideCursor = null;
                MovingElement.ElementView.RatioPoint = MovingElement.Margin;
                MovingElement = null;
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.ButtonState == MouseButtonState.Pressed)
                this.DragMove();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ConfState.Cfg.WindowSize = new(this.ActualWidth, this.ActualHeight);
        }

        private void Window_LocationChanged(object sender, EventArgs e)
        {
            ConfState.Cfg.WindowLocation = new Point(this.Left, this.Top);
        }

        #endregion

        #region Tray

        private void TrayClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void TrayOverlay_Click(object sender, RoutedEventArgs e)
        {
            ConfState.Cfg.AllayOnTop = true;
            RefreshWindows();
        }

        #endregion

    }
}
