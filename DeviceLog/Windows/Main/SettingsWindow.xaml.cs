using System;
using System.Windows;
using DeviceLog.Classes.GUI;

namespace DeviceLog.Windows.Main
{
    /// <inheritdoc cref="Syncfusion.Windows.Shared.ChromelessWindow" />
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow
    {

        private readonly MainWindow _mainWindow;

        public SettingsWindow(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;

            InitializeComponent();
            ChangeVisualStyle();

            LoadSettings();
        }

        /// <summary>
        /// Load all appropriate settings into the GUI
        /// </summary>
        private void LoadSettings()
        {
            try
            {
                CboStyle.SelectedValue = Properties.Settings.Default.VisualStyle;
                CpMetroBrush.Color = Properties.Settings.Default.MetroColor;
                IntBorderThickness.Value = Properties.Settings.Default.BorderThickness;
                ChbLogApplication.IsChecked = Properties.Settings.Default.Application_Log;

                //Keyboard
                ChbKeyBoardControlCharacters.IsChecked = Properties.Settings.Default.KeyBoard_ControlCharacters;
                ChbKeyBoardEnterNewLine.IsChecked = Properties.Settings.Default.KeyBoard_EnterNewLine;
                ChbKeyBoardSpecialCharacters.IsChecked = Properties.Settings.Default.Keyboard_SpecialKeys;
                ChbKeyBoardWindowTitle.IsChecked = Properties.Settings.Default.KeyBoard_WindowTitle;

                //Clipboard
                ChbClipboardLogDate.IsChecked = Properties.Settings.Default.ClipBoard_LogDateTime;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "DeviceLog", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Save all appropriate settings in the Settings repository
        /// </summary>
        private void SaveSettings()
        {
            try
            {
                Properties.Settings.Default.VisualStyle = CboStyle.Text;
                Properties.Settings.Default.MetroColor = CpMetroBrush.Color;
                if (IntBorderThickness.Value != null) Properties.Settings.Default.BorderThickness = (int) IntBorderThickness.Value;
                if (ChbLogApplication.IsChecked != null) Properties.Settings.Default.Application_Log = ChbLogApplication.IsChecked.Value;

                //Keyboard
                if (ChbKeyBoardControlCharacters.IsChecked != null) Properties.Settings.Default.KeyBoard_ControlCharacters = ChbKeyBoardControlCharacters.IsChecked.Value;
                if (ChbKeyBoardEnterNewLine.IsChecked != null) Properties.Settings.Default.KeyBoard_EnterNewLine = ChbKeyBoardEnterNewLine.IsChecked.Value;
                if (ChbKeyBoardSpecialCharacters.IsChecked != null) Properties.Settings.Default.Keyboard_SpecialKeys = ChbKeyBoardSpecialCharacters.IsChecked.Value;
                if (ChbKeyBoardWindowTitle.IsChecked != null) Properties.Settings.Default.KeyBoard_WindowTitle = ChbKeyBoardWindowTitle.IsChecked.Value;
                //Clipboard
                if (ChbClipboardLogDate.IsChecked != null) Properties.Settings.Default.ClipBoard_LogDateTime = ChbClipboardLogDate.IsChecked.Value;

                Properties.Settings.Default.Save();

                _mainWindow.LoadTheme();
                _mainWindow.LoadApplicationModule();
                _mainWindow.LoadKeyBoardModule();
                _mainWindow.LoadClipboardModule();

                ChangeVisualStyle();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "DeviceLog", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Reset all settings to their original values and reload the GUI
        /// </summary>
        private void ResetSettings()
        {
            try
            {
                Properties.Settings.Default.Reset();
                Properties.Settings.Default.Save();

                _mainWindow.LoadTheme();
                _mainWindow.LoadApplicationModule();
                _mainWindow.LoadKeyBoardModule();
                _mainWindow.LoadClipboardModule();

                ChangeVisualStyle();

                LoadSettings();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "DeviceLog", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Change the visual style of the controls, depending on the settings
        /// </summary>
        private void ChangeVisualStyle()
        {
            StyleManager.ChangeStyle(this);
        }

        /// <summary>
        /// Save all settings
        /// </summary>
        /// <param name="sender">The object that called this method</param>
        /// <param name="e">The routed event arguments</param>
        private void BtnSave_OnClick(object sender, RoutedEventArgs e)
        {
            SaveSettings();
        }

        /// <summary>
        /// Reset all settings
        /// </summary>
        /// <param name="sender">The object that called this method</param>
        /// <param name="e">The routed event arguments</param>
        private void BtnReset_OnClick(object sender, RoutedEventArgs e)
        {
            ResetSettings();
        }
    }
}
