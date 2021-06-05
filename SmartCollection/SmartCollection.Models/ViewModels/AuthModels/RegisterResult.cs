using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCollection.Models.ViewModels.AuthModels
{
    [JsonObject]
    public class RegisterResult
    {
        [JsonProperty]
        public bool Succesfull { get; set; }
        [JsonProperty]
        public IEnumerable<string> Errors { get; set; }
    }
}
