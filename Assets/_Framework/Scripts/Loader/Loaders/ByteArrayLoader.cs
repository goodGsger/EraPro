using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public class ByteArrayLoader : WWWLoader
    {
        protected ByteArray _byteArray;

        public ByteArrayLoader() : base()
        {
            _type = LoadType.BYTEARRAY;
        }

        /// <summary>
        /// 字节数据
        /// </summary>
        public ByteArray byteArray
        {
            get { return _byteArray; }
            set { _byteArray = value; }
        }

        protected override void OnLoadComplete()
        {
            _data = _byteArray = new ByteArray(_www.bytes);
        }

        protected override void onDispose()
        {
            base.onDispose();
            _byteArray = null;
        }

        public override void OnPoolReset()
        {
            base.OnPoolGet();
            _byteArray = null;
        }
    }
}
