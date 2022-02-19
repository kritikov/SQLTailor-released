using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SQLTailor.Classes.Functions;

namespace SQLTailor.Classes {
    public static class Logs {

        private static ObservableCollection<string> list = new ObservableCollection<string>();
        private static int MaxLenth = 1000;

        public static ObservableCollection<string> List {
            get => list;
        }

        /// <summary>
        /// Write an entry in the logs
        /// </summary>
        /// <param name="message"></param>
        public static void Write(string message) {
            DispatcherDo(() => {
                Add(message);
            });
        }

        /// <summary>
        /// Add a message in the logs
        /// </summary>
        /// <param name="message"></param>
        private static void Add(string message) {

            if (list.Count() == MaxLenth) {
                list.RemoveAt(0);
            }

            list.Add($@"{DateTime.Now:dd/MM/yyyy hh:mm:ss}:: {message}");
        }
    }
}
