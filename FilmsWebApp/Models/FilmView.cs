using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace FilmsWebApp.Models
{
    public class FilmView
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Title")]
        [StringLength(50, MinimumLength = 0)]
        public string Title { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true)]
        [Display(Name = "Date")]
        public DateTime Year { get; set; }

        [Required]
        [Display(Name = "Format")]
        public FilmFormat Format { get; set; }

        [Display(Name = "Actors")]
        public virtual ICollection<string> Actors { get; set; }
    }
}