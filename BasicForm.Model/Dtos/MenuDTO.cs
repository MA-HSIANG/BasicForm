using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicForm.Model.Dtos
{
    public class MenuDTO(string code, string name, string? iconSvg)
    {
        public long Id { get; set; }
        public long ParentId { get; set; }
        /// <summary>
        /// 菜單唯一編碼
        /// </summary>
        public string Code { get; set; } = code;

        /// <summary>
        /// 菜單顯示名稱
        /// </summary>
        public string Name { get; set; } = name;

        /// <summary>
        /// Svg 圖標
        /// </summary>
        public string? IconSvg { get; set; } = iconSvg;

        /// <summary>
        /// 菜單綁定用戶控件 typeof(UserControl)
        /// </summary>
        public Type? PageType { get; set; }

        /// <summary>
        /// 是否允許關閉頁面
        /// </summary>
        public bool Closeable { get; set; } = true;

        /// <summary>
        /// 子菜單
        /// </summary>
        public List<MenuDTO> Sub { get; set; } = [];
    }
}
