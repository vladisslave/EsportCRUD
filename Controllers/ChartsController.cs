using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EsportMVC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartsController : ControllerBase
    {
        private readonly EsportDBContext _context;

        public ChartsController(EsportDBContext context)
        {
            _context = context;
        }

        [HttpGet("JsonData")]

        public JsonResult JsonData()
        {
            var countries = _context.Countries.Include(b => b.Players).ToList();
            List<object> conPlayer = new List<object>();
            conPlayer.Add(new[] { "Країна", "Кількість гравців" });
            foreach (var c in countries)
            {
                conPlayer.Add(new object[] { c.Name, c.Players.Count() });
            }
            return new JsonResult(conPlayer);
        }
    }
}