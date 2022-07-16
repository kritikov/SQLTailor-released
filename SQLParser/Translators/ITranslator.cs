using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.Windows.Documents;

namespace SQLParser.Translators {
    /// <summary>
    /// use this interface to create new translators from the sql tree to any form
    /// </summary>
    public interface ITranslator {

        /// <summary>
        /// reset all properties to default values before reparse an slq tree
        /// </summary>
        void Reset();

        /// <summary>
        /// Returns the parsed SQL statement as a Flow Document with formatted elements
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        FlowDocument GetFlowDocument(string text);

        /// <summary>
        /// Translates an insert query expression into a formatted string
        /// </summary>
        /// <param name="specification"></param>
        /// <returns></returns>
        string InsertSpecificationParse(InsertSpecification expression, object data = null);

        /// <summary>
        /// Translates an update query expression into a formatted string
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        string UpdateSpecificationParse(UpdateSpecification expression, object data = null);

        /// <summary>
        /// Translates a delete query expression into a formatted string
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        string DeleteSpecificationParse(DeleteSpecification expression, object data = null);

        /// <summary>
        /// Translates a create table expression into a formatted string
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        string CreateTableStatementParse(CreateTableStatement expression, object data = null);

        /// <summary>
        /// Translates a create view expression into a formatted string
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        string CreateViewStatementParse(CreateViewStatement expression, object data = null);

        /// <summary>
        /// Translates a select query expression into a formatted string
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        string SelectStatementParse(SelectStatement expression, object data = null);

        /// <summary>
        /// Translates a select query expression into a formatted string
        /// </summary>
        /// <param name="queryExpression"></param>
        /// <param name="isSubQuery"></param>
        /// <returns></returns>
        string QueryExpressionParse(QueryExpression queryExpression, object data = null);

        /// <summary>
        /// Translates the UNION, EXCEPTS and INTERSECT
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        string BinaryQueryExpressionParse(BinaryQueryExpression expression, object data = null);

        /// <summary>
        /// Translates a select query specification into a formatted string
        /// </summary>
        /// <param name="queryExpression"></param>
        /// <param name="subQuery"></param>
        /// <returns></returns>
        string QuerySpecificationParse(QuerySpecification querySpecification, object data = null);

        /// <summary>
        /// Translates a function call into a formatted string
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        string FunctionCallParse(FunctionCall expression, object data = null);

        /// <summary>
        /// Translates a parenthesis that contains a Select query
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="informations"></param>
        /// <returns></returns>
        string QueryParenthesisExpressionParse(QueryParenthesisExpression expression, object data = null);

        /// <summary>
        /// Translates a parenthesis that contains a Column
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        string ParenthesisExpressionParse(ParenthesisExpression expression, object data = null);

        /// <summary>
        /// Translates the expressions like 'a * b'
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        string BinaryExpressionParse(BinaryExpression expression, object data = null);

        /// <summary>
        /// Translates the expressions like 'a AND b'.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        string BooleanBinaryExpressionParse(BooleanBinaryExpression expression, object data = null);

        /// <summary>
        /// Translates the expressions like 'a <= b'.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        string BooleanComparisonExpressionParse(BooleanComparisonExpression expression, object data = null);

        /// <summary>
        /// Translates the expressions like 'a IS NULL'.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        string BooleanIsNullExpressionParse(BooleanIsNullExpression expression, object data = null);

        /// <summary>
        /// Translates the expressions like 'NOT a'.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        string BooleanNotExpressionParse(BooleanNotExpression expression, object data = null);

        /// <summary>
        /// Translates the expressions in parenthesis like '(a = b)' in the Where section. 
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        string BooleanParenthesisExpressionParse(BooleanParenthesisExpression expression, object data = null);

        /// <summary>
        /// Translates the between expressions like 'a BETWEEN 1 AND 5'.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        string BooleanTernaryExpressionParse(BooleanTernaryExpression expression, object data = null);

        /// <summary>
        /// Translates the name of a Column
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        string ColumnReferenceExpressionParse(ColumnReferenceExpression expression, object data = null);

        /// <summary>
        /// Translates the expressions like 'a IN (1, 2, 4)'.
        /// </summary>
        string InPredicateParse(InPredicate expression, object data = null);

        /// <summary>
        /// Translates the expressions with LIKE operator.
        /// </summary>
        string LikePredicateParse(LikePredicate expression, object data = null);

        /// <summary>
        /// Translates the expressions with EXISTS operator. 
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        string ExistsPredicateParse(ExistsPredicate expression, object data = null);

        /// <summary>
        /// Translates the name of a table
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        string NamedTableReferenceParse(NamedTableReference namedTableReference, object data = null);

        /// <summary>
        /// Translates a JOIN condition
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        string QualifiedJoinParse(QualifiedJoin expression, object data = null);

        /// <summary>
        /// Translates the * selector
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        string SelectStarExpression(SelectStarExpression selectStarExpression, object data = null);

        /// <summary>
        /// Translates a GROUP BY condition
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        string ExpressionGroupingSpecificationParse(ExpressionGroupingSpecification expression, object data = null);

        /// <summary>
        /// Translates a CASE condition
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        string SearchedCaseExpressionParse(SearchedCaseExpression expression, object data = null);

        /// <summary>
        /// Translates the Unary expressions as -1
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        string UnaryExpressionParse(UnaryExpression expression, object data = null);

        /// <summary>
        /// Translates a search condition in CASE
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        string SearchedWhenClauseParse(SearchedWhenClause expression, object data = null);

        /// <summary>
        /// Translates a CAST into a string
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        string CastCallParse(CastCall expression, object data = null);

        /// <summary>
        /// Translates a CONVERT into a string
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        string ConvertCallParse(ConvertCall expression, object data = null);

        /// <summary>
        /// Translates an ORDER BY expression into a formatted string
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        string ExpressionWithSortOrderParse(ExpressionWithSortOrder expression, object data = null);

        /// <summary>
        /// Translates a data type expression into a formatted string
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        string DataTypeParse(DataTypeReference expression, object data = null);

        /// <summary>
        /// Translates a constraint expression into a formatted string
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        string ConstraintDefinitionParse(ConstraintDefinition expression, object data = null);

        /// <summary>
        /// Translates ALTER TABLE ADD column expression into a formatted string
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        string AlterTableAddTableElementStatementParse(AlterTableAddTableElementStatement expression, object data = null);

        /// <summary>
        /// Translates a column definition into a formatted string
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        string ColumnDefinitionParse(ColumnDefinition expression, object data = null);

        /// <summary>
        /// Translates the DROP TABLE
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        string DropTableStatementParse(DropTableStatement expression, object data = null);

        /// <summary>
        /// Translates the COALESCE function
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        string CoalesceExpressionParse(CoalesceExpression expression, object data = null);

    }

}
