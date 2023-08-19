using TrenchWarfare.ToolPanels.State;
using UnityEngine.UI;

namespace TrenchWarfare.ToolPanels {
    public class EditorWallsPanel : EditorToolPanelBase {
        public EditorState state;

        void Start() {
            GetComponent<Toggle>("On Toggle").isOn = state.WallsIsOn;
        }

        public void SetOn(bool isOn) {
            state.WallsIsOn = isOn;
        }
    }
}