using Modules.Core.Shared.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Modules.Template.Shared.CodeTemplate
{
    public class CodeTemplateTreeVM : BaseTreeVM<CodeTemplateTreeVM>
    {
        /*
         * 
         * 代码生成器会覆盖标记之间的内容。
         */
        //CodeGenerator start
        [Required(ErrorMessage = "姓名必须填写")]
        [DisplayName("姓名")]
        public string Username { get; set; }
        [DisplayName("名称")]
        public string Name { get; set; }

        [DisplayName("E-mail")]
        public string Email { get; set; }

        [DisplayName("生日")]
        public DateTime BirthDate { get; set; }

        [DisplayName("租户id")]
        [Required(ErrorMessage = "租户不能为空")]
        public string Notes { get; set; }
        //CodeGenerator end
    }
}
