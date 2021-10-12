using Newtonsoft.Json;

namespace EmmaSharp.Models.SignupForms
{
    public class SignupForm
    {
        [JsonProperty("id")]
        public long? SignupFormId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
