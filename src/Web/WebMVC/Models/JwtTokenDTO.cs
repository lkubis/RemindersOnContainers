using System;
using Newtonsoft.Json;

namespace WebMVC.Models
{
    public class JwtTokenDTO
    {
        [JsonProperty(PropertyName = "token")]
        public string Token { get; set; }

        [JsonProperty(PropertyName = "expiration")]
        public DateTimeOffset Expiration { get; set; }
    }
}