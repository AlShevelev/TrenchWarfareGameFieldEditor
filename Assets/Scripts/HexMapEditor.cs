using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.IO;

public class HexMapEditor : MonoBehaviour {
	public HexGrid hexGrid;

	bool isDrag;
	HexDirection dragDirection;
	HexCell previousCell;

	public HexMapCamera mainCamera;

	public InputField nameInput;

	public EditorState state;

	void Start() {
		UpdateLevelsVisibility();
	}

	void Update () {
		if (!EventSystem.current.IsPointerOverGameObject()) {
			if (Input.GetMouseButton(0)) {
				HandleInput();
				return;
			}
			if (Input.GetKeyDown(KeyCode.U)) {
				if (Input.GetKey(KeyCode.LeftShift)) {
					DestroyUnit();
				}
				else {
					CreateUnit();
				}
			}
		}
		previousCell = null;
	}

	void HandleInput () {
		HexCell currentCell = GetCellUnderCursor();
		if (currentCell) {
			if (previousCell && previousCell != currentCell) {
				ValidateDrag(currentCell);
			}
			else {
				isDrag = false;
			}			
			EditCells(currentCell);
			previousCell = currentCell;
		}
		else {
			previousCell = null;
		}
	}

	void ValidateDrag (HexCell currentCell) {
		for (
			dragDirection = HexDirection.NE;
			dragDirection <= HexDirection.NW;
			dragDirection++
		) {
			if (previousCell.GetNeighbor(dragDirection) == currentCell) {
				isDrag = true;
				return;
			}
		}
		isDrag = false;
	}

	public void UpdateLevelsVisibility() {
		hexGrid.ShowUI(state.labelsIsOn);
	}
		
	void EditCell (HexCell cell) {
		if (cell) {
			if(state.activeTool == Tool.Terrain) {
				cell.TerrainTypeIndex = TerrainToIndex(state.terrainSelected);
				cell.Elevation = state.terrainElevation;
			}

			if(state.activeTool == Tool.Water) {
				cell.WaterLevel = state.waterLevel;
			}

			if(state.activeTool == Tool.Urban) {
				cell.UrbanLevel = state.urbanLevel;
			}

			if (state.activeTool == Tool.Farms) {
				cell.FarmLevel = state.farmLevel;
			}
			if (state.activeTool == Tool.Plants) {
				cell.PlantLevel = state.plantLevel;
			}

			if(state.activeTool == Tool.Rivers && !state.riversIsOn) {
				cell.RemoveRiver();
			}

			if(state.activeTool == Tool.Roads && !state.roadsIsOn) {
				cell.RemoveRoads();
			}
			
			if(state.activeTool == Tool.Walls) {
				cell.Walled = state.wallsIsOn;
			}

			if (isDrag) {
				HexCell otherCell = cell.GetNeighbor(dragDirection.Opposite());
				if (otherCell) {
					if(state.activeTool == Tool.Rivers && state.riversIsOn) {
						otherCell.SetOutgoingRiver(dragDirection);
					}

					if(state.activeTool == Tool.Roads && state.roadsIsOn) {
						otherCell.AddRoad(dragDirection);
					}
				}
			}			
		}
	}

	void EditCells (HexCell center) {
		int centerX = center.coordinates.X;
		int centerZ = center.coordinates.Z;

		int actualBrushSize = 0;
		switch(state.activeTool) {
			case Tool.Terrain: {
				actualBrushSize = state.terrainBrushSize;
				break;
			}
			case Tool.Water: {
				actualBrushSize = state.waterBrushSize;
				break;
			}
			default: {
				// Brush size is ignored for rivers and roads
				actualBrushSize = 0;
				break;
			}
		}

		for (int r = 0, z = centerZ - actualBrushSize; z <= centerZ; z++, r++) {
			for (int x = centerX - r; x <= centerX + actualBrushSize; x++) {
				EditCell(hexGrid.GetCell(new HexCoordinates(x, z)));
			}
		}

		for (int r = 0, z = centerZ + actualBrushSize; z > centerZ; z--, r++) {
			for (int x = centerX - actualBrushSize; x <= centerX + r; x++) {
				EditCell(hexGrid.GetCell(new HexCoordinates(x, z)));
			}
		}
	}

	public void Save() {
		Debug.Log(Application.persistentDataPath);

		string fileName = nameInput.text;
		string path = Path.Combine(Application.persistentDataPath, fileName);

		using(BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.Create))) {
			writer.Write(0);			// Header (format version - 0)
			hexGrid.Save(writer);
		}
	}

	public void Load() {
		string fileName = nameInput.text;

		string path = Path.Combine(Application.persistentDataPath, fileName);

		if(!File.Exists(path)) {
			Debug.LogWarning("File not found: "+fileName);
			return;
		}

		using (BinaryReader reader = new BinaryReader(File.OpenRead(path))) {
			int header = reader.ReadInt32();

			if (header == 0) {			// Format version checking
				hexGrid.Load(reader);
				mainCamera.setStartZoomAndPosition();
			}
			else {
				Debug.LogWarning("Unknown map format " + header);
			}
		}
	}

	HexCell GetCellUnderCursor () {
		Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(inputRay, out hit)) {
			return hexGrid.GetCell(hit.point);
		}
		return null;
	}

	void CreateUnit () {
		HexCell cell = GetCellUnderCursor();
		if (cell && !cell.Unit) {
			hexGrid.AddUnit(Instantiate(HexUnit.unitPrefab), cell);
		}
	}

	void DestroyUnit () {
		HexCell cell = GetCellUnderCursor();
		if (cell && cell.Unit) {
			hexGrid.RemoveUnit(cell.Unit);
		}
	}

	private int TerrainToIndex(Terrain terrain) {
		switch(terrain) {
			case Terrain.Sand:  return 0;
			case Terrain.Grass:  return 1;
			case Terrain.Mud:  return 2;
			case Terrain.Stone:  return 3;
			case Terrain.Snow:  return 4;
			default: throw new InvalidOperationException("This terrain type is not supported: " + terrain);
		}
	}
}