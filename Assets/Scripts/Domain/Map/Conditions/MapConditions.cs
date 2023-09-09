using System.Text;
using System.Collections.Generic;
using System.IO;
using TrenchWarfare.Domain.Map.Conditions.Dto;
using TrenchWarfare.Domain.Enums;
using UnityEngine;

namespace TrenchWarfare.Domain.Map.Conditions {
    public class MapConditions: MapConditionsExternal {
        MapConditionsDto conditions;
        public MapConditionsDto Conditions { get => conditions; }

        public void LoadFromBinary(BinaryReader reader) {
            conditions = new MapConditionsDto();

            var titleLenInBytes = reader.ReadInt32();
            var titleAsBytes = reader.ReadBytes(titleLenInBytes);
            conditions.title = Encoding.UTF8.GetString(titleAsBytes);

            var descriptionLenInBytes = reader.ReadInt32();
            var descriptionAsBytes = reader.ReadBytes(descriptionLenInBytes);
            conditions.description = Encoding.UTF8.GetString(descriptionAsBytes);

            var nationsCount = reader.ReadInt16();
            var nations = new List<NationRecordDto>(nationsCount);
            for (short i = 0; i < nationsCount; i++) {
                var nation = new NationRecordDto();
                nation.code = (Nation)reader.ReadByte();
                nation.aggressiveness = (Aggressiveness)reader.ReadByte();

                nations.Add(nation);
            }
            conditions.nations = nations;

            var diplomacyCount = reader.ReadInt16();
            var diplomacy = new List<DiplomacyRecordDto>(diplomacyCount);
            for (short i = 0; i < diplomacyCount; i++)
            {
                var diplomacyRecord = new DiplomacyRecordDto();
                diplomacyRecord.firstNation = (Nation)reader.ReadByte();
                diplomacyRecord.secondNation = (Nation)reader.ReadByte();
                diplomacyRecord.relationship = (Relationship)reader.ReadByte();

                diplomacy.Add(diplomacyRecord);
            }
            conditions.diplomacy = diplomacy;
        }

        public void SaveToBinary(BinaryWriter writer) {
            var titleAsBytes = Encoding.UTF8.GetBytes(conditions.title);
            writer.Write(titleAsBytes.Length);
            writer.Write(titleAsBytes);

            var descriptionAsBytes = Encoding.UTF8.GetBytes(conditions.description); 
            writer.Write(descriptionAsBytes.Length);
            writer.Write(descriptionAsBytes);

            writer.Write((short)conditions.nations.Count);
            foreach(var nation in conditions.nations) {
                writer.Write((byte)nation.code);
                writer.Write((byte)nation.aggressiveness);
            }

            writer.Write((short)conditions.diplomacy.Count);
            foreach(var diplomacy in conditions.diplomacy) {
                writer.Write((byte)diplomacy.firstNation);
                writer.Write((byte)diplomacy.secondNation);
                writer.Write((byte)diplomacy.relationship);
            }
        }

        public void ImportFromJson(string rawData) {
            conditions = JsonUtility.FromJson<MapConditionsDto>(rawData);
        }

        public string ExportToJson() {
            return JsonUtility.ToJson(conditions);
        }
    }
}