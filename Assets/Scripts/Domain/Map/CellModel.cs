using System;
using System.Linq;
using TrenchWarfare.Domain.Enums;

namespace TrenchWarfare.Domain.Map {
    public class CellModel: CellModelRead {
		bool _hasIncomingRiver;
        public bool HasIncomingRiver { get => _hasIncomingRiver; set => _hasIncomingRiver = value; }

		bool _hasOutgoingRiver;
		public bool HasOutgoingRiver { get => _hasOutgoingRiver; set => _hasOutgoingRiver = value; }

        // In the case of an outgoing river, this indicates where it's going. 
		// And for an incoming river, this indicates where it's coming from.
		HexDirection _incomingRiver;
		public HexDirection IncomingRiver { get => _incomingRiver; set => _incomingRiver = value; }

        HexDirection _outgoingRiver;
		public HexDirection OutgoingRiver { get => _outgoingRiver; set => _outgoingRiver = value; }

		public bool HasRiver { get => _hasIncomingRiver || _hasOutgoingRiver; }

		public bool HasRiverBeginOrEnd { get => _hasIncomingRiver != _hasOutgoingRiver; }

		// The cell has a road in a certain direction
		public bool[] Roads;
		public bool HasRoads { get => Roads.Any(i => i); }

		private CellTerrain _terrainType;
		public CellTerrain TerrainType { get => _terrainType; set => _terrainType = value; }

        public CellModel() {
			Roads = new bool[6];
        }

		public bool HasRiverThroughEdge (HexDirection direction) {
			return
				_hasIncomingRiver && _incomingRiver == direction ||
				_hasOutgoingRiver && _outgoingRiver == direction;
		}

		public bool HasRoadThroughEdge (HexDirection direction) {
			return Roads[(int)direction];
		}
    }
}
