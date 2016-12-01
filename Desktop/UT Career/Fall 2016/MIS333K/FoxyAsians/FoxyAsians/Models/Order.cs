using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FoxyAsians.Models
{
    public partial class Order
    {
        [Key]
        public int OrderID { get; set; }

        [Required]
        public string UserID { get; set; }

        public virtual ApplicationUser user { get; set; }

        public int PaymentID { get; set; }

        public int PromotionID { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LName { get; set; }

        [Required]
        [Display(Name = "Street Address")]
        public string StreetAddress { get; set; }

        [Required]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required]
        [Display(Name = "State")]
        public string State { get; set; }

        [Required]
        [Display(Name = "Zip Code")]
        [RegularExpression(@"^\d{5}(-\d{4})?$", ErrorMessage = "Invalid Zip")]
        public string Zip { get; set; }

        //Show the TOTAL price
        public decimal Total { get; set; }
        //Show the prices for individual items
        public DateTime OrderDate { get; set; } //CHECK - do we need?
        //Show the prices for individual items
        public List<OrderDetail> OrderDetails { get; set; }

    }
}