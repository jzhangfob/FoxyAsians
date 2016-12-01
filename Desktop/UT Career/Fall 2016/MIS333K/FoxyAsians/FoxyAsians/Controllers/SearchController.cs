using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using FoxyAsians.Models;
using System.Net;
using Microsoft.AspNet.Identity;
using System.ComponentModel.DataAnnotations;

namespace FoxyAsians.Controllers
{
    public enum SortOrder { Ascending, Descending }

    public class SearchController : Controller
    {
        AppDbContext db = new AppDbContext();

        //GET: Search
        //public ActionResult Index()
        //{
        //    return View();
        //}

        //POST: Search
        public ActionResult Index(String SearchString)
        {
            ViewBag.TotalCount = db.Songs.Count();
            List<Song> SelectedSongs = new List<Song>();
            if (SearchString == null)
            {
                ViewBag.SelectedCount = db.Songs.Count();
                return View(db.Songs.ToList());
            }
            else
            {
                SelectedSongs = db.Songs.Where(c => c.SongTitle.Contains(SearchString) || c.Artists.ToString().Contains(SearchString) || c.Albums.ToString().Contains(SearchString) || c.Genres.ToString().Contains(SearchString)).ToList();
                var q = SelectedSongs.OrderBy(c => c.SongTitle).ThenBy(c => c.Artists).ThenBy(c => c.Albums).ThenBy(c => c.Genres);
                ViewBag.SelectedCount = SelectedSongs.Count();
                return View(q.ToList());
            }
        }


        //Song Search
        public ActionResult DetailedSearch()
        {
            ViewBag.AllGenres = GetAllGenres();
            return View();
        }

        //Song Search - UNCOMMENT THE END WHEN RATING IS WORKING
        public ActionResult SearchResults(String SearchString, SortOrder TitleOrder, String SearchArtist, SortOrder ArtistOrder, String SearchAlbum, int[] SelectedGenre, String SearchRating, SortOrder SelectedRatingOrder)
        {
            ViewBag.AllGenres = GetAllGenres();

            List<Song> SelectedSongs = new List<Song>();
            var q = from c in db.Songs
                    select c;

            ViewBag.TotalCount = q.Count();

            //Title (Name)
            if (SearchString.Equals(""))
            {
                //do nothing
            }
            else
            {
                q = q.Where(c => c.SongTitle.Contains(SearchString));
            }

            //Artist
            if (SearchArtist.Equals(""))
            {
                //do nothing
            }
            else
            {
                q = q.Where(c => c.Artists.Any(g => g.ArtistName.Contains(SearchArtist)));
            }

            //Album
            if (SearchAlbum.Equals(""))
            {
                //do nothing
            }
            else
            {
                q = q.Where(c => c.Albums.Any(g => g.AlbumTitle.Contains(SearchAlbum)));
            }

            //Genre
            if (SelectedGenre == null || SelectedGenre.Contains(0))
            {
                //do nothing
            }
            else
            {
                q = q.Where(c => c.Genres.Any(g => SelectedGenre.Contains(g.GenreID)));
            }

            ////Rating - NOT YET SEEDED/ WORKING
            //if (SearchRating.Equals(""))
            //{
            //    //do nothing
            //}
            //else
            //{
            //    try
            //    {
            //        q = q.Where(c => c.SongReviews.Any(g => g.Rating >= Convert.ToDecimal(SearchRating)));
            //    }
            //    catch(Exception e)
            //    {
            //        throw;
            //    }
            //}

            ////OrderBy
            //if (TitleOrder == SortOrder.Ascending)
            //{
            //    if(ArtistOrder == SortOrder.Ascending)
            //    {
            //        if (SelectedRatingOrder == SortOrder.Ascending)
            //        {
            //            q = q.OrderBy(c => c.Name).ThenBy(c => c.Artists).ThenBy(c => c.Ratings);
            //        }
            //        else
            //        {
            //            q = q.OrderBy(c => c.Name).ThenBy(c => c.Artists).ThenByDescending(c => c.Ratings);
            //        }
            //    }
            //    else
            //    {
            //        if (SelectedRatingOrder == SortOrder.Ascending)
            //        {
            //            q = q.OrderBy(c => c.Name).ThenByDescending(c => c.Artists).ThenBy(c => c.Ratings);
            //        }
            //        else
            //        {
            //            q = q.OrderBy(c => c.Name).ThenByDescending(c => c.Artists).ThenByDescending(c => c.Ratings);
            //        }
            //    }
            //}
            //else
            //{
            //    if (ArtistOrder == SortOrder.Ascending)
            //    {
            //        if (SelectedRatingOrder == SortOrder.Ascending)
            //        {
            //            q = q.OrderByDescending(c => c.Name).ThenBy(c => c.Artists).ThenBy(c => c.Ratings);
            //        }
            //        else
            //        {
            //            q = q.OrderByDescending(c => c.Name).ThenBy(c => c.Artists).ThenByDescending(c => c.Ratings);
            //        }
            //    }
            //    else
            //    {
            //        if (SelectedRatingOrder == SortOrder.Ascending)
            //        {
            //            q = q.OrderByDescending(c => c.Name).ThenByDescending(c => c.Artists).ThenBy(c => c.Ratings);
            //        }
            //        else
            //        {
            //            q = q.OrderByDescending(c => c.Name).ThenByDescending(c => c.Artists).ThenByDescending(c => c.Ratings);
            //        }
            //    }
            //}
            ViewBag.SelectedCount = q.Count();
            return View(q.ToList());
        }





