using UnityEngine.UI;

public class EditorPlantsPanel : EditorToolPanelBase {
    public EditorState state;

    void Start() {
        InitSliders();
    }

    public void SetLevel(float level) {
        state.plantLevel = (int)level;
    }

    private void InitSliders() {
        InitSlider(
            "Slider", 
            state.plantLevel, 
            state.plantLevelMinMax.Start.Value, 
            state.plantLevelMinMax.End.Value
        );
    }
}
