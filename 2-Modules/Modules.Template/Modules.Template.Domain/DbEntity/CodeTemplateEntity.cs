using SqlSugar;

namespace Modules.Template.Domain.DbEntity
{
    [SugarTable("CodeTemplate")]
    public class CodeTemplateEntity
    {
        [SugarColumn(IsPrimaryKey = true)]
        public long ID { get; set; }
        [SugarColumn(IsNullable = true)]
        public string username { get; set; }
        [SugarColumn(IsNullable = true)]
        public string name { get; set; }
        [SugarColumn(IsNullable = true)]
        public string password { get; set; }
        public DateTime createtime { get; set; }

        [SugarColumn(IsNullable = true)]
        public string notes { get; set; }
    }
}
