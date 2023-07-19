using TrenchWarfare.ToolPanels.State;
using UnityEngine.UI;

namespace TrenchWarfare.ToolPanels {
    public class EditorRoadsPanel : EditorToolPanelBase {
        public EditorState state;

        void Start() {
            GetComponent<Toggle>("On Toggle").isOn = state.roadsIsOn;
        }

        public void SetOn(bool isOn) {
            state.roadsIsOn = isOn;

        }
    }
}