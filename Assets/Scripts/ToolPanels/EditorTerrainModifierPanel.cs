using TrenchWarfare.Domain.Enums;
using TrenchWarfare.ToolPanels.State;

namespace TrenchWarfare.ToolPanels {
    public class EditorTerrainModifierPanel : EditorToolPanelBase {
        public EditorState state;

        public void SetType(int value) {
            state.TerrainModifier = state.TerrainModifier.Copy(i => {
                    i.Modifier = (TerrainModifierType)value;
                    return i;
                }
            );
        }
    }
}

