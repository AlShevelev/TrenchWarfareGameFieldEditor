using System;
using TrenchWarfare.Domain.Map;

namespace TrenchWarfare.Domain.Units {
    public interface ArmyModelExternal: IModel {
        CellModelExternal Cell { get; }
    }
}

