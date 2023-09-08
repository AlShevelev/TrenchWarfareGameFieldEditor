using UnityEngine;
using UnityEngine.U2D;
using System.IO;
using TrenchWarfare.Domain.Units;
using TrenchWarfare.Domain.Map;
using TrenchWarfare.Utility;

namespace TrenchWarfare.UI {
	public class HexUnit : MonoBehaviour {
		/// <summary>
		/// Z-size of a map
		/// </summary>
		int cellCountZ = 0;

		ModelRegistry registry;

		UnitModel model;
		public UnitModelExternal Model { get => model; }

		public SpriteAtlas atlas;

		void Start() {
			UnitSpritesCalculator.AddSprites(gameObject, atlas, model);
		}

        private void OnDestroy() {
            registry.Unregister(model);
        }

        public void Init(
			int cellCountZ,
            UnitModel model,
            HexCell cell,
            ModelRegistry registry
		) {
			this.cellCountZ = cellCountZ;
			this.model = model;
			this.registry = registry;

			model.Cell = cell.Model;
			cell.AddUnit(model);

			transform.localPosition = getPosition(cell);

			registry.Register(model, this);
		}

		public void ValidateLocation() {
			transform.localPosition = getPosition(GetCell());
		}

		public void Die() {
			GetCell().RemoveUnit();
			Destroy(gameObject);
		}

		public void Save(BinaryWriter writer) {
			//location.coordinates.Save(writer);
		}

		public static void Load(BinaryReader reader, HexGrid grid) {
			HexCoordinates coordinates = HexCoordinates.Load(reader);
			// grid.AddUnit(Instantiate(unitPrefab), grid.GetCell(coordinates));
		}

		private Vector3 getPosition(HexCell cell) {
			var result = new Vector3(cell.Position.x, cell.Position.y, cell.Position.z);

			result.y = HexMetrics.unitStartY + (cellCountZ - 1 - cell.coordinates.Z) * 0.01f;

			//if (unitInfo.Type == UnitType.Infantry || unitInfo.Type == UnitType.Cavalry) {
			//	// Move the unit up a little bit
			//	result.z += HexMetrics.outerRadius / 5f;
			//}


			return result;
		}

		private HexCell GetCell() {
			return registry.Get<HexCell>(model.Cell);
		}

	}
}