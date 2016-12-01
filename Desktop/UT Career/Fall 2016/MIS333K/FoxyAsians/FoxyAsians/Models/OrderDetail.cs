using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FoxyAsians.Models
{
    public class OrderDetail
    {
        [Key]
        public int OrderDetailID { get; set; }

        public int OrderID { get; set; }

        public int SongID { get; set; } //do we need one single ID for both albums and songs?

        public int AlbumID { get; set; } //CHECK - could delete?

        public int Quantity { get; set; }

        //public int Genre { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal ActualSalesPrice { get; set; }

        public virtual Song song { get; set; } //Point to a song 

        public virtual Album album { get; set; } //Point to an album

        public virtual Order order { get; set; }
    }
}