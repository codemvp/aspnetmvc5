using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MatchByteMovieApp.Models;

namespace MatchByteMovieApp.Controllers
{
    public class MovieApiController : ApiController
    {
        private MovieDbContext _context = new MovieDbContext();

        // GET api/<controller>      
        public IEnumerable<Movie> Get()
        {
            return _context.Movies;
        }

        // POST api/<controller>
        public void Post([FromBody]Movie movie)
        {
            _context.Movies.Add(movie);
            _context.SaveChanges();
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]Movie movie)
        {
            Movie movieToUpdate = _context.Movies.Find(movie.Id);
            movieToUpdate.Title = movie.Title;
            movieToUpdate.YearReleased = movie.YearReleased;
            movieToUpdate.Rating = movie.Rating;
            _context.Entry(movieToUpdate).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }       
        
        // DELETE api/<controller>/5
        //[Authorize(Roles = "Manager")]
        public void Delete(int id)
        {
            Movie movie = _context.Movies.Find(id);
            if (movie != null)
            {
                _context.Movies.Remove(movie);
                _context.SaveChanges();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Count(e => e.Id == id) > 0;
        }
    }
}
