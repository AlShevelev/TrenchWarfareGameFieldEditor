using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TrenchWarfare.Domain.Enums;

namespace TrenchWarfare {
	public class HexCell : MonoBehaviour {
		public HexCoordinates coordinates;

		bool hasIncomingRiver, hasOutgoingRiver;

		// In the case of an outgoing river, this indicates where it's going. 
		// And for an incoming river, this indicates where it's coming from.
		HexDirection incomingRiver, outgoingRiver;

		// The cell has a road in a certain direction
		[SerializeField]
		bool[] roads;

		public bool HasRoads {
			get {
				for (int i = 0; i < roads.Length; i++) {
					if (roads[i]) {
						return true;
					}
				}
				return false;
			}
		}

		public bool HasIncomingRiver {
			get {
				return hasIncomingRiver;
			}
		}

		public bool HasOutgoingRiver {
			get {
				return hasOutgoingRiver;
			}
		}

		public HexDirection IncomingRiver {
			get {
				return incomingRiver;
			}
		}

		public HexDirection OutgoingRiver {
			get {
				return outgoingRiver;
			}
		}

		public bool HasRiver {
			get {
				return hasIncomingRiver || hasOutgoingRiver;
			}
		}

		public bool HasRiverBeginOrEnd {
			get {
				return hasIncomingRiver != hasOutgoingRiver;
			}
		}

		public Color Color {
			get {
				return HexMetrics.colors[terrainTypeIndex];
			}
		}

		int terrainTypeIndex;
		public int TerrainTypeIndex {
			get {
				return terrainTypeIndex;
			}
			set {
				if (terrainTypeIndex != value) {
					terrainTypeIndex = value;
					Refresh();
				}
			}
		}

		[SerializeField]
		HexCell[] neighbors;

		public RectTransform uiRect;

		public HexGridChunk chunk;

		int elevation = int.MinValue;
		public int Elevation {
			get {
				return elevation;
			}
			set {
				// if (elevation == value) {
				// 	return;
				// }

				elevation = value;
				RefreshPosition();
				ValidateRivers();

				for (int i = 0; i < roads.Length; i++) {
					if (roads[i] && GetElevationDifference((HexDirection)i) > 1) {
						SetRoad(i, false);
					}
				}

				Refresh();
			}
		}

		public Vector3 Position {
			get {
				return transform.localPosition;
			}
		}

		public float StreamBedY {
			get {
				return (elevation + HexMetrics.streamBedElevationOffset) *	HexMetrics.elevationStep;
			}
		}

		public float RiverSurfaceY {
			get {
				return
					(elevation + HexMetrics.waterElevationOffset) *
					HexMetrics.elevationStep;
			}
		}

		public HexDirection RiverBeginOrEndDirection {
			get {
				return hasIncomingRiver ? incomingRiver : outgoingRiver;
			}
		}

		int waterLevel;
		public int WaterLevel {
			get {
				return waterLevel;
			}
			set {
				if (waterLevel == value) {
					return;
				}
				waterLevel = value;
				ValidateRivers();
				Refresh();
			}
		}

		public bool IsUnderwater {
			get {
				return waterLevel > elevation;
			}
		}

		public float WaterSurfaceY {
			get {
				return
					(waterLevel + HexMetrics.waterElevationOffset) *
					HexMetrics.elevationStep;
			}
		}

		int urbanLevel;

		public int UrbanLevel {
			get {
				return urbanLevel;
			}
			set {
				if (urbanLevel != value) {
					urbanLevel = value;
					RefreshSelfOnly();
				}
			}
		}

		int farmLevel;

		public int FarmLevel {
			get {
				return farmLevel;
			}
			set {
				if (farmLevel != value) {
					farmLevel = value;
					RefreshSelfOnly();
				}
			}
		}

        private UnitType _unit;

        public UnitType unit {
            get {
                return _unit;
            }
            set {
                if (_unit != value) {
                    _unit = value;
                    RefreshSelfOnly();
                }
            }
        }

        bool walled;

		public bool Walled {
			get {
				return walled;
			}
			set {
				if (walled != value) {
					walled = value;
					Refresh();
				}
			}
		}

		public HexUnit Unit { get; set; }

		int distance;
		public int Distance {
			get {
				return distance;
			}
			set {
				distance = value;
			}
		}

		public HexCell PathFrom { get; set; }

		public int SearchHeuristic { get; set; }

		public int SearchPriority {
			get {
				return distance + SearchHeuristic;
			}
		}

		public HexCell NextWithSamePriority { get; set; }

		public int SearchPhase { get; set; }

		public bool HasRoadThroughEdge (HexDirection direction) {
			return roads[(int)direction];
		}
		
		public HexCell GetNeighbor (HexDirection direction) {
			return neighbors[(int)direction];
		}

		public void SetNeighbor (HexDirection direction, HexCell cell) {
			neighbors[(int)direction] = cell;
			cell.neighbors[(int)direction.Opposite()] = this;
		}

		public HexEdgeType GetEdgeType (HexDirection direction) {
			return HexMetrics.GetEdgeType(
				elevation, neighbors[(int)direction].elevation
			);
		}

		public HexEdgeType GetEdgeType (HexCell otherCell) {
			return HexMetrics.GetEdgeType(
				elevation, otherCell.elevation
			);
		}

