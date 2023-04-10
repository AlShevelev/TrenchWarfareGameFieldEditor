using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.IO;

public class HexMapEditor : MonoBehaviour {
	public HexGrid hexGrid;

	//int activeElevation;

	//bool applyElevation = true;

	//int brushSize;

	enum OptionalToggle {
		Ignore, Yes, No
	}
	OptionalToggle riverMode, roadMode, walledMode;

	bool isDrag;
	HexDirection dragDirection;
	HexCell previousCell;

	int activeWaterLevel;

	bool applyWaterLevel = true;

	int activeUrbanLevel;

	bool applyUrbanLevel;

	int activeFarmLevel;

	bool applyFarmLevel;


	int activePlantLevel;

	bool applyPlantLevel;

	//int activeTerrainTypeIndex;	

	public HexMapCamera mainCamera;

	public InputField nameInput;

	public EditorState state;

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

	public void SetTerrainTypeIndex (int index) {
		//activeTerrainTypeIndex = index;
	}

	public void SetElevation (float elevation) {
		//activeElevation = (int)elevation;
	}	

	public void SetApplyElevation (bool toggle) {
		//applyElevation = toggle;
	}

	public void SetBrushSize (float size) {
		//brushSize = (int)size;
	}

	public void ShowUI (bool visible) {
		hexGrid.ShowUI(visible);
	}

	public void SetRiverMode (int mode) {
		riverMode = (OptionalToggle)mode;
	}

	public void SetRoadMode (int mode) {
		roadMode = (OptionalToggle)mode;
	}

	public void SetApplyWaterLevel (bool toggle) {
		applyWaterLevel = toggle;
	}
	
	public void SetWaterLevel (float level) {
		activeWaterLevel = (int)level;
	}	

	public void SetApplyUrbanLevel (bool toggle) {
		applyUrbanLevel = toggle;
	}
	
	public void SetUrbanLevel (float level) {
		activeUrbanLevel = (int)level;
	}

	public void SetApplyFarmLevel (bool toggle) {
		applyFarmLevel = toggle;
	}

	public void SetFarmLevel (float level) {
		activeFarmLevel = (int)level;
	}

	public void SetApplyPlantLevel (bool toggle) {
		applyPlantLevel = toggle;
	}

	public void SetPlantLevel (float level) {
		activePlantLevel = (int)level;
	}
		
	void EditCell (HexCell cell) {
		if (cell) {
			// if (activeTerrainTypeIndex >= 0) {
			// 	cell.TerrainTypeIndex = activeTerrainTypeIndex;
			// }
			if(state.activeTool == Tool.Terrain) {
				cell.TerrainTypeIndex = TerrainToIndex(state.terrainSelected);
				cell.Elevation = state.terrainElevation;
			}
			// if (applyElevation) {
			// 	cell.Elevation = activeElevation;
			// }
			if (applyWaterLevel) {
				cell.WaterLevel = activeWaterLevel;
			}			
			if (applyUrbanLevel) {
				cell.UrbanLevel = activeUrbanLevel;
			}
			if (applyFarmLevel) {
				cell.FarmLevel = activeFarmLevel;
			}
			if (applyPlantLevel) {
				cell.PlantLevel = activePlantLevel;
			}
			if (riverMode == OptionalToggle.No) {
				cell.RemoveRiver();
			}
			if (roadMode == OptionalToggle.No) {
				cell.RemoveRoads();
			}
			if (walledMode != OptionalToggle.Ignore) {
				cell.Walled = walledMode == OptionalToggle.Yes;
			}
			if (isDrag) {
				HexCell otherCell = cell.GetNeighbor(dragDirection.Opposite());
				if (otherCell) {
					if (riverMode == OptionalToggle.Yes) {
						otherCell.SetOutgoingRiver(dragDirection);
					}
					if (roadMode == OptionalToggle.Yes) {
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

		
		// if(riverMode == OptionalToggle.Yes || roadMode == OptionalToggle.Yes) {
		// 	actualBrushSize = 0;
		// }

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

	public void SetWalledMode (int mode) {
		walledMode = (OptionalToggle)mode;
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