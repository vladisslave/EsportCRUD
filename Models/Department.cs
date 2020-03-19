using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EsportMVC
{
    public partial class Department
    {
        public int Id { get; set; }
        
        public int OrganisationId { get; set; }
        [Required(ErrorMessage = "Поле повинно бути заповненим")]
        [Display(Name = "Назва відділу")]
        public string Name { get; set; }
        [Display(Name = "Організація")]

        public virtual Organisation Organisation { get; set; }
    }
}
