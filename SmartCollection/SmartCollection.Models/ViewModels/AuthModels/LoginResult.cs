using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCollection.Models.ViewModels.AuthModels
{
    [JsonObject]
    public class LoginResult
    {
        [JsonProperty]
        public bool Successful { get; set; }
        [JsonProperty]
        public string Token { get; set; }
        [JsonProperty]
        public string Error { get; set; }
    }
}
