using TrenchWarfare.Domain.Enums;
using TrenchWarfare.Domain.Map;

namespace TrenchWarfare.Domain.MapObjects
{
    public interface TerrainModifierModelExternal: IModel {
        TerrainModifierType Modifier { get; }

        CellModelExternal Cell { get; }
    }
}

