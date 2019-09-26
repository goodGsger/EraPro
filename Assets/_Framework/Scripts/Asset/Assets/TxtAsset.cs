using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public class TxtAsset : AbstractAsset
    {
        protected string _text;

        public override object asset
        {
            get
            {
                return _asset;
            }

            set
            {
                _asset = value;
                _text = _asset as string;
            }
        }

        public string text
        {
            get { return _text; }
        }

        protected override byte[] CreateBytes()
        {
            if (_text != null)
                return Encoding.Default.GetBytes(_text);

            return null;
        }

        override public void Dispose()
        {
            _text = null;
            base.Dispose();
        }
    }
}
