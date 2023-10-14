using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TrenchWarfare.Domain.Enums;
using TrenchWarfare.Domain.Game;
using TrenchWarfare.Domain.Map;
using TrenchWarfare.Domain.Map.Conditions;
using TrenchWarfare.Domain.Map.Conditions.Dto;
using TrenchWarfare.Domain.MapObjects;
using TrenchWarfare.Domain.Units;
using UnityEngine;

namespace TrenchWarfare.SaveLoad {
    public static class Loader {
        public static (GridModel, GameState) Load(BinaryReader reader) {
            return (LoadGrid(reader), LoadState(reader));
        }

        private static GridModel LoadGrid(BinaryReader reader) {
            reader.ReadByte();          // Format version - unused so far

            var cellCountX = reader.ReadInt32();
            var cellCountZ = reader.ReadInt32();

            var gridModel = new GridModel(cellCountX, cellCountZ);

            var totalCells = reader.ReadInt32();
            gridModel.InitCells(totalCells);
            for(var i=0; i < totalCells; i++) {
                gridModel.SetCell(i, LoadCell(reader));
            }

            gridModel.Conditions = LoadConditions(reader);

            return gridModel;
        }

        private static CellModel LoadCell(BinaryReader reader) {
            var cellModel = new CellModel();

            cellModel.TerrainType = (CellTerrain)reader.ReadByte();
            cellModel.Elevation = reader.ReadByte() - 127;
            cellModel.WaterLevel = reader.ReadByte();

            cellModel.HasIncomingRiver = reader.ReadBoolean();
            cellModel.IncomingRiver = (HexDirection)reader.ReadByte();

            cellModel.HasOutgoingRiver = reader.ReadBoolean();
            cellModel.OutgoingRiver = (HexDirection)reader.ReadByte();

            var directions = HexDirections.All;
			foreach(var direction in directions) {
				cellModel.SetRoadThroughEdge(direction, reader.ReadBoolean());
			}

            var owner = reader.ReadByte();
            cellModel.Owner = owner == SaveLoadConstants.BYTE_NULL ? null : (Nation)owner;

            var hasArmy = reader.ReadBoolean();
            var hasProductionCenter = reader.ReadBoolean();
            var hasTerrainModifier = reader.ReadBoolean();

            if (hasArmy) {
                cellModel.Army = LoadArmy(reader, cellModel);
            }

            if (hasProductionCenter) {
                cellModel.ProductionCenter = LoadProductionCenter(reader, cellModel);
            }


            if (hasTerrainModifier) {
                cellModel.TerrainModifier = LoadTerrainModifier(reader, cellModel);
            }

            return cellModel;
        }

        private static ArmyModel LoadArmy(BinaryReader reader, CellModel parent) {
            var unitsTotal = reader.ReadInt32();

            var armyModel = new ArmyModel(LoadUnit(reader));

            for(var i = 1; i < unitsTotal; i++) {
                armyModel.AddUnit(LoadUnit(reader));
            }

            armyModel.Cell = parent;

            return armyModel;
        }

        private static UnitModel LoadUnit(BinaryReader reader) {
            var experienceRank = (UnitExperienceRank)reader.ReadByte();
            var tookPartInBattles = reader.ReadInt32();
            var type = (UnitType)reader.ReadByte();
            var nation = (Nation)reader.ReadByte();
            var fatigue = reader.ReadSingle();
            var health = reader.ReadSingle();
            var movementPoints = reader.ReadSingle();
            var boost1 = (UnitBoost)reader.ReadByte();
            var boost2 = (UnitBoost)reader.ReadByte();
            var boost3 = (UnitBoost)reader.ReadByte();

            return new UnitModel(
                type,
                nation,
                fatigue,
                health,
                movementPoints,
                experienceRank,
                tookPartInBattles,
                boost1,
                boost2,
                boost3
            );
        }

        private static ProductionCenterModel LoadProductionCenter(
            BinaryReader reader,
            CellModel parent
        ) {
            var model = new ProductionCenterModel(
                type: (ProductionCenterType)reader.ReadByte(),
                level: (ProductionCenterLevel)reader.ReadByte(),
                nation: (Nation)reader.ReadByte(),
                name: LoadString(reader)
            );

            model.Cell = parent;

            return model;
        }

        private static TerrainModifierModel LoadTerrainModifier(
            BinaryReader reader,
            CellModel parent
        ) {
            var model = new TerrainModifierModel(
                modifier: (TerrainModifierType)reader.ReadByte()
            );

            model.Cell = parent;

            return model;
        }

        private static MapConditions LoadConditions(BinaryReader reader) {
            var dto = new MapConditionsDto();

            dto.title = LoadString(reader);
            dto.description = LoadString(reader);

            var totalNations = reader.ReadInt32();
            dto.nations = new List<NationRecordDto>();
            for(var i = 0; i < totalNations; i++) {
                var nation = new NationRecordDto() {
                    code = (Nation)reader.ReadByte(),
                    aggressiveness = (Aggressiveness)reader.ReadByte(),
                    startMoney = reader.ReadInt32(),
                    startIndustryPoints  =reader.ReadInt32()
                };
                dto.nations.Add(nation);
            }

            var totalDiplomacy = reader.ReadInt32();
            dto.diplomacy = new List<DiplomacyRecordDto>();
            for(var i = 0; i < totalDiplomacy; i++) {
                var diplomacy = new DiplomacyRecordDto() {
                    firstNation = (Nation)reader.ReadByte(),
                    secondNation = (Nation)reader.ReadByte(),
                    relationship = (Relationship)reader.ReadByte()
                };
                dto.diplomacy.Add(diplomacy);
            }

            return new MapConditions(dto);
        }

        private static GameState LoadState(BinaryReader reader) {
            var turnNumber = reader.ReadInt32();

            var totalNations = reader.ReadInt32();
            var nations = new Dictionary<Nation, NationGameState>();
            for(var i = 0; i< totalNations; i++) {
                var nation = (Nation)reader.ReadByte();
                var nationState = new NationGameState(
                    nation: nation,
                    money: reader.ReadInt32(),
                    industryPoints: reader.ReadInt32()
                );
                nations.Add(nation, nationState);
            }

            return new GameState(turnNumber, nations);
        }

        private static string LoadString(BinaryReader reader) {
            var lenInBytes = reader.ReadInt32();
            var strAsBytes = reader.ReadBytes(lenInBytes);
            return Encoding.UTF8.GetString(strAsBytes);
        }
    }
}

