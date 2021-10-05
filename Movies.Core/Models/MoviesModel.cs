using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Movies.Core.Model
{
#nullable enable
    [BsonIgnoreExtraElements]
    public class MoviesModel
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("title")]
        public string? Title { get; set; }

        [BsonElement("rated")]
        public string? Rated { get; set; }

        [BsonElement("runtime")]
        public int? Runtime { get; set; }

        [BsonElement("year")]
        public int? Year { get; set; }

        [BsonElement("genres")]
        public string[]? Genres { get; set; }

        [BsonElement("cast")]
        public string[]? Cast { get; set; }

        [BsonElement("directors")]
        public string[]? Directors { get; set; }
        public double Similarity { get; set; }
        [BsonElement("poster")]
        public string? Poster { get; set; }
    }
}
