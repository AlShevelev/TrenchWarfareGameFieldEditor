using System;
using System.Collections.Generic;
using TrenchWarfare.Domain.Map;

namespace TrenchWarfare.Domain.Units {
    public interface ArmyModelExternal: IModel {
        CellModelExternal Cell { get; }

        IEnumerable<UnitModelExternal> Units { get; }
    }
}

