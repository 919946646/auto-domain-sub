using SqlSugar;

namespace Modules.Template.Domain.DbEntity
{
    [SugarTable("CodeTemplate")]
    public class CodeTemplateTreeEntity
    {
        #region 树结构默认列

        [SugarColumn(IsPrimaryKey = true, Length = 64, ColumnDescription = "ID")]
        public string id { get; set; }

        [SugarColumn(Length = 32, IsNullable = true, DefaultValue = "0", ColumnDescription = "父ID")]
        public string parentid { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "更新时间")]
        public DateTime? updatetime { get; set; }

        [SugarColumn(ColumnDescription = "建立时间")]
        public DateTime createtime { get; set; }

        [SugarColumn(Length = 64, IsNullable = true, ColumnDescription = "更新用户")]
        public string updateuid { get; set; }

        [SugarColumn(Length = 64, ColumnDescription = "建立用户")]
        public string createuid { get; set; }

        [SqlSugar.SugarColumn(IsIgnore = true)]
        public List<CodeTemplateTreeEntity> SqlsugarTreeChild { get; set; }//用于映射sqlsugar查询得到的树状结构,注意添加IsIgnore = true
        #endregion

        [SugarColumn(IsNullable = true)]
        public string username { get; set; }

        [SugarColumn(IsNullable = true)]
        public string password { get; set; }

        [SugarColumn(IsNullable = true)]
        public string notes { get; set; }
    }
}
