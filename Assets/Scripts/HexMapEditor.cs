using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Tools = TrenchWarfare.ToolPanels;
using TrenchWarfare.ToolPanels.State;
using SimpleFileBrowser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TrenchWarfare.Domain.Enums;
using TrenchWarfare.Domain.Map;
using TrenchWarfare.ToolPanels;
using TrenchWarfare.SaveLoad;
using TrenchWarfare.Domain.Game;


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

		private const String GRID_ENABLE_FLAG = "GRID_ON";

        void Awake() {
            terrainMaterial.DisableKeyword(GRID_ENABLE_FLAG);
        }

        void Start() {
			UpdateLevelsVisibility();
		}

        void Update () {
			if (!EventSystem.current.IsPointerOverGameObject()) {
				if (Input.GetMouseButtonUp(0)) {
					HandleTap(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift));
				}

				if (Input.GetMouseButton(0)) {
					HandleDrag();
					return;
				}
			}
			previousCell = null;
		}

		void HandleDrag() {
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

		void HandleTap(bool shiftPressed) {
			HexCell cell = GetCellUnderCursor();

			if (state.ActiveTool == Tools.Tool.Units) {
				if (shiftPressed) {
					DestroyUnit(cell);
				} else {
					CreateUnit(cell);
				}

				return;
			}

			if (state.ActiveTool == Tools.Tool.ProductionCenters) {
				if (shiftPressed) {
					DestroyProductionCenter(cell);
				} else {
					CreateProductionCenter(cell);
				}
			}

			if (state.ActiveTool == Tools.Tool.TerrainModifiers) {
				if (shiftPressed) {
					DestroyTerrainModifier(cell);
				} else {
					CreateTerrainModifier(cell);
				}
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
					if (state.TerrainSelected == CellTerrain.Water) {
                        cell.UpdateWaterLevel(state.WaterLevel);
                    } else {
                        cell.UpdateTerrainType(state.TerrainSelected);
                        cell.UpdateElevation(state.TerrainElevation);
                    }
                }

				if(state.ActiveTool == Tools.Tool.Rivers && !state.RiversIsOn) {
					cell.RemoveRiver();
				}

				if(state.ActiveTool == Tools.Tool.Roads && !state.RoadsIsOn) {
					cell.RemoveRoads();
				}
				
				if(state.ActiveTool == Tools.Tool.Domains) {
					cell.UpdateOwner(state.DomainNation);
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

		void EditCells (HexCell center) {
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
			FileBrowser.SetFilters(true, ".map");
			FileBrowser.ShowSaveDialog(
                onSuccess: (path) => {
					using(BinaryWriter writer = new BinaryWriter(File.Open(path[0], FileMode.Create))) {
						Saver.Save(writer, hexGrid.Model, new GameState(0, new Dictionary<Nation, NationGameState>()));
					}
				},
                onCancel: null,
                FileBrowser.PickMode.Files,
                allowMultiSelection: false,
                initialPath: null,
                initialFilename: "map1.map",
                title: "Save map",
                saveButtonText: "Save"
			);
		}

		public void Load() {
			FileBrowser.SetFilters(true, ".map");
			FileBrowser.ShowLoadDialog(
				onSuccess: (path) => {
					using (BinaryReader reader = new BinaryReader(File.OpenRead(path[0]))) {
						var loaded = Loader.Load(reader);
						hexGrid.Restore(loaded.Item1);
						mainCamera.setStartZoomAndPosition();
					}

					var conditions = hexGrid.Model.Conditions;
					state.Nations = conditions.Conditions.nations.Select(i => i.code);

					transform.Find("Menu Panel")
						.GetComponent<EditorMenuPanel>()
						.OnNationsSet(state.Nations);
				},
				onCancel: null,
				FileBrowser.PickMode.Files,
				allowMultiSelection: false,
				initialPath: null,
				initialFilename: null,
				title: "Load map",
				loadButtonText: "Load"
			);
		}

		public void ExportMetadata() {
			FileBrowser.SetFilters(true, ".map.json");
			FileBrowser.ShowSaveDialog(
                onSuccess: (path) => {
					string rawData = hexGrid.Model.Conditions.ExportToJson();
					File.WriteAllText(path[0], rawData);
				},
                onCancel: null,
                FileBrowser.PickMode.Files,
                allowMultiSelection: false,
                initialPath: null,
                initialFilename: "map1.map.json",
                title: "Export map metadata",
                saveButtonText: "Export"
			);
		}

		public void ImportMetadata() {
			FileBrowser.SetFilters(true, ".map.json");
			FileBrowser.ShowLoadDialog(
                onSuccess: (path) => {
					string rawData = File.ReadAllText(path[0]);

					var conditions = hexGrid.Model.Conditions;
					conditions.ImportFromJson(rawData);
			
					state.Nations = conditions.Conditions.nations.Select(i => i.code);

					transform.Find("Menu Panel")
						.GetComponent<EditorMenuPanel>()
						.OnNationsSet(state.Nations);
				},
                onCancel: null,
                FileBrowser.PickMode.Files,
                allowMultiSelection: false,
                initialPath: null,
                initialFilename: null,
                title: "Import map metadata",
                loadButtonText: "Import"
			);
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

		void CreateUnit (HexCell cell) {
			hexGrid.AddUnit(cell, state.UnitInfo.Copy(i => i));
		}

		void DestroyUnit (HexCell cell) {
			hexGrid.RemoveUnit(cell);
		}

		void CreateProductionCenter (HexCell cell) {
			hexGrid.AddProductionCenter(cell, state.ProductionCenter.Copy(i => i));
		}

		void DestroyProductionCenter (HexCell cell) {
			hexGrid.RemoveProductionCenter(cell);
		}

		void CreateTerrainModifier(HexCell cell) {
			hexGrid.AddTerrainModifier(cell, state.TerrainModifier.Copy(i => i));
		}

		void DestroyTerrainModifier(HexCell cell) {
			hexGrid.RemoveTerrainModifier(cell);
		}
	}
}