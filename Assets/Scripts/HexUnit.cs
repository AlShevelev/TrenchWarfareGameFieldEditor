using UnityEngine;
using System.IO;

public class HexUnit : MonoBehaviour {
	HexCell location;

    public HexCell Location {
		get {
			return location;
		}
		set {
			location = value;
			value.Unit = this;
			transform.localPosition = value.Position;
		}
	}

	public static HexUnit unitPrefab;

	public void ValidateLocation () {
		transform.localPosition = location.Position;
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
}