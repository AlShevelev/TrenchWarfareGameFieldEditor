using UnityEngine;
using UnityEngine.UI;
using TrenchWarfare.Domain.Enums;

namespace TrenchWarfare {
	public class HexGridChunk : MonoBehaviour {
		HexCell[] cells;

		public HexMesh terrain, rivers, roads, water, waterShore, estuaries;

		public HexFeatureManager features;

		Canvas gridCanvas;

		static Color color1 = new Color(1f, 0f, 0f);
		static Color color2 = new Color(0f, 1f, 0f);
		static Color color3 = new Color(0f, 0f, 1f);

		void Awake () {
			gridCanvas = GetComponentInChildren<Canvas>();

			cells = new HexCell[HexMetrics.chunkSizeX * HexMetrics.chunkSizeZ];

			ShowUI(true);
		}
		
		public void AddCell (int index, HexCell cell) {
			cells[index] = cell;
			cell.chunk = this;
			cell.transform.SetParent(transform, false);
			cell.uiRect.SetParent(gridCanvas.transform, false);
		}

		public void Refresh () {
			enabled = true;
		}

		void LateUpdate () {
			Triangulate();
			enabled = false;
		}

		public void ShowUI (bool visible) {
			gridCanvas.gameObject.SetActive(visible);
		}

		public void Triangulate () {
			terrain.Clear();
			rivers.Clear();
			roads.Clear();
			water.Clear();
			waterShore.Clear();
			estuaries.Clear();
			features.Clear();

			for (int i = 0; i < cells.Length; i++) {
				Triangulate(cells[i]);
			}

			terrain.Apply();
			rivers.Apply();
			roads.Apply();
			water.Apply();
			waterShore.Apply();
			estuaries.Apply();
			features.Apply();
		}

		void Triangulate (HexCell cell) {
			for (HexDirection d = HexDirection.NE; d <= HexDirection.NW; d++) {
				Triangulate(d, cell);
			}

			if (!cell.Model.IsUnderwater && !cell.Model.HasRiver && !cell.Model.HasRoads) {
				features.AddFeature(cell, cell.Position);
			}
		}

		void Triangulate (HexDirection direction, HexCell cell) {
			Vector3 center = cell.Position;
			EdgeVertices e = new EdgeVertices(
				center + HexMetrics.GetFirstSolidCorner(direction),
				center + HexMetrics.GetSecondSolidCorner(direction)
			);

			if (cell.Model.HasRiver) {
				if (cell.Model.HasRiverThroughEdge(direction)) {
					e.v3.y = cell.StreamBedY;
					if (cell.Model.HasRiverBeginOrEnd) {
						TriangulateWithRiverBeginOrEnd(direction, cell, center, e);
					}
					else {
						TriangulateWithRiver(direction, cell, center, e);
					}
				}
				else {
					TriangulateAdjacentToRiver(direction, cell, center, e);
				}
			}
			else {
				TriangulateWithoutRiver(direction, cell, center, e);

				if (!cell.Model.IsUnderwater && !cell.Model.HasRoadThroughEdge(direction)) {
					features.AddFeature(cell, (center + e.v1 + e.v5) * (1f / 3f));
				}
			}

			if (direction <= HexDirection.SE) {
				TriangulateConnection(direction, cell, e);
			}

			if (cell.Model.IsUnderwater) {
				TriangulateWater(direction, cell, center);
			}
		}

		void TriangulateWater (HexDirection direction, HexCell cell, Vector3 center) {
			center.y = cell.WaterSurfaceY;

			HexCell neighbor = cell.GetNeighbor(direction);
			if (neighbor != null && !neighbor.Model.IsUnderwater) {
				TriangulateWaterShore(direction, cell, neighbor, center);
			}
			else {
				TriangulateOpenWater(direction, cell, neighbor, center);
			}		
		}

		void TriangulateOpenWater (HexDirection direction, HexCell cell, HexCell neighbor, Vector3 center) {
			Vector3 c1 = center + HexMetrics.GetFirstWaterCorner(direction);
			Vector3 c2 = center + HexMetrics.GetSecondWaterCorner(direction);

			water.AddTriangle(center, c1, c2);


			if (direction <= HexDirection.SE  && neighbor != null) {
				Vector3 bridge = HexMetrics.GetWaterBridge(direction);
				Vector3 e1 = c1 + bridge;
				Vector3 e2 = c2 + bridge;

				water.AddQuad(c1, c2, e1, e2);

				if (direction <= HexDirection.E) {
					HexCell nextNeighbor = cell.GetNeighbor(direction.Next());
					if (nextNeighbor == null || !nextNeighbor.Model.IsUnderwater) {
						return;
					}
					water.AddTriangle(
						c2, e2, c2 + HexMetrics.GetWaterBridge(direction.Next())
					);
				}			
			}		
		}

