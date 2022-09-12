using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _12._2Assignment
{
    public class APIManager
    {
        /// <summary>
        /// The field that is used to represent the connection string to the API.
        /// </summary>
        private string connectionString;

        /// <summary>
        /// The field that is used to represent the API Key.
        /// </summary>
        private string apiKey;

        /// <summary>
        /// Initializes a new instance of the APIManager class.
        /// </summary>
        /// <param name="connectionString">The specified connection string.</param>
        /// <param name="apiKey">The specififed api key.</param>
        public APIManager(string connectionString, string apiKey)
        {
            this.connectionString = connectionString;
            this.apiKey = apiKey;
        }

        /// <summary>
        /// Gets the connection string.
        /// </summary>
        public string ConnectionString => this.connectionString;

        /// <summary>
        /// Gets or sets the api key.
        /// </summary>
        public string APIKey
        {
            get => this.apiKey;
            set => this.apiKey = value;
        }

        /// <summary>
        /// Gets or sets the request token.
        /// </summary>
        //public Token RequestToken { get; set; }
    }
}
