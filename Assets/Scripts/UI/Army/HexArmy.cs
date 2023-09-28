using UnityEngine;
using UnityEngine.U2D;
using System.IO;
using TrenchWarfare.Domain.Units;
using TrenchWarfare.Domain.Map;
using TrenchWarfare.Utility;

namespace TrenchWarfare.UI.Army {
	public class HexArmy : MonoBehaviour {
		/// <summary>
		/// Z-size of a map
		/// </summary>
		int cellCountZ = 0;

		ModelRegistry registry;

		ArmyModel model;
		public ArmyModelExternal Model { get => model; }

		public SpriteAtlas atlas;

        private void OnDestroy() {
            registry.Unregister(model);
        }

        public void AddUnit(
			int cellCountZ,
            UnitModel unitModel,
            HexCell cell,
            ModelRegistry registry
		) {
			if (model != null && !model.CanAdd) {
				return;
			}

			if (!unitModel.CanPlace(cell.Model)) {
				return;
			}

			this.cellCountZ = cellCountZ;

			if (model == null) {
				model = new ArmyModel(unitModel);
				this.registry = registry;

				model.Cell = cell.Model;
				cell.UpdateArmy(model);

				transform.localPosition = GetPosition(cell);

				registry.Register(model, this);

			} else {
				model.AddUnit(unitModel);
			}

			RefreshSprites();
		}

		public void ValidateLocation() {
			transform.localPosition = GetPosition(GetCell());
		}

		public void RemoveLastUnit() {
			if (model == null) {
				return;
			}

			var isNotEmpty = model.RemoveLastUnit();

			if (isNotEmpty) {
				RefreshSprites();
			} else {
				GetCell().UpdateArmy(null);
				Destroy(gameObject);
			}
		}

		public void Save(BinaryWriter writer) {
			//location.coordinates.Save(writer);
		}

		public static void Load(BinaryReader reader, HexGrid grid) {
			HexCoordinates coordinates = HexCoordinates.Load(reader);
			// grid.AddUnit(Instantiate(unitPrefab), grid.GetCell(coordinates));
		}

		private void RefreshSprites() {
			UnitSpritesCalculator.AddSprites(gameObject, atlas, model);
		}

		private Vector3 GetPosition(HexCell cell) {
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