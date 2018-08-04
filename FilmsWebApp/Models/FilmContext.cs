using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace FilmsWebApp.Models
{
    public class FilmContext : DbContext
    {
        static FilmContext()
        {
            Database.SetInitializer(new FilmContextInitializer());
        }
        public FilmContext() : base("FilmContext")
        {
            //Database.SetInitializer(new FilmContextInitializer());
        }

        public DbSet<Film> Films { get; set; }
        public DbSet<Actor> Actors { get; set; }


    }
}