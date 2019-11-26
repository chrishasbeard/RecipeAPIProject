using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace RecipeAPIProject.Controllers
{
    public class RecipeController : Controller
    {
        public readonly HttpClient _client;
        
        public RecipeController(IHttpClientFactory client)
        {
            _client = client.CreateClient();
            _client.BaseAddress = new Uri("http://www.recipepuppy.com/api/");

        }
        
        public async Task<IActionResult> DisplayRecipe()
        {
            _client.BaseAddress = new Uri("http://www.recipepuppy.com/api/");
            var serializer = new DataContractJsonSerializer(typeof(Recipe));
            var response = await _client.GetStreamAsync("");
            var results = serializer.ReadObject(response) as Recipe;

             return View();
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}