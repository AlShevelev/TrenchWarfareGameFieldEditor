using System;
using TrenchWarfare.Domain.Enums;
using TrenchWarfare.Domain.Map;

namespace TrenchWarfare.Domain.Units {
    public class UnitModel: Model, UnitModelExternal {
        private UnitExperienceRank experienceRank;

        // for experience rank calculation
        private int tookPartInBattles;

        private UnitType type;
        public UnitType Type { get => type; set => type = value; }

        private Nation nation;
        public Nation Nation { get => nation; set => nation = value; }

        /// <summary>
        /// [0, 1]
        /// </summary>
        private float fatigue;
        public float Fatigue { get => fatigue; set => fatigue = value; }

        private float health;
        public float Health { get => health; set => health = value; }

        private float movementPoints;
        public float MovementPoints { get => movementPoints; set => movementPoints = value; }

        public UnitExperienceRank ExperienceRank {
            get => experienceRank; set => experienceRank = value;
        }

        public int TookPartInBattles { get => tookPartInBattles; set => tookPartInBattles = value; }

        private UnitBoost boost1;
        public UnitBoost Boost1 { get => boost1; set => boost1 = value; }

        private UnitBoost boost2;
        public UnitBoost Boost2 { get => boost2; set => boost2 = value; }

        private UnitBoost boost3;
        public UnitBoost Boost3 { get => boost3; set => boost3 = value; }

        public float MaxHealth {
            get {
                return type switch {
                    UnitType.ArmoredCar => 8f,
                    UnitType.Artillery => 5f,
                    UnitType.Battleship => 50f,
                    UnitType.Carrier => 15f,
                    UnitType.Cavalry => 6f,
                    UnitType.Cruser => 25f,
                    UnitType.Destroyer => 15f,
                    UnitType.Infantry => 5f,
                    UnitType.MachineGunnersCart => 6f,
                    UnitType.MachineGuns => 5f,
                    UnitType.Tank => 10f,
                    _ => throw new NotImplementedException()
                };
            }
        }

        public float MaxMovementPoints {
            get {
                return type switch {
                    UnitType.ArmoredCar => 3f,
                    UnitType.Artillery => 1f,
                    UnitType.Battleship => 2f,
                    UnitType.Carrier => 1f,
                    UnitType.Cavalry => 4f,
                    UnitType.Cruser => 3f,
                    UnitType.Destroyer => 4f,
                    UnitType.Infantry => 2f,
                    UnitType.MachineGunnersCart => 3f,
                    UnitType.MachineGuns => 1f,
                    UnitType.Tank => 2f,
                    _ => throw new NotImplementedException()
                };
            }
        }

        public float Attack {
            get {
                return type switch {
                    UnitType.ArmoredCar => 3f,
                    UnitType.Artillery => 5f,
                    UnitType.Battleship => 12f,
                    UnitType.Carrier => 0f,
                    UnitType.Cavalry => 2f,
                    UnitType.Cruser => 8f,
                    UnitType.Destroyer => 5f,
                    UnitType.Infantry => 1f,
                    UnitType.MachineGunnersCart => 3f,
                    UnitType.MachineGuns => 1f,
                    UnitType.Tank => 4f,
                    _ => throw new NotImplementedException()
                };
            }
        }

        public float Defence {
            get {
                return type switch {
                    UnitType.ArmoredCar => 5f,
                    UnitType.Artillery => 1f,
                    UnitType.Battleship => 20f,
                    UnitType.Carrier => 0f,
                    UnitType.Cavalry => 2f,
                    UnitType.Cruser => 10f,
                    UnitType.Destroyer => 8f,
                    UnitType.Infantry => 1f,
                    UnitType.MachineGunnersCart => 3f,
                    UnitType.MachineGuns => 3f,
                    UnitType.Tank => 8f,
                    _ => throw new NotImplementedException()
                };
            }
        }

        public float DamageMin {
            get {
                return type switch {
                    UnitType.ArmoredCar => 3f,
                    UnitType.Artillery => 3f,
                    UnitType.Battleship => 14f,
                    UnitType.Carrier => 0f,
                    UnitType.Cavalry => 1f,
                    UnitType.Cruser => 8f,
                    UnitType.Destroyer => 5f,
                    UnitType.Infantry => 1f,
                    UnitType.MachineGunnersCart => 2f,
                    UnitType.MachineGuns => 2f,
                    UnitType.Tank => 5f,
                    _ => throw new NotImplementedException()
                };
            }
        }

