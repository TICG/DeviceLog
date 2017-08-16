﻿using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls.Primitives;
using DeviceLog.Classes.GUI;
using DeviceLog.Classes.Log;
using DeviceLog.Classes.Modules.Keyboard;
using Application = System.Windows.Application;
using MessageBox = System.Windows.MessageBox;

namespace DeviceLog.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly UpdateManager.UpdateManager _updateManager;

        private readonly LogController _logController;
        private readonly KeyboardModule _keyboardModule;

        private readonly bool _logDateTime;

        public MainWindow()
        {
            InitializeComponent();
            _updateManager = new UpdateManager.UpdateManager(System.Reflection.Assembly.GetExecutingAssembly().GetName().Version, "https://codedead.com/Software/DeviceLog/update.xml", "DeviceLog");

            _logController = new LogController();
            _keyboardModule = new KeyboardModule(true, true, false, true, true, _logController);

            _logDateTime = true;

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

            _logController.AddLog(new ApplicationLog(_logDateTime, "DeviceLog is currently initializing"));
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
            ToggleButton toggleButton = sender as ToggleButton;
            if (toggleButton == null) return;

            toggleButton.Content = toggleButton.IsChecked == true ? "ON" : "OFF";

            switch (toggleButton.Name)
            {
                case "TgbKeyboard":
                    if (toggleButton.IsChecked == true)
                    {
                        _logController.AddLog(new ApplicationLog(_logDateTime, "The keyboard module has been started"));
                        _keyboardModule.Start();
                    }
                    else
                    {
                        _logController.AddLog(new ApplicationLog(_logDateTime, "The keyboard module has been stopped"));
                        _keyboardModule.Stop();
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
            _logController.AddLog(new ApplicationLog(_logDateTime, "DeviceLog is shutting down"));
            Application.Current.Shutdown();
        }

        private void SettingsItem_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
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
            _logController.AddLog(new ApplicationLog(_logDateTime, "DeviceLog is checking for updates"));
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
            throw new NotImplementedException();
        }

        private void OpenItem_Click(object sender, RoutedEventArgs e)
        {
            if (IsVisible)
            {
                _logController.AddLog(new ApplicationLog(_logDateTime, "DeviceLog has been hidden"));
                Hide();
            }
            else
            {
                _logController.AddLog(new ApplicationLog(_logDateTime, "DeviceLog has been shown"));
                Show();
            }
        }
    }
}
