using System;
using TrenchWarfare.Domain.Enums;

namespace TrenchWarfare.Domain.Map.Conditions.Dto {
[Serializable]
    public class DiplomacyRecordDto {
        public Nation firstNation; 
        public Nation secondNation;
        public Relationship relationship;
    }
}