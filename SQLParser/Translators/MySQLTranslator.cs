using Microsoft.SqlServer.TransactSql.ScriptDom;
using SQLParser.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLParser.Translators {
    
    internal class MySQLTranslator : BaseTranslator {

        #region VARIABLES AND NESTED CLASSES

        public override string Quote { get; set; } = "`";

        #endregion


        #region CONSTRUCTORS

        public MySQLTranslator() : base() {

        }

        public MySQLTranslator(TranslateOptions formatOptions) : base(formatOptions) {

        }

        #endregion


        #region METHODS

        protected override string GetFunctionName(string functionName) {
            if (functionName.ToUpper() == "LEN") {
                return "LENGTH";
            } else if (functionName.ToUpper() == "GETDATE") {
                return "NOW";
            } 
            else if (functionName.ToUpper() == "CONTAINS") {
                return "~inconsistent function, use LIKE~";
            } 
            else if (functionName.ToUpper() == "DATEADD") {
                return "DATEADD~propably inconsistent~";
            } 
            else if (functionName.ToUpper() == "CONVERT") {
                return "DATEADD~propably inconsistent, check CAST~";
            } 
            else {
                return functionName;
            }
        }

        public override string QuerySpecificationParse(QuerySpecification querySpecification, object data = null) {
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
                        } else if (expression is ScalarSubquery scalarSubquery) {
                            this.Data.Level++;
                            string columnName = $"({QueryExpressionParse(scalarSubquery.QueryExpression, true)})";
                            this.Data.Level--;

                            // check if there is an alias for the column
                            if (selectScalarExpression.ColumnName != null) {
                                columnName += $" AS {selectScalarExpression.ColumnName.Value}";
                            }

                            columns.Add(columnName);
                        } else if (expression is SearchedCaseExpression searchedCaseExpression) {
                            this.Data.Level++;
                            string columnName = $"{SearchedCaseExpressionParse(searchedCaseExpression)}";
                            this.Data.Level--;

                            // check if there is an alias for the column
                            if (selectScalarExpression.ColumnName != null) {
                                columnName += $" AS {selectScalarExpression.ColumnName.Value}";
                            }

                            columns.Add(columnName);
                        } else if (expression is IntegerLiteral integerLiteral) {
                            string columnName = integerLiteral.Value;

                            // check if there is an alias for the column
                            if (selectScalarExpression.ColumnName != null) {
                                columnName += $" AS {selectScalarExpression.ColumnName.Value}";
                            }

                            columns.Add(columnName);
                        } else if (expression is StringLiteral stringLiteral) {
                            string columnName = $"{Quote}{stringLiteral.Value}{Quote}";

                            // check if there is an alias for the column
                            if (selectScalarExpression.ColumnName != null) {
                                columnName += $" AS {selectScalarExpression.ColumnName.Value}";
                            }

                            columns.Add(columnName);
                        } else if (expression is NumericLiteral numericLiteral) {
                            string columnName = $"{numericLiteral.Value}";

                            // check if there is an alias for the column
                            if (selectScalarExpression.ColumnName != null) {
                                columnName += $" AS {selectScalarExpression.ColumnName.Value}";
                            }

                            columns.Add(columnName);
                        } else if (expression is NullLiteral) {
                            string columnName = $"";

                            // check if there is an alias for the column
                            if (selectScalarExpression.ColumnName != null) {
                                columnName += $" AS {selectScalarExpression.ColumnName.Value}";
                            }

                            columns.Add(columnName);
                        } else if (expression is VariableReference variableReference) {
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
                        } else if (expression is FunctionCall functionCall) {
                            string columnName = FunctionCallParse(functionCall);

                            if (selectScalarExpression.ColumnName is IdentifierOrValueExpression identifierOrValueExpression) {
                                columnName += $" AS {identifierOrValueExpression.Value}";
                            }

                            columns.Add(columnName);
                        } else if (expression is ParenthesisExpression parenthesisExpression) {
                            string columnName = ParenthesisExpressionParse(parenthesisExpression);

                            // check if there is an alias for the column
                            if (selectScalarExpression.ColumnName != null) {
                                columnName += $" AS {selectScalarExpression.ColumnName.Value}";
                            }

                            columns.Add(columnName);
                        } else if (expression is BinaryExpression binaryExpression) {
                            string columnName = BinaryExpressionParse(binaryExpression);

                            // check if there is an alias for the column
                            if (selectScalarExpression.ColumnName != null) {
                                columnName += $" AS {selectScalarExpression.ColumnName.Value}";
                            }

                            columns.Add(columnName);
                        } else if (expression is CastCall castCall) {
                            string columnName = CastCallParse(castCall);

                            // check if there is an alias for the column
                            if (selectScalarExpression.ColumnName != null) {
                                columnName += $" AS {selectScalarExpression.ColumnName.Value}";
                            }

                            columns.Add(columnName);
                        } else {
                            columns.Add("~UNKNOWN ScalarExpression~");
                        }
                    } else if (column is SelectStarExpression selectStarExpression) {
                        columns.Add(SelectStarExpression(selectStarExpression));
                    } else if (column is SelectSetVariable) {
                        columns.Add("~UNKNOWN SelectElement~");
                    } else {
                        columns.Add("~UNKNOWN SelectElement~");
                    }
                }


                //// TABLES //////

                IList<TableReference> tableReferences = querySpecification.FromClause?.TableReferences;
                if (tableReferences != null) {
                    foreach (TableReference table in tableReferences) {
                        if (table is NamedTableReference namedTableReference) {
                            tables.Add(NamedTableReferenceParse(namedTableReference));
                        } else if (table is QueryDerivedTable queryDerivedTable) {
                            this.Data.Level++;
                            string tableName = $"({QueryExpressionParse(queryDerivedTable.QueryExpression, true)})";
                            this.Data.Level--;

                            // check if the table has an alias
                            if (queryDerivedTable.Alias != null) {
                                tableName += $" AS {queryDerivedTable.Alias?.Value}";
                            }

                            tables.Add(tableName);
                        } else if (table is QualifiedJoin qualifiedJoin) {
                            tables.Add(QualifiedJoinParse(qualifiedJoin));
                        } else {
                            tables.Add($"~UNKNOWN TableReference~");
                        }
                    }
                }


                //// WHERE //////

                if (querySpecification.WhereClause != null) {
                    BooleanExpression condition = querySpecification.WhereClause.SearchCondition;

                    if (condition is BooleanComparisonExpression booleanComparisonExpression) {
                        whereConditions.Add(BooleanComparisonExpressionParse(booleanComparisonExpression));
                    } else if (condition is BooleanBinaryExpression booleanBinaryExpression) {
                        whereConditions.Add(BooleanBinaryExpressionParse(booleanBinaryExpression));
                    } else if (condition is BooleanParenthesisExpression booleanParenthesisExpression) {
                        whereConditions.Add(BooleanParenthesisExpressionParse(booleanParenthesisExpression));
                    } else if (condition is BooleanNotExpression booleanNotExpression) {
                        whereConditions.Add(BooleanNotExpressionParse(booleanNotExpression));
                    } else if (condition is BooleanTernaryExpression booleanTernaryExpression) {
                        whereConditions.Add(BooleanTernaryExpressionParse(booleanTernaryExpression));
                    } else if (condition is BooleanIsNullExpression booleanIsNullExpression) {
                        whereConditions.Add(BooleanIsNullExpressionParse(booleanIsNullExpression));
                    } else if (condition is InPredicate inPredicate) {
                        whereConditions.Add(InPredicateParse(inPredicate));
                    } else if (condition is LikePredicate likePredicate) {
                        whereConditions.Add(LikePredicateParse(likePredicate));
                    } else if (condition is ExistsPredicate existsPredicate) {
                        whereConditions.Add(ExistsPredicateParse(existsPredicate));
                    } else {
                        whereConditions.Add("~UNKNOWN BooleanExpression~");
                    }
                }


                //// GROUPED //////

                if (querySpecification.GroupByClause != null) {
                    foreach (GroupingSpecification condition in querySpecification.GroupByClause.GroupingSpecifications) {
                        if (condition is ExpressionGroupingSpecification expressionGroupingSpecification) {
                            groupConditions.Add(ExpressionGroupingSpecificationParse(expressionGroupingSpecification));
                        } else {
                            groupConditions.Add("~UNKNOWN GroupingSpecification~");
                        }
                    }
                }


                //// HAVING //////

                if (querySpecification.HavingClause != null) {
                    BooleanExpression condition = querySpecification.HavingClause.SearchCondition;

                    if (condition is BooleanComparisonExpression booleanComparisonExpression) {
                        havingConditions.Add(BooleanComparisonExpressionParse(booleanComparisonExpression));
                    } else if (condition is BooleanBinaryExpression booleanBinaryExpression) {
                        havingConditions.Add(BooleanBinaryExpressionParse(booleanBinaryExpression));
                    } else if (condition is BooleanParenthesisExpression booleanParenthesisExpression) {
                        havingConditions.Add(BooleanParenthesisExpressionParse(booleanParenthesisExpression));
                    } else if (condition is BooleanNotExpression booleanNotExpression) {
                        havingConditions.Add(BooleanNotExpressionParse(booleanNotExpression));
                    } else if (condition is BooleanTernaryExpression booleanTernaryExpression) {
                        havingConditions.Add(BooleanTernaryExpressionParse(booleanTernaryExpression));
                    } else if (condition is BooleanIsNullExpression booleanIsNullExpression) {
                        havingConditions.Add(BooleanIsNullExpressionParse(booleanIsNullExpression));
                    } else if (condition is InPredicate inPredicate) {
                        havingConditions.Add(InPredicateParse(inPredicate));
                    } else if (condition is LikePredicate likePredicate) {
                        havingConditions.Add(LikePredicateParse(likePredicate));
                    } else if (condition is ExistsPredicate existsPredicate) {
                        havingConditions.Add(ExistsPredicateParse(existsPredicate));
                    } else {
                        havingConditions.Add("~UNKNOWN BooleanExpression~");
                    }
                }


                //// ORDER BY //////

                if (querySpecification.OrderByClause != null) {
                    foreach (ExpressionWithSortOrder condition in querySpecification.OrderByClause.OrderByElements) {
                        if (condition is ExpressionWithSortOrder expressionWithSortOrder) {
                            orderByConditions.Add(ExpressionWithSortOrderParse(expressionWithSortOrder));
                        } else {
                            orderByConditions.Add("~UKNOWN ExpressionWithSortOrder~");
                        }
                    }
                }


                // type results

                result = $"{Type} ";

                result += isDistinct ? "DISTINCT " : "";

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

                result += isTopSelection ? $"\nLIMIT {isTopCount} " : "";


            } catch {
                result = "~QuerySpecification ERROR~";
            }

            return result;
        }

        #endregion

    }
}
