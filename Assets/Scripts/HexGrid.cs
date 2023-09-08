using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TrenchWarfare.UI;
using TrenchWarfare.Domain.Enums;
using TrenchWarfare.Domain.Map;
using TrenchWarfare.Utility;
using TrenchWarfare.Domain.Units;

namespace TrenchWarfare {
	public class HexGrid : MonoBehaviour {
		int chunkCountX, chunkCountZ;

		GridModel model;

		HexGridChunk[] chunks;

		public HexCell cellPrefab;
		public Text cellLabelPrefab;
		public HexGridChunk chunkPrefab;
		public HexUnit unitPrefab;

		public Texture2D noiseSource;

		public ModelRegistry registry;

		public int seed;

		public Color[] colors;

        public int CellCountX {
			get => model == null ? HexMetrics.defaultCellCountX : model.CellCountX;
		}

        public int CellCountZ {
			get => model == null ? HexMetrics.defaultCellCountZ : model.CellCountZ;
		}

		void Awake () {
			HexMetrics.noiseSource = noiseSource;
			HexMetrics.InitializeHashGrid(seed);
			HexMetrics.colors = colors;

			CreateMap(HexMetrics.defaultCellCountX, HexMetrics.defaultCellCountZ);
		}

		public void CreateMap (int sizeX, int sizeZ) {
			if (sizeX <= 0 || sizeX % HexMetrics.chunkSizeX != 0
				|| sizeZ <= 0 || sizeZ % HexMetrics.chunkSizeZ != 0) {
				Debug.LogError("Unsupported map size.");
				return;
			}

			registry.Clear();
			model = new GridModel(sizeX, sizeZ);

			if (chunks != null) {
				for (int i = 0; i < chunks.Length; i++) {
					Destroy(chunks[i].gameObject);
				}
			}

			// And creating a new one
			chunkCountX = model.CellCountX / HexMetrics.chunkSizeX;
			chunkCountZ = model.CellCountZ / HexMetrics.chunkSizeZ;

			CreateChunks();
			CreateCells();
		}

		void CreateChunks () {
			chunks = new HexGridChunk[chunkCountX * chunkCountZ];

			for (int z = 0, i = 0; z < chunkCountZ; z++) {
				for (int x = 0; x < chunkCountX; x++) {
					HexGridChunk chunk = chunks[i++] = Instantiate(chunkPrefab);
					chunk.transform.SetParent(transform);
				}
			}
		}
		
		void CreateCells () {
			model.InitCells(model.CellCountZ * model.CellCountX);

			for (int z = 0, i = 0; z < model.CellCountZ; z++) {
				for (int x = 0; x < model.CellCountX; x++) {
					CreateCell(x, z, i++);
				}
			}
		}

		void OnEnable () {
			if (!HexMetrics.noiseSource) {
				HexMetrics.noiseSource = noiseSource;
				HexMetrics.InitializeHashGrid(seed);
				HexMetrics.colors = colors;
			}
		}
			
		public HexCell GetCell (Vector3 position) {
			position = transform.InverseTransformPoint(position);
			HexCoordinates coordinates = HexCell.FromPosition(position);

			int index = coordinates.X + coordinates.Z * model.CellCountX + coordinates.Z / 2;
			return GetCell(index);
		}	

		public HexCell GetCell (int xOffset, int zOffset) {
			return GetCell(xOffset + zOffset * model.CellCountX);
		}
		
		public HexCell GetCell (int cellIndex) {
			return registry.Get<HexCell>(model.GetCell(cellIndex));
		}

		void CreateCell (int x, int z, int i) {
			Vector3 position;
			position.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
			position.y = 0f;
			position.z = z * (HexMetrics.outerRadius * 1.5f);

			HexCell cell = Instantiate<HexCell>(cellPrefab);
			cell.AttachModelRegistry(registry);
			model.SetCell(i, cell.Model);
			cell.transform.localPosition = position;
			var coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
			cell.coordinates = coordinates;

			// We must connect cells
			if (x > 0) {
				cell.SetNeighbor(HexDirection.W, model.GetCell(i - 1));
			}
			if (z > 0) {
				if ((z & 1) == 0) {
					cell.SetNeighbor(HexDirection.SE, model.GetCell(i - model.CellCountX));
					if (x > 0) {
						cell.SetNeighbor(HexDirection.SW, model.GetCell(i - model.CellCountX - 1));
					}				
				} else {
					cell.SetNeighbor(HexDirection.SW, model.GetCell(i - model.CellCountX));
					if (x < model.CellCountX - 1) {
						cell.SetNeighbor(HexDirection.SE, model.GetCell(i - model.CellCountX + 1));
					}				
				}
			}		

			Text label = Instantiate<Text>(cellLabelPrefab);
			label.rectTransform.anchoredPosition = new Vector2(position.x, position.z);
			label.text = cell.coordinates.ToStringOnSeparateLines();
			label.color = Color.cyan;

			cell.uiRect = label.rectTransform;

			cell.UpdateElevation(0);

			AddCellToChunk(x, z, cell);
		}

		void AddCellToChunk (int x, int z, HexCell cell) {
			int chunkX = x / HexMetrics.chunkSizeX;
			int chunkZ = z / HexMetrics.chunkSizeZ;
			HexGridChunk chunk = chunks[chunkX + chunkZ * chunkCountX];

			int localX = x - chunkX * HexMetrics.chunkSizeX;
			int localZ = z - chunkZ * HexMetrics.chunkSizeZ;
			chunk.AddCell(localX + localZ * HexMetrics.chunkSizeX, cell);
		}

		public HexCell GetCell (HexCoordinates coordinates) {
			int z = coordinates.Z;
			if (z < 0 || z >= model.CellCountZ) {
				return null;
			}
			int x = coordinates.X + z / 2;
			if (x < 0 || x >= model.CellCountX) {
				return null;
			}
			return registry.Get<HexCell>(model.GetCell(x + z * model.CellCountX));
		}

		public void ShowUI (bool visible) {
			for (int i = 0; i < chunks.Length; i++) {
				chunks[i].ShowUI(visible);
			}
		}

		public void Save (BinaryWriter writer) {
			writer.Write(model.CellCountX);
			writer.Write(model.CellCountZ);

			foreach(var cell in model.Cells) {
				registry.Get<HexCell>(cell).Save(writer);
			}

			model.Conditions.SaveToBinary(writer);
		}

		public void Load (BinaryReader reader) {
			CreateMap(reader.ReadInt32(), reader.ReadInt32());


			foreach(var cell in model.Cells) {
				registry.Get<HexCell>(cell).Load(reader);
			}

			for (int i = 0; i < chunks.Length; i++) {
				chunks[i].Refresh();
			}

			int unitCount = reader.ReadInt32();
			for (int i = 0; i < unitCount; i++) {
				HexUnit.Load(reader, this);
			}

			model.Conditions.LoadFromBinary(reader);
		}

		public void AddUnit (HexCell cell, UnitModel unitModel) {
			if (cell && cell.Model.Unit == null) {
				var unit = Instantiate(unitPrefab);

				unit.Init(model.CellCountZ, unitModel, cell, registry);
				unit.transform.SetParent(transform, false);
			}
		}

		public void RemoveUnit (HexCell cell) {
			if (cell && cell.Model.Unit != null) {
				registry.Get<HexUnit>(cell.Model.Unit).Die();
			}
		}
	}
}