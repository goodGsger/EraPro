using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public class ByteArrayAsset : AbstractAsset
    {
        protected ByteArray _byteArray;

        public override object asset
        {
            get
            {
                return _asset;
            }

            set
            {
                _asset = value;
                _byteArray = _asset as ByteArray;
            }
        }

        public ByteArray byteArray
        {
            get { return _byteArray; }
        }

        protected override byte[] CreateBytes()
        {
            if (_byteArray != null)
                return _byteArray.ReadBytes();

            return null;
        }

        override public void Dispose()
        {
            _byteArray = null;
            base.Dispose();
        }
    }
}
