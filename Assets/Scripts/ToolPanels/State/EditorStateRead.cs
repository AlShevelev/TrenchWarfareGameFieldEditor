namespace TrenchWarfare.ToolPanels.State {
    public interface EditorStateRead {
        Tool ActiveTool {
            get;
        }

        Terrain TerrainSelected {
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

        int UrbanLevel {
            get;
        }

        int FarmLevel {
            get;
        }

        bool LabelsIsOn {
            get;
        }
    }
}