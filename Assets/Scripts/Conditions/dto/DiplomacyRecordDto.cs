using System;

namespace TrenchWarfare.Conditions.Dto {
[Serializable]
    public class DiplomacyRecordDto {
        public Nation firstNation; 
        public Nation secondNation;
        public Relationship relationship;
    }
}