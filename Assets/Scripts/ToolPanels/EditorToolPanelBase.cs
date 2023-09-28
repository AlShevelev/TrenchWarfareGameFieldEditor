using UnityEngine;
using UnityEngine.UI;

namespace TrenchWarfare.ToolPanels {
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

        protected void InitSlider(string gameObjectName, float value, float min, float max) {
            var slider = GetComponent<Slider>(gameObjectName);
            slider.minValue = min;
            slider.maxValue = max;
            slider.value = value;
        }

        protected void InitLabel(string gameObjectName, string value) {
            var label = GetComponent<Text>(gameObjectName);
            label.text = value;
        }

       protected void InitDropdown(string gameObjectName, int value) {
            var dropdown = GetComponent<Dropdown>(gameObjectName);
            dropdown.value = value;
        }

        protected void InitInput(string gameObjectName, string value) {
            var field = GetComponent<InputField>(gameObjectName);
            field.text = value;
        }
    }
}