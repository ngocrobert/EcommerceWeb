﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace WebBanHangOnline.Models.EF
{
    [Table("tb_Product")]
    public class Product : CommonAbstract
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(250)]
        public string Title { get; set; }
        [StringLength(250)]

        public string? Alias { get; set; }
        [StringLength(50)]

        public string ProductCode { get; set; }
        public string? Description { get; set; }
        [AllowHtml]
        public string? Detail { get; set; }
        [StringLength(250)]

        public string? Image { get; set; }
        public decimal Price { get; set; }
        public decimal? PriceSale { get; set; }
        public int Quantity { get; set; }
        public bool?  IsHome { get; set; }
        public bool IsSale { get; set; }
        public bool IsFeacture { get; set; }
        public bool IsHot { get; set; }
        public bool IsActive { get; set; }
        public int? ProductCategoryId { get; set; }
        [StringLength(250)]

        public string? SeoTitle { get; set; }
        [StringLength(500)]

        public string? SeoDescription { get; set; }
        [StringLength(250)]

        public string? SeoKeywords { get; set; }

        public virtual ProductCategory? ProductCategory { get; set; }
    }
}