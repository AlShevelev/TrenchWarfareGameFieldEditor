using TrenchWarfare.Domain.Map.Conditions.Dto;

namespace TrenchWarfare.Domain.Map.Conditions {
    public interface MapConditionsExternal {
        MapConditionsDto Conditions { get; }

        void ImportFromJson(string rawData);

        string ExportToJson();
    }
}

