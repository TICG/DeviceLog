using System;
using System.Windows;
using DeviceLog.Classes.GUI;
using DeviceLog.Classes.Log;
using DeviceLog.Classes.Modules.Application;

namespace DeviceLog.Windows.Main
{
    /// <inheritdoc cref="Syncfusion.Windows.Shared.ChromelessWindow" />
    /// <summary>
    /// Interaction logic for LogWindow.xaml
    /// </summary>
    public partial class LogWindow
    {

        private readonly bool _scrollToEnd;

        public LogWindow(Log log, ApplicationModule applicationModule)
        {
            InitializeComponent();
            ChangeVisualStyle();

            log.LogUpdatedEvent = LogUpdated;
            TxtLog.Text = log.GetData();

            try
            {
                _scrollToEnd = Properties.Settings.Default.LogWindow_ScrollToEnd;
            }
            catch (Exception ex)
            {
                applicationModule?.AddData("DeviceLog was unable to read the settings file in the log window: " + ex.Message);
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
        /// Event that fires whenever the log gets updated
        /// </summary>
        /// <param name="data">The new log data</param>
        private void LogUpdated(string data)
        {
            TxtLog.Text = data;
            if (_scrollToEnd)
            {
                TxtLog.ScrollToEnd();
            }
        }

        private void BtnClose_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
