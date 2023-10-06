using TrenchWarfare.Domain.Enums;

namespace TrenchWarfare.Domain.Game {
    public class NationGameState: NationGameStateRead {
        Nation nation;
        public Nation Nation { get => nation; set => nation = value; }

        int money;
        public int Money { get => money; set => money = value; }

        int industryPoints;
        public int IndustryPoints { get => industryPoints; set => industryPoints = value; }

        public NationGameState(Nation nation, int money, int industryPoints) {
            this.nation = nation;
            this.money = money;
            this.industryPoints = industryPoints;
        }
    }
}
