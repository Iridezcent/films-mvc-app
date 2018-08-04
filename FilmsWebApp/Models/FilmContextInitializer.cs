using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace FilmsWebApp.Models
{
    public class FilmContextInitializer : DropCreateDatabaseIfModelChanges<FilmContext>
    {
        protected override void Seed(FilmContext context)
        {
            var actors = new List<Actor>
            {
                new Actor{FirstName = "Benedict", SecondName = "Camberbetch"},
                new Actor{FirstName = "Tom", SecondName="Holland"},
                new Actor{FirstName = "Robert", SecondName ="Downey"}
            };

            context.Actors.AddRange(actors);
            context.SaveChanges();

            base.Seed(context);
        }
    }
}