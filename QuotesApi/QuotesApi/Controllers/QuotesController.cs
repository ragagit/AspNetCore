using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuotesApi.Data;
using QuotesApi.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace QuotesApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class QuotesController : Controller
    {
        private QuotesDbContext _quotesDbContext;

        public QuotesController(QuotesDbContext quotesDbContext)
        {
            _quotesDbContext = quotesDbContext;
        }

        // GET: api/values
        [HttpGet]
        [ResponseCache(Duration=60, Location = ResponseCacheLocation.Client) ]
        [AllowAnonymous]
        //public IEnumerable<Quote> Get()
        public IActionResult Get(string sort)
        {
            //var quotes = _quotesDbContext.Quotes;
            IQueryable<Quote> quotes;
            switch(sort)
            {
                case "desc":
                    quotes = _quotesDbContext.Quotes.OrderByDescending(q => q.CreatedAt);
                    break;
                case "asc":
                    quotes = _quotesDbContext.Quotes.OrderBy(q => q.CreatedAt);
                    break;
                default:
                    quotes = _quotesDbContext.Quotes;
                    break;

            }

            //return _quotesDbContext.Quotes;
            //return BadRequest();
            //return NotFound();
            //return StatusCode(400);
            //return StatusCode(StatusCodes.Status102Processing);
            return Ok(quotes);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        //public Quote Get(int id)
        public IActionResult Get(int id)
        {
            var quote = _quotesDbContext.Quotes.Find(id);
            if (quote == null)
            {
                return NotFound("Record not found");
            }
            return Ok(quote);
        }

        // POST api/values
        [HttpPost]
        //public void Post([FromBody]Quote quote)
        public IActionResult Post([FromBody]Quote quote)
        {
            //The next line getes the id from the auth token
            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            quote.UserId = userId;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _quotesDbContext.Quotes.Add(quote);
            _quotesDbContext.SaveChanges();
            //Status Code 201 means created
            return StatusCode(StatusCodes.Status201Created);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        //public void Put(int id, [FromBody] Quote quote)
        public IActionResult Put(int id, [FromBody] Quote quote)
        {
            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

            //The next type check the model, such as [Required]
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = _quotesDbContext.Quotes.Find(id);
            if (entity == null)
            {
                return NotFound("Record not found");
            }
            if( userId != entity.UserId)
            {
                return BadRequest("Sorry you can't update this record");
            }
            entity.Title = quote.Title;
            entity.Author = quote.Author;
            entity.Description = quote.Description;
            entity.Type = quote.Type;
            entity.CreatedAt = quote.CreatedAt;
            _quotesDbContext.SaveChanges();
            return Ok("Record updated successfully");
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        //public void Delete(int id)
        public IActionResult Delete(int id)
        {
            var quote = _quotesDbContext.Quotes.Find(id);
            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;


            if (quote == null)
            {
                return NotFound("Record to delete not found");
            }
            if (userId != quote.UserId)
            {
                return BadRequest("Sorry you can't update this record");
            }
            _quotesDbContext.Quotes.Remove(quote);
            _quotesDbContext.SaveChanges();

            return Ok("Qoute deleted");
        }

        //api/quotes/Test/1
        [HttpGet("[action]/{id}")]
        public int Test(int id)
        {
            return id;
        }

        //Paging
        [HttpGet("[action]")]
        public IActionResult PagingQuote(int? pageNumber, int? pageSize)
        {
            var quotes = _quotesDbContext.Quotes;
            var currentPageNumber = pageNumber ?? 1;
            var currentPageSize = pageSize ?? 5;
            return Ok(quotes.Skip((currentPageNumber - 1) * currentPageSize).Take(currentPageSize));
        }

        [HttpGet("[action]")]
        public IActionResult SearchQuote(string type)
        {
            var quotes = _quotesDbContext.Quotes.Where(q => q.Type.StartsWith(type, StringComparison.Ordinal));
            return Ok(quotes);
        }

        [HttpGet("[action]")]
        public IActionResult MyQuotes()
        {
            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var quotes = _quotesDbContext.Quotes.Where(q=>q.UserId == userId);
            return Ok(quotes);
        }
    }
}

