using System.Collections.Generic;
using TrenchWarfare.Domain.Map.Conditions;

namespace TrenchWarfare.Domain.Map
{
    public class GridModel: Model {

        MapConditions conditions;
        public MapConditions Conditions { get => conditions; }

        // map size (width & height)
        private int cellCountX, cellCountZ;
        public int CellCountX { get => cellCountX; }
        public int CellCountZ { get => cellCountZ; }

        CellModelExternal[] cells;
        public IEnumerable<CellModelExternal> Cells { get => cells; }

        public GridModel(int cellCountX, int cellCountZ): base() {
            this.cellCountX = cellCountX;
            this.cellCountZ = cellCountZ;

            conditions = new MapConditions();
        }

        public void InitCells(int quantity) {
            cells = new CellModelExternal[quantity];
        }

        public CellModelExternal GetCell(int index) {
            return cells[index];
        }

        public void SetCell(int index, CellModelExternal model) {
            cells[index] = model;
        }
    }
}

