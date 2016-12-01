using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FoxyAsians.Models
{


    public enum PromotionType
    {
        NoPromotion,
        [Display(Name = "FEATURED ITEMS")]
        FEATUREDITEM,
        [Display(Name = "% OFF")]
        PERCENTOFF

    }

    public class Promotion
    {
        [Key]
        public int PromotionID { get; set; }

        [Required]
        [Display(Name = "PromotionType")]
        public PromotionType PromotionType { get; set; }

        public int Percentage { get; set; }

        public bool Disable { get; set; }
    }
}