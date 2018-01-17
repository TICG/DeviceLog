using System.Windows;
using DeviceLog.Classes.GUI;
using DeviceLog.Classes.Log;

namespace DeviceLog.Windows.Main
{
    /// <inheritdoc cref="Syncfusion.Windows.Shared.ChromelessWindow" />
    /// <summary>
    /// Interaction logic for LogWindow.xaml
    /// </summary>
    public partial class LogWindow
    {
        public LogWindow(Log log)
        {
            InitializeComponent();
            ChangeVisualStyle();

            log.LogUpdatedEvent = LogUpdated;
            TxtLog.Text = log.GetData();
        }

        /// <summary>
        /// Change the visual style of the controls, depending on the settings
        /// </summary>
        private void ChangeVisualStyle()
        {
            StyleManager.ChangeStyle(this);
        }

        private void LogUpdated(string data)
        {
            TxtLog.Text = data;
        }

        private void BtnClose_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
