using UnityEngine.UI;
using System.Collections.Generic;

public class EditorNationsPanel : EditorToolPanelBase, EditorNationsPanelCallback {
    private readonly Dictionary<EditNationPanel, int> nations = new Dictionary<EditNationPanel, int>();

    public NationsState state;

    public EditNationPanel panelPrefab;

    void Start() {
//        GetComponent<Toggle>("Labels Toggle").isOn = state.labelsIsOn;
    }

    public void OnAddNationClick() {
        if(!state.CanAdd) {
            return;
        }

        var nationPanel = Instantiate<EditNationPanel>(panelPrefab);

        var nationStateId = state.AddNation();
        nations.Add(nationPanel, nationStateId);

        nationPanel.RegisterCallback(this);

        nationPanel.transform.SetParent(this.transform);
        nationPanel.transform.SetSiblingIndex(2);
    }

    public void OnDeleteNation(EditNationPanel nationPanel) {
        var nationStateId = nations[nationPanel];
        state.DeleteNation(nationStateId);

        nations.Remove(nationPanel);
        Destroy(nationPanel.gameObject);
    }

    public void OnUpdateNation(EditNationPanel nationPanel) {
        // throw new System.NotImplementedException();
    }

    public void OnNationCodeSelected(EditNationPanel nationPanel, int index) {
        state.UpdateCode(nations[nationPanel], index);
    }
}

// Update code
// Pass Edit button click
// Remove Diplomacy button