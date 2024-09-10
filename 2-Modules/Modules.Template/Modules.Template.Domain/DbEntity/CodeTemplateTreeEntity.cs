using SqlSugar;

namespace Modules.Template.Domain.DbEntity
{
    [SugarTable("CodeTemplate")]
    public class CodeTemplateTreeEntity
    {
        [SugarColumn(IsPrimaryKey = true)]
        public long ID { get; set; }

        [SugarColumn(IsNullable = true, DefaultValue = "0", ColumnDescription = "父ID")]
        public string PARENTID { get; set; }

        [SugarColumn(IsNullable = true)]
        public string username { get; set; }
        [SugarColumn(IsNullable = true)]
        public string password { get; set; }
        public DateTime createtime { get; set; }

        [SugarColumn(IsNullable = true)]
        public string notes { get; set; }

        [SqlSugar.SugarColumn(IsIgnore = true)]
        public List<CodeTemplateTreeEntity> SqlsugarTreeChild { get; set; }//用于映射sqlsugar查询得到的树状结构,注意添加IsIgnore = true
    }
}
