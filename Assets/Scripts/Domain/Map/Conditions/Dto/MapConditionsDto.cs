using System;
using System.Collections.Generic;

namespace TrenchWarfare.Domain.Map.Conditions.Dto {
    [Serializable]
    public class MapConditionsDto {
        public string title;
        public string description;

        public List<NationRecordDto> nations;
        public List<DiplomacyRecordDto> diplomacy;
    }
}