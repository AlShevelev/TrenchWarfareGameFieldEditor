using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrenchWarfare.ToolPanels.State {
    public class EditorState : MonoBehaviour, EditorStateRead {
        private Tool _activeTool = Tool.Terrain;
        public Tool activeTool {
            get { return _activeTool; }
            set { _activeTool = value; }
        }

        private Terrain _terrainSelected = Terrain.Sand;
        public Terrain terrainSelected {
            get { return _terrainSelected; }
            set { _terrainSelected = value; }
        }

        private int _terrainElevation = 0;
        public int terrainElevation {
            get { return _terrainElevation; }
            set { _terrainElevation = value; }
        }

        public readonly Range terrainElevationMinMax = 0..1;

        private int _brushSize = 0;
        public int brushSize {
            get { return _brushSize; }
            set { _brushSize = value; }
        }

        public readonly Range brushSizeMinMax = 0..4;
        
        private int _waterLevel = 1;
        public int waterLevel {
            get { return _waterLevel; }
        }

        private bool _riversIsOn = false;
        public bool riversIsOn {
            get { return _riversIsOn; }
            set { _riversIsOn = value; }
        }

        private bool _roadsIsOn = false;
        public bool roadsIsOn {
            get { return _roadsIsOn; }
            set { _roadsIsOn = value; }
        }

        private bool _wallsIsOn = false;
        public bool wallsIsOn {
            get { return _wallsIsOn; }
            set { _wallsIsOn = value; }
        }

        private int _urbanLevel = 0;
        public int urbanLevel {
            get { return _urbanLevel; }
            set { _urbanLevel = value; }
        }

        public readonly Range urbanLevelMinMax = 0..3;

        private int _farmLevel = 0;
        public int farmLevel {
            get { return _farmLevel; }
            set { _farmLevel = value; }
        }

        public readonly Range farmLevelMinMax = 0..3;

        private int _plantLevel = 0;
        public int plantLevel {
            get { return _plantLevel; }
            set { _plantLevel = value; }
        }

        public readonly Range plantLevelMinMax = 0..3;

        private bool _labelsIsOn = false;
        public bool labelsIsOn {
            get { return _labelsIsOn; }
            set { _labelsIsOn = value; }
        }
    }
}