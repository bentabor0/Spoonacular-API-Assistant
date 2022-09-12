using Newtonsoft.Json;

using RestSharp;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace _12._2Assignment
{
    public class APIController
    {
        /// <summary>
        /// Creates a new http client.
        /// </summary>
        private static HttpClient httpClient = new HttpClient();

        /// <summary>
        /// Gets or sets the API Manager.
        /// </summary>
        public static APIManager APIManager { get; set; }

        public static string SendRequest<T>(string method, string request)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("apiKey", APIManager.APIKey);

            try
            {
                HttpResponseMessage response = httpClient.GetAsync(request).Result;

                HttpContent content = response.Content;
                response.EnsureSuccessStatusCode();
                string result = content.ReadAsStringAsync().Result;
                return result;
            }
            catch (HttpRequestException e)
            {
                throw e;
            }
        }

        public static string SendRequest<T>(string method, string request, T entity)
        {
            string result = "";

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("apiKey", APIManager.APIKey);

            var entityAsJson = JsonConvert.SerializeObject(entity);
            HttpContent content = new StringContent(entityAsJson, Encoding.UTF8, "application/json");
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                HttpResponseMessage response = httpClient.PostAsync(request, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    content = response.Content;
                    result = content.ReadAsStringAsync().Result;
                }
                else
                {
                    Console.WriteLine($"Failed to post data. Status code:{response.StatusCode}");
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static List<T> ParseArrayResponse<T>(string res)
        {
            List<T> entities = JsonConvert.DeserializeObject<List<T>>(res);
            return entities;
        }

        private static T ParseResponse<T>(string res)
        {
            T entity = JsonConvert.DeserializeObject<T>(res);
            return entity;
        }

        public static string AddToMealPlan()
        {
            string fullConnectionString = APIManager.ConnectionString + "/mealplanner/:username/items?hash=3454120786771c03f913404987063dad9c8ebd73&apiKey=" + APIManager.APIKey;

            var test = new { date = 1589500800, slot = 1, position = 0, type = "INGREDIENTS", value = new string[] { "banana"} };

            var client = new RestClient(fullConnectionString);

            var request = new RestRequest();

            request.AddJsonBody(test);

            var response = client.Post(request);

            return response.StatusCode.ToString();
        }

        public static string ConnectUser()
        {
            string fullConnectionString = APIManager.ConnectionString + "/users/connect" + APIManager.APIKey;

            UserDTO user = new UserDTO();
            user.username = "bentabor";
            user.firstname = "Ben";
            user.lastname = "Tabor";
            user.email = "bentabor6215@gmail.com";

            string request = SendRequest("POST", fullConnectionString, user);

            //var client = new RestClient(fullConnectionString);
            //var request = new RestRequest();

            //var entityAsJson = JsonConvert.SerializeObject(user);

            //request.AddJsonBody(entityAsJson);

            //var response = client.Post(request);

            //return response.StatusCode.ToString();

            return null;
        }

        public static List<Recipe> GetRecipes(string ingredients, string numberOfRecipes)
        {
            string fullConnectionString = APIManager.ConnectionString + "/recipes/findByIngredients?ingredients=" + ingredients + "&number=" + numberOfRecipes + "&limitLicense=true&ranking=1&ignorePantry=false&apiKey=" + APIManager.APIKey;

            string request = SendRequest<string>("GET", fullConnectionString);

            List<RecipeDTO> recipeDTO = ParseArrayResponse<RecipeDTO>(request);

            List<Recipe> emptyRecipes = new List<Recipe>();

            for (int i = 0; i < recipeDTO.Count; i++)
            {
                Recipe r = new Recipe();
                r.ID = recipeDTO[i].ID;
                r.Title = recipeDTO[i].Title;
                emptyRecipes.Add(r);
            }

            List<Recipe> finishedRecipes = new List<Recipe>();
            foreach(Recipe r in emptyRecipes)
            {
                Recipe recipe = new Recipe();
                recipe = GetRecipeByID(r.ID);
                finishedRecipes.Add(recipe);
            }

            return finishedRecipes;
        }

        public static Recipe GetRecipeByID(int recipeID)
        {
            string fullConnecionString = APIManager.ConnectionString + "/recipes/" + recipeID + "/information?apiKey=" + APIManager.APIKey + "&includeNutrition=true";

            string request = SendRequest<string>("GET", fullConnecionString);

            RecipeDTO recipeDTO = ParseResponse<RecipeDTO>(request);

            Recipe recipe = new Recipe
            {
                ID = recipeDTO.ID,
                Title = recipeDTO.Title,
                SourceUrl = recipeDTO.SourceUrl,
                Summary = recipeDTO.Summary,
            };

            return recipe;
        }

        public static List<Recipe> GetRandomRecipe(string tag, string numberOfRecipes)
        {
            string fullConnectionString = APIManager.ConnectionString + "/recipes/random?limitLicense=true&tags=" + tag + "&number=" + numberOfRecipes + "&apiKey=" + APIManager.APIKey;

            string request = SendRequest<string>("GET", fullConnectionString);

            RandomRecipesDTO randomRecipeDTO = ParseResponse<RandomRecipesDTO>(request);
            List<Recipe> recipes = new List<Recipe>();

            foreach(Recipe r in randomRecipeDTO.recipes)
            {
                recipes.Add(GetRecipeByID(r.ID));
            }

            return recipes;
        }

        public static string GetAnswer(string question)
        {
            string fullConnectionSting = APIManager.ConnectionString + "/recipes/quickAnswer?q=" + question + "?" + "&apiKey=" + APIManager.APIKey;

            string request = SendRequest<string>("GET", fullConnectionSting);

            AnswerDTO answerDTO = ParseResponse<AnswerDTO>(request);
            Answer answer = new Answer();

            answer.Response = answerDTO.Respone;
            
            return answer.Response;
        }

        public static string GetRandomTriviaOrJoke(bool isJoke)
        {
            string fullConnectionString;
            if (isJoke)
            {
                fullConnectionString = APIManager.ConnectionString + "/food/jokes/random" + "?apiKey=" + APIManager.APIKey;
            }
            else
            {
                fullConnectionString = APIManager.ConnectionString + "/food/trivia/random" + "?apiKey=" + APIManager.APIKey;
            }

            string request = SendRequest<string>("GET", fullConnectionString);
            RandomDTO randomDTO = ParseResponse<RandomDTO>(request);
            RandomResponse random = new RandomResponse();

            random.Text = randomDTO.Text;

            return random.Text;
        }
    }
}
