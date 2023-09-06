using System;
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

        public GridModel(int cellCountX, int cellCountZ) {
            this.cellCountX = cellCountX;
            this.cellCountZ = cellCountZ;

            conditions = new MapConditions();
        }
    }
}

