using System;
using TrenchWarfare.Domain.Enums;

namespace TrenchWarfare.Domain.Units {
    public class UnitInfo {
        private UnitType _type;

        private Nation _nation;

        /// <summary>
        /// [0, 1]
        /// </summary>
        private float _fatigue;

        private float _health;

        private float _movementPoints;

        private UnitExperienceRank _experienceRank;

        // for experience rank calculation
        private int _tookPartInBattles;

        public UnitType Type { get => _type; set => _type = value; }

        public Nation Nation { get => _nation; set => _nation = value; }

        public float Fatigue { get => _fatigue; set => _fatigue = value; }

        public float Health { get => _health; set => _health = value; }

        public float MovementPoints { get => _movementPoints; set => _movementPoints = value; }

        public UnitExperienceRank ExperienceRank {
            get => _experienceRank; set => _experienceRank = value;
        }

        public int TookPartInBattles {
            get => _tookPartInBattles; set => _tookPartInBattles = value;
        }

        public float MaxHealth {
            get {
                return _type switch {
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
                return _type switch {
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
                return _type switch {
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
                return _type switch {
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
                return _type switch {
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
                return _type switch {
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
                return _type switch {
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
                return _type switch {
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
                return _type switch {
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
                return _type switch {
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
                return _type switch {
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
                return _type switch {
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

        public UnitInfo(
            UnitType type,
            Nation nation
        ) {
            _type = type;
            _nation = nation;
            _fatigue = 0f;
            _health = MaxHealth;
            _movementPoints = MaxMovementPoints;
            _experienceRank = UnitExperienceRank.Rookies;
            _tookPartInBattles = 0;
        }


        public UnitInfo(
            UnitType type,
            Nation nation,
            float fatigue,
            float health,
            float movementPoints,
            UnitExperienceRank experienceRank,
            int tookPartInBattles) {
            _type = type;
            _nation = nation;
            _fatigue = fatigue;
            _health = health;
            _movementPoints = movementPoints;
            _experienceRank = experienceRank;
            _tookPartInBattles = tookPartInBattles;
        }

        public UnitInfo Copy(Func<UnitInfo, UnitInfo> update) {
            return update((UnitInfo)MemberwiseClone());
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
    }
}
