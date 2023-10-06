using System;
using TrenchWarfare.Domain.Enums;

namespace TrenchWarfare.Domain.Map.Conditions.Dto {
    [Serializable]
    public class NationRecordDto {
        public Nation code;
        public Aggressiveness aggressiveness;
        public int startMoney;
        public int startIndustryPoints;
    }
}