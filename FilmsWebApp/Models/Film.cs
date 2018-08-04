using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace FilmsWebApp.Models
{
    [Table("FilmInfo")]
    public class Film
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Title")]
        [StringLength(50, MinimumLength = 0)]
        public string Title { get; set; }

        //[Required]
        //[Range(1980, 2024)]
        //[Display(Name = "Year")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true)]
        [Display(Name = "Avaible Date")]
        public DateTime Year { get; set; }

        [Required]
        [Display(Name = "Format")]
        public FilmFormat Format { get; set; }

        [Display(Name = "Actors")]
        public virtual ICollection<Actor> Actors { get; set; }

        //public Film()
        //{
        //    Actors = new List<Actor>();
        //}
    }

    public enum FilmFormat
    {
        VHS = 1,
        DVD = 2,
        BlueRay = 3
    }
}