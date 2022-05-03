using Microsoft.SqlServer.TransactSql.ScriptDom;
using SQLParser.Classes;

namespace SQLParser.Translators {
    public class PoVariationTranslator : SqlOMTranslator {

        #region VARIABLES AND NESTED CLASSES

        public override string SelectQueryText { get => base.SelectQueryText; set => base.SelectQueryText = value; }
        public override string InsertQueryText { get => base.InsertQueryText; set => base.InsertQueryText = value; }
        public override string UpdateQueryText { get => base.UpdateQueryText; set => base.UpdateQueryText = value; }
        public override string DeleteQueryText { get => base.DeleteQueryText; set => base.DeleteQueryText = value; }
        public override string RendererText { get => base.RendererText; set => base.RendererText = value; }

        #endregion


        #region CONSTRUCTORS

        public PoVariationTranslator() {
            Reset();

            SelectQueryText = "poSelectQuery";
            InsertQueryText = "poInsertQuery";
            UpdateQueryText = "poUpdateQuery";
            DeleteQueryText = "poDeleteQuery";
            RendererText = "AppContext.Db.SqlOmRender";
        }

        public PoVariationTranslator(TranslateOptions formatOptions) {
            Reset();

            SelectQueryText = "poSelectQuery";
            InsertQueryText = "poInsertQuery";
            UpdateQueryText = "poUpdateQuery";
            DeleteQueryText = "poDeleteQuery";
            RendererText = "AppContext.Db.SqlOmRender";

            this.FormatOptions = formatOptions;
        }

        #endregion
    }
}
