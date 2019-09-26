using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public class LoadItem
    {
        public string id;
        public string url;
        public LoadType type;
        public LoadPriority priority;
        public bool save;
        public bool cache;
        public bool loadImmediately;

        public IAsset asset;
        public ILoader loader;
        public float progress;
        public string error;
        public bool isLoading;

        public LoadCallback completeCallback;
        public LoadCallback progressCallback;
        public LoadCallback errorCallback;

        public LoadItem(string url, string id = null, LoadType type = LoadType.AUTO, LoadPriority priority = LoadPriority.LV_2, bool save = true, bool cache = true)
        {
            if (id == null)
                this.id = url;
            else
                this.id = id;

            this.url = url;
            this.type = type;
            this.priority = priority;
            this.save = save;
            this.cache = cache;
        }

        public bool isAssetBundle
        {
            get
            {
                return type == LoadType.ASSETBUNDLE || type == LoadType.TEXTURE_ASSET_BUNDLE || type == LoadType.AUDIO_ASSET_BUNDLE ||
                  type == LoadType.ACTION || type == LoadType.TEXTURE_EXT || type == LoadType.TEXTURE_EXT_SPRITE;
            }
        }
    }
}
