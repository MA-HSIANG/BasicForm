using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicForm.Model
{
    [Table("UserRole")]
    public class UserRoleModel
    {
        [Key]
        public long Id { get; set; }
        public long UserId { get; set; }
        public long RoleId { get; set; }
        public bool Status { get; set; }
        public DateTime CreateTime { get; set; }
        public long CreateId { get; set; }
        public DateTime? UpdateTime  { get; set; }
        public long? UpdateId { get; set; }
    }
}
