using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EsportMVC
{
    public partial class Organisation
    {
        public Organisation()
        {
            Departments = new HashSet<Department>();
            Teams = new HashSet<Team>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Поле повинно бути заповненим")]

        [Display(Name = "Назва організації")]
        public string Name { get; set; }
        
        public int CountryId { get; set; }
        [Display(Name = "Дата створення")]
        public DateTime? CreationDate { get; set; }
        [Display(Name = "Країна")]
        public virtual Country Country { get; set; }
        public virtual ICollection<Department> Departments { get; set; }
        public virtual ICollection<Team> Teams { get; set; }
    }
}
