using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Framework
{
    public class WWWLoader : AbstractLoader
    {
        protected WWW _www;

        protected override void InvokeSaveFile(byte[] bytes)
        {
            App.fileManager.WriteFilePersistent(_urlRelative, bytes, true);
        }

        protected override IEnumerator InvokeLoadComplete(byte[] bytes)
        {
            _www = new WWW(new StringBuilder(App.pathManager.persistentDataPathWWW).Append(_urlRelative).ToString());

            while (_www.isDone == false)
            {
                yield return null;
            }

            yield return _www;

            if (_www == null)
            {
                _error = "loadContentIsNull";
                if (_errorCallback != null)
                    _errorCallback.Invoke(this);

                yield break;
            }

            if (_www.error != null)
            {
                if (Retry())
                    yield break;
                _error = _www.error;
                if (_errorCallback != null)
                    _errorCallback.Invoke(this);
            }
            else
            {
                UpdateProgress(1f);
                _stats.Done();

                OnLoadComplete();

                if (_data != null)
                    _asset = AssetFactory.CreateAsset(_type, _data);

                if (_completeCallback != null)
                    _completeCallback.Invoke(this);
            }

            if (_www != null)
            {
                //_www.Dispose();
                _www = null;
            }
        }

        protected virtual void OnLoadComplete()
        {

        }

        public override void Stop()
        {
            if (_www != null)
            {
                //_www.Dispose();
                _www = null;
            }
            base.Stop();
        }
    }
}
