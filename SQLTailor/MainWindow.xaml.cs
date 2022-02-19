using Microsoft.SqlServer.TransactSql.ScriptDom;
using SQLTailor.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;

namespace SQLTailor {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged {

        #region VARIABLES AND NESTED CLASSES

        public event PropertyChangedEventHandler PropertyChanged;

        private readonly int documentWidth = 3000;

        private string message = "";
        public string Message {
            get => message;
            set {
                message = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Message"));
            }
        }

        private CollectionViewSource logsSource = new CollectionViewSource();
        public ICollectionView LogsView {
            get {
                return this.logsSource.View;
            }
        }

        public SQLParser.Parser Parser { get; set; } = new SQLParser.Parser();

        public class SQLError : INotifyPropertyChanged {

            public event PropertyChangedEventHandler PropertyChanged;

            private string message = "";
            public string Message {
                get => message;
                set {
                    message = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Message"));
                }
            }

            public int Line { get; set; }

            public int Column { get; set; }
        }

        public class FixedQuery {
            public string Type { get; set; } = "";
            public string Query { get; set; } = "";

            public FixedQuery() {

            }

            public FixedQuery(string type, string query) {
                this.Type = type;
                this.Query = query;
            }
        }

        public class QueryParameter : INotifyPropertyChanged {

            public event PropertyChangedEventHandler PropertyChanged;

            private string key = "";
            public string Key {
                get => key;
                set {
                    key = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Key"));
                }
            }

            private string parameterValue = "";
            public string Value {
                get => parameterValue;
                set {
                    parameterValue = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Value"));
                }
            }

            public QueryParameter(string key, string value) {
                this.key = key;
                this.parameterValue = value;
            }
        }

        public ObservableCollection<FixedQuery> Queries { get; set; }
        public ObservableCollection<SQLError> Errors { get; set; }
        public ObservableCollection<string[]> Tokens { get; set; }
        public ObservableCollection<QueryParameter> QueryParameters { get; set; }

        private CollectionViewSource tokensSource = new CollectionViewSource();
        public ICollectionView TokensView {
            get {
                return this.tokensSource.View;
            }
        }

        private bool tokensDisplayWhiteSpaces = false;
        public bool TokensDisplayWhiteSpaces {
            get { return tokensDisplayWhiteSpaces; }
            set {
                tokensDisplayWhiteSpaces = value;
                TokensView.Refresh();
            }
        }
        private bool tokensDisplayDots = false;
        public bool TokensDisplayDots {
            get { return tokensDisplayDots; }
            set {
                tokensDisplayDots = value;
                TokensView.Refresh();
            }
        }
        private bool tokensDisplayCommas = false;
        public bool TokensDisplayCommas {
            get { return tokensDisplayCommas; }
            set {
                tokensDisplayCommas = value;
                TokensView.Refresh();
            }
        }
        private bool tokensDisplayIdentifiers = true;
        public bool TokensDisplayIdentifiers {
            get { return tokensDisplayIdentifiers; }
            set {
                tokensDisplayIdentifiers = value;
                TokensView.Refresh();
            }
        }

        private bool useFixedQuery = true;
        public bool UseFixedQuery {
            get { return useFixedQuery; }
            set {
                useFixedQuery = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("UseFixedQuery"));
            }
        }

        private string sqlSource = $@"";
        public FlowDocument SQLSourceDocument {
            get {
                return GetFlowDocument(sqlSource);
            }
            set {
                sqlSource = new TextRange(value.ContentStart, value.ContentEnd).Text;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SQLSourceDocument"));
            }
        }

        private FlowDocument microsoftTSQLDocument;
        public FlowDocument MicrosoftTSQLDocument {
            get => microsoftTSQLDocument;
            set {
                microsoftTSQLDocument = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MicrosoftTSQLDocument"));
            }
        }


        private FlowDocument sqlStructureDocument;
        public FlowDocument SQLStructureDocument {
            get => sqlStructureDocument;
            set {
                sqlStructureDocument = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SQLStructureDocument"));
            }
        }

        private FlowDocument reebSqlOMDocument;
        public FlowDocument ReebSqlOMDocument {
            get => reebSqlOMDocument;
            set {
                reebSqlOMDocument = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ReebSqlOMDocument"));
            }
        }

        private bool poVariation = true;
        public bool PoVariation {
            get => poVariation;

            set {
                poVariation = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PoVariation"));
            }
        }

        #endregion


        #region CONSTRUCTORS

