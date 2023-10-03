using System;
using System.Collections.Generic;
using TrenchWarfare.Domain.Enums;
using TrenchWarfare.Domain.MapObjects;
using TrenchWarfare.Domain.Units;

namespace TrenchWarfare.Domain.Map {
    public interface CellModelExternal: IModel {
        ArmyModelExternal Army { get; }

        ProductionCenterModelExternal ProductionCenter { get; }

        TerrainModifierModelExternal TerrainModifier { get; }

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

        Nation? Owner { get; }

        bool HasRiverThroughEdge (HexDirection direction);

        bool HasRoadThroughEdge (HexDirection direction);

		void SetRoadThroughEdge (HexDirection direction, bool value);

        public IEnumerable<CellModelExternal> Neighbors { get; }

		CellModelExternal GetNeighbor(HexDirection direction);

		void SetNeighbor(HexDirection direction, CellModelExternal model);
    }
}

