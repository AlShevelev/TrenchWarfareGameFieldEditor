using TrenchWarfare.Domain.Enums;

namespace TrenchWarfare.Domain.Game {
    public interface NationGameStateRead {
        public Nation Nation { get; }

        public int Money { get; }

        public int IndustryPoints { get; }
    }
}

