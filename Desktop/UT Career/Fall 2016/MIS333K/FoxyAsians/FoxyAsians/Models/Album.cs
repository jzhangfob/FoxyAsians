using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace FoxyAsians.Models
{
    public class Album
    {
        public Int32 AlbumID { get; set; }

        [Required(ErrorMessage = "Album Name is required.")]
        [Display(Name = "Album Name")]
        public String AlbumTitle { get; set; }

        [Display(Name = "Album Price")]
        public String AlbumPrice { get; set; }




        //Navigational Properties: Genre, Songs, Artists, Review and Rating (?)
        public virtual List<Artist> Artists { get; set; }
        public virtual List<Genre> Genres { get; set; }
        public virtual List<Song> Songs { get; set; }
    }
}