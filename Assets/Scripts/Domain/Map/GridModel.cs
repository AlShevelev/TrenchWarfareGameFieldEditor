using System;
using System.Runtime.InteropServices;
using TrenchWarfare.Domain.Enums;
using TrenchWarfare.Domain.Map.Conditions;

namespace TrenchWarfare.Domain.Map {
    public class GridModel {

        MapConditions _conditions;
        public MapConditions Conditions { get => _conditions; }

        // map size (width & height)
        private int _cellCountX, _cellCountZ;
        public int CellCountX { get => _cellCountX; }
        public int CellCountZ { get => _cellCountZ; }

        public GridModel(int cellCountX, int cellCountZ) {
            _cellCountX = cellCountX;
            _cellCountZ = cellCountZ;

            _conditions = new MapConditions();
        }
    }
}

