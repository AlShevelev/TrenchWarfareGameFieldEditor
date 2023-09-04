using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TrenchWarfare.Domain.Enums;
using TrenchWarfare.Domain.Map;
using TrenchWarfare.UI;

namespace TrenchWarfare {
	public class HexCell : MonoBehaviour {
		CellModel _model;
		public CellModelRead Model { get => _model; }

		public HexCoordinates coordinates;

		public Color Color {
			get {
				return HexMetrics.colors[(int)_model.TerrainType];
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

				for (int i = 0; i < _model.Roads.Length; i++) {
					if (_model.Roads[i] && GetElevationDifference((HexDirection)i) > 1) {
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
			get => _model.HasIncomingRiver ? _model.IncomingRiver : _model.OutgoingRiver;
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

		void Awake () {
			_model = new CellModel();
		}

		public void UpdateTerrainType(CellTerrain terrainType) {
			if (_model.TerrainType != terrainType) {
				_model.TerrainType = terrainType;
				Refresh();
			}
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

		public void RemoveRiver () {
			RemoveOutgoingRiver();
			RemoveIncomingRiver();
		}

		public void RemoveOutgoingRiver () {
			if (!_model.HasOutgoingRiver) {
				return;
			}

			_model.HasOutgoingRiver = false;
			RefreshSelfOnly();

			HexCell neighbor = GetNeighbor(_model.OutgoingRiver);
			neighbor._model.HasIncomingRiver = false;
			neighbor.RefreshSelfOnly();
		}

		public void RemoveIncomingRiver () {
			if (!_model.HasIncomingRiver) {
				return;
			}
			_model.HasIncomingRiver = false;
			RefreshSelfOnly();

			HexCell neighbor = GetNeighbor(_model.IncomingRiver);
			neighbor._model.HasOutgoingRiver = false;
			neighbor.RefreshSelfOnly();
		}

		public void SetOutgoingRiver (HexDirection direction) {
			if (_model.HasOutgoingRiver && _model.OutgoingRiver == direction) {
				return;
			}

			HexCell neighbor = GetNeighbor(direction);
			if (!IsValidRiverDestination(neighbor)) {
				return;
			}

			RemoveOutgoingRiver();
			if (_model.HasIncomingRiver && _model.IncomingRiver == direction) {
				RemoveIncomingRiver();
			}

			_model.HasOutgoingRiver = true;
			_model.OutgoingRiver = direction;

			neighbor.RemoveIncomingRiver();
			neighbor._model.HasIncomingRiver = true;
			neighbor._model.IncomingRiver = direction.Opposite();

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
			if (!_model.Roads[(int)direction] && !_model.HasRiverThroughEdge(direction) &&
				GetElevationDifference(direction) <= 1) {
				SetRoad((int)direction, true);
			}
		}

		public void RemoveRoads () {
			for (int i = 0; i < neighbors.Length; i++) {
				if (_model.Roads[i]) {
					SetRoad(i, false);
				}
			}
		}

		void SetRoad (int index, bool state) {
			_model.Roads[index] = state;
			neighbors[index]._model.Roads[(int)((HexDirection)index).Opposite()] = state;
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
				_model.HasOutgoingRiver &&
				!IsValidRiverDestination(GetNeighbor(_model.OutgoingRiver))
			) {
				RemoveOutgoingRiver();
			}
			if (
				_model.HasIncomingRiver &&
				!GetNeighbor(_model.IncomingRiver).IsValidRiverDestination(this)
			) {
				RemoveIncomingRiver();
			}
		}

		public void Save (BinaryWriter writer) {
			writer.Write((byte)_model.TerrainType);
			writer.Write((byte)(elevation + 127));
			writer.Write((byte)waterLevel);
			writer.Write((byte)urbanLevel);
			writer.Write(walled);

			writer.Write(_model.HasIncomingRiver);
			writer.Write((byte)_model.IncomingRiver);

			writer.Write(_model.HasOutgoingRiver);
			writer.Write((byte)_model.OutgoingRiver);

			for (int i = 0; i < _model.Roads.Length; i++) {
				writer.Write(_model.Roads[i]);
			}
		}

		public void Load (BinaryReader reader) {
			_model.TerrainType = (CellTerrain)reader.ReadByte();
			elevation = reader.ReadByte() - 127;
			RefreshPosition();
			waterLevel = reader.ReadByte();
			urbanLevel = reader.ReadByte();

			walled = reader.ReadBoolean();

			_model.HasIncomingRiver = reader.ReadBoolean();
			_model.IncomingRiver = (HexDirection)reader.ReadByte();

			_model.HasOutgoingRiver = reader.ReadBoolean();
			_model.OutgoingRiver = (HexDirection)reader.ReadByte();

			for (int i = 0; i < _model.Roads.Length; i++) {
				_model.Roads[i] = reader.ReadBoolean();
			}
		}

		public static HexCoordinates FromPosition (Vector3 position) {
			float x = position.x / (HexMetrics.innerRadius * 2f);
			float y = -x;

			float offset = position.z / (HexMetrics.outerRadius * 3f);
			x -= offset;
			y -= offset;

			int iX = Mathf.RoundToInt(x);
			int iY = Mathf.RoundToInt(y);
			int iZ = Mathf.RoundToInt(-x -y);

			if (iX + iY + iZ != 0) {
				float dX = Mathf.Abs(x - iX);
				float dY = Mathf.Abs(y - iY);
				float dZ = Mathf.Abs(-x -y - iZ);

				if (dX > dY && dX > dZ) {
					iX = -iY - iZ;
				}
				else if (dZ > dY) {
					iZ = -iX - iY;
				}
			}		

			return new HexCoordinates(iX, iZ);
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
