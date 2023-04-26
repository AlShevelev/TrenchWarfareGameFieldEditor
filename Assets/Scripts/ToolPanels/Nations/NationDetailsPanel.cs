using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NationDetailsPanel : EditorToolPanelBase, NationDetailsPanelCallback {
    public DiplomacyPanel prefabDiplomacy;

    public void Open (Nation nation, Aggressiveness? aggressiveness, List<Tuple<Nation, Diplomacy?>> diplomacy) {
        var pathToMenuPanel = "Cover Panel/Menu Panel";
        // Init name
        GetComponent<Text>(pathToMenuPanel + "/Nation Name").text = nation.ToString();

        // Init aggersiveness
        var aggressivenessDropDown = GetComponent<Dropdown>(pathToMenuPanel + "/Aggressiveness Dropdown");
        if(aggressiveness != null) {
            aggressivenessDropDown.value = (int)aggressiveness;
        } else {
            aggressivenessDropDown.value = 0;
        }

        // Init diplomacy
        var menuPanel = gameObject.transform.Find(pathToMenuPanel);
        
        var oldDiplomacyPanels = menuPanel.GetComponentsInChildren<DiplomacyPanel>();
        foreach(var panel in oldDiplomacyPanels) {
            Destroy(panel.gameObject);
        }

        foreach(var keyValue in diplomacy) {
            var diplomacyPanel = Instantiate<DiplomacyPanel>(prefabDiplomacy);

            diplomacyPanel.Init(keyValue.Item1, keyValue.Item2);
            diplomacyPanel.RegisterCallback(this);

            diplomacyPanel.transform.SetParent(menuPanel);
            diplomacyPanel.transform.SetSiblingIndex(4);
        }

		Show();
	}

    public void OnSaveAndCloseClick() {
        Hide();
    }

	private void Close () {
		gameObject.SetActive(false);
        // Use callback
	}
}
