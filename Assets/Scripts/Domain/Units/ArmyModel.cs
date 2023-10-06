using System;
using System.Collections.Generic;
using TrenchWarfare.Domain.Map;

namespace TrenchWarfare.Domain.Units {
    public class ArmyModel: Model, ArmyModelExternal {
        CellModelExternal cell;
        public CellModelExternal Cell { get => cell; set => cell = value; }

        List<UnitModel> units;
        public IEnumerable<UnitModelExternal> Units { get => units; }

        private const int maxUnits = 4;

        public UnitModel DisplayUnit { get => units[^1]; }

        public int Quantity { get => units.Count; }

        public bool CanAdd { get => Quantity < maxUnits; }

        public ArmyModel(UnitModel unit): base() {
            units = new List<UnitModel>(maxUnits) { unit };
        }

        public bool AddUnit(UnitModel unit) {
            if (Quantity == maxUnits) {
                return false;
            }

            units.Add(unit);

            return true;
        }

        public bool RemoveLastUnit() {
            if (Quantity == 0) {
                return false;
            }

            units.RemoveAt(units.Count - 1);

            return Quantity > 0;
        }
    }
}

