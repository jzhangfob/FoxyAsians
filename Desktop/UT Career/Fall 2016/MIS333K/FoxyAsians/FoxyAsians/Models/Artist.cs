using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace FoxyAsians.Models
{
    public class Artist
    {
        public Int32 ArtistID { get; set; }

        [Required(ErrorMessage = "Artist Name is required.")]
        [Display(Name = "Artist Name")]
        public String ArtistName { get; set; }

        //Navigational Properties: Genre, Songs, Albums, Review and Rating (?)
        public virtual List<Song> Songs { get; set; }
        public virtual List<Album> Albums { get; set; }
        public virtual List<Genre> Genres { get; set; }
    }
}