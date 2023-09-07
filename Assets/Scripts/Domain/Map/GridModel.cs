using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TrenchWarfare.Domain.Enums;
using TrenchWarfare.Domain.Map.Conditions;

namespace TrenchWarfare.Domain.Map {
    public class GridModel {

        MapConditions conditions;
        public MapConditions Conditions { get => conditions; }

        // map size (width & height)
        private int cellCountX, cellCountZ;
        public int CellCountX { get => cellCountX; }
        public int CellCountZ { get => cellCountZ; }

        CellModelRead[] cells;
        public IEnumerable<CellModelRead> Cells { get => cells; }

        public GridModel(int cellCountX, int cellCountZ) {
            this.cellCountX = cellCountX;
            this.cellCountZ = cellCountZ;

            conditions = new MapConditions();
        }

        public void InitCells(int quantity) {
            cells = new CellModelRead[quantity];
        }

        public CellModelRead GetCell(int index) {
            return cells[index];
        }

        public void SetCell(int index, CellModelRead model) {
            cells[index] = model;
        }
    }
}

