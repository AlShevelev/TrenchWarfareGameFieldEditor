using TrenchWarfare.Domain.Map.Conditions.Dto;
using UnityEngine;

namespace TrenchWarfare.Domain.Map.Conditions {
    public class MapConditions: MapConditionsExternal {
        MapConditionsDto conditions;
        public MapConditionsDto Conditions { get => conditions; set => conditions = value; }

        public MapConditions() { }

        public MapConditions(MapConditionsDto conditions): this() {
            this.conditions = conditions;
        }

        public void ImportFromJson(string rawData) {
            conditions = JsonUtility.FromJson<MapConditionsDto>(rawData);
        }

        public string ExportToJson() {
            return JsonUtility.ToJson(conditions);
        }
    }
}