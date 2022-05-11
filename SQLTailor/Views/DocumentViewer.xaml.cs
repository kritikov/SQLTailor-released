using SQLTailor.Classes;
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
    public partial class DocumentViewer : Window, INotifyPropertyChanged
    {

        #region VARIABLES AND NESTED CLASSES

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

        private ResultsViewer resultsViewer;

        #endregion


        #region CONSTRUCTORS

        public DocumentViewer(FlowDocument document)
        {
            InitializeComponent();

            this.DataContext = this;

            this.document = document;
        }

        public DocumentViewer(FlowDocument document, ResultsViewer resultsViewer) {
            InitializeComponent();

            this.DataContext = this;

            this.resultsViewer = resultsViewer;
            this.document = document;
        }

        #endregion


        #region EVENTS

        private void Window_Closed(object sender, EventArgs e) {
            if (this.resultsViewer != null) {
                this.resultsViewer.RemoveDocumentViewerSubscriber(this);
            }
        }

        #endregion


        #region METHODS

        /// <summary>
        /// update the document from theoriginal source
        /// </summary>
        public void UpdateContent(FlowDocument document) {
            this.Document = document;
        }

        

        #endregion

    }
}
