using System;
using System.Collections.Generic;
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
        public List<Tuple<KeyModifier, Key>> Result { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            Result = new List<Tuple<KeyModifier, Key>>();
            SetUIElements();
            Ok.Click += Ok_Click;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            SetResult();
            this.Close();
        }

        private void SetResult()
        {
            Result.Clear();
            Result.Add(Tuple.Create(GetModifier(PrimaryModifier.SelectedValue), GetKey(PrimaryKey.SelectedValue)));
            Result.Add(Tuple.Create(GetModifier(ActiveWindowModifier.SelectedValue), GetKey(ActiveWindowKey.SelectedValue)));
        }

        private Key GetKey(object selectedValue)
        {
            return (Key)selectedValue;
        }

        private KeyModifier GetModifier(object selectedValue)
        {
            return (KeyModifier)selectedValue;
        }

        private void SetUIElements()
        {
            var keys = Enum.GetValues(typeof(Key)).Cast<Key>().Where(t => IsAllowedKey(t));
            var modifier = Enum.GetValues(typeof(KeyModifier)).Cast<KeyModifier>();
            PrimaryKey.ItemsSource = keys;
            PrimaryKey.SelectedIndex = 16;
            PrimaryModifier.ItemsSource = modifier;
            PrimaryModifier.SelectedIndex = 1;
            ActiveWindowKey.ItemsSource = keys;
            ActiveWindowKey.SelectedIndex = 16;
            ActiveWindowModifier.ItemsSource = modifier;
            ActiveWindowModifier.SelectedIndex = 0;
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
