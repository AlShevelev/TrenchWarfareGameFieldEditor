using UnityEngine;

public class NewMapMenu : MonoBehaviour {

	public HexGrid hexGrid;

    public HexMapCamera mainCamera;

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
		hexGrid.CreateMap(x, z);
        mainCamera.setStartZoomAndPosition();
		Close();
	}
}