		public bool HasRiverThroughEdge (HexDirection direction) {
			return
				hasIncomingRiver && incomingRiver == direction ||
				hasOutgoingRiver && outgoingRiver == direction;
		}

		public void RemoveRiver () {
			RemoveOutgoingRiver();
			RemoveIncomingRiver();
		}

		public void RemoveOutgoingRiver () {
			if (!hasOutgoingRiver) {
				return;
			}
			hasOutgoingRiver = false;
			RefreshSelfOnly();

			HexCell neighbor = GetNeighbor(outgoingRiver);
			neighbor.hasIncomingRiver = false;
			neighbor.RefreshSelfOnly();
		}

		public void RemoveIncomingRiver () {
			if (!hasIncomingRiver) {
				return;
			}
			hasIncomingRiver = false;
			RefreshSelfOnly();

			HexCell neighbor = GetNeighbor(incomingRiver);
			neighbor.hasOutgoingRiver = false;
			neighbor.RefreshSelfOnly();
		}

		public void SetOutgoingRiver (HexDirection direction) {
			if (hasOutgoingRiver && outgoingRiver == direction) {
				return;
			}

			HexCell neighbor = GetNeighbor(direction);
			if (!IsValidRiverDestination(neighbor)) {
				return;
			}

			RemoveOutgoingRiver();
			if (hasIncomingRiver && incomingRiver == direction) {
				RemoveIncomingRiver();
			}

			hasOutgoingRiver = true;
			outgoingRiver = direction;

			neighbor.RemoveIncomingRiver();
			neighbor.hasIncomingRiver = true;
			neighbor.incomingRiver = direction.Opposite();

			SetRoad((int)direction, false);
		}

		void RefreshSelfOnly () {
			chunk.Refresh();

			if (Unit) {
				Unit.ValidateLocation();
			}
		}

		void Refresh () {
			if (chunk) {
				chunk.Refresh();

				for (int i = 0; i < neighbors.Length; i++) {
					HexCell neighbor = neighbors[i];
					if (neighbor != null && neighbor.chunk != chunk) {
						neighbor.chunk.Refresh();
					}
				}

				if (Unit) {
					Unit.ValidateLocation();
				}
			}
		}

		public void AddRoad (HexDirection direction) {
			if (!roads[(int)direction] && !HasRiverThroughEdge(direction) &&
				GetElevationDifference(direction) <= 1) {
				SetRoad((int)direction, true);
			}
		}

		public void RemoveRoads () {
			for (int i = 0; i < neighbors.Length; i++) {
				if (roads[i]) {
					SetRoad(i, false);
				}
			}
		}

		void SetRoad (int index, bool state) {
			roads[index] = state;
			neighbors[index].roads[(int)((HexDirection)index).Opposite()] = state;
			neighbors[index].RefreshSelfOnly();
			RefreshSelfOnly();
		}

		public int GetElevationDifference (HexDirection direction) {
			int difference = elevation - GetNeighbor(direction).elevation;
			return difference >= 0 ? difference : -difference;
		}

		bool IsValidRiverDestination (HexCell neighbor) {
			return neighbor && (
				elevation >= neighbor.elevation || waterLevel == neighbor.elevation
			);
		}

		void ValidateRivers () {
			if (
				hasOutgoingRiver &&
				!IsValidRiverDestination(GetNeighbor(outgoingRiver))
			) {
				RemoveOutgoingRiver();
			}
			if (
				hasIncomingRiver &&
				!GetNeighbor(incomingRiver).IsValidRiverDestination(this)
			) {
				RemoveIncomingRiver();
			}
		}

		public void Save (BinaryWriter writer) {
			writer.Write((byte)terrainTypeIndex);
			writer.Write((byte)(elevation + 127));
			writer.Write((byte)waterLevel);
			writer.Write((byte)urbanLevel);
			writer.Write((byte)farmLevel);
			writer.Write(walled);

			writer.Write(hasIncomingRiver);
			writer.Write((byte)incomingRiver);

			writer.Write(hasOutgoingRiver);
			writer.Write((byte)outgoingRiver);

			for (int i = 0; i < roads.Length; i++) {
				writer.Write(roads[i]);
			}
		}

		public void Load (BinaryReader reader) {
			terrainTypeIndex = reader.ReadByte();
			elevation = reader.ReadByte() - 127;
			RefreshPosition();
			waterLevel = reader.ReadByte();
			urbanLevel = reader.ReadByte();
			farmLevel = reader.ReadByte();

			walled = reader.ReadBoolean();

			hasIncomingRiver = reader.ReadBoolean();
			incomingRiver = (HexDirection)reader.ReadByte();

			hasOutgoingRiver = reader.ReadBoolean();
			outgoingRiver = (HexDirection)reader.ReadByte();

			for (int i = 0; i < roads.Length; i++) {
				roads[i] = reader.ReadBoolean();
			}
		}

		void RefreshPosition () {
			Vector3 position = transform.localPosition;
			position.y = elevation * HexMetrics.elevationStep;
			position.y +=
				(HexMetrics.SampleNoise(position).y * 2f - 1f) *
				HexMetrics.elevationPerturbStrength;
			transform.localPosition = position;

			Vector3 uiPosition = uiRect.localPosition;
			uiPosition.z = -position.y;
			uiRect.localPosition = uiPosition;
		}
	}
}
