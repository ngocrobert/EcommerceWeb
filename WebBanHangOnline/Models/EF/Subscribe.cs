﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBanHangOnline.Models.EF
{
    [Table("tb_Subscribe")]
    public class Subscribe
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
