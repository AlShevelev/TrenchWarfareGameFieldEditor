using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditNationPanel : EditorToolPanelBase {
    private EditorNationsPanelCallback callback;

    private int lastIndex = 0;      // None

    private Dropdown nationDropdown {
        get { return GetComponent<Dropdown>("Nation Dropdown"); }
    }

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
        Nation? code = null;

        if(index > 0) {
            code = (Nation)index-1;
        }

        var isSuccess = callback.OnNationCodeSelected(this, code);

        if(!isSuccess) {
            nationDropdown.SetValueWithoutNotify(lastIndex);
        } else {
            lastIndex = index;
        }
    }
}
