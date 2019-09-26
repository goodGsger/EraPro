using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Framework
{
    public class AssetFactory
    {
        /// <summary>
        /// 创建Asset
        /// </summary>
        /// <param name="type"></param>
        /// <param name="data"></param>
        /// <param name="assetBundle"></param>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static IAsset CreateAsset(LoadType type, object data, AssetBundle assetBundle = null, byte[] bytes = null)
        {
            IAsset asset;

            switch (type)
            {
                case LoadType.BINARY:
                    asset = new BinaryAsset();
                    break;
                case LoadType.BYTEARRAY:
                    asset = new ByteArrayAsset();
                    break;
                case LoadType.TEXT:
                    asset = new TxtAsset();
                    break;
                case LoadType.JSON:
                    asset = new JSONAsset();
                    break;
                case LoadType.XML:
                    asset = new XMLAsset();
                    break;
                case LoadType.ASSETBUNDLE:
                    asset = new AssetBundleAsset();
                    break;
                case LoadType.TEXTURE:
                case LoadType.TEXTURE_ASSET_BUNDLE:
                    asset = new TextureAsset();
                    break;
                case LoadType.AUDIO:
                case LoadType.AUDIO_ASSET_BUNDLE:
                    asset = new AudioAsset();
                    break;
                case LoadType.ACTION:
                    asset = new ActionAsset();
                    break;
                case LoadType.TEXTURE_EXT:
                    asset = new TextureExtAsset();
                    break;
                case LoadType.TEXTURE_EXT_SPRITE:
                    asset = new TextureExtSpriteAsset();
                    break;
                default:
                    asset = new TxtAsset();
                    break;
            }

            // 将加载器中的资源填充到资源中
            asset.asset = data;
            asset.assetBundle = assetBundle;
            //asset.bytes = bytes;

            return asset;
        }
    }
}
