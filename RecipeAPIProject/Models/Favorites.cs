using System;
using System.Collections.Generic;

namespace RecipeAPIProject.Models
{
    public partial class Favorites
    {
        public string UserId { get; set; }
        public string Title { get; set; }
        public string Link { get; set; }
        public string Ingredients { get; set; }
        public int Id { get; set; }

        public virtual AspNetUsers User { get; set; }
        public Favorites() { }
    }
}
