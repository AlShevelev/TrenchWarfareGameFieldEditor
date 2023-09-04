using System.IO;

namespace TrenchWarfare.Domain.Map {
	public struct HexCoordinates {
		private int x, z;

        public int X => x;

        public int Z => z;

        public int Y => -X - Z;

        public HexCoordinates (int x, int z) {
			this.x = x;
			this.z = z;
		}

		public static HexCoordinates FromOffsetCoordinates (int x, int z) {
			return new HexCoordinates(x - z / 2, z);
		}

		public override string ToString () {
			return "(" + X.ToString() + ", " + Y.ToString() + ", " + Z.ToString() + ")";
		}

		public string ToStringOnSeparateLines () {
			return "X: "+X.ToString() + "\nY:" + Y.ToString() +  "\nZ: " + Z.ToString();
		}

		public void Save (BinaryWriter writer) {
			writer.Write(x);
			writer.Write(z);
		}

		public static HexCoordinates Load (BinaryReader reader) {
			HexCoordinates c;
			c.x = reader.ReadInt32();
			c.z = reader.ReadInt32();
			return c;
		}

		public int DistanceTo (HexCoordinates other) {
			return
				((x < other.x ? other.x - x : x - other.x) +
				(Y < other.Y ? other.Y - Y : Y - other.Y) +
				(z < other.z ? other.z - z : z - other.z)) / 2;
		}
	}
}