        //ALBUM SEARCH///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //Album Search
        public ActionResult AlbumDetailedSearch()
        {
            ViewBag.AllGenres = GetAllGenres();
            return View();
        }

        //Album Search - UNCOMMENT THE END WHEN RATING IS WORKING
        public ActionResult AlbumSearchResults(String AlbumSearch, SortOrder TitleOrder, String SearchArtist, SortOrder ArtistOrder, int[] SelectedGenre, String SearchRating, SortOrder SelectedRatingOrder)
        {
            ViewBag.AllGenres = GetAllGenres();

            List<Album> SelectedAlbums = new List<Album>();
            var q = from c in db.Albums
                    select c;

            ViewBag.TotalCount = q.Count();

            //Title (Name)
            if (AlbumSearch.Equals(""))
            {
                //do nothing
            }
            else
            {
                q = q.Where(c => c.AlbumTitle.Contains(AlbumSearch));
            }

            //Artist
            if (SearchArtist.Equals(""))
            {
                //do nothing
            }
            else
            {
                q = q.Where(c => c.Artists.Any(g => g.ArtistName.Contains(SearchArtist)));
            }

            //Genre
            if (SelectedGenre == null || SelectedGenre.Contains(0))
            {
                //do nothing
            }
            else
            {
                q = q.Where(c => c.Genres.Any(g => SelectedGenre.Contains(g.GenreID)));
            }

            ////Rating - NOT YET SEEDED/ WORKING
            //if (SearchRating.Equals(""))
            //{
            //    //do nothing
            //}
            //else
            //{
            //    //Try/ Catch if SearchRating is not int
            //    q = q.Where(c => c.Ratings.Any(g => g.Name == SearchRating.To));
            //}

            ////OrderBy
            //if (TitleOrder == SortOrder.Ascending)
            //{
            //    if(ArtistOrder == SortOrder.Ascending)
            //    {
            //        if (SelectedRatingOrder == SortOrder.Ascending)
            //        {
            //            q = q.OrderBy(c => c.Name).ThenBy(c => c.Artists).ThenBy(c => c.Ratings);
            //        }
            //        else
            //        {
            //            q = q.OrderBy(c => c.Name).ThenBy(c => c.Artists).ThenByDescending(c => c.Ratings);
            //        }
            //    }
            //    else
            //    {
            //        if (SelectedRatingOrder == SortOrder.Ascending)
            //        {
            //            q = q.OrderBy(c => c.Name).ThenByDescending(c => c.Artists).ThenBy(c => c.Ratings);
            //        }
            //        else
            //        {
            //            q = q.OrderBy(c => c.Name).ThenByDescending(c => c.Artists).ThenByDescending(c => c.Ratings);
            //        }
            //    }
            //}
            //else
            //{
            //    if (ArtistOrder == SortOrder.Ascending)
            //    {
            //        if (SelectedRatingOrder == SortOrder.Ascending)
            //        {
            //            q = q.OrderByDescending(c => c.Name).ThenBy(c => c.Artists).ThenBy(c => c.Ratings);
            //        }
            //        else
            //        {
            //            q = q.OrderByDescending(c => c.Name).ThenBy(c => c.Artists).ThenByDescending(c => c.Ratings);
            //        }
            //    }
            //    else
            //    {
            //        if (SelectedRatingOrder == SortOrder.Ascending)
            //        {
            //            q = q.OrderByDescending(c => c.Name).ThenByDescending(c => c.Artists).ThenBy(c => c.Ratings);
            //        }
            //        else
            //        {
            //            q = q.OrderByDescending(c => c.Name).ThenByDescending(c => c.Artists).ThenByDescending(c => c.Ratings);
            //        }
            //    }
            //}
            ViewBag.SelectedCount = q.Count();
            return View(q.ToList());
        }







