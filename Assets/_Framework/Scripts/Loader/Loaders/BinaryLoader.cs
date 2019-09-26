using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public class BinaryLoader : WWWLoader
    {
        public BinaryLoader() : base()
        {
            _type = LoadType.BINARY;
        }
    }
}
