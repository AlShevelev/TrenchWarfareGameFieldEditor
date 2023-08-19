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
            state.TerrainSelected = (Terrain)terraint;
        }

        public void SetElevation(float elevation) {
            state.TerrainElevation = (int)elevation;
        }

        public void SetBrushSize(float brushSize) {
            state.BrushSize = (int)brushSize;
        }

        private void InitToggles() {
            var gameObjectName = "";
            switch(state.TerrainSelected) {
                case Terrain.Plain: {
                    gameObjectName = "Plain Toggle";
                    break;
                }
                case Terrain.Wood: {
                    gameObjectName = "Wood Toggle";
                    break;
                }
                case Terrain.Marsh: {
                    gameObjectName = "Marsh Toggle";
                    break;
                }
                case Terrain.Sand: {
                    gameObjectName = "Sand Toggle";
                    break;
                }
                case Terrain.Hills: {
                    gameObjectName = "Hills Toggle";
                    break;
                }
                case Terrain.Mountains: {
                    gameObjectName = "Mountains Toggle";
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
                state.TerrainElevation, 
                state.terrainElevationMinMax.Start.Value, 
                state.terrainElevationMinMax.End.Value
            );

            InitSlider(
                "Brush Size Slider", 
                state.BrushSize, 
                state.brushSizeMinMax.Start.Value, 
                state.brushSizeMinMax.End.Value
            );
        }
    }
}