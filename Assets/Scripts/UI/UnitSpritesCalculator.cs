using UnityEngine;
using UnityEngine.U2D;
using TrenchWarfare.Domain.Units;

namespace TrenchWarfare.UI {
    public static class UnitSpritesCalculator {
        public static void AddSprites(
            GameObject parent,
            SpriteAtlas atlas,
            UnitInfo unitInfo) {

			var unitSpriteComponent = parent.transform.Find("Unit").GetComponent<SpriteRenderer>();
			unitSpriteComponent.sprite = atlas.GetSprite(UnitSpritesInfo.GetUnitSprite(unitInfo));

			if (unitInfo.MovementPoints == 0f) {
				unitSpriteComponent.color = new Color(0.5f, 0.5f, 0.5f, 1f);
			}

			parent.transform.Find("Health").GetComponent<SpriteRenderer>().sprite =
				atlas.GetSprite(UnitSpritesInfo.GetHealthSprite(unitInfo));

			parent.transform.Find("Banner").GetComponent<SpriteRenderer>().sprite =
				atlas.GetSprite(UnitSpritesInfo.GetBannerSprite(unitInfo));

			parent.transform.Find("Quantity").GetComponent<SpriteRenderer>().sprite =
				atlas.GetSprite(UnitSpritesInfo.GetQuantitySprite(unitInfo));

			var rankSprite = UnitSpritesInfo.GetRankSprite(unitInfo);
			if (rankSprite != null) {
				parent.transform.Find("Rank").GetComponent<SpriteRenderer>().sprite =
					atlas.GetSprite(rankSprite);
			}

			var boost1Sprite = UnitSpritesInfo.GetBoost1Sprite(unitInfo);
			if (boost1Sprite != null) {
				parent.transform.Find("Boost1").GetComponent<SpriteRenderer>().sprite =
					atlas.GetSprite(boost1Sprite);
			}

			var boost2Sprite = UnitSpritesInfo.GetBoost2Sprite(unitInfo);
			if (boost2Sprite != null) {
				parent.transform.Find("Boost2").GetComponent<SpriteRenderer>().sprite =
					atlas.GetSprite(boost2Sprite);
			}

			var boost3Sprite = UnitSpritesInfo.GetBoost3Sprite(unitInfo);
			if (boost3Sprite != null) {
				parent.transform.Find("Boost3").GetComponent<SpriteRenderer>().sprite =
					atlas.GetSprite(boost3Sprite);
			}
        }
    }
}
