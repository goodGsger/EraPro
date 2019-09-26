using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public struct MapTile : IEqualityComparer<MapTile>
    {
        public int x;
        public int y;

        public MapTile(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public bool Equals(MapTile a, MapTile b)
        {
            return a.x == b.x && a.y == b.y;
        }

        public int GetHashCode(MapTile tile)
        {
            return tile.x * 1000 + tile.y;
        }
    }
}
