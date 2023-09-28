using UnityEngine;
using UnityEngine.U2D;
using TrenchWarfare.Domain.Units;

namespace TrenchWarfare.UI.Army {
    public static class UnitSpritesCalculator {
        public static void AddSprites(
            GameObject parent,
            SpriteAtlas atlas,
            ArmyModel army) {

			var unit = army.DisplayUnit;

			var unitSpriteComponent = parent.transform.Find("Unit").GetComponent<SpriteRenderer>();
			unitSpriteComponent.sprite = atlas.GetSprite(UnitSpritesInfo.GetUnitSprite(unit));

			if (unit.MovementPoints == 0f) {
				unitSpriteComponent.color = new Color(0.5f, 0.5f, 0.5f, 1f);
			}

			parent.transform.Find("Health").GetComponent<SpriteRenderer>().sprite =
				atlas.GetSprite(UnitSpritesInfo.GetHealthSprite(unit));

			parent.transform.Find("Banner").GetComponent<SpriteRenderer>().sprite =
				atlas.GetSprite(UnitSpritesInfo.GetBannerSprite(unit));

			parent.transform.Find("Quantity").GetComponent<SpriteRenderer>().sprite =
				atlas.GetSprite(UnitSpritesInfo.GetQuantitySprite(army));

			var rankSprite = UnitSpritesInfo.GetRankSprite(unit);
			if (rankSprite != null) {
				parent.transform.Find("Rank").GetComponent<SpriteRenderer>().sprite =
					atlas.GetSprite(rankSprite);
			}

			var boost1Sprite = UnitSpritesInfo.GetBoost1Sprite(unit);
			if (boost1Sprite != null) {
				parent.transform.Find("Boost1").GetComponent<SpriteRenderer>().sprite =
					atlas.GetSprite(boost1Sprite);
			}

			var boost2Sprite = UnitSpritesInfo.GetBoost2Sprite(unit);
			if (boost2Sprite != null) {
				parent.transform.Find("Boost2").GetComponent<SpriteRenderer>().sprite =
					atlas.GetSprite(boost2Sprite);
			}

			var boost3Sprite = UnitSpritesInfo.GetBoost3Sprite(unit);
			if (boost3Sprite != null) {
				parent.transform.Find("Boost3").GetComponent<SpriteRenderer>().sprite =
					atlas.GetSprite(boost3Sprite);
			}
        }
    }
}
