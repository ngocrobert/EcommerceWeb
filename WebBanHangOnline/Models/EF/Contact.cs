using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBanHangOnline.Models.EF
{
    [Table("tb_Contact")]
    public class Contact : CommonAbstract
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required(ErrorMessage = "Ten khong duoc de trong")]
        [StringLength(150,ErrorMessage = "Khong duoc qua 150 ky tu")]
        public string Name { get; set; }
        [StringLength(150, ErrorMessage = "Khong duoc qua 150 ky tu")]
        public string Email  { get; set; }
        public string Website { get; set; }
        [StringLength(4000)]
        public string Message { get; set; }
        public bool IsRead { get; set; }
    }
}
