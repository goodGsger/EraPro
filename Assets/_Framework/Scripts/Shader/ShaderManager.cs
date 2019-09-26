using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Framework
{
    public class ShaderManager
    {
        private static ShaderManager _inst;

        private Dictionary<string, Shader> _shaderDict;

        public ShaderManager()
        {
            _shaderDict = new Dictionary<string, Shader>();
        }

        public static ShaderManager inst
        {
            get
            {
                if (_inst == null)
                    _inst = new ShaderManager();

                return _inst;
            }
        }

        /// <summary>
        /// 获取Shader
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Shader GetShader(string name)
        {
            Shader shader;
            if (_shaderDict.TryGetValue(name, out shader))
                return shader;

            shader = Shader.Find(name);
            if (shader == null)
            {
                App.logManager.Error("ShaderManager GetShader Error:Shader \"" + name + "\" is not found");
                return null;
            }

            shader.hideFlags = HideFlags.None;
            return shader;
        }

        /// <summary>
        /// 从AssetBundle中加载Shader
        /// </summary>
        /// <param name="assetBundle"></param>
        public void LoadShaderFromAssetBundle(AssetBundle assetBundle)
        {
            UnityEngine.Object[] shaders = assetBundle.LoadAllAssets(typeof(Shader));
            for (int i = 0; i < shaders.Length; i++)
            {
                Shader shader = shaders[i] as Shader;
                _shaderDict[shader.name] = shader;
            }
        }

        /// <summary>
        /// 应用颜色滤镜
        /// </summary>
        /// <param name="material"></param>
        /// <param name="colorFilter"></param>
        public void ApplyColorFilter(Material material, ColorFilter colorFilter)
        {
            if (colorFilter != null)
            {
                //material.EnableKeyword(ShaderKeywords.COLOR_FILTER);
                colorFilter.Apply(material);
            }
            else
            {
                //material.DisableKeyword(ShaderKeywords.COLOR_FILTER);
                material.SetInt("_ApplyColorFilter", 0);
            }
        }
    }
}
