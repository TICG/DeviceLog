using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls.Primitives;
using DeviceLog.Classes.GUI;
using DeviceLog.Classes.Log;
using DeviceLog.Classes.Modules.Application;
using DeviceLog.Classes.Modules.Clipboard;
using DeviceLog.Classes.Modules.Keyboard;
using Application = System.Windows.Application;
using MessageBox = System.Windows.MessageBox;

namespace DeviceLog.Windows.Main
{
    /// <inheritdoc cref="Syncfusion.Windows.Shared.ChromelessWindow" />
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly UpdateManager.UpdateManager _updateManager;

        private readonly LogController _logController;
        private readonly ApplicationModule _applicationModule;
        private readonly KeyboardModule _keyboardModule;
        private readonly ClipboardModule _clipboardModule;

        public MainWindow()
        {
            InitializeComponent();
            _updateManager = new UpdateManager.UpdateManager(System.Reflection.Assembly.GetExecutingAssembly().GetName().Version, "https://codedead.com/Software/DeviceLog/update.xml", "DeviceLog");

            _logController = new LogController();

            _applicationModule = new ApplicationModule(true, _logController);
            _keyboardModule = new KeyboardModule(true, true, true, false, true, true, _logController);
            _clipboardModule = new ClipboardModule(this, true, _logController);

            LoadTheme();

            try
            {
                if (Properties.Settings.Default.AutoUpdate)
                {
                    _updateManager.CheckForUpdate(false, false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "DeviceLog", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            _applicationModule.AddData("DeviceLog is currently initializing");
        }


        /// <summary>
        /// Change the visual style of the controls, depending on the settings
        /// </summary>
        internal void LoadTheme()
        {
            StyleManager.ChangeStyle(this);
        }

        /// <summary>
        /// Change the text of a togglebutton whenever it becomes (un)checked
        /// </summary>
        /// <param name="sender">The togglebutton which has been (un)checked</param>
        /// <param name="e">The routed event arguments</param>
        private void ToggleButton_CheckChanged(object sender, RoutedEventArgs e)
        {
            ToggleButton toggleButton = (ToggleButton) sender;
            if (toggleButton == null) return;

            toggleButton.Content = toggleButton.IsChecked == true ? "ON" : "OFF";

            switch (toggleButton.Name)
            {
                case "TgbKeyboard":
                    if (toggleButton.IsChecked == true)
                    {
                        _applicationModule.AddData("The keyboard module has been activated");
                        _keyboardModule.Start();
                    }
                    else
                    {
                        _applicationModule.AddData("The keyboard module has been disabled");
                        _keyboardModule.Stop();
                    }
                    break;
                case "TgbClipboard":
                    if (toggleButton.IsChecked == true)
                    {
                        _applicationModule.AddData("The clipboard module has been activated");
                        _clipboardModule.Start();
                    }
                    else
                    {
                        _applicationModule.AddData("The keyboard module has been disabled");
                        _clipboardModule.Stop();
                    }
                    break;
            }
        }

        private void ExportItem_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ExitItem_OnClick(object sender, RoutedEventArgs e)
        {
            _applicationModule.AddData("DeviceLog is shutting down");
            Application.Current.Shutdown();
        }

        private void SettingsItem_OnClick(object sender, RoutedEventArgs e)
        {
            new SettingsWindow().ShowDialog();
        }

        private void HelpItem_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(AppDomain.CurrentDomain.BaseDirectory + "\\help.pdf");
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "DeviceLog", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateItem_OnClick(object sender, RoutedEventArgs e)
        {
            _applicationModule.AddData("DeviceLog is checking for updates");
            _updateManager.CheckForUpdate(true, true);
        }

        private void HomePageItem_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start("https://codedead.com/");
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "DeviceLog", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LicenseItem_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(AppDomain.CurrentDomain.BaseDirectory + "\\gpl.pdf");
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "DeviceLog", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DonateItem_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start("https://codedead.com/?page_id=302");
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "DeviceLog", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AboutItem_OnClick(object sender, RoutedEventArgs e)
        {
            new AboutWindow().ShowDialog();
        }

        private void OpenItem_Click(object sender, RoutedEventArgs e)
        {
            if (IsVisible)
            {
                _applicationModule.AddData("DeviceLog window has been hidden");
                Hide();
            }
            else
            {
                _applicationModule.AddData("DeviceLog window has been shown to the user");
                Show();
            }
        }
    }
}
