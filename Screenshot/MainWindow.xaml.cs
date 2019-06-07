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
        private List<Tuple<KeyModifier, Key>> _userSetValues = new List<Tuple<KeyModifier, Key>>();

        public MainWindow()
        {
            InitializeComponent();
            Result = new List<Tuple<KeyModifier, Key>>();
            Ok.Click += Ok_Click;
        }

        public void SetUserSettingValues(IEnumerable<Tuple<KeyModifier, Key>> values)
        {
            _userSetValues = values.ToList();
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
            PrimaryKey.SelectedValue = _userSetValues[0].Item2;
            PrimaryModifier.ItemsSource = modifier;
            PrimaryModifier.SelectedValue = _userSetValues[0].Item1; ;
            ActiveWindowKey.ItemsSource = keys;
            ActiveWindowKey.SelectedValue = _userSetValues[1].Item2; 
            ActiveWindowModifier.ItemsSource = modifier;
            ActiveWindowModifier.SelectedValue = _userSetValues[1].Item1;
        }

        private bool IsAllowedKey(Key t)
        {
            var intVal = (int)t;
            if (intVal >= 44 && intVal <= 69) return true;
            if (intVal >= 90 && intVal <= 99) return true;
            return false;
        }

        internal void ShowWindow()
        {
            SetUIElements();
            this.Show();
        }
    }
}
