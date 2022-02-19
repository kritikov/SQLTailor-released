using Microsoft.SqlServer.TransactSql.ScriptDom;
using SQLParser.Classes;
using SQLParser.Translators;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace SQLParser {
    public class Parser : INotifyPropertyChanged {

        #region VARIABLES AND NESTED CLASSES

        public event PropertyChangedEventHandler PropertyChanged;

        private List<ParseError> errors = new List<ParseError>();
        public List<ParseError> Errors {
            get { return errors; }
        }

        private TSqlFragment sqlTree;

        public IList<TSqlParserToken> Tokens {
            get { return sqlTree.ScriptTokenStream; }
        }

        private string sqlSource = "";
        public string SQLSource {
            get => sqlSource;
            set => sqlSource = value;
        }

        private TranslateOptions options = new TranslateOptions();
        public TranslateOptions Options {
            get => options;
            set => options = value;
        }

        public bool AlignClauseBodies {
            get => Options.AlignClauseBodies;
            set {
                Options.AlignClauseBodies = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AlignClauseBodies"));
            }
        }
        public bool NewLineBeforeOffsetClause {
            get => Options.NewLineBeforeOffsetClause;
            set {
                Options.NewLineBeforeOffsetClause = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("NewLineBeforeOffsetClause"));
            }
        }
        public bool MultilineInsertSourcesList {
            get => Options.MultilineInsertSourcesList;
            set {
                Options.MultilineInsertSourcesList = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MultilineInsertSourcesList"));
            }
        }
        public bool MultilineInsertTargetsList {
            get => Options.MultilineInsertTargetsList;
            set {
                Options.MultilineInsertTargetsList = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MultilineInsertTargetsList"));
            }
        }
        public bool MultilineSetClauseItems {
            get => Options.MultilineSetClauseItems;
            set {
                Options.MultilineSetClauseItems = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MultilineSetClauseItems"));
            }
        }
        public bool AlignSetClauseItem {
            get => Options.AlignSetClauseItem;
            set {
                Options.AlignSetClauseItem = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AlignSetClauseItem"));
            }
        }
        public bool IndentSetClause {
            get => Options.IndentSetClause;
            set {
                Options.IndentSetClause = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IndentSetClause"));
            }
        }
        public bool AsKeywordOnOwnLine {
            get => Options.AsKeywordOnOwnLine;
            set {
                Options.AsKeywordOnOwnLine = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AsKeywordOnOwnLine"));
            }
        }
        public bool MultilineViewColumnsList {
            get => Options.MultilineViewColumnsList;
            set {
                Options.MultilineViewColumnsList = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MultilineViewColumnsList"));
            }
        }
        public bool IndentViewBody {
            get => Options.IndentViewBody;
            set {
                Options.IndentViewBody = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IndentViewBody"));
            }
        }
        public bool MultilineWherePredicatesList {
            get => Options.MultilineWherePredicatesList;
            set {
                Options.MultilineWherePredicatesList = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MultilineWherePredicatesList"));
            }
        }
        public bool MultilineSelectElementsList {
            get => Options.MultilineSelectElementsList;
            set {
                Options.MultilineSelectElementsList = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MultilineSelectElementsList"));
            }
        }
        public bool NewLineBeforeOutputClause {
            get => Options.NewLineBeforeOutputClause;
            set {
                Options.NewLineBeforeOutputClause = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("NewLineBeforeOutputClause"));
            }
        }
        public bool NewLineBeforeCloseParenthesisInMultilineList {
            get => Options.NewLineBeforeCloseParenthesisInMultilineList;
            set {
                Options.NewLineBeforeCloseParenthesisInMultilineList = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("NewLineBeforeCloseParenthesisInMultilineList"));
            }
        }
        public bool NewLineBeforeJoinClause {
            get => Options.NewLineBeforeJoinClause;
            set {
                Options.NewLineBeforeJoinClause = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("NewLineBeforeJoinClause"));
            }
        }
        public bool NewLineBeforeHavingClause {
            get => Options.NewLineBeforeHavingClause;
            set {
                Options.NewLineBeforeHavingClause = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("NewLineBeforeHavingClause"));
            }
        }
        public bool NewLineBeforeOrderByClause {
            get => Options.NewLineBeforeOrderByClause;
            set {
                Options.NewLineBeforeOrderByClause = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("NewLineBeforeOrderByClause"));
            }
        }
        public bool NewLineBeforeGroupByClause {
            get => Options.NewLineBeforeGroupByClause;
            set {
                Options.NewLineBeforeGroupByClause = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("NewLineBeforeGroupByClause"));
            }
        }
        public bool NewLineBeforeWhereClause {
            get => Options.NewLineBeforeWhereClause;
            set {
                Options.NewLineBeforeWhereClause = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("NewLineBeforeWhereClause"));
            }
        }
        public bool NewLineBeforeFromClause {
            get => Options.NewLineBeforeFromClause;
            set {
                Options.NewLineBeforeFromClause = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("NewLineBeforeFromClause"));
            }
        }
        public bool AlignColumnDefinitionFields {
            get => Options.AlignColumnDefinitionFields;
            set {
                Options.AlignColumnDefinitionFields = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AlignColumnDefinitionFields"));
            }
        }
        public bool IncludeSemicolons {
            get => Options.IncludeSemicolons;
            set {
                Options.IncludeSemicolons = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IncludeSemicolons"));
            }
        }
        public bool NewLineBeforeOpenParenthesisInMultilineList {
            get => Options.NewLineBeforeOpenParenthesisInMultilineList;
            set {
                Options.NewLineBeforeOpenParenthesisInMultilineList = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("NewLineBeforeOpenParenthesisInMultilineList"));
            }
        }
        public int IndentationSize {
            get => Options.IndentationSize;
            set {
                Options.IndentationSize = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IndentationSize"));
            }
        }
        public int SqlVersion {
            get => (int)Options.SqlVersion;
            set {
                Options.SqlVersion = (SqlVersion)value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SqlVersion"));
            }
        }

        public Dictionary<string, string> QueryParameters {
            get {
                return Options.QueryParameters;
            }
            set {
                Options.QueryParameters = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("QueryParameters"));
            }
        }

        public bool UpdateQueryParametersList {
            get => Options.UpdateQueryParametersList;
            set {
                Options.UpdateQueryParametersList = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("UpdateQueryParametersList"));
            }
        }

        public bool ReplaceQueryParametersWithValues {
            get => Options.ReplaceQueryParametersWithValues;
            set {
                Options.ReplaceQueryParametersWithValues = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ReplaceQueryParametersWithValues"));
            }
        }

        //public SqlEngineType SqlEngineType { get; set; }

        public int KeywordCasing {
            get {
                if (Options.KeywordCasing == Microsoft.SqlServer.TransactSql.ScriptDom.KeywordCasing.Lowercase) {
                    return 0;
                }
                else if (Options.KeywordCasing == Microsoft.SqlServer.TransactSql.ScriptDom.KeywordCasing.Uppercase) {
                    return 1;
                }
                else {
                    return 2;
                }
            }
            set {
                if (value == 0) {
                    Options.KeywordCasing = Microsoft.SqlServer.TransactSql.ScriptDom.KeywordCasing.Lowercase;
                }
                else if (value == 1) {
                    Options.KeywordCasing = Microsoft.SqlServer.TransactSql.ScriptDom.KeywordCasing.Uppercase;
                }
                else {
                    Options.KeywordCasing = Microsoft.SqlServer.TransactSql.ScriptDom.KeywordCasing.PascalCase;
                }
            }
        }

        #endregion


        #region CONSTRUCTORS

        public Parser() {

        }

        public Parser(string sqlSource) : base() {
            this.SQLSource = sqlSource;
        }

        #endregion


        #region METHODS

        private static (TSqlFragment sqlTree, IList<ParseError> errors) ProcessSql(string procText) {
            TSql150Parser parser = new TSql150Parser(false); // false tells to parser whether you have quoted identifiers on or not


            using (StringReader textReader = new StringReader(procText)) {
                TSqlFragment sqlTree = parser.Parse(textReader, out IList<ParseError> errors);

                return (sqlTree, errors);
            }
        }

        /// <summary>
        /// read the sql script and parse it all the available forms
        /// </summary>
        public void Process() {
            (TSqlFragment sqlTree, IList<ParseError> errors) processed = ProcessSql(this.sqlSource);

            sqlTree = processed.sqlTree;
            errors = (List<ParseError>)processed.errors;
        }

        /// <summary>
        /// get the tokens of the script in a formatted list for display as grid
        /// </summary>
        /// <returns></returns>
        public List<string[]> GetTokensList() {
            List<string[]> list = new List<string[]>();

            foreach (TSqlParserToken token in sqlTree.ScriptTokenStream) {
                string[] item = { token.Line.ToString(), token.Column.ToString(), token.Offset.ToString(), token.Text, token.TokenType.ToString() };
                list.Add(item);
            }

            return list;
        }

        /// <summary>
        /// get the formatted SQL using the native method of the Microsoft libary
        /// </summary>
        /// <returns></returns>
        public string GetTSQL() {
            Sql150ScriptGenerator scriptGenerator = new Sql150ScriptGenerator(options);
            scriptGenerator.GenerateScript(sqlTree, out string result);

            //options.SqlEngineType = SqlEngineType.Standalone;
            //options.SqlVersion = SqlVersion.Sql100;

            return result;
        }

        public FlowDocument GetTSQLAsFlowDocument() {

            string text = GetTSQL();

            // use the formation of the Translator
            Translator translator = new Translator(this.Options);
            return translator.GetFlowDocument(text);
        }

        /// <summary>
        /// get the translated sql tree to a T-SQL form
        /// </summary>
        /// <returns></returns>
        public string GetSQLStructure() {
            string result = "";

            Visitor visitor = new Visitor();
            sqlTree.Accept(visitor);

            if (Options.UpdateQueryParametersList) {
                Options.QueryParameters.Clear();
            }

            Translator translator = new Translator(Options);

            foreach (TSqlFragment statement in visitor.Statements) {
                if (statement is SelectStatement selectStatement) {
                    result += translator.SelectStatementParse(selectStatement);
                }
                else if (statement is InsertStatement insertStatement) {
                    result += translator.InsertSpecificationParse(insertStatement.InsertSpecification);
                }
                else if (statement is UpdateStatement updateStatement) {
                    result += translator.UpdateSpecificationParse(updateStatement.UpdateSpecification);
                }
                else if (statement is DeleteStatement deleteStatement) {
                    result += translator.DeleteSpecificationParse(deleteStatement.DeleteSpecification);
                }
                else if (statement is CreateTableStatement createTableStatement) {
                    result += translator.CreateTableStatementParse(createTableStatement);
                }
                else if (statement is CreateViewStatement createViewStatement) {
                    result += translator.CreateViewStatementParse(createViewStatement);
                }
                else if (statement is AlterTableAddTableElementStatement alterTableAddTableElementStatement) {
                    result += translator.AlterTableAddTableElementStatementParse(alterTableAddTableElementStatement);
                }
                else if (statement is AlterTableAlterColumnStatement alterTableAlterColumnStatement) {
                    result += translator.AlterTableAlterColumnStatementParse(alterTableAlterColumnStatement);
                }
            }

            return result;
        }


        public FlowDocument GetSQLStructureAsFlowDocument() {
            string result = GetSQLStructure();
            Translator translator = new Translator(Options);
            return translator.GetFlowDocument(result);
        }

        public string GetSqlOM() {
            string result = "";

            Visitor visitor = new Visitor();
            sqlTree.Accept(visitor);

            SqlOMTranslator translator = new SqlOMTranslator(Options);

            foreach (TSqlFragment statement in visitor.Statements) {
                if (statement is SelectStatement selectStatement) {
                    result += translator.SelectStatementParse(selectStatement);
                }
                else if (statement is InsertStatement insertStatement) {
                    result += translator.InsertSpecificationParse(insertStatement.InsertSpecification);
                }
                else if (statement is UpdateStatement updateStatement) {
                    result += translator.UpdateSpecificationParse(updateStatement.UpdateSpecification);
                }
                else if (statement is DeleteStatement deleteStatement) {
                    result += translator.DeleteSpecificationParse(deleteStatement.DeleteSpecification);
                }
            }


            return result;
        }

        public FlowDocument GetSqlOMAsFlowDocument() {
            string result = GetSqlOM();
            SqlOMTranslator translator = new SqlOMTranslator(Options);
            return translator.GetFlowDocument(result);
        }

        public string GetPoVariation() {
            string result = "";

            Visitor visitor = new Visitor();
            sqlTree.Accept(visitor);

            PoVariationTranslator translator = new PoVariationTranslator(Options);

            foreach (TSqlFragment statement in visitor.Statements) {
                if (statement is SelectStatement selectStatement) {
                    result += translator.SelectStatementParse(selectStatement);
                }
                else if (statement is InsertStatement insertStatement) {
                    result += translator.InsertSpecificationParse(insertStatement.InsertSpecification);
                }
                else if (statement is UpdateStatement updateStatement) {
                    result += translator.UpdateSpecificationParse(updateStatement.UpdateSpecification);
                }
                else if (statement is DeleteStatement deleteStatement) {
                    result += translator.DeleteSpecificationParse(deleteStatement.DeleteSpecification);
                }
            }

            return result;
        }

        public FlowDocument GetPoVariationAsFlowDocument() {
            string result = GetPoVariation();
            PoVariationTranslator translator = new PoVariationTranslator(Options);
            return translator.GetFlowDocument(result);
        }

        #endregion

    }


}
