using TrenchWarfare.ToolPanels.State;
using TrenchWarfare.Domain.Enums;
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
            state.TerrainSelected = (CellTerrain)terraint;
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
                case CellTerrain.Plain: {
                    gameObjectName = "Plain Toggle";
                    break;
                }
                case CellTerrain.Wood: {
                    gameObjectName = "Wood Toggle";
                    break;
                }
                case CellTerrain.Marsh: {
                    gameObjectName = "Marsh Toggle";
                    break;
                }
                case CellTerrain.Sand: {
                    gameObjectName = "Sand Toggle";
                    break;
                }
                case CellTerrain.Hills: {
                    gameObjectName = "Hills Toggle";
                    break;
                }
                case CellTerrain.Mountains: {
                    gameObjectName = "Mountains Toggle";
                    break;
                }
                case CellTerrain.Snow: {
                    gameObjectName = "Snow Toggle";
                    break;
                }
                case CellTerrain.Water: {
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