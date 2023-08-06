using TrenchWarfare.Units;

namespace TrenchWarfare.ToolPanels.State {
    public interface EditorStateRead {
        Tool activeTool {
            get;
        }

        Terrain terrainSelected {
            get;
        }    

        int terrainElevation {
            get;
        }

        int brushSize {
            get;
        }
        
        int waterLevel {
            get;
        }

        bool riversIsOn {
            get;
        }

        bool roadsIsOn {
            get;
        }

        bool wallsIsOn {
            get;
        }

        int urbanLevel {
            get;
        }

        int farmLevel {
            get;
        }

        int plantLevel {
            get;
        }

        bool labelsIsOn {
            get;
        }
    }
}