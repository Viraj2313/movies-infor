using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WbApp.Controllers
{
    [Route("api")]
    [ApiController]
    public class MovieDetailsController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public MovieDetailsController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet("movie_details")]
        public async Task<IActionResult> getMovieDetails(string imdbID)
        {
            var apiKey = "419a0f01";
            var url = $"http://www.omdbapi.com/?i={imdbID}&apikey={apiKey}";
            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var movieDetails = await response.Content.ReadAsStringAsync();
                return Ok(movieDetails);
            }
            return BadRequest("error fetching data");
        }

    }
}
