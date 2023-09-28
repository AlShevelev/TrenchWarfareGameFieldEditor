using System;
using System.Collections.Generic;
using UnityEngine;
using TrenchWarfare.Domain.Enums;
using TrenchWarfare.Domain.Units;
using TrenchWarfare.Domain.MapObjects;

namespace TrenchWarfare.ToolPanels.State {
    public class EditorState : MonoBehaviour, EditorStateRead {
        List<Nation> nations = new List<Nation>();
        public IEnumerable<Nation> Nations {
            get => nations;
            set {
                nations = new List<Nation>(value);

                if (nations.Count != 0) {
                    unitInfo = new UnitModel(
                        UnitType.ArmoredCar,
                        nations[0]
                    );

                    productionCenter = new ProductionCenterModel(
                        ProductionCenterType.City,
                        ProductionCenterLevel.Level1,
                        nations[0]
                    );
                }
            }
        }

        Tool activeTool = Tool.Terrain;
        public Tool ActiveTool { get => activeTool; set => activeTool = value; }

        CellTerrain terrainSelected = CellTerrain.Plain;
        public CellTerrain TerrainSelected {
            get => terrainSelected;
            set => terrainSelected = value;
        }

        int terrainElevation = 0;
        public int TerrainElevation { get => terrainElevation; set => terrainElevation = value; }

        public readonly Range terrainElevationMinMax = 0..3;

        int brushSize = 0;
        public int BrushSize { get => brushSize; set => brushSize = value; }

        public readonly Range brushSizeMinMax = 0..4;
        
        int waterLevel = 1;
        public int WaterLevel { get => waterLevel; }

        bool riversIsOn = false;
        public bool RiversIsOn { get => riversIsOn; set => riversIsOn = value; }

        bool roadsIsOn = false;
        public bool RoadsIsOn { get => roadsIsOn; set => roadsIsOn = value; }

        bool wallsIsOn = false;
        public bool WallsIsOn { get => wallsIsOn; set => wallsIsOn = value; }

        int urbanLevel = 0;
        public int UrbanLevel { get => urbanLevel; set => urbanLevel = value; }

        public readonly Range urbanLevelMinMax = 0..3;

        bool labelsIsOn = false;
        public bool LabelsIsOn { get => labelsIsOn; set => labelsIsOn = value; }

        UnitModel unitInfo = new UnitModel(
            UnitType.ArmoredCar,
            Nation.AustriaHungary
        );
        public UnitModel UnitInfo { get => unitInfo; set => unitInfo = value; }

        ProductionCenterModel productionCenter = new ProductionCenterModel(
            ProductionCenterType.City,
            ProductionCenterLevel.Level1,
            Nation.AustriaHungary
        );
        public ProductionCenterModel ProductionCenter {
            get => productionCenter;
            set => productionCenter = value;
        }
    }
}