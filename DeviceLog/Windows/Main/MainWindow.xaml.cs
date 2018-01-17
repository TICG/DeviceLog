using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls.Primitives;
using DeviceLog.Classes.GUI;
using DeviceLog.Classes.Log;
using DeviceLog.Classes.Modules.Application;
using DeviceLog.Classes.Modules.Clipboard;
using DeviceLog.Classes.Modules.FileSystem;
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
        private ApplicationModule _applicationModule;
        private KeyboardModule _keyboardModule;
        private ClipboardModule _clipboardModule;
        private FileSystemModule _fileSystemModule;

        public MainWindow()
        {
            _logController = new LogController();
            LoadApplicationModule();
            _applicationModule?.AddData("DeviceLog is currently initializing...");

            InitializeComponent();
            _updateManager = new UpdateManager.UpdateManager(System.Reflection.Assembly.GetExecutingAssembly().GetName().Version, "https://codedead.com/Software/DeviceLog/update.xml", "DeviceLog");

            LoadKeyBoardModule();
            LoadClipboardModule();
            LoadFileSystemModule();

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
            _applicationModule?.AddData("DeviceLog is done initializing.");
        }

        /// <summary>
        /// Load the ApplicationModule
        /// </summary>
        internal void LoadApplicationModule()
        {
            _applicationModule = Properties.Settings.Default.Application_Log ? new ApplicationModule(true, _logController) : null;
        }

        /// <summary>
        /// Load the KeyBoardModule and set all the approperiate settings
        /// </summary>
        internal void LoadKeyBoardModule()
        {
            _applicationModule?.AddData("DeviceLog is currently loading the keyboard module...");
            try
            {
                bool special = Properties.Settings.Default.Keyboard_SpecialKeys;
                bool control = Properties.Settings.Default.KeyBoard_ControlCharacters;
                bool enterNewLine = Properties.Settings.Default.KeyBoard_EnterNewLine;
                bool windowTitle = Properties.Settings.Default.KeyBoard_WindowTitle;

                if (_keyboardModule != null)
                {
                    _keyboardModule.SetLogSpecialCharacters(special);
                    _keyboardModule.SetLogControlCharacters(control);
                    _keyboardModule.SetLogEnterKeyNewLines(enterNewLine);
                    _keyboardModule.SetLogWindowTitle(windowTitle);
                }
                else
                {
                    _keyboardModule = new KeyboardModule(special, control, enterNewLine, false, true, windowTitle, _logController);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "DeviceLog", MessageBoxButton.OK, MessageBoxImage.Error);
                _applicationModule?.AddData("Error occured while loading keyboard module: " + ex.Message);
            }
            _applicationModule?.AddData("DeviceLog is done loading the keyboard module.");
        }

        /// <summary>
        /// Load the ClipBoardModule and set all the appropriate settings
        /// </summary>
        internal void LoadClipboardModule()
        {
            _applicationModule?.AddData("DeviceLog is currently loading the clipboard module...");
            try
            {
                bool logDate = Properties.Settings.Default.ClipBoard_LogDateTime;
                if (_clipboardModule != null)
                {
                    _clipboardModule.SetLogDate(logDate);
                }
                else
                {
                    _clipboardModule = new ClipboardModule(this, logDate, _logController);
                }
            }
            catch (Exception ex)
            {
                _applicationModule?.AddData("Error occured while loading keyboard module: " + ex.Message);
                MessageBox.Show(ex.Message, "DeviceLog", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            _applicationModule?.AddData("DeviceLog is done loading the clipboard module.");
        }

        internal void LoadFileSystemModule()
        {
            _applicationModule?.AddData("DeviceLog is currently loading the FileSystem module...");
            _fileSystemModule = new FileSystemModule(Path.GetPathRoot(Environment.SystemDirectory), "*.*", true, true, true, true, true, _logController);
            _applicationModule?.AddData("DeviceLog is done loading the FileSystem module.");
        }

        /// <summary>
        /// Change the visual style of the controls, depending on the settings
        /// </summary>
        internal void LoadTheme()
        {
            _applicationModule?.AddData("DeviceLog is currently changing the theme...");
            StyleManager.ChangeStyle(this);
            _applicationModule?.AddData("DeviceLog is done changing the theme.");
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
                        _applicationModule?.AddData("The keyboard module has been activated!");
                        _keyboardModule.Start();
                    }
                    else
                    {
                        _applicationModule?.AddData("The keyboard module has been disabled!");
                        _keyboardModule.Stop();
                    }
                    break;
                case "TgbClipboard":
                    if (toggleButton.IsChecked == true)
                    {
                        _applicationModule?.AddData("The clipboard module has been activated!");
                        _clipboardModule.Start();
                    }
                    else
                    {
                        _applicationModule?.AddData("The keyboard module has been disabled!");
                        _clipboardModule.Stop();
                    }
                    break;
                case "TgbFileSystem":
                    if (toggleButton.IsChecked == true)
                    {
                        _applicationModule?.AddData("The FileSystem module has been activated!");
                        _fileSystemModule.Start();
                    }
                    else
                    {
                        _applicationModule?.AddData("The FileSystem module has been disabled!");
                        _fileSystemModule.Stop();
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
            _applicationModule?.AddData("DeviceLog is shutting down");
            Application.Current.Shutdown();
        }

        private void SettingsItem_OnClick(object sender, RoutedEventArgs e)
        {
            _applicationModule?.AddData("DeviceLog is currently showing the settings window...");
            new SettingsWindow(this).ShowDialog();
            _applicationModule?.AddData("DeviceLog is done showing the settings window..");
        }

        private void HelpItem_OnClick(object sender, RoutedEventArgs e)
        {
            _applicationModule?.AddData("DeviceLog is currently loading the help documentation...");
            try
            {
                Process.Start(AppDomain.CurrentDomain.BaseDirectory + "\\help.pdf");
            }
            catch (Exception ex)
            {
                _applicationModule?.AddData("Error occured while loading help documentation: " + ex.Message);
                MessageBox.Show(this, ex.Message, "DeviceLog", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            _applicationModule?.AddData("DeviceLog is done loading the help documentation.");
        }

        private void UpdateItem_OnClick(object sender, RoutedEventArgs e)
        {
            _applicationModule?.AddData("DeviceLog is currently checking for updates...");
            _updateManager.CheckForUpdate(true, true);
            _applicationModule?.AddData("DeviceLog is done checking for updates...");
        }

        private void HomePageItem_OnClick(object sender, RoutedEventArgs e)
        {
            _applicationModule?.AddData("DeviceLog is currently opening the CodeDead website...");
            try
            {
                Process.Start("https://codedead.com/");
            }
            catch (Exception ex)
            {
                _applicationModule?.AddData("Error occured while opening CodeDead site: " + ex.Message);
                MessageBox.Show(this, ex.Message, "DeviceLog", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            _applicationModule?.AddData("DeviceLog is done opening the CodeDead website.");
        }

        private void LicenseItem_OnClick(object sender, RoutedEventArgs e)
        {
            _applicationModule?.AddData("DeviceLog is currently loading the license...");
            try
            {
                Process.Start(AppDomain.CurrentDomain.BaseDirectory + "\\gpl.pdf");
            }
            catch (Exception ex)
            {
                _applicationModule?.AddData("Error occured while loading license: " + ex.Message);
                MessageBox.Show(this, ex.Message, "DeviceLog", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            _applicationModule?.AddData("DeviceLog is done loading the license.");
        }

        private void DonateItem_OnClick(object sender, RoutedEventArgs e)
        {
            _applicationModule?.AddData("DeviceLog is currently loading the donation page...");
            try
            {
                Process.Start("https://codedead.com/?page_id=302");
            }
            catch (Exception ex)
            {
                _applicationModule?.AddData("Error occured while loading donation page: " + ex.Message);
                MessageBox.Show(this, ex.Message, "DeviceLog", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            _applicationModule?.AddData("DeviceLog is done loading the donation page...");
        }

        private void AboutItem_OnClick(object sender, RoutedEventArgs e)
        {
            _applicationModule?.AddData("DeviceLog is currently showing the About window...");
            new AboutWindow().ShowDialog();
            _applicationModule?.AddData("DeviceLog is done showing the About window.");
        }

        private void OpenItem_Click(object sender, RoutedEventArgs e)
        {
            if (IsVisible)
            {
                _applicationModule?.AddData("DeviceLog window has been hidden.");
                Hide();
            }
            else
            {
                _applicationModule?.AddData("DeviceLog window has been shown to the user.");
                Show();
            }
        }

        private void BtnKeyBoardLogs_OnClick(object sender, RoutedEventArgs e)
        {
            _applicationModule?.AddData("DeviceLog is currently showing the keyboard logs to the user...");
            Log l = _logController.GetKeyboardLogs()[_logController.GetKeyboardLogs().Count - 1];
            new LogWindow(l).ShowDialog();
            _applicationModule?.AddData("DeviceLog is done showing the keyboard logs to the user.");
        }

        private void BtnClipboardLogs_OnClick(object sender, RoutedEventArgs e)
        {
            _applicationModule?.AddData("DeviceLog is currently showing the clipboard logs to the user...");
            Log l = _logController.GetClipboardLogs()[_logController.GetClipboardLogs().Count - 1];
            new LogWindow(l).ShowDialog();
            _applicationModule?.AddData("DeviceLog is done showing the clipboard logs to the user.");
        }

        private void ApplicationLogsItem_OnClick(object sender, RoutedEventArgs e)
        {
            if (_applicationModule == null || _applicationModule?.GetLog() == null)
            {
                return;
            }
            _applicationModule?.AddData("DeviceLog is currently showing the application logs to the user...");
            new LogWindow(_applicationModule?.GetLog()).ShowDialog();
            _applicationModule?.AddData("DeviceLog is done showing the application logs to the user.");
        }

        private void BtnFileSystemLogs_OnClick(object sender, RoutedEventArgs e)
        {
            _applicationModule?.AddData("DeviceLog is currently showing the FileSystem logs to the user...");
            Log l = _logController.GetFileSystemLogs()[_logController.GetFileSystemLogs().Count - 1];
            new LogWindow(l).ShowDialog();
            _applicationModule?.AddData("DeviceLog is done showing the FileSystem logs to the user.");
        }
    }
}
