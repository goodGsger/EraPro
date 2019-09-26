using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Framework
{
    public class LangManager : Manager, ILangManager
    {
        public static int MaxArgs = 10;

        private Dictionary<int, string> _langDict;
        private Dictionary<int, Regex> _regDict;

        protected override void Init()
        {
            _langDict = new Dictionary<int, string>();
            _regDict = new Dictionary<int, Regex>();

            for (int i = 0; i < MaxArgs; i++)
                _regDict[i] = new Regex("\\{" + i + "\\}");
        }

        /// <summary>
        /// 设置语言
        /// </summary>
        /// <param name="key"></param>
        /// <param name="text"></param>
        public void SetLang(int key, string text)
        {
            _langDict[key] = text;
        }

        /// <summary>
        /// 获取翻译
        /// </summary>
        /// <param name="key"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public string GetLang(int key, params string[] param)
        {
            if (_langDict.ContainsKey(key) == false)
                return "LANG_CANT_FIND_" + key;

            string text = _langDict[key];
            for (int i = 0; i < param.Length; i++)
            {
                if (i >= MaxArgs)
                    break;

                text = _regDict[i].Replace(text, param[i]);
            }

            return text;
        }

        /// <summary>
        /// 移除翻译
        /// </summary>
        /// <param name="key"></param>
        public void RemoveLang(int key)
        {
            if (_regDict.ContainsKey(key))
                _regDict.Remove(key);
        }
    }
}
