using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApi.Data;
using MovieApi.DTOs;
using MovieApi.Models;

namespace MovieApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly MovieDbContext _context;
        public MoviesController(MovieDbContext context)
        {
            _context = context;
        }
        //Main Endpoint to get movies with optional filtering, sorting, and pagination
        [HttpGet]
        public async Task<IActionResult> GetMovies(string? title, string? genre,string? sortby, string sortbydirection="asc", int limit=10, int page=1)
        {
            //Validating the parameters
            if (page<1)
            {
                return BadRequest("Page must be greater than 0");
            }

            if (limit < 1 || limit > 100)
            {
                return BadRequest("Limit must be between 1 and 100");
            }

            var query = _context.Movies.AsNoTracking();

        
            if (!string.IsNullOrWhiteSpace(title))
            {
                query = query.Where(m => m.Title.Contains(title));
            }
            if (!string.IsNullOrWhiteSpace(genre))
            {
                query = query.Where(m => m.Genre.Contains(genre));
            }

            if (!string.IsNullOrWhiteSpace(sortby))
            {
                switch (sortby.ToLower())
                {
                    case "title":
                        query = sortbydirection.ToLower() == "desc" ? query.OrderByDescending(m => m.Title) : query.OrderBy(m => m.Title);
                        break;
                    case "releasedate":
                        query = sortbydirection.ToLower() == "desc" ? query.OrderByDescending(m => m.ReleaseDate) : query.OrderBy(m => m.ReleaseDate);
                        break;
                    default:
                        return BadRequest("Invalid sortby parameter. Allowed values are 'title' or 'releaseDate'");
                }
            }
            else
            {
                query = query.OrderBy(m => m.Id); // Default sorting by Id 
            }
            //Calculating pagination details - useful to construct the result set.
            var totalCount = await query.CountAsync();
            var totalpages = (int)Math.Ceiling((double)totalCount / limit);


            var movies = await query.Skip((page - 1) * limit).Take(limit).ToListAsync();
            var result = new PagedResult     
            {
                Page = page,
                Limit = limit,
                TotalCount = totalCount,
                PageCount = totalpages,
                Movies = movies
            };
            return Ok(result);
        }

        //Additional endpoint to get popular movies, sorted by popularity in descending order
        [HttpGet("popular")]
        public async Task<IActionResult> GetPopularMovies(int limit=10)
        {
            //Validate limit
            if (limit < 1 || limit > 100)
            {
                return BadRequest("Limit must be between 1 and 100");
            }
            
            var movies = await _context.Movies.AsNoTracking().OrderByDescending(m => m.Popularity).Take(limit).ToListAsync();
            return Ok(movies);
        }

       
    }   
}