        public float DamageMax {
            get {
                return type switch {
                    UnitType.ArmoredCar => 4f,
                    UnitType.Artillery => 6f,
                    UnitType.Battleship => 16f,
                    UnitType.Carrier => 0f,
                    UnitType.Cavalry => 3f,
                    UnitType.Cruser => 10f,
                    UnitType.Destroyer => 6f,
                    UnitType.Infantry => 3f,
                    UnitType.MachineGunnersCart => 3f,
                    UnitType.MachineGuns => 3f,
                    UnitType.Tank => 6f,
                    _ => throw new NotImplementedException()
                };
            }
        }

        public float CostInMoney {
            get {
                return type switch {
                    UnitType.ArmoredCar => 1f,
                    UnitType.Artillery => 1f,
                    UnitType.Battleship => 4f,
                    UnitType.Carrier => 0f,
                    UnitType.Cavalry => 2f,
                    UnitType.Cruser => 3f,
                    UnitType.Destroyer => 2f,
                    UnitType.Infantry => 1f,
                    UnitType.MachineGunnersCart => 2f,
                    UnitType.MachineGuns => 2f,
                    UnitType.Tank => 1f,
                    _ => throw new NotImplementedException()
                };
            }
        }

        public float CostInIndustryPoints {
            get {
                return type switch {
                    UnitType.ArmoredCar => 3f,
                    UnitType.Artillery => 3f,
                    UnitType.Battleship => 12f,
                    UnitType.Carrier => 0f,
                    UnitType.Cavalry => 0f,
                    UnitType.Cruser => 8f,
                    UnitType.Destroyer => 6f,
                    UnitType.Infantry => 0f,
                    UnitType.MachineGunnersCart => 2f,
                    UnitType.MachineGuns => 1f,
                    UnitType.Tank => 4f,
                    _ => throw new NotImplementedException()
                };
            }
        }

        public bool IsLand {
            get {
                return type switch {
                    UnitType.ArmoredCar => true,
                    UnitType.Artillery => true,
                    UnitType.Battleship => false,
                    UnitType.Carrier => false,
                    UnitType.Cavalry => true,
                    UnitType.Cruser => false,
                    UnitType.Destroyer => false,
                    UnitType.Infantry => true,
                    UnitType.MachineGunnersCart => true,
                    UnitType.MachineGuns => true,
                    UnitType.Tank => true,
                    _ => throw new NotImplementedException()
                };
            }
        }

        public int NeedCityLevelToBuild {
            get {
                return type switch {
                    UnitType.ArmoredCar => 0,
                    UnitType.Artillery => 0,
                    UnitType.Battleship => 0,
                    UnitType.Carrier => 0,
                    UnitType.Cavalry => 2,
                    UnitType.Cruser => 0,
                    UnitType.Destroyer => 0,
                    UnitType.Infantry => 1,
                    UnitType.MachineGunnersCart => 0,
                    UnitType.MachineGuns => 2,
                    UnitType.Tank => 0,
                    _ => throw new NotImplementedException()
                };
            }
        }

        public int NeedFactoryLevelToBuild {
            get {
                return type switch {
                    UnitType.ArmoredCar => 3,
                    UnitType.Artillery => 2,
                    UnitType.Battleship => 0,
                    UnitType.Carrier => 0,
                    UnitType.Cavalry => 0,
                    UnitType.Cruser => 0,
                    UnitType.Destroyer => 0,
                    UnitType.Infantry => 0,
                    UnitType.MachineGunnersCart => 1,
                    UnitType.MachineGuns => 0,
                    UnitType.Tank => 4,
                    _ => throw new NotImplementedException()
                };
            }
        }

        public int NeedNavalBaseLevelToBuild {
            get {
                return type switch {
                    UnitType.ArmoredCar => 0,
                    UnitType.Artillery => 0,
                    UnitType.Battleship => 3,
                    UnitType.Carrier => 0,
                    UnitType.Cavalry => 0,
                    UnitType.Cruser => 2,
                    UnitType.Destroyer => 1,
                    UnitType.Infantry => 0,
                    UnitType.MachineGunnersCart => 0,
                    UnitType.MachineGuns => 0,
                    UnitType.Tank => 0,
                    _ => throw new NotImplementedException()
                };
            }
        }

