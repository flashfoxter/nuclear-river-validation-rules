namespace NuClear.ValidationRules.Domain.Model.Facts
{
    public sealed class GlobalAssociatedPosition : IErmFactObject
    {
        public long Id { get; set; }
        public long RulesetId { get; set; }
        public long AssociatedPositionId { get; set; }
        public long PrincipalPositionId { get; set; }
        public int ObjectBindingType { get; set; }
    }
}
