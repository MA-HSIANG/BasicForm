using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BasicForm.Model
{
    [Table("UserLoginInfo")]
    public class UserLoginInfoModel
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string LoginName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool Enable { get; set; }
        public bool Status { get; set; }
        public DateTime CreateTime { get; set; }
        public long CreateId { get; set; }
        public DateTime? UpdateTime { get; set; }
        public long? UpdateId { get; set; }
    }
}
