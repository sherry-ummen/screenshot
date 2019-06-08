using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Interop;

namespace Screenshot
{
    internal class ShortcutKeys : IDisposable
    {
        private const int WmHotKey = 0x0312;
        private Dictionary<int, Action> _shortcutKeys = new Dictionary<int, Action>();

        public void Register(KeyModifier keyModifiers, Key k, Action action)
        {
            int virtualKeyCode = KeyInterop.VirtualKeyFromKey(k);
            var id = virtualKeyCode + ((int)keyModifiers * 0x10000);
            if (_shortcutKeys.ContainsKey(id)) throw new Exception($"This shortcut key ({keyModifiers},{k}) is already registered");
            bool result = User32.RegisterHotKey(new WindowInteropHelper(App.Current.MainWindow).Handle, id, (UInt32)keyModifiers, (UInt32)virtualKeyCode);
            _shortcutKeys.Add(id, action);
            ComponentDispatcher.ThreadFilterMessage += new ThreadMessageEventHandler(ComponentDispatcherThreadFilterMessage);
        }

        public void UnRegister(KeyModifier keyModifiers, Key k)
        {
            int virtualKeyCode = KeyInterop.VirtualKeyFromKey(k);
            var id = virtualKeyCode + ((int)keyModifiers * 0x10000);
            if (_shortcutKeys.ContainsKey(id))
            {
                User32.UnregisterHotKey(IntPtr.Zero, id);
                _shortcutKeys.Remove(id);
            }
        }

        public void UnRegisterAll()
        {
            foreach (var shortcut in _shortcutKeys)
            {
                User32.UnregisterHotKey(IntPtr.Zero, shortcut.Key);
            }
            _shortcutKeys.Clear();
        }


        private void ComponentDispatcherThreadFilterMessage(ref MSG msg, ref bool handled)
        {
            if (!handled)
            {
                if (msg.message == WmHotKey)
                {
                    Action action;

                    if (_shortcutKeys.TryGetValue((int)msg.wParam, out action))
                    {
                        if (action != null)
                        {
                            action.Invoke();
                        }
                        handled = true;
                    }
                }
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    UnRegisterAll();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
