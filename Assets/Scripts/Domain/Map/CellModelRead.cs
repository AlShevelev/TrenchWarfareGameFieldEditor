using System;
using TrenchWarfare.Domain.Enums;

namespace TrenchWarfare.Domain.Map {
    public interface CellModelRead: Model {
        bool HasIncomingRiver { get; }

        bool HasOutgoingRiver { get; }

        HexDirection IncomingRiver { get; }

		HexDirection OutgoingRiver { get; }

		bool HasRiver { get; }

		bool HasRiverBeginOrEnd { get; }

        bool[] Roads { get; }
        bool HasRoads { get; }

        bool HasRiverThroughEdge (HexDirection direction);

        bool HasRoadThroughEdge (HexDirection direction);

		HexDirection RiverBeginOrEndDirection {	get; }

        CellTerrain TerrainType { get; }

		int Elevation { get; }

        int WaterLevel { get; }

        bool IsUnderwater { get; }

        int UrbanLevel { get; }

		bool Walled { get; }

        CellModelRead[] Neighbors { get; }
    }
}

