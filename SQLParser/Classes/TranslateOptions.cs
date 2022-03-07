using Microsoft.SqlServer.TransactSql.ScriptDom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLParser.Classes {

    /// <summary>
    /// This class contains all the options needed to when translating a query.
    /// </summary>
    public class TranslateOptions : SqlScriptGeneratorOptions {

        public new int SqlVersion {
            get => (int)base.SqlVersion;
            set {
                base.SqlVersion = (SqlVersion)value;
            }
        }

        public new int KeywordCasing {
            get {
                if (base.KeywordCasing == Microsoft.SqlServer.TransactSql.ScriptDom.KeywordCasing.Lowercase) {
                    return 0;
                }
                else if (base.KeywordCasing == Microsoft.SqlServer.TransactSql.ScriptDom.KeywordCasing.Uppercase) {
                    return 1;
                }
                else {
                    return 2;
                }
            }
            set {
                if (value == 0) {
                    base.KeywordCasing = Microsoft.SqlServer.TransactSql.ScriptDom.KeywordCasing.Lowercase;
                }
                else if (value == 1) {
                    base.KeywordCasing = Microsoft.SqlServer.TransactSql.ScriptDom.KeywordCasing.Uppercase;
                }
                else {
                    base.KeywordCasing = Microsoft.SqlServer.TransactSql.ScriptDom.KeywordCasing.PascalCase;
                }
            }
        }

        public Dictionary<string, string> QueryParameters { get; set; } = new Dictionary<string, string>();

        public bool UpdateQueryParametersList { get; set; } = true;

        public bool ReplaceQueryParametersWithValues { get; set; } = false;

        public bool SelectContentsInBrackets { get; set; } = false;

        public bool UseIndentation { get; set; } = true;

        //public SqlEngineType SqlEngineType { get; set; }
    }

}
