namespace kokshengbi.Contracts.Common
{
    public class PageRequest
    {
        public int? Current { get; set; } = 1; // Default page number
        public int? PageSize { get; set; } = 10; // Default page size
        public string? SortField { get; set; }
        public string? SortOrder { get; set; }

        public void ApplyDefaults()
        {
            if (!Current.HasValue || Current.Value <= 0)
            {
                Current = 1;
            }
            if (!PageSize.HasValue || PageSize.Value <= 0)
            {
                PageSize = 10;
            }
        }
    }
}
