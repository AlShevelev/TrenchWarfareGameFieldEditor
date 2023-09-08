using System;
using System.Collections.Generic;
using TrenchWarfare.Domain;
using UnityEngine;

namespace TrenchWarfare.Utility {
    public class ModelRegistry : MonoBehaviour {
        Dictionary<Guid, MonoBehaviour> registry;

        private void Awake() {
            registry = new Dictionary<Guid, MonoBehaviour>();
        }

        public void Register(IModel model, MonoBehaviour gameObject) {
            registry[model.Id] = gameObject;
        }

        public void Unregister(IModel model) {
            registry.Remove(model.Id);
        }

        public T Get<T>(IModel model) where T: MonoBehaviour {
            if (model == null) {
                return null;
            }

            var value = registry[model.Id];

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