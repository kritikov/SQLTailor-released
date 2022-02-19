using System;
using System.Windows;

namespace SQLTailor.Classes {
    public static class Functions {

        /// <summary>
        /// do an action in the application main thread
        /// </summary>
        /// <param name="action"></param>
        public static void DispatcherDo(Action action) {
            if (Application.Current != null) {
                Application.Current.Dispatcher.Invoke(action);
            }
        }
    }
}
