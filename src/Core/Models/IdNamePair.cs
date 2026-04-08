namespace Core.Models
{
    public class IdValuePair<T>
    {
        public required T Id { get; set; }
        public string? Value { get; set; }
    }
}
