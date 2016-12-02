namespace FoxyAsians.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Whatever : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ArtistAlbums", "Artist_ArtistID", "dbo.Artists");
            DropForeignKey("dbo.ArtistAlbums", "Album_AlbumID", "dbo.Albums");
            DropForeignKey("dbo.GenreAlbums", "Genre_GenreID", "dbo.Genres");
            DropForeignKey("dbo.GenreAlbums", "Album_AlbumID", "dbo.Albums");
            DropForeignKey("dbo.GenreArtists", "Genre_GenreID", "dbo.Genres");
            DropForeignKey("dbo.GenreArtists", "Artist_ArtistID", "dbo.Artists");
            DropForeignKey("dbo.SongAlbums", "Song_SongID", "dbo.Songs");
            DropForeignKey("dbo.SongAlbums", "Album_AlbumID", "dbo.Albums");
            DropForeignKey("dbo.SongArtists", "Song_SongID", "dbo.Songs");
            DropForeignKey("dbo.SongArtists", "Artist_ArtistID", "dbo.Artists");
            DropForeignKey("dbo.SongGenres", "Song_SongID", "dbo.Songs");
            DropForeignKey("dbo.SongGenres", "Genre_GenreID", "dbo.Genres");
            DropIndex("dbo.ArtistAlbums", new[] { "Artist_ArtistID" });
            DropIndex("dbo.ArtistAlbums", new[] { "Album_AlbumID" });
            DropIndex("dbo.GenreAlbums", new[] { "Genre_GenreID" });
            DropIndex("dbo.GenreAlbums", new[] { "Album_AlbumID" });
            DropIndex("dbo.GenreArtists", new[] { "Genre_GenreID" });
            DropIndex("dbo.GenreArtists", new[] { "Artist_ArtistID" });
            DropIndex("dbo.SongAlbums", new[] { "Song_SongID" });
            DropIndex("dbo.SongAlbums", new[] { "Album_AlbumID" });
            DropIndex("dbo.SongArtists", new[] { "Song_SongID" });
            DropIndex("dbo.SongArtists", new[] { "Artist_ArtistID" });
            DropIndex("dbo.SongGenres", new[] { "Song_SongID" });
            DropIndex("dbo.SongGenres", new[] { "Genre_GenreID" });
            AddColumn("dbo.AspNetUsers", "isActive", c => c.Boolean(nullable: false));
            DropTable("dbo.Albums");
            DropTable("dbo.Artists");
            DropTable("dbo.Genres");
            DropTable("dbo.Songs");
            DropTable("dbo.ArtistAlbums");
            DropTable("dbo.GenreAlbums");
            DropTable("dbo.GenreArtists");
            DropTable("dbo.SongAlbums");
            DropTable("dbo.SongArtists");
            DropTable("dbo.SongGenres");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.SongGenres",
                c => new
                    {
                        Song_SongID = c.Int(nullable: false),
                        Genre_GenreID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Song_SongID, t.Genre_GenreID });
            
            CreateTable(
                "dbo.SongArtists",
                c => new
                    {
                        Song_SongID = c.Int(nullable: false),
                        Artist_ArtistID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Song_SongID, t.Artist_ArtistID });
            
            CreateTable(
                "dbo.SongAlbums",
                c => new
                    {
                        Song_SongID = c.Int(nullable: false),
                        Album_AlbumID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Song_SongID, t.Album_AlbumID });
            
            CreateTable(
                "dbo.GenreArtists",
                c => new
                    {
                        Genre_GenreID = c.Int(nullable: false),
                        Artist_ArtistID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Genre_GenreID, t.Artist_ArtistID });
            
            CreateTable(
                "dbo.GenreAlbums",
                c => new
                    {
                        Genre_GenreID = c.Int(nullable: false),
                        Album_AlbumID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Genre_GenreID, t.Album_AlbumID });
            
            CreateTable(
                "dbo.ArtistAlbums",
                c => new
                    {
                        Artist_ArtistID = c.Int(nullable: false),
                        Album_AlbumID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Artist_ArtistID, t.Album_AlbumID });
            
            CreateTable(
                "dbo.Songs",
                c => new
                    {
                        SongID = c.Int(nullable: false, identity: true),
                        SongTitle = c.String(nullable: false),
                        SongPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Discount = c.Boolean(nullable: false),
                        DiscountPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.SongID);
            
            CreateTable(
                "dbo.Genres",
                c => new
                    {
                        GenreID = c.Int(nullable: false, identity: true),
                        GenreName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.GenreID);
            
            CreateTable(
                "dbo.Artists",
                c => new
                    {
                        ArtistID = c.Int(nullable: false, identity: true),
                        ArtistName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ArtistID);
            
            CreateTable(
                "dbo.Albums",
                c => new
                    {
                        AlbumID = c.Int(nullable: false, identity: true),
                        AlbumTitle = c.String(nullable: false),
                        AlbumPrice = c.String(),
                    })
                .PrimaryKey(t => t.AlbumID);
            
            DropColumn("dbo.AspNetUsers", "isActive");
            CreateIndex("dbo.SongGenres", "Genre_GenreID");
            CreateIndex("dbo.SongGenres", "Song_SongID");
            CreateIndex("dbo.SongArtists", "Artist_ArtistID");
            CreateIndex("dbo.SongArtists", "Song_SongID");
            CreateIndex("dbo.SongAlbums", "Album_AlbumID");
            CreateIndex("dbo.SongAlbums", "Song_SongID");
            CreateIndex("dbo.GenreArtists", "Artist_ArtistID");
            CreateIndex("dbo.GenreArtists", "Genre_GenreID");
            CreateIndex("dbo.GenreAlbums", "Album_AlbumID");
            CreateIndex("dbo.GenreAlbums", "Genre_GenreID");
            CreateIndex("dbo.ArtistAlbums", "Album_AlbumID");
            CreateIndex("dbo.ArtistAlbums", "Artist_ArtistID");
            AddForeignKey("dbo.SongGenres", "Genre_GenreID", "dbo.Genres", "GenreID", cascadeDelete: true);
            AddForeignKey("dbo.SongGenres", "Song_SongID", "dbo.Songs", "SongID", cascadeDelete: true);
            AddForeignKey("dbo.SongArtists", "Artist_ArtistID", "dbo.Artists", "ArtistID", cascadeDelete: true);
            AddForeignKey("dbo.SongArtists", "Song_SongID", "dbo.Songs", "SongID", cascadeDelete: true);
            AddForeignKey("dbo.SongAlbums", "Album_AlbumID", "dbo.Albums", "AlbumID", cascadeDelete: true);
            AddForeignKey("dbo.SongAlbums", "Song_SongID", "dbo.Songs", "SongID", cascadeDelete: true);
            AddForeignKey("dbo.GenreArtists", "Artist_ArtistID", "dbo.Artists", "ArtistID", cascadeDelete: true);
            AddForeignKey("dbo.GenreArtists", "Genre_GenreID", "dbo.Genres", "GenreID", cascadeDelete: true);
            AddForeignKey("dbo.GenreAlbums", "Album_AlbumID", "dbo.Albums", "AlbumID", cascadeDelete: true);
            AddForeignKey("dbo.GenreAlbums", "Genre_GenreID", "dbo.Genres", "GenreID", cascadeDelete: true);
            AddForeignKey("dbo.ArtistAlbums", "Album_AlbumID", "dbo.Albums", "AlbumID", cascadeDelete: true);
            AddForeignKey("dbo.ArtistAlbums", "Artist_ArtistID", "dbo.Artists", "ArtistID", cascadeDelete: true);
        }
    }
}
