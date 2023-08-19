using TrenchWarfare.ToolPanels.State;
using UnityEngine.UI;

namespace TrenchWarfare.ToolPanels {
    public class EditorFarmsPanel : EditorToolPanelBase {
        public EditorState state;

        void Start() {
            InitSliders();
        }

        public void SetLevel(float level) {
            state.FarmLevel = (int)level;
        }

        private void InitSliders() {
            InitSlider(
                "Slider", 
                state.FarmLevel, 
                state.farmLevelMinMax.Start.Value, 
                state.farmLevelMinMax.End.Value
            );
        }
    }
}