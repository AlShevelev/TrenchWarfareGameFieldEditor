﻿using System;
using System.Collections.Generic;
using TrenchWarfare.Domain.Enums;

namespace TrenchWarfare.Domain.Map {
    public interface CellModelRead: Model {
        bool HasIncomingRiver { get; }

        bool HasOutgoingRiver { get; }

        HexDirection IncomingRiver { get; }

		HexDirection OutgoingRiver { get; }

		bool HasRiver { get; }

		bool HasRiverBeginOrEnd { get; }

        IEnumerable<bool> Roads { get; }
        bool HasRoads { get; }

		HexDirection RiverBeginOrEndDirection {	get; }

        CellTerrain TerrainType { get; }

		int Elevation { get; }

        int WaterLevel { get; }

        bool IsUnderwater { get; }

        int UrbanLevel { get; }

		bool Walled { get; }

        bool HasRiverThroughEdge (HexDirection direction);

        bool HasRoadThroughEdge (HexDirection direction);

		void SetRoadThroughEdge (HexDirection direction, bool value);

        public IEnumerable<CellModelRead> Neighbors { get; }

		CellModelRead GetNeighbor(HexDirection direction);

		void SetNeighbor(HexDirection direction, CellModelRead model);
    }
}

