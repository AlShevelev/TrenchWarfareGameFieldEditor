using TrenchWarfare.ToolPanels.State;
using UnityEngine.UI;

namespace TrenchWarfare.ToolPanels {
    public class EditorSystemPanel : EditorToolPanelBase {
        public EditorState state;

        public HexMapEditor editor;

        void Start() {
            GetComponent<Toggle>("Labels Toggle").isOn = state.labelsIsOn;
        }

        public void SetOn(bool isOn) {
            state.labelsIsOn = isOn;
            editor.UpdateLevelsVisibility();
        }
    }
}