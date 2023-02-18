using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HexGrid : MonoBehaviour {
	public int width = 6;
	public int height = 6;

	public HexCell cellPrefab;
	public Text cellLabelPrefab;

	public Color defaultColor = Color.white;

	Canvas gridCanvas;
	HexMesh hexMesh;

    HexCell[] cells;

	public Texture2D noiseSource;

    void Awake () {
		HexMetrics.noiseSource = noiseSource;

		gridCanvas = GetComponentInChildren<Canvas>();
		hexMesh = GetComponentInChildren<HexMesh>();

		cells = new HexCell[height * width];

		for (int z = 0, i = 0; z < height; z++) {
			for (int x = 0; x < width; x++) {
				CreateCell(x, z, i++);
			}
		}
	}

	void OnEnable () {
		HexMetrics.noiseSource = noiseSource;
	}
	
	void Start () {
		hexMesh.Triangulate(cells);
	}
	
	// public void ColorCell (Vector3 position, Color color) {
	// 	// We need to tranform the position form world space to local space
	// 	position = transform.InverseTransformPoint(position);
	// 	HexCoordinates coordinates = HexCoordinates.FromPosition(position);

	// 	int index = coordinates.X + coordinates.Z * width + coordinates.Z / 2;
	// 	HexCell cell = cells[index];
	// 	cell.color = color;
	// 	hexMesh.Triangulate(cells);
	// }	

	public HexCell GetCell (Vector3 position) {
		position = transform.InverseTransformPoint(position);
		HexCoordinates coordinates = HexCoordinates.FromPosition(position);
		int index = coordinates.X + coordinates.Z * width + coordinates.Z / 2;
		return cells[index];
	}	

	public void Refresh () {
		hexMesh.Triangulate(cells);
	}

	void CreateCell (int x, int z, int i) {
		Vector3 position;
		position.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
		position.y = 0f;
		position.z = z * (HexMetrics.outerRadius * 1.5f);

		HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
		cell.transform.SetParent(transform, false);
		cell.transform.localPosition = position;
		var coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
		cell.coordinates = coordinates;
		cell.color = defaultColor;

		// We must connect cells
		if (x > 0) {
			cell.SetNeighbor(HexDirection.W, cells[i - 1]);
		}
		if (z > 0) {
			if ((z & 1) == 0) {
				cell.SetNeighbor(HexDirection.SE, cells[i - width]);
				if (x > 0) {
					cell.SetNeighbor(HexDirection.SW, cells[i - width - 1]);
				}				
			} else {
				cell.SetNeighbor(HexDirection.SW, cells[i - width]);
				if (x < width - 1) {
					cell.SetNeighbor(HexDirection.SE, cells[i - width + 1]);
				}				
			}
		}		

		Text label = Instantiate<Text>(cellLabelPrefab);
		label.rectTransform.SetParent(gridCanvas.transform, false);
		label.rectTransform.anchoredPosition = new Vector2(position.x, position.z);
		label.text = cell.coordinates.ToStringOnSeparateLines();

		cell.uiRect = label.rectTransform;

		cell.Elevation = 0;
	}
}
