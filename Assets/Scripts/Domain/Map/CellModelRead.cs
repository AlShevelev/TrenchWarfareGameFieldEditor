using System;
using TrenchWarfare.Domain.Enums;

namespace TrenchWarfare.Domain.Map {
    public interface CellModelRead {
        bool HasIncomingRiver { get; }

        bool HasOutgoingRiver { get; }

        HexDirection IncomingRiver { get; }

		HexDirection OutgoingRiver { get; }

		bool HasRiver { get; }

		bool HasRiverBeginOrEnd { get; }

        bool HasRiverThroughEdge (HexDirection direction);
    }
}

