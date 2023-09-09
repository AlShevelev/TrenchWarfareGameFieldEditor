using System;
using TrenchWarfare.Domain.Map.Conditions;

namespace TrenchWarfare.Domain.Map {
    public interface GridModelExternal: IModel {
        MapConditionsExternal Conditions { get; }
    }
}
