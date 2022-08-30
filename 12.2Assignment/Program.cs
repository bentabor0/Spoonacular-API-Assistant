using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _12._2Assignment
{
    internal class Program
    {
        static void Main(string[] args)
        {

            // Gets the app settings of the app.config file.
            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = configFile.AppSettings.Settings;

            // Creates a new api manager with the api url and api key from app.config
            APIController.APIManager = new APIManager(ConfigurationManager.ConnectionStrings["default"].ConnectionString, settings["APIKey"].Value);

            Console.WriteLine("Welcome to my spoonacular API project");
            Console.WriteLine(" ");
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine(" ");
                Console.WriteLine("Enter '1' to find a recipes by ingredient(s)");
                Console.WriteLine("Enter '2' to find a specific type of recipe like chinese or vegan");
                Console.WriteLine("Enter '3' to ask a nutrition question");
                Console.WriteLine("Enter '4' for a food joke");
                Console.WriteLine("Enter '5' for some food trivia");
                Console.WriteLine("Enter 'exit' to leave");
                Console.WriteLine(" ");

                string input = Console.ReadLine();
                switch (input)
                {
                    case "exit":
                        exit = true;
                        break;

                    case "1":
                        Console.WriteLine("Enter the ingredients you are searching for in a list seperated by commas like this 'apples,bananas'");
                        string ingredients = Console.ReadLine().ToLower();

                        Console.WriteLine("Enter the number of recipes to search for as a number, 50 is the max");
                        string numberOfIngredients = Console.ReadLine();
                        Console.WriteLine(" ");

                        List<Recipe> result = APIController.GetRecipes(ingredients, numberOfIngredients);

                        if (result.Count != 0)
                        {
                            Console.WriteLine("Results:");
                            foreach (Recipe recipe in result)
                            {
                                Console.WriteLine("Title: " + recipe.Title);
                                Console.WriteLine("Url: " + recipe.SourceUrl);
                                Console.WriteLine(" ");
                            }
                        }
                        else
                        {
                            Console.WriteLine("No recipes were found with those ingredients");
                        }
                        break;

                    case "2":
                        Console.WriteLine("Enter the type of food you are searching for");
                        string type = Console.ReadLine().ToLower().Trim();

                        Console.WriteLine("Enter the number of recipes to search for");
                        string numberOfRecipes = Console.ReadLine();

                        List<Recipe> result1 = APIController.GetRandomRecipe(type, numberOfRecipes);
                        if (result1.Count != 0)
                        {
                            foreach (Recipe recipe in result1)
                            {
                                Console.WriteLine("Title: " + recipe.Title);
                                Console.Write("Url: " + recipe.SourceUrl);
                                Console.WriteLine(" ");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Sorry, no recipes were found. There may be an error in your tag name, otherwise your tag has no recipes listed under it.");
                        }

                        Console.ReadLine();
                        break;

                    case "3":
                        Console.WriteLine("Enter a nutrition related question like 'How much vitamin c is in 1 orange'");
                        string result3 = APIController.GetAnswer(Console.ReadLine().ToLower());

                        if (result3 != null)
                        {
                            Console.WriteLine("Answer: " + result3);
                        }
                        else
                        {
                            Console.WriteLine("No answer available");
                        }
                        break;

                    case "4":
                        string joke = APIController.GetRandomTriviaOrJoke(true);

                        if (joke != null) Console.WriteLine("Joke: " + joke);
                        else Console.WriteLine("There was an error :(");
                        break;

                    case "5":
                        string trivia = APIController.GetRandomTriviaOrJoke(false);

                        if (trivia != null) Console.WriteLine("Trivia: " + trivia);
                        else Console.WriteLine("There was an error :(");
                        break;
                }
            }

            //List<Recipe> result = APIController.GetRandomRecipe("vegan");
            //foreach (Recipe recipe in result)
            //{
            //    Console.WriteLine("Id: " + recipe.ID);
            //    Console.WriteLine("Title: " + recipe.Title);
            //    Console.Write(recipe.SourceUrl);
            //    Console.WriteLine(" ");
            //}
            //Console.ReadLine();

            //string response = APIController.ConnectUser();
            //Console.WriteLine(response);
            //Console.ReadLine(); 

            //string response = APIController.GetAnswer("How much");
            //Console.WriteLine(response);
            //Console.ReadLine();
        }
    }
}
