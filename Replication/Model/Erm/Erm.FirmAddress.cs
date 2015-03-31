// ReSharper disable once CheckNamespace
namespace NuClear.AdvancedSearch.Replication.Model
{
    public static partial class Erm
    {
        public sealed class FirmAddress : IIdentifiable
        {
            public FirmAddress()
            {
                IsActive = true;
            }

            public long Id { get; set; }

            public long FirmId { get; set; }

            public bool ClosedForAscertainment { get; set; }

            public bool IsActive { get; set; }

            public bool IsDeleted { get; set; }
        }
    }
}