using System;
using TrenchWarfare.Domain.Enums;
using TrenchWarfare.Domain.Map;

namespace TrenchWarfare.Domain.MapObjects {
    public class TerrainModifierModel: Model, TerrainModifierModelExternal {
        TerrainModifierType modifier;
        public TerrainModifierType Modifier { get => modifier; set => modifier = value; }

        CellModelExternal cell = null;
        public CellModelExternal Cell { get => cell; set => cell = value; }

        public TerrainModifierModel(TerrainModifierType modifier) {
            this.modifier = modifier;
        }

        public bool CanPlace(CellModelExternal cell) {
            if (cell.ProductionCenter != null) {
                return false;
            }

            if (cell.TerrainModifier != null) {
                return false;
            }

            if (cell.IsUnderwater && modifier != TerrainModifierType.SeaMine) {
                return false;
            }

            if (!cell.IsUnderwater && modifier == TerrainModifierType.SeaMine) {
                return false;
            }

            if (cell.Army != null &&
                (modifier == TerrainModifierType.SeaMine || modifier == TerrainModifierType.LandMine)
            ) {
                return false;
            }

            return true;
        }

        public TerrainModifierModel Copy(Func<TerrainModifierModel, TerrainModifierModel> update) {
            var copy = update((TerrainModifierModel)MemberwiseClone());
            copy.Id = NewId();
            return copy;
        }
    }
}

