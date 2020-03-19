using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EsportMVC
{
    public partial class Game
    {
        public Game()
        {
            Teams = new HashSet<Team>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Поле повинно бути заповненим")]
        [Display(Name = "Назва гри")]
        public string Name { get; set; }

        public virtual ICollection<Team> Teams { get; set; }
    }
}
