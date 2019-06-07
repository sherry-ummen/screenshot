using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace Screenshot
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        private NotifyIcon _systemTrayIcon;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            MainWindow = new MainWindow();
            MainWindow.Closing += MainWindow_Closing;

            CreateSystemTrayIcon();
        }

        private void CreateSystemTrayIcon()
        {
            InitializeSystemTrayIcon();
            CreateContextMenuForSystemTrayIcon();
        }

        private void CreateContextMenuForSystemTrayIcon()
        {
            _systemTrayIcon.ContextMenuStrip =
             new ContextMenuStrip();
            _systemTrayIcon.ContextMenuStrip.Items.Add("Shortcut Keys").Click += ShowWindow;
            _systemTrayIcon.ContextMenuStrip.Items.Add("Exit").Click += ExitApp;
        }

        private void ExitApp(object sender, EventArgs e)
        {
            MainWindow.Close();
            _systemTrayIcon.Dispose();
        }

        private void InitializeSystemTrayIcon()
        {
            _systemTrayIcon = new NotifyIcon();
            _systemTrayIcon.DoubleClick += ShowWindow;
            _systemTrayIcon.Icon = Screenshot.Properties.Resources.screenshot;
            _systemTrayIcon.Visible = true;
        }

        private void ShowWindow(object sender, EventArgs e)
        {
            MainWindow.Show();
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            MainWindow.Hide();
            var result = (MainWindow as MainWindow).Result;
            SetGlobalHotKey(result);
        }

        private void SetGlobalHotKey(List<Tuple<KeyModifier, Key>> result)
        {
            throw new NotImplementedException();
        }
    }
}
