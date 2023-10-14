using UnityEngine;
using TrenchWarfare.Domain.MapObjects;
using TrenchWarfare.Utility;
using UnityEngine.U2D;
using System;
using TrenchWarfare.Domain.Enums;

namespace TrenchWarfare.UI.TerrainModifier {
    public class HexTerrainModifier : MonoBehaviour {
		/// <summary>
		/// Z-size of a map
		/// </summary>
		int cellCountZ = 0;

		ModelRegistry registry;

		TerrainModifierModel model;
		public TerrainModifierModelExternal Model { get => model; }

        public SpriteAtlas atlas;

        public void AddTerrainModifier(
			int cellCountZ,
            TerrainModifierModel tmModel,
            HexCell cell,
            ModelRegistry registry
		) {
            if (model != null) {
                return;
            }

            if (cell.Model.TerrainModifier != null || !tmModel.CanPlace(cell.Model)) {
                return;
            }

            this.cellCountZ = cellCountZ;

            model = tmModel;
            this.registry = registry;
            model.Cell = cell.Model;
            cell.UpdateTerrainModifier(model);

            transform.localPosition = GetPosition(cell);

            registry.Register(model, this);

            SetSprites();
        }

        public void RestoreTerrainModifier(
			int cellCountZ,
            TerrainModifierModel tmModel,
            HexCell cell,
            ModelRegistry registry
		) {
            this.cellCountZ = cellCountZ;

            model = tmModel;
            this.registry = registry;
            model.Cell = cell.Model;

            transform.localPosition = GetPosition(cell);

            registry.Register(model, this);

            SetSprites();
        }

        public void Remove(HexCell cell) {
            model.Cell = null;
            cell.UpdateTerrainModifier(null);

            Destroy(gameObject);
        }

        private void OnDestroy() {
            if (registry != null && model != null) {
                registry.Unregister(model);
            }
        }

		private Vector3 GetPosition(HexCell cell) {
			var result = new Vector3(cell.Position.x, cell.Position.y, cell.Position.z);

			result.y = HexMetrics.fieldObjectStartY + (cellCountZ - 1 - cell.coordinates.Z) * 0.01f;

			return result;
		}

        private void SetSprites() {
            var spriteComponent = gameObject
                .transform.Find("Background")
            .GetComponent<SpriteRenderer>();

            var spriteName = model.Modifier switch {
                TerrainModifierType.AntiAirGun => "Anti air gun",
                TerrainModifierType.BarbedWire => "Barbed wire",
                TerrainModifierType.LandFort => "Land fort",
                TerrainModifierType.LandMine => "Land mine",
                TerrainModifierType.SeaMine => "Sea mine",
                TerrainModifierType.Trench => "Trench",
                _ => throw new NotImplementedException(),
            };

            spriteComponent.sprite = atlas.GetSprite(spriteName);
        }
    }
}
