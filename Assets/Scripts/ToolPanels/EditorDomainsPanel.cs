using System.Collections.Generic;
using System.Linq;
using TrenchWarfare.Domain.Enums;
using TrenchWarfare.ToolPanels.State;
using TrenchWarfare.Utility;
using UnityEngine.UI;

namespace TrenchWarfare.ToolPanels {
    public class EditorDomainPanel : EditorToolPanelBase {
        public EditorState state;

        void Start() {
            InitNationDropdown();
        }

        public void SetNation(int value) {
            if (value == 0) {
                state.DomainNation = null;
            } else {
                state.DomainNation = state.Nations.GetByIndex<Nation>(value - 1);
            }
        }

        protected void InitNationDropdown() {
            var dropdown = GetComponent<Dropdown>("Nation");

            dropdown.ClearOptions();

            var options = new List<string>() { "None" };
            options.AddRange(state.Nations.Select(i => i.ToString()));

            dropdown.AddOptions(options);
            
            dropdown.value = 0;
        }
    }
}