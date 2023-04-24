using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditNationPanel : MonoBehaviour {
    private EditorNationsPanelCallback callback;

    public void RegisterCallback(EditorNationsPanelCallback callback) {
        this.callback = callback;
    }

    public void OnDeleteClick() {
        callback.OnDeleteNation(this);
    }

    public void OnEditClick() {
        callback.OnUpdateNation(this);
    }

    public void OnNationSelected(int index) {
        callback.OnNationCodeSelected(this, index);
    }
}
