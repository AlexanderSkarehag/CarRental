namespace Core.Interfaces
{
    public interface IAggregateRoot<Key>
    {
        public Key Id { get; set; }
        public DateTimeOffset InsertedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
