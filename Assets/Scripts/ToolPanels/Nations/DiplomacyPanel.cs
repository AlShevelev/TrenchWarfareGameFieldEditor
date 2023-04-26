using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiplomacyPanel : EditorToolPanelBase {
    private NationDetailsPanelCallback callback;

    public void Init(Nation nation, Diplomacy? diplomacy) {
        GetComponent<Text>("Nation Name").text = nation.ToString();

        var diplomacyDropdown = GetComponent<Dropdown>("Diplomacy Dropdown");
        if(diplomacy == null) {
            diplomacyDropdown.value = 0;
        } else {
            diplomacyDropdown.value = (int)diplomacy;
        }
    }

    public void RegisterCallback(NationDetailsPanelCallback callback) {
        this.callback = callback;
    }
}
