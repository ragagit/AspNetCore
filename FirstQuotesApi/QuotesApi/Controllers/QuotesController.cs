using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuotesApi.Data;
using QuotesApi.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace QuotesApi.Controllers
{
    [Route("api/[controller]")]
    public class QuotesController : Controller
    {
        private QuotesDbContext _quotesDbContext;

        public QuotesController(QuotesDbContext quotesDbContext)
        {
            _quotesDbContext = quotesDbContext;
        }

        // GET: api/values
        [HttpGet]
        //public IEnumerable<Quote> Get()
        public IActionResult Get()
        {
            var quotes = _quotesDbContext.Quotes;

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
            if( quote == null)
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
            var entity = _quotesDbContext.Quotes.Find(id);
            if( entity == null)
            {
                return NotFound("Record not found");
            }
            entity.Title = quote.Title;
            entity.Author = quote.Author;
            entity.Description = quote.Description;
            _quotesDbContext.SaveChanges();
            return Ok("Record updated successfully");
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        //public void Delete(int id)
        public IActionResult Delete(int id)
        {
            var quote =_quotesDbContext.Quotes.Find(id);

            if(quote == null)
            {
                return NotFound("Record to delete not found");
            }
            _quotesDbContext.Quotes.Remove(quote);
            _quotesDbContext.SaveChanges();

            return Ok("Qoute deleted");
        }
    }
}
