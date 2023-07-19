using UnityEngine;

namespace TrenchWarfare {
	public class NewMapMenu : MonoBehaviour {

		public HexGrid hexGrid;

		public HexMapCamera mainCamera;

		bool generateMaps = true;

		public HexMapGenerator mapGenerator;

		public void ToggleMapGeneration (bool toggle) {
			generateMaps = toggle;
		}

		public void Open () {
			gameObject.SetActive(true);
			mainCamera.Locked = true;
		}

		public void Close () {
			gameObject.SetActive(false);
			mainCamera.Locked = false;
		}

		public void CreateSmallMap () {
			CreateMap(20, 15);
		}

		public void CreateMediumMap () {
			CreateMap(40, 30);
		}

		public void CreateLargeMap () {
			CreateMap(80, 60);
		}

		void CreateMap (int x, int z) {
			if (generateMaps) {
				mapGenerator.GenerateMap(x, z);
			}
			else {
				hexGrid.CreateMap(x, z);
			}
			mainCamera.setStartZoomAndPosition();
			Close();
		}
	}
}