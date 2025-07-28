using System.Collections.Generic;
using Newtonsoft.Json;

namespace MainMigration.Models
{
    public class CaptchaResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("error-codes")]
        public List<string>? ErrorCodes { get; set; }
    }
}