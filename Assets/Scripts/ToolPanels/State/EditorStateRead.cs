using TrenchWarfare.Domain.Enums;

namespace TrenchWarfare.ToolPanels.State {
    public interface EditorStateRead {
        Tool ActiveTool {
            get;
        }

        CellTerrain TerrainSelected {
            get;
        }    

        int TerrainElevation {
            get;
        }

        int BrushSize {
            get;
        }
        
        int WaterLevel {
            get;
        }

        bool RiversIsOn {
            get;
        }

        bool RoadsIsOn {
            get;
        }

        bool WallsIsOn {
            get;
        }

        bool LabelsIsOn {
            get;
        }
    }
}