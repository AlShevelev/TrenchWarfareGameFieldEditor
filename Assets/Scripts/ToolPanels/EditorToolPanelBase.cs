using UnityEngine;
using UnityEngine.UI;

public abstract class EditorToolPanelBase : MonoBehaviour {
    public void Show() {
        gameObject.SetActive(true);
    }

    public void Hide() {
        gameObject.SetActive(false);
    }

    protected T GetComponent<T>(string gameObjectName) {
        return gameObject.transform.Find(gameObjectName).GetComponent<T>();
    }

    protected void InitSlider(string gameObjectName, int value, int min, int max) {
        var elevationSlider = GetComponent<Slider>(gameObjectName);
        elevationSlider.minValue = min;
        elevationSlider.maxValue = max;
        elevationSlider.value = value;
    }
}
