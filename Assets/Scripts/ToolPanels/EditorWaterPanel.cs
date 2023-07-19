using TrenchWarfare.ToolPanels.State;

namespace TrenchWarfare.ToolPanels {
    public class EditorWaterPanel : EditorToolPanelBase {
        public EditorState state;

        void Start() {
            InitSliders();
        }

        public void SetLevel(float level) {
            state.waterLevel = (int)level;
        }

        public void SetBrushSize(float brushSize) {
            state.waterBrushSize = (int)brushSize;
        }

        private void InitSliders() {
            InitSlider(
                "Level Slider", 
                state.waterLevel, 
                state.waterLevelMinMax.Start.Value, 
                state.waterLevelMinMax.End.Value
            );

            InitSlider(
                "Brush Size Slider", 
                state.waterBrushSize, 
                state.waterBrushSizeMinMax.Start.Value, 
                state.waterBrushSizeMinMax.End.Value
            );
        }
    }
}