		void TriangulateWaterShore (HexDirection direction, HexCell cell, HexCell neighbor, Vector3 center) {
			EdgeVertices e1 = new EdgeVertices(
				center + HexMetrics.GetFirstWaterCorner(direction),
				center + HexMetrics.GetSecondWaterCorner(direction)
			);
			
			water.AddTriangle(center, e1.v1, e1.v2);
			water.AddTriangle(center, e1.v2, e1.v3);
			water.AddTriangle(center, e1.v3, e1.v4);
			water.AddTriangle(center, e1.v4, e1.v5);	

			Vector3 center2 = neighbor.Position;
			center2.y = center.y;
			EdgeVertices e2 = new EdgeVertices(
				center2 + HexMetrics.GetSecondSolidCorner(direction.Opposite()),
				center2 + HexMetrics.GetFirstSolidCorner(direction.Opposite())
			);

			if (cell.Model.HasRiverThroughEdge(direction)) {
				TriangulateEstuary(e1, e2, cell.Model.IncomingRiver == direction);
			}
			else {
				waterShore.AddQuad(e1.v1, e1.v2, e2.v1, e2.v2);
				waterShore.AddQuad(e1.v2, e1.v3, e2.v2, e2.v3);
				waterShore.AddQuad(e1.v3, e1.v4, e2.v3, e2.v4);
				waterShore.AddQuad(e1.v4, e1.v5, e2.v4, e2.v5);		
				waterShore.AddQuadUV(0f, 0f, 0f, 1f);
				waterShore.AddQuadUV(0f, 0f, 0f, 1f);
				waterShore.AddQuadUV(0f, 0f, 0f, 1f);
				waterShore.AddQuadUV(0f, 0f, 0f, 1f);
			}

			HexCell nextNeighbor = cell.GetNeighbor(direction.Next());
			if (nextNeighbor != null) {
				Vector3 v3 = nextNeighbor.Position + (nextNeighbor.Model.IsUnderwater ?
					HexMetrics.GetFirstWaterCorner(direction.Previous()) :
					HexMetrics.GetFirstSolidCorner(direction.Previous()));
				v3.y = center.y;

				waterShore.AddTriangle(e1.v5, e2.v5, v3);

				waterShore.AddTriangleUV(
					new Vector2(0f, 0f),
					new Vector2(0f, 1f),
					new Vector2(0f, nextNeighbor.Model.IsUnderwater ? 0f : 1f)
				);
			}		
		}

		void TriangulateEstuary (EdgeVertices e1, EdgeVertices e2, bool incomingRiver) {
			waterShore.AddTriangle(e2.v1, e1.v2, e1.v1);
			waterShore.AddTriangle(e2.v5, e1.v5, e1.v4);
			waterShore.AddTriangleUV(
				new Vector2(0f, 1f), new Vector2(0f, 0f), new Vector2(0f, 0f)
			);
			waterShore.AddTriangleUV(
				new Vector2(0f, 1f), new Vector2(0f, 0f), new Vector2(0f, 0f)
			);

			estuaries.AddQuad(e2.v1, e1.v2, e2.v2, e1.v3);
			estuaries.AddTriangle(e1.v3, e2.v2, e2.v4);
			estuaries.AddQuad(e1.v3, e1.v4, e2.v4, e2.v5);

			estuaries.AddQuadUV(
				new Vector2(0f, 1f), new Vector2(0f, 0f),
				new Vector2(1f, 1f), new Vector2(0f, 0f)
			);
			estuaries.AddTriangleUV(
				new Vector2(0f, 0f), new Vector2(1f, 1f), new Vector2(1f, 1f)
			);
			estuaries.AddQuadUV(
				new Vector2(0f, 0f), new Vector2(0f, 0f),
				new Vector2(1f, 1f), new Vector2(0f, 1f)
			);

			if (incomingRiver) {
				estuaries.AddQuadUV2(
					new Vector2(1.5f, 1f), new Vector2(0.7f, 1.15f),
					new Vector2(1f, 0.8f), new Vector2(0.5f, 1.1f)
				);
				estuaries.AddTriangleUV2(
					new Vector2(0.5f, 1.1f),
					new Vector2(1f, 0.8f),
					new Vector2(0f, 0.8f)
				);
				estuaries.AddQuadUV2(
					new Vector2(0.5f, 1.1f), new Vector2(0.3f, 1.15f),
					new Vector2(0f, 0.8f), new Vector2(-0.5f, 1f)
				);
			}
			else {
				estuaries.AddQuadUV2(
					new Vector2(-0.5f, -0.2f), new Vector2(0.3f, -0.35f),
					new Vector2(0f, 0f), new Vector2(0.5f, -0.3f)
				);
				estuaries.AddTriangleUV2(
					new Vector2(0.5f, -0.3f),
					new Vector2(0f, 0f),
					new Vector2(1f, 0f)
				);
				estuaries.AddQuadUV2(
					new Vector2(0.5f, -0.3f), new Vector2(0.7f, -0.35f),
					new Vector2(1f, 0f), new Vector2(1.5f, -0.2f)
				);
			}
		}

