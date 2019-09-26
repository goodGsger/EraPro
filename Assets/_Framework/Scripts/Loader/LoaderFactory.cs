using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public class LoaderFactory
    {
        public const string TEXT = "txt";
        public const string JSON = "json";
        public const string ASSETBUNDLE = "ab";
        public const string UNITY3D = "unity3d";
        public const string XML = "xml";
        public const string JPG = "jpg";
        public const string JPEG = "jpeg";
        public const string PNG = "png";
        public const string BMP = "bmp";
        public const string BINARY = "binary";
        public const string BYTEARRAY = "byteArray";
        public const string ACTION = "action";
        public const string MP3 = "mp3";
        public const string WAV = "wav";

        /// <summary>
        /// 创建下载器
        /// </summary>
        /// <param name="urlRelative"></param>
        /// <param name="urlAbsolute"></param>
        /// <param name="type"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static ILoader CreateLoader(string urlRelative, string urlAbsolute, LoadType type = LoadType.AUTO, params LoadParam[] loadParams)
        {
            if (type == LoadType.AUTO)
                type = GetLoadTypeByUrl(urlRelative);

            ILoader loader = CreateLoader(type, urlRelative, urlAbsolute);

            foreach (LoadParam param in loadParams)
            {
                if (param is LoadParam)
                    loader.AddParam(param);
            }

            return loader;
        }

        /// <summary>
        /// 创建下载器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="urlRelative"></param>
        /// <param name="urlAbsolute"></param>
        /// <param name="type"></param>
        /// <param name="loadParams"></param>
        /// <returns></returns>
        public static T CreateLoader<T>(string urlRelative, string urlAbsolute, LoadType type = LoadType.AUTO, params LoadParam[] loadParams) where T : ILoader
        {
            ILoader loader = CreateLoader(urlRelative, urlAbsolute, type, loadParams);
            if (loader is T)
                return (T)loader;

            return default(T);
        }

        private static ILoader CreateLoader(LoadType type, string urlRelative, string urlAbsolute)
        {
            ILoader loader;
            switch (type)
            {

                case LoadType.BINARY:
                    loader = new BinaryLoader();
                    break;
                case LoadType.BYTEARRAY:
                    loader = new ByteArrayLoader();
                    break;
                case LoadType.TEXT:
                    loader = new TextLoader();
                    break;
                case LoadType.JSON:
                    loader = new JSONLoader();
                    break;
                case LoadType.XML:
                    loader = new XMLLoader();
                    break;
                case LoadType.ASSETBUNDLE:
                    loader = new AssetBundleLoader();
                    break;
                case LoadType.TEXTURE:
                    loader = new TextureLoader();
                    break;
                case LoadType.TEXTURE_ASSET_BUNDLE:
                    loader = new TextureAssetBundleLoader();
                    break;
                case LoadType.AUDIO:
                    loader = new AudioLoader();
                    break;
                case LoadType.AUDIO_ASSET_BUNDLE:
                    loader = new AudioAssetBundleLoader();
                    break;
                case LoadType.ACTION:
                    loader = new ActionLoader();
                    break;
                case LoadType.TEXTURE_EXT:
                    loader = new TextureExtLoader();
                    break;
                case LoadType.TEXTURE_EXT_SPRITE:
                    loader = new TextureExtSpriteLoader();
                    break;
                default:
                    loader = new TextLoader();
                    break;
            }

            loader.urlRelative = urlRelative;
            //loader.urlAbsolute = urlAbsolute;

            return loader;
        }

        private static LoadType GetLoadTypeByUrl(string url)
        {
            int pIndex = url.LastIndexOf(".");
            if (pIndex == -1)
                return LoadType.TEXT;

            int qIndex = url.LastIndexOf("?");
            if (qIndex != -1)
                url = url.Substring(0, qIndex);

            string extension = url.Substring(pIndex + 1).ToLower();

            switch (extension)
            {
                case ACTION:
                    return LoadType.ACTION;
                case ASSETBUNDLE:
                case UNITY3D:
                    return LoadType.ASSETBUNDLE;
                case JPG:
                case JPEG:
                case PNG:
                case BMP:
                    return LoadType.TEXTURE;
                case TEXT:
                    return LoadType.TEXT;
                case MP3:
                case WAV:
                    return LoadType.AUDIO;
                case JSON:
                    return LoadType.JSON;
                case XML:
                    return LoadType.XML;
                case BINARY:
                    return LoadType.BINARY;
                case BYTEARRAY:
                    return LoadType.BYTEARRAY;
                default:
                    return LoadType.TEXT;
            }
        }
    }
}
