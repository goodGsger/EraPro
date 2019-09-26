using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public class BinaryAsset : AbstractAsset
    {
        protected override byte[] CreateBytes()
        {
            return _asset as byte[];
        }
    }
}
