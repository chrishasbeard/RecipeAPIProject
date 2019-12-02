using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Security.Claims;
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
        //private List<Favorites> userFavorites = new List<Favorites>();

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
        public IActionResult AddToFavorites()
        {
            string user = GetUser();
            var theseFavorites = _context.Favorites.Where(favorites => favorites.UserId == user).ToList();
            return View(theseFavorites);
        }

        public IActionResult AddFavorites(int favorite)
        {
            var foundFavoriteRecipe = HttpContext.Session.GetString("FavListSession");
            var aListOfStuff = JsonConvert.DeserializeObject<RecipeRoot>(foundFavoriteRecipe);
            Result theFavorite = aListOfStuff.results[favorite];
            var User = GetUser();
            Favorites ummUmmOK = new Favorites();
            ummUmmOK.UserId = User;
            ummUmmOK.Ingredients = theFavorite.ingredients;
            ummUmmOK.Link = theFavorite.href;
            ummUmmOK.Title = theFavorite.title;
            _context.Favorites.Add(ummUmmOK);
            _context.SaveChanges();
            //userFavorites.Add(ummUmmOK);
            //string favListJson = HttpContext.Session.GetString("FavListSession");
            //var something = JsonConvert.DeserializeObject<RecipeRoot>(favListJson);

            //var favoriteRecipe = something.results[favorite];
           
            return RedirectToAction("AddToFavorites");
        }
        private string GetUser()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
        //private void PopulateFavorites()
        //{
        //    string FavList = HttpContext.Session.GetString("FavListSession");
        //    if (FavList != null)
        //    {
        //        userFavorites = JsonConvert.DeserializeObject<List<Favorites>>(FavList);
        //    }
        //}
        public IActionResult Index()
        {
            return View();
        }

    }

}