        public MainWindow() {
            InitializeComponent();

            this.DataContext = this;

            logsSource.Source = Logs.List;

            Queries = new ObservableCollection<FixedQuery>();
            Errors = new ObservableCollection<SQLError>();
            Tokens = new ObservableCollection<string[]>();
            QueryParameters = new ObservableCollection<QueryParameter>();

            tokensSource.Source = Tokens;
            TokensView.Filter = TokensFilter;

            Parser.SqlVersion = 7;
            Parser.NewLineBeforeJoinClause = false;
            Parser.IndentationSize = 7;

            FillFixedQueries();
        }

        #endregion


        #region EVENTS

        private void AnalyzeSqlClick(object sender, RoutedEventArgs e) {

            if (string.IsNullOrWhiteSpace(sqlSource)) {
                return;
            }

            // clear formatting of the text in the sql editor
            ClearEditorFormat(SQLSourceEditor);

            if (Parser.ReplaceQueryParametersWithValues) {
                FillParserQueryParametersFromList();
            }

            SQLSourceDocument = SQLSourceEditor.Document;

            Parser.SQLSource = sqlSource;
            Parser.Process();

            MicrosoftTSQLDocument = Parser.GetTSQLAsFlowDocument();
            SQLStructureDocument = Parser.GetSQLStructureAsFlowDocument();

            if (Parser.UpdateQueryParametersList) {
                QueryParameters.Clear();
                foreach (KeyValuePair<string, string> parameter in Parser.QueryParameters) {
                    QueryParameters.Add(new QueryParameter(parameter.Key, parameter.Value));
                }
            }

            if (PoVariation) {
                ReebSqlOMDocument = Parser.GetPoVariationAsFlowDocument();
            }
            else {
                ReebSqlOMDocument = Parser.GetSqlOMAsFlowDocument();
            }

            MicrosoftTSQLDocument.PageWidth = documentWidth;
            SQLStructureDocument.PageWidth = documentWidth;
            ReebSqlOMDocument.PageWidth = documentWidth;

            FillErrorsList(Parser.Errors);
            FillTokensList(Parser.GetTokensList());

            if (Parser.Errors.Count > 0) {
                Message = "there are errors parsing the query";
                Logs.Write(Message);
            }
            else {
                Message = "the query parsed successfully";
                Logs.Write(Message);
            }
        }

        private void FixedQuerySelected(object sender, SelectionChangedEventArgs e) {
            if (UseFixedQuery) {
                if ((sender as ListView).SelectedItem is FixedQuery fixedQuery) {
                    SQLSourceDocument = GetFlowDocument(fixedQuery.Query);
                }
            }
        }

        private void ErrorSelected(object sender, SelectionChangedEventArgs e) {
            if ((sender as ListView).SelectedItem is SQLError error) {
                Message = $"line: {error.Line}, Column: {error.Column}";
                HighlightError(error, SQLSourceEditor.Document);
            }
        }

        #endregion


        #region METHODS

