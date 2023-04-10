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

    int terrainBrushSize {
        get;
    }
    
    int waterLevel {
        get;
    }

    int waterBrushSize {
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