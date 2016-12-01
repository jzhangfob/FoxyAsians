using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace FoxyAsians.Models
{
    public class Song
    {
        [Display(Name = "Song ID")]
        public Int32 SongID { get; set; }

        [Required(ErrorMessage = "Song Title is required.")]
        [Display(Name = "Song Title")]
        public String SongTitle { get; set; }

        [Required(ErrorMessage = "Song Price is required.")]
        [Display(Name = "Song Price")]
        public decimal SongPrice { get; set; }

        [Required]
        [Display(Name = "Discounted?")]
        public bool Discount { get; set; }

        [Display(Name = "Discounted Price")]
        public decimal DiscountPrice { get; set; }

        //Navigational properties needed: Artist, Album, Genre, Ratings and Review (?)
        public virtual List<Artist> Artists { get; set; }
        public virtual List<Album> Albums { get; set; }
        public virtual List<Genre> Genres { get; set; }
    }
}