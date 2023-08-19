using System;
using TrenchWarfare.Domain.Enums;

namespace TrenchWarfare.Conditions.Dto {
    [Serializable]
    public class NationRecordDto {
        public Nation code;
        public Aggressiveness aggressiveness;
    }
}