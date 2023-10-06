using System.Collections.Generic;

namespace TrenchWarfare.Domain.Game {
    public interface GameStateRead {
        int TurnNumber { get; }

        public IEnumerable<NationGameStateRead> Nations { get; }
    }
}

