using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicForm.Model
{
    [Table("RoleMenuPermission")]
    public class RoleMenuPermissionModel
    {
        [Key]
        public long Id { get; set; }
        public long MenuId { get; set; }
        public long RoleId { get; set; }
        public bool Status { get; set; } = true;
        public DateTime CreateTime { get; set; }
        public long CreateId { get; set; }
        public DateTime? UpdateTime { get; set; }
        public long? UpdateId { get; set; }
    }
}