		void TriangulateWithoutRiver (
			HexDirection direction, HexCell cell, Vector3 center, EdgeVertices e
		) {
			TriangulateEdgeFan(center, e, cell.Model.TerrainType);

			if (cell.Model.HasRoads) {
				Vector2 interpolators = GetRoadInterpolators(direction, cell);
				TriangulateRoad(
					center,
					Vector3.Lerp(center, e.v1, interpolators.x),
					Vector3.Lerp(center, e.v5, interpolators.y),
					e, cell.Model.HasRoadThroughEdge(direction)
				);
			}
		}

		void TriangulateAdjacentToRiver (HexDirection direction, HexCell cell, Vector3 center, EdgeVertices e) {
			if (cell.Model.HasRoads) {
				TriangulateRoadAdjacentToRiver(direction, cell, center, e);
			}

			if (cell.Model.HasRiverThroughEdge(direction.Next())) {
				if (cell.Model.HasRiverThroughEdge(direction.Previous())) {
					center += HexMetrics.GetSolidEdgeMiddle(direction) *
						(HexMetrics.innerToOuter * 0.5f);
				}
				else if (cell.Model.HasRiverThroughEdge(direction.Previous2())) {
					center += HexMetrics.GetFirstSolidCorner(direction) * 0.25f;
				}
			}
			else if (
				cell.Model.HasRiverThroughEdge(direction.Previous()) &&
				cell.Model.HasRiverThroughEdge(direction.Next2())
			) {
				center += HexMetrics.GetSecondSolidCorner(direction) * 0.25f;
			}

			EdgeVertices m = new EdgeVertices(
				Vector3.Lerp(center, e.v1, 0.5f),
				Vector3.Lerp(center, e.v5, 0.5f)
			);
			
			TriangulateEdgeStrip(m, color1, cell.Model.TerrainType, e, color1, cell.Model.TerrainType);
			TriangulateEdgeFan(center, m, cell.Model.TerrainType);

			if (!cell.Model.IsUnderwater && !cell.Model.HasRoadThroughEdge(direction)) {
				features.AddFeature(cell, (center + e.v1 + e.v5) * (1f / 3f));
			}
		}

		void TriangulateWithRiverBeginOrEnd (HexDirection direction, HexCell cell, Vector3 center, EdgeVertices e) {
			EdgeVertices m = new EdgeVertices(
				Vector3.Lerp(center, e.v1, 0.5f),
				Vector3.Lerp(center, e.v5, 0.5f)
			);

			m.v3.y = e.v3.y;

			TriangulateEdgeStrip(m, color1, cell.Model.TerrainType, e, color1, cell.Model.TerrainType);
			TriangulateEdgeFan(center, m, cell.Model.TerrainType);

			if (!cell.Model.IsUnderwater) {
				bool reversed = cell.Model.HasIncomingRiver;
				TriangulateRiverQuad(m.v2, m.v4, e.v2, e.v4, cell.RiverSurfaceY, 0.6f, reversed);

				center.y = m.v2.y = m.v4.y = cell.RiverSurfaceY;
				rivers.AddTriangle(center, m.v2, m.v4);
				if (reversed) {
					rivers.AddTriangleUV(
						new Vector2(0.5f, 0.4f), new Vector2(1f, 0.2f), new Vector2(0f, 0.2f)
					);
				}
				else {
					rivers.AddTriangleUV(
						new Vector2(0.5f, 0.4f), new Vector2(0f, 0.6f), new Vector2(1f, 0.6f)
					);
				}		
			}
		}
		
