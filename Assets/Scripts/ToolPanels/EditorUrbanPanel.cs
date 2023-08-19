using TrenchWarfare.ToolPanels.State;
using UnityEngine.UI;

namespace TrenchWarfare.ToolPanels {
    public class EditorUrbanPanel : EditorToolPanelBase {
        public EditorState state;

        void Start() {
            InitSliders();
        }

        public void SetLevel(float level) {
            state.UrbanLevel = (int)level;
        }

        private void InitSliders() {
            InitSlider(
                "Slider", 
                state.UrbanLevel, 
                state.urbanLevelMinMax.Start.Value, 
                state.urbanLevelMinMax.End.Value
            );
        }
    }
}