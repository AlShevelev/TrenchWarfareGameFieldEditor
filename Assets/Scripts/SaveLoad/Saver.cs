using System.Linq;
using System.IO;
using TrenchWarfare.Domain.Enums;
using TrenchWarfare.Domain.Map;
using TrenchWarfare.Domain.MapObjects;
using TrenchWarfare.Domain.Units;
using System.Text;
using TrenchWarfare.Domain.Map.Conditions.Dto;
using TrenchWarfare.Domain.Game;

namespace TrenchWarfare.SaveLoad {
    public static class Saver {
        public static void Save(
            BinaryWriter writer,
            GridModelExternal grid,
            GameStateRead gameState
        ) {
            SaveGrid(writer, grid);
            SaveGameState(writer, gameState);
        }

        static void SaveGrid(BinaryWriter writer, GridModelExternal model) {
            writer.Write(SaveLoadConstants.FORMAT_VERSION);     // byte

            writer.Write(model.CellCountX);     // int
            writer.Write(model.CellCountZ);     // int

            writer.Write(model.Cells.Count());  // int
            foreach(var cell in model.Cells) {
                SaveCell(writer, cell);
            }

            SaveConditions(writer, model.Conditions.Conditions);
        }

        static void SaveGameState(BinaryWriter writer, GameStateRead gameState) {
            writer.Write(gameState.TurnNumber);     // int

            writer.Write(gameState.Nations.Count());    // int
            foreach(var nation in gameState.Nations) {
                writer.Write((byte)nation.Nation);
                writer.Write(nation.Money);             // int
                writer.Write(nation.IndustryPoints);    // int
            }
        }

        static void SaveCell(BinaryWriter writer, CellModelExternal cell) {
            writer.Write((byte)cell.TerrainType);
			writer.Write((byte)(cell.Elevation + 127));
			writer.Write((byte)cell.WaterLevel);

			writer.Write(cell.HasIncomingRiver);        // bool
			writer.Write((byte)cell.IncomingRiver);

			writer.Write(cell.HasOutgoingRiver);        // bool
			writer.Write((byte)cell.OutgoingRiver);

            var directions = HexDirections.All;
			foreach(var direction in directions) {
				writer.Write(cell.HasRoadThroughEdge(direction));   // bool (6 values)
			}

            byte owner = SaveLoadConstants.BYTE_NULL;
            if (cell.Owner != null) {
                owner = (byte)cell.Owner;
            }
            writer.Write(owner);

            writer.Write(cell.Army != null);            // bool
            writer.Write(cell.ProductionCenter != null);
            writer.Write(cell.TerrainModifier != null);

            if (cell.Army != null) {
                SaveArmy(writer, cell.Army);
            }

            if (cell.ProductionCenter != null) {
                SavePC(writer, cell.ProductionCenter);
            }

            if (cell.TerrainModifier != null) {
                SaveTerrainModifier(writer, cell.TerrainModifier);
            }
        }

        static void SaveConditions(BinaryWriter writer, MapConditionsDto conditions) {
            SaveString(writer, conditions.title);
            SaveString(writer, conditions.description);

            writer.Write(conditions.nations.Count); // int
            foreach(var nation in conditions.nations) {
                writer.Write((byte)nation.code);
                writer.Write((byte)nation.aggressiveness);
                writer.Write(nation.startMoney);            // int
                writer.Write(nation.startIndustryPoints);   // int
            }

            writer.Write(conditions.diplomacy.Count); // int
            foreach(var diplomacy in conditions.diplomacy) {
                writer.Write((byte)diplomacy.firstNation);
                writer.Write((byte)diplomacy.secondNation);
                writer.Write((byte)diplomacy.relationship);
            }
        }

        static void SaveArmy(BinaryWriter writer, ArmyModelExternal army) {
            writer.Write(army.Units.Count());       // int

            foreach (var unit in army.Units) {
                SaveUnit(writer, unit);
            }
        }

        static void SavePC(BinaryWriter writer, ProductionCenterModelExternal pc) {
            writer.Write((byte)pc.Type);
            writer.Write((byte)pc.Level);
            writer.Write((byte)pc.Nation);
            SaveString(writer, pc.Name);
            // see how to save a name in the conditions; add a method for saving a string to this class
        }

        static void SaveTerrainModifier(BinaryWriter writer, TerrainModifierModelExternal tm) {
            writer.Write((byte)tm.Modifier);
        }

        static void SaveUnit(BinaryWriter writer, UnitModelExternal unit) {
            writer.Write((byte)unit.ExperienceRank);
            writer.Write(unit.TookPartInBattles);       // int
            writer.Write((byte)unit.Type);
            writer.Write((byte)unit.Nation);
            writer.Write(unit.Fatigue);         // float
            writer.Write(unit.Health);         // float
            writer.Write(unit.MovementPoints);         // float
            writer.Write((byte)unit.Boost1);
            writer.Write((byte)unit.Boost2);
            writer.Write((byte)unit.Boost3);
        }

        static void SaveString(BinaryWriter writer, string value) {
            if (value == null) {
                writer.Write(0);        // int
            } else {
                var valueAsBytes = Encoding.UTF8.GetBytes(value);
                writer.Write(valueAsBytes.Length); // int
                writer.Write(valueAsBytes);
            }
        }
    }
}
