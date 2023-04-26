
public interface EditorNationsPanelCallback {
    void OnDeleteNation(EditNationPanel nationPanel);

    void OnUpdateNation(EditNationPanel nationPanel);

    bool OnNationCodeSelected(EditNationPanel nationPanel, Nation? code);
}
