using Microsoft.SqlServer.TransactSql.ScriptDom;
using SQLParser.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace SQLParser.Translators {
    public class SqlOMTranslator : ITranslator {

        /// <summary>
        /// RULES
        /// 1. Every Parse function returns as result the combined strings from all its sub-parsed functions that must be printed
        /// 2. Every Parse function returns -if it's able- a TermString that is the SqlOM Term or New that creates the object
        /// 3. Every Parse function returns -if it's able- a SqlExpressionString that is the SqlOM SqlExpression object for this expression
        /// </summary>


        #region VARIABLES AND NESTED CLASSES

        public virtual string SelectQueryText { get; set; } = "SelectQuery";
        public virtual string InsertQueryText { get; set; } = "InsertQuery";
        public virtual string UpdateQueryText { get; set; } = "UpdateQuery";
        public virtual string DeleteQueryText { get; set; } = "DeleteQuery";

        private List<ReservedWord> reservedWords = new List<ReservedWord>() {
            new ReservedWord("new", Colors.Blue, FontWeights.Normal),
            new ReservedWord("string", Colors.Blue, FontWeights.Normal),

            new ReservedWord("poSelectQuery", (Color) ColorConverter.ConvertFromString("#FF0A6C5E"), FontWeights.Normal),
            new ReservedWord("SelectQuery", (Color) ColorConverter.ConvertFromString("#FF0A6C5E"), FontWeights.Normal),
            new ReservedWord("poInsertQuery", (Color) ColorConverter.ConvertFromString("#FF0A6C5E"), FontWeights.Normal),
            new ReservedWord("InsertQuery", (Color) ColorConverter.ConvertFromString("#FF0A6C5E"), FontWeights.Normal),
            new ReservedWord("poUpdateQuery", (Color) ColorConverter.ConvertFromString("#FF0A6C5E"), FontWeights.Normal),
            new ReservedWord("UpdateQuery", (Color) ColorConverter.ConvertFromString("#FF0A6C5E"), FontWeights.Normal),
            new ReservedWord("poDeleteQuery", (Color) ColorConverter.ConvertFromString("#FF0A6C5E"), FontWeights.Normal),
            new ReservedWord("DeleteQuery", (Color) ColorConverter.ConvertFromString("#FF0A6C5E"), FontWeights.Normal),
            new ReservedWord("FromTerm", (Color) ColorConverter.ConvertFromString("#FF0A6C5E"), FontWeights.Normal),
            new ReservedWord("FromClause", (Color) ColorConverter.ConvertFromString("#FF0A6C5E"), FontWeights.Normal),
            new ReservedWord("WhereClause", (Color) ColorConverter.ConvertFromString("#FF0A6C5E"), FontWeights.Normal),
            new ReservedWord("WhereTerm", (Color) ColorConverter.ConvertFromString("#FF0A6C5E"), FontWeights.Normal),
            new ReservedWord("SqlExpression", (Color) ColorConverter.ConvertFromString("#FF0A6C5E"), FontWeights.Normal),
            new ReservedWord("SqlConstant", (Color) ColorConverter.ConvertFromString("#FF0A6C5E"), FontWeights.Normal),
            new ReservedWord("CompareOperator", (Color) ColorConverter.ConvertFromString("#FF0A6C5E"), FontWeights.Normal),
            new ReservedWord("SelectColumn", (Color) ColorConverter.ConvertFromString("#FF0A6C5E"), FontWeights.Normal),
            new ReservedWord("WhereClauseRelationship", (Color) ColorConverter.ConvertFromString("#FF0A6C5E"), FontWeights.Normal),
            new ReservedWord("JoinType", (Color) ColorConverter.ConvertFromString("#FF0A6C5E"), FontWeights.Normal),

            new ReservedWord("CreateCompare", (Color) ColorConverter.ConvertFromString("#FF723F0C"), FontWeights.Normal),
            new ReservedWord("CreateBetween", (Color) ColorConverter.ConvertFromString("#FF723F0C"), FontWeights.Normal),
            new ReservedWord("CreateNotBetween", (Color) ColorConverter.ConvertFromString("#FF723F0C"), FontWeights.Normal),
            new ReservedWord("CreateIsNotNull", (Color) ColorConverter.ConvertFromString("#FF723F0C"), FontWeights.Normal),
            new ReservedWord("CreateIsNull", (Color) ColorConverter.ConvertFromString("#FF723F0C"), FontWeights.Normal),
            new ReservedWord("CreateNotIn", (Color) ColorConverter.ConvertFromString("#FF723F0C"), FontWeights.Normal),
            new ReservedWord("CreateIn", (Color) ColorConverter.ConvertFromString("#FF723F0C"), FontWeights.Normal),
            new ReservedWord("CreateNotExists", (Color) ColorConverter.ConvertFromString("#FF723F0C"), FontWeights.Normal),
            new ReservedWord("CreateExists", (Color) ColorConverter.ConvertFromString("#FF723F0C"), FontWeights.Normal),
            new ReservedWord("Add", (Color) ColorConverter.ConvertFromString("#FF723F0C"), FontWeights.Normal),
            new ReservedWord("Join", (Color) ColorConverter.ConvertFromString("#FF723F0C"), FontWeights.Normal),
            new ReservedWord("Field", (Color) ColorConverter.ConvertFromString("#FF723F0C"), FontWeights.Normal),
            new ReservedWord("Constant", (Color) ColorConverter.ConvertFromString("#FF723F0C"), FontWeights.Normal),
            new ReservedWord("Number", (Color) ColorConverter.ConvertFromString("#FF723F0C"), FontWeights.Normal)
        };

        public class Informations {

            internal static int subqueryIndex = 0;
            internal static int tableIndex = 0;
            internal static int whereClauseIndex = 0;
            internal static int caseClauseIndex = 0;
            internal static bool unionIsCreated = false;

            public static int NextTableIndex() {
                return ++tableIndex;
            }
            public static int NextSubqueryIndex() {
                return ++subqueryIndex;
            }
            public static int NextWhereClauseIndex() {
                return ++whereClauseIndex;
            }
            public static int NextCaseClauseIndex() {
                return ++caseClauseIndex;
            }

            public int IndentationSize = 4;

            public Informations Parent = null; // the direct parent of the object
            public Informations BelongsTo = null;    // the expression parent of the parent. Keeps the variable name that belongs the object
            public int Level = 0; // how deep is the object in the tree
            public string VariableName = "";  // the variable name. It is used to create a new SqlOm object as SelectQuery or WhereClause etc and get a name
            public string FunctionName = "";  // used in columns when there is a function
            public string BaseTable = "";
            public bool Not = false;
            public string Alias = "";     //set an alias for column or table
            public string TableName = "";
            public string ColumnName = "";
            public string LeftTableName = "";
            public string LeftColumnName = "";
            public string RightTableName = "";
            public string RightColumnName = "";
            public string SqlExpressionString = "";
            public string TermString = "";

            /// <summary>
            /// create a new Informations object with some basic properties equals of another.
            /// Copies only the Parent, BelongsTo and Level
            /// </summary>
            /// <returns></returns>
            public Informations CopyLite() {
                Informations newInformations = new Informations();

                newInformations.Parent = this.Parent;
                newInformations.BelongsTo = this.BelongsTo;
                newInformations.Level = this.Level;
                newInformations.IndentationSize = this.IndentationSize;

                return newInformations;
            }
        }


        public TranslateOptions FormatOptions = new TranslateOptions();

        #endregion


        #region CONSTRUCTORS

        public SqlOMTranslator() {
            Reset();
        }

        public SqlOMTranslator(TranslateOptions formatOptions) {
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
            Informations.subqueryIndex = 0;
            Informations.tableIndex = 0;
            Informations.whereClauseIndex = 0;
            Informations.caseClauseIndex = 0;
            Informations.unionIsCreated = false;
        }

        public virtual FlowDocument GetFlowDocument(string text) {
            FlowDocument document = new FlowDocument();

            // create a flow document from the string
            using (StringReader reader = new StringReader(text)) {
                string newLine;
                bool isText = false;
                while ((newLine = reader.ReadLine()) != null) {
                    Paragraph paragraph = Format(newLine, ref isText);
                    document.Blocks.Add(paragraph);
                }
            }

            return document;
        }

        public virtual Paragraph Format(string text, ref bool isText) {
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

                    // check for text from previous line
                    if (isText) {
                        word.Text = current.Substring(0);

                        int nextPos = word.Text.IndexOf('\"');
                        if (nextPos > 0) {
                            word.Text = current.Substring(0, nextPos + 1);
                            isText = false;
                        }
                        else {
                            word.Text = current;
                            isText = true;
                        }

                        found = true;
                    }
                    // check for text
                    else if (current[0] == '\"') {
                        word.Text = current.Substring(1);

                        int nextPos = word.Text.IndexOf('\"');
                        if (nextPos > 0) {
                            word.Text = current.Substring(0, nextPos + 2);
                            isText = false;
                        }
                        else {
                            word.Text = current;
                            isText = true;
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

                            if (current.StartsWith(word.Text)) {

                                // check if the text is a word
                                if (IsWord(i, word.Text)) {
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

            // check if the current text is word by checking its bounding characters
            bool IsWord(int startPosition, string word) {
                if (startPosition != 0 && !char.IsWhiteSpace(text[startPosition - 1]) && char.IsLetterOrDigit(text[startPosition - 1]))
                    return false;

                if (startPosition + word.Length < length - 1 && !char.IsWhiteSpace(text[startPosition + +word.Length]) && char.IsLetterOrDigit(text[startPosition + +word.Length]))
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

        public virtual string InsertSpecificationParse(InsertSpecification expression, object data = null) {
            string result = "";
            Informations currentData = (Informations)data;
            if (currentData == null) {
                currentData = new Informations();
                currentData.Level = -1;
            }

            currentData.VariableName = $"query{Informations.NextSubqueryIndex()}";
            currentData.Level++;

            try {
                string target;

                //// TARGET /////

                TableReference table = expression.Target;
                if (table is NamedTableReference namedTableReference) {
                    Informations childData = currentData.CopyLite();

                    result += NamedTableReferenceParse(namedTableReference, childData);
                    target = childData.Alias;
                }
                else if (table is QueryDerivedTable queryDerivedTable) {
                    target = "~UNSUPPORTED QueryDerivedTable~";
                }
                else if (table is QualifiedJoin qualifiedJoin) {
                    target = "~UNSUPPORTED QualifiedJoin~";
                }
                else {
                    target = $"~UNKNOWN TableReference~";
                }

                result = $"{Indentation(currentData.Level)}{InsertQueryText} {currentData.VariableName} = new {InsertQueryText}(\"{target}\");\n";


                //// COLUMNS /////
                string columnName;
                List<string> columns = new List<string>();
                foreach (ColumnReferenceExpression column in expression.Columns) {
                    if (column is ColumnReferenceExpression columnReferenceExpression) {
                        Informations childData = currentData.CopyLite();

                        result += ColumnReferenceExpressionParse(columnReferenceExpression, childData);
                        columnName = childData.ColumnName;
                    }
                    else {
                        columnName = "~UNKNOWN ColumnReferenceExpression~";
                    }
                    columns.Add(columnName);
                }

                //// VALUES /////
                var source = expression.InsertSource as ValuesInsertSource;
                foreach (RowValue row in source.RowValues) {
                    string rowValue = "";

                    List<string> values = new List<string>();
                    foreach (ScalarExpression value in row.ColumnValues) {

                        if (value is IntegerLiteral integerLiteral) {
                            rowValue = $"SqlExpression.Number({integerLiteral.Value})";
                        }
                        else if (value is StringLiteral stringLiteral) {

                            // check if the string is date
                            if (DateTime.TryParse(stringLiteral.Value, out DateTime date2)) {
                                rowValue = $"SqlExpression.Date(\"{stringLiteral.Value}\")";
                            }
                            else {
                                rowValue = $"SqlExpression.String(\"{stringLiteral.Value}\")";
                            }
                        }
                        else if (value is NumericLiteral numericLiteral) {
                            rowValue = $"SqlExpression.Number({numericLiteral.Value})";
                        }
                        else if (value is NullLiteral nullLiteral1) {
                            rowValue = $"SqlExpression.Null()";
                        }
                        else if (value is VariableReference variableReference) {
                            rowValue = $"SqlExpression.Parameter(\"{variableReference.Name}\")";
                        }
                        else {
                            rowValue = $"~UNKNOWN ScalarExpression~";
                        }

                        values.Add(rowValue);
                    }

                    int index = 0;
                    foreach (string column in columns) {
                        if (values[index] != null) {
                            result += $"{Indentation(currentData.Level)}{currentData.VariableName}.Terms.Add(new UpdateTerm(\"{column}\", {values[index]}));\n";
                        }
                        index++;
                    }
                }
            }
            catch {
                result = "~InsertSpecification ERROR~";
            }
            finally {
                result += $"\n";
            }

            return result;
        }

        public virtual string UpdateSpecificationParse(UpdateSpecification expression, object data = null) {
            string result = "";
            Informations currentData = (Informations)data;
            if (currentData == null) {
                currentData = new Informations();
                currentData.Level = -1;
            }

            try {
                string target;
                string columnName;
                string setValue;
                List<string> columns = new List<string>();
                List<string> whereConditions = new List<string>();

                currentData.VariableName = $"query{Informations.NextSubqueryIndex()}";
                currentData.Level++;


                //// TARGET /////

                TableReference table = expression.Target;
                if (table is NamedTableReference namedTableReference) {
                    Informations childData = currentData.CopyLite();

                    result += NamedTableReferenceParse(namedTableReference, childData);
                    target = childData.Alias;
                }
                else if (table is QueryDerivedTable queryDerivedTable) {
                    target = "~UNSUPPORTED QueryDerivedTable~";
                }
                else if (table is QualifiedJoin qualifiedJoin) {
                    target = "~UNSUPPORTED QualifiedJoin~";
                }
                else {
                    target = $"~UNKNOWN TableReference~";
                }

                result = $"{Indentation(currentData.Level)}{UpdateQueryText} {currentData.VariableName} = new {UpdateQueryText}(\"{target}\");\n";


                //// VALUES /////
                foreach (AssignmentSetClause assignmentSetClause in expression.SetClauses) {
                    if (assignmentSetClause.Column is ColumnReferenceExpression columnReferenceExpression) {
                        Informations childData = currentData.CopyLite();

                        result += ColumnReferenceExpressionParse(columnReferenceExpression, childData);
                        columnName = childData.ColumnName;
                    }
                    else {
                        columnName = "~UNKNOWN ColumnReferenceExpression~";
                    }

                    if (assignmentSetClause.NewValue is ColumnReferenceExpression columnReferenceExpression2) {
                        Informations childData = currentData.CopyLite();

                        result += ColumnReferenceExpressionParse(columnReferenceExpression2, childData);
                        setValue = childData.SqlExpressionString;
                    }
                    else if (assignmentSetClause.NewValue is IntegerLiteral integerLiteral) {
                        setValue = $"SqlExpression.Number({integerLiteral.Value})";
                    }
                    else if (assignmentSetClause.NewValue is StringLiteral stringLiteral) {

                        // check if the string is date
                        if (DateTime.TryParse(stringLiteral.Value, out DateTime date2)) {
                            setValue = $"SqlExpression.Date(\"{stringLiteral.Value}\")";
                        }
                        else {
                            setValue = $"SqlExpression.String(\"{stringLiteral.Value}\")";
                        }
                    }
                    else if (assignmentSetClause.NewValue is NumericLiteral numericLiteral) {
                        setValue = $"SqlExpression.Number({numericLiteral.Value})";
                    }
                    else if (assignmentSetClause.NewValue is NullLiteral nullLiteral1) {
                        setValue = $"SqlExpression.Null()";
                    }
                    else if (assignmentSetClause.NewValue is VariableReference variableReference) {
                        setValue = $"SqlExpression.Parameter(\"{variableReference.Name}\")";
                    }
                    else {
                        setValue = $"~UNKNOWN ScalarExpression~";
                    }

                    result += $"{Indentation(currentData.Level)}{currentData.VariableName}.Terms.Add(new UpdateTerm(\"{columnName}\", {setValue}));\n";

                }


                //// WHERE //////

                if (expression.WhereClause != null) {
                    BooleanExpression condition = expression.WhereClause.SearchCondition;

                    if (condition is BooleanComparisonExpression booleanComparisonExpression) {
                        Informations childData = currentData.CopyLite();
                        childData.BelongsTo = currentData;

                        result += BooleanComparisonExpressionParse(booleanComparisonExpression, childData);
                        result += $"{Indentation(currentData.Level)}{currentData.VariableName}.WhereClause.Terms.Add({childData.TermString});\n";
                    }
                    else if (condition is BooleanBinaryExpression booleanBinaryExpression) {
                        Informations childData = currentData.CopyLite();
                        childData.BelongsTo = currentData;

                        result += BooleanBinaryExpressionParse(booleanBinaryExpression, childData);
                        result += $"{Indentation(currentData.Level)}{currentData.VariableName}.WhereClause.SubClauses.Add({childData.VariableName});\n";
                    }
                    else if (condition is BooleanParenthesisExpression booleanParenthesisExpression) {
                        Informations childData = currentData.CopyLite();
                        childData.BelongsTo = currentData;

                        result += BooleanParenthesisExpressionParse(booleanParenthesisExpression, childData);

                        if (childData.VariableName != "") {
                            result += $"{Indentation(currentData.Level)}{currentData.VariableName}.WhereClause.SubClauses.Add({childData.VariableName});\n";
                        }
                        else {
                            result += $"{Indentation(currentData.Level)}{currentData.VariableName}.WhereClause.Terms.Add({childData.TermString});\n";
                        }
                    }
                    else if (condition is BooleanNotExpression booleanNotExpression) {
                        Informations childData = currentData.CopyLite();
                        childData.BelongsTo = currentData;

                        result += BooleanNotExpressionParse(booleanNotExpression, childData);
                        result += $"{Indentation(currentData.Level)}{currentData.VariableName}.WhereClause.Terms.Add({childData.TermString});\n";
                    }
                    else if (condition is BooleanTernaryExpression booleanTernaryExpression) {
                        Informations childData = currentData.CopyLite();
                        childData.BelongsTo = currentData;

                        result += BooleanTernaryExpressionParse(booleanTernaryExpression, childData);
                        result += $"{Indentation(currentData.Level)}{currentData.VariableName}.WhereClause.Terms.Add({childData.TermString});\n";
                    }
                    else if (condition is BooleanIsNullExpression booleanIsNullExpression) {
                        Informations childData = currentData.CopyLite();
                        childData.BelongsTo = currentData;

                        result += BooleanIsNullExpressionParse(booleanIsNullExpression, childData);
                        result += $"{Indentation(currentData.Level)}{currentData.VariableName}.WhereClause.Terms.Add({childData.TermString});\n";
                    }
                    else if (condition is InPredicate inPredicate) {
                        Informations childData = currentData.CopyLite();
                        childData.BelongsTo = currentData;

                        result += InPredicateParse(inPredicate, childData);
                        result += $"{Indentation(currentData.Level)}{currentData.VariableName}.WhereClause.Terms.Add({childData.TermString});\n";
                    }
                    else if (condition is LikePredicate likePredicate) {
                        Informations childData = currentData.CopyLite();
                        childData.BelongsTo = currentData;

                        result += LikePredicateParse(likePredicate, childData);
                        result += $"{Indentation(currentData.Level)}{currentData.VariableName}.WhereClause.Terms.Add({childData.TermString});\n";
                    }
                    else if (condition is ExistsPredicate existsPredicate) {
                        Informations childData = currentData.CopyLite();
                        childData.BelongsTo = currentData;

                        result += ExistsPredicateParse(existsPredicate, childData);
                        result += $"{Indentation(currentData.Level)}{currentData.VariableName}.WhereClause.Terms.Add({childData.TermString});\n";
                    }
                    else {
                        result += "~UNKNOWN BooleanExpression~";
                    }
                }

            }
            catch {
                result = "~UpdateSpecification ERROR~";
            }
            finally {
                result += $"\n";
            }

            return result;
        }

        public virtual string DeleteSpecificationParse(DeleteSpecification expression, object data = null) {
            string result = "";
            Informations currentData = (Informations)data;
            if (currentData == null) {
                currentData = new Informations();
                currentData.Level = -1;
            }

            try {
                string target;
                List<string> whereConditions = new List<string>();

                currentData.VariableName = $"query{Informations.NextSubqueryIndex()}";
                currentData.Level++;


                //// TARGET /////

                TableReference table = expression.Target;
                if (table is NamedTableReference namedTableReference) {
                    Informations childData = currentData.CopyLite();

                    result += NamedTableReferenceParse(namedTableReference, childData);
                    target = childData.Alias;
                }
                else if (table is QueryDerivedTable queryDerivedTable) {
                    target = "~UNSUPPORTED QueryDerivedTable~";
                }
                else if (table is QualifiedJoin qualifiedJoin) {
                    target = "~UNSUPPORTED QualifiedJoin~";
                }
                else {
                    target = $"~UNKNOWN TableReference~";
                }

                result = $"{Indentation(currentData.Level)}{DeleteQueryText} {currentData.VariableName} = new {DeleteQueryText}(\"{target}\");\n";


                //// WHERE //////

                if (expression.WhereClause != null) {
                    BooleanExpression condition = expression.WhereClause.SearchCondition;

                    if (condition is BooleanComparisonExpression booleanComparisonExpression) {
                        Informations childData = currentData.CopyLite();
                        childData.BelongsTo = currentData;

                        result += BooleanComparisonExpressionParse(booleanComparisonExpression, childData);
                        result += $"{Indentation(currentData.Level)}{currentData.VariableName}.WhereClause.Terms.Add({childData.TermString});\n";
                    }
                    else if (condition is BooleanBinaryExpression booleanBinaryExpression) {
                        Informations childData = currentData.CopyLite();
                        childData.BelongsTo = currentData;

                        result += BooleanBinaryExpressionParse(booleanBinaryExpression, childData);
                        result += $"{Indentation(currentData.Level)}{currentData.VariableName}.WhereClause.SubClauses.Add({childData.VariableName});\n";
                    }
                    else if (condition is BooleanParenthesisExpression booleanParenthesisExpression) {
                        Informations childData = currentData.CopyLite();
                        childData.BelongsTo = currentData;

                        result += BooleanParenthesisExpressionParse(booleanParenthesisExpression, childData);

                        if (childData.VariableName != "") {
                            result += $"{Indentation(currentData.Level)}{currentData.VariableName}.WhereClause.SubClauses.Add({childData.VariableName});\n";
                        }
                        else {
                            result += $"{Indentation(currentData.Level)}{currentData.VariableName}.WhereClause.Terms.Add({childData.TermString});\n";
                        }
                    }
                    else if (condition is BooleanNotExpression booleanNotExpression) {
                        Informations childData = currentData.CopyLite();
                        childData.BelongsTo = currentData;

                        result += BooleanNotExpressionParse(booleanNotExpression, childData);
                        result += $"{Indentation(currentData.Level)}{currentData.VariableName}.WhereClause.Terms.Add({childData.TermString});\n";
                    }
                    else if (condition is BooleanTernaryExpression booleanTernaryExpression) {
                        Informations childData = currentData.CopyLite();
                        childData.BelongsTo = currentData;

                        result += BooleanTernaryExpressionParse(booleanTernaryExpression, childData);
                        result += $"{Indentation(currentData.Level)}{currentData.VariableName}.WhereClause.Terms.Add({childData.TermString});\n";
                    }
                    else if (condition is BooleanIsNullExpression booleanIsNullExpression) {
                        Informations childData = currentData.CopyLite();
                        childData.BelongsTo = currentData;

                        result += BooleanIsNullExpressionParse(booleanIsNullExpression, childData);
                        result += $"{Indentation(currentData.Level)}{currentData.VariableName}.WhereClause.Terms.Add({childData.TermString});\n";
                    }
                    else if (condition is InPredicate inPredicate) {
                        Informations childData = currentData.CopyLite();
                        childData.BelongsTo = currentData;

                        result += InPredicateParse(inPredicate, childData);
                        result += $"{Indentation(currentData.Level)}{currentData.VariableName}.WhereClause.Terms.Add({childData.TermString});\n";
                    }
                    else if (condition is LikePredicate likePredicate) {
                        Informations childData = currentData.CopyLite();
                        childData.BelongsTo = currentData;

                        result += LikePredicateParse(likePredicate, childData);
                        result += $"{Indentation(currentData.Level)}{currentData.VariableName}.WhereClause.Terms.Add({childData.TermString});\n";
                    }
                    else if (condition is ExistsPredicate existsPredicate) {
                        Informations childData = currentData.CopyLite();
                        childData.BelongsTo = currentData;

                        result += ExistsPredicateParse(existsPredicate, childData);
                        result += $"{Indentation(currentData.Level)}{currentData.VariableName}.WhereClause.Terms.Add({childData.TermString});\n";
                    }
                    else {
                        result += "~UNKNOWN BooleanExpression~";
                    }
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
            Informations currentData = (Informations)data;

            try {
                currentData.TermString = "~SqlOM doesn't supports create table~";
            }
            catch {
                currentData.TermString = "~CreateTableStatement ERROR~";
            }

            return result;
        }

        public string CreateViewStatementParse(CreateViewStatement expression, object data = null) {
            string result = "";
            Informations currentData = (Informations)data;

            try {
                currentData.TermString = "~UNDEVELOPED CreateViewStatement~";
            }
            catch {
                currentData.TermString = "~CreateViewStatement ERROR~";
            }

            return result;
        }

        public virtual string SelectStatementParse(SelectStatement expression, object data = null) {
            string result = "";

            try {
                Informations parentInformations = new Informations();

                Informations childData = new Informations() {
                    Parent = parentInformations,
                    BelongsTo = parentInformations,
                    Level = -1
                };

                result = QueryExpressionParse(expression.QueryExpression, childData);
            }
            catch {
                result = "~SelectStatement ERROR~";
            }
            finally {
                result += $"\n";
            }

            return result;
        }

        public virtual string QueryExpressionParse(QueryExpression expression, object data = null) {
            string result = "";
            Informations currentData = (Informations)data;

            try {

                if (expression is QuerySpecification querySpecification) {
                    Informations childData = currentData.CopyLite();

                    result += QuerySpecificationParse(querySpecification, childData);

                    currentData.TermString = childData.TermString;
                    currentData.SqlExpressionString = childData.SqlExpressionString;
                    currentData.VariableName = childData.VariableName;
                }
                else if (expression is QueryParenthesisExpression queryParenthesisExpression) {
                    Informations childData = currentData.CopyLite();

                    result += QueryParenthesisExpressionParse(queryParenthesisExpression, childData);
                }
                else if (expression is BinaryQueryExpression binaryQueryExpression) {
                    Informations childData = currentData.CopyLite();

                    // SqlOM supports only one level for unions
                    if (!Informations.unionIsCreated) {
                        Informations.unionIsCreated = true;

                        result += $"{Indentation(currentData.Level)}SqlUnion union = new SqlUnion();\n";
                        result += BinaryQueryExpressionParse(binaryQueryExpression, childData);
                    }
                    else {
                        result = "~SqlOM doesn support nested unions~\n";
                    }
                }
                else {
                    result = "~UNKNOWN QueryExpression~\n";
                }
            }
            catch {
                result = "~QueryExpression ERROR~\n";
            }

            return result;
        }

        public virtual string BinaryQueryExpressionParse(BinaryQueryExpression expression, object data = null) {
            string result = "";
            Informations currentData = (Informations)data;

            try {
                if (expression.BinaryQueryExpressionType == BinaryQueryExpressionType.Union) {
                    string modifier;

                    if (expression.All == true) {
                        modifier = "DistinctModifier.All";
                    }
                    else {
                        modifier = "DistinctModifier.Distinct";
                    }

                    if (expression.FirstQueryExpression is QuerySpecification querySpecification1) {
                        Informations childData = currentData.CopyLite();
                        childData.Level++;

                        result += QuerySpecificationParse(querySpecification1, childData);
                        result += $"{Indentation(currentData.Level)}union.Add({childData.VariableName}, {modifier});\n";
                    }
                    else if (expression.FirstQueryExpression is QueryParenthesisExpression queryParenthesisExpression1) {
                        Informations childData = currentData.CopyLite();
                        childData.Level++;

                        result += QueryParenthesisExpressionParse(queryParenthesisExpression1, childData);
                        result += $"{Indentation(currentData.Level)}union.Add({childData.VariableName}, {modifier});\n";

                    }
                    else if (expression.FirstQueryExpression is BinaryQueryExpression binaryQueryExpression1) {
                        Informations childData = currentData.CopyLite();

                        result += BinaryQueryExpressionParse(binaryQueryExpression1, childData);
                    }
                    else {
                        result += $"{Indentation(currentData.Level)}union.Add(~UNKNOWN QueryExpression~, {modifier});\n";
                    }


                    if (expression.SecondQueryExpression is QuerySpecification querySpecification2) {
                        Informations childData = currentData.CopyLite();
                        childData.Level++;

                        result += QuerySpecificationParse(querySpecification2, childData);
                        result += $"{Indentation(currentData.Level)}union.Add({childData.VariableName}, {modifier});\n";
                    }
                    else if (expression.SecondQueryExpression is QueryParenthesisExpression queryParenthesisExpression2) {
                        Informations childData = currentData.CopyLite();
                        childData.Level++;

                        result += QueryParenthesisExpressionParse(queryParenthesisExpression2, childData);
                        result += $"{Indentation(currentData.Level)}union.Add({childData.VariableName}, {modifier});\n";
                    }
                    else if (expression.SecondQueryExpression is BinaryQueryExpression binaryQueryExpression2) {
                        result += $"{Indentation(currentData.Level)}union.Add(~SqlOM doesn support nested unions~, {modifier});\n";
                    }
                    else {
                        result += $"{Indentation(currentData.Level)}union.Add(~UNKNOWN QueryExpression~, {modifier});\n";
                    }

                }
                else if (expression.BinaryQueryExpressionType == BinaryQueryExpressionType.Except) {
                    result = "~SqlOM doesn support Except~";
                }
                else if (expression.BinaryQueryExpressionType == BinaryQueryExpressionType.Intersect) {
                    result = "~SqlOM doesn support Intersect~";
                }
                else {
                    result = "~UNKNOWN BinaryQueryExpressionType~";
                }
            }
            catch {
                result = "~BinaryQueryExpression ERROR~";
            }

            return result;
        }

        public virtual string QuerySpecificationParse(QuerySpecification querySpecification, object data = null) {
            string result;
            Informations currentData = (Informations)data;
            currentData.VariableName = $"query{Informations.NextSubqueryIndex()}";
            currentData.Level++;

            try {
                bool isDistinct;
                bool isTopSelection;
                bool isTopPercent;
                int isTopCount;

                result = $"{Indentation(currentData.Level)}{SelectQueryText} {currentData.VariableName} = new {SelectQueryText}();\n";

                //// VARIOUS /////

                isDistinct = querySpecification.UniqueRowFilter.ToString().ToUpper().Equals("DISTINCT");
                isTopSelection = querySpecification.TopRowFilter != null;
                isTopPercent = isTopSelection ? querySpecification.TopRowFilter.Percent : false;
                isTopCount = (querySpecification.TopRowFilter?.Expression is IntegerLiteral) ? int.Parse((querySpecification.TopRowFilter.Expression as IntegerLiteral).Value) : 0;

                result += isDistinct ? $"{Indentation(currentData.Level)}{currentData.VariableName}.Distinct = true;\n" : "";
                result += isTopSelection ? $"{Indentation(currentData.Level)}{currentData.VariableName}.Top = {isTopCount};\n" : "";


                //// TABLES //////

                IList<TableReference> tableReferences = querySpecification.FromClause?.TableReferences;
                if (tableReferences != null) {
                    foreach (TableReference table in tableReferences) {
                        if (table is NamedTableReference namedTableReference) {

                            Informations childData = currentData.CopyLite();
                            childData.BelongsTo = currentData;

                            result += NamedTableReferenceParse(namedTableReference, childData);
                            result += $"{Indentation(currentData.Level)}{childData.TermString};\n";

                            // BaseName
                            if (currentData.BaseTable == "") {
                                currentData.BaseTable = childData.Alias;
                                result += $"{Indentation(currentData.Level)}{currentData.VariableName}.FromClause.BaseTable = {currentData.BaseTable};\n";
                            }
                        }
                        else if (table is QueryDerivedTable queryDerivedTable) {
                            string alias = queryDerivedTable.Alias != null ? queryDerivedTable.Alias.Value : $"table{Informations.NextTableIndex()}";

                            Informations childData = currentData.CopyLite();
                            childData.BelongsTo = currentData;

                            result += QueryExpressionParse(queryDerivedTable.QueryExpression, childData);

                            result += $"{Indentation(currentData.Level)}FromTerm {alias} = FromTerm.SubQuery({childData.VariableName}, \"{alias}\");\n";

                            // BaseName
                            if (currentData.BaseTable == "") {
                                currentData.BaseTable = alias;
                                result += $"{Indentation(currentData.Level)}{currentData.VariableName}.FromClause.BaseTable = {alias};\n";
                            }
                        }
                        else if (table is QualifiedJoin qualifiedJoin) {

                            Informations childData = currentData.CopyLite();
                            childData.BelongsTo = currentData;

                            result += QualifiedJoinParse(qualifiedJoin, childData);

                            // BaseName
                            if (currentData.BaseTable == "") {
                                currentData.BaseTable = childData.LeftTableName;
                                result += $"{Indentation(currentData.Level)}{currentData.VariableName}.FromClause.BaseTable = {currentData.BaseTable};\n";
                            }
                        }
                        else {
                            result += $"{Indentation(currentData.Level)}FromTerm ~UNKNOWN TableReference~;\n";
                        }
                    }
                }


                //// WHERE //////

                if (querySpecification.WhereClause != null) {
                    BooleanExpression condition = querySpecification.WhereClause.SearchCondition;

                    if (condition is BooleanComparisonExpression booleanComparisonExpression) {
                        Informations childData = currentData.CopyLite();
                        childData.BelongsTo = currentData;

                        result += BooleanComparisonExpressionParse(booleanComparisonExpression, childData);
                        result += $"{Indentation(currentData.Level)}{currentData.VariableName}.WherePhrase.Terms.Add({childData.TermString});\n";
                    }
                    else if (condition is BooleanBinaryExpression booleanBinaryExpression) {
                        Informations childData = currentData.CopyLite();
                        childData.BelongsTo = currentData;

                        result += BooleanBinaryExpressionParse(booleanBinaryExpression, childData);
                        result += $"{Indentation(currentData.Level)}{currentData.VariableName}.WherePhrase.SubClauses.Add({childData.VariableName});\n";
                    }
                    else if (condition is BooleanParenthesisExpression booleanParenthesisExpression) {
                        Informations childData = currentData.CopyLite();
                        childData.BelongsTo = currentData;

                        result += BooleanParenthesisExpressionParse(booleanParenthesisExpression, childData);

                        if (childData.VariableName != "") {
                            //result += temp;
                            result += $"{Indentation(currentData.Level)}{currentData.VariableName}.WherePhrase.SubClauses.Add({childData.VariableName});\n";
                        }
                        else {
                            result += $"{Indentation(currentData.Level)}{currentData.VariableName}.WherePhrase.Terms.Add({childData.TermString});\n";
                        }
                    }
                    else if (condition is BooleanNotExpression booleanNotExpression) {
                        Informations childData = currentData.CopyLite();
                        childData.BelongsTo = currentData;

                        result += BooleanNotExpressionParse(booleanNotExpression, childData);
                        result += $"{Indentation(currentData.Level)}{currentData.VariableName}.WherePhrase.Terms.Add({childData.TermString});\n";
                    }
                    else if (condition is BooleanTernaryExpression booleanTernaryExpression) {
                        Informations childData = currentData.CopyLite();
                        childData.BelongsTo = currentData;

                        result += BooleanTernaryExpressionParse(booleanTernaryExpression, childData);
                        result += $"{Indentation(currentData.Level)}{currentData.VariableName}.WherePhrase.Terms.Add({childData.TermString});\n";
                    }
                    else if (condition is BooleanIsNullExpression booleanIsNullExpression) {
                        Informations childData = currentData.CopyLite();
                        childData.BelongsTo = currentData;

                        result += BooleanIsNullExpressionParse(booleanIsNullExpression, childData);
                        result += $"{Indentation(currentData.Level)}{currentData.VariableName}.WherePhrase.Terms.Add({childData.TermString});\n";
                    }
                    else if (condition is InPredicate inPredicate) {
                        Informations childData = currentData.CopyLite();
                        childData.BelongsTo = currentData;

                        result += InPredicateParse(inPredicate, childData);
                        result += $"{Indentation(currentData.Level)}{currentData.VariableName}.WherePhrase.Terms.Add({childData.TermString});\n";
                    }
                    else if (condition is LikePredicate likePredicate) {
                        Informations childData = currentData.CopyLite();
                        childData.BelongsTo = currentData;

                        result += LikePredicateParse(likePredicate, childData);
                        result += $"{Indentation(currentData.Level)}{currentData.VariableName}.WherePhrase.Terms.Add({childData.TermString});\n";
                    }
                    else if (condition is ExistsPredicate existsPredicate) {
                        Informations childData = currentData.CopyLite();
                        childData.BelongsTo = currentData;

                        result += ExistsPredicateParse(existsPredicate, childData);
                        result += $"{Indentation(currentData.Level)}{currentData.VariableName}.WherePhrase.Terms.Add({childData.TermString});\n";
                    }
                    else {
                        result += "~UNKNOWN BooleanExpression~";
                    }
                }


                //// GROUPED //////

                if (querySpecification.GroupByClause != null) {
                    foreach (GroupingSpecification condition in querySpecification.GroupByClause.GroupingSpecifications) {
                        if (condition is ExpressionGroupingSpecification expressionGroupingSpecification) {
                            Informations childData = currentData.CopyLite();
                            childData.BelongsTo = currentData;

                            result += ExpressionGroupingSpecificationParse(expressionGroupingSpecification, childData);
                            result += $"{Indentation(currentData.Level)}{currentData.VariableName}.GroupByTerms.Add({childData.TermString});\n";
                        }
                        else {
                            result += $"{Indentation(currentData.Level)}{currentData.VariableName}.GroupByTerms.Add(~UKNOWN ExpressionWithSortOrder~);\n";
                        }
                    }
                }


                //// HAVING //////

                if (querySpecification.HavingClause != null) {
                    BooleanExpression condition = querySpecification.HavingClause.SearchCondition;

                    if (condition is BooleanComparisonExpression booleanComparisonExpression) {
                        Informations childData = currentData.CopyLite();
                        childData.BelongsTo = currentData;

                        BooleanComparisonExpressionParse(booleanComparisonExpression, childData);
                        result += $"{Indentation(currentData.Level)}{currentData.VariableName}.HavingPhrase.Terms.Add({childData.TermString});\n";
                    }
                    else if (condition is BooleanBinaryExpression booleanBinaryExpression) {
                        Informations childData = currentData.CopyLite();
                        childData.BelongsTo = currentData;

                        result += BooleanBinaryExpressionParse(booleanBinaryExpression, childData);
                        result += $"{Indentation(currentData.Level)}{currentData.VariableName}.HavingPhrase.SubClauses.Add({childData.VariableName});\n";
                    }
                    else if (condition is BooleanParenthesisExpression booleanParenthesisExpression) {
                        Informations childData = currentData.CopyLite();
                        childData.BelongsTo = currentData;

                        result += BooleanParenthesisExpressionParse(booleanParenthesisExpression, childData);

                        if (childData.VariableName != "") {
                            //result += temp;
                            result += $"{Indentation(currentData.Level)}{currentData.VariableName}.HavingPhrase.SubClauses.Add({childData.VariableName});\n";
                        }
                        else {
                            result += $"{Indentation(currentData.Level)}{currentData.VariableName}.HavingPhrase.Terms.Add({childData.TermString});\n";
                        }
                    }
                    else if (condition is BooleanNotExpression booleanNotExpression) {
                        Informations childData = currentData.CopyLite();
                        childData.BelongsTo = currentData;

                        result += BooleanNotExpressionParse(booleanNotExpression, childData);
                        result += $"{Indentation(currentData.Level)}{currentData.VariableName}.HavingPhrase.Terms.Add({childData.TermString});\n";
                    }
                    else if (condition is BooleanTernaryExpression booleanTernaryExpression) {
                        Informations childData = currentData.CopyLite();
                        childData.BelongsTo = currentData;

                        result += BooleanTernaryExpressionParse(booleanTernaryExpression, childData);
                        result += $"{Indentation(currentData.Level)}{currentData.VariableName}.HavingPhrase.Terms.Add({childData.TermString});\n";
                    }
                    else if (condition is BooleanIsNullExpression booleanIsNullExpression) {
                        Informations childData = currentData.CopyLite();
                        childData.BelongsTo = currentData;

                        result += BooleanIsNullExpressionParse(booleanIsNullExpression, childData);
                        result += $"{Indentation(currentData.Level)}{currentData.VariableName}.HavingPhrase.Terms.Add({childData.TermString});\n";
                    }
                    else if (condition is InPredicate inPredicate) {
                        Informations childData = currentData.CopyLite();
                        childData.BelongsTo = currentData;

                        result += InPredicateParse(inPredicate, childData);
                        result += $"{Indentation(currentData.Level)}{currentData.VariableName}.HavingPhrase.Terms.Add({childData.TermString});\n";
                    }
                    else if (condition is LikePredicate likePredicate) {
                        Informations childData = currentData.CopyLite();
                        childData.BelongsTo = currentData;

                        result += LikePredicateParse(likePredicate, childData);
                        result += $"{Indentation(currentData.Level)}{currentData.VariableName}.HavingPhrase.Terms.Add({childData.TermString});\n";
                    }
                    else if (condition is ExistsPredicate existsPredicate) {
                        Informations childData = currentData.CopyLite();
                        childData.BelongsTo = currentData;

                        result += ExistsPredicateParse(existsPredicate, childData);
                        result += $"{Indentation(currentData.Level)}{currentData.VariableName}.HavingPhrase.Terms.Add({childData.TermString});\n";
                    }
                    else {
                        result += "~UNKNOWN BooleanExpression~";
                    }
                }


                //// ORDER BY //////

                if (querySpecification.OrderByClause != null) {
                    foreach (ExpressionWithSortOrder condition in querySpecification.OrderByClause.OrderByElements) {
                        if (condition is ExpressionWithSortOrder expressionWithSortOrder) {
                            Informations childData = currentData.CopyLite();
                            childData.BelongsTo = currentData;

                            result += ExpressionWithSortOrderParse(expressionWithSortOrder, childData);
                            result += $"{Indentation(currentData.Level)}{currentData.VariableName}.OrderByTerms.Add({childData.TermString});\n";
                        }
                        else {
                            result += $"{Indentation(currentData.Level)}{currentData.VariableName}.OrderByTerms.Add(~UKNOWN ExpressionWithSortOrder~);\n";
                        }
                    }
                }


                //// COLUMNS /////

                IList<SelectElement> elements = querySpecification.SelectElements;
                foreach (SelectElement column in elements) {
                    if (column is SelectScalarExpression selectScalarExpression) {

                        ScalarExpression expression = selectScalarExpression.Expression;
                        if (expression is ColumnReferenceExpression columnReferenceExpression) {

                            string alias = selectScalarExpression.ColumnName != null ? selectScalarExpression.ColumnName.Value : "";

                            Informations childData = currentData.CopyLite();
                            childData.BelongsTo = currentData;
                            childData.Alias = alias;

                            result += ColumnReferenceExpressionParse(columnReferenceExpression, childData);

                            result += $"{Indentation(currentData.Level)}{currentData.VariableName}.Columns.Add({childData.TermString});\n";
                        }
                        else if (expression is ScalarSubquery scalarSubquery) {

                            string alias = selectScalarExpression.ColumnName != null ? selectScalarExpression.ColumnName.Value : "";

                            Informations childData = currentData.CopyLite();
                            childData.BelongsTo = currentData;
                            childData.Alias = alias;

                            result += QueryExpressionParse(scalarSubquery.QueryExpression, childData);
                            result += $"{Indentation(currentData.Level)}{currentData.VariableName}.Columns.Add(SqlExpression.SubQuery({childData.VariableName}), \"{alias}\");\n";
                        }
                        else if (expression is SearchedCaseExpression searchedCaseExpression) {

                            string alias = selectScalarExpression.ColumnName != null ? selectScalarExpression.ColumnName.Value : "";

                            Informations childData = currentData.CopyLite();
                            childData.BelongsTo = currentData;
                            childData.Alias = alias;

                            result += SearchedCaseExpressionParse(searchedCaseExpression, childData);
                            result += $"{Indentation(currentData.Level)}{currentData.VariableName}.Columns.Add({childData.TermString});\n";
                        } 
                        else if (expression is IntegerLiteral integerLiteral) {
                            string columnName = $"new SelectColumn(SqlExpression.Number({integerLiteral.Value})";

                            // check if there is an alias for the column
                            if (selectScalarExpression.ColumnName != null) {
                                columnName += $", \"{selectScalarExpression.ColumnName.Value}\"";
                            }

                            columnName += ")";

                            result += $"{currentData.VariableName}.Columns.Add({columnName});\n";
                        }
                        else if (expression is StringLiteral stringLiteral) {
                            string columnName = $"new SelectColumn(SqlExpression.String(\"{stringLiteral.Value}\")";

                            // check if there is an alias for the column
                            if (selectScalarExpression.ColumnName != null) {
                                columnName += $", \"{selectScalarExpression.ColumnName.Value}\"";
                            }

                            columnName += ")";

                            result += $"{Indentation(currentData.Level)}{currentData.VariableName}.Columns.Add({columnName});\n";
                        }
                        else if (expression is NumericLiteral numericLiteral) {
                            string columnName = $"new SelectColumn(SqlExpression.Number({numericLiteral.Value})";

                            // check if there is an alias for the column
                            if (selectScalarExpression.ColumnName != null) {
                                columnName += $", \"{selectScalarExpression.ColumnName.Value}\"";
                            }

                            columnName += ")";

                            result += $"{currentData.VariableName}.Columns.Add({columnName});\n";
                        }
                        else if (expression is NullLiteral nullLiteral1) {
                            string columnName = $"new SelectColumn(SqlExpression.Null()";

                            // check if there is an alias for the column
                            if (selectScalarExpression.ColumnName != null) {
                                columnName += $", \"{selectScalarExpression.ColumnName.Value}\"";
                            }

                            columnName += ")";

                            result += $"{currentData.VariableName}.Columns.Add({columnName});\n";
                        }
                        else if (expression is VariableReference variableReference) {
                            string columnName = $"new SelectColumn(SqlExpression.Parameter(\"{variableReference.Name}\")";

                            // check if there is an alias for the column
                            if (selectScalarExpression.ColumnName != null) {
                                columnName += $", \"{selectScalarExpression.ColumnName.Value}\"";
                            }

                            columnName += ")";

                            result += $"{currentData.VariableName}.Columns.Add({columnName});\n";
                        }
                        else if (expression is FunctionCall functionCall) {

                            string alias = selectScalarExpression.ColumnName != null ? selectScalarExpression.ColumnName.Value : "";

                            Informations childData = currentData.CopyLite();
                            childData.BelongsTo = currentData;
                            childData.Alias = alias;

                            result += FunctionCallParse(functionCall, childData);
                            result += $"{Indentation(currentData.Level)}{currentData.VariableName}.Columns.Add({childData.TermString});\n";

                        }
                        else if (expression is ParenthesisExpression parenthesisExpression) {
                            string alias = selectScalarExpression.ColumnName != null ? selectScalarExpression.ColumnName.Value : "";

                            Informations childData = currentData.CopyLite();
                            childData.BelongsTo = currentData;
                            childData.Alias = alias;

                            result += ParenthesisExpressionParse(parenthesisExpression, childData);

                            result += $"{Indentation(currentData.Level)}{currentData.VariableName}.Columns.Add({childData.TermString});\n";

                        }
                        else if (expression is BinaryExpression binaryExpression) {
                            Informations childData = currentData.CopyLite();
                            childData.BelongsTo = currentData;

                            BinaryExpressionParse(binaryExpression, childData);

                            string alias = selectScalarExpression.ColumnName != null ? selectScalarExpression.ColumnName.Value : "";

                            result += $"{Indentation(currentData.Level)}{currentData.VariableName}.Columns.Add({childData.SqlExpressionString}, \"{alias}\");\n";
                        }
                        else {
                            result += $"{Indentation(currentData.Level)}{currentData.VariableName}.Columns.Add(~UNKNOWN ScalarExpression~);\n";
                        }
                    }
                    else if (column is SelectStarExpression selectStarExpression) {
                        Informations childData = currentData.CopyLite();
                        childData.BelongsTo = currentData;

                        SelectStarExpression(selectStarExpression, childData);
                        result += $"{Indentation(currentData.Level)}{currentData.VariableName}.Columns.Add({childData.TermString});\n";
                    }
                    else if (column is SelectSetVariable) {
                        //UNKNOWN SelectSetVariable
                        result += $"{Indentation(currentData.Level)}{currentData.VariableName}.Columns.Add(~UNKNOWN SelectSetVariable~);\n";
                    }
                    else {
                        //UNKNOWN COLUMN
                        result += $"{Indentation(currentData.Level)}{currentData.VariableName}.Columns.Add(~UNKNOWN COLUMN~);\n";
                    }
                }
            }
            catch {
                result = "~QuerySpecification ERROR~";
            }

            return result;
        }

        public virtual string FunctionCallParse(FunctionCall expression, object data = null) {
            string result = "";
            Informations currentData = (Informations)data;

            try {
                foreach (var parameter in expression.Parameters) {
                    if (parameter is CastCall castCall) {

                        Translator translator = new Translator();
                        translator.Data.Level = currentData.Level + 1;
                        translator.FormatOptions.IndentationSize = FormatOptions.IndentationSize;

                        currentData.SqlExpressionString = $"SqlExpression.Raw(\"{translator.CastCallParse(castCall)}\")";
                        currentData.TermString = $"new SelectColumn({currentData.SqlExpressionString}, \"{currentData.Alias}\")";
                    } 
                    else if (parameter is ColumnReferenceExpression columnReferenceExpression2) {
                        string function = expression.FunctionName.Value;

                        if (function.ToLower() == "Sum".ToLower()) {
                            function = $"SqlAggregationFunction.Sum";
                        } else if (function.ToLower() == "Count".ToLower()) {
                            function = $"SqlAggregationFunction.Count";
                        } else if (function.ToLower() == "Avg".ToLower()) {
                            function = $"SqlAggregationFunction.Avg";
                        } else if (function.ToLower() == "Min".ToLower()) {
                            function = $"SqlAggregationFunction.Min";
                        } else if (function.ToLower() == "Max".ToLower()) {
                            function = $"SqlAggregationFunction.Max";
                        } else if (function.ToLower() == "Grouping".ToLower()) {
                            function = $"SqlAggregationFunction.Grouping";
                        } else {
                            function = $"SqlAggregationFunction.~UNKNOWN function~";
                        }

                        Informations childData = currentData.CopyLite();
                        childData.BelongsTo = currentData;
                        childData.Alias = currentData.Alias;
                        childData.FunctionName = function;

                        result += ColumnReferenceExpressionParse(columnReferenceExpression2, childData);
                    } 
                    else if (parameter is SearchedCaseExpression searchedCaseExpression2) {

                        Translator translator = new Translator();
                        translator.Data.Level = currentData.Level + 1;
                        translator.FormatOptions.IndentationSize = FormatOptions.IndentationSize;

                        currentData.SqlExpressionString = $"SqlExpression.Raw(\"{translator.FunctionCallParse(expression)}\")";
                        currentData.TermString = $"new SelectColumn({currentData.SqlExpressionString}, \"{currentData.Alias}\")";
                    } 
                    else if (parameter is ParenthesisExpression parenthesisExpression) {
                        Translator translator = new Translator();
                        translator.Data.Level = currentData.Level + 1;
                        translator.FormatOptions.IndentationSize = FormatOptions.IndentationSize;

                        currentData.SqlExpressionString = $"SqlExpression.Raw(\"{translator.FunctionCallParse(expression)}\")";
                        currentData.TermString = $"new SelectColumn({currentData.SqlExpressionString}, \"{currentData.Alias}\")";
                    } 
                    else if (parameter is IntegerLiteral integerLiteral) {
                        Translator translator = new Translator();
                        translator.Data.Level = currentData.Level + 1;
                        translator.FormatOptions.IndentationSize = FormatOptions.IndentationSize;

                        currentData.SqlExpressionString = $"SqlExpression.Raw(\"{translator.FunctionCallParse(expression)}\")";
                        currentData.TermString = $"new SelectColumn({currentData.SqlExpressionString}, \"{currentData.Alias}\")";
                    } 
                    else if (parameter is StringLiteral stringLiteral) {
                        Translator translator = new Translator();
                        translator.Data.Level = currentData.Level + 1;
                        translator.FormatOptions.IndentationSize = FormatOptions.IndentationSize;

                        currentData.SqlExpressionString = $"SqlExpression.Raw(\"{translator.FunctionCallParse(expression)}\")";
                        currentData.TermString = $"new SelectColumn({currentData.SqlExpressionString}, \"{currentData.Alias}\")";
                    } else if (parameter is NumericLiteral numericLiteral) {
                        Translator translator = new Translator();
                        translator.Data.Level = currentData.Level + 1;
                        translator.FormatOptions.IndentationSize = FormatOptions.IndentationSize;

                        currentData.SqlExpressionString = $"SqlExpression.Raw(\"{translator.FunctionCallParse(expression)}\")";
                        currentData.TermString = $"new SelectColumn({currentData.SqlExpressionString}, \"{currentData.Alias}\")";
                    } 
                    else if (parameter is NullLiteral nullLiteral) {
                        Translator translator = new Translator();
                        translator.Data.Level = currentData.Level + 1;
                        translator.FormatOptions.IndentationSize = FormatOptions.IndentationSize;

                        currentData.SqlExpressionString = $"SqlExpression.Raw(\"{translator.FunctionCallParse(expression)}\")";
                        currentData.TermString = $"new SelectColumn({currentData.SqlExpressionString}, \"{currentData.Alias}\")";
                    } 
                    else if (parameter is BinaryExpression binaryExpression) {
                        Translator translator = new Translator();
                        translator.Data.Level = currentData.Level + 1;
                        translator.FormatOptions.IndentationSize = FormatOptions.IndentationSize;

                        currentData.SqlExpressionString = $"SqlExpression.Raw(\"{translator.FunctionCallParse(expression)}\")";
                        currentData.TermString = $"new SelectColumn({currentData.SqlExpressionString}, \"{currentData.Alias}\")";
                    } 
                    else {
                        result += $"(UNKNOWN parameter)";
                    }
                }
            } catch {
                currentData.TermString = "~FunctionCall ERROR~";
            }

            return result;
        }

        public virtual string QueryParenthesisExpressionParse(QueryParenthesisExpression expression, object data = null) {
            string result = "";
            Informations currentData = (Informations)data;

            try {
                if (expression is QueryExpression queryExpression) {
                    Informations childData = currentData.CopyLite();

                    result += QueryExpressionParse(expression.QueryExpression, childData);

                    currentData.TermString = childData.TermString;
                    currentData.VariableName = childData.VariableName;
                    currentData.SqlExpressionString = childData.SqlExpressionString;

                }
                else {
                    result = "~UNDEVELOPED QueryParenthesisExpression~";
                }
            }
            catch {
                result = "~QueryParenthesisExpression ERROR~";
            }

            return result;
        }

        public virtual string ParenthesisExpressionParse(ParenthesisExpression expression, object data = null) {
            string result = "";
            Informations currentData = (Informations)data;

            try {
                if (expression.Expression is ColumnReferenceExpression columnReferenceExpression) {

                    Informations childData = currentData.CopyLite();
                    childData.Alias = currentData.Alias;

                    result += ColumnReferenceExpressionParse(columnReferenceExpression, childData);

                    currentData.TermString = childData.TermString;
                    currentData.VariableName = childData.VariableName;
                    currentData.SqlExpressionString = childData.SqlExpressionString;
                }
                else if (expression.Expression is ParenthesisExpression parenthesisExpression) {
                    Informations childData = currentData.CopyLite();
                    childData.Alias = currentData.Alias;

                    result += ParenthesisExpressionParse(parenthesisExpression, childData);

                    currentData.TermString = childData.TermString;
                    currentData.VariableName = childData.VariableName;
                    currentData.SqlExpressionString = childData.SqlExpressionString;
                }
                else if (expression.Expression is SearchedCaseExpression searchedCaseExpression) {
                    Informations childData = currentData.CopyLite();
                    childData.Alias = currentData.Alias;

                    result += SearchedCaseExpressionParse(searchedCaseExpression, childData);

                    currentData.TermString = $"new SelectColumn({childData.SqlExpressionString}, \"{currentData.Alias}\")";
                    currentData.VariableName = childData.VariableName;
                    currentData.SqlExpressionString = childData.SqlExpressionString;
                }
                else if (expression.Expression is BinaryExpression binaryExpression) {
                    Informations childData = currentData.CopyLite();

                    result += BinaryExpressionParse(binaryExpression, childData);

                    currentData.TermString = $"new SelectColumn({childData.SqlExpressionString}, \"{currentData.Alias}\")";
                    currentData.VariableName = childData.VariableName;
                    currentData.SqlExpressionString = childData.SqlExpressionString;
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
            string result = "";
            Informations currentData = (Informations)data;

            try {
                Translator translator = new Translator();
                translator.Data.Level = currentData.Level + 1;
                translator.FormatOptions.IndentationSize = FormatOptions.IndentationSize;

                string subQuery = translator.BinaryExpressionParse(expression);

                currentData.SqlExpressionString = $"SqlExpression.Raw(\"{subQuery}\")";
            }
            catch {
                currentData.SqlExpressionString = "~BinaryExpression ERROR~";
            }

            return result;
        }

        public virtual string BooleanBinaryExpressionParse(BooleanBinaryExpression expression, object data = null) {
            string result = "";
            Informations currentData = (Informations)data;

            try {
                string comparison;

                currentData.VariableName = $"whereClause{Informations.NextWhereClauseIndex()}";
                currentData.Level++;

                // comparison
                if (expression.BinaryExpressionType == BooleanBinaryExpressionType.And) {
                    comparison = "And";
                }
                else if (expression.BinaryExpressionType == BooleanBinaryExpressionType.Or) {
                    comparison = "Or";
                }
                else {
                    comparison = "~UNKNOWN BooleanBinaryExpressionType~";
                }

                result = $"{Indentation(currentData.Level)}WhereClause {currentData.VariableName} = new WhereClause(WhereClauseRelationship.{comparison});\n";


                // first expression
                if (expression.FirstExpression is BooleanComparisonExpression booleanComparisonExpression1) {
                    Informations childData = currentData.CopyLite();
                    result += BooleanComparisonExpressionParse(booleanComparisonExpression1, childData);
                    result += $"{Indentation(currentData.Level)}{currentData.VariableName}.Terms.Add({childData.TermString});\n";
                }
                else if (expression.FirstExpression is BooleanBinaryExpression booleanBinaryExpression1) {
                    Informations childData = currentData.CopyLite();
                    result += BooleanBinaryExpressionParse(booleanBinaryExpression1, childData);
                    result += $"{Indentation(currentData.Level)}{currentData.VariableName}.SubClauses.Add({childData.VariableName});\n";
                }
                else if (expression.FirstExpression is BooleanParenthesisExpression booleanParenthesisExpression1) {
                    Informations childData = currentData.CopyLite();
                    result += BooleanParenthesisExpressionParse(booleanParenthesisExpression1, childData);

                    if (childData.VariableName != "") {
                        //result += temp;
                        result += $"{Indentation(currentData.Level)}{currentData.VariableName}.SubClauses.Add({childData.VariableName});\n";
                    }
                    else {
                        result += $"{Indentation(currentData.Level)}{currentData.VariableName}.Terms.Add({childData.TermString});\n";
                    }
                }
                else if (expression.FirstExpression is BooleanNotExpression booleanNotExpression1) {
                    Informations childData = currentData.CopyLite();
                    result += BooleanNotExpressionParse(booleanNotExpression1, childData);
                    result += $"{Indentation(currentData.Level)}{currentData.VariableName}.Terms.Add({childData.TermString});\n";
                }
                else if (expression.FirstExpression is ExistsPredicate existsPredicate1) {
                    Informations childData = currentData.CopyLite();
                    result += ExistsPredicateParse(existsPredicate1, childData);
                    result += $"{Indentation(currentData.Level)}{currentData.VariableName}.Terms.Add({childData.TermString});\n";
                }
                else if (expression.FirstExpression is BooleanIsNullExpression booleanIsNullExpression) {
                    Informations childData = currentData.CopyLite();
                    result += BooleanIsNullExpressionParse(booleanIsNullExpression, childData);
                    result += $"{Indentation(currentData.Level)}{currentData.VariableName}.Terms.Add({childData.TermString});\n";
                }
                else if (expression.FirstExpression is InPredicate inPredicate) {
                    Informations childData = currentData.CopyLite();
                    result += InPredicateParse(inPredicate, childData);
                    result += $"{Indentation(currentData.Level)}{currentData.VariableName}.Terms.Add({childData.TermString});\n";
                }
                else if (expression.FirstExpression is BooleanTernaryExpression booleanTernaryExpression1) {
                    Informations childData = currentData.CopyLite();
                    result += BooleanTernaryExpressionParse(booleanTernaryExpression1, childData);
                    result += $"{Indentation(currentData.Level)}{currentData.VariableName}.Terms.Add({childData.TermString});\n";
                }
                else {
                    result += $"{Indentation(currentData.Level)}{currentData.VariableName}.Terms.Add(~UNKNOWN BooleanExpression~);\n";
                }

                // second expression
                if (expression.SecondExpression is BooleanComparisonExpression booleanComparisonExpression2) {
                    Informations childData = currentData.CopyLite();
                    result += BooleanComparisonExpressionParse(booleanComparisonExpression2, childData);
                    result += $"{Indentation(currentData.Level)}{currentData.VariableName}.Terms.Add({childData.TermString});\n";
                }
                else if (expression.SecondExpression is BooleanBinaryExpression booleanBinaryExpression2) {
                    Informations childData = currentData.CopyLite();
                    result += BooleanBinaryExpressionParse(booleanBinaryExpression2, childData);
                    result += $"{Indentation(currentData.Level)}{currentData.VariableName}.SubClauses.Add({childData.VariableName});\n";
                }
                else if (expression.SecondExpression is BooleanParenthesisExpression booleanParenthesisExpression2) {
                    Informations childData = currentData.CopyLite();
                    childData.BelongsTo = currentData;

                    result += BooleanParenthesisExpressionParse(booleanParenthesisExpression2, childData);

                    if (childData.VariableName != "") {
                        //result += temp;
                        result += $"{Indentation(currentData.Level)}{currentData.VariableName}.SubClauses.Add({childData.VariableName});\n";
                    }
                    else {
                        result += $"{Indentation(currentData.Level)}{currentData.VariableName}.Terms.Add({childData.TermString});\n";
                    }
                }
                else if (expression.SecondExpression is BooleanNotExpression booleanNotExpression2) {
                    Informations childData = currentData.CopyLite();
                    result += BooleanNotExpressionParse(booleanNotExpression2, childData);
                    result += $"{Indentation(currentData.Level)}{currentData.VariableName}.Terms.Add({childData.TermString});\n";
                }
                else if (expression.SecondExpression is ExistsPredicate existsPredicate2) {
                    Informations childData = currentData.CopyLite();
                    result += ExistsPredicateParse(existsPredicate2, childData);
                    result += $"{Indentation(currentData.Level)}{currentData.VariableName}.Terms.Add({childData.TermString});\n";
                }
                else if (expression.SecondExpression is BooleanIsNullExpression booleanIsNullExpression2) {
                    Informations childData = currentData.CopyLite();
                    result += BooleanIsNullExpressionParse(booleanIsNullExpression2, childData);
                    result += $"{Indentation(currentData.Level)}{currentData.VariableName}.Terms.Add({childData.TermString});\n";
                }
                else if (expression.SecondExpression is BooleanTernaryExpression booleanTernaryExpression2) {
                    Informations childData = currentData.CopyLite();
                    result += BooleanTernaryExpressionParse(booleanTernaryExpression2, childData);
                    result += $"{Indentation(currentData.Level)}{currentData.VariableName}.Terms.Add({childData.TermString});\n";
                }
                else {
                    result += $"{Indentation(currentData.Level)}{currentData.VariableName}.Terms.Add(~UNKNOWN BooleanExpression~);\n";
                }
            }
            catch {
                result = "~BooleanBinaryExpression ERROR~";
            }

            return result;
        }

        public virtual string BooleanComparisonExpressionParse(BooleanComparisonExpression expression, object data = null) {
            string result = "";
            Informations currentData = (Informations)data;

            try {
                string firstExpression;
                string secondExpression;
                string comparisonExpression;

                // first expression
                if (expression.FirstExpression is ColumnReferenceExpression columnReferenceExpression1) {
                    Informations childData1 = new Informations() {
                        Parent = currentData,
                        BelongsTo = currentData.BelongsTo
                    };

                    result += ColumnReferenceExpressionParse(columnReferenceExpression1, childData1);
                    firstExpression = childData1.SqlExpressionString;
                    currentData.LeftTableName = childData1.TableName;
                    currentData.LeftColumnName = childData1.ColumnName;
                }
                else if (expression.FirstExpression is IntegerLiteral integerLiteral1) {
                    firstExpression = $"SqlExpression.Number({integerLiteral1.Value})";
                }
                else if (expression.FirstExpression is StringLiteral stringLiteral1) {

                    // check if the string is date
                    if (DateTime.TryParse(stringLiteral1.Value, out DateTime date2)) {
                        firstExpression = $"SqlExpression.Date(\"{stringLiteral1.Value}\")";
                    }
                    else {
                        firstExpression = $"SqlExpression.String(\"{stringLiteral1.Value}\")";
                    }
                }
                else if (expression.FirstExpression is NumericLiteral numericLiteral1) {
                    firstExpression = $"SqlExpression.Number({numericLiteral1.Value})";
                }
                else if (expression.FirstExpression is NullLiteral nullLiteral1) {
                    firstExpression = $"SqlExpression.Null()";
                }
                else if (expression.FirstExpression is VariableReference variableReference1) {
                    firstExpression = $"SqlExpression.Parameter(\"{variableReference1.Name}\")";
                }
                else {
                    firstExpression = "~UNKNOWN FirstExpression~";
                }

                // second expression
                if (expression.SecondExpression is ColumnReferenceExpression columnReferenceExpression2) {

                    Informations childData2 = new Informations() {
                        Parent = currentData,
                        BelongsTo = currentData.BelongsTo
                    };

                    result += ColumnReferenceExpressionParse(columnReferenceExpression2, childData2);
                    secondExpression = childData2.SqlExpressionString;
                    currentData.RightTableName = childData2.TableName;
                    currentData.RightColumnName = childData2.ColumnName;
                }
                else if (expression.SecondExpression is IntegerLiteral integerLiteral2) {
                    secondExpression = $"SqlExpression.Number({integerLiteral2.Value})";

                }
                else if (expression.SecondExpression is StringLiteral stringLiteral2) {

                    // check if the string is date
                    if (DateTime.TryParse(stringLiteral2.Value, out DateTime date2)) {
                        secondExpression = $"SqlExpression.Date(\"{stringLiteral2.Value}\")";
                    }
                    else {
                        secondExpression = $"SqlExpression.String(\"{stringLiteral2.Value}\")";
                    }
                }
                else if (expression.SecondExpression is NumericLiteral numericLiteral2) {
                    secondExpression = $"SqlExpression.Number({numericLiteral2.Value})";
                }
                else if (expression.SecondExpression is NullLiteral nullLiteral2) {
                    secondExpression = $"SqlExpression.Null()";
                }
                else if (expression.SecondExpression is VariableReference variableReference2) {
                    secondExpression = $"SqlExpression.Parameter(\"{variableReference2.Name}\")";
                }
                else {
                    secondExpression = "~UNKNOWN SecondExpression~";
                }

                // comparison
                if (expression.ComparisonType == BooleanComparisonType.Equals) {
                    comparisonExpression = "CompareOperator.Equal";
                }
                else if (expression.ComparisonType == BooleanComparisonType.GreaterThan) {
                    comparisonExpression = "CompareOperator.Greater";
                }
                else if (expression.ComparisonType == BooleanComparisonType.GreaterThanOrEqualTo) {
                    comparisonExpression = "CompareOperator.GreaterOrEqual";
                }
                else if (expression.ComparisonType == BooleanComparisonType.LeftOuterJoin) {
                    comparisonExpression = "~UNKNOWN comparisorType~";
                }
                else if (expression.ComparisonType == BooleanComparisonType.LessThan) {
                    comparisonExpression = "CompareOperator.Less";
                }
                else if (expression.ComparisonType == BooleanComparisonType.LessThanOrEqualTo) {
                    comparisonExpression = "CompareOperator.LessOrEqual";
                }
                else if (expression.ComparisonType == BooleanComparisonType.NotEqualToBrackets) {
                    comparisonExpression = "CompareOperator.NotEqual";
                }
                else if (expression.ComparisonType == BooleanComparisonType.NotEqualToExclamation) {
                    comparisonExpression = "~UNKNOWN comparisorType~";
                }
                else if (expression.ComparisonType == BooleanComparisonType.NotGreaterThan) {
                    comparisonExpression = "CompareOperator.LessOrEqual";
                }
                else if (expression.ComparisonType == BooleanComparisonType.NotLessThan) {
                    comparisonExpression = "CompareOperator.GreaterOrEqual";
                }
                else if (expression.ComparisonType == BooleanComparisonType.RightOuterJoin) {
                    comparisonExpression = "~UNKNOWN comparisorType~";
                }
                else {
                    comparisonExpression = "~UNKNOWN comparisorType~";
                }

                currentData.TermString = $"WhereTerm.CreateCompare({firstExpression}, {secondExpression}, {comparisonExpression})";
            }
            catch {
                currentData.TermString = "~BooleanComparisonExpression ERROR~";
            }

            return result;
        }

        public virtual string BooleanIsNullExpressionParse(BooleanIsNullExpression expression, object data = null) {
            string result = "";
            Informations currentData = (Informations)data;

            try {
                string createMethod = expression.IsNot ? "CreateIsNotNull" : "CreateIsNull";

                if (expression.Expression is ColumnReferenceExpression columnReferenceExpression) {
                    Informations childData = currentData.CopyLite();

                    result += ColumnReferenceExpressionParse(columnReferenceExpression, childData);

                    currentData.SqlExpressionString = childData.SqlExpressionString;
                }
                else if (expression.Expression is IntegerLiteral integerLiteral) {
                    currentData.SqlExpressionString = $"SqlExpression.Number({integerLiteral.Value})";
                }
                else if (expression.Expression is StringLiteral stringLiteral) {

                    // check if the string is date
                    if (DateTime.TryParse(stringLiteral.Value, out DateTime date1)) {
                        currentData.SqlExpressionString = $"SqlExpression.Date(\"{stringLiteral.Value}\")";
                    }
                    else {
                        currentData.SqlExpressionString = $"SqlExpression.String(\"{stringLiteral.Value}\")";
                    }
                }
                else if (expression.Expression is NumericLiteral numericLiteral) {
                    currentData.SqlExpressionString = $"SqlExpression.Number({numericLiteral.Value})";
                }
                else if (expression.Expression is NullLiteral nullLiteral) {
                    currentData.SqlExpressionString = $"SqlExpression.Null()";
                }
                else if (expression.Expression is VariableReference variableReference) {
                    currentData.SqlExpressionString = $"SqlExpression.Parameter(\"{variableReference.Name}\")";
                }
                else {
                    currentData.SqlExpressionString = "~UKNOWN BooleanIsNullExpression~";
                }

                currentData.TermString = $"WhereTerm.{createMethod}({currentData.SqlExpressionString})";
            }
            catch {
                currentData.TermString = "~BooleanIsNullExpression ERROR~";
            }

            return result;
        }

        public virtual string BooleanNotExpressionParse(BooleanNotExpression expression, object data = null) {
            string result = "";
            Informations currentData = (Informations)data;

            try {
                if (expression.Expression is ExistsPredicate existsPredicate) {
                    Informations childData = currentData.CopyLite();
                    childData.Not = true;

                    result += ExistsPredicateParse(existsPredicate, childData);
                    currentData.TermString = childData.TermString;
                }
                else {
                    currentData.TermString = "~SqlOM doesnt support NOT operator like this~";
                }
            }
            catch {
                currentData.TermString = "~BooleanNotExpression ERROR~";
            }

            return result;
        }

        public virtual string BooleanParenthesisExpressionParse(BooleanParenthesisExpression expression, object data = null) {
            string result = "";
            Informations currentData = (Informations)data;

            try {

                if (expression.Expression is BooleanComparisonExpression booleanComparisonExpression) {
                    Informations childData = currentData.CopyLite();

                    result += BooleanComparisonExpressionParse(booleanComparisonExpression, childData);
                    currentData.TermString = childData.TermString;
                    currentData.LeftTableName = childData.LeftTableName;
                    currentData.LeftColumnName = childData.LeftColumnName;
                    currentData.RightTableName = childData.RightTableName;
                    currentData.RightColumnName = childData.RightColumnName;
                }
                else if (expression.Expression is BooleanBinaryExpression booleanBinaryExpression) {
                    Informations childData = currentData.CopyLite();

                    result += BooleanBinaryExpressionParse(booleanBinaryExpression, childData);
                    currentData.VariableName = childData.VariableName;
                }
                else if (expression.Expression is BooleanNotExpression booleanNotExpression) {
                    Informations childData = currentData.CopyLite();

                    result += BooleanNotExpressionParse(booleanNotExpression, childData);
                    currentData.TermString = childData.TermString;
                    currentData.VariableName = childData.VariableName;
                }
                else if (expression.Expression is BooleanTernaryExpression booleanTernaryExpression) {
                    Informations childData = currentData.CopyLite();

                    result += BooleanTernaryExpressionParse(booleanTernaryExpression, childData);
                    currentData.TermString = childData.TermString;
                }
                else if (expression.Expression is BooleanIsNullExpression booleanIsNullExpression) {
                    result += "~UNDEVELOPED BooleanIsNullExpression~";
                }
                else if (expression.Expression is BooleanParenthesisExpression booleanParenthesisExpression) {
                    Informations childData = currentData.CopyLite();

                    result += BooleanParenthesisExpressionParse(booleanParenthesisExpression, childData);

                    currentData.TermString = childData.TermString;
                    currentData.VariableName = childData.VariableName;
                }
                else if (expression.Expression is InPredicate inPredicate) {
                    Informations childData = currentData.CopyLite();

                    result += InPredicateParse(inPredicate, childData);
                    currentData.TermString = childData.TermString;
                    currentData.VariableName = childData.VariableName;
                }
                else {
                    result = "~UKNOWN BooleanParenthesisExpression~";
                }
            }
            catch {
                result = "~BooleanParenthesisExpression ERROR~";
            }

            return result;
        }

        public virtual string BooleanTernaryExpressionParse(BooleanTernaryExpression expression, object data = null) {
            string result = "";
            Informations currentData = (Informations)data;

            try {
                string firstExpression;
                string secondExpression;
                string thirdExpression;
                string comparison;

                // first expression
                if (expression.FirstExpression is ColumnReferenceExpression columnReferenceExpression1) {
                    Informations childData = currentData.CopyLite();
                    result += ColumnReferenceExpressionParse(columnReferenceExpression1, childData);
                    firstExpression = childData.SqlExpressionString;
                }
                else if (expression.FirstExpression is IntegerLiteral integerLiteral1) {
                    firstExpression = $"SqlExpression.Number({integerLiteral1.Value})";
                }
                else if (expression.FirstExpression is StringLiteral stringLiteral1) {

                    // check if the string is date
                    if (DateTime.TryParse(stringLiteral1.Value, out DateTime date1)) {
                        firstExpression = $"SqlExpression.Date(\"{stringLiteral1.Value}\")";
                    }
                    else {
                        firstExpression = $"SqlExpression.String(\"{stringLiteral1.Value}\")";
                    }
                }
                else if (expression.FirstExpression is NumericLiteral numericLiteral1) {
                    firstExpression = $"SqlExpression.Number({numericLiteral1.Value})";
                }
                else if (expression.FirstExpression is NullLiteral nullLiteral1) {
                    firstExpression = $"SqlExpression.Null()";
                }
                else if (expression.FirstExpression is VariableReference variableReference1) {
                    firstExpression = $"SqlExpression.Parameter(\"{variableReference1.Name}\")";
                }
                else {
                    firstExpression = "~UNKNOWN ScalarExpression~";
                }

                // second expression
                if (expression.SecondExpression is ColumnReferenceExpression columnReferenceExpression2) {

                    Informations childData = currentData.CopyLite();

                    result += ColumnReferenceExpressionParse(columnReferenceExpression2, childData);
                    secondExpression = childData.SqlExpressionString;
                }
                else if (expression.SecondExpression is IntegerLiteral integerLiteral2) {
                    secondExpression = $"SqlExpression.Number({integerLiteral2.Value})";

                }
                else if (expression.SecondExpression is StringLiteral stringLiteral2) {

                    // check if the string is date
                    if (DateTime.TryParse(stringLiteral2.Value, out DateTime date2)) {
                        secondExpression = $"SqlExpression.Date(\"{stringLiteral2.Value}\")";
                    }
                    else {
                        secondExpression = $"SqlExpression.String(\"{stringLiteral2.Value}\")";
                    }
                }
                else if (expression.SecondExpression is NumericLiteral numericLiteral2) {
                    secondExpression = $"SqlExpression.Number({numericLiteral2.Value})";
                }
                else if (expression.SecondExpression is NullLiteral nullLiteral2) {
                    secondExpression = $"SqlExpression.Null()";
                }
                else if (expression.SecondExpression is VariableReference variableReference2) {
                    secondExpression = $"SqlExpression.Parameter(\"{variableReference2.Name}\")";
                }
                else {
                    secondExpression = "~UNKNOWN ScalarExpression~";
                }

                // third expression
                if (expression.ThirdExpression is ColumnReferenceExpression columnReferenceExpression3) {

                    Informations childData = currentData.CopyLite();

                    result += ColumnReferenceExpressionParse(columnReferenceExpression3, childData);
                    thirdExpression = childData.SqlExpressionString;
                }
                else if (expression.ThirdExpression is IntegerLiteral integerLiteral3) {
                    thirdExpression = $"SqlExpression.Number({integerLiteral3.Value})";
                }
                else if (expression.ThirdExpression is StringLiteral stringLiteral3) {

                    // check if the string is date
                    if (DateTime.TryParse(stringLiteral3.Value, out DateTime date3)) {
                        thirdExpression = $"SqlExpression.Date(\"{stringLiteral3.Value}\")";
                    }
                    else {
                        thirdExpression = $"SqlExpression.String(\"{stringLiteral3.Value}\")";
                    }
                }
                else if (expression.ThirdExpression is NumericLiteral numericLiteral3) {
                    thirdExpression = $"SqlExpression.Number({numericLiteral3.Value})";
                }
                else if (expression.ThirdExpression is NullLiteral nullLiteral3) {
                    thirdExpression = $"SqlExpression.Null()";
                }
                else if (expression.ThirdExpression is VariableReference variableReference3) {
                    thirdExpression = $"SqlExpression.Parameter(\"{variableReference3.Name}\")";
                }
                else {
                    thirdExpression = "~UNKNOWN ScalarExpression~";
                }

                // comparison
                if (expression.TernaryExpressionType == BooleanTernaryExpressionType.Between) {
                    comparison = "CreateBetween";
                }
                else if (expression.TernaryExpressionType == BooleanTernaryExpressionType.NotBetween) {
                    comparison = "~SqlOM doesnt support Not Between~";
                }
                else {
                    comparison = "~UNKNOWN BooleanTernaryExpressionType~";
                }

                currentData.TermString = $"WhereTerm.{comparison}({firstExpression}, {secondExpression}, {thirdExpression})";
            }
            catch {
                result = "~BooleanTernaryExpression ERROR~";
            }

            return result;
        }

        public virtual string ColumnReferenceExpressionParse(ColumnReferenceExpression expression, object data = null) {
            string result = "";
            Informations currentData = (Informations)data;

            try {
                MultiPartIdentifier identifier = expression.MultiPartIdentifier;
                string column = "";
                string table = "";
                string alias = "";
                string function = "";

                if (expression.ColumnType == ColumnType.Regular) {
                    // single column name
                    if (identifier.Identifiers.Count == 1) {
                        column = identifier.Identifiers[0].Value;
                    }
                    // column name with table identifier
                    else if (identifier.Identifiers.Count == 2) {
                        column = identifier.Identifiers[1].Value;
                        table = identifier.Identifiers[0].Value;
                    }
                    // column with domain identifier
                    else if (identifier.Identifiers.Count == 3) {
                        column = $"{identifier.Identifiers[0].Value}.{identifier.Identifiers[1].Value}";
                        table = identifier.Identifiers[2].Value;
                    }

                    if (currentData != null) {
                        alias = currentData.Alias != "" ? currentData.Alias : "";
                        function = currentData.FunctionName != "" ? currentData.FunctionName : "";
                    }

                    if (function != "") {
                        currentData.TermString = $"\"{column}\", {table}, \"{alias}\", {function}";
                        currentData.SqlExpressionString = $"SqlExpression.Field(\"{column}\", {table})";
                    }
                    else if (alias != "") {
                        currentData.TermString = $"\"{column}\", {table}, \"{alias}\"";
                        currentData.SqlExpressionString = $"SqlExpression.Field(\"{column}\", {table})";
                    }
                    else if (table != "") {
                        currentData.TermString = $"\"{column}\", {table}";
                        currentData.SqlExpressionString = $"SqlExpression.Field(\"{column}\", {table})";
                    }
                    else {
                        currentData.TermString = $"\"{column}\"";
                        currentData.SqlExpressionString = $"SqlExpression.Field(\"{column}\")";
                    }

                    currentData.TermString = $"new SelectColumn({currentData.TermString})";

                }
                else if (expression.ColumnType == ColumnType.Wildcard) {
                    currentData.TermString = "~UNKNOWN ColumnReferenceExpression~";
                }
                else {
                    currentData.TermString = "~UNKNOWN ColumnReferenceExpression~";
                }

                currentData.TableName = table;
                currentData.ColumnName = column;
                currentData.Alias = alias;
            }
            catch {
                currentData.TermString = "~ColumnReferenceExpression ERROR~";
            }

            return result;
        }

        public virtual string InPredicateParse(InPredicate expression, object data = null) {
            string result = "";
            Informations currentData = (Informations)data;

            try {
                string searchExpression = "";
                string valuesList = "\"(";


                if (expression.Expression is ColumnReferenceExpression columnReferenceExpression1) {
                    Informations childData = currentData.CopyLite();

                    result += ColumnReferenceExpressionParse(columnReferenceExpression1, childData);
                    searchExpression = childData.SqlExpressionString;
                }
                else if (expression.Expression is IntegerLiteral integerLiteral) {
                    searchExpression = $"SqlExpression.Number({integerLiteral.Value})";
                }
                else if (expression.Expression is StringLiteral stringLiteral) {

                    // check if the string is date
                    if (DateTime.TryParse(stringLiteral.Value, out DateTime date)) {
                        searchExpression = $"SqlExpression.Date(\"{stringLiteral.Value}\")";
                    }
                    else {
                        searchExpression = $"SqlExpression.String(\"{stringLiteral.Value}\")";
                    }
                }
                else if (expression.Expression is NumericLiteral numericLiteral) {
                    searchExpression = $"SqlExpression.Number({numericLiteral.Value})";
                }
                else if (expression.Expression is NullLiteral nullLiteral) {
                    searchExpression = $"SqlExpression.Null()";
                }
                else if (expression.Expression is VariableReference variableReference) {
                    searchExpression = $"SqlExpression.Parameter(\"{variableReference.Name}\")";
                }
                else {
                    searchExpression = "~UNKNOWN ScalarExpression~";
                }

                if (expression.Values.Count > 0) {
                    foreach (ScalarExpression value in expression.Values) {
                        if (valuesList != "\"(") {
                            valuesList += ", ";
                        }

                        if (value is ColumnReferenceExpression columnReferenceExpression2) {
                            Informations childData = currentData.CopyLite();

                            result += ColumnReferenceExpressionParse(columnReferenceExpression2, childData);
                            valuesList += childData.ColumnName;
                        }
                        else if (value is StringLiteral stringLiteral) {
                            valuesList += $"'{stringLiteral.Value}'";
                        }
                        else if (value is IntegerLiteral integerLiteral) {
                            valuesList += integerLiteral.Value;
                        }
                        else if (value is NumericLiteral numericLiteral) {
                            valuesList += numericLiteral.Value;
                        }
                        else if (value is NullLiteral nullLiteral) {
                            valuesList += $"NULL";
                        }
                        else if (value is VariableReference variableReference) {
                            valuesList += $"{variableReference.Name}";
                        }
                        else {
                            valuesList += "~UNKNOWN ScalarExpression~";
                        }
                    }

                    valuesList += ")\"";
                }
                else if (expression.Subquery != null) {
                    Translator translator = new Translator();
                    translator.Data.Level = currentData.Level + 1;
                    translator.FormatOptions.IndentationSize = FormatOptions.IndentationSize;

                    string subQuery = translator.QueryExpressionParse(expression.Subquery.QueryExpression);

                    string variableName = $"query{Informations.NextSubqueryIndex()}";
                    result += $"{translator.Indentation(translator.Data.Level - 1)}string {variableName} = $@\"{subQuery}\";\n";

                    valuesList = variableName;
                }

                if (expression.NotDefined) {
                    currentData.TermString = $"WhereTerm.CreateNotIn({searchExpression}, {valuesList})";
                }
                else {
                    currentData.TermString = $"WhereTerm.CreateIn({searchExpression}, {valuesList})";
                }

            }
            catch {
                currentData.TermString = "~InPredicate ERROR~";
            }

            return result;
        }

        public virtual string LikePredicateParse(LikePredicate expression, object data = null) {
            string result = "";
            Informations currentData = (Informations)data;

            try {
                currentData.TermString = "~SqlOM doesn't supports Like~";
            }
            catch {
                currentData.TermString = "~LikePredicate ERROR~";
            }

            return result;
        }

        public virtual string ExistsPredicateParse(ExistsPredicate expression, object data = null) {
            string result = "";
            Informations currentData = (Informations)data;

            try {
                if (expression.Subquery is ScalarSubquery scalarSubquery) {
                    Translator translator = new Translator();
                    translator.Data.Level = currentData.Level + 1;
                    translator.FormatOptions.IndentationSize = FormatOptions.IndentationSize;

                    string subQuery = translator.QueryExpressionParse(scalarSubquery.QueryExpression);

                    currentData.VariableName = $"query{Informations.NextSubqueryIndex()}";
                    result += $"{translator.Indentation(translator.Data.Level - 1)}string {currentData.VariableName} = $@\"{subQuery}\";\n";
                }
                else {
                    currentData.VariableName = $"~UNKNOWN ScalarSubquery~";
                }

                if (currentData.Not == false) {
                    currentData.TermString = $"WhereTerm.CreateExists({currentData.VariableName})";
                }
                else {
                    currentData.TermString = $"WhereTerm.CreateNotExists({currentData.VariableName})";
                }
            }
            catch {
                currentData.TermString = "~ExistsPredicate ERROR~";
            }

            return result;
        }

        public virtual string NamedTableReferenceParse(NamedTableReference namedTableReference, object data = null) {
            string result = "";
            Informations currentData = (Informations)data;

            try {
                currentData.TableName = namedTableReference.SchemaObject.BaseIdentifier.Value;
                currentData.Alias = namedTableReference.Alias?.Value ?? "";

                // check if the table has an alias
                if (currentData.Alias != "") {
                    currentData.TermString = $"FromTerm {currentData.Alias} = FromTerm.Table(\"{currentData.TableName}\", \"{currentData.Alias}\")";
                }
                else {
                    currentData.TermString = $"FromTerm {currentData.TableName} = FromTerm.Table(\"{currentData.TableName}\")";
                    currentData.Alias = currentData.TableName;
                }
            }
            catch {
                currentData.TermString = "~NamedTableReference ERROR~";
            }

            return result;
        }

        public virtual string QualifiedJoinParse(QualifiedJoin expression, object data = null) {
            string result = "";
            Informations currentData = (Informations)data;

            try {
                string searchCondition = "";
                string qualifiedJoinType;

                // left table
                if (expression.FirstTableReference is NamedTableReference namedTableReference1) {
                    Informations childData = currentData.CopyLite();

                    result += NamedTableReferenceParse(namedTableReference1, childData);
                    result += $"{Indentation(currentData.Level)}{childData.TermString};\n";
                    currentData.LeftTableName = childData.Alias;
                }
                else if (expression.FirstTableReference is QueryDerivedTable queryDerivedTable1) {
                    Informations childData = currentData.CopyLite();
                    string alias = queryDerivedTable1.Alias.Value;

                    result += QueryExpressionParse(queryDerivedTable1.QueryExpression, childData);
                    result += $"{Indentation(currentData.Level)}FromTerm {alias} = FromTerm.SubQuery({childData.VariableName}, \"{alias}\");\n";
                    currentData.LeftTableName = alias;
                }
                else if (expression.FirstTableReference is QualifiedJoin qualifiedJoin1) {
                    Informations childData = currentData.CopyLite();

                    result += QualifiedJoinParse(qualifiedJoin1, childData);
                    currentData.LeftTableName = childData.LeftTableName;
                }
                else {
                    currentData.LeftTableName = $"~UNKNOWN_TABLE~";
                }

                // right table
                if (expression.SecondTableReference is NamedTableReference namedTableReference2) {
                    Informations childData = currentData.CopyLite();

                    result += NamedTableReferenceParse(namedTableReference2, childData);
                    result += $"{Indentation(currentData.Level)}{childData.TermString};\n";
                    currentData.RightTableName = childData.Alias;
                }
                else if (expression.SecondTableReference is QueryDerivedTable queryDerivedTable2) {
                    Informations childData = currentData.CopyLite();
                    string alias = queryDerivedTable2.Alias.Value;

                    result += QueryExpressionParse(queryDerivedTable2.QueryExpression, childData);
                    result += $"{Indentation(currentData.Level)}FromTerm {alias} = FromTerm.SubQuery({childData.VariableName}, \"{alias}\");\n";
                    currentData.RightTableName = alias;
                }
                else if (expression.SecondTableReference is QualifiedJoin qualifiedJoin2) {
                    currentData.RightTableName = $"~UNDEVELOPED QualifiedJoin~";
                }
                else {
                    currentData.RightTableName = $"UNKNOWN_TABLE";
                }

                // search condition
                if (expression.SearchCondition is BooleanComparisonExpression booleanComparisonExpression) {
                    Informations childData = currentData.CopyLite();
                    childData.Level++;

                    string whereClause = $"whereClause{Informations.NextWhereClauseIndex()}";
                    result += BooleanComparisonExpressionParse(booleanComparisonExpression, childData);
                    result += $"{Indentation(childData.Level)}WhereClause {whereClause} = new WhereClause(WhereClauseRelationship.And);\n";
                    result += $"{Indentation(childData.Level)}{whereClause}.Terms.Add({childData.TermString});\n";
                    searchCondition = whereClause;
                }
                else if (expression.SearchCondition is BooleanBinaryExpression booleanBinaryExpression) {
                    Informations childData = currentData.CopyLite();

                    result += BooleanBinaryExpressionParse(booleanBinaryExpression, childData);
                    searchCondition = $"{childData.VariableName}";
                }
                else if (expression.SearchCondition is BooleanParenthesisExpression booleanParenthesisExpression) {
                    Informations childData = currentData.CopyLite();

                    result += BooleanParenthesisExpressionParse(booleanParenthesisExpression, childData);

                    if (childData.VariableName != "") {
                        searchCondition = childData.VariableName;
                    }
                    else {
                        childData.Level++;
                        string whereClause = $"whereClause{Informations.NextWhereClauseIndex()}";
                        result += $"{Indentation(childData.Level)}WhereClause {whereClause} = new WhereClause(WhereClauseRelationship.And);\n";
                        result += $"{Indentation(childData.Level)}{whereClause}.Terms.Add({childData.TermString});\n";
                        searchCondition = whereClause;
                    }
                }
                else if (expression.SearchCondition is BooleanNotExpression booleanNotExpression) {
                    Informations childData = currentData.CopyLite();
                    childData.Level++;

                    string whereClause = $"whereClause{Informations.NextWhereClauseIndex()}";
                    result += $"{Indentation(childData.Level)}WhereClause {whereClause} = new WhereClause(WhereClauseRelationship.And);\n";
                    result += BooleanNotExpressionParse(booleanNotExpression, childData);
                    result += $"{Indentation(childData.Level)}{whereClause}.Terms.Add({childData.TermString});\n";

                    searchCondition = whereClause;
                }
                else if (expression.SearchCondition is BooleanTernaryExpression booleanTernaryExpression) {
                    Informations childData = currentData.CopyLite();
                    childData.Level++;

                    string whereClause = $"whereClause{Informations.NextWhereClauseIndex()}";
                    result += $"{Indentation(childData.Level)}WhereClause {whereClause} = new WhereClause(WhereClauseRelationship.And);\n";
                    result += BooleanTernaryExpressionParse(booleanTernaryExpression, childData);
                    result += $"{Indentation(childData.Level)}{whereClause}.Terms.Add({childData.TermString});\n";

                    searchCondition = whereClause;
                }
                else if (expression.SearchCondition is BooleanIsNullExpression booleanIsNullExpression) {
                    Informations childData = currentData.CopyLite();
                    childData.Level++;

                    string whereClause = $"whereClause{Informations.NextWhereClauseIndex()}";
                    result += $"{Indentation(childData.Level)}WhereClause {whereClause} = new WhereClause(WhereClauseRelationship.And);\n";
                    result += BooleanIsNullExpressionParse(booleanIsNullExpression, childData);
                    result += $"{Indentation(childData.Level)}{whereClause}.Terms.Add({childData.TermString});\n";

                    searchCondition = whereClause;
                }
                else if (expression.SearchCondition is InPredicate inPredicate) {
                    Informations childData = currentData.CopyLite();
                    childData.Level++;

                    string whereClause = $"whereClause{Informations.NextWhereClauseIndex()}";
                    result = $"{Indentation(childData.Level)}WhereClause {whereClause} = new WhereClause(WhereClauseRelationship.And);\n";
                    result += InPredicateParse(inPredicate, childData);
                    result += $"{Indentation(childData.Level)}{whereClause}.Terms.Add({childData.TermString});\n";
                    searchCondition = whereClause;
                }
                else if (expression.SearchCondition is LikePredicate likePredicate) {
                    Informations childData = currentData.CopyLite();
                    childData.Level++;

                    string whereClause = $"whereClause{Informations.NextWhereClauseIndex()}";
                    result += $"{Indentation(childData.Level)}WhereClause {whereClause} = new WhereClause(WhereClauseRelationship.And);\n";
                    result += LikePredicateParse(likePredicate, childData);
                    result += $"{Indentation(childData.Level)}{whereClause}.Terms.Add({childData.TermString});\n";
                    searchCondition = whereClause;
                }
                else {
                    searchCondition = "~UNKNOWN BooleanExpression~";
                }

                // join type
                if (expression.QualifiedJoinType == QualifiedJoinType.FullOuter) {
                    qualifiedJoinType = "JoinType.Full";
                }
                else if (expression.QualifiedJoinType == QualifiedJoinType.Inner) {
                    qualifiedJoinType = "JoinType.Inner";
                }
                else if (expression.QualifiedJoinType == QualifiedJoinType.LeftOuter) {
                    qualifiedJoinType = "JoinType.Left";
                }
                else if (expression.QualifiedJoinType == QualifiedJoinType.RightOuter) {
                    qualifiedJoinType = "JoinType.Right";
                }
                else {
                    qualifiedJoinType = "~UNKNOWN QualifiedJoinType~";
                }

                result += $"{Indentation(currentData.Level)}{currentData.BelongsTo.VariableName}.FromClause.Join({qualifiedJoinType}, {currentData.LeftTableName}, {currentData.RightTableName}, {searchCondition});\n";
            }
            catch {
                result = "~QualifiedJoin ERROR~";
            }

            return result;
        }

        public virtual string SelectStarExpression(SelectStarExpression selectStarExpression, object data = null) {
            string result = "";
            Informations currentData = (Informations)data;

            try {
                string column = "*";
                string table = "";

                // star expression with table identifier
                if (selectStarExpression.Qualifier != null) {
                    if (selectStarExpression.Qualifier.Identifiers.Count > 0) {
                        table = selectStarExpression.Qualifier.Identifiers[0].Value;
                    }
                }

                if (table != "") {
                    currentData.TermString = $"\"{column}\", {table}";
                }
                else {
                    currentData.TermString = $"\"{column}\"";
                }

                currentData.TermString = $"new SelectColumn({currentData.TermString})";

            }
            catch {
                currentData.TermString = "~SelectStarExpression ERROR~";
            }

            return result;
        }

        public virtual string ExpressionGroupingSpecificationParse(ExpressionGroupingSpecification expression, object data = null) {
            string result = "";
            Informations currentData = (Informations)data;

            try {
                if (expression.Expression is ColumnReferenceExpression columnReferenceExpression) {
                    Informations childData = currentData.CopyLite();

                    result += ColumnReferenceExpressionParse(columnReferenceExpression, childData);

                    if (childData.TableName != "") {
                        currentData.TermString = $"new GroupByTerm(\"{childData.ColumnName}\", {childData.TableName})";
                    }
                    else {
                        currentData.TermString = $"new GroupByTerm(\"{childData.ColumnName}\")";
                    }
                    currentData.ColumnName = childData.ColumnName;
                    currentData.TableName = childData.TableName;
                }
            }
            catch {
                result = "~ExpressionGroupingSpecification ERROR~";
            }

            return result;
        }

        public virtual string SearchedCaseExpressionParse(SearchedCaseExpression expression, object data = null) {
            string result;
            Informations currentData = (Informations)data;

            try {
                string elseExpression;

                currentData.VariableName = $"caseClause{Informations.NextCaseClauseIndex()}";
                currentData.Level++;

                result = $"{Indentation(currentData.Level)}CaseClause {currentData.VariableName} = new CaseClause();\n";

                if (expression.ElseExpression is ColumnReferenceExpression columnReferenceExpression1) {
                    Informations childData = currentData.CopyLite();

                    result += ColumnReferenceExpressionParse(columnReferenceExpression1, childData);
                    elseExpression = childData.SqlExpressionString;
                }
                else if (expression.ElseExpression is IntegerLiteral integerLiteral) {
                    elseExpression = $"SqlExpression.Number({integerLiteral.Value})";
                }
                else if (expression.ElseExpression is StringLiteral stringLiteral) {

                    // check if the string is date
                    if (DateTime.TryParse(stringLiteral.Value, out DateTime date)) {
                        elseExpression = $"SqlExpression.Date(\"{stringLiteral.Value}\")";
                    }
                    else {
                        elseExpression = $"SqlExpression.String(\"{stringLiteral.Value}\")";
                    }
                }
                else if (expression.ElseExpression is NumericLiteral numericLiteral) {
                    elseExpression = $"SqlExpression.Number({numericLiteral.Value})";
                }
                else if (expression.ElseExpression is NullLiteral nullLiteral) {
                    elseExpression = $"SqlExpression.Null()";
                }
                else if (expression.ElseExpression is VariableReference variableReference) {
                    elseExpression = $"SqlExpression.Parameter(\"{variableReference.Name}\")";
                }
                else if (expression.ElseExpression is CastCall castCall) {
                    Informations childData = currentData.CopyLite();

                    result += CastCallParse(castCall, childData);
                    elseExpression = childData.TermString;
                } 
                else if (expression.ElseExpression is UnaryExpression unaryExpression) {
                    Informations childData = currentData.CopyLite();

                    result += UnaryExpressionParse(unaryExpression, childData);
                    elseExpression = childData.TermString;
                } 
                else {
                    elseExpression = "~UNKNOWN ScalarExpression~";
                }

                result += $"{Indentation(currentData.Level)}{currentData.VariableName}.ElseValue = {elseExpression};\n";


                foreach (SearchedWhenClause whenClause in expression.WhenClauses) {
                    Informations childData = currentData.CopyLite();

                    if (whenClause is SearchedWhenClause searchedWhenClause) {
                        result += SearchedWhenClauseParse(searchedWhenClause, childData);
                        result += $"{Indentation(currentData.Level)}{currentData.VariableName}.Terms.Add({childData.TermString});\n";
                    }
                    else {
                        result += $"{Indentation(currentData.Level)}{currentData.VariableName}.Terms.Add(~UNKNOWN SearchedWhenClause~);\n";
                    }
                }

                currentData.SqlExpressionString = $"SqlExpression.Case({currentData.VariableName})";
                currentData.TermString = $"new SelectColumn(SqlExpression.Case({currentData.VariableName}), \"{currentData.Alias}\")";
            }
            catch {
                result = "~SearchedCaseExpression ERROR~";
            }

            return result;
        }

        public virtual string UnaryExpressionParse(UnaryExpression expression, object data = null) {
            string result = "";
            Informations currentData = (Informations)data;

            string pros = "";

            if (expression.UnaryExpressionType == UnaryExpressionType.Negative) {
                pros = $"-";
            }

            try {
                if (expression.Expression is IntegerLiteral integerLiteral) {
                    currentData.TermString = $"SqlExpression.Number({pros}{integerLiteral.Value})";
                } 
                else if (expression.Expression is StringLiteral stringLiteral) {

                    // check if the string is date
                    if (DateTime.TryParse(stringLiteral.Value, out DateTime date)) {
                        currentData.TermString = $"SqlExpression.Date(\"{stringLiteral.Value}\")";
                    } 
                    else {
                        currentData.TermString = $"SqlExpression.String(\"{stringLiteral.Value}\")";
                    }
                } 
                else if (expression.Expression is NumericLiteral numericLiteral) {
                    currentData.TermString = $"SqlExpression.Number({pros}{numericLiteral.Value})";
                } 
                else if (expression.Expression is NullLiteral nullLiteral) {
                    currentData.TermString = $"SqlExpression.Null()";
                } 
                else if (expression.Expression is VariableReference variableReference) {
                    currentData.TermString = $"SqlExpression.Parameter(\"{variableReference.Name}\")";
                }
            } catch {
                currentData.TermString = "~UnaryExpressionParse ERROR~";
            }

            return result;
        }

        public virtual string SearchedWhenClauseParse(SearchedWhenClause expression, object data = null) {
            string result = "";
            Informations currentData = (Informations)data;

            try {
                string sqlExpression = "";
                string whereClause = "";

                if (expression.WhenExpression is BooleanComparisonExpression booleanComparisonExpression) {
                    Informations childData = currentData.CopyLite();
                    childData.Level++;

                    whereClause = $"whereClause{Informations.NextWhereClauseIndex()}";
                    result += $"{Indentation(childData.Level)}WhereClause {whereClause} = new WhereClause(WhereClauseRelationship.And);\n";
                    result += BooleanComparisonExpressionParse(booleanComparisonExpression, childData);
                    result += $"{Indentation(childData.Level)}{whereClause}.Terms.Add({childData.TermString});\n";
                }
                else if (expression.WhenExpression is BooleanBinaryExpression booleanBinaryExpression) {
                    Informations childData = currentData.CopyLite();

                    result += BooleanBinaryExpressionParse(booleanBinaryExpression, childData);
                    whereClause = childData.VariableName;
                }
                else if (expression.WhenExpression is BooleanParenthesisExpression booleanParenthesisExpression) {
                    Informations childData = currentData.CopyLite();
                    childData.Level++;

                    whereClause = $"whereClause{Informations.NextWhereClauseIndex()}";
                    result += $"{Indentation(childData.Level)}WhereClause {whereClause} = new WhereClause(WhereClauseRelationship.And);\n";
                    result += BooleanParenthesisExpressionParse(booleanParenthesisExpression, childData);
                    if (childData.VariableName != "") {
                        result += $"{Indentation(childData.Level)}{whereClause}.WherePhrase.SubClauses.Add({childData.VariableName});\n";
                    }
                    else {
                        result += $"{Indentation(childData.Level)}{whereClause}.Terms.Add({childData.TermString});\n";
                    }
                }
                else if (expression.WhenExpression is BooleanNotExpression booleanNotExpression) {
                    Informations childData = currentData.CopyLite();
                    childData.Level++;

                    whereClause = $"whereClause{Informations.NextWhereClauseIndex()}";
                    result += $"{Indentation(childData.Level)}WhereClause {whereClause} = new WhereClause(WhereClauseRelationship.And);\n";
                    result += BooleanNotExpressionParse(booleanNotExpression, childData);
                    result += $"{Indentation(childData.Level)}{whereClause}.Terms.Add({childData.TermString});\n";
                }
                else if (expression.WhenExpression is BooleanTernaryExpression booleanTernaryExpression) {
                    Informations childData = currentData.CopyLite();
                    childData.Level++;

                    whereClause = $"whereClause{Informations.NextWhereClauseIndex()}";
                    result += $"{Indentation(childData.Level)}WhereClause {whereClause} = new WhereClause(WhereClauseRelationship.And);\n";
                    result += BooleanTernaryExpressionParse(booleanTernaryExpression, childData);
                    result += $"{Indentation(childData.Level)}{whereClause}.Terms.Add({childData.TermString});\n";
                }
                else if (expression.WhenExpression is BooleanIsNullExpression booleanIsNullExpression) {
                    Informations childData = currentData.CopyLite();
                    childData.Level++;

                    whereClause = $"whereClause{Informations.NextWhereClauseIndex()}";
                    result += $"{Indentation(childData.Level)}WhereClause {whereClause} = new WhereClause(WhereClauseRelationship.And);\n";
                    result += BooleanIsNullExpressionParse(booleanIsNullExpression, childData);
                    result += $"{Indentation(childData.Level)}{whereClause}.Terms.Add({childData.TermString});\n";
                }
                else if (expression.WhenExpression is InPredicate inPredicate) {
                    Informations childData = currentData.CopyLite();
                    childData.Level++;

                    whereClause = $"whereClause{Informations.NextWhereClauseIndex()}";
                    result += $"{Indentation(childData.Level)}WhereClause {whereClause} = new WhereClause(WhereClauseRelationship.And);\n";
                    result += InPredicateParse(inPredicate, childData);
                    result += $"{Indentation(childData.Level)}{whereClause}.Terms.Add({childData.TermString});\n";
                }
                else if (expression.WhenExpression is LikePredicate likePredicate) {
                    Informations childData = currentData.CopyLite();
                    childData.Level++;

                    whereClause = $"whereClause{Informations.NextWhereClauseIndex()}";
                    result += $"{Indentation(childData.Level)}WhereClause {whereClause} = new WhereClause(WhereClauseRelationship.And);\n";
                    result += LikePredicateParse(likePredicate, childData);
                    result += $"{Indentation(childData.Level)}{whereClause}.Terms.Add({childData.TermString});\n";
                }
                else {
                    whereClause = "~UNKNOWN BooleanExpression~";
                }


                if (expression.ThenExpression is ColumnReferenceExpression columnReferenceExpression1) {
                    Informations childData = currentData.CopyLite();

                    result += ColumnReferenceExpressionParse(columnReferenceExpression1, childData);
                    sqlExpression = childData.SqlExpressionString;
                }
                else if (expression.ThenExpression is IntegerLiteral integerLiteral) {
                    sqlExpression = $"SqlExpression.Number({integerLiteral.Value})";
                }
                else if (expression.ThenExpression is StringLiteral stringLiteral) {

                    // check if the string is date
                    if (DateTime.TryParse(stringLiteral.Value, out DateTime date)) {
                        sqlExpression = $"SqlExpression.Date(\"{stringLiteral.Value}\")";
                    }
                    else {
                        sqlExpression = $"SqlExpression.String(\"{stringLiteral.Value}\")";
                    }
                }
                else if (expression.ThenExpression is NumericLiteral numericLiteral) {
                    sqlExpression = $"SqlExpression.Number({numericLiteral.Value})";
                }
                else if (expression.ThenExpression is NullLiteral nullLiteral) {
                    sqlExpression = $"SqlExpression.Null()";
                }
                else if (expression.ThenExpression is VariableReference variableReference) {
                    sqlExpression = $"SqlExpression.Parameter(\"{variableReference.Name}\")";
                }
                else if (expression.ThenExpression is CastCall castCall) {
                    sqlExpression = $"SqlExpression.Parameter(~SqlOM doesnt support CastCall~)";
                }
                else {
                    sqlExpression = "~UNKNOWN ScalarExpression~";
                }

                currentData.TermString = $"new CaseTerm({whereClause}, {sqlExpression})";
            }
            catch {
                currentData.TermString = "~SearchedWhenClause ERROR~";
            }

            return result;
        }

        public virtual string CastCallParse(CastCall expression, object data = null) {
            string result = "";
            Informations currentData = (Informations)data;

            try {
                currentData.TermString = "~SqlOM doesn't supports Cast~";
            }
            catch {
                currentData.TermString = "~CastCall ERROR~";
            }

            return result;
        }

        public virtual string ExpressionWithSortOrderParse(ExpressionWithSortOrder expression, object data = null) {
            string result = "";
            Informations currentData = (Informations)data;

            try {

                string sortOrder = "";
                if (expression.SortOrder == SortOrder.Ascending) {
                    sortOrder = "OrderByDirection.Ascending";
                }
                else if (expression.SortOrder == SortOrder.Descending) {
                    sortOrder = "OrderByDirection.Descending";
                }
                else {
                    sortOrder = "OrderByDirection.Ascending";
                }

                if (expression.Expression is ColumnReferenceExpression columnReferenceExpression) {
                    Informations childData = currentData.CopyLite();

                    result += ColumnReferenceExpressionParse(columnReferenceExpression, childData);

                    if (childData.TableName != "") {
                        currentData.TermString = $"new OrderByTerm(\"{childData.ColumnName}\", {childData.TableName}, {sortOrder})";
                    }
                    else {
                        currentData.TermString = $"new OrderByTerm(\"{childData.ColumnName}\", {sortOrder})";
                    }
                    currentData.ColumnName = childData.ColumnName;
                    currentData.TableName = childData.TableName;
                    currentData.Alias = childData.Alias;
                }
                else {
                    currentData.TermString = "~UKNOWN ScalarExpression~";
                }
            }
            catch {
                currentData.TermString = "~ExpressionWithSortOrder ERROR~";
            }

            return result;
        }

        public virtual string DataTypeParse(DataTypeReference expression, object data = null) {
            string result = "";
            Informations currentData = (Informations)data;

            try {
                currentData.TermString = "~UNDEVELOPED DataTypeReference~";
            }
            catch {
                currentData.TermString = "~DataTypeReference ERROR~";
            }

            return result;
        }

        public virtual string ConstraintDefinitionParse(ConstraintDefinition expression, object data = null) {
            string result = "";
            Informations currentData = (Informations)data;

            try {
                currentData.TermString = "~UNDEVELOPED ConstraintDefinition~";
            }
            catch {
                currentData.TermString = "~ConstraintDefinition ERROR~";
            }

            return result;
        }

        public virtual string AlterTableAddTableElementStatementParse(AlterTableAddTableElementStatement expression, object data = null) {
            string result = "";
            Informations currentData = (Informations)data;

            try {
                currentData.TermString = "~UNDEVELOPED AlterTableAddTableElementStatement~";
            }
            catch {
                currentData.TermString = "~AlterTableAddTableElementStatement ERROR~";
            }

            return result;
        }

        public virtual string ColumnDefinitionParse(ColumnDefinition expression, object data = null) {
            string result = "";
            Informations currentData = (Informations)data;

            try {
                currentData.TermString = "~UNDEVELOPED ColumnDefinition~";
            }
            catch {
                currentData.TermString = "~ColumnDefinition ERROR~";
            }

            return result;
        }

        public virtual string DropTableStatementParse(DropTableStatement expression, object data = null) {
            string result = "";
            Informations currentData = (Informations)data;

            try {
                currentData.TermString = "~UNDEVELOPED DropTableStatement~";
            } catch {
                currentData.TermString = "~DropTableStatement ERROR~";
            }

            return result;
        }

        #endregion

    }

}
