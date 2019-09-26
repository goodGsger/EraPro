using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public class JSONLoader : WWWLoader
    {
        public JSONLoader() : base()
        {
            _type = LoadType.JSON;
        }
    }
}
