using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicForm.Model
{
    [Table("Role")]
    public class RoleModel
    {
        [Key]
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }    
        public bool Enable { get; set; }
        public bool Status { get; set; }
        public DateTime CreateTime { get; set; }
        public string CreateId { get; set; }
        public DateTime? UpdateTime { get; set; }
        public long? UpdateId { get; set; }

    }
}
