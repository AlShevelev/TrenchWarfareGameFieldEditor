using System.Collections;
using System.Collections.Generic;
using TrenchWarfare.Conditions.Dto;
using UnityEngine;

namespace TrenchWarfare.Conditions {
    public class MapConditions {
        public MapConditionsDto conditions;

        public void LoadFromBinary() {
            
        }

        public void SaveToBinary() {
        }

        public void ImportFromJson(string rawData) {
            conditions = JsonUtility.FromJson<MapConditionsDto>(rawData);
        }

        public string ExportToJson() {
            return JsonUtility.ToJson(conditions);
        }
    }
}