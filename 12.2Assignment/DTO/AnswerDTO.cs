using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _12._2Assignment
{
    public class AnswerDTO
    {
        [JsonProperty("answer")]
        public string Respone { get; set; }
    }
}
