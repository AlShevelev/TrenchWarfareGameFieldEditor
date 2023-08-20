using System;
using UnityEngine;
using TrenchWarfare.Domain.Enums;
using TrenchWarfare.Domain.Units;

namespace TrenchWarfare.ToolPanels.State {
    public class EditorState : MonoBehaviour, EditorStateRead {
        private Tool _activeTool = Tool.Terrain;
        public Tool ActiveTool {
            get => _activeTool;
            set => _activeTool = value;
        }

        private Terrain _terrainSelected = Terrain.Plain;
        public Terrain TerrainSelected {
            get => _terrainSelected;
            set => _terrainSelected = value;
        }

        private int _terrainElevation = 0;
        public int TerrainElevation {
            get => _terrainElevation;
            set => _terrainElevation = value;
        }

        public readonly Range terrainElevationMinMax = 0..3;

        private int _brushSize = 0;
        public int BrushSize {
            get => _brushSize;
            set => _brushSize = value;
        }

        public readonly Range brushSizeMinMax = 0..4;
        
        private int _waterLevel = 1;
        public int WaterLevel {
            get => _waterLevel;
        }

        private bool _riversIsOn = false;
        public bool RiversIsOn {
            get => _riversIsOn;
            set => _riversIsOn = value;
        }

        private bool _roadsIsOn = false;
        public bool RoadsIsOn {
            get => _roadsIsOn;
            set => _roadsIsOn = value;
        }

        private bool _wallsIsOn = false;
        public bool WallsIsOn {
            get => _wallsIsOn;
            set => _wallsIsOn = value;
        }

        private int _urbanLevel = 0;
        public int UrbanLevel {
            get => _urbanLevel;
            set => _urbanLevel = value;
        }

        public readonly Range urbanLevelMinMax = 0..3;

        private int _farmLevel = 0;
        public int FarmLevel {
            get => _farmLevel;
            set => _farmLevel = value;
        }

        public readonly Range farmLevelMinMax = 0..3;

        private bool _labelsIsOn = false;
        public bool LabelsIsOn {
            get => _labelsIsOn;
            set => _labelsIsOn = value;
        }

        private UnitInfo _unitInfo = new UnitInfo(
            UnitType.ArmoredCar,
            Nation.AustriaHungary
        );
        public UnitInfo UnitInfo {
            get => _unitInfo;
            set => _unitInfo = value;
        }
    }
}