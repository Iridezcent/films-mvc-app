using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using FilmsWebApp.Models;

namespace FilmsWebApp.Controllers
{
    public class FilmsController : Controller
    {
        //TODO: Add Actors Controller
        private FilmContext db = new FilmContext();

        public ActionResult Index(string sort)
        {
            if (string.IsNullOrEmpty(sort) || !sort.Equals("Name"))
                return View(db.Films.ToList());
            else
            {
                var films = db.Films.OrderBy(f => f.Title).ToList();
                return View(films);
            }
        }

        //public ActionResult Sort(IEnumerable<Film> films)
        //{
        //    if (films == null)
        //        RedirectToAction("Index");

        //    films = films.OrderBy(f => f.Title);
        //}

        // GET: Films/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Film film = db.Films.Find(id);
            if (film == null)
            {
                return HttpNotFound();
            }
            //string actors = string.Empty;
            //foreach (var actor in film.Actors)
            //{
            //    actors += String.Format("{0} {1}, ", actor.FirstName, actor.SecondName);
            //}
            //actors = actors.TrimEnd(',',' ');
            ViewBag.Actors = GetActorsString(film.Actors);
            return View(film);
        }

        //[ActionName("Details")]
        //public ActionResult DetailsForMoreFilms(IEnumerable<Film> films)
        //{
        //    return View("DetailsForMoreFilms", films);
        //}

        public ActionResult ActorFilms(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Actor actor = db.Actors.Find(id);
            if (actor != null)
            {
                var actorFilms = db.Films.Where(f => f.Actors.Select(a => a.Id).Contains(actor.Id)).ToList();
                return View(actorFilms);
            }
            return RedirectToAction("Search");
        }

        public ActionResult Create()
        {
            ViewBag.Actors = new MultiSelectList(GetActors(db.Actors.ToList()));
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FilmView film, string[] actorsList)
        {
            ViewBag.Actors = new MultiSelectList(GetActors(db.Actors.ToList()));
            if (ModelState.IsValid)
            {
                var actors = new List<Actor>();
                foreach (var name in actorsList)
                {
                    var nameSplited = name.Split(' ');
                    string firstName = nameSplited[0], secondName = nameSplited[1];
                    var newActor = db.Actors.FirstOrDefault(a => a.FirstName == firstName && a.SecondName == secondName );
                    if (newActor != null)
                    {
                        actors.Add(newActor);
                    }
                }
                var newFilm = new Film { Format = film.Format, Id = film.Id, Title = film.Title, Year = film.Year, Actors = actors };
                db.Films.Add(newFilm);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(film);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Film film = db.Films.Find(id);
            if (film == null)
            {
                return HttpNotFound();
            }
            var filmView = new FilmView { Id = film.Id, Title = film.Title, Year = film.Year, Format = film.Format};
            ViewBag.Actors = new MultiSelectList(GetActors(db.Actors.ToList()));
            return View(filmView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FilmView film, string[] actors)            
        {                                                                   //TODO: Add AJAX Form                               
            if (actors == null)
            {
                ModelState.AddModelError("Actors", "Actors are not added!");
            }
            //Or System.InvalidOperationException  
            ViewBag.Actors = new MultiSelectList(GetActors(db.Actors.ToList()));           
            if (ModelState.IsValid)
            {
                //film.Actors = newActors;
                var editedFilm = db.Films.Find(film.Id);
                editedFilm.Title = film.Title;
                editedFilm.Year = film.Year;
                editedFilm.Format = film.Format;
                   
                editedFilm.Actors.Clear();
                foreach (var actor in db.Actors.Where(a => actors.Contains(a.FirstName + " " + a.SecondName)))
                {
                    editedFilm.Actors.Add(actor);
                }
                db.Entry(editedFilm).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(film);
        }

        public ActionResult Delete(int? id)                 //TODO: Edit DeleteComfirm view (add actors)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Film film = db.Films.Find(id);
            if (film == null)
            {
                return HttpNotFound();
            }
            return View(film);
        }       

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Film film = db.Films.Find(id);
            db.Films.Remove(film);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Search()
        {
            return View(db.Films.ToList());
        }                   //TODO: Edit Search View (no need table in begining, fix form)

        [HttpPost]
        public ActionResult Search(string searchParametr, string select)
        {
            if (!string.IsNullOrEmpty(searchParametr) && select != null)
            {
                if (select == "By Film Title")
                {
                    var film = db.Films.FirstOrDefault(p => p.Title == searchParametr);
                    if (film != null)
                        return RedirectToAction("Details", new { id = film.Id });
                    ModelState.AddModelError("", String.Format("No films found with name : {0}", searchParametr));
                }
                if (select == "By Actor Name")
                {
                    string[] actorSplited = searchParametr.TrimEnd(' ').Split();
                    string firstName = actorSplited[0], secondName = actorSplited[1];
                    Actor actor = db.Actors.FirstOrDefault(a => a.FirstName == firstName && a.SecondName == secondName);
                    if (actor != null)
                    {
                        return RedirectToAction("ActorFilms", new { id = actor.Id });
                    }
                    ModelState.AddModelError("", String.Format("No films found with actor : {0}", searchParametr));
                }
            }
            return View(db.Films.ToList());
        }      

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        #region This must be in Infrastracture class
        //Or even should not exist
        private IEnumerable<string> GetActors(IEnumerable<Actor> actors)
        {
            var actorsList = new List<string>();
            foreach (var actor in actors)
            {
                actorsList.Add(String.Format("{0} {1}", actor.FirstName, actor.SecondName));
            }

            return actorsList;
        }

        public string GetActorsString(IEnumerable<Actor> actors)
        {
            IEnumerable<string> actorsList = GetActors(actors);
            string actorsString = string.Empty;
            foreach (var actor in actorsList)
            {
                actorsString += actor + ", ";
            }
            return actorsString = actorsString.TrimEnd(',',' ');
        }
        #endregion

    }
}
