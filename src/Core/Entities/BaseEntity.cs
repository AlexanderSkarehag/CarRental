using System.Text.Json.Serialization;
using Core.Interfaces;

namespace Core.Entities
{
    public class BaseEntity<Key> : IAggregateRoot<Key>
    {
        [JsonPropertyName("id")]
        public required Key Id { get; set; }
        public DateTimeOffset InsertedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
