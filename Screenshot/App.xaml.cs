﻿using Screenshot.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.Linq;
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
        private bool _exit;
        private ShortcutKeys _shortcutKeys;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            MainWindow = new MainWindow();
            MainWindow.Closing += MainWindow_Closing;
            _shortcutKeys = new ShortcutKeys();
            CreateSystemTrayIcon();
            var settings = GetSettings();
            SetSettings(settings);
            SetGlobalHotKey(settings, new Action[] { OnPrimaryScreenshot, OnActiveWindowScreenshot });
        }

        private void OnActiveWindowScreenshot()
        {
            ScreenCapture.ScreenshotOfActiveWindow(GetFileName, ImageFormat.Jpeg);
        }

        private void OnPrimaryScreenshot()
        {
            ScreenCapture.FullScreenshotPrimaryMonitor(GetFileName, ImageFormat.Jpeg);
        }

        private string GetFileName => $"Screendhot_{DateTime.Now.ToString("dd.MM.yy.HH.mm.ss.ffff")}.jpeg";

        private void SetGlobalHotKey(IEnumerable<Tuple<KeyModifier, Key>> settings, IEnumerable<Action> actions)
        {
            _shortcutKeys.UnRegisterAll();
            foreach (var item in settings.Zip(actions, (s, a) => new { s, a }))
            {
                _shortcutKeys.Register(item.s.Item1, item.s.Item2, item.a);
            }
        }

        private IEnumerable<Tuple<KeyModifier, Key>> GetSettings()
        {
            List<Tuple<KeyModifier, Key>> values;
            if (Settings.Default.ActiveWindow == null)
            {
                // set default values
                values = new List<Tuple<KeyModifier, Key>>(){
                    Tuple.Create(KeyModifier.Ctrl, Key.Q),
                    Tuple.Create(KeyModifier.Alt, Key.Q),
                };
            }
            else
            {
                // get set values
                values = new List<Tuple<KeyModifier, Key>>(){
                    Tuple.Create((KeyModifier)Enum.Parse(typeof(KeyModifier), Settings.Default.PrimaryScreen[0]),(Key)(Enum.Parse(typeof(Key),Settings.Default.PrimaryScreen[1]))),
                    Tuple.Create((KeyModifier)Enum.Parse(typeof(KeyModifier), Settings.Default.ActiveWindow[0]),(Key)(Enum.Parse(typeof(Key),Settings.Default.ActiveWindow[1])))
                };

            }
            return values;
        }

        private void SetSettings(IEnumerable<Tuple<KeyModifier, Key>> settings)
        {
            (MainWindow as Screenshot.MainWindow).SetUserSettingValues(settings);
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
            _exit = true;
            MainWindow.Close();
            _systemTrayIcon.Dispose();
            App.Current.Shutdown();
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
            (MainWindow as MainWindow).ShowWindow();
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            if (_exit)
            {
                _shortcutKeys.Dispose();
                return;
            }
            MainWindow.Hide();
            e.Cancel = true;
            var result = (MainWindow as MainWindow).Result;
            SetUserConfigSettings(result);
        }

        private void SetUserConfigSettings(List<Tuple<KeyModifier, Key>> result)
        {
            SetGlobalHotKey(result, new Action[] { OnPrimaryScreenshot, OnActiveWindowScreenshot });
            Settings.Default.PrimaryScreen = new System.Collections.Specialized.StringCollection();
            Settings.Default.ActiveWindow = new System.Collections.Specialized.StringCollection();
            Settings.Default.PrimaryScreen.Add(result.First().Item1.ToString());
            Settings.Default.PrimaryScreen.Add(result.First().Item2.ToString());
            Settings.Default.ActiveWindow.Add(result.Last().Item1.ToString());
            Settings.Default.ActiveWindow.Add(result.Last().Item2.ToString());
            Settings.Default.Save();
            SetSettings(result);
        }
    }
}
