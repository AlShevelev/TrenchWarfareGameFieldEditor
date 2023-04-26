using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class EditorNationsPanel : EditorToolPanelBase, EditorNationsPanelCallback {
    private readonly Dictionary<EditNationPanel, int> nations = new Dictionary<EditNationPanel, int>();

    public NationsState state;

    public EditNationPanel panelPrefab;

    public NationDetailsPanel nationDetailsPanel;

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
        if(!state.CanEdit) {
            return;
        }

        var nationStateId = nations[nationPanel];

        var nation = state.GetNation(nationStateId);
        var aggressiveness = state.GetNationAggresiveness(nationStateId);
        var diplomacy = state.GetDiplomacy(nationStateId);

        nationDetailsPanel.Open(nation, aggressiveness, diplomacy);
    }

    public bool OnNationCodeSelected(EditNationPanel nationPanel, Nation? code) {
        return state.UpdateCode(nations[nationPanel], code);
    }
}