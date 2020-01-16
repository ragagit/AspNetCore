using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExpressoApi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ExpressoApi.Controllers
{
    [Route("api/[controller]")]
    public class MenusController : Controller
    {
        ExpressoDbContext _expressoDbContext;

        public MenusController(ExpressoDbContext expressoDbContext)
        {
            _expressoDbContext = expressoDbContext;
        }

        // GET: api/values
        [HttpGet]
        public IActionResult GetMenus()
        {
            //The include is eager loading
            var menus = _expressoDbContext.Menus.Include("SubMenus");
            return Ok(menus);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var menu = _expressoDbContext.Menus.Include("SubMenus").FirstOrDefault(m=>m.Id==id);
            if(menu==null)
            {
                NotFound("Unable to find submenu");
            }
            return Ok(menu);
        }
    }
}
