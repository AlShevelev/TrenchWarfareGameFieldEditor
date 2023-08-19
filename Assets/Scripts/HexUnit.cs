using UnityEngine;
using UnityEngine.U2D;
using System.IO;
using TrenchWarfare.Domain.Enums;
using TrenchWarfare.Domain.Units;
using System;

namespace TrenchWarfare {
	public class HexUnit : MonoBehaviour {
		HexCell location;

		/// <summary>
		/// Z-size of a map
		/// </summary>
		private int cellCountZ = 0;

		private UnitInfo unitInfo;

		public HexCell Location {
			get {
				return location;
			}
			set {
				location = value;
				value.Unit = this;

				transform.localPosition = getPosition(value);
			}
		}

		public SpriteAtlas atlas;

		void Start() {
			var sprite = atlas.GetSprite(GetSpriteIcon());
			GetComponent<SpriteRenderer>().sprite = sprite;
		}

		public void AttachUnitInfo(UnitInfo unitInfo) {
			this.unitInfo = unitInfo;
		}

		public void ValidateLocation() {
			transform.localPosition = getPosition(location);
		}

		public void Die() {
			location.Unit = null;
			Destroy(gameObject);
		}

		public void Save(BinaryWriter writer) {
			location.coordinates.Save(writer);
		}

		public static void Load(BinaryReader reader, HexGrid grid) {
			HexCoordinates coordinates = HexCoordinates.Load(reader);
			// grid.AddUnit(Instantiate(unitPrefab), grid.GetCell(coordinates));
		}

		public void Init(int cellCountZ) {
			this.cellCountZ = cellCountZ;

		}

		private Vector3 getPosition(HexCell cell) {
			var result = new Vector3(cell.Position.x, cell.Position.y, cell.Position.z);

			result.y = HexMetrics.unitStarY + (cellCountZ - 1 - cell.coordinates.Z) * 0.01f;

			if (unitInfo.Type == UnitType.Infantry || unitInfo.Type == UnitType.Cavalry) {
				// Move the unit up a little bit
				result.z += HexMetrics.outerRadius / 3f;
			}


			return result;
		}

		private string GetSpriteIcon() {
			switch (unitInfo.Type) {
				case UnitType.ArmoredCar: return "Armored car";
				case UnitType.Artillery: return "Artillery";
				case UnitType.Infantry: {
					switch (unitInfo.Nation) {
						case Nation.AustriaHungary: return "Infantry Austro-Hungary";
						case Nation.Belgium: return "Infantry Belgia";
						case Nation.Bulgaria: return "Infantry Bulgaria";
						case Nation.China: return "Infantry China";
						case Nation.France: return "Infantry France";
						case Nation.Germany: return "Infantry Germany";
						case Nation.GreatBritain: return "Infantry UK";
						case Nation.Greece: return "Infantry Greece";
						case Nation.Italy: return "Infantry Italy";
						case Nation.Japan: return "Infantry Japan";
						case Nation.Korea: return "Infantry Korea";
						case Nation.Mexico: return "Infantry Mexico";
						case Nation.Mongolia: return "Infantry Mongolia";
						case Nation.Montenegro: return "Infantry Montenegro";
						case Nation.Romania: return "Infantry Romania";
						case Nation.Russia: return "Infantry Russia";
						case Nation.Serbia: return "Infantry Serbia";
						case Nation.Turkey: return "Infantry Turkey";
						case Nation.USA: return "Infantry US";
						case Nation.USNorth: return "Infantry US North";
						case Nation.USSouth: return "Infantry US South";
						default: throw new NotImplementedException();
					}
				};
                case UnitType.Cavalry: {
					switch (unitInfo.Nation) {
						case Nation.AustriaHungary: return "Cavalry Austro-Hungaria";
						case Nation.Belgium: return "Cavalry Belgia";
						case Nation.Bulgaria: return "Cavalry Bulgaria";
						case Nation.China: return "Cavalry China";
						case Nation.France: return "Cavalry France";
						case Nation.Germany: return "Cavalry Germany";
						case Nation.GreatBritain: return "Cavalry UK";
						case Nation.Greece: return "Cavalry Greece";
						case Nation.Italy: return "Cavalry Italy";
						case Nation.Japan: return "Cavalry Japan";
						case Nation.Korea: return "Cavalry Korea";
						case Nation.Mexico: return "Cavalry Mexico";
						case Nation.Mongolia: return "Cavalry Mongolia";
						case Nation.Montenegro: return "Cavalry Montenegro";
						case Nation.Romania: return "Cavalry Romania";
						case Nation.Russia: return "Cavalry Russia";
						case Nation.Serbia: return "Cavalry Serbia";
						case Nation.Turkey: return "Cavalry Turkey";
						case Nation.USA: return "Cavalry US";
						case Nation.USNorth: return "Cavalry US North";
						case Nation.USSouth: return "Cavalry US South";
						default: throw new NotImplementedException();
					}
				}
                case UnitType.MachineGunnersCart: return "Machine gunners cart";
                case UnitType.MachineGuns: return "Machine guns";
                case UnitType.Tank: return "Tank";
                case UnitType.Destroyer: return "Destroyer";
                case UnitType.Cruser: return "Cruser";
                case UnitType.Battleship: return "Battleship";
                case UnitType.Carrier: return "Carrier";
				default: throw new NotImplementedException();
            };
		}
	}
}