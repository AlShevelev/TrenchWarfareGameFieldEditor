using UnityEngine;

namespace TrenchWarfare {
	public class HexFeatureManager : MonoBehaviour {

		public HexFeatureCollection[] urbanCollections, farmCollections;

		Transform container;

		public HexMesh walls;

		public Transform bridge;

		public void Clear () {
			if (container) {
				Destroy(container.gameObject);
			}
			container = new GameObject("Features Container").transform;
			container.SetParent(transform, false);

			walls.Clear();
		}

		public void Apply () {
			walls.Apply();
		}

		public void AddFeature (HexCell cell, Vector3 position) {
			HexHash hash = HexMetrics.SampleHashGrid(position);
			
			Transform prefab = PickPrefab(urbanCollections, cell.UrbanLevel, hash.a, hash.d);
			Transform otherPrefab = PickPrefab(farmCollections, cell.FarmLevel, hash.b, hash.d);
			
			float usedHash = hash.a;
			
			if (prefab) {
				if (otherPrefab && hash.b < hash.a) {
					prefab = otherPrefab;
					usedHash = hash.b;
				}
			}
			else if (otherPrefab) {
				prefab = otherPrefab;
				usedHash = hash.b;
			} else {
				return;
			}

			Transform instance = Instantiate(prefab);

			position.y += instance.localScale.y * 0.5f;
			instance.localPosition = HexMetrics.Perturb(position);
			instance.localRotation = Quaternion.Euler(0f, 360f * hash.e, 0f);
			instance.SetParent(container, false);
		}

		Transform PickPrefab (
			HexFeatureCollection[] collection,
			int level, float hash, float choice
		) {
			if (level > 0) {
				float[] thresholds = HexMetrics.GetFeatureThresholds(level - 1);
				for (int i = 0; i < thresholds.Length; i++) {
					if (hash < thresholds[i]) {
						return collection[i].Pick(choice);
					}
				}
			}
			return null;
		}

		public void AddWall (
			EdgeVertices near, HexCell nearCell,
			EdgeVertices far, HexCell farCell
		) {
			if (nearCell.Walled != farCell.Walled) {
				AddWallSegment(near.v1, far.v1, near.v2, far.v2);
				AddWallSegment(near.v2, far.v2, near.v3, far.v3);
				AddWallSegment(near.v3, far.v3, near.v4, far.v4);
				AddWallSegment(near.v4, far.v4, near.v5, far.v5);
			}
		}

		public void AddWall (
			Vector3 c1, HexCell cell1,
			Vector3 c2, HexCell cell2,
			Vector3 c3, HexCell cell3
		) {
			if (cell1.Walled) {
				if (cell2.Walled) {
					if (!cell3.Walled) {
						AddWallSegment(c3, cell3, c1, cell1, c2, cell2);
					}
				}
				else if (cell3.Walled) {
					AddWallSegment(c2, cell2, c3, cell3, c1, cell1);
				}
				else {
					AddWallSegment(c1, cell1, c2, cell2, c3, cell3);
				}
			}
			else if (cell2.Walled) {
				if (cell3.Walled) {
					AddWallSegment(c1, cell1, c2, cell2, c3, cell3);
				}
				else {
					AddWallSegment(c2, cell2, c3, cell3, c1, cell1);
				}
			}
			else if (cell3.Walled) {
				AddWallSegment(c3, cell3, c1, cell1, c2, cell2);
			}
		}

		public void AddBridge (Vector3 roadCenter1, Vector3 roadCenter2) {
			roadCenter1 = HexMetrics.Perturb(roadCenter1);
			roadCenter2 = HexMetrics.Perturb(roadCenter2);
			Transform instance = Instantiate(bridge);
			instance.localPosition = (roadCenter1 + roadCenter2) * 0.5f;
			instance.forward = roadCenter2 - roadCenter1;

			float length = Vector3.Distance(roadCenter1, roadCenter2);

			Vector3 currentScale = instance.localScale;
			instance.localScale = new Vector3(
				currentScale.x, 
				currentScale.y, 
				currentScale.z * (length / HexMetrics.bridgeDesignLength)
			);

			instance.SetParent(container, false);
		}

		void AddWallSegment (Vector3 nearLeft, Vector3 farLeft, Vector3 nearRight, Vector3 farRight) {
			nearLeft = HexMetrics.Perturb(nearLeft);
			farLeft = HexMetrics.Perturb(farLeft);
			nearRight = HexMetrics.Perturb(nearRight);
			farRight = HexMetrics.Perturb(farRight);

			Vector3 left = HexMetrics.WallLerp(nearLeft, farLeft);
			Vector3 right = HexMetrics.WallLerp(nearRight, farRight);

			Vector3 leftThicknessOffset = HexMetrics.WallThicknessOffset(nearLeft, farLeft);
			Vector3 rightThicknessOffset = HexMetrics.WallThicknessOffset(nearRight, farRight);

			float leftTop = left.y + HexMetrics.wallHeight;
			float rightTop = right.y + HexMetrics.wallHeight;

			Vector3 v1, v2, v3, v4;
			v1 = v3 = left - leftThicknessOffset;
			v2 = v4 = right - rightThicknessOffset;
			v3.y = leftTop;
			v4.y = rightTop;
			walls.AddQuadUnperturbed(v1, v2, v3, v4);

			Vector3 t1 = v3, t2 = v4;

			v1 = v3 = left + leftThicknessOffset;
			v2 = v4 = right + rightThicknessOffset;
			v3.y = leftTop;
			v4.y = rightTop;
			walls.AddQuadUnperturbed(v2, v1, v4, v3);

			walls.AddQuadUnperturbed(t1, t2, v3, v4);
		}

		void AddWallSegment (
			Vector3 pivot, HexCell pivotCell,
			Vector3 left, HexCell leftCell,
			Vector3 right, HexCell rightCell
		) {
			AddWallSegment(pivot, left, pivot, right);
		}
	}
}