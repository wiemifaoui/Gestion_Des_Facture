using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Domain
{
    public class Convertion
    {
        [JsonProperty("rates")]
        public Rate Rates { get; set; }
        [JsonProperty("motd")]
        public Motd Motd { get; set; }
        [JsonProperty("success")]
        public string Success { get; set; }
        [JsonProperty("historical")]
        public string Historical { get; set; }
        [JsonProperty("base")]
        public string Base { get; set; }
        [JsonProperty("date")]
        public string Date { get; set; }

    }
}
