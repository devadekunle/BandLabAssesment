using Newtonsoft.Json;

namespace BandLabAssesment.Domain;

public abstract class BaseEntity
{
    [JsonProperty("id")]
    public string Id { get; protected set; }
}