using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace FilmsWebApp.Models
{
    [Table("ActorInfo")]
    public class Actor
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required]
        [StringLength(15)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(30)]
        [Display(Name = "Second Name")]
        public string SecondName { get; set; }

        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public string FullName
        {
            get
            {
                return FirstName + " " + SecondName;
            }
        }

        [Display(Name = "Actors")]
        public virtual ICollection<Film> Films { get; set; }

        public Actor()
        {
            Films = new List<Film>();
        }
       
        //TODO: ViewModel for search
    }
}