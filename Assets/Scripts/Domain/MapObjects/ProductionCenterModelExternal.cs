using System.Collections.Generic;
using TrenchWarfare.Domain.Enums;
using TrenchWarfare.Domain.Map;

namespace TrenchWarfare.Domain.MapObjects {
    public interface ProductionCenterModelExternal: IModel {
        CellModelExternal Cell { get; }

        ProductionCenterType Type { get; }

        ProductionCenterLevel Level { get; }

        public string Name { get; }

        Nation Nation { get; }

        List<ProductionCenterLevel> GetLevelsForType(ProductionCenterType type);
    }
}

