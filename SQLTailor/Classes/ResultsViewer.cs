using SQLTailor.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace SQLTailor.Classes {
    public class ResultsViewer : INotifyPropertyChanged {

        #region VARIABLES AND NESTED CLASSES

        public event PropertyChangedEventHandler PropertyChanged;

        private double documentWidth = 3000;

        private FlowDocument document = new FlowDocument();
        public FlowDocument Document {
            get => document;
            set {
                document = value;
                document.PageWidth = documentWidth;
                UpdateSubscribers();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Document"));
            }
        }

        private List<DocumentViewer> subscribers = new List<DocumentViewer>();

        #endregion


        #region CONSTRUCTORS

        public ResultsViewer() {
            
        }

        #endregion


        #region METHODS

        /// <summary>
        /// create a clone of a flow document
        /// </summary>
        /// <param name="document"></param>
        /// <returns>a FlowDocument</returns>
        public static FlowDocument CloneDocument(FlowDocument document) {
            if (document == null) {
                document = new FlowDocument();
            }

            TextRange range = new TextRange(document.ContentStart, document.ContentEnd);
            MemoryStream stream = new MemoryStream();
            System.Windows.Markup.XamlWriter.Save(range, stream);
            range.Save(stream, DataFormats.XamlPackage);

            FlowDocument clone = new FlowDocument();
            TextRange range2 = new TextRange(clone.ContentEnd, clone.ContentEnd);
            range2.Load(stream, DataFormats.XamlPackage);

            clone.PageWidth = document.PageWidth;

            return clone;
        }

        /// <summary>
        /// create a clone of the document
        /// </summary>
        /// <returns></returns>
        public FlowDocument CloneDocument() {

            return CloneDocument(this.document);
        }

        /// <summary>
        /// copy the document in the clipboard
        /// </summary>
        /// <param name="document"></param>
        public void CopyDocument() {

            if (document == null) {
                document = new FlowDocument();
            }

            TextRange range = new TextRange(document.ContentStart, document.ContentEnd);
            Clipboard.SetText(range.Text);
        }

        /// <summary>
        /// display the document as copy in a new window
        /// </summary>
        /// <param name="document"></param>
        public DocumentViewer FloatDocument(string title) {
            FlowDocument newDocument = this.CloneDocument();
            DocumentViewer window = new DocumentViewer(newDocument);
            window.Title = title;
            window.Show();

            return window;
        }

        public void FloatLinkendDocument(string title) {
            FlowDocument newDocument = this.CloneDocument();
            DocumentViewer viewer = new DocumentViewer(newDocument, this);
            AddDocumentViewerSubscriber(viewer);
            viewer.Title = title;
            viewer.Show();
        }


        /// <summary>
        /// subscribe a DocumentViewer to refresh its content when the original documents is changed
        /// </summary>
        /// <param name="viewer"></param>
        public void AddDocumentViewerSubscriber(DocumentViewer viewer) {

            if (!subscribers.Contains(viewer)) {
                subscribers.Add(viewer);
            }
        }

        /// <summary>
        /// remove a DocumentViewer from the subscribers
        /// </summary>
        /// <param name="viewer"></param>
        public void RemoveDocumentViewerSubscriber(DocumentViewer viewer) {
            subscribers.Remove(viewer);
        }


        /// <summary>
        /// update the documents in the subscribers
        /// </summary>
        private void UpdateSubscribers() {

            if (subscribers.Count > 0) {
                FlowDocument document = this.CloneDocument();

                foreach (var viewer in subscribers) {
                    viewer.UpdateContent(document);
                }
            }
        }


        #endregion
    }
}
