using System;
using System.Collections.Generic;
using System.Linq;
using TrenchWarfare.Domain.Enums;
using TrenchWarfare.Domain.MapObjects;
using TrenchWarfare.Domain.Units;

namespace TrenchWarfare.Domain.Map {
    public class CellModel: Model, CellModelExternal {
		ArmyModelExternal army = null;
		public ArmyModelExternal Army { get => army; set => army = value; }

		ProductionCenterModelExternal productionCenter = null;
		public ProductionCenterModelExternal ProductionCenter {
			get => productionCenter; set => productionCenter = value;
		}

		bool hasIncomingRiver;
        public bool HasIncomingRiver { get => hasIncomingRiver; set => hasIncomingRiver = value; }

		bool hasOutgoingRiver;
		public bool HasOutgoingRiver { get => hasOutgoingRiver; set => hasOutgoingRiver = value; }

        // In the case of an outgoing river, this indicates where it's going. 
		// And for an incoming river, this indicates where it's coming from.
		HexDirection incomingRiver;
		public HexDirection IncomingRiver { get => incomingRiver; set => incomingRiver = value; }

        HexDirection outgoingRiver;
		public HexDirection OutgoingRiver { get => outgoingRiver; set => outgoingRiver = value; }

		public bool HasRiver { get => hasIncomingRiver || hasOutgoingRiver; }

		public bool HasRiverBeginOrEnd { get => hasIncomingRiver != hasOutgoingRiver; }

		// The cell has a road in a certain direction
		bool[] roads;
		public IEnumerable<bool> Roads { get => roads; }
		public bool HasRoads { get => roads.Any(i => i); }

		private CellTerrain terrainType;
		public CellTerrain TerrainType { get => terrainType; set => terrainType = value; }

		public HexDirection RiverBeginOrEndDirection {
			get => HasIncomingRiver ? IncomingRiver : OutgoingRiver;
		}

		int elevation = int.MinValue;
		public int Elevation { get => elevation; set => elevation = value; }

		int waterLevel;
		public int WaterLevel { get => waterLevel; set => waterLevel = value; }

		public bool IsUnderwater { get => waterLevel > elevation; }

        bool walled;
		public bool Walled { get => walled; set => walled = value; }

		CellModelExternal[] neighbors;
		public IEnumerable<CellModelExternal> Neighbors { get => neighbors; }

        public CellModel(): base() {
			roads = new bool[6];
			neighbors = new CellModel[6];
        }

		public bool HasRiverThroughEdge (HexDirection direction) {
			return
				hasIncomingRiver && incomingRiver == direction ||
				hasOutgoingRiver && outgoingRiver == direction;
		}

		public bool HasRoadThroughEdge (HexDirection direction) {
			return roads[(int)direction];
		}

		public void SetRoadThroughEdge (HexDirection direction, bool value) {
			roads[(int)direction] = value;
		}

		public CellModelExternal GetNeighbor(HexDirection direction) {
			return neighbors[(int)direction];
		}

		public void SetNeighbor(HexDirection direction, CellModelExternal model) {
			neighbors[(int)direction] = model;
		}
    }
}
