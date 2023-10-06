using System.Collections.Generic;
using System.Linq;
using TrenchWarfare.Domain.Enums;

namespace TrenchWarfare.Domain.Game {
    public class GameState: GameStateRead {
        int turnNumber;
        public int TurnNumber { get => turnNumber; set => turnNumber = value; }

        Dictionary<Nation, NationGameState> nations;
        public IEnumerable<NationGameStateRead> Nations {
            get => nations.Values.Select(i => i);
        }

        public GameState(int turnNumber, Dictionary<Nation, NationGameState> nations) {
            this.turnNumber = turnNumber;
            this.nations = nations;
        }
    }
}
