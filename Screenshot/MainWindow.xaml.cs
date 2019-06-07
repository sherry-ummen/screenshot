using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Screenshot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            SetUIElements();
        }

        private void SetUIElements()
        {
            var keys = Enum.GetValues(typeof(Key)).Cast<Key>().Where(t => IsAllowedKey(t));
            PrimaryKey.ItemsSource = keys;
            PrimaryKey.SelectedIndex = 16;
            ActiveWindowKey.ItemsSource = keys;
            ActiveWindowKey.SelectedIndex = 16;
        }

        private bool IsAllowedKey(Key t)
        {
            var intVal = (int)t;
            if (intVal >= 44 && intVal <= 69) return true;
            if (intVal >= 90 && intVal <= 99) return true;
            return false;
        }
    }
}
