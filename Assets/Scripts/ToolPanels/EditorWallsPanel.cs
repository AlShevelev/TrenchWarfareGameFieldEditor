using UnityEngine.UI;

public class EditorWallsPanel : EditorToolPanelBase {
    public EditorState state;

    void Start() {
        GetComponent<Toggle>("On Toggle").isOn = state.wallsIsOn;
    }

    public void SetOn(bool isOn) {
        state.wallsIsOn = isOn;
    }
}