        public UnitModel(
            UnitType type,
            Nation nation
        ): base() {
            this.type = type;
            this.nation = nation;
            fatigue = 0f;
            health = MaxHealth;
            movementPoints = MaxMovementPoints;
            experienceRank = UnitExperienceRank.Rookies;
            tookPartInBattles = 0;
            boost1 = UnitBoost.None;
            boost2 = UnitBoost.None;
            boost3 = UnitBoost.None;
        }


        public UnitModel(
            UnitType type,
            Nation nation,
            float fatigue,
            float health,
            float movementPoints,
            UnitExperienceRank experienceRank,
            int tookPartInBattles,
            UnitBoost boost1,
            UnitBoost boost2,
            UnitBoost boost3
        ): base() {
            this.type = type;
            this.nation = nation;
            this.fatigue = fatigue;
            this.health = health;
            this.movementPoints = movementPoints;
            this.experienceRank = experienceRank;
            this.tookPartInBattles = tookPartInBattles;
            this.boost1 = boost1;
            this.boost2 = boost2;
            this.boost3 = boost3;
        }

        public UnitModel Copy(Func<UnitModel, UnitModel> update) {
            var copy = update((UnitModel)MemberwiseClone());
            copy.Id = NewId();
            return copy;
        }

        public int GetBattlesForExperienceRank(UnitExperienceRank unitExperienceRank) {
            return unitExperienceRank switch {
                UnitExperienceRank.Rookies => 0,
                UnitExperienceRank.Fighters => 1,
                UnitExperienceRank.Proficients => 3,
                UnitExperienceRank.Veterans => 6,
                UnitExperienceRank.Elite => 10,
                _ => throw new NotImplementedException()
            };
        }

        public bool CanPlace(CellModelExternal cell) {
            if (!IsLand && cell.IsUnderwater) {
                return true;
            }

            if (IsLand && cell.IsUnderwater) {
                return false;
            }

            switch(type) {
                case UnitType.ArmoredCar: {
                    return cell.TerrainType == CellTerrain.Plain ||
                            cell.TerrainType == CellTerrain.Wood ||
                            cell.TerrainType == CellTerrain.Sand ||
                            cell.TerrainType == CellTerrain.Hills ||
                            cell.TerrainType == CellTerrain.Snow;
                }
                case UnitType.Artillery: {
                    return cell.TerrainType == CellTerrain.Plain ||
                            cell.TerrainType == CellTerrain.Wood ||
                            cell.TerrainType == CellTerrain.Sand ||
                            cell.TerrainType == CellTerrain.Hills ||
                            cell.TerrainType == CellTerrain.Snow;
                }
                case UnitType.Cavalry: {
                    return cell.TerrainType == CellTerrain.Plain ||
                            cell.TerrainType == CellTerrain.Wood ||
                            cell.TerrainType == CellTerrain.Marsh ||
                            cell.TerrainType == CellTerrain.Sand ||
                            cell.TerrainType == CellTerrain.Hills ||
                            cell.TerrainType == CellTerrain.Snow;
                }
                case UnitType.Infantry: {
                    return cell.TerrainType == CellTerrain.Plain ||
                            cell.TerrainType == CellTerrain.Wood ||
                            cell.TerrainType == CellTerrain.Marsh ||
                            cell.TerrainType == CellTerrain.Sand ||
                            cell.TerrainType == CellTerrain.Hills ||
                            cell.TerrainType == CellTerrain.Mountains ||
                            cell.TerrainType == CellTerrain.Snow;
                }
                case UnitType.MachineGunnersCart: {
                    return cell.TerrainType == CellTerrain.Plain ||
                            cell.TerrainType == CellTerrain.Wood ||
                            cell.TerrainType == CellTerrain.Sand ||
                            cell.TerrainType == CellTerrain.Hills ||
                            cell.TerrainType == CellTerrain.Snow;
                }
                case UnitType.MachineGuns: {
                    return cell.TerrainType == CellTerrain.Plain ||
                            cell.TerrainType == CellTerrain.Wood ||
                            cell.TerrainType == CellTerrain.Marsh ||
                            cell.TerrainType == CellTerrain.Sand ||
                            cell.TerrainType == CellTerrain.Hills ||
                            cell.TerrainType == CellTerrain.Snow;
                }
                case UnitType.Tank: {
                    return cell.TerrainType == CellTerrain.Plain ||
                            cell.TerrainType == CellTerrain.Wood ||
                            cell.TerrainType == CellTerrain.Sand ||
                            cell.TerrainType == CellTerrain.Hills ||
                            cell.TerrainType == CellTerrain.Snow;
                }
                default: return false;
            }
        }
    }
}
