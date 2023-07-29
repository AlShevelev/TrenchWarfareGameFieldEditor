using TrenchWarfare.ToolPanels.State;
using UnityEngine;
using UnityEngine.UI;

namespace TrenchWarfare.ToolPanels {
    public class EditorTerrainPanel : EditorToolPanelBase {
        public EditorState state;

        void Start() {
            InitToggles();
            InitSliders();
        }

        public void SetTerrain(int terraint) {
            state.terrainSelected = (Terrain)terraint;
        }

        public void SetElevation(float elevation) {
            state.terrainElevation = (int)elevation;
        }

        public void SetBrushSize(float brushSize) {
            state.brushSize = (int)brushSize;
        }

        private void InitToggles() {
            var gameObjectName = "";
            switch(state.terrainSelected) {
                case Terrain.Sand: {
                    gameObjectName = "Sand Toggle";
                    break;
                }
                case Terrain.Grass: {
                    gameObjectName = "Grass Toggle";
                    break;
                }
                case Terrain.Mud: {
                    gameObjectName = "Mud Toggle";
                    break;
                }
                case Terrain.Stone: {
                    gameObjectName = "Stone Toggle";
                    break;
                }
                case Terrain.Snow: {
                    gameObjectName = "Snow Toggle";
                    break;
                }
                case Terrain.Water: {
                    gameObjectName = "Water Toggle";
                    break;
                }

            }

            GetComponent<Toggle>(gameObjectName).isOn = true;
        }

        private void InitSliders() {
            InitSlider(
                "Elevation Slider", 
                state.terrainElevation, 
                state.terrainElevationMinMax.Start.Value, 
                state.terrainElevationMinMax.End.Value
            );

            InitSlider(
                "Brush Size Slider", 
                state.brushSize, 
                state.brushSizeMinMax.Start.Value, 
                state.brushSizeMinMax.End.Value
            );
        }
    }
}