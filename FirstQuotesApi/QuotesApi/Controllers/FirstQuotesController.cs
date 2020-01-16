using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QuotesApi.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FirstQuotesApi.Controllers
{
    [Route("api/[controller]")]
    public class FirstQuotesController : Controller
    {
        static List<Quote> quotes = new List<Quote>
        {
            new Quote(){Id=0, Title="Imagination", Author="Einstein", Description="Imagination is more important that knowledge"},
            new Quote(){Id=1, Title="Alive in Morning", Author="Tom Hard", Description="If we are alive in the morning we are not dead"}
        };

        [HttpGet]
        public IEnumerable<Quote> Get()
        {
            return quotes;
        }

        [HttpPost]
        public void Post([FromBody] Quote quote)
        {
            if (quote == null)
            {
                return;
            }
            quotes.Add(quote);
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Quote quote)
        {
            quotes[id] = quote;
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            quotes.RemoveAt(id);
        }

        //// GET: api/values
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/values/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/values
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/values/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/values/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
