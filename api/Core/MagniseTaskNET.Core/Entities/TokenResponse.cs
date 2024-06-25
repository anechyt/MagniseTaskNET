using System.Text.Json.Serialization;

namespace MagniseTaskNET.Core.Entities
{
    public class TokenResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }
    }
}
