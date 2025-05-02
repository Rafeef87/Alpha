using System.Text.Json.Serialization;

namespace WebApp.Models;

public class CookieConsent
{
    [JsonPropertyName("necessary")]
    public bool Essential { get; set; }
    [JsonPropertyName("functional")]
    public bool Functional { get; set; }
    [JsonPropertyName("analytics")]
    public bool Analytics { get; set; }
    [JsonPropertyName("marketing")]
    public bool Marketing { get; set; }
   
}
