using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TrenchWarfare.Domain.Enums;
using TrenchWarfare.Domain.Map;
using TrenchWarfare.UI;
using TrenchWarfare.Utility;

namespace TrenchWarfare {
	public class HexCell : MonoBehaviour {
		ModelRegistry modelRegistry;

		CellModel model;
		public CellModelRead Model { get => model; }

		public HexCoordinates coordinates;

		public Color Color { get => HexMetrics.colors[(int)model.TerrainType];	}

		public RectTransform uiRect;

		public HexGridChunk chunk;

		public Vector3 Position {
			get {
				return transform.localPosition;
			}
		}

		public float StreamBedY {
			get	=> (model.Elevation + HexMetrics.streamBedElevationOffset) * HexMetrics.elevationStep;
		}

		public float RiverSurfaceY {
			get {
				return
					(model.Elevation + HexMetrics.waterElevationOffset) *
					HexMetrics.elevationStep;
			}
		}

		public float WaterSurfaceY {
			get => 	(model.WaterLevel + HexMetrics.waterElevationOffset) *	HexMetrics.elevationStep;
		}

		public HexUnit Unit { get; set; }

		void Awake () {
			model = new CellModel();
		}

        private void OnDestroy() {
            modelRegistry.Unregister(model);
        }

        public void AttachModelRegistry(ModelRegistry registry) {
			modelRegistry = registry;
			modelRegistry.Register(model, this);
		}

		public void UpdateWalled(bool walled) {
			model.Walled = walled;
			Refresh();
		}

		public void UpdateUrbanLevel(int urbanLevel) {
			model.UrbanLevel = urbanLevel;
			RefreshSelfOnly();
		}

		public void UpdateWaterLevel(int waterLevel) {
			model.WaterLevel = waterLevel;

			ValidateRivers();
			Refresh();
		}

		public void UpdateElevation(int elevation) {
			model.Elevation = elevation;

			RefreshPosition();
			ValidateRivers();

			for (int i = 0; i < model.Roads.Length; i++) {
				if (model.Roads[i] && GetElevationDifference((HexDirection)i) > 1) {
					SetRoad(i, false);
				}
			}

			Refresh();
		}

		public void UpdateTerrainType(CellTerrain terrainType) {
			if (model.TerrainType != terrainType) {
				model.TerrainType = terrainType;
				Refresh();
			}
		}
		
		public HexCell GetNeighbor (HexDirection direction) {
			return modelRegistry.Get<HexCell>(model.Neighbors[(int)direction]);
		}

		public void SetNeighbor (HexDirection direction, HexCell cell) {
			model.Neighbors[(int)direction] = cell.Model;
			cell.Model.Neighbors[(int)direction.Opposite()] = model;
		}

		public HexEdgeType GetEdgeType (HexDirection direction) {
			return HexMetrics.GetEdgeType(
				model.Elevation, model.Neighbors[(int)direction].Elevation
			);
		}

		public HexEdgeType GetEdgeType (HexCell otherCell) {
			return HexMetrics.GetEdgeType(
				model.Elevation, otherCell.Model.Elevation
			);
		}

		public void RemoveRiver () {
			RemoveOutgoingRiver();
			RemoveIncomingRiver();
		}

		public void RemoveOutgoingRiver () {
			if (!model.HasOutgoingRiver) {
				return;
			}

			model.HasOutgoingRiver = false;
			RefreshSelfOnly();

			HexCell neighbor = GetNeighbor(model.OutgoingRiver);
			neighbor.model.HasIncomingRiver = false;
			neighbor.RefreshSelfOnly();
		}

		public void RemoveIncomingRiver () {
			if (!model.HasIncomingRiver) {
				return;
			}
			model.HasIncomingRiver = false;
			RefreshSelfOnly();

			HexCell neighbor = GetNeighbor(model.IncomingRiver);
			neighbor.model.HasOutgoingRiver = false;
			neighbor.RefreshSelfOnly();
		}

