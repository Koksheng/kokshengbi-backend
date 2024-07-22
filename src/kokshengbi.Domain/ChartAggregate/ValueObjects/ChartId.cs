using kokshengbi.Domain.Common.Models;

namespace kokshengbi.Domain.ChartAggregate.ValueObjects
{
    public sealed class ChartId : AggregateRootId<int>
    {
        public override int Value { get; protected set; }
        private ChartId(int value)
        {
            Value = value;
        }
        public static ChartId Create(int value)
        {
            return new ChartId(value);
        }
        //public static InterfaceInfoId CreateUnique()
        //{
        //    int uniqueId = GenerateUniqueId();
        //    return new InterfaceInfoId(uniqueId);
        //}
        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
        //private static int GenerateUniqueId()
        //{
        //    // Simulate an ID generation logic
        //    Random random = new Random();
        //    return random.Next(1, int.MaxValue);
        //}
    }
}
