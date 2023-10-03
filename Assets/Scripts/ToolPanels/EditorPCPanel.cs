using System.Linq;
using TrenchWarfare.Domain.Enums;
using TrenchWarfare.ToolPanels.State;
using TrenchWarfare.Utility;
using UnityEngine.UI;

namespace TrenchWarfare.ToolPanels {
    public class EditorPCPanel : EditorToolPanelBase {
        public EditorState state;

        void Start() {
            InitDropdown("Type", (int)state.ProductionCenter.Type);
            InitNationDropdown();
            InitLevelDropdown();

            InitInput("Name Input", state.ProductionCenter.Name);
        }

        public void SetType(int value) {
            state.ProductionCenter = state.ProductionCenter.Copy(i => {
                    i.Type = (ProductionCenterType)value;
                    return i;
                }
            );

            InitLevelDropdown();

            SetLevel(0);

        }

        public void SetNation(int value) {
            state.ProductionCenter = state.ProductionCenter.Copy(i => {
                    i.Nation = state.Nations.GetByIndex<Nation>(value);
                    return i;
                }
            );
        }

        public void SetLevel(int value) {
            var allLevels = state.ProductionCenter.GetLevelsForType(state.ProductionCenter.Type);

            state.ProductionCenter = state.ProductionCenter.Copy(i => {
                    i.Level = allLevels[value];
                    return i;
                }
            );
        }

        public void OnNameUpdated(string value) {
            state.ProductionCenter = state.ProductionCenter.Copy(i => {
                    i.Name = value;
                    return i;
                }
            );
        }

        protected void InitNationDropdown() {
            var dropdown = GetComponent<Dropdown>("Nation");

            dropdown.ClearOptions();
            dropdown.AddOptions(state.Nations.Select(i => i.ToString()).ToList());
            
            dropdown.value = state.Nations.IndexOf(i => i == state.UnitInfo.Nation);
        }

       protected void InitLevelDropdown() {
            var dropdown = GetComponent<Dropdown>("Level");
            var allLevels = state.ProductionCenter.GetLevelsForType(state.ProductionCenter.Type);

            dropdown.ClearOptions();
            dropdown.AddOptions(allLevels.Select(i => i.ToString()).ToList());
            
            dropdown.value = 0;
        }
    }
}