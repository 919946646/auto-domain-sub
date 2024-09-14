using SqlSugar;

namespace Modules.Template.Domain.DbEntity
{
    [SugarTable("CodeTemplate")]
    public class CodeTemplateEntity
    {
        #region 系统默认列
        [SugarColumn(Length = 36, IsPrimaryKey = true)]
        public string id { get; set; }

        [SugarColumn(ColumnDescription = "建立时间")]
        public DateTime createtime { get; set; }

        [SugarColumn(Length = 36, IsNullable = false, ColumnDescription = "建立用户")]
        public string createuid { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "更新时间")]
        public DateTime? updatetime { get; set; }

        [SugarColumn(Length = 36, IsNullable = true, ColumnDescription = "更新用户")]
        public string updateuid { get; set; }
        #endregion

        [SugarColumn(IsNullable = true)]
        public string username { get; set; }

        [SugarColumn(IsNullable = true)]
        public string password { get; set; }

        [SugarColumn(IsNullable = true)]
        public string notes { get; set; }
    }
}
