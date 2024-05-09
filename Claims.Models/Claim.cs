﻿using MongoDB.Bson.Serialization.Attributes;

namespace Claims.Models;

public class Claim
{
    [BsonId]
    public string Id { get; set; }

    [BsonElement("coverId")]
    public string CoverId { get; set; }

    [BsonElement("created")]
    [BsonDateTimeOptions(DateOnly = true)]
    public DateTime Created { get; set; }

    [BsonElement("name")]
    public string Name { get; set; }

    [BsonElement("claimType")]
    public ClaimType Type { get; set; }

    [BsonElement("damageCost")]
    public decimal DamageCost { get; set; }
}
