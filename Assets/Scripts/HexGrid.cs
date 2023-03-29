using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class HexGrid : MonoBehaviour {
	public HexCell cellPrefab;
	public Text cellLabelPrefab;

	public int cellCountX = 20, cellCountZ = 15;

	int chunkCountX, chunkCountZ;

    HexCell[] cells;

	HexGridChunk[] chunks;

	public Texture2D noiseSource;

	public HexGridChunk chunkPrefab;

	public int seed;

	public Color[] colors;

	List<HexUnit> units = new List<HexUnit>();

	public HexUnit unitPrefab;

	HexCellPriorityQueue searchFrontier;

    void Awake () {
		HexMetrics.noiseSource = noiseSource;
		HexMetrics.InitializeHashGrid(seed);
		HexUnit.unitPrefab = unitPrefab;
		HexMetrics.colors = colors;

		CreateMap(cellCountX, cellCountZ);
	}

	public void CreateMap (int x, int z) {
		if (x <= 0 || x % HexMetrics.chunkSizeX != 0 || z <= 0 || z % HexMetrics.chunkSizeZ != 0) {
			Debug.LogError("Unsupported map size.");
			return;
		}

		// Clearing an old map
		ClearUnits();
		if (chunks != null) {
			for (int i = 0; i < chunks.Length; i++) {
				Destroy(chunks[i].gameObject);
			}
		}

		// And creating a new one
		cellCountX = x;
		cellCountZ = z;
		chunkCountX = cellCountX / HexMetrics.chunkSizeX;
		chunkCountZ = cellCountZ / HexMetrics.chunkSizeZ;

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
		cells = new HexCell[cellCountZ * cellCountX];

		for (int z = 0, i = 0; z < cellCountZ; z++) {
			for (int x = 0; x < cellCountX; x++) {
				CreateCell(x, z, i++);
			}
		}
	}

	void OnEnable () {
		if (!HexMetrics.noiseSource) {
			HexMetrics.noiseSource = noiseSource;
			HexMetrics.InitializeHashGrid(seed);
			HexUnit.unitPrefab = unitPrefab;
			HexMetrics.colors = colors;
		}
	}
		
	public HexCell GetCell (Vector3 position) {
		position = transform.InverseTransformPoint(position);
		HexCoordinates coordinates = HexCoordinates.FromPosition(position);
		int index = coordinates.X + coordinates.Z * cellCountX + coordinates.Z / 2;
		return cells[index];
	}	

	void CreateCell (int x, int z, int i) {
		Vector3 position;
		position.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
		position.y = 0f;
		position.z = z * (HexMetrics.outerRadius * 1.5f);

		HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);		
		cell.transform.localPosition = position;
		var coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
		cell.coordinates = coordinates;

		// We must connect cells
		if (x > 0) {
			cell.SetNeighbor(HexDirection.W, cells[i - 1]);
		}
		if (z > 0) {
			if ((z & 1) == 0) {
				cell.SetNeighbor(HexDirection.SE, cells[i - cellCountX]);
				if (x > 0) {
					cell.SetNeighbor(HexDirection.SW, cells[i - cellCountX - 1]);
				}				
			} else {
				cell.SetNeighbor(HexDirection.SW, cells[i - cellCountX]);
				if (x < cellCountX - 1) {
					cell.SetNeighbor(HexDirection.SE, cells[i - cellCountX + 1]);
				}				
			}
		}		

		Text label = Instantiate<Text>(cellLabelPrefab);
		label.rectTransform.anchoredPosition = new Vector2(position.x, position.z);
		label.text = cell.coordinates.ToStringOnSeparateLines();

		cell.uiRect = label.rectTransform;

		cell.Elevation = 0;

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
		if (z < 0 || z >= cellCountZ) {
			return null;
		}
		int x = coordinates.X + z / 2;
		if (x < 0 || x >= cellCountX) {
			return null;
		}
		return cells[x + z * cellCountX];
	}

	public void ShowUI (bool visible) {
		for (int i = 0; i < chunks.Length; i++) {
			chunks[i].ShowUI(visible);
		}
	}

	public void Save (BinaryWriter writer) {
		writer.Write(cellCountX);
		writer.Write(cellCountZ);

		for (int i = 0; i < cells.Length; i++) {
			cells[i].Save(writer);
		}

		writer.Write(units.Count);
		for (int i = 0; i < units.Count; i++) {
			units[i].Save(writer);
		}
	}

	public void Load (BinaryReader reader) {
		StopAllCoroutines();

		ClearUnits();

		CreateMap(reader.ReadInt32(), reader.ReadInt32());

		for (int i = 0; i < cells.Length; i++) {
			cells[i].Load(reader);
		}

		for (int i = 0; i < chunks.Length; i++) {
			chunks[i].Refresh();
		}

		int unitCount = reader.ReadInt32();
		for (int i = 0; i < unitCount; i++) {
			HexUnit.Load(reader, this);
		}
	}

	void ClearUnits () {
		for (int i = 0; i < units.Count; i++) {
			units[i].Die();
		}
		units.Clear();
	}

	public void AddUnit (HexUnit unit, HexCell location) {
		units.Add(unit);
		unit.transform.SetParent(transform, false);
		unit.Location = location;
	}

	public void RemoveUnit (HexUnit unit) {
		units.Remove(unit);
		unit.Die();
	}

	public void FindPath (HexCell fromCell, HexCell toCell) {
		StopAllCoroutines();
		StartCoroutine(Search(fromCell, toCell));
	}

	IEnumerator Search (HexCell fromCell, HexCell toCell) {
		if (searchFrontier == null) {
			searchFrontier = new HexCellPriorityQueue();
		}
		else {
			searchFrontier.Clear();
		}

		for (int i = 0; i < cells.Length; i++) {
			cells[i].Distance = int.MaxValue;
		}

		WaitForSeconds delay = new WaitForSeconds(1 / 60f);
		
		fromCell.Distance = 0;
		searchFrontier.Enqueue(fromCell);

		while (searchFrontier.Count > 0) {
			yield return delay;

			HexCell current = searchFrontier.Dequeue();
			
			if (current == toCell) {
				break;
			}

			for (HexDirection d = HexDirection.NE; d <= HexDirection.NW; d++) {
				HexCell neighbor = current.GetNeighbor(d);

				if (neighbor == null) {
					continue;
				}
				if (neighbor.IsUnderwater) {
					continue;
				}

				HexEdgeType edgeType = current.GetEdgeType(neighbor);
				if (edgeType == HexEdgeType.Cliff) {
					continue;
				}

				int distance = current.Distance;
				if (current.HasRoadThroughEdge(d)) {
					distance += 1;
				}
				else {
					distance += edgeType == HexEdgeType.Flat ? 5 : 10;
					distance += neighbor.UrbanLevel + neighbor.FarmLevel + neighbor.PlantLevel;
				}

				if (neighbor.Distance == int.MaxValue) {
					neighbor.Distance = distance;
					neighbor.PathFrom = current;

					neighbor.SearchHeuristic = neighbor.coordinates.DistanceTo(toCell.coordinates);

					searchFrontier.Enqueue(neighbor);
				}
				else if (current.Walled != neighbor.Walled) {
					continue;
				}
				else if (distance < neighbor.Distance) {
					int oldPriority = neighbor.SearchPriority;
					neighbor.Distance = distance;
					neighbor.PathFrom = current;
					searchFrontier.Change(neighbor, oldPriority);
				}
			}
		}
	}
}
