using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Framework
{
    public class TextureExt : IPooledObject
    {
        private const string TEXTURE_SUFFIX = "_tex_rgb";
        private const string ALPHA_TEXTURE_SUFFIX = "_tex_a";
        private const string PARAM_SUFFIX = "_txt";

        public static Dictionary<string, TextureExtParams> paramsCache = new Dictionary<string, TextureExtParams>();

        public string fileName;
        public Texture2D texture;
        public Texture2D alphaTexture;
        public TextureExtParam[] paramList;
        public Dictionary<string, TextureExtParam> paramDict;

        public void Init(string fileName, AssetBundle assetBundle)
        {
            this.fileName = fileName;
            texture = assetBundle.LoadAsset<Texture2D>(fileName + TEXTURE_SUFFIX);
            alphaTexture = assetBundle.LoadAsset<Texture2D>(fileName + ALPHA_TEXTURE_SUFFIX);

            TextureExtParams extParams;
            if (paramsCache.TryGetValue(fileName, out extParams))
            {
                paramList = extParams.paramList;
                paramDict = extParams.paramDict;
            }
            else
            {
                string paramText = assetBundle.LoadAsset<TextAsset>(fileName + PARAM_SUFFIX).text;
                string[] lines = paramText.Split(StringUtil.WARP, StringSplitOptions.None);
                paramList = new TextureExtParam[lines.Length - 1];
                paramDict = new Dictionary<string, TextureExtParam>();
                for (int i = 0; i < lines.Length - 1; i++)
                {
                    string line = lines[i];
                    string[] paramArray = line.Split(';');
                    TextureExtParam param = new TextureExtParam();
                    param.index = i;
                    param.name = paramArray[0];
                    param.rect = new Rect(float.Parse(paramArray[1]), float.Parse(paramArray[2]), float.Parse(paramArray[3]), float.Parse(paramArray[4]));
                    param.uv = new Vector2(float.Parse(paramArray[5]), float.Parse(paramArray[6]));
                    paramList[i] = param;
                    paramDict[param.name] = param;
                }

                extParams = new TextureExtParams();
                extParams.paramList = paramList;
                extParams.paramDict = paramDict;
                paramsCache[fileName] = extParams;
            }
        }

        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public TextureExtParam GetParam(int index)
        {
            return paramList[index];
        }

        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public TextureExtParam GetParam(string name)
        {
            if (paramDict.ContainsKey(name))
                return paramDict[name];
            return null;
        }

        /// <summary>
        /// 是否包含参数
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool HasParam(int index)
        {
            return paramList.Length > index;
        }

        /// <summary>
        /// 是否包含参数
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool HasParam(string name)
        {
            return paramDict.ContainsKey(name);
        }

        /// <summary>
        /// 重置
        /// </summary>
        public void Reset()
        {
            fileName = null;
            if (texture != null)
            {
                UnityEngine.Object.DestroyImmediate(texture, true);
                texture = null;
            }
            if (alphaTexture != null)
            {
                UnityEngine.Object.DestroyImmediate(alphaTexture, true);
                alphaTexture = null;
            }
            paramList = null;
            paramDict = null;
        }

        /// <summary>
        /// 销毁
        /// </summary>
        public void Dispose()
        {

            Reset();
        }

        /// <summary>
        /// 从对象池中获取
        /// </summary>
        public virtual void OnPoolGet()
        {

        }

        /// <summary>
        /// 重置对象池对象
        /// </summary>
        public virtual void OnPoolReset()
        {
            Reset();
        }

        /// <summary>
        /// 销毁对象池对象
        /// </summary>
        public virtual void OnPoolDispose()
        {
            Dispose();
        }
    }
}
