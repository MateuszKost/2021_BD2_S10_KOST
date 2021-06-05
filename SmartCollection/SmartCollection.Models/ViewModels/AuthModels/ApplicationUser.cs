using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCollection.Models.ViewModels.AuthModels
{
    public class ApplicationUser
    {
        [JsonProperty]
        public bool IsAuthenticated { get; set; }
        [JsonProperty]
        public string UserName { get; set; }
        [JsonProperty]
        public string Email { get; set; }
        [JsonProperty]
        public string Id { get; set; }
        [JsonProperty]
        public Dictionary<string, string> Claims { get; set; }
    }
}
