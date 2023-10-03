using System;
using System.Collections.Generic;
using TrenchWarfare.Domain.Enums;
using TrenchWarfare.ToolPanels.State;
using TrenchWarfare.Utility;
using UnityEngine.UI;

namespace TrenchWarfare.ToolPanels {
    public class EditorMenuPanel : EditorToolPanelBase {
        public List<EditorToolPanelBase> tools;

        public EditorState state;

        private bool start = true;

        void Awake() {
        }

        void Start() {
            GetToolToggle(state.ActiveTool).isOn = true;

            GetToolToggle(Tool.Units).interactable = false;
            GetToolToggle(Tool.ProductionCenters).interactable = false;
            GetToolToggle(Tool.Domains).interactable = false;

            start = false;
        }

        public void SelectTool(int index) {
            Tool selectedTool = (Tool)index;
            int toolsTotal = Enum.GetNames(typeof(Tool)).Length;
            
            for(int i=0; i < toolsTotal; i++) {
                Tool tool = (Tool)i;

                if(tool == selectedTool) {
                    tools[i].Show();
                } else {
                    tools[i].Hide();
                }
            }

            if(!start) {
                state.ActiveTool = selectedTool;
            }
        }

        public void OnNationsSet(IEnumerable<Nation> nations) {
            var enabled = nations.IsNotEmpty();
            GetToolToggle(Tool.Units).interactable = enabled;
            GetToolToggle(Tool.ProductionCenters).interactable = enabled;
            GetToolToggle(Tool.Domains).interactable = enabled;
         }

        private Toggle GetToolToggle(Tool tool) {
            String gameObjectName = GetToolToggleGameObjectName(tool);
            return GetComponent<Toggle>(gameObjectName);
        }

        private String GetToolToggleGameObjectName(Tool tool) {
            switch(tool) {
                case Tool.Terrain: return "Terrain Toggle";
                case Tool.Rivers: return "Rivers Toggle";
                case Tool.Roads: return "Roads Toggle";
                case Tool.Units: return "Units Toggle";
                case Tool.Domains: return "Domains Toggle";
                case Tool.ProductionCenters: return "PC Toggle";
                case Tool.TerrainModifiers: return "TM Toggle";
                case Tool.System: return "System Toggle";
                case Tool.SaveLoad: return "Files Toggle";
                default: return "";
            }       
        }
    }
}