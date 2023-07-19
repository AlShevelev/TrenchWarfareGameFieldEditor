using TrenchWarfare.ToolPanels.State;
using UnityEngine.UI;

namespace TrenchWarfare.ToolPanels {
    public class EditorRiversPanel : EditorToolPanelBase {
        public EditorState state;

        void Start() {
            GetComponent<Toggle>("On Toggle").isOn = state.riversIsOn;
        }

        public void SetOn(bool isOn) {
            state.riversIsOn = isOn;
        }
    }
}