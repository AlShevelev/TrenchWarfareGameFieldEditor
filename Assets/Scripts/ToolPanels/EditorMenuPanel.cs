using System;
using System.Collections;
using System.Collections.Generic;
using TrenchWarfare.ToolPanels.State;
using UnityEngine;
using UnityEngine.UI;

namespace TrenchWarfare.ToolPanels {
    public class EditorMenuPanel : EditorToolPanelBase {
        public List<EditorToolPanelBase> tools;

        public EditorState state;

        private bool start = true;

        void Awake() {
        }

        void Start() {
            GetToolToggle(state.activeTool).isOn = true;
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
                state.activeTool = selectedTool;
            }
        }

        private Toggle GetToolToggle(Tool tool) {
            String gameObjectName = GetToolToggleGameObjectName(tool);
            return GetComponent<Toggle>(gameObjectName);
        }

        private String GetToolToggleGameObjectName(Tool tool) {
            switch(tool) {
                case Tool.Terrain: return "Terrain Toggle";
                case Tool.Water: return "Water Toggle";
                case Tool.Rivers: return "Rivers Toggle";
                case Tool.Roads: return "Roads Toggle";
                case Tool.Walls: return "Walls Toggle";
                case Tool.Urban: return "Urban Toggle";
                case Tool.Farms: return "Farms Toggle";
                case Tool.Plants: return "Plants Toggle";
                case Tool.System: return "System Toggle";
                case Tool.SaveLoad: return "Files Toggle";
                default: return "";
            }       
        }
    }
}