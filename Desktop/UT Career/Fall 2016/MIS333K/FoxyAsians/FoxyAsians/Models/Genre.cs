using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace FoxyAsians.Models
{
    public class Genre
    {
        [Display(Name = "Genre ID")]
        [Required(ErrorMessage = "Genre ID is Required")]
        public Int32 GenreID { get; set; }
        [Display(Name = "Genre")]
        [Required(ErrorMessage = "Please select a genre")]
        public string GenreName { get; set; }

        //TODO: Naviagational Properties
        public virtual List<Artist> Artists { get; set; }
        public virtual List<Album> Albums { get; set; }
        public virtual List<Song> Songs { get; set; }
    }
}