        //ARTIST SEARCH///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //Artist Search
        public ActionResult ArtistDetailedSearch()
        {
            ViewBag.AllGenres = GetAllGenres();
            return View();
        }

        //Artist Search - UNCOMMENT THE END WHEN RATING IS WORKING
        public ActionResult ArtistSearchResults(String SearchArtist, SortOrder NameOrder, int[] SelectedGenre, String SearchRating, SortOrder SelectedRatingOrder)
        {
            ViewBag.AllGenres = GetAllGenres();

            List<Artist> SelectedArtists = new List<Artist>();
            var q = from c in db.Artists
                    select c;

            ViewBag.TotalCount = q.Count();

            //Artist
            if (SearchArtist.Equals(""))
            {
                //do nothing
            }
            else
            {
                q = q.Where(c => c.ArtistName.Contains(SearchArtist));
            }

            //Genre
            if (SelectedGenre == null || SelectedGenre.Contains(0))
            {
                //do nothing
            }
            else
            {
                q = q.Where(c => c.Genres.Any(g => SelectedGenre.Contains(g.GenreID)));
            }

            ////Rating - NOT YET SEEDED/ WORKING
            //if (SearchRating.Equals(""))
            //{
            //    //do nothing
            //}
            //else
            //{
            //    //Try/ Catch if SearchRating is not int
            //    q = q.Where(c => c.Ratings.Any(g => g.Name == SearchRating.To));
            //}

            ////OrderBy
            //if (TitleOrder == SortOrder.Ascending)
            //{
            //    if(ArtistOrder == SortOrder.Ascending)
            //    {
            //        if (SelectedRatingOrder == SortOrder.Ascending)
            //        {
            //            q = q.OrderBy(c => c.Name).ThenBy(c => c.Artists).ThenBy(c => c.Ratings);
            //        }
            //        else
            //        {
            //            q = q.OrderBy(c => c.Name).ThenBy(c => c.Artists).ThenByDescending(c => c.Ratings);
            //        }
            //    }
            //    else
            //    {
            //        if (SelectedRatingOrder == SortOrder.Ascending)
            //        {
            //            q = q.OrderBy(c => c.Name).ThenByDescending(c => c.Artists).ThenBy(c => c.Ratings);
            //        }
            //        else
            //        {
            //            q = q.OrderBy(c => c.Name).ThenByDescending(c => c.Artists).ThenByDescending(c => c.Ratings);
            //        }
            //    }
            //}
            //else
            //{
            //    if (ArtistOrder == SortOrder.Ascending)
            //    {
            //        if (SelectedRatingOrder == SortOrder.Ascending)
            //        {
            //            q = q.OrderByDescending(c => c.Name).ThenBy(c => c.Artists).ThenBy(c => c.Ratings);
            //        }
            //        else
            //        {
            //            q = q.OrderByDescending(c => c.Name).ThenBy(c => c.Artists).ThenByDescending(c => c.Ratings);
            //        }
            //    }
            //    else
            //    {
            //        if (SelectedRatingOrder == SortOrder.Ascending)
            //        {
            //            q = q.OrderByDescending(c => c.Name).ThenByDescending(c => c.Artists).ThenBy(c => c.Ratings);
            //        }
            //        else
            //        {
            //            q = q.OrderByDescending(c => c.Name).ThenByDescending(c => c.Artists).ThenByDescending(c => c.Ratings);
            //        }
            //    }
            //}
            ViewBag.SelectedCount = q.Count();
            return View(q.ToList());
        }



        public SelectList GetAllGenres()
        {
            var query = from c in db.Genres
                        orderby c.GenreName
                        select c;
            List<Genre> GenreList = query.Distinct().ToList();

            //Add in choice for not selecting a frequency
            Genre NoChoice = new Genre() { GenreID = 0, GenreName = "All Genres" };
            GenreList.Add(NoChoice);
            SelectList GenresList = new SelectList(GenreList.OrderBy(f => f.GenreName), "GenreID", "Name");
            return GenresList;
        }

        //public List<Song> GetSongRatings()
        //{
        //    var q = from c in db.Song
        //           select c;

        //    List<Song> SongsList = q.ToList();
        //    return SongsList;
        //}
    }
}