		public void SetOutgoingRiver (HexDirection direction) {
			if (model.HasOutgoingRiver && model.OutgoingRiver == direction) {
				return;
			}

			HexCell neighbor = GetNeighbor(direction);
			if (!IsValidRiverDestination(neighbor)) {
				return;
			}

			RemoveOutgoingRiver();
			if (model.HasIncomingRiver && model.IncomingRiver == direction) {
				RemoveIncomingRiver();
			}

			model.HasOutgoingRiver = true;
			model.OutgoingRiver = direction;

			neighbor.RemoveIncomingRiver();
			neighbor.model.HasIncomingRiver = true;
			neighbor.model.IncomingRiver = direction.Opposite();

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

				for (int i = 0; i < model.Neighbors.Length; i++) {
					HexCell neighbor = modelRegistry.Get<HexCell>(model.Neighbors[i]);
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
			if (!model.Roads[(int)direction] && !model.HasRiverThroughEdge(direction) &&
				GetElevationDifference(direction) <= 1) {
				SetRoad((int)direction, true);
			}
		}

		public void RemoveRoads () {
			for (int i = 0; i < model.Neighbors.Length; i++) {
				if (model.Roads[i]) {
					SetRoad(i, false);
				}
			}
		}

		void SetRoad (int index, bool state) {
			model.Roads[index] = state;

			var neighbor = modelRegistry.Get<HexCell>(model.Neighbors[index]);

			neighbor.Model.Roads[(int)((HexDirection)index).Opposite()] = state;
			neighbor.RefreshSelfOnly();

			RefreshSelfOnly();
		}

		public int GetElevationDifference (HexDirection direction) {
			int difference = model.Elevation - GetNeighbor(direction).Model.Elevation;
			return difference >= 0 ? difference : -difference;
		}

		bool IsValidRiverDestination (HexCell neighbor) {
			return neighbor && (
				model.Elevation >= neighbor.Model.Elevation ||
				model.WaterLevel == neighbor.Model.Elevation
			);
		}

		void ValidateRivers () {
			if (
				model.HasOutgoingRiver &&
				!IsValidRiverDestination(GetNeighbor(model.OutgoingRiver))
			) {
				RemoveOutgoingRiver();
			}
			if (
				model.HasIncomingRiver &&
				!GetNeighbor(model.IncomingRiver).IsValidRiverDestination(this)
			) {
				RemoveIncomingRiver();
			}
		}

		public void Save (BinaryWriter writer) {
			writer.Write((byte)model.TerrainType);
			writer.Write((byte)(model.Elevation + 127));
			writer.Write((byte)model.WaterLevel);
			writer.Write((byte)model.UrbanLevel);
			writer.Write(model.Walled);

			writer.Write(model.HasIncomingRiver);
			writer.Write((byte)model.IncomingRiver);

			writer.Write(model.HasOutgoingRiver);
			writer.Write((byte)model.OutgoingRiver);

			for (int i = 0; i < model.Roads.Length; i++) {
				writer.Write(model.Roads[i]);
			}
		}

		public void Load (BinaryReader reader) {
			model.TerrainType = (CellTerrain)reader.ReadByte();
			model.Elevation = reader.ReadByte() - 127;
			RefreshPosition();
			model.WaterLevel = reader.ReadByte();
			model.UrbanLevel = reader.ReadByte();

			model.Walled = reader.ReadBoolean();

			model.HasIncomingRiver = reader.ReadBoolean();
			model.IncomingRiver = (HexDirection)reader.ReadByte();

			model.HasOutgoingRiver = reader.ReadBoolean();
			model.OutgoingRiver = (HexDirection)reader.ReadByte();

			for (int i = 0; i < model.Roads.Length; i++) {
				model.Roads[i] = reader.ReadBoolean();
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
			position.y = model.Elevation * HexMetrics.elevationStep;
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
