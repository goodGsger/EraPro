using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Framework
{
    public class TextLoader : WWWLoader
    {
        protected string _text;

        public TextLoader() : base()
        {
            _type = LoadType.TEXT;
        }

        /// <summary>
        /// 文本
        /// </summary>
        public string text
        {
            get { return _text; }
            set { _text = value; }
        }

        protected override void OnLoadComplete()
        {
            _data = _text = _www.text;
        }

        protected override void onDispose()
        {
            base.onDispose();
            _text = null;
        }

        public override void OnPoolReset()
        {
            base.OnPoolGet();
            _text = null;
        }
    }
}
