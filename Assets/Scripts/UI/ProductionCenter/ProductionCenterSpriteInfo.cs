using System;
using TrenchWarfare.Domain.Enums;
using TrenchWarfare.Domain.MapObjects;

namespace TrenchWarfare.UI.ProductionCenter {
    public static class ProductionCenterSpriteInfo {
        public static string GetBackgroundSprite(ProductionCenterModel model) {
            return model.Type switch {
                ProductionCenterType.AirField => "Air Field Bcg",
                ProductionCenterType.City => "City Bcg",
                ProductionCenterType.Factory => "Factory Bcg",
                ProductionCenterType.NavalBase => "Naval Base Bcg",
                _ => throw new NotImplementedException(),
            };
        }

        public static string GetCenterSprite(ProductionCenterModel model) {
            switch(model.Type) {
                case ProductionCenterType.AirField: {
                    return model.Level switch {
                        ProductionCenterLevel.Level1 => "Air Field Level 1",
                        ProductionCenterLevel.Level2 => "Air Field Level 2",
                        _ => throw new NotImplementedException(),
                    };
                }
                case ProductionCenterType.City: {
                    return model.Level switch {
                        ProductionCenterLevel.Level1 => "City Level 1",
                        ProductionCenterLevel.Level2 => "City Level 2",
                        ProductionCenterLevel.Level3 => "City Level 3",
                        ProductionCenterLevel.Level4 => "City Level 4",
                        ProductionCenterLevel.Capital => "City Level 5",
                        _ => throw new NotImplementedException(),
                    };
                }
                case ProductionCenterType.Factory: {
                    return model.Level switch {
                        ProductionCenterLevel.Level1 => "Factory Level 1",
                        ProductionCenterLevel.Level2 => "Factory Level 2",
                        ProductionCenterLevel.Level3 => "Factory Level 3",
                        ProductionCenterLevel.Level4 => "Factory Level 4",
                        _ => throw new NotImplementedException(),
                    };
                }
                case ProductionCenterType.NavalBase: {
                    return model.Level switch {
                        ProductionCenterLevel.Level1 => "Naval Base Level 1",
                        ProductionCenterLevel.Level2 => "Naval Base Level 2",
                        ProductionCenterLevel.Level3 => "Naval Base Level 3",
                        _ => throw new NotImplementedException(),
                    };
                }
				default: throw new NotImplementedException();
            };
        }

        public static string GetLevelSprite(ProductionCenterModel model) {
            return model.Level switch {
                ProductionCenterLevel.Level1 => "PC Level 1",
                ProductionCenterLevel.Level2 => "PC Level 2",
                ProductionCenterLevel.Level3 => "PC Level 3",
                ProductionCenterLevel.Level4 => "PC Level 4",
                ProductionCenterLevel.Capital => "PC Level Capital",
                _ => throw new NotImplementedException(),
            };
        }
    }
}

