using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public class XMLAsset : AbstractAsset
    {
        protected XML _xml;

        public override object asset
        {
            get
            {
                return _asset;
            }

            set
            {
                _asset = value;
                _xml = _asset as XML;
            }
        }

        public XML xml
        {
            get { return _xml; }
        }

        protected override byte[] CreateBytes()
        {
            if (_xml != null)
                return Encoding.Default.GetBytes(_xml.ToString());

            return null;
        }

        override public void Dispose()
        {
            _xml = null;
            base.Dispose();
        }
    }
}