        /// <summary>
        /// fill the list with the fixed queries
        /// </summary>
        private void FillFixedQueries() {

            string query;

            query = $@"SELECT AAA.a
FROM AAA, BBB
    INNER JOIN (SELECT BBB.b
FROM BBB) AS CCC ON a = n";
            Queries.Add(new FixedQuery("select with join 1", query));

            query = $@"SELECT a    
FROM (select a as col4 from table3) b
    INNER JOIN TABLE4 ON A=B";
            Queries.Add(new FixedQuery("select with join 2", query));

            query = $@"SELECT col1 
 FROM table1 INNER JOIN table2 ON a=2 and f=5";
            Queries.Add(new FixedQuery("select with join 3", query));

            query = $@"SELECT CASE
        WHEN table1.Sep in (1, 2, @ddddd) THEN '-'
        WHEN table1.Sep = 1 and table2.f = 4 THEN 2
        WHEN table1.Sep in (1, 2, 3) THEN '\'
        ELSE NULL
    END AS aaa
FROM table1
WHERE t1.a = 3";
            Queries.Add(new FixedQuery("select with case", query));

            query = $@"SELECT a
FROM table1
WHERE ( a in (select b from t3) and (g<f) ) or c not between 1 and 5";
            Queries.Add(new FixedQuery("select with comlex where", query));

            query = $@"SELECT table1.a
FROM table1
WHERE table1.b = @1111111 AND table1.c = @22222222";
            Queries.Add(new FixedQuery("select with parameters", query));

            query = $@"SELECT col1 
FROM table1
WHERE EXISTS(
    SELECT table2.Id
    FROM UserRights AS table2)";
            Queries.Add(new FixedQuery("select with exists", query));


            query = $@"SELECT DISTINCT TOP 5 table1.col1 as a, col2 as b, col3, col4, col5, (SELECT DISTINCT col3, col3 FROM table4) 
FROM table1 as a, table2, (select a as col4 from table3) b
    INNER JOIN TABLE4 ON A=B
WHERE table1.ID = table2.ID AND table3 = 3 AND table4 = 4
GROUP BY table1.a
HAVING table1.b > 10
ORDER BY table1.a DESC";
            Queries.Add(new FixedQuery("select with full choices", query));

            query = $@"SELECT Accounts.ID, 
        Accounts.Code, 
        Accounts.Description, 
        Accounts.ParentID, 
                (CASE WHEN (Accounts.DettachedID IS NOT NULL
                    AND acc.ID IS NOT NULL) THEN CAST (1 AS SMALLINT) ELSE CAST (0 AS SMALLINT) END) AS Detached, 
        Accounts.CompanyID, 
        Accounts.Global, 
        Accounts.AccountID, 
        Accounts.Transactional, 
        Accounts.BookEntry, 
        Accounts.Grade AS AccountGrade
FROM Accounts 
        LEFT OUTER JOIN Accounts AS Acc ON (Acc.Code = Accounts.Code 
        AND Acc.AccountSystemID = Accounts.AccountSystemID 
        AND Acc.Global = 0 
        AND Acc.CompanyID = 'd701e34c-a37f-11e3-be7a-f4b7e2d0efba')
WHERE (((Accounts.Global = 1 
        AND 0 = 1) 
        OR (Acc.ID IS NULL 
        AND Accounts.Global = 1 
        AND 0 = 0) 
        OR (Acc.ID IS NOT NULL 
        AND Accounts.CompanyID = '532' 
        AND 0 = 0)))";
            Queries.Add(new FixedQuery("select rich", query));

            query = $@"(SELECT a, 'sddfg' as b
 FROM HEARTICLES AS ARTS)";
            Queries.Add(new FixedQuery("select in parenthesis", query));

            query = $@"SELECT a
FROM table1
UNION
(SELECT b
    FROM table2)
UNION
(SELECT c
    FROM table3
    UNION
    SELECT d
    FROM table4);";
            Queries.Add(new FixedQuery("select with unions", query));

            query = $@"SELECT a.Document, a.Date, a.Type, a.DocumentNumber, a.ID AS aID,
    b.InvoiceType, c.Kind
FROM table1 AS a
    INNER JOIN table3 AS c ON (a.ArticleID = c.ID)
    INNER JOIN table2 AS b ON(a.ID = b.AppentityID)
    INNER JOIN table4 ON(a.CompanyID = table4.ID)
    INNER JOIN (SELECT d.*
        FROM table4 AS d
        WHERE (d.CompanyID = 1000
            AND (NOT EXISTS (SELECT e.ID
                FROM CompanyAccessRights AS e
                WHERE (e.CompanyID = d.CompanyID
                    AND e.UserID = 5))
                    OR EXISTS(SELECT g.ID
                        FROM CompanyAccessRights AS g
                        WHERE (g.CompanyID = d.CompanyID
                            AND g.BranchID IS NULL
                            AND g.UserID = 5))
                    OR EXISTS(SELECT f.ID
                        FROM CompanyAccessRights AS f
                        WHERE (f.CompanyID = d.CompanyID
                            AND f.BranchID = d.ID
                            AND f.UserID = 5))
                    OR EXISTS(SELECT h.ID
                        FROM CompanyAccessRights AS h
                        WHERE (h.CompanyID = d.CompanyID
                            AND h.BranchID IS NULL
                            AND h.UserID = 5
                            AND EXISTS (SELECT t.ID
                                FROM CompanyAccessRights AS t
                                WHERE (t.CompanyID = d.CompanyID
                                    AND t.BranchID <> d.ID
                                    AND t.UserID = 5))))))) AS CBR
        ON(table4.ID = CBR.ID)
WHERE(a.CompanyID = 1000
    AND a.Document IS NOT NULL
    AND c.Kind = 0);";
            Queries.Add(new FixedQuery("select complex", query));

            query = $@"INSERT INTO table_name (column0, column1, columnN) 
VALUES 
    ('value10', 11, 'value12'),
    ('value20', 21, 'value22'),
    ('value20', 21, 'value22')";
            Queries.Add(new FixedQuery("insert", query));

