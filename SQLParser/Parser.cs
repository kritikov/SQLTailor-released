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

    /// <summary>
    /// Parser reads a SQL query and produces a tree with components using the ScriptDom library.
    /// The tree contains the tokens found in the query with the form of the many classes of ScriptDom.
    /// The parser then uses the translator classes to trasform the tree back to SQL or some other script that produces SQL.
    /// </summary>
    public class Parser {

        #region VARIABLES AND NESTED CLASSES

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

        #endregion


        #region CONSTRUCTORS

        public Parser() {

        }

        public Parser(string sqlSource) {
            this.SQLSource = sqlSource;
        }

        #endregion


        #region METHODS

        /// <summary>
        /// Gets a SQL query in text form and returns a tree with components of ScriptDom
        /// </summary>
        /// <param name="procText"></param>
        /// <returns></returns>
        private static (TSqlFragment sqlTree, IList<ParseError> errors) ProcessSql(string procText) {
            TSql150Parser parser = new TSql150Parser(false); // false tells to parser whether you have quoted identifiers on or not

            using (StringReader textReader = new StringReader(procText)) {
                TSqlFragment sqlTree = parser.Parse(textReader, out IList<ParseError> errors);

                return (sqlTree, errors);
            }
        }

        /// <summary>
        /// Read the sql script and parse it all the available forms
        /// </summary>
        public void Process() {
            (TSqlFragment sqlTree, IList<ParseError> errors) processed = ProcessSql(this.sqlSource);

            sqlTree = processed.sqlTree;
            errors = (List<ParseError>)processed.errors;
        }

        /// <summary>
        /// Writes the tokens of the script in a formatted list to display it with a grid grid
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
        /// Get the formatted SQL using the native method of the ScriptDom libary
        /// </summary>
        /// <returns></returns>
        public string GetTSQL() {
            Sql150ScriptGenerator scriptGenerator = new Sql150ScriptGenerator(options);
            scriptGenerator.GenerateScript(sqlTree, out string result);

            //options.SqlEngineType = SqlEngineType.Standalone;
            //options.SqlVersion = SqlVersion.Sql100;

            return result;
        }

        /// <summary>
        /// Add colors to SQL query using the settings of the Translator class
        /// </summary>
        /// <returns></returns>
        public FlowDocument GetTSQLAsFlowDocument() {
            string text = GetTSQL();

            // use the formation of the Translator
            BaseTranslator translator = new BaseTranslator(this.Options);
            return translator.GetFlowDocument(text);
        }

        /// <summary>
        /// Transform the sql tree into plain text using the basic Translator
        /// </summary>
        /// <returns></returns>
        public string GetBaseTranslation() {
            string result = "";

            Visitor visitor = new Visitor();
            sqlTree.Accept(visitor);

            if (Options.UpdateQueryParametersList) {
                Options.QueryParameters.Clear();
            }

            BaseTranslator translator = new BaseTranslator(Options);

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
                else if (statement is DropTableStatement dropTableStatement) {
                    result += translator.DropTableStatementParse(dropTableStatement);
                }
            }

            return result;
        }

        /// <summary>
        /// Transform the sql tree into a formatted text using the basic Translator
        /// </summary>
        /// <returns></returns>
        public FlowDocument GetBaseTranslationAsFlowDocument() {
            string result = GetBaseTranslation();
            BaseTranslator translator = new BaseTranslator(Options);
            return translator.GetFlowDocument(result);
        }

        /// <summary>
        /// Create the script in SqlOM and C# that produces the given SQL query using the SqlOMTranslator. The result is a plain text.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Create the script in SqlOM and C# that produces the given SQL query using the SqlOMTranslator. The result is a formatted text.
        /// </summary>
        /// <returns></returns>
        public FlowDocument GetSqlOMAsFlowDocument() {
            string result = GetSqlOM();
            SqlOMTranslator translator = new SqlOMTranslator(Options);
            return translator.GetFlowDocument(result);
        }

        /// <summary>
        /// Create the script in PoVariation and C# that produces the given SQL query using the PoVariationTranslator.
        /// The PoVariationTranslator is a simple inheritance from SqlOMTranslator and just adds the prefix 'po' before some objects.
        /// The result is a plain text.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Create the script in PoVariation and C# that produces the given SQL query using the PoVariationTranslator.
        /// The PoVariationTranslator is a simple inheritance from SqlOMTranslator and just adds the prefix 'po' before some objects.
        /// The result is a formatted text.
        /// </summary>
        /// <returns></returns>
        public FlowDocument GetPoVariationAsFlowDocument() {
            string result = GetPoVariation();
            PoVariationTranslator translator = new PoVariationTranslator(Options);
            return translator.GetFlowDocument(result);
        }

        /// <summary>
        /// Transform the sql tree into plain text using the basic Translator
        /// </summary>
        /// <returns></returns>
        public string GetMSSQLTranslation() {
            string result = "";

            Visitor visitor = new Visitor();
            sqlTree.Accept(visitor);

            if (Options.UpdateQueryParametersList) {
                Options.QueryParameters.Clear();
            }

            MSSQLTranslator translator = new MSSQLTranslator(Options);

            foreach (TSqlFragment statement in visitor.Statements) {
                if (statement is SelectStatement selectStatement) {
                    result += translator.SelectStatementParse(selectStatement);
                } else if (statement is InsertStatement insertStatement) {
                    result += translator.InsertSpecificationParse(insertStatement.InsertSpecification);
                } else if (statement is UpdateStatement updateStatement) {
                    result += translator.UpdateSpecificationParse(updateStatement.UpdateSpecification);
                } else if (statement is DeleteStatement deleteStatement) {
                    result += translator.DeleteSpecificationParse(deleteStatement.DeleteSpecification);
                } else if (statement is CreateTableStatement createTableStatement) {
                    result += translator.CreateTableStatementParse(createTableStatement);
                } else if (statement is CreateViewStatement createViewStatement) {
                    result += translator.CreateViewStatementParse(createViewStatement);
                } else if (statement is AlterTableAddTableElementStatement alterTableAddTableElementStatement) {
                    result += translator.AlterTableAddTableElementStatementParse(alterTableAddTableElementStatement);
                } else if (statement is AlterTableAlterColumnStatement alterTableAlterColumnStatement) {
                    result += translator.AlterTableAlterColumnStatementParse(alterTableAlterColumnStatement);
                } else if (statement is DropTableStatement dropTableStatement) {
                    result += translator.DropTableStatementParse(dropTableStatement);
                }
            }

            return result;
        }

        /// <summary>
        /// Transform the sql tree into a formatted text using the basic Translator
        /// </summary>
        /// <returns></returns>
        public FlowDocument GetMSSQLTranslationAsFlowDocument() {
            string result = GetMSSQLTranslation();
            MSSQLTranslator translator = new MSSQLTranslator(Options);
            return translator.GetFlowDocument(result);
        }

        /// <summary>
        /// Transform the sql tree into plain text using the basic Translator
        /// </summary>
        /// <returns></returns>
        public string GetMySQLTranslation() {
            string result = "";

            Visitor visitor = new Visitor();
            sqlTree.Accept(visitor);

            if (Options.UpdateQueryParametersList) {
                Options.QueryParameters.Clear();
            }

            MySQLTranslator translator = new MySQLTranslator(Options);

            foreach (TSqlFragment statement in visitor.Statements) {
                if (statement is SelectStatement selectStatement) {
                    result += translator.SelectStatementParse(selectStatement);
                } else if (statement is InsertStatement insertStatement) {
                    result += translator.InsertSpecificationParse(insertStatement.InsertSpecification);
                } else if (statement is UpdateStatement updateStatement) {
                    result += translator.UpdateSpecificationParse(updateStatement.UpdateSpecification);
                } else if (statement is DeleteStatement deleteStatement) {
                    result += translator.DeleteSpecificationParse(deleteStatement.DeleteSpecification);
                } else if (statement is CreateTableStatement createTableStatement) {
                    result += translator.CreateTableStatementParse(createTableStatement);
                } else if (statement is CreateViewStatement createViewStatement) {
                    result += translator.CreateViewStatementParse(createViewStatement);
                } else if (statement is AlterTableAddTableElementStatement alterTableAddTableElementStatement) {
                    result += translator.AlterTableAddTableElementStatementParse(alterTableAddTableElementStatement);
                } else if (statement is AlterTableAlterColumnStatement alterTableAlterColumnStatement) {
                    result += translator.AlterTableAlterColumnStatementParse(alterTableAlterColumnStatement);
                } else if (statement is DropTableStatement dropTableStatement) {
                    result += translator.DropTableStatementParse(dropTableStatement);
                }
            }

            return result;
        }

        /// <summary>
        /// Transform the sql tree into a formatted text using the basic Translator
        /// </summary>
        /// <returns></returns>
        public FlowDocument GetMySQLTranslationAsFlowDocument() {
            string result = GetMySQLTranslation();
            MySQLTranslator translator = new MySQLTranslator(Options);
            return translator.GetFlowDocument(result);
        }
        #endregion

    }


}
