using MovieApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using MovieApi.Controllers;
using MovieApi.Models;
using MovieApi.DTOs;

namespace MovieApi.Tests
{
    public class MoviesControllerTests
    {
        private MovieDbContext CreateTestDBContext()
        {
            var options = new DbContextOptionsBuilder<MovieDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_"+Guid.NewGuid().ToString())
                .Options;
            return new MovieDbContext(options);
        }

        [Fact] 
        public async Task GetMovies_InvaildPage_BadRequest()
        {
            using var context = CreateTestDBContext();
            var controller = new MoviesController(context);

            var result = await controller.GetMovies(null, null, null, "asc", 10, 0);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GetPopularMovies_Popularity()
        {
            using var context = CreateTestDBContext();
            
            //Set up the data for the test
            context.Movies.AddRange(
                new Movie { Title = "Movie A", Popularity = 5 },
                new Movie { Title = "Movie B", Popularity = 10 },
                new Movie { Title = "Movie C", Popularity = 7 }
            );

            await context.SaveChangesAsync();

            var controller = new MoviesController(context);

            //Test now
            var result = await controller.GetPopularMovies(10);
            var Okresult_200= Assert.IsType<OkObjectResult>(result);

            var movies = Assert.IsType<List<Movie>>(Okresult_200.Value);

            Assert.Equal(new[] { "Movie B", "Movie C", "Movie A" }, movies.Select(m => m.Title));
        }

        [Fact]
        public async Task GetMovies_FilterByTitle_ReturnsFilteredResults()
        {
            using var context = CreateTestDBContext();
            context.Movies.AddRange(
                new Movie { Title = "The Matrix" },
                new Movie { Title = "Inception" },
                new Movie { Title = "Interstellar" }
            );
            await context.SaveChangesAsync();
            var controller = new MoviesController(context);
            var result = await controller.GetMovies( title:"Inception", genre: null, limit:10, page:1, sortby:null, sortbydirection: "asc");
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<PagedResult>(okResult.Value);
            Assert.Single(response.Movies);
            Assert.Equal("Inception", response.Movies[0].Title);
        }

        
    }
}