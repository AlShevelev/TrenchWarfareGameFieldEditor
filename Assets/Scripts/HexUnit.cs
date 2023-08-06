using UnityEngine;
using System.IO;

namespace TrenchWarfare {
	public class HexUnit : MonoBehaviour {
		HexCell location;

		/// <summary>
		/// Z-size of a map
		/// </summary>
		private int cellCountZ = 0;

        public HexCell Location {
			get {
				return location;
			}
			set {
				location = value;
				value.Unit = this;

                transform.localPosition = getPosition(value);
			}
		}

		public static HexUnit unitPrefab;

		public void ValidateLocation () {
			transform.localPosition = getPosition(location);
		}

		public void Die () {
			location.Unit = null;
			Destroy(gameObject);
		}

		public void Save (BinaryWriter writer) {
			location.coordinates.Save(writer);
		}

		public static void Load (BinaryReader reader, HexGrid grid) {
			HexCoordinates coordinates = HexCoordinates.Load(reader);
			grid.AddUnit(Instantiate(unitPrefab), grid.GetCell(coordinates));
		}

		public void Init(int cellCountZ) {
			this.cellCountZ = cellCountZ;

        }

		private Vector3 getPosition(HexCell cell) {
			var result = new Vector3(cell.Position.x, cell.Position.y, cell.Position.z);

			result.y = HexMetrics.unitStarY + (cellCountZ - 1 - cell.coordinates.Z) * 0.01f;

			// Move the unit up a little bit
			result.z += HexMetrics.outerRadius / 3f;

			return result;
        }
	}
}