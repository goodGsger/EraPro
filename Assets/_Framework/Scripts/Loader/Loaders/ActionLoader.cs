using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public class ActionLoader : AssetBundleLoader
    {
        protected ActionData _actionData;

        public ActionLoader() : base()
        {
            _type = LoadType.ACTION;
        }

        /// <summary>
        /// 动作数据
        /// </summary>
        public ActionData actionData
        {
            get { return _actionData; }
            set { _actionData = value; }
        }

        protected override void OnLoadComplete()
        {
            string fileName = UrlUtil.GetFileName(_urlRelative);

            _actionData = App.objectPoolManager.GetObject<ActionData>();
            _actionData.Init(fileName, _assetBundle);
            _data = _actionData;

            //_assetBundle.Unload(false);
        }

        protected override void onDispose()
        {
            base.onDispose();
            _actionData = null;
        }

        public override void OnPoolReset()
        {
            base.OnPoolGet();
            _actionData = null;
        }
    }
}