		void TriangulateWithRiver (HexDirection direction, HexCell cell, Vector3 center, EdgeVertices e) {
			Vector3 centerL, centerR;
			if (cell.Model.HasRiverThroughEdge(direction.Opposite())) {
				centerL = center + HexMetrics.GetFirstSolidCorner(direction.Previous()) * 0.25f;
				centerR = center + HexMetrics.GetSecondSolidCorner(direction.Next()) * 0.25f;
			}
			else if (cell.Model.HasRiverThroughEdge(direction.Next())) {
				centerL = center;
				centerR = Vector3.Lerp(center, e.v5, 2f / 3f);
			}
			else if (cell.Model.HasRiverThroughEdge(direction.Previous())) {
				centerL = Vector3.Lerp(center, e.v1, 2f / 3f);
				centerR = center;
			}		
			else if (cell.Model.HasRiverThroughEdge(direction.Next2())) {
				centerL = center;
				centerR = center + HexMetrics.GetSolidEdgeMiddle(direction.Next()) * 
					(0.5f * HexMetrics.innerToOuter);
			}		
			else {
				centerL = center + HexMetrics.GetSolidEdgeMiddle(direction.Previous()) * 
					(0.5f * HexMetrics.innerToOuter);
				centerR = center;
			}
			center = Vector3.Lerp(centerL, centerR, 0.5f);

			EdgeVertices m = new EdgeVertices(
				Vector3.Lerp(centerL, e.v1, 0.5f),
				Vector3.Lerp(centerR, e.v5, 0.5f),
				1f / 6f
			);

			m.v3.y = center.y = e.v3.y;

			TriangulateEdgeStrip(m, color1, cell.Model.TerrainType, e, color1, cell.Model.TerrainType);

			terrain.AddTriangle(centerL, m.v1, m.v2);
			terrain.AddQuad(centerL, center, m.v2, m.v3);
			terrain.AddQuad(center, centerR, m.v3, m.v4);
			terrain.AddTriangle(centerR, m.v4, m.v5);

			terrain.AddTriangleColor(color1);
			terrain.AddQuadColor(color1);
			terrain.AddQuadColor(color1);
			terrain.AddTriangleColor(color1);

			Vector3 types;
			types.x = types.y = types.z = (int)cell.Model.TerrainType;
			terrain.AddTriangleTerrainTypes(types);
			terrain.AddQuadTerrainTypes(types);
			terrain.AddQuadTerrainTypes(types);
			terrain.AddTriangleTerrainTypes(types);

			if (!cell.Model.IsUnderwater) {
				bool reversed = cell.Model.IncomingRiver == direction;
				TriangulateRiverQuad(centerL, centerR, m.v2, m.v4, cell.RiverSurfaceY, 0.4f, reversed);
				TriangulateRiverQuad(m.v2, m.v4, e.v2, e.v4, cell.RiverSurfaceY, 0.6f, reversed);
			}
		}	

		void TriangulateWaterfallInWater (
			Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4,
			float y1, float y2, float waterY
		) {
			v1.y = v2.y = y1;
			v3.y = v4.y = y2;

			v1 = HexMetrics.Perturb(v1);
			v2 = HexMetrics.Perturb(v2);
			v3 = HexMetrics.Perturb(v3);
			v4 = HexMetrics.Perturb(v4);

			float t = (waterY - y2) / (y1 - y2);
			v3 = Vector3.Lerp(v3, v1, t);
			v4 = Vector3.Lerp(v4, v2, t);

			rivers.AddQuadUnperturbed(v1, v2, v3, v4);
			rivers.AddQuadUV(0f, 1f, 0.8f, 1f);
		}

		void TriangulateConnection (
			HexDirection direction, HexCell cell, EdgeVertices e1
		) {
			HexCell neighbor = cell.GetNeighbor(direction);
			if (neighbor == null) {
				return;
			}
			
			Vector3 bridge = HexMetrics.GetBridge(direction);
			
			bridge.y = neighbor.Position.y - cell.Position.y;
			EdgeVertices e2 = new EdgeVertices(
				e1.v1 + bridge,
				e1.v5 + bridge
			);

			if (cell.Model.HasRiverThroughEdge(direction)) {
				e2.v3.y = neighbor.StreamBedY;

				if (!cell.Model.IsUnderwater) {
					if (!neighbor.Model.IsUnderwater) {
						TriangulateRiverQuad(
							e1.v2, e1.v4, e2.v2, e2.v4,
							cell.RiverSurfaceY, neighbor.RiverSurfaceY, 0.8f,
							cell.Model.HasIncomingRiver && cell.Model.IncomingRiver == direction
						);
					}
					else if (cell.Model.Elevation > neighbor.Model.WaterLevel) {
						TriangulateWaterfallInWater(
							e1.v2, e1.v4, e2.v2, e2.v4,
							cell.RiverSurfaceY, neighbor.RiverSurfaceY,
							neighbor.WaterSurfaceY
						);
					}
				}
				else if (!neighbor.Model.IsUnderwater && neighbor.Model.Elevation > cell.Model.WaterLevel) {
					TriangulateWaterfallInWater(
						e2.v4, e2.v2, e1.v4, e1.v2,
						neighbor.RiverSurfaceY, cell.RiverSurfaceY,
						cell.WaterSurfaceY
					);
				}
			}
			
			if (cell.GetEdgeType(direction) == HexEdgeType.Slope) {
				TriangulateEdgeTerraces(e1, cell, e2, neighbor, cell.Model.HasRoadThroughEdge(direction));
			} else {
				TriangulateEdgeStrip(
					e1, color1, cell.Model.TerrainType,
					e2, color2, neighbor.Model.TerrainType,
					cell.Model.HasRoadThroughEdge(direction));
			}

			features.AddWall(e1, cell, e2, neighbor);

			HexCell nextNeighbor = cell.GetNeighbor(direction.Next());

			if (direction <= HexDirection.E && nextNeighbor != null) {
				Vector3 v5 = e1.v5 + HexMetrics.GetBridge(direction.Next());
				v5.y = nextNeighbor.Position.y;

				if (cell.Model.Elevation <= neighbor.Model.Elevation) {
					if (cell.Model.Elevation <= nextNeighbor.Model.Elevation) {
						TriangulateCorner(e1.v5, cell, e2.v5, neighbor, v5, nextNeighbor);
					} else {
						TriangulateCorner(v5, nextNeighbor, e1.v5, cell, e2.v5, neighbor);
					}
				} else if (neighbor.Model.Elevation <= nextNeighbor.Model.Elevation) {
					TriangulateCorner(e2.v5, neighbor, v5, nextNeighbor, e1.v5, cell);
				}
				else {
					TriangulateCorner(v5, nextNeighbor, e1.v5, cell, e2.v5, neighbor);
				}
			}	
		}

