using UnityEngine.UI;

public class EditorUrbanPanel : EditorToolPanelBase {
    public EditorState state;

    void Start() {
        InitSliders();
    }

    public void SetLevel(float level) {
        state.urbanLevel = (int)level;
    }

    private void InitSliders() {
        InitSlider(
            "Slider", 
            state.urbanLevel, 
            state.urbanLevelMinMax.Start.Value, 
            state.urbanLevelMinMax.End.Value
        );
    }
}
