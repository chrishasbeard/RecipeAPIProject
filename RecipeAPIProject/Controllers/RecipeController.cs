using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RecipeAPIProject.Models;

namespace RecipeAPIProject.Controllers
{
    public class RecipeController : Controller
    {
        public readonly HttpClient _client;
        private readonly RecipeDbContext _context;


        public RecipeController(IHttpClientFactory client, RecipeDbContext context)
        {
            _client = client.CreateClient();
            _client.BaseAddress = new Uri("http://www.recipepuppy.com/api/");
            _context = context;

        }

        public async Task<IActionResult> DisplayRecipe(string tag)
        {
            //_client.BaseAddress = new Uri("http://www.recipepuppy.com/api/");
            //var serializer = new DataContractJsonSerializer(typeof(RecipeRoot));
            //var response = await _client.GetStreamAsync($"?format=json&i={tag}");
            //var results = serializer.ReadObject(response) as RecipeRoot;

            return View();
        }
        [HttpGet]
        public async Task<IActionResult> DisplayAllRecipe()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> DisplayAllRecipe(string tag)
        {

            // var serializer = new DataContractJsonSerializer(typeof(RecipeRoot));
            //var response = await _client.GetStreamAsync($"?format=json&i={tag}");

            var response = await _client.GetAsync($"?format=json&i={tag}");
            var results = await response.Content.ReadAsStringAsync();

            HttpContext.Session.SetString("FavListSession", results);

            var recipes = JsonConvert.DeserializeObject<RecipeRoot>(results);

            //var results = serializer.ReadObject(response) as RecipeRoot;


            return View(recipes);
        }
        public IActionResult AddFavorites(int favorite)
        {
            string favListJson = HttpContext.Session.GetString("FavListSession");
            var something = JsonConvert.DeserializeObject<RecipeRoot>(favListJson);

            var favoriteRecipe = something.results[favorite];
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }

    }

}