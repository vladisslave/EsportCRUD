using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace EsportMVC
{
    public partial class Country
    {
        public Country()
        {
            Organisations = new HashSet<Organisation>();
            Players = new HashSet<Player>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Поле повинно бути заповненим")]
        [Display(Name = "Назва країни")]
        [Valid.Isletter(ErrorMessage = "Недопустиме ім'я")]
        public string Name { get; set; }

        public virtual ICollection<Organisation> Organisations { get; set; }
        public virtual ICollection<Player> Players { get; set; }
    }
}
