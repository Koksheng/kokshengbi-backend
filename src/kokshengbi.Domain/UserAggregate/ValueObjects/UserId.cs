using kokshengbi.Domain.Common.Models;

namespace kokshengbi.Domain.UserAggregate.ValueObjects
{
    public sealed class UserId : AggregateRootId<int>
    {
        public override int Value { get; protected set; }
        public UserId(int value)
        {
            Value = value;
        }
        public static UserId Create(int value)
        {
            return new UserId(value);
        }
        //public static UserId CreateUnique()
        //{
        //    // Assuming you have a mechanism to generate unique integers, like an auto-incremented ID from a database.
        //    // Here, just for example, let's simulate generating a unique integer ID.
        //    // This should be replaced with your actual unique ID generation logic.
        //    int uniqueId = GenerateUniqueId();
        //    return new UserId(uniqueId);
        //}
        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
        //private static int GenerateUniqueId()
        //{
        //    // This is a placeholder. Replace it with your unique integer ID generation logic.
        //    // For example, you might fetch this from a database sequence or an auto-increment column.
        //    // Here, we'll just use a static counter for demonstration purposes.

        //    // Simulate an ID generation logic
        //    Random random = new Random();
        //    return random.Next(1, int.MaxValue);
        //}
    }
}
