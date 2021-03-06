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
using System.Windows.Input;
using System.Windows.Media;
using SQLTailor.Views;
using System.Diagnostics;
using System.Text;

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

        public bool AlignClauseBodies {
            get => Parser.Options.AlignClauseBodies;
            set {
                Parser.Options.AlignClauseBodies = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AlignClauseBodies"));
            }
        }
        public bool NewLineBeforeOffsetClause {
            get => Parser.Options.NewLineBeforeOffsetClause;
            set {
                Parser.Options.NewLineBeforeOffsetClause = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("NewLineBeforeOffsetClause"));
            }
        }
        public bool MultilineInsertSourcesList {
            get => Parser.Options.MultilineInsertSourcesList;
            set {
                Parser.Options.MultilineInsertSourcesList = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MultilineInsertSourcesList"));
            }
        }
        public bool MultilineInsertTargetsList {
            get => Parser.Options.MultilineInsertTargetsList;
            set {
                Parser.Options.MultilineInsertTargetsList = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MultilineInsertTargetsList"));
            }
        }
        public bool MultilineSetClauseItems {
            get => Parser.Options.MultilineSetClauseItems;
            set {
                Parser.Options.MultilineSetClauseItems = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MultilineSetClauseItems"));
            }
        }
        public bool AlignSetClauseItem {
            get => Parser.Options.AlignSetClauseItem;
            set {
                Parser.Options.AlignSetClauseItem = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AlignSetClauseItem"));
            }
        }
        public bool IndentSetClause {
            get => Parser.Options.IndentSetClause;
            set {
                Parser.Options.IndentSetClause = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IndentSetClause"));
            }
        }
        public bool AsKeywordOnOwnLine {
            get => Parser.Options.AsKeywordOnOwnLine;
            set {
                Parser.Options.AsKeywordOnOwnLine = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AsKeywordOnOwnLine"));
            }
        }
        public bool MultilineViewColumnsList {
            get => Parser.Options.MultilineViewColumnsList;
            set {
                Parser.Options.MultilineViewColumnsList = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MultilineViewColumnsList"));
            }
        }
        public bool IndentViewBody {
            get => Parser.Options.IndentViewBody;
            set {
                Parser.Options.IndentViewBody = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IndentViewBody"));
            }
        }
        public bool MultilineWherePredicatesList {
            get => Parser.Options.MultilineWherePredicatesList;
            set {
                Parser.Options.MultilineWherePredicatesList = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MultilineWherePredicatesList"));
            }
        }
        public bool MultilineSelectElementsList {
            get => Parser.Options.MultilineSelectElementsList;
            set {
                Parser.Options.MultilineSelectElementsList = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MultilineSelectElementsList"));
            }
        }
        public bool NewLineBeforeOutputClause {
            get => Parser.Options.NewLineBeforeOutputClause;
            set {
                Parser.Options.NewLineBeforeOutputClause = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("NewLineBeforeOutputClause"));
            }
        }
        public bool NewLineBeforeCloseParenthesisInMultilineList {
            get => Parser.Options.NewLineBeforeCloseParenthesisInMultilineList;
            set {
                Parser.Options.NewLineBeforeCloseParenthesisInMultilineList = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("NewLineBeforeCloseParenthesisInMultilineList"));
            }
        }
        public bool NewLineBeforeJoinClause {
            get => Parser.Options.NewLineBeforeJoinClause;
            set {
                Parser.Options.NewLineBeforeJoinClause = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("NewLineBeforeJoinClause"));
            }
        }
        public bool NewLineBeforeHavingClause {
            get => Parser.Options.NewLineBeforeHavingClause;
            set {
                Parser.Options.NewLineBeforeHavingClause = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("NewLineBeforeHavingClause"));
            }
        }
        public bool NewLineBeforeOrderByClause {
            get => Parser.Options.NewLineBeforeOrderByClause;
            set {
                Parser.Options.NewLineBeforeOrderByClause = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("NewLineBeforeOrderByClause"));
            }
        }
        public bool NewLineBeforeGroupByClause {
            get => Parser.Options.NewLineBeforeGroupByClause;
            set {
                Parser.Options.NewLineBeforeGroupByClause = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("NewLineBeforeGroupByClause"));
            }
        }
        public bool NewLineBeforeWhereClause {
            get => Parser.Options.NewLineBeforeWhereClause;
            set {
                Parser.Options.NewLineBeforeWhereClause = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("NewLineBeforeWhereClause"));
            }
        }
        public bool NewLineBeforeFromClause {
            get => Parser.Options.NewLineBeforeFromClause;
            set {
                Parser.Options.NewLineBeforeFromClause = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("NewLineBeforeFromClause"));
            }
        }
        public bool AlignColumnDefinitionFields {
            get => Parser.Options.AlignColumnDefinitionFields;
            set {
                Parser.Options.AlignColumnDefinitionFields = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AlignColumnDefinitionFields"));
            }
        }
        public bool IncludeSemicolons {
            get => Parser.Options.IncludeSemicolons;
            set {
                Parser.Options.IncludeSemicolons = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IncludeSemicolons"));
            }
        }
        public bool NewLineBeforeOpenParenthesisInMultilineList {
            get => Parser.Options.NewLineBeforeOpenParenthesisInMultilineList;
            set {
                Parser.Options.NewLineBeforeOpenParenthesisInMultilineList = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("NewLineBeforeOpenParenthesisInMultilineList"));
            }
        }
        public int IndentationSize {
            get => Parser.Options.IndentationSize;
            set {
                Parser.Options.IndentationSize = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IndentationSize"));
            }
        }
        public int SqlVersion {
            get => Parser.Options.SqlVersion;
            set {
                Parser.Options.SqlVersion = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SqlVersion"));
            }
        }
        public bool UpdateQueryParametersList {
            get => Parser.Options.UpdateQueryParametersList;
            set {
                Parser.Options.UpdateQueryParametersList = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("UpdateQueryParametersList"));
            }
        }
        public bool ReplaceQueryParametersWithValues {
            get => Parser.Options.ReplaceQueryParametersWithValues;
            set {
                Parser.Options.ReplaceQueryParametersWithValues = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ReplaceQueryParametersWithValues"));
            }
        }
        public int KeywordCasing {
            get => Parser.Options.KeywordCasing;
            set {
                Parser.Options.KeywordCasing = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("KeywordCasing"));
            }
        }
        public bool SelectContentsInBrackets {
            get => Parser.Options.SelectContentsInBrackets;
            set {
                Parser.Options.SelectContentsInBrackets = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectContentsInBrackets"));
            }
        }
        public bool UseIndentation {
            get => Parser.Options.UseIndentation;
            set {
                Parser.Options.UseIndentation = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("UseIndentation"));
            }
        }

        //public SqlEngineType SqlEngineType { get; set; }

        private DatabaseType databaseVersion = DatabaseType.unspecified;
        public DatabaseType DatabaseVersion {
            get => databaseVersion;
            set {
                databaseVersion = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DatabaseVersion"));
            }
        }

        private FluentSQLType fluentSQLVersion = FluentSQLType.SqlOM;
        public FluentSQLType FluentSQLVersion {
            get => fluentSQLVersion;
            set {
                fluentSQLVersion = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FluentSQLVersion"));
            }
        }

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

        public ObservableCollection<FixedQuery> Queries { get; set; } = new ObservableCollection<FixedQuery>();
        public ObservableCollection<SQLError> Errors { get; set; } = new ObservableCollection<SQLError>();
        public ObservableCollection<string[]> Tokens { get; set; } = new ObservableCollection<string[]>();
        public ObservableCollection<QueryParameter> QueryParameters { get; set; } = new ObservableCollection<QueryParameter>();

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

        public ResultsViewer MicrosoftTSQLViewer { get; set; } = new ResultsViewer();
        public ResultsViewer SqlScriptViewer { get; set; } = new ResultsViewer();
        public ResultsViewer FluentScriptViewer { get; set; } = new ResultsViewer();

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
            tokensSource.Source = Tokens;
            TokensView.Filter = TokensFilter;

            AlignClauseBodies = false;
            AlignColumnDefinitionFields = false;
            AlignSetClauseItem = false;
            AsKeywordOnOwnLine = false;
            IncludeSemicolons = false;
            IndentSetClause = false;
            IndentViewBody = false;
            MultilineInsertSourcesList = false;
            MultilineInsertTargetsList = false;
            MultilineSelectElementsList = true;
            MultilineSetClauseItems = false;
            MultilineViewColumnsList = false;
            MultilineWherePredicatesList = true;
            NewLineBeforeCloseParenthesisInMultilineList = false;
            NewLineBeforeFromClause = true;
            NewLineBeforeGroupByClause = true;
            NewLineBeforeHavingClause = true;
            NewLineBeforeJoinClause = true;
            NewLineBeforeOffsetClause = false;
            NewLineBeforeOpenParenthesisInMultilineList = false;
            NewLineBeforeOrderByClause = true;
            NewLineBeforeOutputClause = false;
            NewLineBeforeWhereClause = true;
            IndentationSize = 3;
            UpdateQueryParametersList = true;
            ReplaceQueryParametersWithValues = false;
            KeywordCasing = 1;
            SqlVersion = 7;
            SelectContentsInBrackets = false;
            UseIndentation = true;

            FillFixedQueries();
        }

        #endregion


        #region EVENTS

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            SQLSourceDocument = GetFlowDocument("");
            
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            // close all open windows when the applications is closing
            Application.Current.Shutdown();
        }

        private void AnalyzeSql(object sender, RoutedEventArgs e) {

            //if (string.IsNullOrWhiteSpace(sqlSource)) {
            //    return;
            //}

            try {
                Mouse.OverrideCursor = Cursors.Wait;

                // clear formatting of the text in the sql editor
                ClearEditorFormat(SQLSourceEditor);

                if (Parser.Options.ReplaceQueryParametersWithValues) {
                    FillParserQueryParametersFromList();
                }

                SQLSourceDocument = SQLSourceEditor.Document;

                Parser.SQLSource = sqlSource;
                Parser.Process();

                MicrosoftTSQLViewer.Document = Parser.GetTSQLAsFlowDocument();

                if (DatabaseVersion == DatabaseType.MSSQL) {
                    SqlScriptViewer.Document = Parser.GetMSSQLTranslationAsFlowDocument();
                } else if (DatabaseVersion == DatabaseType.MySQL) {
                    SqlScriptViewer.Document = Parser.GetMySQLTranslationAsFlowDocument();
                } else {
                    SqlScriptViewer.Document = Parser.GetBaseTranslationAsFlowDocument();
                }
                

                if (Parser.Options.UpdateQueryParametersList) {
                    QueryParameters.Clear();
                    foreach (KeyValuePair<string, string> parameter in Parser.Options.QueryParameters) {
                        QueryParameters.Add(new QueryParameter(parameter.Key, parameter.Value));
                    }
                }

                if (PoVariation) {
                    FluentScriptViewer.Document = Parser.GetPoVariationAsFlowDocument();
                } else {
                    FluentScriptViewer.Document = Parser.GetSqlOMAsFlowDocument();
                }

                FillErrorsList(Parser.Errors);
                FillTokensList(Parser.GetTokensList());

                if (Parser.Errors.Count > 0) {
                    Message = "there are errors parsing the query";
                    Logs.Write(Message);
                } else {
                    Message = "the query parsed successfully";
                    Logs.Write(Message);
                }
            } 
            catch {

            } 
            finally {
                Mouse.OverrideCursor = null;
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

        private void TSqlInformations_PreviewMouseUp(object sender, MouseButtonEventArgs e) {

            TSQLTranslatorInformationsWindow window = new TSQLTranslatorInformationsWindow();

            if (window.ShowDialog() == true) { }
        }

        private void SQLScriptTranslator_PreviewMouseUp(object sender, MouseButtonEventArgs e) {

            SQLScriptTranslatorInformationWindow window = new SQLScriptTranslatorInformationWindow();

            if (window.ShowDialog() == true) { }
        }

        private void SqlOMTranslator_PreviewMouseUp(object sender, MouseButtonEventArgs e) {

            SqlOMTranslatorInformationWindow window = new SqlOMTranslatorInformationWindow();

            if (window.ShowDialog() == true) { }
        }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e) {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private void ClearEditor(object sender, RoutedEventArgs e) {
            SQLSourceDocument = GetFlowDocument("");
        }

        private void CopyFromEditor(object sender, RoutedEventArgs e) {
            FlowDocument document = SQLSourceEditor.Document;
            string source = new TextRange(document.ContentStart, document.ContentEnd).Text;
            Clipboard.SetText(source);
        }

        private void CopyMicrosoftTSQL(object sender, RoutedEventArgs e) {
            MicrosoftTSQLViewer.CopyDocument();
        }

        private void CopySQLScriptTranslation(object sender, RoutedEventArgs e) {
            SqlScriptViewer.CopyDocument();
        }

        private void CopySqlOMTranslation(object sender, RoutedEventArgs e) {
            FluentScriptViewer.CopyDocument();
        }

        private void MicrosoftTSQLDocumentFloat_Click(object sender, RoutedEventArgs e)
        {
            MicrosoftTSQLViewer.FloatDocument("Microsoft TSQL document");
        }

        private void SQLScriptDocumentFloat_Click(object sender, RoutedEventArgs e)
        {
            SqlScriptViewer.FloatDocument("SQL script");
        }

        private void SqlOMTranslationDocumentFloat_Click(object sender, RoutedEventArgs e)
        {
            FluentScriptViewer.FloatDocument("fluent script");
        }

        private void MicrosoftTSQLDocumentLinkedFloat_Click(object sender, RoutedEventArgs e) {
            MicrosoftTSQLViewer.FloatLinkendDocument("linked Microsoft TSQL script");
        }

        private void SQLScriptDocumentLinkedFloat_Click(object sender, RoutedEventArgs e) {
            SqlScriptViewer.FloatLinkendDocument("linked SQL script");
        }

        private void SqlOMTranslationDocumentLinkedFloat_Click(object sender, RoutedEventArgs e) {
            FluentScriptViewer.FloatLinkendDocument("linked fluent script");
        }

        #endregion


        #region METHODS

        /// <summary>
        /// fill the list with the fixed queries
        /// </summary>
        private void FillFixedQueries() {

            string query;

            query = $@"SELECT table1.a
FROM table1, table2
    INNER JOIN (SELECT table3.b
FROM table3) AS table3 ON table3.b = table2.c
WHERE  table1.d = table2.f;";
            Queries.Add(new FixedQuery("select with join 1", query));

            query = $@"SELECT a    
FROM (select a as col1 from table1) b
    INNER JOIN TABLE2 ON c = f";
            Queries.Add(new FixedQuery("select with join 2", query));

            query = $@"SELECT a 
 FROM table1 INNER JOIN table2 ON a=2 and f='name'";
            Queries.Add(new FixedQuery("select with join 3", query));

            query = $@"SELECT CASE
        WHEN table1.a in (1, 2, @ddddd) THEN '-'
        WHEN table1.a = 1 and table1.b = 4 THEN 2
        WHEN table1.a in (1, 2, 3) THEN '\'
        ELSE NULL
    END AS col1
FROM table1
WHERE table1.a = 3";
            Queries.Add(new FixedQuery("select with case", query));

            query = $@"SELECT LEN(a)
FROM table1
WHERE ( a in (select b from table2) and (g < f) ) or c not between 1 and 5";
            Queries.Add(new FixedQuery("select with comlex where", query));

            query = $@"SELECT table1.a
FROM table1
WHERE table1.b = @1111111 AND table1.c = @22222222";
            Queries.Add(new FixedQuery("select with parameters", query));

            query = $@"SELECT a 
FROM table1
WHERE EXISTS(
    SELECT alias1.b
    FROM table2 AS alias1)";
            Queries.Add(new FixedQuery("select with exists", query));

            query = $@"SELECT DISTINCT TOP 5 table1.a as col1, b as col2, c, d, e, (SELECT DISTINCT f, g FROM table4) 
FROM table1 as alias1, table2, (select a as col4 from table3) b
    INNER JOIN table4 ON a = b
WHERE table1.a = table2.a AND table3.b = 3 AND table4.c = 4
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
";
            Queries.Add(new FixedQuery("union 1", query));

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
            Queries.Add(new FixedQuery("union 2", query));

            query = $@"SELECT a
FROM table1
UNION
(SELECT b
    FROM (SELECT c
        FROM table3
        UNION
        SELECT d
        FROM table4)
    )
";
            Queries.Add(new FixedQuery("union 3", query));


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
            Queries.Add(new FixedQuery("select complex 1", query));

            query = $@"
SELECT isnull(alias6.a, 0) a, alias6.b, alias6.c, alias6.m, alias6.d, alias6.e, alias6.f, alias6.g, alias6.h, alias6.j, alias6.k
FROM (
       SELECT  isnull(alias3.a, 0) a, alias3.b, alias3.c, alias3.m, alias3.e, alias3.f, alias3.g, case when ((alias2.d = 0 or (alias2.d = 1 and alias3.g is not null and alias3.f is not null ))) then 0 when ((alias2.d = 6 or (alias2.d = 7
AND EXISTS (SELECT distinct 1 as col1
FROM table1 as alias1 
INNER JOIN table2 as alias2  on (alias1.l = alias2.m)
WHERE (alias2.d = 1 and alias1.n = alias4.n and alias1.g = alias4.p and alias1.n = 'key 1'))))) then 6 when (alias2.d = 1) then 1 when (alias2.d = 7) then 7 else -1 end d, alias2.d k, 10 h, case when (alias2.d = 0) then 'Category 1' when (alias2.d = 1) then 'Category 2' when (alias2.d = 2) then 'Category 3' when (alias2.d = 3) then 'Category 4' when (alias2.d = 4) then 'Category 5' when (alias2.d = 5) then 'Category 6' when (alias2.d = 6) then 'Category 7' when (alias2.d = 7) then 'Category 8' when (alias2.d = 100) then 'Category 9' else null end j
FROM table1 as alias3 
INNER JOIN table2alias2  on (alias3.l = alias2.m)
INNER JOIN table3  as alias4  on (alias3.n = alias4.n and alias3.e = alias4.m)
WHERE (alias3.n = 'key 1' and alias4.n = 'key 1' and (alias2.d = 0 or (alias2.d = 1 and alias3.g is not null )))

UNION

SELECT isnull(alias3.a, 0) a, alias3.b, alias3.c, alias3.m, alias3.e, alias3.f, alias3.g, case when ((alias2.d = 0 or (alias2.d = 1 and alias3.g is not null and alias3.f is not null ))) then 0 when ((alias2.d = 6 or (alias2.d = 7
AND EXISTS (SELECT distinct 1 as col1
FROM table1 as alias1 
INNER JOIN table2 as alias2  on (alias1.l = alias2.m)
WHERE (alias2.d = 1 and alias1.n = alias4.n and alias1.g = alias4.p and alias1.n = 'key 1'))))) then 6 when (alias2.d = 1) then 1 when (alias2.d = 7) then 7 else -1 end d, alias2.d k, 11 h, case when (alias2.d = 0) then 'Category 1' when (alias2.d = 1) then 'Category 2' when (alias2.d = 2) then 'Category 3' when (alias2.d = 3) then 'Category 4' when (alias2.d = 4) then 'Category 5' when (alias2.d = 5) then 'Category 6' when (alias2.d = 6) then 'Category 7' when (alias2.d = 7) then 'Category 8' when (alias2.d = 100) then 'Category 9' else null end j
FROM table1 as alias3 
INNER JOIN table2 as alias2  on (alias3.l = alias2.m)
INNER JOIN table3  as alias4  on (alias3.n = alias4.n and alias3.e = alias4.m)
WHERE (alias3.n = 'key 1' and alias4.n = 'key 1' and ((alias2.d = 6) or (alias2.d = 7
AND EXISTS (SELECT top 1 1 as col2
FROM table1 as alias5 
WHERE (alias5.g = alias4.p and alias5.n = alias4.n))))) ) alias6
";
            Queries.Add(new FixedQuery("select complex 2", query));

            query = $@"
SELECT alias1.b as col7, alias1.c, case when(alias1.d = 0) then 'Value 1' when(alias1.d = 1) then 'Value 2' when(alias1.d = 2) then 'Value 3' when(alias1.d = 10) then 'Value 4' when(alias1.d = 11) then 'Value 5' else '' end as col1, case when(alias1.e in ('0', '6') and alias1.f in ('0', '6', '7')) then cast(-1 as decimal(19, 7))*alias1.x else col2 end as col6, case when(alias1.e in ('0') and alias1.f in ('0')) then cast(-1 as decimal(19, 7))*cast(Alias1.x as decimal(19, 7)) * cast(alias1.G as decimal(19, 7)) / cast(100 as decimal(19, 7)) when
(alias1.e in ('1')) then cast(alias1.x as decimal(19, 7))*cast(alias1.G as decimal(19, 7)) / cast(100 as decimal(19, 7)) when(alias1.e in ('0') and alias1.f in ('1')) then cast(Alias1.x as decimal(19, 7))*cast(alias1.G as decimal(19, 7)) / cast(100 as decimal(19, 7)) else 0 end as col3, alias1.h, alias1.j, alias1.e, alias1.d, alias1.v
FROM table1 as alias2 
INNER JOIN table2  as alias3 on(alias2.t = alias3.c)
INNER JOIN (SELECT isnull(alias4.b, 0) b, alias4.k, alias4.l, alias4.c, alias4.e, alias4.q, alias4.h, alias4.j, alias4.d, alias4.v, alias4.f
FROM(
       SELECT  isnull(alias1.b, 0)b, alias1.k, alias1.l, alias1.c, alias1.q, alias1.h, alias1.j, case when((alias6.e = 0 or (alias6.e = 1 and alias1.j is not null and alias1.h is not null))) then 0 when((alias6.e = 6 or(alias6.e = 7
AND EXISTS(SELECT distinct 1 as col4
FROM table3 as alias5 
INNER JOIN table4 as alias6 on (alias5.m = alias6.c)
WHERE (alias6.e = 1 and alias5.n = alias7.n and alias5.j = alias7.p and alias5.n = 'key 1'))))) then 6 when(alias6.e = 1) then 1 when(alias6.e = 7) then 7 else -1 end e, alias6.e as f, 10d, case when(alias6.e = 0) then 'Category 1' when(alias6.e = 1) then 'Category 2' when(alias6.e = 2) then 'Category 3' when(alias6.e = 3) then 'Category 4' when(alias6.e = 4) then 'Category 5' when(alias6.e = 5) then 'Category 6' when(alias6.e = 6) then 'Category 7' when(alias6.e = 7) then 'Category 8' when(alias6.e = 100) then 'Category 9' else null end as v
FROM table3 as alias1 
INNER JOIN table4 as alias6 on(alias1.m = alias6.c)
INNER JOIN table5  as alias7 on(alias1.n = alias7.n and alias1.q = alias7.c)
WHERE (alias1.n = 'key 1' and alias7.n = 'key 1' and(alias6.e = 0 or (alias6.e = 1 and alias1.j is not null )))

UNION

SELECT isnull(alias1.b, 0) b, alias1.k, alias1.l, alias1.c, alias1.q, alias1.h, alias1.j, case when((alias6.e = 0 or(alias6.e = 1 and alias1.j is not null and alias1.h is not null))) then 0 when ((alias6.e = 6 or (alias6.e = 7
AND EXISTS (SELECT distinct 1 as col4
FROM table3 as alias5 
INNER JOIN table4 as alias6 on(alias5.m = alias6.c)
WHERE (alias6.e = 1 and alias5.n = alias7.n and alias5.j = alias7.p and alias5.n = 'key 1'))))) then 6 when(alias6.e = 1) then 1 when (alias6.e = 7) then 7 else -1 end e, alias6.e as f, 11d, case when (alias6.e = 0) then 'Category 1' when (alias6.e = 1) then 'Category 2' when (alias6.e = 2) then 'Category 3' when (alias6.e = 3) then 'Category 4' when (alias6.e = 4) then 'Category 5' when (alias6.e = 5) then 'Category 6' when (alias6.e = 6) then 'Category 7' when (alias6.e = 7) then 'Category 8' when (alias6.e = 100) then 'Category 9' else null end v
FROM table3 as alias1 
INNER JOIN table4 as alias6 on(alias1.m = alias6.c)
INNER JOIN table5 as alias7 on(alias1.n = alias7.n and alias1.q = alias7.c)
WHERE (alias1.n = 'key 1' and alias7.n = 'key 1' and ((alias6.e = 6) or(alias6.e = 7
AND EXISTS(SELECT top 1 1 as col5
FROM table3 as alias8 
WHERE (alias8.j = alias7.p and alias8.n = alias7.n))))) ) alias4 ) alias1 on(alias2.v = alias1.c)
INNER JOIN table6 as alias8 on (alias3.r = alias8.c)
WHERE (alias3.r = 'key 2' and alias3.s between isnull(alias1.k, alias3.s) and isnull(alias1.l, alias3.s) and alias1.e in ('0', '1', '6', '7') and alias2.n = 'key 1' and alias3.n = 'key 1' and alias3.s between '2022-01-02' and '2022-02-02')
";
            Queries.Add(new FixedQuery("select complex 3", query));


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

            query = $@"DROP TABLE table1, table2";
            Queries.Add(new FixedQuery("drop table", query));

            query = $@"DROP TABLE IF EXISTS table1, table2";
            Queries.Add(new FixedQuery("drop table if exists", query));

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
            Parser.Options.QueryParameters.Clear();

            foreach (QueryParameter parameter in QueryParameters) {
                Parser.Options.QueryParameters.Add(parameter.Key, parameter.Value);
            }
        }


        #endregion

       
    }
}
