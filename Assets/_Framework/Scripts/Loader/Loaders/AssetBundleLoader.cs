using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Framework
{
    public class AssetBundleLoader : AbstractLoader
    {
        protected AssetBundle _assetBundle;

        public AssetBundleLoader() : base()
        {
            _type = LoadType.ASSETBUNDLE;
        }

        protected override void InvokeSaveFile(byte[] bytes)
        {
            App.fileManager.WriteFilePersistentAsync(_urlRelative, bytes, true);
        }

        protected override IEnumerator InvokeLoadComplete(byte[] bytes)
        {
            AssetBundleCreateRequest abcr;
            if (bytes != null)
                abcr = AssetBundle.LoadFromMemoryAsync(bytes);
            else
                abcr = AssetBundle.LoadFromFileAsync(new StringBuilder(App.pathManager.persistentDataPathFile).Append(_urlRelative).ToString());

            while (abcr.isDone == false)
                yield return null;
            _assetBundle = abcr.assetBundle;

            if (_assetBundle == null)
            {
                _error = "loadContentIsNull";
                if (_errorCallback != null)
                    _errorCallback.Invoke(this);

                yield break;
            }

            UpdateProgress(1f);
            _stats.Done();

            OnLoadComplete();

            if (_data != null)
                _asset = AssetFactory.CreateAsset(_type, _data, _assetBundle);

            if (_completeCallback != null)
                _completeCallback.Invoke(this);
        }

        protected virtual void OnLoadComplete()
        {
            _data = _assetBundle;
        }

        public AssetBundle assetBundle
        {
            get { return _assetBundle; }
        }

        protected override void onDispose()
        {
            base.onDispose();
            _assetBundle = null;
        }

        public override void OnPoolReset()
        {
            base.OnPoolReset();
            _assetBundle = null;
        }
    }
}
