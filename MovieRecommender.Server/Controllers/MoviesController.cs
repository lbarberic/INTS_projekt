using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using Movies.Core;
using Movies.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace MovieRecommender.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : Controller
    {

        private readonly MoviesService _service;

        public MoviesController(MoviesService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("GetAllMovies")]
        public async Task<IActionResult> GetMovies()
        {

            try
            {
                Console.WriteLine("Fetching data");
                var result = await _service.GetAllMovies();
                Console.WriteLine("Data fetched");
                return Ok(result);
            }
            catch (Exception ex)
            {

                var result = new ContentResult
                {
                    Content = ex.ToString(),
                    StatusCode = 500
                };
                return result;
            }
        }

        [HttpGet]
        [Route("GetSpecificMovies/{title}")]
        public async Task<IActionResult> GetSpecificMovie(string title)
        {

            try
            {
                var result = await _service.GetSpecificMovie(title);
                return Ok(result);
            }
            catch (Exception ex)
            {

                var result = new ContentResult
                {
                    Content = ex.ToString(),
                    StatusCode = 500
                };
                return result;
            }
        }


    }
}
