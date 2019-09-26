using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public class XMLLoader : WWWLoader
    {
        protected XML _xml;

        public XMLLoader() : base()
        {
            _type = LoadType.XML;
        }

        /// <summary>
        /// 文本
        /// </summary>
        public XML xml
        {
            get { return _xml; }
            set { _xml = value; }
        }

        protected override void OnLoadComplete()
        {
            _data = _xml = new XML(_www.text);
        }

        protected override void onDispose()
        {
            base.onDispose();
            _xml = null;
        }

        public override void OnPoolReset()
        {
            base.OnPoolGet();
            _xml = null;
        }
    }
}
