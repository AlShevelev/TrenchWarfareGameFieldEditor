using System;
using System.Collections;
using System.Collections.Generic;

namespace TrenchWarfare.Conditions.Dto {
    [Serializable]
    public class MapConditionsDto {
        public List<NationRecordDto> nations;
        public List<DiplomacyRecordDto> diplomacy;
    }
}