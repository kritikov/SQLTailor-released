using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SQLTailor.Views
{
    /// <summary>
    /// Interaction logic for FloatDocument.xaml
    /// </summary>
    public partial class FloatDocument : Window, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        private FlowDocument document;
        public FlowDocument Document
        {
            get => document;
            set
            {
                document = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Document"));
            }
        }

        public FloatDocument(FlowDocument document)
        {
            InitializeComponent();

            this.DataContext = this;

            this.document = document;
        }
    }
}
