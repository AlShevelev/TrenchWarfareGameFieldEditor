using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Tools = TrenchWarfare.ToolPanels;
using TrenchWarfare.ToolPanels.State;
using System;
using System.IO;
using TrenchWarfare.Conditions;

namespace TrenchWarfare {
	public class HexMapEditor : MonoBehaviour {
		public HexGrid hexGrid;

		public HexMapCamera mainCamera;

		public InputField nameInput;

		public InputField metadataNameInput;

		public EditorState state;

		bool isDrag;
		HexDirection dragDirection;
		HexCell previousCell;

		MapConditions mapConditions;

		void Start() {
			mapConditions = new MapConditions();

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
				if(state.activeTool == Tools.Tool.Terrain) {
					if (state.terrainSelected == Tools.Terrain.Water) {
                        cell.WaterLevel = state.waterLevel;
                    } else {
                        cell.TerrainTypeIndex = TerrainToIndex(state.terrainSelected);
                        cell.Elevation = state.terrainElevation;
                    }
                }

                //if(state.activeTool == Tools.Tool.Water) {
                //	cell.WaterLevel = state.waterLevel;
                //}

                if (state.activeTool == Tools.Tool.Urban) {
					cell.UrbanLevel = state.urbanLevel;
				}

				if (state.activeTool == Tools.Tool.Farms) {
					cell.FarmLevel = state.farmLevel;
				}
				if (state.activeTool == Tools.Tool.Plants) {
					cell.PlantLevel = state.plantLevel;
				}

				if(state.activeTool == Tools.Tool.Rivers && !state.riversIsOn) {
					cell.RemoveRiver();
				}

				if(state.activeTool == Tools.Tool.Roads && !state.roadsIsOn) {
					cell.RemoveRoads();
				}
				
				if(state.activeTool == Tools.Tool.Walls) {
					cell.Walled = state.wallsIsOn;
				}

				if (isDrag) {
					HexCell otherCell = cell.GetNeighbor(dragDirection.Opposite());
					if (otherCell) {
						if(state.activeTool == Tools.Tool.Rivers && state.riversIsOn) {
							otherCell.SetOutgoingRiver(dragDirection);
						}

						if(state.activeTool == Tools.Tool.Roads && state.roadsIsOn) {
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
			if (state.activeTool == Tools.Tool.Terrain) {
				actualBrushSize = state.brushSize;
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

				mapConditions.SaveToBinary(writer);
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
					mapConditions.LoadFromBinary(reader);
					mainCamera.setStartZoomAndPosition();
				}
				else {
					Debug.LogWarning("Unknown map format " + header);
				}
			}
		}

		public void ExportMetadata() {
			string fileName = metadataNameInput.text;
			string path = Path.Combine(Application.persistentDataPath, fileName);

			string rawData = mapConditions.ExportToJson();
			File.WriteAllText(path, rawData);
		}

		public void ImportMetadata() {
			string fileName = metadataNameInput.text;
			string path = Path.Combine(Application.persistentDataPath, fileName);

			if(!File.Exists(path)) {
				Debug.LogWarning("File not found: "+fileName);
				return;
			}

			 string rawData = File.ReadAllText(path);

			 mapConditions.ImportFromJson(rawData);
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

		private int TerrainToIndex(Tools.Terrain terrain) {
			switch(terrain) {
				case Tools.Terrain.Sand:  return 0;
				case Tools.Terrain.Grass:  return 1;
				case Tools.Terrain.Mud:  return 2;
				case Tools.Terrain.Stone:  return 3;
				case Tools.Terrain.Snow:  return 4;
                case Tools.Terrain.Water: return 5;
                default: throw new InvalidOperationException("This terrain type is not supported: " + terrain);
			}
		}
	}
}