using UnityEngine;
using System.Collections.Generic;
using TrenchWarfare.Domain;

namespace TrenchWarfare.Utility {
    public class ModelRegistry : MonoBehaviour {
        Dictionary<Model, MonoBehaviour> registry;

        private void Awake() {
            registry = new Dictionary<Model, MonoBehaviour>();
        }

        public void Register(Model model, MonoBehaviour gameObject) {
            registry[model] = gameObject;
        }

        public void Unregister(Model model) {
            registry.Remove(model);
        }

        public T Get<T>(Model model) where T: MonoBehaviour {
            if (model == null) {
                return null;
            }

            var value = registry[model];

            if (value != null) {
                return (T)value;
            } else {
                return null;
            }
        }

        public void Clear() {
            registry.Clear();
        }
    }
}