		void TriangulateEdgeTerraces (
			EdgeVertices begin, HexCell beginCell,
			EdgeVertices end, HexCell endCell,
			bool hasRoad
		) {
			EdgeVertices e2 = EdgeVertices.TerraceLerp(begin, end, 1);
			Color c2 = HexMetrics.TerraceLerp(color1, color2, 1);

			var t1 = beginCell.Model.TerrainType;
			var t2 = endCell.Model.TerrainType;

			TriangulateEdgeStrip(begin, color1, t1, e2, c2, t2, hasRoad);

			for (int i = 2; i < HexMetrics.terraceSteps; i++) {
				EdgeVertices e1 = e2;
				Color c1 = c2;
				e2 = EdgeVertices.TerraceLerp(begin, end, i);
				c2 = HexMetrics.TerraceLerp(color1, color2, i);
				TriangulateEdgeStrip(e1, c1, t1, e2, c2, t2, hasRoad);
			}

			TriangulateEdgeStrip(e2, c2, t1, end, color2, t2, hasRoad);
		}
		
		void TriangulateCorner (
			Vector3 bottom, HexCell bottomCell,
			Vector3 left, HexCell leftCell,
			Vector3 right, HexCell rightCell
		) {
			HexEdgeType leftEdgeType = bottomCell.GetEdgeType(leftCell);
			HexEdgeType rightEdgeType = bottomCell.GetEdgeType(rightCell);

			if (leftEdgeType == HexEdgeType.Slope) {
				if (rightEdgeType == HexEdgeType.Slope) {
					TriangulateCornerTerraces(
						bottom, bottomCell, left, leftCell, right, rightCell
					);
				}
				else if (rightEdgeType == HexEdgeType.Flat) {
					TriangulateCornerTerraces(
						left, leftCell, right, rightCell, bottom, bottomCell
					);
				}
				else {
					TriangulateCornerTerracesCliff(
						bottom, bottomCell, left, leftCell, right, rightCell
					);
				}
			}
			else if (rightEdgeType == HexEdgeType.Slope) {
				if (leftEdgeType == HexEdgeType.Flat) {
					TriangulateCornerTerraces(
						right, rightCell, bottom, bottomCell, left, leftCell
					);
				}
				else {
					TriangulateCornerCliffTerraces(
						bottom, bottomCell, left, leftCell, right, rightCell
					);
				}
			}
			else if (leftCell.GetEdgeType(rightCell) == HexEdgeType.Slope) {
				if (leftCell.Model.Elevation < rightCell.Model.Elevation) {
					TriangulateCornerCliffTerraces(
						right, rightCell, bottom, bottomCell, left, leftCell
					);
				}
				else {
					TriangulateCornerTerracesCliff(
						left, leftCell, right, rightCell, bottom, bottomCell
					);
				}
			}
			else {
				terrain.AddTriangle(bottom, left, right);
				terrain.AddTriangleColor(color1, color2, color3);

				Vector3 types;
				types.x = (int)bottomCell.Model.TerrainType;
				types.y = (int)leftCell.Model.TerrainType;
				types.z = (int)rightCell.Model.TerrainType;
				terrain.AddTriangleTerrainTypes(types);
			}

			features.AddWall(bottom, bottomCell, left, leftCell, right, rightCell);
		}	

