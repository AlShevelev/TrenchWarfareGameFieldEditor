using TrenchWarfare.ToolPanels.State;
using UnityEngine.UI;

namespace TrenchWarfare.ToolPanels {
    public class EditorSystemPanel : EditorToolPanelBase {
        public EditorState state;

        public HexMapEditor editor;

        void Start() {
            GetComponent<Toggle>("Labels Toggle").isOn = state.LabelsIsOn;
        }

        public void SetLabelsOn(bool isOn) {
            state.LabelsIsOn = isOn;
            editor.UpdateLevelsVisibility();
        }

        public void SetGridOn(bool isOn) {
            editor.ShowGrid(isOn);
        }
    }
}