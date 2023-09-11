using System;
using TrenchWarfare.Domain.Enums;
using TrenchWarfare.Domain.Units;
using UnityEditor.PackageManager;

namespace TrenchWarfare.UI {
	public static class UnitSpritesInfo {
    	public static string GetUnitSprite(UnitModel unitInfo) {
			switch (unitInfo.Type) {
				case UnitType.ArmoredCar: return "Unit Armored car";
				case UnitType.Artillery: return "Unit Artillery";
				case UnitType.Infantry: {
                        return unitInfo.Nation switch {
                            Nation.AustriaHungary => "Unit Infantry Austro-Hungary",
                            Nation.Belgium => "Unit Infantry Belgia",
                            Nation.Bulgaria => "Unit Infantry Bulgaria",
                            Nation.China => "Unit Infantry China",
                            Nation.France => "Unit Infantry France",
                            Nation.Germany => "Unit Infantry Germany",
                            Nation.GreatBritain => "Unit Infantry UK",
                            Nation.Greece => "Unit Infantry Greece",
                            Nation.Italy => "Unit Infantry Italy",
                            Nation.Japan => "Unit Infantry Japan",
                            Nation.Korea => "Unit Infantry Korea",
                            Nation.Mexico => "Unit Infantry Mexico",
                            Nation.Mongolia => "Unit Infantry Mongolia",
                            Nation.Montenegro => "Unit Infantry Montenegro",
                            Nation.Romania => "Unit Infantry Romania",
                            Nation.Russia => "Unit Infantry Russia",
                            Nation.Serbia => "Unit Infantry Serbia",
                            Nation.Turkey => "Unit Infantry Turkey",
                            Nation.USA => "Unit Infantry US",
                            Nation.USNorth => "Unit Infantry US North",
                            Nation.USSouth => "Unit Infantry US South",
                            _ => throw new NotImplementedException(),
                        };
                    };
				case UnitType.Cavalry: {
                    return unitInfo.Nation switch {
                        Nation.AustriaHungary => "Unit Cavalry Austro-Hungaria",
                        Nation.Belgium => "Unit Cavalry Belgia",
                        Nation.Bulgaria => "Unit Cavalry Bulgaria",
                        Nation.China => "Unit Cavalry China",
                        Nation.France => "Unit Cavalry France",
                        Nation.Germany => "Unit Cavalry Germany",
                        Nation.GreatBritain => "Unit Cavalry UK",
                        Nation.Greece => "Unit Cavalry Greece",
                        Nation.Italy => "Unit Cavalry Italy",
                        Nation.Japan => "Unit Cavalry Japan",
                        Nation.Korea => "Unit Cavalry Korea",
                        Nation.Mexico => "Unit Cavalry Mexico",
                        Nation.Mongolia => "Unit Cavalry Mongolia",
                        Nation.Montenegro => "Unit Cavalry Montenegro",
                        Nation.Romania => "Unit Cavalry Romania",
                        Nation.Russia => "Unit Cavalry Russia",
                        Nation.Serbia => "Unit Cavalry Serbia",
                        Nation.Turkey => "Unit Cavalry Turkey",
                        Nation.USA => "Unit Cavalry US",
                        Nation.USNorth => "Unit Cavalry US North",
                        Nation.USSouth => "Unit Cavalry US South",
                        _ => throw new NotImplementedException(),
                    };
                }
				case UnitType.MachineGunnersCart: return "Unit Machine gunners cart";
				case UnitType.MachineGuns: return "Unit Machine guns";
				case UnitType.Tank: return "Unit Tank";
				case UnitType.Destroyer: return "Unit Destroyer";
				case UnitType.Cruser: return "Unit Cruser";
				case UnitType.Battleship: return "Unit Battleship";
				case UnitType.Carrier: return "Unit Carrier";
				default: throw new NotImplementedException();
			};
		}

		public static string GetBannerSprite(UnitModel unitInfo) {
            return unitInfo.Nation switch {
                Nation.AustriaHungary => "Banner Austro-Hungaria",
                Nation.Belgium => "Banner Belgium",
                Nation.Bulgaria => "Banner Bulgaria",
                Nation.China => "Banner China",
                Nation.France => "Banner France",
                Nation.Germany => "Banner Germany",
                Nation.GreatBritain => "Banner UK",
                Nation.Greece => "Banner Greece",
                Nation.Italy => "Banner Italy",
                Nation.Japan => "Banner Japan",
                Nation.Korea => "Banner Korea",
                Nation.Mexico => "Banner Mexico",
                Nation.Mongolia => "Banner Mongolia",
                Nation.Montenegro => "Banner Montenegro",
                Nation.Romania => "Banner Romania",
                Nation.Russia => "Banner Russia",
                Nation.Serbia => "Banner Serbia",
                Nation.Turkey => "Banner Turkey",
                Nation.USA => "Banner US",
                Nation.USNorth => "Banner US North",
                Nation.USSouth => "Banner US South",
                _ => throw new NotImplementedException(),
            };
        }

