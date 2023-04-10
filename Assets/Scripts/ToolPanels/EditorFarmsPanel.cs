using UnityEngine.UI;

public class EditorFarmsPanel : EditorToolPanelBase {
    public EditorState state;

    void Start() {
        InitSliders();
    }

    public void SetLevel(float level) {
        state.farmLevel = (int)level;
    }

    private void InitSliders() {
        InitSlider(
            "Slider", 
            state.farmLevel, 
            state.farmLevelMinMax.Start.Value, 
            state.farmLevelMinMax.End.Value
        );
    }
}
