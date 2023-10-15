using TMPro;
using TrenchWarfare.Domain.MapObjects;
using TrenchWarfare.UI.Army;
using TrenchWarfare.Utility;
using UnityEngine;
using UnityEngine.U2D;

namespace TrenchWarfare.UI.ProductionCenter {
    public class HexProductionCenter : MonoBehaviour {
		/// <summary>
		/// Z-size of a map
		/// </summary>
		int cellCountZ = 0;

		ModelRegistry registry;

		ProductionCenterModel model;
		public ProductionCenterModelExternal Model { get => model; }

        public SpriteAtlas atlas;

        private void OnDestroy() {
            if (registry != null && model != null) {
                registry.Unregister(model);
            }
        }

        public void AddProductionCenter(
			int cellCountZ,
            ProductionCenterModel pcModel,
            HexCell cell,
            ModelRegistry registry
		) {
            if (model != null) {
                return;
            }

            if (cell.Model.ProductionCenter != null || !pcModel.CanPlace(cell.Model)) {
                return;
            }

            this.cellCountZ = cellCountZ;

            model = pcModel;
            this.registry = registry;
            model.Cell = cell.Model;
            cell.UpdateProductionCenter(model);

            transform.localPosition = GetPosition(cell);

            registry.Register(model, this);

            SetSprites();
        }

        public void RestoreProductionCenter(
			int cellCountZ,
            ProductionCenterModel pcModel,
            HexCell cell,
            ModelRegistry registry
		) {
            this.cellCountZ = cellCountZ;

            model = pcModel;
            this.registry = registry;
            model.Cell = cell.Model;

            transform.localPosition = GetPosition(cell);

            registry.Register(model, this);

            SetSprites();
        }

        public void Remove(HexCell cell) {
            model.Cell = null;
            cell.UpdateProductionCenter(null);

            Destroy(gameObject);
        }

		private Vector3 GetPosition(HexCell cell) {
			var result = new Vector3(cell.Position.x, cell.Position.y, cell.Position.z);

			result.y = HexMetrics.fieldObjectStartY + (cellCountZ - 1 - cell.coordinates.Z) * 0.01f;

			return result;
		}

        private void SetSprites() {
            SetSprite(
                gameObjectName: "Background",
                spriteName: ProductionCenterSpriteInfo.GetBackgroundSprite(model)
            );

            SetSprite(
                gameObjectName: "Center",
                spriteName: ProductionCenterSpriteInfo.GetCenterSprite(model)
            );

            SetSprite(
                gameObjectName: "Level",
                spriteName: ProductionCenterSpriteInfo.GetLevelSprite(model)
            );

            SetSprite(
                gameObjectName: "Banner",
                spriteName: UnitSpritesInfo.GetBannerSprite(model.Nation)
            );

            var textGo = gameObject.transform.Find("Name");
            if (model.Name == null || model.Name == "") {
                textGo.gameObject.SetActive(false);
            } else {
                textGo.gameObject.SetActive(true);

                var text = textGo.GetComponent<TextMeshPro>();
                text.text = model.Name;
                text.rectTransform.anchoredPosition = new Vector3(0f, -HexMetrics.outerRadius/6.5f, 0f);
            }
        }

        private void SetSprite(string gameObjectName, string spriteName) {
            var spriteComponent = gameObject
                .transform.Find(gameObjectName)
                .GetComponent<SpriteRenderer>();

            spriteComponent.sprite = atlas.GetSprite(spriteName);
        }
    }
}
