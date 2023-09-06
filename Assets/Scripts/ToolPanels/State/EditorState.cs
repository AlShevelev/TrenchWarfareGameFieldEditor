using System;
using UnityEngine;
using TrenchWarfare.Domain.Enums;
using TrenchWarfare.Domain.Units;

namespace TrenchWarfare.ToolPanels.State {
    public class EditorState : MonoBehaviour, EditorStateRead {
        private Tool activeTool = Tool.Terrain;
        public Tool ActiveTool {
            get => activeTool;
            set => activeTool = value;
        }

        private CellTerrain terrainSelected = CellTerrain.Plain;
        public CellTerrain TerrainSelected {
            get => terrainSelected;
            set => terrainSelected = value;
        }

        private int terrainElevation = 0;
        public int TerrainElevation {
            get => terrainElevation;
            set => terrainElevation = value;
        }

        public readonly Range terrainElevationMinMax = 0..3;

        private int brushSize = 0;
        public int BrushSize {
            get => brushSize;
            set => brushSize = value;
        }

        public readonly Range brushSizeMinMax = 0..4;
        
        private int waterLevel = 1;
        public int WaterLevel {
            get => waterLevel;
        }

        private bool riversIsOn = false;
        public bool RiversIsOn {
            get => riversIsOn;
            set => riversIsOn = value;
        }

        private bool roadsIsOn = false;
        public bool RoadsIsOn {
            get => roadsIsOn;
            set => roadsIsOn = value;
        }

        private bool wallsIsOn = false;
        public bool WallsIsOn {
            get => wallsIsOn;
            set => wallsIsOn = value;
        }

        private int urbanLevel = 0;
        public int UrbanLevel {
            get => urbanLevel;
            set => urbanLevel = value;
        }

        public readonly Range urbanLevelMinMax = 0..3;

        private bool labelsIsOn = false;
        public bool LabelsIsOn {
            get => labelsIsOn;
            set => labelsIsOn = value;
        }

        private UnitModel unitInfo = new UnitModel(
            UnitType.ArmoredCar,
            Nation.AustriaHungary
        );
        public UnitModel UnitInfo {
            get => unitInfo;
            set => unitInfo = value;
        }
    }
}