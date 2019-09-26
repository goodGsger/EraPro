using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game
{
    public class WorldManager
    {
        private static WorldManager _inst;

        public WorldManager()
        {
            
        }

        public static WorldManager inst
        {
            get
            {
                if (_inst == null)
                    _inst = new WorldManager();

                return _inst;
            }
        }
    }
}
