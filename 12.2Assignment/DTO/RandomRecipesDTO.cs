using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _12._2Assignment
{
    public class RandomRecipesDTO
    {
        [JsonProperty("recipes")]
        public List<Recipe> recipes { get; set; }
    }
}
