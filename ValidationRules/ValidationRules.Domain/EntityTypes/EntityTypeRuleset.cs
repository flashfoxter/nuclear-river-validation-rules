using NuClear.Model.Common.Entities;
using NuClear.River.Common.Metadata;

namespace NuClear.ValidationRules.Domain.EntityTypes
{
    public sealed class EntityTypeRuleset : EntityTypeBase<EntityTypeRuleset>
    {
        public override int Id { get; } = EntityTypeIds.Ruleset;
        public override string Description { get; } = nameof(EntityTypeIds.Ruleset);
    }
}