		void TriangulateCornerTerraces (
			Vector3 begin, HexCell beginCell,
			Vector3 left, HexCell leftCell,
			Vector3 right, HexCell rightCell
		) {
			Vector3 v3 = HexMetrics.TerraceLerp(begin, left, 1);
			Vector3 v4 = HexMetrics.TerraceLerp(begin, right, 1);
			Color c3 = HexMetrics.TerraceLerp(color1, color2, 1);
			Color c4 = HexMetrics.TerraceLerp(color1, color3, 1);

			Vector3 types;
			types.x = (int)beginCell.Model.TerrainType;
			types.y = (int)leftCell.Model.TerrainType;
			types.z = (int)rightCell.Model.TerrainType;

			terrain.AddTriangle(begin, v3, v4);
			terrain.AddTriangleColor(beginCell.Color, c3, c4);	
			terrain.AddTriangleTerrainTypes(types);	

			for (int i = 2; i < HexMetrics.terraceSteps; i++) {
				Vector3 v1 = v3;
				Vector3 v2 = v4;
				Color c1 = c3;
				Color c2 = c4;
				v3 = HexMetrics.TerraceLerp(begin, left, i);
				v4 = HexMetrics.TerraceLerp(begin, right, i);
				c3 = HexMetrics.TerraceLerp(color1, color2, i);
				c4 = HexMetrics.TerraceLerp(color1, color3, i);
				terrain.AddQuad(v1, v2, v3, v4);
				terrain.AddQuadColor(c1, c2, c3, c4);
				terrain.AddQuadTerrainTypes(types);
			}		

			terrain.AddQuad(v3, v4, left, right);
			terrain.AddQuadColor(c3, c4, color2, color3);
			terrain.AddQuadTerrainTypes(types);
		}

		void TriangulateCornerTerracesCliff (
			Vector3 begin, HexCell beginCell,
			Vector3 left, HexCell leftCell,
			Vector3 right, HexCell rightCell
		) {
			float b = 1f / (rightCell.Model.Elevation - beginCell.Model.Elevation);

			if (b < 0) {
				b = -b;
			}

			Vector3 boundary = Vector3.Lerp(HexMetrics.Perturb(begin), HexMetrics.Perturb(right), b);
			Color boundaryColor = Color.Lerp(color1, color3, b);

			Vector3 types;
			types.x = (int)beginCell.Model.TerrainType;
			types.y = (int)leftCell.Model.TerrainType;
			types.z = (int)rightCell.Model.TerrainType;

			TriangulateBoundaryTriangle(
				begin, color1, left, color2, boundary, boundaryColor, types
			);

			if (leftCell.GetEdgeType(rightCell) == HexEdgeType.Slope) {
				TriangulateBoundaryTriangle(
					left, color2, right, color3, boundary, boundaryColor, types
				);
			}
			else {
				terrain.AddTriangleUnperturbed(HexMetrics.Perturb(left), HexMetrics.Perturb(right), boundary);
				terrain.AddTriangleColor(color2, color3, boundaryColor);
				terrain.AddTriangleTerrainTypes(types);
			}		
		}

		void TriangulateCornerCliffTerraces (
			Vector3 begin, HexCell beginCell,
			Vector3 left, HexCell leftCell,
			Vector3 right, HexCell rightCell
		) {
			float b = 1f / (leftCell.Model.Elevation - beginCell.Model.Elevation);

			if (b < 0) {
				b = -b;
			}

			Vector3 boundary = Vector3.Lerp(HexMetrics.Perturb(begin), HexMetrics.Perturb(left), b);
			Color boundaryColor = Color.Lerp(color1, color2, b);

			Vector3 types;
			types.x = (int)beginCell.Model.TerrainType;
			types.y = (int)leftCell.Model.TerrainType;
			types.z = (int)rightCell.Model.TerrainType;

			TriangulateBoundaryTriangle(
				right, color3, begin, color1, boundary, boundaryColor, types
			);

			if (leftCell.GetEdgeType(rightCell) == HexEdgeType.Slope) {
				TriangulateBoundaryTriangle(
					left, color2, right, color3, boundary, boundaryColor, types
				);
			}
			else {
				terrain.AddTriangleUnperturbed(HexMetrics.Perturb(left), HexMetrics.Perturb(right), boundary);
				terrain.AddTriangleColor(color2, color3, boundaryColor);
				terrain.AddTriangleTerrainTypes(types);
			}
		}

