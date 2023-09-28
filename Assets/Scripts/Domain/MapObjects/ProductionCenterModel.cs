using System;
using System.Collections.Generic;
using TrenchWarfare.Domain.Enums;
using TrenchWarfare.Domain.Map;

namespace TrenchWarfare.Domain.MapObjects {
    public class ProductionCenterModel: Model, ProductionCenterModelExternal {
        CellModelExternal cell = null;
        public CellModelExternal Cell { get => cell; set => cell = value; }

        ProductionCenterType type;
        public ProductionCenterType Type { get => type; set => type = value; }

        ProductionCenterLevel level;
        public ProductionCenterLevel Level { get => level; set => level = value; }

        string name = null;
        public string Name { get => name; set => name = value; }

        private Nation nation;
        public Nation Nation { get => nation; set => nation = value; }

        public ProductionCenterModel(
            ProductionCenterType type,
            ProductionCenterLevel level,
            Nation nation,
            string name = null
        ) {
            this.type = type;
            this.level = level;
            this.name = name;
            this.nation = nation;
        }

        public List<ProductionCenterLevel> GetLevelsForType(ProductionCenterType type) {
            return type switch {
                ProductionCenterType.City => new List<ProductionCenterLevel> {
                    ProductionCenterLevel.Level1,
                    ProductionCenterLevel.Level2,
                    ProductionCenterLevel.Level3,
                    ProductionCenterLevel.Level4,
                    ProductionCenterLevel.Capital,
                },

                ProductionCenterType.Factory => new List<ProductionCenterLevel> {
                    ProductionCenterLevel.Level1,
                    ProductionCenterLevel.Level2,
                    ProductionCenterLevel.Level3,
                    ProductionCenterLevel.Level4,
                },

                ProductionCenterType.NavalBase => new List<ProductionCenterLevel> {
                    ProductionCenterLevel.Level1,
                    ProductionCenterLevel.Level2,
                    ProductionCenterLevel.Level3,
                },

                ProductionCenterType.AirField => new List<ProductionCenterLevel> {
                    ProductionCenterLevel.Level1,
                    ProductionCenterLevel.Level2,
                },

                _ => throw new NotImplementedException()
            };
        }

        public ProductionCenterModel Copy(Func<ProductionCenterModel, ProductionCenterModel> update) {
            var copy = update((ProductionCenterModel)MemberwiseClone());
            copy.Id = NewId();
            return copy;
        }

        public bool CanPlace(CellModelExternal cell) {
            if (cell.IsUnderwater && type != ProductionCenterType.NavalBase) {
                return false;
            }

            if (!cell.IsUnderwater && type == ProductionCenterType.NavalBase) {
                return false;
            }

            // A bridge is not supported
            if (cell.HasRoads && cell.HasRiver) {
                return false;
            }

            if (cell.Army != null) {
                return false;
            }

            // no terrain modifiers

            if (cell.IsUnderwater && type == ProductionCenterType.NavalBase) {
                return true;
            }

            switch(cell.TerrainType) {
                case CellTerrain.Plain: return type != ProductionCenterType.NavalBase;
                case CellTerrain.Wood: return type == ProductionCenterType.City ||
                        type == ProductionCenterType.AirField;
                case CellTerrain.Marsh: return false;
                case CellTerrain.Sand: return type == ProductionCenterType.City ||
                        type == ProductionCenterType.AirField;
                case CellTerrain.Hills: return type == ProductionCenterType.City ||
                        type == ProductionCenterType.Factory;
                case CellTerrain.Mountains: return false;
                case CellTerrain.Snow: return type == ProductionCenterType.City ||
                        type == ProductionCenterType.Factory;

                default: return false;
            }
        }
    }
}
