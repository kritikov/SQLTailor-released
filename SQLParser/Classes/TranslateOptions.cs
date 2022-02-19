using Microsoft.SqlServer.TransactSql.ScriptDom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLParser.Classes {
    public class TranslateOptions : SqlScriptGeneratorOptions {

        public Dictionary<string, string> QueryParameters = new Dictionary<string, string>();

        public bool UpdateQueryParametersList = true;
        public bool ReplaceQueryParametersWithValues = false;
    }

}
