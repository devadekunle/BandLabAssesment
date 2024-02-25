using Newtonsoft.Json;
using System;

namespace BandLabAssesment.Domain;

public abstract class BaseEntity
{
    [JsonProperty("id")]
    protected string Id { get; set; }
}