		void TriangulateBoundaryTriangle (
			Vector3 begin, Color beginColor,
			Vector3 left, Color leftColor,
			Vector3 boundary, Color boundaryColor, Vector3 types
		) {
			Vector3 v2 = HexMetrics.Perturb(HexMetrics.TerraceLerp(begin, left, 1));
			Color c2 = HexMetrics.TerraceLerp(beginColor, leftColor, 1);

			terrain.AddTriangleUnperturbed(HexMetrics.Perturb(begin), v2, boundary);
			terrain.AddTriangleColor(beginColor, c2, boundaryColor);
			terrain.AddTriangleTerrainTypes(types);

			for (int i = 2; i < HexMetrics.terraceSteps; i++) {
				Vector3 v1 = v2;
				Color c1 = c2;
				
				v2 = HexMetrics.Perturb(HexMetrics.TerraceLerp(begin, left, i));
				c2 = HexMetrics.TerraceLerp(beginColor, leftColor, i);
				
				terrain.AddTriangleUnperturbed(v1, v2, boundary);
				terrain.AddTriangleColor(c1, c2, boundaryColor);
				terrain.AddTriangleTerrainTypes(types);
			}

			terrain.AddTriangleUnperturbed(v2, HexMetrics.Perturb(left), boundary);
			terrain.AddTriangleColor(c2, leftColor, boundaryColor);
			terrain.AddTriangleTerrainTypes(types);
		}

		void TriangulateEdgeFan (Vector3 center, EdgeVertices edge, CellTerrain type) {
			terrain.AddTriangle(center, edge.v1, edge.v2);
			terrain.AddTriangle(center, edge.v2, edge.v3);
			terrain.AddTriangle(center, edge.v3, edge.v4);
			terrain.AddTriangle(center, edge.v4, edge.v5);

			terrain.AddTriangleColor(color1);
			terrain.AddTriangleColor(color1);
			terrain.AddTriangleColor(color1);
			terrain.AddTriangleColor(color1);

			Vector3 types;
			types.x = types.y = types.z = (int)type;
			terrain.AddTriangleTerrainTypes(types);
			terrain.AddTriangleTerrainTypes(types);
			terrain.AddTriangleTerrainTypes(types);
			terrain.AddTriangleTerrainTypes(types);
		}

		void TriangulateEdgeStrip (
			EdgeVertices e1, Color c1, CellTerrain type1,
			EdgeVertices e2, Color c2, CellTerrain type2,
			bool hasRoad = false
		) {
			terrain.AddQuad(e1.v1, e1.v2, e2.v1, e2.v2);
			terrain.AddQuadColor(c1, c2);
			terrain.AddQuad(e1.v2, e1.v3, e2.v2, e2.v3);
			terrain.AddQuadColor(c1, c2);
			terrain.AddQuad(e1.v3, e1.v4, e2.v3, e2.v4);
			terrain.AddQuadColor(c1, c2);
			terrain.AddQuad(e1.v4, e1.v5, e2.v4, e2.v5);
			terrain.AddQuadColor(c1, c2);

			Vector3 types;
			types.x = types.z = (int)type1;
			types.y = (int)type2;
			terrain.AddQuadTerrainTypes(types);
			terrain.AddQuadTerrainTypes(types);
			terrain.AddQuadTerrainTypes(types);
			terrain.AddQuadTerrainTypes(types);

			if (hasRoad) {
				TriangulateRoadSegment(e1.v2, e1.v3, e1.v4, e2.v2, e2.v3, e2.v4);
			}
		}

		void TriangulateRiverQuad (Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4, float y, float v, bool reversed) {
			TriangulateRiverQuad(v1, v2, v3, v4, y, y, v, reversed);
		}

		void TriangulateRiverQuad (Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4, float y1, float y2, float v, bool reversed) {
			v1.y = v2.y = y1;
			v3.y = v4.y = y2;
			rivers.AddQuad(v1, v2, v3, v4);
			if (reversed) {
				rivers.AddQuadUV(1f, 0f, 0.8f - v, 0.6f - v);
			}
			else {
				rivers.AddQuadUV(0f, 1f, v, v + 0.2f);
			}
		}

		void TriangulateRoadSegment (
			Vector3 v1, Vector3 v2, Vector3 v3,
			Vector3 v4, Vector3 v5, Vector3 v6
		) {
			roads.AddQuad(v1, v2, v4, v5);
			roads.AddQuad(v2, v3, v5, v6);
			roads.AddQuadUV(0f, 1f, 0f, 0f);
			roads.AddQuadUV(1f, 0f, 0f, 0f);
		}

		void TriangulateRoad (
			Vector3 center, Vector3 mL, Vector3 mR,
			EdgeVertices e, bool hasRoadThroughCellEdge
		) {
			if (hasRoadThroughCellEdge) {
				Vector3 mC = Vector3.Lerp(mL, mR, 0.5f);
				TriangulateRoadSegment(mL, mC, mR, e.v2, e.v3, e.v4);
				roads.AddTriangle(center, mL, mC);
				roads.AddTriangle(center, mC, mR);
				roads.AddTriangleUV(
					new Vector2(1f, 0f), new Vector2(0f, 0f), new Vector2(1f, 0f)
				);
				roads.AddTriangleUV(
					new Vector2(1f, 0f), new Vector2(1f, 0f), new Vector2(0f, 0f)
				);
			}
			else {
				TriangulateRoadEdge(center, mL, mR);
			}
		}

