using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace SQLParser.Classes {
    /// <summary>
    /// Class for text with properties for FlowDocument formations as color of font weight etc.
    /// </summary>
    internal class ReservedWord {
        internal string Text = "";
        internal SolidColorBrush ForegroundColor = new SolidColorBrush(Colors.Black);
        internal FontWeight FontWeight = FontWeights.Normal;

        internal ReservedWord() {

        }

        internal ReservedWord(string text) : base() {
            this.Text = text;
        }

        internal ReservedWord(string text, Color forecolor) : base() {
            Text = text;
            ForegroundColor = new SolidColorBrush(forecolor);
        }

        internal ReservedWord(string text, Color forecolor, FontWeight fontWeight) : base() {
            Text = text;
            ForegroundColor = new SolidColorBrush(forecolor);
            FontWeight = fontWeight;
        }
    }
}