		public static string GetHealthSprite(UnitModel unitInfo) {
			var relativeHealth = unitInfo.Health / unitInfo.MaxHealth;

			if (relativeHealth <= 0.05f)
				return "Unit Health 5";
			if (relativeHealth <= 0.1f)
				return "Unit Health 10";
			if (relativeHealth <= 0.15f)
				return "Unit Health 15";
			if (relativeHealth <= 0.2f)
				return "Unit Health 20";
			if (relativeHealth <= 0.25f)
				return "Unit Health 25";
			if (relativeHealth <= 0.3f)
				return "Unit Health 30";
			if (relativeHealth <= 0.35f)
				return "Unit Health 35";
			if (relativeHealth <= 0.4f)
				return "Unit Health 40";
			if (relativeHealth <= 0.45f)
				return "Unit Health 45";
			if (relativeHealth <= 0.5f)
				return "Unit Health 50";
			if (relativeHealth <= 0.55f)
				return "Unit Health 55";
			if (relativeHealth <= 0.6f)
				return "Unit Health 60";
			if (relativeHealth <= 0.65f)
				return "Unit Health 65";
			if (relativeHealth <= 0.7f)
				return "Unit Health 70";
			if (relativeHealth <= 0.75f)
				return "Unit Health 75";
			if (relativeHealth <= 0.8f)
				return "Unit Health 80";
			if (relativeHealth <= 0.85f)
				return "Unit Health 85";
			if (relativeHealth <= 0.9f)
				return "Unit Health 90";
			if (relativeHealth <= 0.95f)
				return "Unit Health 95";
			return "Unit Health 100";
		}

		public static string GetQuantitySprite(ArmyModel army) {
            return army.Quantity switch {
                1 => "Unit Quantity 1",
                2 => "Unit Quantity 2",
                3 => "Unit Quantity 3",
                4 => "Unit Quantity 4",
                _ => throw new NotImplementedException(),
            };
		}

		public static string GetRankSprite(UnitModel unitInfo) {
			if (unitInfo.IsLand) {
                return unitInfo.ExperienceRank switch {
                    UnitExperienceRank.Rookies => null,
                    UnitExperienceRank.Fighters => "Unit Land Rank 2",
                    UnitExperienceRank.Proficients => "Unit Land Rank 3",
                    UnitExperienceRank.Veterans => "Unit Land Rank 4",
                    UnitExperienceRank.Elite => "Unit Land Rank 5",
                    _ => throw new NotImplementedException(),
                };
            } else {
                return unitInfo.ExperienceRank switch {
                    UnitExperienceRank.Rookies => null,
                    UnitExperienceRank.Fighters => "Unit Sea Rank 2",
                    UnitExperienceRank.Proficients => "Unit Sea Rank 3",
                    UnitExperienceRank.Veterans => "Unit Sea Rank 4",
                    UnitExperienceRank.Elite => "Unit Sea Rank 5",
                    _ => throw new NotImplementedException(),
                };
            }
		}

		public static string GetBoost1Sprite(UnitModel unitInfo) {
            if (unitInfo.IsLand) {
                return unitInfo.Boost1 switch {
                    UnitBoost.Attack => "Unit Land Boost 1 Attack",
                    UnitBoost.Commander => "Unit Land Boost 1 Commander",
                    UnitBoost.Defence => "Unit Land Boost 1 Defence",
                    UnitBoost.Transport => "Unit Land Boost 1 Transport",
                    UnitBoost.None => null,
                    _ => throw new NotImplementedException(),
                };
            } else {
                return unitInfo.Boost1 switch {
                    UnitBoost.Attack => "Unit Sea Boost 1 Attack",
                    UnitBoost.Commander => "Unit Sea Boost 1 Commander",
                    UnitBoost.Defence => "Unit Sea Boost 1 Defence",
                    UnitBoost.Transport => "Unit Sea Boost 1 Transport",
                    UnitBoost.None => null,
                    _ => throw new NotImplementedException(),
                };
            }
        }

		public static string GetBoost2Sprite(UnitModel unitInfo) {
            if (unitInfo.IsLand) {
                return unitInfo.Boost2 switch {
                    UnitBoost.Attack => "Unit Land Boost 2 Attack",
                    UnitBoost.Commander => "Unit Land Boost 2 Commander",
                    UnitBoost.Defence => "Unit Land Boost 2 Defence",
                    UnitBoost.Transport => "Unit Land Boost 2 Transport",
                    UnitBoost.None => null,
                    _ => throw new NotImplementedException(),
                };
            } else {
                return unitInfo.Boost2 switch {
                    UnitBoost.Attack => "Unit Sea Boost 2 Attack",
                    UnitBoost.Commander => "Unit Sea Boost 2 Commander",
                    UnitBoost.Defence => "Unit Sea Boost 2 Defence",
                    UnitBoost.Transport => "Unit Sea Boost 2 Transport",
                    UnitBoost.None => null,
                    _ => throw new NotImplementedException(),
                };
            }
        }

		public static string GetBoost3Sprite(UnitModel unitInfo) {
            if (unitInfo.IsLand) {
                return unitInfo.Boost3 switch {
                    UnitBoost.Attack => "Unit Land Boost 3 Attack",
                    UnitBoost.Commander => "Unit Land Boost 3 Commander",
                    UnitBoost.Defence => "Unit Land Boost 3 Defence",
                    UnitBoost.Transport => "Unit Land Boost 3 Transport",
                    UnitBoost.None => null,
                    _ => throw new NotImplementedException(),
                };
            } else {
                return unitInfo.Boost3 switch {
                    UnitBoost.Attack => "Unit Sea Boost 3 Attack",
                    UnitBoost.Commander => "Unit Sea Boost 3 Commander",
                    UnitBoost.Defence => "Unit Sea Boost 3 Defence",
                    UnitBoost.Transport => "Unit Sea Boost 3 Transport",
                    UnitBoost.None => null,
                    _ => throw new NotImplementedException(),
                };
            }
        }
	}
}
