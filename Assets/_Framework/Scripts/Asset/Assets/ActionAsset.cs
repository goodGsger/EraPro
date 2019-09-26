using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Framework
{
    public class ActionAsset : AbstractAsset
    {
        protected ActionData _actionData;

        public override object asset
        {
            get
            {
                return _asset;
            }

            set
            {
                _asset = value;
                _actionData = _asset as ActionData;
            }
        }

        public override void OnAdd()
        {
            _lastUseTime = Time.time;
        }

        public ActionData actionData
        {
            get { return _actionData; }
        }

        protected override byte[] CreateBytes()
        {
            return null;
        }

        override public void Dispose()
        {
            if (_actionData != null)
            {
                App.objectPoolManager.ReleaseObject(_actionData);
                _actionData = null;
            }
            base.Dispose();
        }
    }
}
