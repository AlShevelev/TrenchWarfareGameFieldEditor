using System;
using System.Collections.Generic;
using TrenchWarfare.Domain.Map.Conditions;

namespace TrenchWarfare.Domain.Map {
    public interface GridModelExternal: IModel {
        MapConditionsExternal Conditions { get; }

        int CellCountX { get; }
        int CellCountZ { get; }

        IEnumerable<CellModelExternal> Cells { get; }
    }
}