		void TriangulateRoadEdge (Vector3 center, Vector3 mL, Vector3 mR) {
			roads.AddTriangle(center, mL, mR);
			roads.AddTriangleUV(
				new Vector2(1f, 0f), new Vector2(0f, 0f), new Vector2(0f, 0f)
			);
		}
		
		Vector2 GetRoadInterpolators (HexDirection direction, HexCell cell) {
			Vector2 interpolators;
			if (cell.Model.HasRoadThroughEdge(direction)) {
				interpolators.x = interpolators.y = 0.5f;
			}
			else {
				interpolators.x = cell.Model.HasRoadThroughEdge(direction.Previous()) ? 0.5f : 0.25f;
				interpolators.y = cell.Model.HasRoadThroughEdge(direction.Next()) ? 0.5f : 0.25f;
			}
			return interpolators;
		}

		void TriangulateRoadAdjacentToRiver (
			HexDirection direction, HexCell cell, Vector3 center, EdgeVertices e
		) {
			bool hasRoadThroughEdge = cell.Model.HasRoadThroughEdge(direction);

			bool previousHasRiver = cell.Model.HasRiverThroughEdge(direction.Previous());
			bool nextHasRiver = cell.Model.HasRiverThroughEdge(direction.Next());

			Vector2 interpolators = GetRoadInterpolators(direction, cell);
			Vector3 roadCenter = center;

			if (cell.Model.HasRiverBeginOrEnd) {
				roadCenter += HexMetrics.GetSolidEdgeMiddle(
					cell.Model.RiverBeginOrEndDirection.Opposite()
				) * (1f / 3f);
			}
			else if (cell.Model.IncomingRiver == cell.Model.OutgoingRiver.Opposite()) {
				Vector3 corner;
				if (previousHasRiver) {
					if (!hasRoadThroughEdge && !cell.Model.HasRoadThroughEdge(direction.Next())) {
						return;
					}
					corner = HexMetrics.GetSecondSolidCorner(direction);
				}
				else {
					if (!hasRoadThroughEdge && !cell.Model.HasRoadThroughEdge(direction.Previous())) {
						return;
					}				
					corner = HexMetrics.GetFirstSolidCorner(direction);
				}
				roadCenter += corner * 0.5f;

				if (cell.Model.IncomingRiver == direction.Next() && (
					cell.Model.HasRoadThroughEdge(direction.Next2()) ||
					cell.Model.HasRoadThroughEdge(direction.Opposite())
				)) {
					features.AddBridge(roadCenter, center - corner * 0.5f);
				}
				
				center += corner * 0.25f;			
			}
			else if (cell.Model.IncomingRiver == cell.Model.OutgoingRiver.Previous()) {
				roadCenter -= HexMetrics.GetSecondCorner(cell.Model.IncomingRiver) * 0.2f;
			}
			else if (cell.Model.IncomingRiver == cell.Model.OutgoingRiver.Next()) {
				roadCenter -= HexMetrics.GetFirstCorner(cell.Model.IncomingRiver) * 0.2f;
			}		
			else if (previousHasRiver && nextHasRiver) {
				if (!hasRoadThroughEdge) {
					return;
				}			
				Vector3 offset = HexMetrics.GetSolidEdgeMiddle(direction) *	HexMetrics.innerToOuter;
				roadCenter += offset * 0.7f;
				center += offset * 0.5f;			
			}		
			else {
				HexDirection middle;
				if (previousHasRiver) {
					middle = direction.Next();
				}
				else if (nextHasRiver) {
					middle = direction.Previous();
				}
				else {
					middle = direction;
				}
				if (
					!cell.Model.HasRoadThroughEdge(middle) &&
					!cell.Model.HasRoadThroughEdge(middle.Previous()) &&
					!cell.Model.HasRoadThroughEdge(middle.Next())
				) {
					return;
				}			
				Vector3 offset = HexMetrics.GetSolidEdgeMiddle(middle);
				roadCenter += offset * 0.25f;
				if (direction == middle && cell.Model.HasRoadThroughEdge(direction.Opposite())) {
					features.AddBridge(
						roadCenter,
						center - offset * (HexMetrics.innerToOuter * 0.7f)
					);
				}
			}

			Vector3 mL = Vector3.Lerp(roadCenter, e.v1, interpolators.x);
			Vector3 mR = Vector3.Lerp(roadCenter, e.v5, interpolators.y);
			TriangulateRoad(roadCenter, mL, mR, e, hasRoadThroughEdge);

			if (previousHasRiver) {
				TriangulateRoadEdge(roadCenter, center, mL);
			}
			if (previousHasRiver) {
				TriangulateRoadEdge(roadCenter, mR, center);
			}
		}
	}
}