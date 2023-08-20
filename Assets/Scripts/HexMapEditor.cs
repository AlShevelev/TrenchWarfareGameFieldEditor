using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Tools = TrenchWarfare.ToolPanels;
using TrenchWarfare.ToolPanels.State;
using System;
using System.IO;
using TrenchWarfare.Conditions;
using TrenchWarfare.Domain.Enums;
using TrenchWarfare.Domain.Units;

namespace TrenchWarfare {
	public class HexMapEditor : MonoBehaviour {
		public HexGrid hexGrid;

		public HexMapCamera mainCamera;

		public InputField nameInput;

		public InputField metadataNameInput;

		public EditorState state;

        public Material terrainMaterial;

        bool isDrag;
		HexDirection dragDirection;
		HexCell previousCell;

		MapConditions mapConditions;

		private const String GRID_ENABLE_FLAG = "GRID_ON";

        void Awake() {
            terrainMaterial.DisableKeyword(GRID_ENABLE_FLAG);
        }

        void Start() {
			mapConditions = new MapConditions();

			UpdateLevelsVisibility();
		}

		void Update () {
			if (!EventSystem.current.IsPointerOverGameObject()) {
				if (Input.GetMouseButton(0)) {
					HandleInput(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift));
					return;
				}
			}
			previousCell = null;
		}

		void HandleInput (bool shiftPressed) {
			HexCell currentCell = GetCellUnderCursor();
			if (currentCell) {
				if (previousCell && previousCell != currentCell) {
					ValidateDrag(currentCell);
				}
				else {
					isDrag = false;
				}			
				EditCells(currentCell, shiftPressed);
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
			hexGrid.ShowUI(state.LabelsIsOn);
		}
			
		void EditCell (HexCell cell) {
			if (cell) {
				if(state.ActiveTool == Tools.Tool.Terrain) {
					if (state.TerrainSelected == Tools.Terrain.Water) {
                        cell.WaterLevel = state.WaterLevel;
                    } else {
                        cell.TerrainTypeIndex = TerrainToIndex(state.TerrainSelected);
                        cell.Elevation = state.TerrainElevation;
                    }
                }

                if (state.ActiveTool == Tools.Tool.Urban) {
					cell.UrbanLevel = state.UrbanLevel;		
				}

				if (state.ActiveTool == Tools.Tool.Farms) {
					cell.FarmLevel = state.FarmLevel;
				}

				if(state.ActiveTool == Tools.Tool.Rivers && !state.RiversIsOn) {
					cell.RemoveRiver();
				}

				if(state.ActiveTool == Tools.Tool.Roads && !state.RoadsIsOn) {
					cell.RemoveRoads();
				}
				
				if(state.ActiveTool == Tools.Tool.Walls) {
					cell.Walled = state.WallsIsOn;
				}

				if (isDrag) {
					HexCell otherCell = cell.GetNeighbor(dragDirection.Opposite());
					if (otherCell) {
						if(state.ActiveTool == Tools.Tool.Rivers && state.RiversIsOn) {
							otherCell.SetOutgoingRiver(dragDirection);
						}

						if(state.ActiveTool == Tools.Tool.Roads && state.RoadsIsOn) {
							otherCell.AddRoad(dragDirection);
						}
					}
				}			
			}
		}

		void EditCells (HexCell center, bool shiftPressed) {
			// todo Units panel will be used in the next commits
			if (state.ActiveTool == Tools.Tool.Units) {
				if (shiftPressed) {
					DestroyUnit();
				} else {
					CreateUnit();
				}

				return;
			}


			int centerX = center.coordinates.X;
			int centerZ = center.coordinates.Z;

			int actualBrushSize = 0;
			if (state.ActiveTool == Tools.Tool.Terrain) {
				actualBrushSize = state.BrushSize;
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

        public void ShowGrid(bool visible) {
            if (visible) {
                terrainMaterial.EnableKeyword(GRID_ENABLE_FLAG);
            } else {
                terrainMaterial.DisableKeyword(GRID_ENABLE_FLAG);
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
				var prefab = Instantiate(hexGrid.unitPrefab);
				prefab.AttachUnitInfo(state.UnitInfo);

				hexGrid.AddUnit(prefab, cell);
			}
		}

		void DestroyUnit () {
			HexCell cell = GetCellUnderCursor();
			if (cell && cell.Unit) {
				hexGrid.RemoveUnit(cell.Unit);
			}
		}

		private int TerrainToIndex(Tools.Terrain terrain) {
			return (int)terrain;
		}
	}
}