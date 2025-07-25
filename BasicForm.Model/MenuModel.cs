using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicForm.Model
{
    [Table("Menu")]
    public class MenuModel()
    {
        [Key]
        public long Id { get; set; }
        public long? ParentId { get; set; }
        /// <summary>
        /// 菜單唯一編碼
        /// </summary>
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// 菜單顯示名稱
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Svg 圖標
        /// </summary>
        public string? IconSvg { get; set; }

        /// <summary>
        /// 菜單綁定用戶控件 typeof(UserControl)
        /// </summary>
        public string PageTypeName { get; set; } = string.Empty;

        /// <summary>
        /// 是否允許關閉頁面
        /// </summary>
        public bool Closeable { get; set; }
    }
}
