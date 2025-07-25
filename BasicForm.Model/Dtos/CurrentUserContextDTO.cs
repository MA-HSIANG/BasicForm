using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicForm.Model.Dtos
{
    public class CurrentUserContextDTO
    {
        public long Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public List<MenuDTO> Menus { get; set; } = new();
    }
}
