using System;
using TrenchWarfare.Domain.Enums;
using TrenchWarfare.Domain.Map;

namespace TrenchWarfare.Domain.Units {
    public interface UnitModelExternal: IModel {
        CellModelExternal Cell { get; }

        UnitType Type { get; }

        Nation Nation { get; }

        /// <summary>
        /// [0, 1]
        /// </summary>
        float Fatigue { get; }

        float Health { get; }

        float MovementPoints { get; }

        UnitExperienceRank ExperienceRank { get; }

        int TookPartInBattles { get; }

        UnitBoost Boost1 { get; }

        UnitBoost Boost2 { get; }

        UnitBoost Boost3 { get; }

        float MaxHealth { get; }

        float MaxMovementPoints { get; }

        float Attack { get; }

        float Defence { get; }

        float DamageMin { get; }

        float DamageMax { get; }

        float CostInMoney { get; }

        float CostInIndustryPoints { get; }

        bool IsLand { get; }

        int NeedCityLevelToBuild { get; }

        int NeedFactoryLevelToBuild { get; }

        int NeedNavalBaseLevelToBuild { get; }

        int GetBattlesForExperienceRank(UnitExperienceRank unitExperienceRank);
    }
}