            query = $@"UPDATE table1
SET a = table1.b
WHERE b > 5";
            Queries.Add(new FixedQuery("update", query));

            query = $@"DELETE
FROM Customers
WHERE CustomerName='Alfreds Futterkiste'";
            Queries.Add(new FixedQuery("delete", query));

            query = $@"CREATE VIEW[Brazil Customers] AS
SELECT CustomerName, ContactName
FROM Customers
WHERE Country = 'Brazil';";
            Queries.Add(new FixedQuery("create view", query));

            query = $@"CREATE TABLE table_name (
    column1 VARCHAR(50) not null unique,
    column2 VARCHAR(50),
    column3 integer,
    column4 date not null)";
            Queries.Add(new FixedQuery("create table", query));


            query = $@"ALTER TABLE Customers
ADD Email varchar(255),
    column1 VARCHAR(50) not null unique,
    column2 VARCHAR(50),
    column3 integer,
    column4 date not null";
            Queries.Add(new FixedQuery("alter table", query));
        }

        /// <summary>
        /// fill the errors list after parsing a query statemen
        /// </summary>
        /// <param name="errors"></param>
        private void FillErrorsList(IList<ParseError> errors) {
            Errors.Clear();
            foreach (var error in errors) {
                Errors.Add(new SQLError() {
                    Message = error.Message,
                    Line = error.Line,
                    Column = error.Column
                });
            }
        }

        /// <summary>
        /// fill the list with the extracted tokens from the sql source
        /// </summary>
        /// <param name="tokens"></param>
        private void FillTokensList(List<string[]> tokens) {
            Tokens.Clear();
            foreach (var token in tokens) {
                Tokens.Add(token);
            }
        }

        /// <summary>
        /// get a flow document from a string to display it in a rich textbox
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private FlowDocument GetFlowDocument(string text) {
            FlowDocument document = new FlowDocument();

            using (StringReader reader = new StringReader(text)) {
                string newLine;
                while ((newLine = reader.ReadLine()) != null) {
                    Paragraph paragraph = new Paragraph(new Run(newLine));
                    document.Blocks.Add(paragraph);
                }
            }

            document.PageWidth = documentWidth;

            return document;
        }

        /// <summary>
        /// clear the formatting of the text in the sql editor
        /// </summary>
        private void ClearEditorFormat(RichTextBox editor) {

            //string text = new TextRange(editor.Document.ContentStart, editor.Document.ContentEnd).Text;
            //editor.Document = GetFlowDocument(text);
        }

        /// <summary>
        /// Highlight an error in a flow document
        /// </summary>
        /// <param name="error"></param>
        /// <param name="editor"></param>
        private void HighlightError(SQLError error, FlowDocument document) {

            int i = 0;
            foreach (Block block in document.Blocks) {
                if (block is Paragraph) {
                    if (i == error.Line - 1) {
                        TextRange textRange = new TextRange(block.ContentStart, block.ContentEnd);
                        TextPointer start = block.ContentStart;
                        TextPointer startPos = start.GetPositionAtOffset(error.Column - 2) ?? start.GetPositionAtOffset(error.Column);
                        TextPointer endPos = start.GetPositionAtOffset(error.Column + 2) ?? start.GetPositionAtOffset(error.Column);
                        textRange.Select(startPos, endPos);
                        textRange.ApplyPropertyValue(TextElement.BackgroundProperty, new SolidColorBrush(Colors.Yellow));
                        textRange.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
                        break;
                    }
                    i++;
                }
            }
        }

        /// <summary>
        /// criteria to filter the tokens to be displayed
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private bool TokensFilter(object item) {
            string[] token = (string[])item;

            if (!TokensDisplayWhiteSpaces && token[4] == "WhiteSpace") {
                return false;
            }
            if (!TokensDisplayCommas && token[4] == "Comma") {
                return false;
            }
            if (!TokensDisplayDots && token[4] == "Dot") {
                return false;
            }
            if (!TokensDisplayIdentifiers && token[4] == "Identifier") {
                return false;
            }

            return true;
        }

        /// <summary>
        /// update the parser query parameters list with the parameters of the GUI
        /// </summary>
        private void FillParserQueryParametersFromList() {
            Parser.QueryParameters.Clear();

            foreach (QueryParameter parameter in QueryParameters) {
                Parser.QueryParameters.Add(parameter.Key, parameter.Value);
            }
        }

        #endregion


    }
}
