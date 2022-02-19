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

        public Dictionary<string, string> QueryParameters = new Dictionary<string, string>();

        public bool UpdateQueryParametersList = true;
        public bool ReplaceQueryParametersWithValues = false;
    }

}
