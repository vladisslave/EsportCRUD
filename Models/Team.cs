using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EsportMVC
{
    public partial class Team
    {
        public Team()
        {
            Players = new HashSet<Player>();
        }

        public int Id { get; set; }
        public int OrganisationId { get; set; }
        [Required(ErrorMessage = "Поле повинно бути заповненим")]
        [Display(Name = "Назва команди")]
        public string Name { get; set; }
        public int GameId { get; set; }
        [Display(Name = "Гра")]

        public virtual Game Game { get; set; }
        [Display(Name = "Організація")]
        public virtual Organisation Organisation { get; set; }
        public virtual ICollection<Player> Players { get; set; }
    }
}
