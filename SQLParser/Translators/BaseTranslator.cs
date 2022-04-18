using Microsoft.SqlServer.TransactSql.ScriptDom;
using SQLParser.Classes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace SQLParser.Translators {
    public class BaseTranslator : ITranslator {

        #region VARIABLES AND NESTED CLASSES

        public class Informations {
            public int Level { get; set; } = 0;
        }

        public Informations Data = new Informations();

        public TranslateOptions FormatOptions = new TranslateOptions();

        private List<ReservedWord> reservedWords = new List<ReservedWord>() {
            new ReservedWord("ADD", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("ALL", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("ALTER", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("AND", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("ANY", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("AS", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("ASC", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("AUTHORIZATION", Colors.DarkGreen, FontWeights.SemiBold),

            new ReservedWord("BACKUP", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("BEGIN", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("BETWEEN", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("BREAK", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("BROWSE", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("BULK", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("BY", Colors.DarkGreen, FontWeights.SemiBold),

            new ReservedWord("CASCADE", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("CASE", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("CHECK", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("CHECKPOINT", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("CLOSE", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("CLUSTERED", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("COALESCE", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("COLLATE", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("COLUMN", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("COMMIT", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("COMPUTE", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("CONSTRAINT", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("CONTAINS", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("CONTAINSTABLE", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("CONTINUE", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("CONVERT", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("CREATE", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("CROSS", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("CURRENT", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("CURRENT_DATE", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("CURRENT_TIME", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("CURRENT_TIMESTAMP", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("CURRENT_USER", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("CURSOR", Colors.DarkGreen, FontWeights.SemiBold),

            new ReservedWord("DATABASE", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("DBCC", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("DEALLOCATE", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("DECLARE", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("DEFAULT", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("DELETE", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("DENY", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("DESC", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("DISK", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("DISTINCT", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("DISTRIBUTED", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("DOUBLE", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("DROP", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("DUMP", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("date", Colors.DarkGreen, FontWeights.SemiBold),

            new ReservedWord("ELSE", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("END", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("ERRLVL", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("ESCAPE", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("EXCEPT", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("EXEC", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("EXECUTE", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("EXISTS", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("EXIT", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("EXTERNAL", Colors.DarkGreen, FontWeights.SemiBold),

            new ReservedWord("FETCH", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("FILE", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("FILLFACTOR", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("FOR", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("FOREIGN", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("FREETEXT", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("FREETEXTTABLE", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("FROM", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("FULL", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("FUNCTION", Colors.DarkGreen, FontWeights.SemiBold),

            new ReservedWord("GOTO", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("GRANT", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("GROUP", Colors.DarkGreen, FontWeights.SemiBold),

            new ReservedWord("HAVING", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("HOLDLOCK", Colors.DarkGreen, FontWeights.SemiBold),

            new ReservedWord("IDENTITY", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("IDENTITY_INSERT", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("IDENTITYCOL", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("IF", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("IN", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("INDEX", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("INNER", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("INSERT", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("INTERSECT", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("INTO", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("IS", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("int", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("integer", Colors.DarkGreen, FontWeights.SemiBold),

            new ReservedWord("JOIN", Colors.DarkGreen, FontWeights.SemiBold),

            new ReservedWord("KEY", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("KILL", Colors.DarkGreen, FontWeights.SemiBold),

            new ReservedWord("LEFT", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("LIKE", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("LIMIT", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("LINENO", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("LOAD", Colors.DarkGreen, FontWeights.SemiBold),

            new ReservedWord("MERGE", Colors.DarkGreen, FontWeights.SemiBold),

            new ReservedWord("NATIONAL", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("NOCHECK", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("NONCLUSTERED", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("NOT", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("NULL", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("NULLIF", Colors.DarkGreen, FontWeights.SemiBold),

            new ReservedWord("OF", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("OFF", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("OFFSETS", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("ON", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("OPEN", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("OPENDATASOURCE", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("OPENQUERY", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("OPENROWSET", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("OPENXML", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("OPTION", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("OR", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("ORDER", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("OUTER", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("OVER", Colors.DarkGreen, FontWeights.SemiBold),

            new ReservedWord("PERCENT", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("PIVOT", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("PLAN", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("PRECISION", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("PRIMARY", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("PRINT", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("PROC", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("PROCEDURE", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("PUBLIC", Colors.DarkGreen, FontWeights.SemiBold),

            new ReservedWord("RAISERROR", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("READ", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("READTEXT", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("RECONFIGURE", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("REFERENCES", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("REPLICATION", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("RESTORE", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("RESTRICT", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("RETURN", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("REVERT", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("REVOKE", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("RIGHT", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("ROLLBACK", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("ROWCOUNT", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("ROWGUIDCOL", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("RULE", Colors.DarkGreen, FontWeights.SemiBold),

            new ReservedWord("SAVE", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("SCHEMA", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("SECURITYAUDIT", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("SELECT", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("SEMANTICKEYPHRASETABLE", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("SEMANTICSIMILARITYDETAILSTABLE", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("SEMANTICSIMILARITYTABLE", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("SESSION_USER", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("SET", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("SETUSER", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("SHUTDOWN", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("SOME", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("STATISTICS", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("SYSTEM_USER", Colors.DarkGreen, FontWeights.SemiBold),

            new ReservedWord("TABLE", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("TABLESAMPLE", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("TEXTSIZE", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("THEN", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("TO", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("TOP", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("TRAN", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("TRANSACTION", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("TRIGGER", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("TRUNCATE", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("TRY_CONVERT", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("TSEQUAL", Colors.DarkGreen, FontWeights.SemiBold),

            new ReservedWord("UNION", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("UNIQUE", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("UNPIVOT", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("UPDATE", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("UPDATETEXT", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("USE", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("USER", Colors.DarkGreen, FontWeights.SemiBold),

            new ReservedWord("VALUES", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("VARYING", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("VIEW", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("VARCHAR", Colors.DarkGreen, FontWeights.SemiBold),

            new ReservedWord("WAITFOR", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("WHEN", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("WHERE", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("WHILE", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("WITH", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("WITHIN GROUP", Colors.DarkGreen, FontWeights.SemiBold),
            new ReservedWord("WRITETEXT", Colors.DarkGreen, FontWeights.SemiBold),

            // functions
            new ReservedWord("ISNULL", Colors.DarkBlue, FontWeights.SemiBold),
            new ReservedWord("CAST", Colors.DarkBlue, FontWeights.SemiBold),
            new ReservedWord("AVG", Colors.DarkBlue, FontWeights.SemiBold),
            new ReservedWord("SUM", Colors.DarkBlue, FontWeights.SemiBold),
            new ReservedWord("MIN", Colors.DarkBlue, FontWeights.SemiBold),
            new ReservedWord("MAX", Colors.DarkBlue, FontWeights.SemiBold),
            new ReservedWord("ROUND", Colors.DarkBlue, FontWeights.SemiBold)

        };

        public virtual string Quote { get; set; } = "'";

        #endregion


        #region CONSTRUCTORS

        public BaseTranslator() {
            Reset();
        }

        public BaseTranslator(TranslateOptions formatOptions) {
            Reset();
            FormatOptions = formatOptions;
        }

        #endregion


        #region METHODS

        public string Indentation(int level = 0) {
            string indentation = "";
            string indentationMeasure = new string(' ', FormatOptions.IndentationSize);

            for (int i = 1; i <= level; i++) {
                indentation += indentationMeasure;
            }

            return indentation;
        }

        public virtual void Reset() {
            Data = new Informations();
        }

        public virtual FlowDocument GetFlowDocument(string text) {
            FlowDocument document = new FlowDocument();

            // create a flow document from the string
            using (StringReader reader = new StringReader(text)) {
                string newLine;
                while ((newLine = reader.ReadLine()) != null) {
                    Paragraph paragraph = Format(newLine);
                    document.Blocks.Add(paragraph);
                }
            }

            return document;
        }

        /// <summary>
        /// formats the final text to a Paragraph of FlowDocument with colors and styles
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        protected Paragraph Format(string text) {
            Paragraph paragraph = new Paragraph();

            int length = text.Length;

            // if the text is not empty then search for words to format
            if (length > 0) {
                int i = 0;
                int previousTextStart = 0;
                int previousTextLength = 0;
                ReservedWord previousText = new ReservedWord();

                while (i < length) {
                    previousText.Text = text.Substring(previousTextStart, previousTextLength);
                    string current = text.Substring(i, length - i);

                    bool found = false;
                    ReservedWord word = new ReservedWord("", Colors.Brown, FontWeights.Normal);

                    // check for text
                    if (current[0] == Quote[0]) {
                        word.Text = current.Substring(1);

                        int nextPos = word.Text.IndexOf(Quote[0]);
                        if (nextPos > 0) {
                            word.Text = current.Substring(0, nextPos + 2);
                        }
                        else {
                            word.Text = current;
                        }

                        found = true;
                    }
                    else if (current[0] == '~') {
                        word.Text = current.Substring(1);
                        word.ForegroundColor = new SolidColorBrush(Colors.Red);
                        word.FontWeight = FontWeights.Bold;

                        int nextPos = word.Text.IndexOf('~');
                        if (nextPos > 0) {
                            word.Text = current.Substring(0, nextPos + 2);
                        }
                        else {
                            word.Text = current;
                        }

                        found = true;
                    }
                    else {
                        int wordsIndex = 0;

                        // from the current position check if the text begins with a word that must be formatted
                        while (wordsIndex < reservedWords.Count) {
                            word = reservedWords[wordsIndex];

                            if (current.ToLower().StartsWith(word.Text.ToLower())) {

                                // check if the text is a word
                                if (IsWord(i, word.Text)) {
                                    word.Text = FormattedText(word.Text);

                                    found = true;
                                    break;
                                }
                            }
                            wordsIndex++;
                        }
                    }

                    if (found) {
                        // add in the paragraph the previous characters that are formatted
                        AddTextToParagraph(previousText);

                        // add the formatted word in the paragraph
                        AddTextToParagraph(word);

                        i += word.Text.Length;
                        previousTextStart = i;
                        previousTextLength = 0;

                    }
                    else {
                        i++;
                        previousTextLength++;
                    }
                }

                // add in the paragraph what is left from unformatted characters at the end
                string lastText = text.Substring(previousTextStart, previousTextLength);
                AddTextToParagraph(new ReservedWord(lastText));

            }

            // format a text based on the FormatOptions
            string FormattedText(string word) {
                string formattedWord;

                if (FormatOptions.KeywordCasing == (int)KeywordCasing.Lowercase) {
                    formattedWord = word.ToLower();
                }
                else if (FormatOptions.KeywordCasing == (int)KeywordCasing.Uppercase) {
                    formattedWord = word.ToUpper();
                }
                else if (FormatOptions.KeywordCasing == (int)KeywordCasing.PascalCase) {
                    TextInfo info = CultureInfo.CurrentCulture.TextInfo;
                    formattedWord = info.ToTitleCase(word.ToLower()).Replace(" ", string.Empty);
                }
                else {
                    formattedWord = word;
                }

                return formattedWord;
            }

            // check if the current text is word by checking its bounding characters
            bool IsWord(int startPosition, string word) {
                if (startPosition != 0 && !char.IsWhiteSpace(text[startPosition - 1]) && char.IsLetterOrDigit(text[startPosition - 1]))
                    return false;

                if (startPosition + word.Length <= length - 1 && !char.IsWhiteSpace(text[startPosition + +word.Length]) && char.IsLetterOrDigit(text[startPosition + +word.Length]))
                    return false;

                return true;
            }

            // add a text in the paragraph
            void AddTextToParagraph(ReservedWord word) {
                Run currentRun = new Run(word.Text);

                currentRun.Foreground = word.ForegroundColor;
                currentRun.FontWeight = word.FontWeight;

                paragraph.Inlines.Add(currentRun);
            }

            return paragraph;
        }

        /// <summary>
        /// Insert new parameters in the list with the query parameters
        /// </summary>
        /// <param name="parameter"></param>
        protected void InsertIntoQueryParametersList(string parameter) {
            if (!FormatOptions.QueryParameters.ContainsKey(parameter)) {
                FormatOptions.QueryParameters.Add(parameter, parameter);
            }
        }

        /// <summary>
        /// Returns the value of a parameter in the query parameters list. 
        /// If no parameter exists in the list then returns the parameter name
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        protected string GetQueryParameterFromList(string parameter) {
            string value = parameter;

            if (FormatOptions.QueryParameters.ContainsKey(parameter)) {
                value = FormatOptions.QueryParameters[parameter];
            }

            return value;

        }

        /// <summary>
        /// Return the corresponding SQL function. 
        /// It is a separated function because some SQL functions are implemented 
        /// differently in various databases
        /// </summary>
        /// <param name="functionName"></param>
        /// <returns></returns>
        protected virtual string GetFunctionName(string functionName) {
            return functionName;
        }

        public virtual string InsertSpecificationParse(InsertSpecification expression, object data = null) {
            string result = "";

            try {
                string target;
                List<string> columns = new List<string>();
                List<string> values = new List<string>();

                //// TARGET /////

                TableReference table = expression.Target;
                if (table is NamedTableReference namedTableReference) {
                    target = NamedTableReferenceParse(namedTableReference);
                }
                else if (table is QueryDerivedTable queryDerivedTable) {
                    this.Data.Level++;
                    target = $"({QueryExpressionParse(queryDerivedTable.QueryExpression, true)})";
                    this.Data.Level--;

                    // check if the table has an alias
                    if (queryDerivedTable.Alias != null) {
                        target += $" AS {queryDerivedTable.Alias?.Value}";
                    }
                }
                else if (table is QualifiedJoin qualifiedJoin) {
                    target = QualifiedJoinParse(qualifiedJoin);
                }
                else {
                    target = $"~UNKNOWN TableReference~";
                }


                //// COLUMNS /////

                foreach (ColumnReferenceExpression column in expression.Columns) {
                    if (column is ColumnReferenceExpression columnReferenceExpression) {
                        string columnName = ColumnReferenceExpressionParse(columnReferenceExpression);
                        columns.Add(columnName);
                    }
                    else {
                        columns.Add("~UNKNOWN ColumnReferenceExpression~");
                    }
                }


                //// VALUES /////
                var source = expression.InsertSource as ValuesInsertSource;
                foreach (RowValue row in source.RowValues) {
                    string rowValues = "";

                    foreach (ScalarExpression value in row.ColumnValues) {
                        if (rowValues != "") {
                            rowValues += ", ";
                        }

                        if (value is StringLiteral stringLiteral) {
                            rowValues += $"{Quote}{stringLiteral.Value}{Quote}";
                        }
                        else if (value is IntegerLiteral integerLiteral) {
                            rowValues += $"{integerLiteral.Value}";
                        }
                        else if (value is NumericLiteral numericLiteral) {
                            rowValues += $"{numericLiteral.Value}";
                        }
                        else if (value is NullLiteral) {
                            rowValues += "NULL";
                        }
                        else if (value is VariableReference variableReference) {
                            string variableName = FormatOptions.ReplaceQueryParametersWithValues ? GetQueryParameterFromList(variableReference.Name) : variableReference.Name;

                            rowValues += $"{variableName}";

                            if (FormatOptions.UpdateQueryParametersList) {
                                InsertIntoQueryParametersList(variableReference.Name);
                            }
                        }
                        else {
                            rowValues += $"~UNKNOWN ScalarExpression~";
                        }
                    }
                    values.Add($"({rowValues})");
                }


                // TYPE RESULTS

                result = $"INSERT INTO {target} ";

                // columns
                if (columns.Count > 0) {
                    result += $"({columns[0]}";
                }
                for (int i = 1; i < columns.Count; i++) {
                    result += $", \n{Indentation(Data.Level + 1)}{columns[i]}";
                }
                result += $")";

                // values
                result += $"\nVALUES ";
                if (values.Count > 0) {
                    result += $"{values[0]}";
                }
                for (int i = 1; i < values.Count; i++) {
                    result += $", \n{Indentation(Data.Level + 1)}{values[i]}";
                }
            }
            catch {
                result = "~InsertSpecification ERROR~";
            }
            finally {
                result += $"\n\n";
            }

            return result;
        }

        public virtual string UpdateSpecificationParse(UpdateSpecification expression, object data = null) {
            string result = "";

            try {
                string target;
                List<string> columns = new List<string>();
                List<string> whereConditions = new List<string>();


                //// TARGET /////

                TableReference table = expression.Target;
                if (table is NamedTableReference namedTableReference) {
                    target = NamedTableReferenceParse(namedTableReference);
                }
                else if (table is QueryDerivedTable queryDerivedTable) {
                    this.Data.Level++;
                    target = $"({QueryExpressionParse(queryDerivedTable.QueryExpression, true)})";
                    this.Data.Level--;

                    // check if the table has an alias
                    if (queryDerivedTable.Alias != null) {
                        target += $" AS {queryDerivedTable.Alias?.Value}";
                    }
                }
                else if (table is QualifiedJoin qualifiedJoin) {
                    target = QualifiedJoinParse(qualifiedJoin);
                }
                else {
                    target = $"~UNKNOWN TableReference~";
                }


                //// COLUMNS /////
                string columnName;
                foreach (AssignmentSetClause assignmentSetClause in expression.SetClauses) {
                    if (assignmentSetClause.Column is ColumnReferenceExpression columnReferenceExpression1) {
                        columnName = ColumnReferenceExpressionParse(columnReferenceExpression1);
                    } 
                    else {
                        columnName = "~UNKNOWN ColumnReferenceExpression~";
                    }

                    if (assignmentSetClause.NewValue is StringLiteral stringLiteral) {
                        columnName += $" = '{stringLiteral.Value}'";
                    } 
                    else if (assignmentSetClause.NewValue is IntegerLiteral integerLiteral) {
                        columnName += $" = {integerLiteral.Value}";
                    } 
                    else if (assignmentSetClause.NewValue is NumericLiteral numericLiteral) {
                        columnName += $" = {numericLiteral.Value}";
                    } 
                    else if (assignmentSetClause.NewValue is NullLiteral) {
                        columnName += " = NULL";
                    } 
                    else if (assignmentSetClause.NewValue is VariableReference variableReference) {
                        string variableName = FormatOptions.ReplaceQueryParametersWithValues ? GetQueryParameterFromList(variableReference.Name) : variableReference.Name;

                        columnName += $" = {variableName}";

                        if (FormatOptions.UpdateQueryParametersList) {
                            InsertIntoQueryParametersList(variableReference.Name);
                        }
                    } 
                    else if (assignmentSetClause.NewValue is ColumnReferenceExpression columnReferenceExpression2) {
                        columnName += $" = {ColumnReferenceExpressionParse(columnReferenceExpression2)}";
                    }
                    else {
                        columnName += $" = ~UNKNOWN ScalarExpression~";
                    }

                    columns.Add(columnName);
                }


                //// WHERE //////

                if (expression.WhereClause != null) {
                    BooleanExpression condition = expression.WhereClause.SearchCondition;

                    if (condition is BooleanComparisonExpression booleanComparisonExpression) {
                        whereConditions.Add(BooleanComparisonExpressionParse(booleanComparisonExpression));
                    }
                    else if (condition is BooleanBinaryExpression booleanBinaryExpression) {
                        whereConditions.Add(BooleanBinaryExpressionParse(booleanBinaryExpression));
                    }
                    else if (condition is BooleanParenthesisExpression booleanParenthesisExpression) {
                        whereConditions.Add(BooleanParenthesisExpressionParse(booleanParenthesisExpression));
                    }
                    else if (condition is BooleanNotExpression booleanNotExpression) {
                        whereConditions.Add(BooleanNotExpressionParse(booleanNotExpression));
                    }
                    else if (condition is BooleanTernaryExpression booleanTernaryExpression) {
                        whereConditions.Add(BooleanTernaryExpressionParse(booleanTernaryExpression));
                    }
                    else if (condition is BooleanIsNullExpression booleanIsNullExpression) {
                        whereConditions.Add(BooleanIsNullExpressionParse(booleanIsNullExpression));
                    }
                    else if (condition is InPredicate inPredicate) {
                        whereConditions.Add(InPredicateParse(inPredicate));
                    }
                    else if (condition is LikePredicate likePredicate) {
                        whereConditions.Add(LikePredicateParse(likePredicate));
                    }
                    else if (condition is ExistsPredicate existsPredicate) {
                        whereConditions.Add(ExistsPredicateParse(existsPredicate));
                    }
                    else {
                        whereConditions.Add("~UNKNOWN BooleanExpression~");
                    }
                }


                // TYPE RESULTS

                result = $"UPDATE {target}\n";
                result += $"SET ";

                // columns
                if (columns.Count > 0) {
                    result += $"{columns[0]}";
                }
                for (int i = 1; i < columns.Count; i++) {
                    result += $", \n{Indentation(Data.Level + 1)}{columns[i]}";
                }

                // where conditions
                result += whereConditions.Count > 0 ? $"\n{Indentation(Data.Level)}WHERE " : "";
                if (whereConditions.Count > 0) {
                    result += $"{whereConditions[0]}";
                }
                for (int i = 1; i < whereConditions.Count; i++) {
                    result += $", \n{Indentation(Data.Level + 1)}{whereConditions[i]}";
                }

            }
            catch {
                result = "~UpdateSpecification ERROR~";
            }
            finally {
                result += $"\n\n";
            }

            return result;
        }

        public virtual string DeleteSpecificationParse(DeleteSpecification expression, object data = null) {
            string result = "";

            try {
                string target;
                List<string> columns = new List<string>();
                List<string> whereConditions = new List<string>();


                //// TARGET /////

                TableReference table = expression.Target;
                if (table is NamedTableReference namedTableReference) {
                    target = NamedTableReferenceParse(namedTableReference);
                }
                else if (table is QueryDerivedTable queryDerivedTable) {
                    this.Data.Level++;
                    target = $"({QueryExpressionParse(queryDerivedTable.QueryExpression, true)})";
                    this.Data.Level--;

                    // check if the table has an alias
                    if (queryDerivedTable.Alias != null) {
                        target += $" AS {queryDerivedTable.Alias?.Value}";
                    }
                }
                else if (table is QualifiedJoin qualifiedJoin) {
                    target = QualifiedJoinParse(qualifiedJoin);
                }
                else {
                    target = $"~UNKNOWN TableReference~";
                }


                //// WHERE //////

                if (expression.WhereClause != null) {
                    BooleanExpression condition = expression.WhereClause.SearchCondition;

                    if (condition is BooleanComparisonExpression booleanComparisonExpression) {
                        whereConditions.Add(BooleanComparisonExpressionParse(booleanComparisonExpression));
                    }
                    else if (condition is BooleanBinaryExpression booleanBinaryExpression) {
                        whereConditions.Add(BooleanBinaryExpressionParse(booleanBinaryExpression));
                    }
                    else if (condition is BooleanParenthesisExpression booleanParenthesisExpression) {
                        whereConditions.Add(BooleanParenthesisExpressionParse(booleanParenthesisExpression));
                    }
                    else if (condition is BooleanNotExpression booleanNotExpression) {
                        whereConditions.Add(BooleanNotExpressionParse(booleanNotExpression));
                    }
                    else if (condition is BooleanTernaryExpression booleanTernaryExpression) {
                        whereConditions.Add(BooleanTernaryExpressionParse(booleanTernaryExpression));
                    }
                    else if (condition is BooleanIsNullExpression booleanIsNullExpression) {
                        whereConditions.Add(BooleanIsNullExpressionParse(booleanIsNullExpression));
                    }
                    else if (condition is InPredicate inPredicate) {
                        whereConditions.Add(InPredicateParse(inPredicate));
                    }
                    else if (condition is LikePredicate likePredicate) {
                        whereConditions.Add(LikePredicateParse(likePredicate));
                    }
                    else if (condition is ExistsPredicate existsPredicate) {
                        whereConditions.Add(ExistsPredicateParse(existsPredicate));
                    }
                    else {
                        whereConditions.Add("~UNKNOWN BooleanExpression~");
                    }
                }


                // TYPE RESULTS

                result = $"{Indentation(Data.Level)}DELETE FROM {target}";

                // where conditions
                result += whereConditions.Count > 0 ? $"\n{Indentation(Data.Level)}WHERE " : "";
                if (whereConditions.Count > 0) {
                    result += $"{whereConditions[0]}";
                }
                for (int i = 1; i < whereConditions.Count; i++) {
                    result += $", \n{Indentation(Data.Level + 1)}{whereConditions[i]}";
                }

            }
            catch {
                result = "~DeleteSpecification ERROR~";
            }
            finally {
                result += $"\n";
            }

            return result;
        }

        public virtual string CreateTableStatementParse(CreateTableStatement expression, object data = null) {
            string result = "";

            try {
                string target;

                List<string> columns = new List<string>();

                target = expression.SchemaObjectName.BaseIdentifier.Value;


                //// COLUMNS /////
                foreach (ColumnDefinition column in expression.Definition.ColumnDefinitions) {
                    string columnName = ColumnDefinitionParse(column);
                    columns.Add(columnName);
                }


                // TYPE RESULTS

                result = $"CREATE TABLE {target}\n (";

                // columns
                if (columns.Count > 0) {
                    result += $"{columns[0]}";
                }
                for (int i = 1; i < columns.Count; i++) {
                    result += $", \n{Indentation(Data.Level + 1)}{columns[i]}";
                }

                result += ")\n\n";

            }
            catch {
                result = "~CreateTableStatement ERROR~";
            }
            finally {
                result += $"\n\n";
            }

            return result;
        }

        public virtual string CreateViewStatementParse(CreateViewStatement expression, object data = null) {
            string result = "";

            try {
                string viewName = expression.SchemaObjectName.BaseIdentifier.Value;

                result = $"{Indentation(Data.Level)}CREATE VIEW {viewName} AS\n";
            }
            catch {
                result = "~CreateViewStatement ERROR~\n";
            }

            return result;
        }

        public virtual string SelectStatementParse(SelectStatement expression, object data = null) {
            string result = "";

            try {
                result = QueryExpressionParse(expression.QueryExpression);
            }
            catch {
                result = "~SelectStatement ERROR~";
            }
            finally {
                result += $"\n\n";
            }

            return result;
        }

        public virtual string QueryExpressionParse(QueryExpression expression, object data = null) {
            string result;

            try {
                if (expression is QuerySpecification querySpecification) {
                    result = QuerySpecificationParse(querySpecification);
                }
                else if (expression is QueryParenthesisExpression queryParenthesisExpression) {
                    result = QueryParenthesisExpressionParse(queryParenthesisExpression);
                }
                else if (expression is BinaryQueryExpression binaryQueryExpression) {
                    result = BinaryQueryExpressionParse(binaryQueryExpression);
                }
                else {
                    result = "~UNKNOWN QueryExpression~";
                }
            }
            catch {
                result = "~QueryExpression ERROR~";
            }

            return result;
        }

        public virtual string BinaryQueryExpressionParse(BinaryQueryExpression expression, object data = null) {
            string result;

            try {
                string firstQueryExpression;
                string secondQueryExpression;
                string type;

                if (expression.FirstQueryExpression is QuerySpecification querySpecification1) {
                    firstQueryExpression = QuerySpecificationParse(querySpecification1);
                }
                else if (expression.FirstQueryExpression is QueryParenthesisExpression queryParenthesisExpression1) {
                    firstQueryExpression = QueryParenthesisExpressionParse(queryParenthesisExpression1);
                }
                else if (expression.FirstQueryExpression is BinaryQueryExpression binaryQueryExpression1) {
                    firstQueryExpression = BinaryQueryExpressionParse(binaryQueryExpression1);
                }
                else {
                    firstQueryExpression = "~UNKNOWN QueryExpression~";
                }

                if (expression.SecondQueryExpression is QuerySpecification querySpecification2) {
                    secondQueryExpression = QuerySpecificationParse(querySpecification2);
                }
                else if (expression.SecondQueryExpression is QueryParenthesisExpression queryParenthesisExpression2) {
                    secondQueryExpression = QueryParenthesisExpressionParse(queryParenthesisExpression2);
                }
                else if (expression.SecondQueryExpression is BinaryQueryExpression binaryQueryExpression2) {
                    secondQueryExpression = BinaryQueryExpressionParse(binaryQueryExpression2);
                }
                else {
                    secondQueryExpression = "~UNKNOWN QueryExpression~";
                }

                if (expression.BinaryQueryExpressionType == BinaryQueryExpressionType.Union) {
                    type = "UNION";
                }
                else if (expression.BinaryQueryExpressionType == BinaryQueryExpressionType.Except) {
                    type = "EXCEPT";
                }
                else if (expression.BinaryQueryExpressionType == BinaryQueryExpressionType.Intersect) {
                    type = "INTERSECT";
                }
                else {
                    type = "~UNKNOWN BinaryQueryExpressionType~";
                }

                string all;
                if (expression.All == true) {
                    all = " All";
                }
                else {
                    all = "";
                }

                result = $"{firstQueryExpression}\n{Indentation(Data.Level)}{type}{all}\n{Indentation(Data.Level)}{secondQueryExpression}";
            }
            catch {
                result = "~ParenthesisExpression ERROR~";
            }

            return result;
        }

        public virtual string QuerySpecificationParse(QuerySpecification querySpecification, object data = null) {
            string result;

            try {
                string Type;
                List<string> columns = new List<string>();
                List<string> tables = new List<string>();
                List<string> whereConditions = new List<string>();
                List<string> groupConditions = new List<string>();
                List<string> havingConditions = new List<string>();
                List<string> orderByConditions = new List<string>();
                bool isDistinct;
                bool isTopSelection;
                bool isTopPercent;
                int isTopCount;


                //// VARIOUS /////

                Type = "SELECT";
                isDistinct = querySpecification.UniqueRowFilter.ToString().ToUpper().Equals("DISTINCT");
                isTopSelection = querySpecification.TopRowFilter != null;
                isTopPercent = isTopSelection ? querySpecification.TopRowFilter.Percent : false;
                isTopCount = (querySpecification.TopRowFilter?.Expression is IntegerLiteral) ? int.Parse((querySpecification.TopRowFilter.Expression as IntegerLiteral).Value) : 0;


                //// COLUMNS /////

                IList<SelectElement> elements = querySpecification.SelectElements;
                foreach (SelectElement column in elements) {
                    if (column is SelectScalarExpression selectScalarExpression) {

                        ScalarExpression expression = selectScalarExpression.Expression;
                        if (expression is ColumnReferenceExpression columnReferenceExpression) {

                            string columnName = ColumnReferenceExpressionParse(columnReferenceExpression);

                            // check if there is an alias for the column
                            if (selectScalarExpression.ColumnName != null) {
                                columnName += $" AS {selectScalarExpression.ColumnName.Value}";
                            }

                            columns.Add(columnName);
                        }
                        else if (expression is ScalarSubquery scalarSubquery) {
                            this.Data.Level++;
                            string columnName = $"({QueryExpressionParse(scalarSubquery.QueryExpression, true)})";
                            this.Data.Level--;

                            // check if there is an alias for the column
                            if (selectScalarExpression.ColumnName != null) {
                                columnName += $" AS {selectScalarExpression.ColumnName.Value}";
                            }

                            columns.Add(columnName);
                        }
                        else if (expression is SearchedCaseExpression searchedCaseExpression) {
                            this.Data.Level++;
                            string columnName = $"{SearchedCaseExpressionParse(searchedCaseExpression)}";
                            this.Data.Level--;

                            // check if there is an alias for the column
                            if (selectScalarExpression.ColumnName != null) {
                                columnName += $" AS {selectScalarExpression.ColumnName.Value}";
                            }

                            columns.Add(columnName);
                        }
                        else if (expression is IntegerLiteral integerLiteral) {
                            string columnName = integerLiteral.Value;

                            // check if there is an alias for the column
                            if (selectScalarExpression.ColumnName != null) {
                                columnName += $" AS {selectScalarExpression.ColumnName.Value}";
                            }

                            columns.Add(columnName);
                        }
                        else if (expression is StringLiteral stringLiteral) {
                            string columnName = $"{Quote}{stringLiteral.Value}{Quote}";

                            // check if there is an alias for the column
                            if (selectScalarExpression.ColumnName != null) {
                                columnName += $" AS {selectScalarExpression.ColumnName.Value}";
                            }

                            columns.Add(columnName);
                        }
                        else if (expression is NumericLiteral numericLiteral) {
                            string columnName = $"{numericLiteral.Value}";

                            // check if there is an alias for the column
                            if (selectScalarExpression.ColumnName != null) {
                                columnName += $" AS {selectScalarExpression.ColumnName.Value}";
                            }

                            columns.Add(columnName);
                        }
                        else if (expression is NullLiteral) {
                            string columnName = $"";

                            // check if there is an alias for the column
                            if (selectScalarExpression.ColumnName != null) {
                                columnName += $" AS {selectScalarExpression.ColumnName.Value}";
                            }

                            columns.Add(columnName);
                        }
                        else if (expression is VariableReference variableReference) {
                            string variableName = FormatOptions.ReplaceQueryParametersWithValues ? GetQueryParameterFromList(variableReference.Name) : variableReference.Name;

                            string columnName = $"{variableName}";

                            if (FormatOptions.UpdateQueryParametersList) {
                                InsertIntoQueryParametersList(variableReference.Name);
                            }

                            // check if there is an alias for the column
                            if (selectScalarExpression.ColumnName != null) {
                                columnName += $" AS {selectScalarExpression.ColumnName.Value}";
                            }

                            columns.Add(columnName);
                        }
                        else if (expression is FunctionCall functionCall) {
                            string columnName = FunctionCallParse(functionCall);

                            if (selectScalarExpression.ColumnName is IdentifierOrValueExpression identifierOrValueExpression) {
                                columnName += $" AS {identifierOrValueExpression.Value}";
                            }

                            columns.Add(columnName);
                        }
                        else if (expression is ParenthesisExpression parenthesisExpression) {
                            string columnName = ParenthesisExpressionParse(parenthesisExpression);

                            // check if there is an alias for the column
                            if (selectScalarExpression.ColumnName != null) {
                                columnName += $" AS {selectScalarExpression.ColumnName.Value}";
                            }

                            columns.Add(columnName);
                        }
                        else if (expression is BinaryExpression binaryExpression) {
                            string columnName = BinaryExpressionParse(binaryExpression);

                            // check if there is an alias for the column
                            if (selectScalarExpression.ColumnName != null) {
                                columnName += $" AS {selectScalarExpression.ColumnName.Value}";
                            }

                            columns.Add(columnName);
                        }
                        else if (expression is CastCall castCall) {
                            string columnName = CastCallParse(castCall);

                            // check if there is an alias for the column
                            if (selectScalarExpression.ColumnName != null) {
                                columnName += $" AS {selectScalarExpression.ColumnName.Value}";
                            }

                            columns.Add(columnName);
                        }
                        else {
                            columns.Add("~UNKNOWN ScalarExpression~");
                        }
                    }
                    else if (column is SelectStarExpression selectStarExpression) {
                        columns.Add(SelectStarExpression(selectStarExpression));
                    }
                    else if (column is SelectSetVariable) {
                        columns.Add("~UNKNOWN SelectElement~");
                    }
                    else {
                        columns.Add("~UNKNOWN SelectElement~");
                    }
                }


                //// TABLES //////

                IList<TableReference> tableReferences = querySpecification.FromClause?.TableReferences;
                if (tableReferences != null) {
                    foreach (TableReference table in tableReferences) {
                        if (table is NamedTableReference namedTableReference) {
                            tables.Add(NamedTableReferenceParse(namedTableReference));
                        }
                        else if (table is QueryDerivedTable queryDerivedTable) {
                            this.Data.Level++;
                            string tableName = $"({QueryExpressionParse(queryDerivedTable.QueryExpression, true)})";
                            this.Data.Level--;

                            // check if the table has an alias
                            if (queryDerivedTable.Alias != null) {
                                tableName += $" AS {queryDerivedTable.Alias?.Value}";
                            }

                            tables.Add(tableName);
                        }
                        else if (table is QualifiedJoin qualifiedJoin) {
                            tables.Add(QualifiedJoinParse(qualifiedJoin));
                        }
                        else {
                            tables.Add($"~UNKNOWN TableReference~");
                        }
                    }
                }


                //// WHERE //////

                if (querySpecification.WhereClause != null) {
                    BooleanExpression condition = querySpecification.WhereClause.SearchCondition;

                    if (condition is BooleanComparisonExpression booleanComparisonExpression) {
                        whereConditions.Add(BooleanComparisonExpressionParse(booleanComparisonExpression));
                    }
                    else if (condition is BooleanBinaryExpression booleanBinaryExpression) {
                        whereConditions.Add(BooleanBinaryExpressionParse(booleanBinaryExpression));
                    }
                    else if (condition is BooleanParenthesisExpression booleanParenthesisExpression) {
                        whereConditions.Add(BooleanParenthesisExpressionParse(booleanParenthesisExpression));
                    }
                    else if (condition is BooleanNotExpression booleanNotExpression) {
                        whereConditions.Add(BooleanNotExpressionParse(booleanNotExpression));
                    }
                    else if (condition is BooleanTernaryExpression booleanTernaryExpression) {
                        whereConditions.Add(BooleanTernaryExpressionParse(booleanTernaryExpression));
                    }
                    else if (condition is BooleanIsNullExpression booleanIsNullExpression) {
                        whereConditions.Add(BooleanIsNullExpressionParse(booleanIsNullExpression));
                    }
                    else if (condition is InPredicate inPredicate) {
                        whereConditions.Add(InPredicateParse(inPredicate));
                    }
                    else if (condition is LikePredicate likePredicate) {
                        whereConditions.Add(LikePredicateParse(likePredicate));
                    }
                    else if (condition is ExistsPredicate existsPredicate) {
                        whereConditions.Add(ExistsPredicateParse(existsPredicate));
                    }
                    else {
                        whereConditions.Add("~UNKNOWN BooleanExpression~");
                    }
                }


                //// GROUPED //////

                if (querySpecification.GroupByClause != null) {
                    foreach (GroupingSpecification condition in querySpecification.GroupByClause.GroupingSpecifications) {
                        if (condition is ExpressionGroupingSpecification expressionGroupingSpecification) {
                            groupConditions.Add(ExpressionGroupingSpecificationParse(expressionGroupingSpecification));
                        }
                        else {
                            groupConditions.Add("~UNKNOWN GroupingSpecification~");
                        }
                    }
                }


                //// HAVING //////

                if (querySpecification.HavingClause != null) {
                    BooleanExpression condition = querySpecification.HavingClause.SearchCondition;

                    if (condition is BooleanComparisonExpression booleanComparisonExpression) {
                        havingConditions.Add(BooleanComparisonExpressionParse(booleanComparisonExpression));
                    }
                    else if (condition is BooleanBinaryExpression booleanBinaryExpression) {
                        havingConditions.Add(BooleanBinaryExpressionParse(booleanBinaryExpression));
                    }
                    else if (condition is BooleanParenthesisExpression booleanParenthesisExpression) {
                        havingConditions.Add(BooleanParenthesisExpressionParse(booleanParenthesisExpression));
                    }
                    else if (condition is BooleanNotExpression booleanNotExpression) {
                        havingConditions.Add(BooleanNotExpressionParse(booleanNotExpression));
                    }
                    else if (condition is BooleanTernaryExpression booleanTernaryExpression) {
                        havingConditions.Add(BooleanTernaryExpressionParse(booleanTernaryExpression));
                    }
                    else if (condition is BooleanIsNullExpression booleanIsNullExpression) {
                        havingConditions.Add(BooleanIsNullExpressionParse(booleanIsNullExpression));
                    }
                    else if (condition is InPredicate inPredicate) {
                        havingConditions.Add(InPredicateParse(inPredicate));
                    }
                    else if (condition is LikePredicate likePredicate) {
                        havingConditions.Add(LikePredicateParse(likePredicate));
                    }
                    else if (condition is ExistsPredicate existsPredicate) {
                        havingConditions.Add(ExistsPredicateParse(existsPredicate));
                    }
                    else {
                        havingConditions.Add("~UNKNOWN BooleanExpression~");
                    }
                }


                //// ORDER BY //////

                if (querySpecification.OrderByClause != null) {
                    foreach (ExpressionWithSortOrder condition in querySpecification.OrderByClause.OrderByElements) {
                        if (condition is ExpressionWithSortOrder expressionWithSortOrder) {
                            orderByConditions.Add(ExpressionWithSortOrderParse(expressionWithSortOrder));
                        }
                        else {
                            orderByConditions.Add("~UKNOWN ExpressionWithSortOrder~");
                        }
                    }
                }


                // type results

                result = $"{Type} ";

                result += isDistinct ? "DISTINCT " : "";
                result += isTopSelection ? $"TOP {isTopCount} " : "";
                result += isTopPercent ? "PERCENT " : "";

                // columns
                if (columns.Count > 0) {
                    result += $"{columns[0]}";
                }
                for (int i = 1; i < columns.Count; i++) {
                    result += $", \n{Indentation(Data.Level + 1)}{columns[i]}";
                }

                //tables
                result += tables.Count > 0 ? $"\n{Indentation(Data.Level)}FROM " : "";
                if (tables.Count > 0) {
                    result += $"{tables[0]}";
                }
                for (int i = 1; i < tables.Count; i++) {
                    result += $", \n{Indentation(Data.Level + 1)}{tables[i]}";
                }

                // where conditions
                result += whereConditions.Count > 0 ? $"\n{Indentation(Data.Level)}WHERE " : "";
                if (whereConditions.Count > 0) {
                    result += $"{whereConditions[0]}";
                }
                for (int i = 1; i < whereConditions.Count; i++) {
                    result += $", \n{Indentation(Data.Level + 1)}{whereConditions[i]}";
                }

                // group conditions
                result += groupConditions.Count > 0 ? $"\n{Indentation(Data.Level)}GROUP BY " : "";
                if (groupConditions.Count > 0) {
                    result += $"{groupConditions[0]}";
                }
                for (int i = 1; i < groupConditions.Count; i++) {
                    result += $", \n{Indentation(Data.Level + 1)}{groupConditions[i]}";
                }

                // having conditions
                result += havingConditions.Count > 0 ? $"\n{Indentation(Data.Level)}HAVING " : "";
                if (havingConditions.Count > 0) {
                    result += $"{havingConditions[0]}";
                }
                for (int i = 1; i < havingConditions.Count; i++) {
                    result += $", \n{Indentation(Data.Level + 1)}{havingConditions[i]}";
                }

                // order by conditions 
                result += orderByConditions.Count > 0 ? $"\n{Indentation(Data.Level)}ORDER BY " : "";
                if (orderByConditions.Count > 0) {
                    result += $"{orderByConditions[0]}";
                }
                for (int i = 1; i < orderByConditions.Count; i++) {
                    result += $", \n{Indentation(Data.Level + 1)}{orderByConditions[i]}";
                }
            }
            catch {
                result = "~QuerySpecification ERROR~";
            }

            return result;
        }
        
        public virtual string FunctionCallParse(FunctionCall expression, object data = null) {
            string result;

            string functionName = GetFunctionName(expression.FunctionName.Value);
            result = $"{functionName}";
            string parameters = "";

            foreach (ScalarExpression parameter in expression.Parameters) {

                if (parameters != "") {
                    parameters += ", ";
                }

                if (parameter is CastCall castCall) {
                    string dataType;
                    if (castCall.DataType is SqlDataTypeReference) {
                        dataType = castCall.DataType.Name.BaseIdentifier.Value;
                    } else {
                        dataType = "~UNKNOWN DataType~";
                    }

                    string parameter2;
                    if (castCall.Parameter is SearchedCaseExpression searchedCaseExpression) {
                        parameter2 = SearchedCaseExpressionParse(searchedCaseExpression);
                    } else {
                        parameter2 = "~UNKNOWN ScalarExpression~";
                    }

                    parameters += $"CAST({parameter2} AS {dataType})";
                } 
                else if (parameter is ColumnReferenceExpression columnReferenceExpression) {
                    parameters += $"{ColumnReferenceExpressionParse(columnReferenceExpression)}";
                } 
                else if (parameter is SearchedCaseExpression searchedCaseExpression) {
                    Data.Level++;
                    parameters += $"{SearchedCaseExpressionParse(searchedCaseExpression)}";
                    Data.Level--;
                } 
                else if (parameter is ParenthesisExpression parenthesisExpression) {
                    parameters += $"({ParenthesisExpressionParse(parenthesisExpression)})";
                } 
                else if (parameter is IntegerLiteral integerLiteral) {
                    parameters += integerLiteral.Value;
                } 
                else if (parameter is StringLiteral stringLiteral) {
                    parameters += $"{Quote}{stringLiteral.Value}{Quote}";
                } 
                else if (parameter is NumericLiteral numericLiteral) {
                    parameters += $"{numericLiteral.Value}";
                } 
                else if (parameter is NullLiteral nullLiteral) {
                    parameters += $"NULL";
                } 
                else if (parameter is BinaryExpression binaryExpression) {
                    parameters += BinaryExpressionParse(binaryExpression);
                } 
                else if (parameter is UnaryExpression unaryExpression) {
                    parameters += UnaryExpressionParse(unaryExpression);
                } 
                else if (parameter is FunctionCall functionCall) {
                    parameters += FunctionCallParse(functionCall);
                } 
                else {
                    parameters += $"~UNKNOWN ScalarExpression~";
                }

            }

            string uniqueRow = "";
            if (expression.UniqueRowFilter == UniqueRowFilter.Distinct) {
                uniqueRow = "Distinct ";
            }
            else if (expression.UniqueRowFilter == UniqueRowFilter.All) {
                uniqueRow = "All ";
            }

            result += $"({uniqueRow}{parameters})";

            return result;
        }

        public virtual string QueryParenthesisExpressionParse(QueryParenthesisExpression expression, object data = null) {
            string result;

            try {
                result = $"({QueryExpressionParse(expression.QueryExpression)})";
            }
            catch {
                result = "~QueryParenthesisExpression ERROR~";
            }

            return result;
        }

        public virtual string ParenthesisExpressionParse(ParenthesisExpression expression, object data = null) {
            string result;

            try {
                if (expression.Expression is ColumnReferenceExpression columnReferenceExpression) {
                    result = $"({ColumnReferenceExpressionParse(columnReferenceExpression)})";
                }
                else if (expression.Expression is ParenthesisExpression parenthesisExpression) {
                    result = $"({ParenthesisExpressionParse(parenthesisExpression)})";
                }
                else if (expression.Expression is SearchedCaseExpression searchedCaseExpression) {
                    Data.Level++;
                    result = $"({SearchedCaseExpressionParse(searchedCaseExpression)})";
                    Data.Level--;
                }
                else if (expression.Expression is BinaryExpression binaryExpression) {
                    result = $"({BinaryExpressionParse(binaryExpression)})";
                }
                else {
                    result = "~UNKNOWN ParenthesisExpression~";
                }
            }
            catch {
                result = "~ParenthesisExpression ERROR~";
            }

            return result;
        }

        public virtual string BinaryExpressionParse(BinaryExpression expression, object data = null) {
            string result;

            try {
                string firstExpression;
                string secondExpression;
                string symbol;

                if (expression.FirstExpression is ColumnReferenceExpression columnReferenceExpression1) {
                    firstExpression = ColumnReferenceExpressionParse(columnReferenceExpression1);
                } 
                else if (expression.FirstExpression is ParenthesisExpression parenthesisExpression1) {
                    firstExpression = $"({ParenthesisExpressionParse(parenthesisExpression1)})";
                } 
                else if (expression.FirstExpression is IntegerLiteral integerLiteral1) {
                    firstExpression = $"{integerLiteral1.Value}";
                } 
                else if (expression.FirstExpression is StringLiteral stringLiteral1) {
                    firstExpression = $"{Quote}{stringLiteral1.Value}{Quote}";
                } 
                else if (expression.FirstExpression is NumericLiteral numericLiteral1) {
                    firstExpression = $"{numericLiteral1.Value}";
                }
                else if (expression.FirstExpression is NullLiteral nullLiteral1) {
                    firstExpression = $"NULL";
                }
                else if (expression.FirstExpression is BinaryExpression binaryExpression1) {
                    firstExpression = BinaryExpressionParse(binaryExpression1);
                } 
                else if (expression.FirstExpression is CastCall castCall1) {
                    firstExpression = CastCallParse(castCall1);
                } 
                else if (expression.FirstExpression is FunctionCall functionCall1) {
                    firstExpression = FunctionCallParse(functionCall1);
                } 
                else if (expression.FirstExpression is UnaryExpression unaryExpression1) {
                    firstExpression = UnaryExpressionParse(unaryExpression1);
                } 
                else {
                    firstExpression = "~UNKNOWN ScalarExpression~";
                }

                if (expression.SecondExpression is ColumnReferenceExpression columnReferenceExpression2) {
                    secondExpression = ColumnReferenceExpressionParse(columnReferenceExpression2);
                } 
                else if (expression.SecondExpression is ParenthesisExpression parenthesisExpression2) {
                    secondExpression = $"({ParenthesisExpressionParse(parenthesisExpression2)})";
                } 
                else if (expression.SecondExpression is IntegerLiteral integerLiteral2) {
                    secondExpression = integerLiteral2.Value;
                }
                else if (expression.SecondExpression is StringLiteral stringLiteral2) {
                    secondExpression = $"{Quote}{stringLiteral2.Value}{Quote}";
                }
                else if (expression.SecondExpression is NumericLiteral numericLiteral2) {
                    secondExpression = $"{numericLiteral2.Value}";
                }
                else if (expression.SecondExpression is NullLiteral nullLiteral2) {
                    secondExpression = $"NULL";
                }
                else if (expression.SecondExpression is BinaryExpression binaryExpression2) {
                    secondExpression = BinaryExpressionParse(binaryExpression2);
                } 
                else if (expression.SecondExpression is CastCall castCall2) {
                    secondExpression = CastCallParse(castCall2);
                } 
                else if (expression.SecondExpression is FunctionCall functionCall2) {
                    secondExpression = FunctionCallParse(functionCall2);
                } 
                else if (expression.SecondExpression is UnaryExpression unaryExpression2) {
                    secondExpression = UnaryExpressionParse(unaryExpression2);
                } 
                else {
                    secondExpression = "~UNKNOWN ScalarExpression~";
                }

                if (expression.BinaryExpressionType == BinaryExpressionType.Modulo) {
                    symbol = "%";
                }
                else if (expression.BinaryExpressionType == BinaryExpressionType.Add) {
                    symbol = "+";
                }
                else if (expression.BinaryExpressionType == BinaryExpressionType.BitwiseAnd) {
                    symbol = "&";
                }
                else if (expression.BinaryExpressionType == BinaryExpressionType.BitwiseOr) {
                    symbol = "|";
                }
                else if (expression.BinaryExpressionType == BinaryExpressionType.BitwiseXor) {
                    symbol = "^";
                }
                else if (expression.BinaryExpressionType == BinaryExpressionType.Divide) {
                    symbol = "/";
                }
                else if (expression.BinaryExpressionType == BinaryExpressionType.Multiply) {
                    symbol = "*";
                }
                else if (expression.BinaryExpressionType == BinaryExpressionType.Subtract) {
                    symbol = "-";
                }
                else {
                    symbol = "~UNKNOWN BinaryExpressionType~";
                }

                result = $"{firstExpression} {symbol} {secondExpression}";
            }
            catch {
                result = "~BinaryExpression ERROR~";
            }

            return result;
        }

        public virtual string BooleanBinaryExpressionParse(BooleanBinaryExpression expression, object data = null) {

            string result;

            try {
                string firstExpression;
                string secondExpression;
                string comparison;

                // comparison
                if (expression.BinaryExpressionType == BooleanBinaryExpressionType.And) {
                    comparison = "AND";
                }
                else if (expression.BinaryExpressionType == BooleanBinaryExpressionType.Or) {
                    comparison = "OR";
                }
                else {
                    comparison = " ~UNKNOWN BooleanBinaryExpressionType~ ";
                }

                // first expression
                if (expression.FirstExpression is BooleanComparisonExpression booleanComparisonExpression1) {
                    firstExpression = $"{BooleanComparisonExpressionParse(booleanComparisonExpression1)}";
                }
                else if (expression.FirstExpression is BooleanBinaryExpression booleanBinaryExpression1) {
                    firstExpression = $"{BooleanBinaryExpressionParse(booleanBinaryExpression1)}";
                }
                else if (expression.FirstExpression is BooleanParenthesisExpression booleanParenthesisExpression1) {
                    firstExpression = $"{BooleanParenthesisExpressionParse(booleanParenthesisExpression1)}";
                }
                else if (expression.FirstExpression is BooleanNotExpression booleanNotExpression1) {
                    firstExpression = $"{BooleanNotExpressionParse(booleanNotExpression1)}";
                }
                else if (expression.FirstExpression is ExistsPredicate existsPredicate1) {
                    Data.Level++;
                    firstExpression = $"{ExistsPredicateParse(existsPredicate1)}";
                    Data.Level--;
                }
                else if (expression.FirstExpression is BooleanIsNullExpression booleanIsNullExpression1) {
                    firstExpression = BooleanIsNullExpressionParse(booleanIsNullExpression1);
                }
                else if (expression.FirstExpression is InPredicate inPredicate1) {
                    Data.Level++;
                    firstExpression = InPredicateParse(inPredicate1);
                    Data.Level--;
                }
                else if (expression.FirstExpression is BooleanTernaryExpression booleanTernaryExpression1) {
                    firstExpression = BooleanTernaryExpressionParse(booleanTernaryExpression1);
                } 
                else if (expression.FirstExpression is LikePredicate likePredicate1) {
                    firstExpression = LikePredicateParse(likePredicate1);
                } 
                else {
                    firstExpression = "~UNKNOWN BooleanExpression~";
                }

                // second expression
                if (expression.SecondExpression is BooleanComparisonExpression booleanComparisonExpression2) {
                    secondExpression = $"{BooleanComparisonExpressionParse(booleanComparisonExpression2)}";
                }
                else if (expression.SecondExpression is BooleanBinaryExpression booleanBinaryExpression2) {
                    secondExpression = $"{BooleanBinaryExpressionParse(booleanBinaryExpression2)}";
                }
                else if (expression.SecondExpression is BooleanParenthesisExpression booleanParenthesisExpression2) {
                    secondExpression = $"{BooleanParenthesisExpressionParse(booleanParenthesisExpression2)}";
                }
                else if (expression.SecondExpression is BooleanNotExpression booleanNotExpression2) {
                    secondExpression = $"{BooleanNotExpressionParse(booleanNotExpression2)}";
                }
                else if (expression.SecondExpression is ExistsPredicate existsPredicate2) {
                    Data.Level++;
                    secondExpression = $"{ExistsPredicateParse(existsPredicate2)}";
                    Data.Level--;
                }
                else if (expression.SecondExpression is InPredicate inPredicate2) {
                    Data.Level++;
                    secondExpression = InPredicateParse(inPredicate2);
                    Data.Level--;
                }
                else if (expression.SecondExpression is BooleanIsNullExpression booleanIsNullExpression2) {
                    secondExpression = BooleanIsNullExpressionParse(booleanIsNullExpression2);
                }
                else if (expression.SecondExpression is BooleanTernaryExpression booleanTernaryExpression2) {
                    secondExpression = BooleanTernaryExpressionParse(booleanTernaryExpression2);
                } 
                else if (expression.SecondExpression is LikePredicate likePredicate2) {
                    secondExpression = LikePredicateParse(likePredicate2);
                } 
                else {
                    secondExpression = "~UNKNOWN BooleanExpression~";
                }

                result = $"{firstExpression} \n{Indentation(Data.Level + 1)}{comparison} {secondExpression}";
            }
            catch {
                result = "~BooleanBinaryExpression ERROR~";
            }

            return result;
        }

        public virtual string BooleanComparisonExpressionParse(BooleanComparisonExpression expression, object data = null) {
            string result;

            try {
                string firstExpression;
                string secondExpression;
                string comparisonExpression;

                // first expression
                if (expression.FirstExpression is ColumnReferenceExpression columnReferenceExpression1) {
                    firstExpression = ColumnReferenceExpressionParse(columnReferenceExpression1);
                }
                else if (expression.FirstExpression is IntegerLiteral integerLiteral1) {
                    firstExpression = integerLiteral1.Value;
                }
                else if (expression.FirstExpression is StringLiteral stringLiteral1) {
                    firstExpression = $"{Quote}{stringLiteral1.Value}{Quote}";
                }
                else if (expression.FirstExpression is NumericLiteral numericLiteral1) {
                    firstExpression = $"{numericLiteral1.Value}";
                }
                else if (expression.FirstExpression is NullLiteral nullLiteral1) {
                    firstExpression = $"NULL";
                }
                else if (expression.FirstExpression is VariableReference variableReference1) {
                    string variableName = FormatOptions.ReplaceQueryParametersWithValues ? GetQueryParameterFromList(variableReference1.Name) : variableReference1.Name;

                    firstExpression = $"{variableName}";

                    if (FormatOptions.UpdateQueryParametersList) {
                        InsertIntoQueryParametersList(variableReference1.Name);
                    }
                }
                else if (expression.FirstExpression is FunctionCall functionCall1) {
                    firstExpression = FunctionCallParse(functionCall1);
                } 
                else if (expression.FirstExpression is CoalesceExpression coalesceExpression1) {
                    firstExpression = CoalesceExpressionParse(coalesceExpression1);
                } 
                else {
                    firstExpression = "~UNKNOWN BooleanComparisonExpression~";
                }

                // second expression
                if (expression.SecondExpression is ColumnReferenceExpression columnReferenceExpression2) {
                    secondExpression = ColumnReferenceExpressionParse(columnReferenceExpression2);
                }
                else if (expression.SecondExpression is IntegerLiteral integerLiteral2) {
                    secondExpression = integerLiteral2.Value;
                }
                else if (expression.SecondExpression is StringLiteral stringLiteral2) {
                    secondExpression = $"{Quote}{stringLiteral2.Value}{Quote}";
                }
                else if (expression.FirstExpression is NumericLiteral numericLiteral2) {
                    secondExpression = $"{numericLiteral2.Value}";
                }
                else if (expression.FirstExpression is NullLiteral nullLiteral2) {
                    secondExpression = $"NULL";
                }
                else if (expression.SecondExpression is VariableReference variableReference2) {
                    string variableName = FormatOptions.ReplaceQueryParametersWithValues ? GetQueryParameterFromList(variableReference2.Name) : variableReference2.Name;

                    secondExpression = $"{variableName}";

                    if (FormatOptions.UpdateQueryParametersList) {
                        InsertIntoQueryParametersList(variableReference2.Name);
                    }
                } 
                else if (expression.SecondExpression is FunctionCall functionCall2) {
                    secondExpression = FunctionCallParse(functionCall2);
                } 
                else {
                    secondExpression = "~UNKNOWN BooleanComparisonExpression~";
                }

                // comparison
                if (expression.ComparisonType == BooleanComparisonType.Equals) {
                    comparisonExpression = "=";
                }
                else if (expression.ComparisonType == BooleanComparisonType.GreaterThan) {
                    comparisonExpression = ">";
                }
                else if (expression.ComparisonType == BooleanComparisonType.GreaterThanOrEqualTo) {
                    comparisonExpression = ">=";
                }
                else if (expression.ComparisonType == BooleanComparisonType.LeftOuterJoin) {
                    comparisonExpression = "~UNKNOWN BooleanComparisonType~";
                }
                else if (expression.ComparisonType == BooleanComparisonType.LessThan) {
                    comparisonExpression = "<";
                }
                else if (expression.ComparisonType == BooleanComparisonType.LessThanOrEqualTo) {
                    comparisonExpression = "<=";
                }
                else if (expression.ComparisonType == BooleanComparisonType.NotEqualToBrackets) {
                    comparisonExpression = "<>";
                }
                else if (expression.ComparisonType == BooleanComparisonType.NotEqualToExclamation) {
                    comparisonExpression = "~UNKNOWN BooleanComparisonType~";
                }
                else if (expression.ComparisonType == BooleanComparisonType.NotGreaterThan) {
                    comparisonExpression = "<=";
                }
                else if (expression.ComparisonType == BooleanComparisonType.NotLessThan) {
                    comparisonExpression = ">=";
                }
                else if (expression.ComparisonType == BooleanComparisonType.RightOuterJoin) {
                    comparisonExpression = "~UNKNOWN BooleanComparisonType~";
                }
                else {
                    comparisonExpression = "~UNKNOWN BooleanComparisonType~";
                }

                result = $"{firstExpression} {comparisonExpression} {secondExpression}";
            }
            catch {
                result = "~BooleanComparisonExpression ERROR~";
            }

            return result;
        }

        public virtual string BooleanIsNullExpressionParse(BooleanIsNullExpression expression, object data = null) {
            string result;

            try {
                if (expression.Expression is ColumnReferenceExpression columnReferenceExpression) {
                    result = ColumnReferenceExpressionParse(columnReferenceExpression);
                }
                else if (expression.Expression is IntegerLiteral integerLiteral) {
                    result = integerLiteral.Value;
                }
                else if (expression.Expression is StringLiteral stringLiteral) {
                    result = $"{Quote}{stringLiteral.Value}{Quote}";
                }
                else if (expression.Expression is NumericLiteral numericLiteral) {
                    result = numericLiteral.Value;
                }
                else if (expression.Expression is NullLiteral nullLiteral) {
                    result = $"NULL";
                }
                else if (expression.Expression is VariableReference variableReference) {
                    string variableName = FormatOptions.ReplaceQueryParametersWithValues ? GetQueryParameterFromList(variableReference.Name) : variableReference.Name;

                    result = $"{variableName}";

                    if (FormatOptions.UpdateQueryParametersList) {
                        InsertIntoQueryParametersList(variableReference.Name);
                    }
                }
                else {
                    result = "~UNKNOWN BooleanIsNullExpression~";
                }

                result = expression.IsNot ? $"{result} IS NOT NULL" : $"{result} IS NULL";
            }
            catch {
                result = "~BooleanIsNullExpression ERROR~";
            }

            return result;
        }

        public virtual string BooleanNotExpressionParse(BooleanNotExpression expression, object data = null) {
            string result;

            try {
                if (expression.Expression is BooleanComparisonExpression booleanComparisonExpression) {
                    result = $"{BooleanComparisonExpressionParse(booleanComparisonExpression)}";
                }
                else if (expression.Expression is BooleanBinaryExpression booleanBinaryExpression) {
                    result = $"{BooleanBinaryExpressionParse(booleanBinaryExpression)}";
                }
                else if (expression.Expression is BooleanParenthesisExpression booleanParenthesisExpression) {
                    result = $"{BooleanParenthesisExpressionParse(booleanParenthesisExpression)}";
                }
                else if (expression.Expression is ExistsPredicate existsPredicate) {
                    result = $"{ExistsPredicateParse(existsPredicate)}";
                }
                else {
                    result = "~UNKNOWN BooleanExpression~";
                }
            }
            catch {
                result = "~BooleanNotExpression ERROR~";
            }

            return $"NOT {result}";
        }

        public virtual string BooleanParenthesisExpressionParse(BooleanParenthesisExpression expression, object data = null) {
            string result;

            try {
                if (expression.Expression is BooleanComparisonExpression booleanComparisonExpression) {
                    result = $"({BooleanComparisonExpressionParse(booleanComparisonExpression)})";
                }
                else if (expression.Expression is BooleanBinaryExpression booleanBinaryExpression) {
                    result = $"({BooleanBinaryExpressionParse(booleanBinaryExpression)})";
                }
                else if (expression.Expression is BooleanNotExpression booleanNotExpression) {
                    result = $"({BooleanNotExpressionParse(booleanNotExpression)})";
                }
                else if (expression.Expression is BooleanTernaryExpression booleanTernaryExpression) {
                    result = BooleanTernaryExpressionParse(booleanTernaryExpression);
                }
                else if (expression.Expression is BooleanIsNullExpression booleanIsNullExpression) {
                    result = $"({BooleanIsNullExpressionParse(booleanIsNullExpression)})";
                }
                else if (expression.Expression is BooleanParenthesisExpression booleanParenthesisExpression) {
                    result = $"({BooleanParenthesisExpressionParse(booleanParenthesisExpression)})";
                }
                else if (expression.Expression is InPredicate inPredicate) {
                    result = $"({InPredicateParse(inPredicate)})";
                } 
                else if (expression.Expression is LikePredicate likePredicate) {
                    result = $"({LikePredicateParse(likePredicate)})";
                } 
                else {
                    result = "~UNKNOWN BooleanExpression~";
                }
            }
            catch {
                result = "~BooleanParenthesisExpression ERROR~";
            }

            return result;
        }

        public virtual string BooleanTernaryExpressionParse(BooleanTernaryExpression expression, object data = null) {
            string result;
            try {
                string firstExpression;
                string secondExpression;
                string thirdExpression;
                string comparison;

                // first expression
                if (expression.FirstExpression is ColumnReferenceExpression columnReferenceExpression1) {
                    firstExpression = ColumnReferenceExpressionParse(columnReferenceExpression1);
                }
                else if (expression.FirstExpression is IntegerLiteral integerLiteral1) {
                    firstExpression = integerLiteral1.Value;
                }
                else if (expression.FirstExpression is StringLiteral stringLiteral1) {
                    firstExpression = stringLiteral1.Value;
                }
                else if (expression.FirstExpression is NumericLiteral numericLiteral1) {
                    firstExpression = numericLiteral1.Value;
                }
                else if (expression.FirstExpression is NullLiteral nullLiteral1) {
                    firstExpression = $"NULL";
                }
                else if (expression.FirstExpression is VariableReference variableReference1) {
                    string variableName = FormatOptions.ReplaceQueryParametersWithValues ? GetQueryParameterFromList(variableReference1.Name) : variableReference1.Name;

                    firstExpression = $"{variableName}";

                    if (FormatOptions.UpdateQueryParametersList) {
                        InsertIntoQueryParametersList(variableReference1.Name);
                    }
                } 
                else if (expression.FirstExpression is FunctionCall functionCall1) {
                    firstExpression = FunctionCallParse(functionCall1);
                } 
                else {
                    firstExpression = "~UNKNOWN ScalarExpression~";
                }

                // second expression
                if (expression.SecondExpression is ColumnReferenceExpression columnReferenceExpression2) {
                    secondExpression = ColumnReferenceExpressionParse(columnReferenceExpression2);
                }
                else if (expression.SecondExpression is IntegerLiteral integerLiteral2) {
                    secondExpression = integerLiteral2.Value;
                }
                else if (expression.SecondExpression is StringLiteral stringLiteral2) {
                    secondExpression = $"{Quote}{stringLiteral2.Value}{Quote}";
                }
                else if (expression.SecondExpression is NumericLiteral numericLiteral2) {
                    secondExpression = numericLiteral2.Value;
                }
                else if (expression.SecondExpression is NullLiteral nullLiteral2) {
                    secondExpression = $"NULL";
                }
                else if (expression.SecondExpression is VariableReference variableReference2) {
                    string variableName = FormatOptions.ReplaceQueryParametersWithValues ? GetQueryParameterFromList(variableReference2.Name) : variableReference2.Name;

                    secondExpression = $"{variableName}";

                    if (FormatOptions.UpdateQueryParametersList) {
                        InsertIntoQueryParametersList(variableReference2.Name);
                    }
                } 
                else if (expression.SecondExpression is FunctionCall functionCall2) {
                    secondExpression = FunctionCallParse(functionCall2);
                } 
                else {
                    secondExpression = "~UNKNOWN ScalarExpression~";
                }

                // third expression
                if (expression.ThirdExpression is ColumnReferenceExpression columnReferenceExpression3) {
                    thirdExpression = ColumnReferenceExpressionParse(columnReferenceExpression3);
                }
                else if (expression.ThirdExpression is IntegerLiteral integerLiteral3) {
                    thirdExpression = integerLiteral3.Value;
                }
                else if (expression.ThirdExpression is StringLiteral stringLiteral3) {
                    thirdExpression = $"{Quote}{stringLiteral3.Value}{Quote}";
                }
                else if (expression.ThirdExpression is NumericLiteral numericLiteral3) {
                    thirdExpression = numericLiteral3.Value;
                }
                else if (expression.ThirdExpression is NullLiteral nullLiteral3) {
                    thirdExpression = $"NULL";
                }
                else if (expression.ThirdExpression is VariableReference variableReference3) {
                    string variableName = FormatOptions.ReplaceQueryParametersWithValues ? GetQueryParameterFromList(variableReference3.Name) : variableReference3.Name;

                    thirdExpression = $"{variableName}";

                    if (FormatOptions.UpdateQueryParametersList) {
                        InsertIntoQueryParametersList(variableReference3.Name);
                    }
                } 
                else if (expression.ThirdExpression is FunctionCall functionCall3) {
                    thirdExpression = FunctionCallParse(functionCall3);
                } 
                else {
                    thirdExpression = "~UNKNOWN ScalarExpression~";
                }

                // comparison
                if (expression.TernaryExpressionType == BooleanTernaryExpressionType.Between) {
                    comparison = "BETWEEN";
                }
                else if (expression.TernaryExpressionType == BooleanTernaryExpressionType.NotBetween) {
                    comparison = "NOT BETWEEN";
                }
                else {
                    comparison = "~UNKNOWN BooleanTernaryExpressionType~";
                }

                result = $"{firstExpression} {comparison} {secondExpression} AND {thirdExpression}";
            }
            catch {
                result = "~BooleanTernaryExpression ERROR~";
            }

            return result;
        }

        public virtual string ColumnReferenceExpressionParse(ColumnReferenceExpression expression, object data = null) {
            string result;

            try {

                string collation = "";
                if (expression.Collation != null) {
                    collation = $" COLLATE {expression.Collation.Value} " ;
                }

                if (expression.ColumnType == ColumnType.Regular) {
                    MultiPartIdentifier identifier = expression.MultiPartIdentifier;

                    // single column name
                    if (identifier.Identifiers.Count == 1) {
                        result = $"{identifier.Identifiers[0].Value}{collation}";
                    }
                    // column name with table identifier
                    else if (identifier.Identifiers.Count == 2) {
                        result = $"{identifier.Identifiers[0].Value}.{identifier.Identifiers[1].Value}{collation}";
                    }
                    // column with domain identifier
                    else if (identifier.Identifiers.Count == 3) {
                        result = $"{identifier.Identifiers[0].Value}.{identifier.Identifiers[1].Value}.{identifier.Identifiers[2].Value}{collation}";
                    }
                    else {
                        result = "~UKNOWN ColumnReferenceExpression~";
                    }
                }
                else if (expression.ColumnType == ColumnType.Wildcard) {
                    result = "*";
                }
                else {
                    result = "~UKNOWN ColumnReferenceExpression~";
                }
            }
            catch {
                result = "~ColumnReferenceExpression ERROR~";
            }

            return result;
        }

        public virtual string InPredicateParse(InPredicate expression, object data = null) {
            string result;

            try {
                if (expression.Expression is ColumnReferenceExpression columnReferenceExpression1) {
                    result = ColumnReferenceExpressionParse(columnReferenceExpression1);
                }
                else if (expression.Expression is IntegerLiteral integerLiteral1) {
                    result = integerLiteral1.Value;
                }
                else if (expression.Expression is StringLiteral stringLiteral1) {
                    result = $"{Quote}{stringLiteral1.Value}{Quote}";
                }
                else if (expression.Expression is NumericLiteral numericLiteral1) {
                    result = numericLiteral1.Value;
                }
                else if (expression.Expression is NullLiteral nullLiteral1) {
                    result = $"NULL";
                }
                else if (expression.Expression is VariableReference variableReference1) {
                    string variableName = FormatOptions.ReplaceQueryParametersWithValues ? GetQueryParameterFromList(variableReference1.Name) : variableReference1.Name;

                    result = $"{variableName}";

                    if (FormatOptions.UpdateQueryParametersList) {
                        InsertIntoQueryParametersList(variableReference1.Name);
                    }
                }
                else {
                    result = "~UNKNOWN ScalarExpression~";
                }

                result += expression.NotDefined ? $" NOT IN (" : $" IN (";

                if (expression.Values.Count > 0) {
                    foreach (ScalarExpression value in expression.Values) {
                        if (result[result.Length - 1] != '(') {
                            result += ", ";
                        }

                        if (value is ColumnReferenceExpression columnReferenceExpression2) {
                            result += $"{ColumnReferenceExpressionParse(columnReferenceExpression2)}";
                        }
                        else if (value is StringLiteral stringLiteral) {
                            result += $"{Quote}{stringLiteral.Value}{Quote}";
                        }
                        else if (value is IntegerLiteral integerLiteral) {
                            result += integerLiteral.Value;
                        }
                        else if (value is NumericLiteral numericLiteral) {
                            result += numericLiteral.Value;
                        }
                        else if (value is NullLiteral nullLiteral) {
                            result += $"NULL";
                        }
                        else if (value is VariableReference variableReference) {
                            string variableName = FormatOptions.ReplaceQueryParametersWithValues ? GetQueryParameterFromList(variableReference.Name) : variableReference.Name;

                            result += $"{variableName}";

                            if (FormatOptions.UpdateQueryParametersList) {
                                InsertIntoQueryParametersList(variableReference.Name);
                            }
                        }
                        else {
                            result += "~UNKNOWN ScalarExpression~";
                        }
                    }
                }
                else if (expression.Subquery != null) {
                    this.Data.Level++;
                    result += QueryExpressionParse(expression.Subquery.QueryExpression);
                    this.Data.Level--;
                }

                result += ")";

            }
            catch {
                result = "~InPredicate ERROR~";
            }

            return result;
        }

        public virtual string LikePredicateParse(LikePredicate expression, object data = null) {
            string result;

            try {
                string firstExpression;
                string secondExpression;
                string comparison;

                if (expression.FirstExpression is ColumnReferenceExpression columnReferenceExpression1) {
                    firstExpression = ColumnReferenceExpressionParse(columnReferenceExpression1);
                }
                else if (expression.FirstExpression is BinaryExpression binaryExpression1) {
                    firstExpression = BinaryExpressionParse(binaryExpression1);
                }
                else if (expression.FirstExpression is StringLiteral stringLiteral1) {
                    firstExpression = $"{Quote}{stringLiteral1.Value}{Quote}";
                }
                else if (expression.FirstExpression is IntegerLiteral integerLiteral1) {
                    firstExpression = integerLiteral1.Value;
                }
                else if (expression.FirstExpression is NumericLiteral numericLiteral1) {
                    firstExpression = numericLiteral1.Value;
                }
                else if (expression.FirstExpression is NullLiteral nullLiteral1) {
                    firstExpression = $"NULL";
                }
                else if (expression.FirstExpression is VariableReference variableReference1) {
                    string variableName = FormatOptions.ReplaceQueryParametersWithValues ? GetQueryParameterFromList(variableReference1.Name) : variableReference1.Name;

                    firstExpression = $"{variableName}";

                    if (FormatOptions.UpdateQueryParametersList) {
                        InsertIntoQueryParametersList(variableReference1.Name);
                    }
                } 
                else if (expression.FirstExpression is FunctionCall functionCall1) {
                    firstExpression = FunctionCallParse(functionCall1);
                } 
                else {
                    firstExpression = "~UNKNOWN ScalarExpression~";
                }

                if (expression.SecondExpression is ColumnReferenceExpression columnReferenceExpression2) {
                    secondExpression = ColumnReferenceExpressionParse(columnReferenceExpression2);
                }
                else if (expression.SecondExpression is BinaryExpression binaryExpression2) {
                    secondExpression = BinaryExpressionParse(binaryExpression2);
                }
                else if (expression.SecondExpression is StringLiteral stringLiteral2) {
                    secondExpression = $"{Quote}{stringLiteral2.Value}{Quote}";
                }
                else if (expression.SecondExpression is IntegerLiteral integerLiteral2) {
                    secondExpression = integerLiteral2.Value;
                }
                else if (expression.SecondExpression is NumericLiteral numericLiteral2) {
                    secondExpression = numericLiteral2.Value;
                }
                else if (expression.SecondExpression is NullLiteral nullLiteral2) {
                    secondExpression = $"NULL";
                }
                else if (expression.SecondExpression is VariableReference variableReference2) {
                    string variableName = FormatOptions.ReplaceQueryParametersWithValues ? GetQueryParameterFromList(variableReference2.Name) : variableReference2.Name;

                    secondExpression = $"{variableName}";

                    if (FormatOptions.UpdateQueryParametersList) {
                        InsertIntoQueryParametersList(variableReference2.Name);
                    }
                } 
                else if (expression.SecondExpression is FunctionCall functionCall2) {
                    secondExpression = FunctionCallParse(functionCall2);
                } 
                else {
                    secondExpression = "~UNKNOWN ScalarExpression~";
                }

                comparison = expression.NotDefined ? $" NOT LIKE " : $" LIKE ";

                result = $"{firstExpression} {comparison} {secondExpression}";
            }
            catch {
                result = "~LikePredicate ERROR~";
            }

            return result;
        }

        public virtual string ExistsPredicateParse(ExistsPredicate expression, object data = null) {
            string result;

            try {
                if (expression.Subquery is ScalarSubquery scalarSubquery) {
                    this.Data.Level++;
                    string subQuery = QueryExpressionParse(scalarSubquery.QueryExpression);
                    this.Data.Level--;
                    result = $"EXISTS ({subQuery})";
                }
                else {
                    result = "~UNKNOWN ScalarSubquery~";
                }

            }
            catch {
                result = "~ExistsPredicate ERROR~";
            }

            return result;
        }

        public virtual string NamedTableReferenceParse(NamedTableReference namedTableReference, object data = null) {
            string result;

            try {
                result = namedTableReference.SchemaObject.BaseIdentifier.Value;
                string alias = namedTableReference.Alias?.Value;

                // check if the table has an alias
                if (alias != null) {
                    result += $" AS {alias}";
                }
            }
            catch {
                result = "~NamedTableReference ERROR~";
            }

            return result;
        }

        public virtual string QualifiedJoinParse(QualifiedJoin expression, object data = null) {
            string result;

            try {
                string firstTableReference;
                string secondTableReference;
                string searchCondition;
                string qualifiedJoinType;

                // first reference
                if (expression.FirstTableReference is NamedTableReference namedTableReference1) {
                    firstTableReference = NamedTableReferenceParse(namedTableReference1);
                }
                else if (expression.FirstTableReference is QueryDerivedTable queryDerivedTable1) {
                    Data.Level += 2;
                    firstTableReference = QueryExpressionParse(queryDerivedTable1.QueryExpression);
                    Data.Level -= 2;

                    firstTableReference = $"({firstTableReference})";

                    // check if the table has an alias
                    if (queryDerivedTable1.Alias != null) {
                        firstTableReference += $" AS {queryDerivedTable1.Alias?.Value}";
                    }
                }
                else if (expression.FirstTableReference is QualifiedJoin qualifiedJoin1) {
                    firstTableReference = QualifiedJoinParse(qualifiedJoin1);
                }
                else {
                    firstTableReference = $"~UNKNOWN TableReference~";
                }

                // second reference
                if (expression.SecondTableReference is NamedTableReference namedTableReference2) {
                    secondTableReference = NamedTableReferenceParse(namedTableReference2);
                }
                else if (expression.SecondTableReference is QueryDerivedTable queryDerivedTable2) {
                    Data.Level += 2;
                    secondTableReference = QueryExpressionParse(queryDerivedTable2.QueryExpression);
                    Data.Level -= 2;

                    secondTableReference = $"({secondTableReference})";

                    // check if the table has an alias
                    if (queryDerivedTable2.Alias != null) {
                        secondTableReference += $" AS {queryDerivedTable2.Alias?.Value}";
                    }
                }
                else if (expression.SecondTableReference is QualifiedJoin qualifiedJoin2) {
                    secondTableReference = QualifiedJoinParse(qualifiedJoin2);
                }
                else {
                    secondTableReference = $"~UNKNOWN TableReference~";
                }

                // search condition
                if (expression.SearchCondition is BooleanComparisonExpression booleanComparisonExpression) {
                    searchCondition = BooleanComparisonExpressionParse(booleanComparisonExpression);
                }
                else if (expression.SearchCondition is BooleanBinaryExpression booleanBinaryExpression) {
                    searchCondition = BooleanBinaryExpressionParse(booleanBinaryExpression);
                }
                else if (expression.SearchCondition is BooleanParenthesisExpression booleanParenthesisExpression) {
                    searchCondition = BooleanParenthesisExpressionParse(booleanParenthesisExpression);
                }
                else if (expression.SearchCondition is BooleanNotExpression booleanNotExpression) {
                    searchCondition = BooleanNotExpressionParse(booleanNotExpression);
                }
                else if (expression.SearchCondition is BooleanTernaryExpression booleanTernaryExpression) {
                    searchCondition = BooleanTernaryExpressionParse(booleanTernaryExpression);
                }
                else if (expression.SearchCondition is BooleanIsNullExpression booleanIsNullExpression) {
                    searchCondition = BooleanIsNullExpressionParse(booleanIsNullExpression);
                }
                else if (expression.SearchCondition is InPredicate inPredicate) {
                    searchCondition = InPredicateParse(inPredicate);
                }
                else if (expression.SearchCondition is LikePredicate likePredicate) {
                    searchCondition = LikePredicateParse(likePredicate);
                }
                else {
                    searchCondition = "~UNKNOWN BooleanExpression~";
                }

                // join type
                if (expression.QualifiedJoinType == QualifiedJoinType.FullOuter) {
                    qualifiedJoinType = "FULL OUTER JOIN";
                }
                else if (expression.QualifiedJoinType == QualifiedJoinType.Inner) {
                    qualifiedJoinType = "INNER JOIN";
                }
                else if (expression.QualifiedJoinType == QualifiedJoinType.LeftOuter) {
                    qualifiedJoinType = "LEFT OUTER JOIN";
                }
                else if (expression.QualifiedJoinType == QualifiedJoinType.RightOuter) {
                    qualifiedJoinType = "RIGHT OUTER JOIN";
                }
                else {
                    qualifiedJoinType = "~UNKNOWN QualifiedJoinType~";
                }

                result = $"{firstTableReference} \n{Indentation(Data.Level + 1)}{qualifiedJoinType} {secondTableReference} ON {searchCondition}";
            }
            catch {
                result = "~QualifiedJoin ERROR~";
            }

            return result;
        }

        public virtual string SelectStarExpression(SelectStarExpression selectStarExpression, object data = null) {
            string result;

            try {
                result = "*";

                // star expression with table identifier
                if (selectStarExpression.Qualifier != null) {
                    if (selectStarExpression.Qualifier.Identifiers.Count > 0) {
                        result = $"{selectStarExpression.Qualifier.Identifiers[0].Value}.{result}";
                    }
                }
            }
            catch {
                result = "~SelectStarExpression ERROR~";
            }

            return result;
        }

        public virtual string ExpressionGroupingSpecificationParse(ExpressionGroupingSpecification expression, object data = null) {
            string result;

            try {
                if (expression.Expression is ColumnReferenceExpression columnReferenceExpression) {
                    result = ColumnReferenceExpressionParse(columnReferenceExpression);
                }
                else {
                    result = "~UNKNOWN ScalarExpression~";
                }
            }
            catch {
                result = "~ExpressionGroupingSpecification ERROR~";
            }

            return result;
        }

        public virtual string SearchedCaseExpressionParse(SearchedCaseExpression expression, object data = null) {
            string result = $"CASE";

            try {
                this.Data.Level++;
                foreach (SearchedWhenClause whenClause in expression.WhenClauses) {
                    if (whenClause is SearchedWhenClause searchedWhenClause) {
                        result += $"\n{Indentation(Data.Level)}{SearchedWhenClauseParse(searchedWhenClause)} ";
                    }
                    else {
                        result += $"\n{Indentation(Data.Level)}~UNKNOWN SearchedWhenClause~";
                    }
                }

                if (expression.ElseExpression is StringLiteral stringLiteral) {
                    result += $"\n{Indentation(Data.Level)} ELSE {Quote}{stringLiteral.Value}{Quote}";
                }
                else if (expression.ElseExpression is IntegerLiteral integerLiteral) {
                    result += $"\n{Indentation(Data.Level)} ELSE {integerLiteral.Value}";
                }
                else if (expression.ElseExpression is NumericLiteral numericLiteral) {
                    result += $"\n{Indentation(Data.Level)} ELSE {numericLiteral.Value}";
                }
                else if (expression.ElseExpression is NullLiteral) {
                    result += $"\n{Indentation(Data.Level)} ELSE NULL";
                }
                else if (expression.ElseExpression is VariableReference variableReference) {
                    string variableName = FormatOptions.ReplaceQueryParametersWithValues ? GetQueryParameterFromList(variableReference.Name) : variableReference.Name;

                    result += $"\n{Indentation(Data.Level)} ELSE {variableName}";

                    if (FormatOptions.UpdateQueryParametersList) {
                        InsertIntoQueryParametersList(variableReference.Name);
                    }
                }
                else if (expression.ElseExpression is CastCall castCall) {
                    result += $"\n{Indentation(Data.Level)} ELSE {CastCallParse(castCall)}";
                } 
                else if (expression.ElseExpression is UnaryExpression unaryExpression) {
                    result += $"\n{Indentation(Data.Level)} ELSE {UnaryExpressionParse(unaryExpression)}";
                } 
                else if (expression.ElseExpression is ColumnReferenceExpression columnReferenceExpression) {
                    result += $"\n{Indentation(Data.Level)} ELSE {ColumnReferenceExpressionParse(columnReferenceExpression)}";
                } 
                else {
                    result += $"\n{Indentation(Data.Level)} ELSE ~UNKNOWN ScalarExpression~";
                }

                this.Data.Level--;

                result += $"\n{Indentation(Data.Level)}END";
            } 
            catch {
                result = "~SearchedCaseExpression ERROR~";
            }

            return result;
        }

        public virtual string UnaryExpressionParse(UnaryExpression expression, object data = null) {
            string result;

            try {
                string value;

                if (expression.Expression is StringLiteral stringLiteral) {
                    value = $"{Quote}{stringLiteral.Value}{Quote}";
                } 
                else if (expression.Expression is IntegerLiteral integerLiteral) {
                    value = $"{integerLiteral.Value}";
                } 
                else if (expression.Expression is NumericLiteral numericLiteral) {
                    value = $"{numericLiteral.Value}";
                } 
                else if (expression.Expression is NullLiteral nullLiteral) {
                    value = $"NULL";
                } 
                else if (expression.Expression is VariableReference variableReference) {
                    string variableName = FormatOptions.ReplaceQueryParametersWithValues ? GetQueryParameterFromList(variableReference.Name) : variableReference.Name;

                    value = $"{variableName}";

                    if (FormatOptions.UpdateQueryParametersList) {
                        InsertIntoQueryParametersList(variableReference.Name);
                    }
                } else {
                    value = $"~UNKNOWN Expression~";
                }

                if (expression.UnaryExpressionType == UnaryExpressionType.Negative) {
                    value = $"-{value}";
                }

                result = value;
            }
            catch {
                result = "~UnaryExpressionParse ERROR~";
            }

            return result;
        }

        public virtual string SearchedWhenClauseParse(SearchedWhenClause expression, object data = null) {
            string result = "WHEN ";

            try {
                if (expression.WhenExpression is BooleanComparisonExpression booleanComparisonExpression) {
                    result += BooleanComparisonExpressionParse(booleanComparisonExpression);
                }
                else if (expression.WhenExpression is BooleanBinaryExpression booleanBinaryExpression) {
                    result += BooleanBinaryExpressionParse(booleanBinaryExpression);
                }
                else if (expression.WhenExpression is BooleanParenthesisExpression booleanParenthesisExpression) {
                    result += BooleanParenthesisExpressionParse(booleanParenthesisExpression);
                }
                else if (expression.WhenExpression is BooleanNotExpression booleanNotExpression) {
                    result += BooleanNotExpressionParse(booleanNotExpression);
                }
                else if (expression.WhenExpression is BooleanTernaryExpression booleanTernaryExpression) {
                    result += BooleanTernaryExpressionParse(booleanTernaryExpression);
                }
                else if (expression.WhenExpression is BooleanIsNullExpression booleanIsNullExpression) {
                    result += BooleanIsNullExpressionParse(booleanIsNullExpression);
                }
                else if (expression.WhenExpression is InPredicate inPredicate) {
                    result += InPredicateParse(inPredicate);
                }
                else if (expression.WhenExpression is LikePredicate likePredicate) {
                    result += LikePredicateParse(likePredicate);
                }
                else {
                    result += $"~UNKNOWN BooleanExpression~";
                }

                if (expression.ThenExpression is ColumnReferenceExpression columnReferenceExpression) {
                    result += $" THEN {ColumnReferenceExpressionParse(columnReferenceExpression)}";
                } 
                else if (expression.ThenExpression is ParenthesisExpression parenthesisExpression) {
                    result += $" THEN {ParenthesisExpressionParse(parenthesisExpression)}";
                } 
                else if (expression.ThenExpression is StringLiteral stringLiteral) {
                    result += $" THEN {Quote}{stringLiteral.Value}{Quote}";
                }
                else if (expression.ThenExpression is IntegerLiteral integerLiteral) {
                    result += $" THEN {integerLiteral.Value}";
                }
                else if (expression.ThenExpression is NumericLiteral numericLiteral) {
                    result += $" THEN {numericLiteral.Value}";
                }
                else if (expression.ThenExpression is NullLiteral nullLiteral) {
                    result += $" THEN NULL";
                }
                else if (expression.ThenExpression is VariableReference variableReference) {
                    string variableName = FormatOptions.ReplaceQueryParametersWithValues ? GetQueryParameterFromList(variableReference.Name) : variableReference.Name;

                    result += $" THEN {variableName}";

                    if (FormatOptions.UpdateQueryParametersList) {
                        InsertIntoQueryParametersList(variableReference.Name);
                    }
                }
                else if (expression.ThenExpression is CastCall castCall) {
                    result += $" THEN {CastCallParse(castCall)}";
                } 
                else if (expression.ThenExpression is BinaryExpression binaryExpression) {
                    result += $" THEN {BinaryExpressionParse(binaryExpression)}";
                } 
                else {
                    result += $" THEN ~UNKNOWN ScalarExpression~";
                }
            }
            catch {
                result = "~SearchedWhenClause ERROR~";
            }

            return result;
        }

        public virtual string CastCallParse(CastCall expression, object data = null) {
            string result;

            try {
                string dataType;
                if (expression.DataType is SqlDataTypeReference) {
                    dataType = expression.DataType.Name.BaseIdentifier.Value;
                }
                else {
                    dataType = "~UNKNOWN DataType~";
                }

                string parameter;
                if (expression.Parameter is SearchedCaseExpression searchedCaseExpression) {
                    parameter = SearchedCaseExpressionParse(searchedCaseExpression);
                }
                else if (expression.Parameter is StringLiteral stringLiteral) {
                    parameter = $"{Quote}{stringLiteral.Value}{Quote}";
                }
                else if (expression.Parameter is IntegerLiteral integerLiteral) {
                    parameter = $"{integerLiteral.Value}";
                }
                else if (expression.Parameter is NumericLiteral numericLiteral) {
                    parameter = $"{numericLiteral.Value}";
                }
                else if (expression.Parameter is NullLiteral nullLiteral) {
                    parameter = $"NULL";
                }
                else if (expression.Parameter is VariableReference variableReference) {
                    string variableName = FormatOptions.ReplaceQueryParametersWithValues ? GetQueryParameterFromList(variableReference.Name) : variableReference.Name;

                    parameter = $"{variableName}";

                    if (FormatOptions.UpdateQueryParametersList) {
                        InsertIntoQueryParametersList(variableReference.Name);
                    }
                }
                else if (expression.Parameter is CastCall castCall) {
                    parameter = $" {CastCallParse(castCall)}";
                } 
                else if (expression.Parameter is UnaryExpression unaryExpression) {
                    parameter = $" {UnaryExpressionParse(unaryExpression)}";
                } 
                else if (expression.Parameter is ColumnReferenceExpression columnReferenceExpression) {
                    parameter = $" {ColumnReferenceExpressionParse(columnReferenceExpression)}";
                } 
                else {
                    parameter = "~UNKNOWN ScalarExpression~";
                }

                result = $"CAST({parameter} AS {dataType})";
            }
            catch {
                result = "~SearchedCaseExpression ERROR~";
            }

            return result;
        }

        public virtual string ExpressionWithSortOrderParse(ExpressionWithSortOrder expression, object data = null) {
            string result;

            try {
                if (expression.Expression is ColumnReferenceExpression columnReferenceExpression) {
                    result = ColumnReferenceExpressionParse(columnReferenceExpression);
                }
                else {
                    result = "~UKNOWN ScalarExpression~";
                }

                string sortOrder = "";
                if (expression.SortOrder == SortOrder.Ascending) {
                    sortOrder = "ASC";
                }
                else if (expression.SortOrder == SortOrder.Descending) {
                    sortOrder = "DESC";
                }
                else {
                    sortOrder = "";
                }

                result = $"{result} {sortOrder}";
            }
            catch {
                result = "~ExpressionWithSortOrder ERROR~";
            }

            return result;
        }

        public virtual string DataTypeParse(DataTypeReference expression, object data = null) {
            string result;

            try {
                if (expression is SqlDataTypeReference sqlDataTypeReference) {
                    result = sqlDataTypeReference.Name.BaseIdentifier.Value;
                    if (sqlDataTypeReference.Parameters.Count > 0) {
                        result += "(";
                        foreach (var parameter in sqlDataTypeReference.Parameters) {
                            if (!result.EndsWith("(")) {
                                result += ", ";
                            }

                            if (parameter is StringLiteral stringLiteral) {
                                result += $"{Quote}{stringLiteral.Value}{Quote}";
                            }
                            else if (parameter is IntegerLiteral integerLiteral) {
                                result += $"{integerLiteral.Value}";
                            }
                            else if (parameter is NumericLiteral numericLiteral) {
                                result += $"{numericLiteral.Value}";
                            }
                            else if (parameter is NullLiteral nullLiteral) {
                                result += $"NULL";
                            }
                            else {
                                result += "~UNKNOWN ScalarExpression~";
                            }
                        }
                        result += ")";
                    }
                }
                else if (expression is UserDataTypeReference userDataTypeReference) {
                    result = userDataTypeReference.Name.BaseIdentifier.Value;
                    if (userDataTypeReference.Parameters.Count > 0) {
                        result += "(";
                        foreach (var parameter in userDataTypeReference.Parameters) {
                            if (!result.EndsWith("(")) {
                                result += ", ";
                            }

                            if (parameter is StringLiteral stringLiteral) {
                                result += $"{Quote}{stringLiteral.Value}{Quote}";
                            }
                            else if (parameter is IntegerLiteral integerLiteral) {
                                result += $"{integerLiteral.Value}";
                            }
                            else if (parameter is NumericLiteral numericLiteral) {
                                result += $"{numericLiteral.Value}";
                            }
                            else if (parameter is NullLiteral nullLiteral) {
                                result += $"NULL";
                            }
                            else {
                                result += "~UNKNOWN ScalarExpression~";
                            }
                        }
                        result += ")";
                    }
                }
                else {
                    result = "~UNKNOWN DataTypeReference~";
                }
            }
            catch {
                result = "~DataTypeReference ERROR~";
            }

            return result;
        }

        public virtual string ConstraintDefinitionParse(ConstraintDefinition expression, object data = null) {
            string result = "";

            try {
                if (expression is NullableConstraintDefinition nullableConstraintDefinition) {
                    if (nullableConstraintDefinition.Nullable == false) {
                        result += "NOT NULL";

                    }
                }
                else if (expression is UniqueConstraintDefinition uniqueConstraintDefinition) {
                    result += "UNIQUE";
                }
                else {
                    result = "~UNKNOWN Constraint~";
                }
            }
            catch {
                result = "~ConstraintDefinition ERROR~";
            }

            return result;
        }

        public virtual string AlterTableAddTableElementStatementParse(AlterTableAddTableElementStatement expression, object data = null) {
            string result;

            try {
                List<string> columns = new List<string>();
                string table = expression.SchemaObjectName.BaseIdentifier.Value;

                foreach (ColumnDefinition column in expression.Definition.ColumnDefinitions) {
                    string columnName = ColumnDefinitionParse(column);
                    columns.Add(columnName);
                }

                result = $"{Indentation(Data.Level)}ALTER TABLE {table}";
                result += $"\n{Indentation(Data.Level)}ADD ";

                // columns
                if (columns.Count > 0) {
                    result += $"{columns[0]}";
                }
                for (int i = 1; i < columns.Count; i++) {
                    result += $", \n{Indentation(Data.Level + 1)}{columns[i]}";
                }

                result += "\n\n";
            }
            catch {
                result = "~AlterTableAddTableElementStatement ERROR~";
            }

            return result;
        }

        public virtual string ColumnDefinitionParse(ColumnDefinition expression, object data = null) {
            string result;

            try {
                string columnName = expression.ColumnIdentifier.Value;
                string dataType = DataTypeParse(expression.DataType);

                string constraints = "";
                foreach (ConstraintDefinition constraint in expression.Constraints) {
                    if (constraints != "") {
                        constraints += " ";
                    }
                    constraints += ConstraintDefinitionParse(constraint);
                }

                result = $"{columnName} {dataType} {constraints}";
            }
            catch {
                result = "~ColumnDefinition ERROR~";
            }

            return result;
        }

        public virtual string AlterTableAlterColumnStatementParse(AlterTableAlterColumnStatement expression, object data = null) {
            string result;

            try {
                List<string> columns = new List<string>();
                string table = expression.SchemaObjectName.BaseIdentifier.Value;
                string columnName = expression.ColumnIdentifier.Value;
                string dataType = DataTypeParse(expression.DataType);

                string options = "";
                if (expression.AlterTableAlterColumnOption == AlterTableAlterColumnOption.Null) {
                    options = "NULL";
                }
                else if (expression.AlterTableAlterColumnOption == AlterTableAlterColumnOption.NotNull) {
                    options = "NOT NULL";
                }
                else if (expression.AlterTableAlterColumnOption == AlterTableAlterColumnOption.AddHidden) {
                    options = "~UNDEVELOPED AlterTableAlterColumnOption~";
                }
                else if (expression.AlterTableAlterColumnOption == AlterTableAlterColumnOption.AddMaskingFunction) {
                    options = "~UNDEVELOPED AlterTableAlterColumnOption~";
                }
                else if (expression.AlterTableAlterColumnOption == AlterTableAlterColumnOption.AddNotForReplication) {
                    options = "~UNDEVELOPED AlterTableAlterColumnOption~";
                }
                else if (expression.AlterTableAlterColumnOption == AlterTableAlterColumnOption.AddPersisted) {
                    options = "~UNDEVELOPED AlterTableAlterColumnOption~";
                }
                else if (expression.AlterTableAlterColumnOption == AlterTableAlterColumnOption.AddRowGuidCol) {
                    options = "~UNDEVELOPED AlterTableAlterColumnOption~";
                }
                else if (expression.AlterTableAlterColumnOption == AlterTableAlterColumnOption.AddSparse) {
                    options = "~UNDEVELOPED AlterTableAlterColumnOption~";
                }
                else if (expression.AlterTableAlterColumnOption == AlterTableAlterColumnOption.DropHidden) {
                    options = "~UNDEVELOPED AlterTableAlterColumnOption~";
                }
                else if (expression.AlterTableAlterColumnOption == AlterTableAlterColumnOption.DropMaskingFunction) {
                    options = "~UNDEVELOPED AlterTableAlterColumnOption~";
                }
                else if (expression.AlterTableAlterColumnOption == AlterTableAlterColumnOption.DropNotForReplication) {
                    options = "~UNDEVELOPED AlterTableAlterColumnOption~";
                }
                else if (expression.AlterTableAlterColumnOption == AlterTableAlterColumnOption.DropPersisted) {
                    options = "~UNDEVELOPED AlterTableAlterColumnOption~";
                }
                else if (expression.AlterTableAlterColumnOption == AlterTableAlterColumnOption.DropRowGuidCol) {
                    options = "~UNDEVELOPED AlterTableAlterColumnOption~";
                }
                else if (expression.AlterTableAlterColumnOption == AlterTableAlterColumnOption.DropSparse) {
                    options = "~UNDEVELOPED AlterTableAlterColumnOption~";
                }
                else if (expression.AlterTableAlterColumnOption == AlterTableAlterColumnOption.Encryption) {
                    options = "~UNDEVELOPED AlterTableAlterColumnOption~";
                }
                else if (expression.AlterTableAlterColumnOption == AlterTableAlterColumnOption.NoOptionDefined) {
                    options = "";
                }
                else {
                    options = "~UNKNOWN AlterTableAlterColumnOption~";
                }

                result = $"{Indentation(Data.Level)}ALTER TABLE {table}";
                result += $"\n{Indentation(Data.Level)}ALTER COLUMN {columnName} {dataType} {options}";
                result += "\n\n";
            }
            catch {
                result = "~AlterTableAddTableElementStatement ERROR~";
            }

            return result;
        }

        public virtual string DropTableStatementParse(DropTableStatement expression, object data = null) {
            string result = "";

            try {
                List<string> tables = new List<string>();

                foreach (SchemaObjectName table in expression.Objects) {
                    string tableName = table.BaseIdentifier.Value;
                    tables.Add(tableName);
                }

                result += $"{Indentation(Data.Level)}DROP ";

                if (expression.IsIfExists) {
                    result += "IF EXISTS ";
                }

                if (tables.Count > 0) {
                    result += $"{tables[0]}";
                }
                for (int i = 1; i < tables.Count; i++) {
                    result += $", \n{Indentation(Data.Level + 1)}{tables[i]}";
                }

                result += "\n\n";
            } catch {
                result = "~DropTableStatement ERROR~";
            }

            return result;
        }

        public virtual string CoalesceExpressionParse(CoalesceExpression expression, object data = null) {
            string result = "Coalesce";

            try {
                string parameters = "(";

                foreach (var parameter in expression.Expressions) {
                    if (parameters != "(") {
                        parameters += ", ";
                    }

                    if (parameter is ColumnReferenceExpression columnReferenceExpression1) {
                        parameters += $"{ColumnReferenceExpressionParse(columnReferenceExpression1)}";
                    } 
                    else if (parameter is IntegerLiteral integerLiteral) {
                        parameters += $"{integerLiteral.Value}";
                    } 
                    else if (parameter is StringLiteral stringLiteral) {
                        parameters += $"{Quote}{stringLiteral.Value}{Quote}";
                    } 
                    else if (parameter is NumericLiteral numericLiteral) {
                        parameters = numericLiteral.Value;
                    } 
                    else if (parameter is NullLiteral nullLiteral) {
                        parameters += $"NULL";
                    } 
                    else if (parameter is VariableReference variableReference) {
                        string variableName = FormatOptions.ReplaceQueryParametersWithValues ? GetQueryParameterFromList(variableReference.Name) : variableReference.Name;

                        parameters += $"{variableName}";

                        if (FormatOptions.UpdateQueryParametersList) {
                            InsertIntoQueryParametersList(variableReference.Name);
                        }
                    } else {
                        parameters = "~UNKNOWN CoalesceParameter~";
                    }
                }

                result += $"{parameters})";
            } catch {
                result = "~CoalesceExpression ERROR~";
            }

            return result;
        }

        #endregion

    }

}
